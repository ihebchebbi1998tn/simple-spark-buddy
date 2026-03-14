/**
 * @file candidateAuthController.mjs
 * @description Candidate registration controller
 */

import { asyncHandler } from '../../../middleware/errorHandler.mjs';
import { successResponse, errorResponse } from '../../../utils/responseHelper.mjs';
import { validateEmail, validatePhone, sanitizeString } from '../../../middleware/validation.mjs';
import { hashPassword, validatePasswordStrength } from '../../../utils/passwordHelper.mjs';
import { generateToken } from '../../../utils/jwtHelper.mjs';
import CandidateModel from '../models/Candidate.mjs';
import CandidateProfileModel from '../models/CandidateProfile.mjs';
import CandidateAvailabilityModel from '../models/CandidateAvailability.mjs';
import CandidateContractPreferencesModel from '../models/CandidateContractPreferences.mjs';
import CandidateTestScoresModel from '../models/CandidateTestScores.mjs';
import CandidateSystemInfoModel from '../models/CandidateSystemInfo.mjs';
import { createAlert } from './candidateAlertController.mjs';

/**
 * Complete candidate registration - Single step
 * POST /api/candidates/register
 */
/**
 * Check if email or phone already exists
 * POST /api/candidates/check-availability
 */
export const checkAvailability = asyncHandler(async (req, res) => {
  const { email, phone } = req.body;

  const result = {
    emailExists: false,
    phoneExists: false
  };

  // Check email if provided
  if (email) {
    try {
      const normalizedEmail = validateEmail(email);
      const existingEmail = await CandidateModel.findByEmail(normalizedEmail);
      result.emailExists = !!existingEmail;
    } catch (e) {
      // Invalid email format
      result.emailExists = false;
    }
  }

  // Check phone if provided
  if (phone) {
    try {
      const validPhone = validatePhone(phone);
      const collection = await CandidateModel.getCollection();
      const existingPhone = await collection.findOne({ phone: validPhone });
      result.phoneExists = !!existingPhone;
    } catch (e) {
      // Invalid phone format
      result.phoneExists = false;
    }
  }

  return successResponse(res, result, 'Availability check completed');
});

/**
 * Complete candidate registration - Single step
 * POST /api/candidates/register
 */
export const registerCandidate = asyncHandler(async (req, res) => {
  const {
    // Group A: Contact Information
    civilite,
    nom,
    prenom,
    telephone,
    villeResidence,
    email,
    motDePasse,
    
    // Group B: Profile/CV Information
    posteRecherche,
    experienceGlobale,
    experiencePosteRecherche,
    activitePrincipale,
    experienceActivitePrincipale,
    operation1,
    experienceOperation1,
    operation2,
    experienceOperation2,
    langueMaternelle,
    langueForeign1,
    niveauLangueForeign1,
    hasSecondForeignLanguage,
    langueForeign2,
    niveauLangueForeign2,
    bilingue,
    
    // Group C: Availability
    modeTravail,
    tempsTravail,
    parcTravail,
    
    // Test scores (if any)
    testLanguage,
    testScore,
    
    // Registration source
    registrationSource = 'platform'
  } = req.body;

  console.log('📝 Registration request received:', {
    email,
    experienceGlobale,
    experienceLabel: experienceGlobale === '0' ? 'Aucune expérience' : 
                     experienceGlobale === '1' ? '0-6 mois' :
                     experienceGlobale === '2' ? '6-12 mois' :
                     experienceGlobale === '3' ? '1-2 ans' :
                     experienceGlobale === '4' ? '2-3 ans' :
                     experienceGlobale === '5' ? '3-5 ans' :
                     experienceGlobale === '6' ? '5-7 ans' :
                     experienceGlobale === '7' ? 'Plus de 7 ans' : 'Unknown',
    posteRecherche,
    // Log test data
    testLanguage: testLanguage || 'NOT PROVIDED',
    testScore: testScore ? (typeof testScore === 'object' ? JSON.stringify(testScore) : testScore) : 'NOT PROVIDED',
    hasTestData: !!(testLanguage && testScore)
  });

  // Validate required fields
  if (!email || !motDePasse) {
    return errorResponse(res, 'Email and password are required', 400);
  }

  // Validate email and password
  const normalizedEmail = validateEmail(email);
  validatePasswordStrength(motDePasse);
  const validPhone = validatePhone(telephone);

  // Check if email or phone already exists
  const existingEmail = await CandidateModel.findByEmail(normalizedEmail);
  if (existingEmail) {
    return errorResponse(res, 'Email already registered', 409);
  }

  const existingPhone = await CandidateModel.getCollection()
    .then(col => col.findOne({ phone: validPhone }));
  if (existingPhone) {
    return errorResponse(res, 'Phone number already registered', 409);
  }

  // Hash password
  const passwordHash = await hashPassword(motDePasse);

  // Create main candidate record
  const candidateData = {
    email: normalizedEmail,
    phone: validPhone,
    name: sanitizeString(nom),
    surname: sanitizeString(prenom),
    gender: sanitizeString(civilite),
    city: sanitizeString(villeResidence),
    registration_source: registrationSource,
    registration_date: new Date(),
    is_deleted: false
  };

  const candidate = await CandidateModel.create(candidateData);
  const candidateId = candidate._id;

  // Create related records in parallel
  console.log('💾 Creating candidate profile with experience:', {
    experienceGlobale,
    posteRecherche,
    experiencePosteRecherche
  });
  
  await Promise.all([
    // Profile/CV
    CandidateProfileModel.create({
      candidate_id: candidateId,
      desired_position: sanitizeString(posteRecherche),
      call_center_experience: sanitizeString(experienceGlobale),
      position_experience: experiencePosteRecherche ? sanitizeString(experiencePosteRecherche) : null,
      primary_activity: activitePrincipale ? sanitizeString(activitePrincipale) : null,
      primary_activity_experience: experienceActivitePrincipale ? sanitizeString(experienceActivitePrincipale) : null,
      operation_1: operation1 ? sanitizeString(operation1) : null,
      operation_1_experience: experienceOperation1 ? sanitizeString(experienceOperation1) : null,
      operation_2: operation2 ? sanitizeString(operation2) : null,
      operation_2_experience: experienceOperation2 ? sanitizeString(experienceOperation2) : null,
      native_language: sanitizeString(langueMaternelle),
      foreign_language_1: sanitizeString(langueForeign1),
      foreign_language_1_level: sanitizeString(niveauLangueForeign1),
      foreign_language_2: langueForeign2 ? sanitizeString(langueForeign2) : null,
      foreign_language_2_level: niveauLangueForeign2 ? sanitizeString(niveauLangueForeign2) : null,
      is_bilingual: bilingue === 'oui'
    }),

    // Availability
    // Note: Don't sanitize work_mode, work_time, work_park - they're from controlled inputs
    // and sanitization corrupts special characters
    CandidateAvailabilityModel.create({
      candidate_id: candidateId,
      work_mode: modeTravail?.trim() || '',
      work_time: tempsTravail?.trim() || '',
      work_park: parcTravail?.trim() || ''
    }),

    // Contract Preferences
    CandidateContractPreferencesModel.create({
      candidate_id: candidateId
    }),

    // Test Scores - Always create with all language fields initialized
    (async () => {
      const testScoreData = {
        candidate_id: candidateId,
        cgu_accepted: true,
        cgu_accepted_at: new Date(),
      };
      
      // If user completed a language test before registration, save those scores
      if (testScore && testLanguage) {
        // Map frontend language codes to backend field names
        const languageMapping = {
          'francais': 'french',
          'french': 'french',
          'english': 'english',
          'deutsch': 'german',
          'german': 'german',
          'italiano': 'italian',
          'italian': 'italian',
          'espanol': 'spanish',
          'spanish': 'spanish'
        };
        
        const language = languageMapping[testLanguage.toLowerCase()] || testLanguage.toLowerCase();
        
        console.log('📊 Processing test scores for registration:', {
          originalLanguage: testLanguage,
          mappedLanguage: language,
          testScore: typeof testScore === 'object' ? testScore : { overall: testScore }
        });
        
        // Parse test score if it's a complete score object or single number
        let scores = {};
        if (typeof testScore === 'object' && testScore !== null) {
          scores = {
            linguistic: testScore.linguistic || testScore.linguisticScore || null,
            softSkills: testScore.softSkills || testScore.soft_skills_score || null,
            jobSkills: testScore.jobSkills || testScore.job_skills_score || null,
            overall: testScore.overall || testScore.overallScore || parseInt(testScore.score) || null
          };
        } else {
          // If it's just a single score number, use it as overall score
          scores.overall = parseInt(testScore);
        }
        
        console.log('✅ Parsed test scores:', scores);
        
        // Set the specific language test fields
        testScoreData[`${language}_test_completed`] = true;
        testScoreData[`${language}_linguistic_score`] = scores.linguistic;
        testScoreData[`${language}_soft_skills_score`] = scores.softSkills;
        testScoreData[`${language}_job_skills_score`] = scores.jobSkills;
        testScoreData[`${language}_overall_score`] = scores.overall;
      }
      
      return CandidateTestScoresModel.create(testScoreData);
    })(),

    // System Info
    CandidateSystemInfoModel.create({
      candidate_id: candidateId,
      password_hash: passwordHash,
      registration_ip: req.ip || req.connection.remoteAddress,
      registration_device: req.headers['user-agent'],
      last_login: new Date(),
      failed_login_attempts: 0,
      is_locked: false
    })
  ]);

  // Generate JWT token
  const token = generateToken({
    id: candidateId,
    email: normalizedEmail,
    role: 'candidate'
  });

  // Create welcome alert
  await createAlert(candidateId, {
    type: 'success',
    title: 'Bienvenue sur CCM!',
    message: 'Votre inscription a été réalisée avec succès. Complétez votre profil pour maximiser vos opportunités.',
    action_type: 'system'
  });

  console.log('✅ Registration successful:', {
    candidateId: candidateId.toString(),
    email: normalizedEmail,
    experienceGlobale,
    allDataSaved: true
  });

  // Return success with token
  return successResponse(res, {
    token,
    user: {
      id: candidateId,
      email: normalizedEmail,
      name: candidateData.name,
      surname: candidateData.surname,
      phone: candidateData.phone,
      city: candidateData.city
    }
  }, 'Registration completed successfully', 201);
});
