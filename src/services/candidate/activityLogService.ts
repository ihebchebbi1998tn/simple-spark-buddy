import { apiClient } from '../common/api';
import { authService } from './authService';

// ============================================================================
// ACTION TYPES (matches backend validator: 1-53)
// ============================================================================
export const ACTION_TYPES = {
  REGISTRATION_STARTED: 1,
  REGISTRATION_COMPLETED: 2,
  EMAIL_VERIFIED: 3,
  PHONE_VERIFIED: 4,
  LOGIN_SUCCESS: 5,
  LOGIN_FAILED: 6,
  LOGOUT: 7,
  PASSWORD_RESET_REQUESTED: 8,
  PASSWORD_RESET_COMPLETED: 9,
  PROFILE_VIEWED: 12,
  PROFILE_UPDATED: 13,
  PROFILE_SECTION_UPDATED: 14,
  SKILL_ADDED: 17,
  LANGUAGE_ADDED: 19,
  TEST_STARTED: 20,
  TEST_COMPLETED: 21,
  TEST_PASSED: 22,
  TEST_FAILED: 23,
  SETTINGS_VIEWED: 24,
  NOTIFICATION_PREFERENCES_UPDATED: 25,
  PRIVACY_SETTINGS_UPDATED: 26,
  PAGE_VIEW: 41,
  DASHBOARD_VIEWED: 42,
  HELP_ACCESSED: 43,
  CONTACT_SUPPORT: 44,
  AI_BOT_CONVERSATION: 45,
} as const;

// ============================================================================
// DEVICE TYPE DETECTION
// ============================================================================
function getDeviceType(): number {
  const ua = navigator.userAgent;
  if (/tablet|ipad/i.test(ua)) return 3; // tablet
  if (/mobile|android|iphone/i.test(ua)) return 2; // mobile
  return 1; // desktop
}

// ============================================================================
// SESSION ID (persisted per browser session)
// ============================================================================
let _sessionId: string | null = null;
function getSessionId(): string {
  if (!_sessionId) {
    _sessionId = sessionStorage.getItem('activity_session_id');
    if (!_sessionId) {
      _sessionId = `sess_${Date.now()}_${Math.random().toString(36).slice(2, 12)}`;
      sessionStorage.setItem('activity_session_id', _sessionId);
    }
  }
  return _sessionId;
}

// ============================================================================
// SERVICE
// ============================================================================
export const activityLogService = {
  /**
   * Log a user activity to the V2 backend.
   * Fires and forgets — errors are silently caught to avoid disrupting UX.
   */
  async log(actionType: number, metadata?: Record<string, any>) {
    try {
      const userData = authService.getUserData();
      const leadId = userData?.lead_id || userData?.id;

      const payload: Record<string, any> = {
        action_type: actionType,
        is_authenticated: !!leadId,
        session_id: getSessionId(),
        ip_address: '0.0.0.0', // backend should override with real IP from req
        user_agent: navigator.userAgent,
        device_type: getDeviceType(),
        source: 'web' as const,
        page_url: window.location.pathname,
      };

      if (leadId) payload.lead_id = leadId;
      if (metadata) payload.metadata = metadata;

      // Use the lead-scoped route if authenticated, otherwise global
      const url = leadId
        ? `/api/v2/leads/${leadId}/activity-logs`
        : '/api/v2/activity-logs';

      await apiClient.post(url, payload, !!leadId);
    } catch (err) {
      // Silent fail — activity logging should never break the app
      console.warn('⚠️ Activity log failed (non-critical):', err);
    }
  },

  /**
   * Get activity logs for the current lead
   */
  async getMyLogs(page = 1, limit = 20) {
    const userData = authService.getUserData();
    const leadId = userData?.lead_id || userData?.id;
    if (!leadId) return { data: [], pagination: {} };

    const response = await apiClient.get(
      `/api/v2/leads/${leadId}/activity-logs?page=${page}&limit=${limit}`,
      true
    );
    return response;
  },
};
