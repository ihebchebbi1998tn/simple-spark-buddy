import { apiClient } from '../common/api';
import { authService } from './authService';
import {
  mapToBackendRegistration,
  mapToBackendProfile,
  mapToBackendLanguages,
  mapToBackendLanguageTest,
  type V2RegistrationPayload,
  type V2ProfilePayload,
} from '../common/fieldMapping';

// ============================================================================
// TYPES
// ============================================================================

export interface RegistrationFirstStepData {
  posteRecherche: string;
  experienceGlobale: string;
  experiencePosteRecherche?: string;
  civilite: string;
  nom: string;
  prenom: string;
  telephone: string;
  villeResidence: string;
}

export interface FullRegistrationData extends RegistrationFirstStepData {
  activitePrincipale?: string;
  experienceActivitePrincipale?: string;
  operation1?: string;
  experienceOperation1?: string;
  operation2?: string;
  experienceOperation2?: string;
  langueMaternelle: string;
  langueForeign1: string;
  niveauLangueForeign1: string;
  hasSecondForeignLanguage: string;
  langueForeign2?: string;
  niveauLangueForeign2?: string;
  bilingue: string;
  modeTravail: string;
  tempsTravail: string;
  parcTravail: string;
  email: string;
  motDePasse: string;
  dateNaissance?: string;
}

export interface V2RegisterResponse {
  success: boolean;
  message: string;
  data?: {
    lead_id: string;
    public_id: string;
    email: string;
    first_name: string;
    last_name: string;
  };
}

export interface V2LoginResponse {
  success: boolean;
  message: string;
  data?: {
    lead_id: string;
    public_id: string;
    email: string;
    first_name: string;
    last_name: string;
    token: string;
  };
}

// ============================================================================
// SERVICE
// ============================================================================

export const leadService = {
  /**
   * Register a new lead via V2 API
   * Step 1: Create the lead account
   * Step 2: Auto-login to get JWT token
   * Step 3: Update profile with detailed data (activities, languages, work prefs)
   */
  async register(frontendData: any): Promise<V2LoginResponse> {
    // Step 1: Register the lead
    const registrationPayload = mapToBackendRegistration(frontendData);
    console.log('📤 V2 Registration payload:', { ...registrationPayload, password: '[REDACTED]', date_of_birth: registrationPayload.date_of_birth });

    const registerResponse: V2RegisterResponse = await apiClient.post(
      '/api/v2/leads/register',
      registrationPayload
    );

    if (!registerResponse.success || !registerResponse.data) {
      throw new Error(registerResponse.message || 'Registration failed');
    }

    const leadId = registerResponse.data.lead_id;
    console.log('✅ Lead registered:', leadId);

    // Step 2: Try auto-login to get JWT token
    // NOTE: Backend may return 403 if account requires activation - this is expected
    let loginResponse: V2LoginResponse | null = null;
    try {
      loginResponse = await apiClient.post(
        '/api/v2/leads/login',
        {
          email: registrationPayload.email,
          password: registrationPayload.password,
        }
      );
    } catch (loginError: any) {
      console.warn('⚠️ Auto-login failed after registration (account may need activation):', loginError?.message);
    }

    if (!loginResponse?.success || !loginResponse?.data?.token) {
      // Registration succeeded but login failed (e.g. account not yet active)
      // Return success so user sees confirmation and can login later
      console.log('ℹ️ Registration successful, account pending activation');
      return {
        success: true,
        message: 'Inscription réussie ! Votre compte est en cours d\'activation.',
        data: {
          lead_id: leadId,
          public_id: registerResponse.data.public_id,
          email: registerResponse.data.email,
          first_name: registerResponse.data.first_name,
          last_name: registerResponse.data.last_name,
          token: '',
        },
      };
    }

    console.log('✅ Auto-login successful');

    // CRITICAL: Store the JWT token immediately so subsequent authenticated requests work
    authService.storeAuthToken(loginResponse.data.token);
    authService.storeUserData({
      lead_id: leadId,
      email: registerResponse.data.email,
      name: registerResponse.data.last_name,
      surname: registerResponse.data.first_name,
    });
    console.log('🔐 Token stored for subsequent profile update requests');

    // Step 3: Update profile with detailed data using specific sub-endpoints
    try {
      const profilePayload = mapToBackendProfile(frontendData);
      console.log('📤 V2 Profile update payload:', profilePayload);

      // 3a: Update activities via dedicated endpoint
      if (profilePayload.activities && profilePayload.activities.length > 0) {
        await apiClient.put(
          `/api/v2/leads/${leadId}/profile/activities`,
          { activities: profilePayload.activities },
          true
        );
        console.log('✅ Activities updated');
      }

      // 3b: Update work preferences via dedicated endpoint
      const wp = profilePayload.work_preferences;
      if (wp && (wp.mode || wp.time || wp.shift)) {
        await apiClient.put(
          `/api/v2/leads/${leadId}/profile/work-preferences`,
          { work_preferences: wp },
          true
        );
        console.log('✅ Work preferences updated');
      }

      console.log('✅ Profile updated with detailed data');
    } catch (profileError) {
      console.warn('⚠️ Profile update after registration failed (non-critical):', profileError);
    }

    // Step 3b: Save spoken languages as language skills
    try {
      const languageData = mapToBackendLanguages(frontendData);
      console.log('📤 V2 Language data:', languageData);

      // Save native/primary language
      if (languageData.primary_language_code) {
        await apiClient.post(
          `/api/v2/leads/${leadId}/skills/language`,
          {
            language_code: languageData.primary_language_code,
            is_native: true,
            proficiency_level: 7,
            test_method: 1,
            completed: true,
            scores: { linguistic: 100, soft_skills: 100, job_skills: 100, global: 100 },
            source: 1,
          },
          true
        );
        console.log('✅ Native language saved:', languageData.primary_language_code);
      }

      // Save first foreign language
      if (languageData.secondary_language_code && languageData.secondary_language_level) {
        await apiClient.post(
          `/api/v2/leads/${leadId}/skills/language`,
          {
            language_code: languageData.secondary_language_code,
            is_native: false,
            proficiency_level: languageData.secondary_language_level,
            test_method: 1,
            completed: false,
            source: 1,
          },
          true
        );
        console.log('✅ Foreign language 1 saved:', languageData.secondary_language_code);
      }

      // Save second foreign language if provided
      if (languageData.tertiary_language_code && languageData.tertiary_language_level) {
        await apiClient.post(
          `/api/v2/leads/${leadId}/skills/language`,
          {
            language_code: languageData.tertiary_language_code,
            is_native: false,
            proficiency_level: languageData.tertiary_language_level,
            test_method: 1,
            completed: false,
            source: 1,
          },
          true
        );
        console.log('✅ Foreign language 2 saved:', languageData.tertiary_language_code);
      }
    } catch (langError) {
      console.warn('⚠️ Language skills save after registration failed (non-critical):', langError);
    }

    // Step 4: Save language test scores if available
    if (frontendData.testLanguage && frontendData.testScore) {
      try {
        const languageTestPayload = mapToBackendLanguageTest(
          frontendData.testLanguage,
          frontendData.testScore
        );
        console.log('📤 V2 Language test payload:', languageTestPayload);

        await apiClient.post(
          `/api/v2/leads/${leadId}/skills/language`,
          languageTestPayload,
          true
        );
        console.log('✅ Language test scores saved');
      } catch (testError) {
        console.warn('⚠️ Language test save after registration failed (non-critical):', testError);
      }
    }

    // Return login response with token
    return loginResponse;
  },

  /**
   * Login a lead via V2 API
   */
  async login(email: string, password: string): Promise<V2LoginResponse> {
    return apiClient.post('/api/v2/leads/login', { email, password });
  },
};
