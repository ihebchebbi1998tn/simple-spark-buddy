// Maps frontend form field names to V2 backend API field names
// V2 uses English field names with numeric enums/IDs

// ============================================================================
// MAPPING TABLES: Frontend values → Backend numeric codes
// ============================================================================

// Gender: civilite → gender (0=Male, 1=Female, 2=Other)
export const GENDER_MAP: Record<string, number> = {
  'monsieur': 0,
  'mr': 0,
  'madame': 1,
  'mme': 1,
  'autre': 2,
};

// City: villeResidence → city_id (numeric)
export const CITY_MAP: Record<string, number> = {
  'tunis': 1,
  'ariana': 2,
  'ben_arous': 3,
  'manouba': 4,
  'nabeul': 5,
  'zaghouan': 6,
  'bizerte': 7,
  'beja': 8,
  'jendouba': 9,
  'kef': 10,
  'siliana': 11,
  'kairouan': 12,
  'kasserine': 13,
  'sidi_bouzid': 14,
  'sousse': 15,
  'monastir': 16,
  'mahdia': 17,
  'sfax': 18,
  'gabes': 19,
  'medenine': 20,
  'tataouine': 21,
  'gafsa': 22,
  'tozeur': 23,
  'kebili': 24,
};

// Work mode: modeTravail → work_mode (1=Remote, 2=On-site, 3=Hybrid)
const WORK_MODE_MAP: Record<string, number> = {
  'remote': 1,
  'teletravail': 1,
  'on-site': 2,
  'onsite': 2,
  'presentiel': 2,
  'sur_site': 2,
  'hybrid': 3,
  'hybride': 3,
  'any': 3, // "any" defaults to hybrid
};

// Work time: tempsTravail → work_time (1=Full-time, 2=Part-time, etc.)
const WORK_TIME_MAP: Record<string, number> = {
  'temps_plein': 1,
  'plein_temps': 1,
  'full_time': 1,
  'full': 1,
  'temps_partiel': 2,
  'mi_temps': 2,
  'part_time': 2,
  'part': 2,
  'any': 1, // "any" defaults to full-time
  'freelance': 3,
  'stage': 4,
  'alternance': 5,
};

// Work shift: parcTravail → work_shift (1=Morning, 2=Afternoon, 3=Night)
const WORK_SHIFT_MAP: Record<string, number> = {
  'jour': 1,
  'matin': 1,
  'morning': 1,
  'apres_midi': 2,
  'afternoon': 2,
  'nuit': 3,
  'night': 3,
};

// Activity type: activitePrincipale → activity_type (1-4)
const ACTIVITY_MAP: Record<string, number> = {
  '1': 1, // Télévente
  '2': 2, // Téléprospection
  '3': 3, // Prise de RDV
  '4': 4, // Service client
};

// Language code mapping
const LANGUAGE_CODE_MAP: Record<string, string> = {
  'francais': 'FR',
  'français': 'FR',
  'french': 'FR',
  'arabe': 'AR',
  'arabic': 'AR',
  'english': 'EN',
  'anglais': 'EN',
  'espanol': 'ES',
  'español': 'ES',
  'espagnol': 'ES',
  'spanish': 'ES',
  'italiano': 'IT',
  'italien': 'IT',
  'italian': 'IT',
  'deutsch': 'DE',
  'german': 'DE',
  'allemand': 'DE',
  'portugais': 'PT',
  'turc': 'TR',
  'néerlandais': 'NL',
  'neerlandais': 'NL',
};

// Language level: niveauLangue → CEFR numeric (1-7)
const LANGUAGE_LEVEL_MAP: Record<string, number> = {
  'a1': 1,
  'debutant': 1,
  'a2': 2,
  'elementaire': 2,
  'b1': 3,
  'intermediaire': 3,
  'b2': 4,
  'avance': 4,
  'c1': 5,
  'expert': 5,
  'c2': 6,
  'maitrise': 6,
  'natif': 7,
  'native': 7,
  'bilingue': 7,
  // Numeric fallbacks
  '1': 1,
  '2': 2,
  '3': 3,
  '4': 4,
  '5': 5,
  '6': 6,
  '7': 7,
};

// Experience: string value → numeric (1-7)
// The frontend already uses numeric strings "0"-"7" for experience
const EXPERIENCE_MAP: Record<string, number> = {
  '0': 1, // Aucune expérience → maps to 1 (lowest)
  '1': 1, // 0-6 mois
  '2': 2, // 6-12 mois
  '3': 3, // 1-2 ans
  '4': 4, // 2-3 ans
  '5': 5, // 3-5 ans
  '6': 6, // 5-7 ans
  '7': 7, // Plus de 7 ans
};

// ============================================================================
// HELPER FUNCTIONS
// ============================================================================

const formatPhone = (phone: string): string => {
  const cleaned = phone.replace(/\D/g, '');
  if (cleaned.length === 8) {
    return `+216${cleaned}`;
  }
  return cleaned.startsWith('+') ? cleaned : `+${cleaned}`;
};

const mapValue = <T>(value: string | undefined | null, map: Record<string, T>): T | null => {
  if (!value) return null;
  const normalized = value.toLowerCase().trim();
  return map[normalized] ?? null;
};

const toInt = (value: string | undefined | null): number | null => {
  if (!value) return null;
  const num = parseInt(value, 10);
  return isNaN(num) ? null : num;
};

// ============================================================================
// MAIN MAPPING FUNCTION
// ============================================================================

export interface V2RegistrationPayload {
  // Required fields for leads table
  email: string;
  password: string;
  password_confirm: string;
  first_name: string;
  last_name: string;
  phone: string;
  date_of_birth: string | null;
  gender: number | null;
  country_code: string;
  city_id: number | null;
  postal_code: string | null;
  channel: number;
  language: string;
  device_type: string | null;
  // Source tracking
  source_type: string;
  campaign_id: string | null;
  referral_code: string | null;
  // Terms acceptance
  accept_terms: boolean;
}

export interface V2ProfilePayload {
  desired_position: number | null;
  call_center_experience: number | null;
  position_experience: number | null;
  activities: Array<{
    activity_type: number;
    activity_experience: number;
    operations: Array<{
      operation_type: number;
      operation_experience: number;
    }>;
  }>;
  work_preferences: {
    mode: number | null;
    time: number | null;
    shift: number | null;
  };
}

export interface V2LanguageData {
  primary_language_code: string | null;
  primary_language_level: number | null;
  secondary_language_code: string | null;
  secondary_language_level: number | null;
  tertiary_language_code: string | null;
  tertiary_language_level: number | null;
}

/**
 * Map frontend registration form data to V2 backend registration payload
 * This only sends fields accepted by POST /api/v2/leads/register
 */
export function mapToBackendRegistration(frontendData: any): V2RegistrationPayload {
  const password = frontendData.motDePasse || frontendData.password || '';
  return {
    email: frontendData.email || '',
    password,
    password_confirm: password,
    first_name: frontendData.prenom || frontendData.first_name || '',
    last_name: frontendData.nom || frontendData.last_name || '',
    phone: formatPhone(frontendData.telephone || frontendData.phone || ''),
    date_of_birth: frontendData.dateNaissance || frontendData.date_of_birth || null,
    gender: mapValue(frontendData.civilite, GENDER_MAP) ?? frontendData.gender ?? null,
    country_code: 'TN', // Default Tunisia
    city_id: mapValue(frontendData.villeResidence, CITY_MAP) ?? frontendData.city_id ?? null,
    postal_code: frontendData.codePostal || frontendData.postal_code || null,
    channel: 2, // organic
    language: 'fr',
    device_type: null,
    source_type: 'platform_registration', // Backend only accepts 'social_ads' or 'platform_registration'
    campaign_id: null,
    referral_code: null,
    accept_terms: frontendData.acceptCGU ?? frontendData.accept_terms ?? true,
  };
}

/**
 * Map frontend form data to V2 profile update payload
 * Used after registration to update lead profile with detailed info
 */
export function mapToBackendProfile(frontendData: any): V2ProfilePayload {
  // Build activities array
  const activities: V2ProfilePayload['activities'] = [];

  if (frontendData.activitePrincipale) {
    const activityType = toInt(frontendData.activitePrincipale);
    const activityExp = EXPERIENCE_MAP[frontendData.experienceActivitePrincipale] || 1;

    if (activityType) {
      const operations: Array<{ operation_type: number; operation_experience: number }> = [];

      if (frontendData.operation1) {
        const opType = toInt(frontendData.operation1);
        const opExp = EXPERIENCE_MAP[frontendData.experienceOperation1] || 1;
        if (opType) {
          operations.push({ operation_type: opType, operation_experience: opExp });
        }
      }

      if (frontendData.operation2) {
        const opType = toInt(frontendData.operation2);
        const opExp = EXPERIENCE_MAP[frontendData.experienceOperation2] || 1;
        if (opType) {
          operations.push({ operation_type: opType, operation_experience: opExp });
        }
      }

      activities.push({
        activity_type: activityType,
        activity_experience: activityExp,
        operations,
      });
    }
  }

  return {
    desired_position: toInt(frontendData.posteRecherche),
    call_center_experience: EXPERIENCE_MAP[frontendData.experienceGlobale] || null,
    position_experience: EXPERIENCE_MAP[frontendData.experiencePosteRecherche] || null,
    activities,
    work_preferences: {
      mode: mapValue(frontendData.modeTravail, WORK_MODE_MAP) || toInt(frontendData.modeTravail),
      time: mapValue(frontendData.tempsTravail, WORK_TIME_MAP) || toInt(frontendData.tempsTravail),
      shift: mapValue(frontendData.parcTravail, WORK_SHIFT_MAP) || toInt(frontendData.parcTravail),
    },
  };
}

/**
 * Map frontend language form data to V2 language summary data
 * Used to update lead's summary_metrics language fields
 */
export function mapToBackendLanguages(frontendData: any): V2LanguageData {
  return {
    primary_language_code: mapValue(frontendData.langueMaternelle, LANGUAGE_CODE_MAP),
    primary_language_level: 7, // Native = 7
    secondary_language_code: mapValue(frontendData.langueForeign1, LANGUAGE_CODE_MAP),
    secondary_language_level: mapValue(frontendData.niveauLangueForeign1, LANGUAGE_LEVEL_MAP),
    tertiary_language_code: frontendData.hasSecondForeignLanguage === 'oui'
      ? mapValue(frontendData.langueForeign2, LANGUAGE_CODE_MAP)
      : null,
    tertiary_language_level: frontendData.hasSecondForeignLanguage === 'oui'
      ? mapValue(frontendData.niveauLangueForeign2, LANGUAGE_LEVEL_MAP)
      : null,
  };
}

/**
 * Map test scores to V2 language skill format for POST /api/v2/leads/:leadId/skills/language
 */
export function mapToBackendLanguageTest(testLanguage: string, testScore: any) {
  const languageCode = LANGUAGE_CODE_MAP[testLanguage?.toLowerCase()] || testLanguage?.toUpperCase() || 'FR';

  const scores = typeof testScore === 'object' ? {
    linguistic: testScore?.linguistic || 0,
    soft_skills: testScore?.softSkills || 0,
    job_skills: testScore?.jobSkills || 0,
    global: testScore?.overall || testScore?.global || 0,
  } : {
    linguistic: 0,
    soft_skills: 0,
    job_skills: 0,
    global: typeof testScore === 'number' ? testScore : 0,
  };

  return {
    language_code: languageCode,
    test_method: 1, // 1 = platform written test
    completed: true,
    scores,
    source: 1, // 1 = platform
    is_native: false,
    proficiency_level: null, // Will be computed by backend based on score
  };
}

// ============================================================================
// REVERSE MAPPING: Backend V2 → Frontend display values
// ============================================================================

const REVERSE_GENDER_MAP: Record<number, string> = { 0: 'monsieur', 1: 'madame', 2: 'autre' };
const REVERSE_CITY_MAP: Record<number, string> = Object.fromEntries(
  Object.entries(CITY_MAP).map(([k, v]) => [v, k])
);

export function mapFromBackendLead(lead: any) {
  return {
    _id: lead._id,
    email: lead.email,
    phone: lead.phone,
    name: lead.last_name,
    surname: lead.first_name,
    gender: REVERSE_GENDER_MAP[lead.gender] || '',
    city: REVERSE_CITY_MAP[lead.location?.city_id] || '',
    registration_date: lead.created_at,
    birthday: lead.date_of_birth,
    postal_code: lead.location?.postal_code || '',
  };
}

export function mapFromBackendProfile(profile: any) {
  return {
    desired_position: profile?.desired_position?.toString() || '',
    call_center_experience: profile?.call_center_experience?.toString() || '',
    position_experience: profile?.position_experience?.toString() || '',
    primary_activity: profile?.activities?.[0]?.activity_type?.toString() || '',
    primary_activity_experience: profile?.activities?.[0]?.activity_experience?.toString() || '',
    operation_1: profile?.activities?.[0]?.operations?.[0]?.operation_type?.toString() || '',
    operation_1_experience: profile?.activities?.[0]?.operations?.[0]?.operation_experience?.toString() || '',
    operation_2: profile?.activities?.[0]?.operations?.[1]?.operation_type?.toString() || '',
    operation_2_experience: profile?.activities?.[0]?.operations?.[1]?.operation_experience?.toString() || '',
    native_language: '', // Comes from lead summary_metrics
    foreign_language_1: '',
    foreign_language_1_level: '',
    foreign_language_2: '',
    foreign_language_2_level: '',
    is_bilingual: false,
  };
}

export function mapFromBackendAvailability(profile: any) {
  const REVERSE_WORK_MODE: Record<number, string> = { 1: 'teletravail', 2: 'presentiel', 3: 'hybride' };
  const REVERSE_WORK_TIME: Record<number, string> = { 1: 'temps_plein', 2: 'temps_partiel', 3: 'freelance', 4: 'stage', 5: 'alternance' };
  const REVERSE_WORK_SHIFT: Record<number, string> = { 1: 'jour', 2: 'apres_midi', 3: 'nuit' };

  return {
    work_mode: REVERSE_WORK_MODE[profile?.work_preferences?.mode] || '',
    work_time: REVERSE_WORK_TIME[profile?.work_preferences?.time] || '',
    work_park: REVERSE_WORK_SHIFT[profile?.work_preferences?.shift] || '',
    nearby_cities: profile?.work_preferences?.preferred_cities?.map((id: number) => REVERSE_CITY_MAP[id] || '') || [],
    blacklist: profile?.company_preferences?.blacklisted_companies?.map((c: any) => c.company_id) || [],
    whitelist: profile?.company_preferences?.whitelisted_companies || [],
    international_offers: profile?.availability?.international_offers || false,
  };
}

// Keep old mapToBackendFormat for backward compatibility during migration
// This wraps the new functions
export function mapToBackendFormat(frontendData: any) {
  return mapToBackendRegistration(frontendData);
}

export interface FrontendFormData {
  // Step 1 - from CandidateInscriptionForm
  posteRecherche: string;
  experienceGlobale: string;
  experiencePosteRecherche?: string;
  civilite: string;
  nom: string;
  prenom: string;
  telephone: string;
  villeResidence: string;
  // Step 2: Experience - from DetailedInscriptionForm
  activitePrincipale?: string;
  experienceActivitePrincipale?: string;
  operation1?: string;
  experienceOperation1?: string;
  operation2?: string;
  experienceOperation2?: string;
  // Step 3: Languages
  langueMaternelle: string;
  langueForeign1: string;
  niveauLangueForeign1: string;
  hasSecondForeignLanguage: string;
  langueForeign2?: string;
  niveauLangueForeign2?: string;
  langueForeign3?: string;
  niveauLangueForeign3?: string;
  bilingue: string;
  // Step 4: Work Preferences
  modeTravail: string;
  tempsTravail: string;
  parcTravail: string;
  // Step 5: Authentication
  email: string;
  motDePasse: string;
  acceptCGU?: boolean;
  // Optional fields
  birthday?: string;
  newsletter?: boolean;
}
