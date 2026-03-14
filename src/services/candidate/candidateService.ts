import { apiClient } from '../common/api';
import { authService } from './authService';
import {
  mapFromBackendLead,
  mapFromBackendProfile,
  mapFromBackendAvailability,
  mapToBackendProfile,
  mapToBackendLanguages,
  mapToBackendLanguageTest,
  GENDER_MAP,
  CITY_MAP,
} from '../common/fieldMapping';

// ============================================================================
// TYPES (kept compatible with existing dashboard components)
// ============================================================================

export interface CandidateProfile {
  candidate: {
    _id: string;
    email: string;
    phone: string;
    name: string;
    surname: string;
    gender: string;
    city: string;
    registration_date: string;
    birthday?: string;
    postal_code?: string;
  };
  profile: {
    desired_position: string;
    call_center_experience: string;
    position_experience?: string;
    primary_activity?: string;
    primary_activity_experience?: string;
    operation_1?: string;
    operation_1_experience?: string;
    operation_2?: string;
    operation_2_experience?: string;
    native_language: string;
    foreign_language_1: string;
    foreign_language_1_level: string;
    foreign_language_2?: string;
    foreign_language_2_level?: string;
    is_bilingual: boolean;
  };
  availability: {
    availability?: string;
    work_mode: string;
    work_time: string;
    work_park: string;
    nearby_cities?: string[];
    blacklist?: string[];
    whitelist?: string[];
    international_offers?: boolean;
  };
  contractPreferences?: any;
  testScores?: {
    [key: string]: boolean | number | undefined;
  };
  // Raw V2 data for components that need it
  _raw?: any;
}

// ============================================================================
// HELPER: Get lead ID from stored user data
// ============================================================================

function getLeadId(): string {
  const userData = authService.getUserData();
  const leadId = userData?.lead_id || userData?.id;
  if (!leadId) {
    throw new Error('No lead ID found. User may not be authenticated.');
  }
  return leadId;
}

// ============================================================================
// SERVICE
// ============================================================================

export const candidateService = {
  /**
   * Check if email or phone already exists
   */
  async checkAvailability(data: { email?: string; phone?: string }): Promise<{
    success: boolean;
    data: { emailExists: boolean; phoneExists: boolean };
  }> {
    // V2: Check email via dedicated endpoint
    const result = { emailExists: false, phoneExists: false };

    if (data.email) {
      try {
        const response = await apiClient.get(`/api/v2/leads/email/${encodeURIComponent(data.email)}`);
        result.emailExists = response.success && !!response.data;
      } catch {
        result.emailExists = false;
      }
    }

    // Phone check not directly supported in V2 - return false
    return { success: true, data: result };
  },

  /**
   * Get complete candidate profile with all related data
   * Fetches from V2 and maps to old CandidateProfile interface for compatibility
   */
  async getProfile(): Promise<{ success: boolean; data: CandidateProfile }> {
    const leadId = getLeadId();

    // Fetch lead with profile, scoring, and settings
    const leadResponse = await apiClient.get(
      `/api/v2/leads/${leadId}?includeProfile=true&includeScoring=true&includeSettings=true`,
      true
    );

    if (!leadResponse.success || !leadResponse.data) {
      throw new Error('Failed to fetch profile');
    }

    const lead = leadResponse.data;

    // Fetch skills/tests
    let skills: any[] = [];
    try {
      const skillsResponse = await apiClient.get(`/api/v2/leads/${leadId}/skills`, true);
      skills = skillsResponse.data || [];
    } catch {
      console.warn('⚠️ Could not fetch skills');
    }

    // Map to old CandidateProfile format for backward compatibility
    const candidate = mapFromBackendLead(lead);
    const profile = mapFromBackendProfile(lead.profile);
    const availability = mapFromBackendAvailability(lead.profile);

    // Map language data from lead summary_metrics
    if (lead.summary_metrics) {
      const sm = lead.summary_metrics;
      profile.native_language = sm.primary_language_code || '';
      profile.foreign_language_1 = sm.secondary_language_code || '';
      profile.foreign_language_1_level = sm.secondary_language_level?.toString() || '';
      profile.foreign_language_2 = sm.tertiary_language_code || '';
      profile.foreign_language_2_level = sm.tertiary_language_level?.toString() || '';
    }

    // Map test scores from skills (qualification_type=1 = language tests)
    const testScores: Record<string, boolean | number | undefined> = {};
    const languageSkills = skills.filter((s: any) => s.qualification_type === 1 && s.completed);

    const codeToLang: Record<string, string> = {
      'FR': 'french',
      'EN': 'english',
      'IT': 'italian',
      'ES': 'spanish',
      'DE': 'german',
    };

    for (const skill of languageSkills) {
      const langKey = codeToLang[skill.language_code] || skill.language_code?.toLowerCase();
      if (langKey) {
        testScores[`${langKey}_test_completed`] = true;
        testScores[`${langKey}_overall_score`] = skill.scores?.global || 0;
      }
    }

    return {
      success: true,
      data: {
        candidate,
        profile,
        availability,
        testScores,
        _raw: lead,
      },
    };
  },

  /**
   * Update main candidate information
   */
  async updateCandidateInfo(data: {
    name?: string;
    surname?: string;
    phone?: string;
    city?: string;
    gender?: string;
    birthday?: string;
    postal_code?: string;
    facebook?: string;
    instagram?: string;
    linkedin?: string;
  }) {
    const leadId = getLeadId();

    // Map to V2 format (using shared maps from fieldMapping)

    const updateData: any = {};
    if (data.surname) updateData.first_name = data.surname;
    if (data.name) updateData.last_name = data.name;
    if (data.phone) updateData.phone = data.phone;
    if (data.gender) updateData.gender = GENDER_MAP[data.gender.toLowerCase()] ?? undefined;

    // Location must be sent as nested object for backend allowedFields
    if (data.city) {
      const cityId = CITY_MAP[data.city.toLowerCase()];
      if (cityId) {
        updateData.location = { city_id: cityId };
      }
    }
    if (data.postal_code) {
      updateData.location = { ...(updateData.location || {}), postal_code: data.postal_code };
    }

    return apiClient.put(`/api/v2/leads/${leadId}/profile`, updateData, true);
  },

  /**
   * Update candidate profile (CV/Profile)
   * Accepts BOTH frontend field names (activitePrincipale) AND backend field names (primary_activity)
   */
  async updateProfile(data: any) {
    const leadId = getLeadId();
    const promises: Promise<any>[] = [];

    // --- Handle language data (separate from activities) ---
    // If data contains language fields, these don't go through mapToBackendProfile
    const hasLanguageData = data.native_language || data.foreign_language_1 || data.is_bilingual !== undefined;
    if (hasLanguageData) {
      const languagePayload = mapToBackendLanguages({
        langueMaternelle: data.native_language || data.langueMaternelle,
        langueForeign1: data.foreign_language_1 || data.langueForeign1,
        niveauLangueForeign1: data.foreign_language_1_level || data.niveauLangueForeign1,
        hasSecondForeignLanguage: (data.foreign_language_2 || data.langueForeign2) ? 'oui' : 'non',
        langueForeign2: data.foreign_language_2 || data.langueForeign2,
        niveauLangueForeign2: data.foreign_language_2_level || data.niveauLangueForeign2,
      });
      // Language data goes to the generic profile endpoint as summary_metrics
      promises.push(
        apiClient.put(`/api/v2/leads/${leadId}/profile`, { summary_metrics: languagePayload }, true)
      );
      console.log('📤 Language data sent to profile/summary_metrics:', languagePayload);
    }

    // --- Handle activity/work preference data ---
    // Normalize: accept both backend field names (from ProfileSection) and frontend field names (from registration)
    const normalizedData = {
      activitePrincipale: data.activitePrincipale || data.primary_activity || '',
      experienceActivitePrincipale: data.experienceActivitePrincipale || data.primary_activity_experience || data.call_center_experience || '',
      operation1: data.operation1 || data.operation_1 || '',
      experienceOperation1: data.experienceOperation1 || data.operation_1_experience || '',
      operation2: data.operation2 || data.operation_2 || '',
      experienceOperation2: data.experienceOperation2 || data.operation_2_experience || '',
      posteRecherche: data.posteRecherche || data.desired_position || '',
      modeTravail: data.modeTravail || data.work_mode || '',
      tempsTravail: data.tempsTravail || data.work_time || '',
      parcTravail: data.parcTravail || data.work_park || '',
    };

    const profilePayload = mapToBackendProfile(normalizedData);

    if (profilePayload.activities && profilePayload.activities.length > 0) {
      promises.push(
        apiClient.put(`/api/v2/leads/${leadId}/profile/activities`, { activities: profilePayload.activities }, true)
      );
    }

    const wp = profilePayload.work_preferences;
    if (wp && (wp.mode || wp.time || wp.shift)) {
      promises.push(
        apiClient.put(`/api/v2/leads/${leadId}/profile/work-preferences`, { work_preferences: wp }, true)
      );
    }

    if (promises.length > 0) {
      await Promise.all(promises);
    }

    return { success: true, message: 'Profile updated' };
  },

  /**
   * Update candidate availability / work preferences
   * Handles: plain availability field, single values, AND comma-joined values from ContractPreferences
   */
  async updateAvailability(data: {
    availability?: string;
    work_mode?: string;
    work_time?: string;
    work_park?: string;
  }) {
    const leadId = getLeadId();

    const WORK_MODE_MAP: Record<string, number> = { 'teletravail': 1, 'presentiel': 2, 'hybride': 3 };
    const WORK_TIME_MAP: Record<string, number> = { 'temps_plein': 1, 'temps_partiel': 2, 'freelance': 3, 'stage': 4, 'alternance': 5 };
    const WORK_SHIFT_MAP: Record<string, number> = { 'jour': 1, 'apres_midi': 2, 'nuit': 3 };

    // Helper: extract first valid value from potentially comma-joined string
    const mapFirst = (value: string | undefined, map: Record<string, number>): number | undefined => {
      if (!value) return undefined;
      // Try direct lookup first
      if (map[value.trim()]) return map[value.trim()];
      // Try first value from comma-separated list
      const first = value.split(',').map(v => v.trim()).find(v => map[v]);
      if (first) return map[first];
      // Try parsing as number
      const num = parseInt(value);
      return isNaN(num) ? undefined : num;
    };

    const promises: Promise<any>[] = [];

    // Send work preferences if any mode/time/shift provided
    const work_preferences: any = {};
    const mode = mapFirst(data.work_mode, WORK_MODE_MAP);
    const time = mapFirst(data.work_time, WORK_TIME_MAP);
    const shift = mapFirst(data.work_park, WORK_SHIFT_MAP);
    if (mode) work_preferences.mode = mode;
    if (time) work_preferences.time = time;
    if (shift) work_preferences.shift = shift;

    if (Object.keys(work_preferences).length > 0) {
      promises.push(
        apiClient.put(`/api/v2/leads/${leadId}/profile/work-preferences`, { work_preferences }, true)
      );
    }

    // Send availability field (immédiate, etc.) to generic profile endpoint
    if (data.availability) {
      promises.push(
        apiClient.put(`/api/v2/leads/${leadId}/profile`, { availability: { status: data.availability } }, true)
      );
    }

    if (promises.length > 0) {
      await Promise.all(promises);
    }

    return { success: true, message: 'Availability updated' };
  },

  /**
   * Update contract preferences
   */
  async updateContractPreferences(data: any) {
    const leadId = getLeadId();
    return apiClient.put(`/api/v2/leads/${leadId}/profile`, data, true);
  },

  /**
   * Add/Update language test scores
   */
  async addTestScore(language: string, scores: {
    linguistic: number;
    softSkills: number;
    jobSkills: number;
    overall: number;
  }) {
    const leadId = getLeadId();
    const payload = mapToBackendLanguageTest(language, {
      linguistic: scores.linguistic,
      softSkills: scores.softSkills,
      jobSkills: scores.jobSkills,
      overall: scores.overall,
    });

    return apiClient.post(`/api/v2/leads/${leadId}/skills/language`, payload, true);
  },

  /**
   * Update call center blacklist/whitelist
   */
  async updateCallCenters(data: {
    blacklist?: string[];
    whitelist?: string[];
    blacklist_reasons?: Record<string, string>;
  }) {
    const leadId = getLeadId();

    const company_preferences: any = {};
    if (data.blacklist) {
      company_preferences.blacklisted_companies = data.blacklist.map(id => ({
        company_id: id,
        reason: 1,
      }));
    }
    if (data.whitelist) {
      company_preferences.whitelisted_companies = data.whitelist;
    }

    return apiClient.put(`/api/v2/leads/${leadId}/profile`, { company_preferences }, true);
  },

  /**
   * Update notification and privacy settings
   */
  async updateNotificationSettings(data: {
    email_notifications?: boolean;
    sms_notifications?: boolean;
    profile_visibility?: string;
    secure_data_sharing?: boolean;
  }) {
    const leadId = getLeadId();

    // Backend expects nested objects, not dot-notation
    const settingsUpdate: any = {};

    // Notifications must be { email: bool, sms: bool }
    const notifications: any = {};
    if (data.email_notifications !== undefined) notifications.email = data.email_notifications;
    if (data.sms_notifications !== undefined) notifications.sms = data.sms_notifications;
    if (Object.keys(notifications).length > 0) settingsUpdate.notifications = notifications;

    // Privacy must be { profile_visibility: string }
    const privacy: any = {};
    if (data.profile_visibility) privacy.profile_visibility = data.profile_visibility;
    if (Object.keys(privacy).length > 0) settingsUpdate.privacy = privacy;

    return apiClient.put(`/api/v2/leads/${leadId}/settings`, settingsUpdate, true);
  },

  /**
   * Update geographic proximity preferences
   */
  async updateProximityPreferences(data: {
    villeResidence?: string;
    villesProximite?: string[];
    offresEtranger?: boolean;
  }) {
    const leadId = getLeadId();

    // Using shared CITY_MAP from fieldMapping

    const work_preferences: any = {};
    if (data.villesProximite) {
      work_preferences.preferred_cities = data.villesProximite
        .map(city => CITY_MAP[city.toLowerCase()])
        .filter(Boolean);
    }

    const availability: any = {};
    if (data.offresEtranger !== undefined) {
      availability.international_offers = data.offresEtranger;
    }

    const promises: Promise<any>[] = [];

    if (Object.keys(work_preferences).length > 0) {
      promises.push(
        apiClient.put(`/api/v2/leads/${leadId}/profile/work-preferences`, { work_preferences }, true)
      );
    }

    if (Object.keys(availability).length > 0) {
      promises.push(
        apiClient.put(`/api/v2/leads/${leadId}/profile`, { availability }, true)
      );
    }

    if (promises.length > 0) {
      await Promise.all(promises);
    }

    return { success: true };
  },

  /**
   * Change password
   */
  async changePassword(data: {
    currentPassword: string;
    newPassword: string;
  }) {
    const leadId = getLeadId();
    return apiClient.put(`/api/v2/leads/${leadId}/password`, {
      current_password: data.currentPassword,
      new_password: data.newPassword,
      password_confirm: data.newPassword,
    }, true);
  },

  /**
   * Change email address
   */
  async changeEmail(data: {
    newEmail: string;
    password: string;
  }) {
    const leadId = getLeadId();
    return apiClient.put(`/api/v2/leads/${leadId}/email`, {
      new_email: data.newEmail,
      password: data.password,
    }, true);
  },

  /**
   * Delete account
   */
  async deleteAccount(data: {
    password: string;
    confirmation: string;
  }) {
    const leadId = getLeadId();
    return apiClient.delete(`/api/v2/leads/${leadId}`, data, true);
  },

  /**
   * Get all notifications for authenticated lead
   */
  async getAlerts(): Promise<{
    success: boolean;
    data: {
      alerts: Array<{
        _id: string;
        type: 'success' | 'warning' | 'info' | 'error';
        title: string;
        message: string;
        action_type: string;
        read: boolean;
        created_at: string;
      }>;
      unreadCount: number;
    };
  }> {
    const leadId = getLeadId();

    try {
      const response = await apiClient.get(`/api/v2/leads/${leadId}/notifications?limit=50`, true);

      // Map V2 notifications to old alert format
      const notifications = response.data || [];
      const alerts = notifications.map((n: any) => ({
        _id: n._id,
        type: n.priority >= 3 ? 'warning' : 'info',
        title: n.title,
        message: n.message,
        action_type: n.notification_type?.toString() || '',
        read: n.status >= 4, // 4 = read
        created_at: n.created_at,
      }));

      const unreadCount = alerts.filter((a: any) => !a.read).length;

      return { success: true, data: { alerts, unreadCount } };
    } catch {
      return { success: true, data: { alerts: [], unreadCount: 0 } };
    }
  },

  /**
   * Mark specific alert as read
   */
  async markAlertAsRead(alertId: string) {
    const leadId = getLeadId();
    return apiClient.post(`/api/v2/leads/${leadId}/notifications/${alertId}/mark-read`, {}, true);
  },

  /**
   * Mark all alerts as read (mark each individually since V2 has no bulk endpoint)
   */
  async markAllAlertsAsRead() {
    const leadId = getLeadId();
    try {
      // Fetch current unread notifications
      const response = await apiClient.get(`/api/v2/leads/${leadId}/notifications?limit=50`, true);
      const notifications = response.data || [];
      const unread = notifications.filter((n: any) => n.status < 4);

      // Mark each as read in parallel
      await Promise.all(
        unread.map((n: any) =>
          apiClient.post(`/api/v2/leads/${leadId}/notifications/${n._id}/mark-read`, {}, true)
        )
      );
    } catch (err) {
      console.warn('⚠️ markAllAlertsAsRead partial failure:', err);
    }
    return { success: true };
  },

  /**
   * Delete old alerts
   */
  async deleteOldAlerts() {
    return { success: true };
  },
};
