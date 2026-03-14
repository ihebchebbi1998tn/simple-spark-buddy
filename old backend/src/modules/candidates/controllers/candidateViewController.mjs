/**
 * @file candidateViewController.mjs
 * @description Candidate profile view controller
 */

import { asyncHandler } from '../../../middleware/errorHandler.mjs';
import { successResponse, errorResponse } from '../../../utils/responseHelper.mjs';
import CandidateModel from '../models/Candidate.mjs';
import CandidateProfileModel from '../models/CandidateProfile.mjs';
import CandidateAvailabilityModel from '../models/CandidateAvailability.mjs';
import CandidateContractPreferencesModel from '../models/CandidateContractPreferences.mjs';
import CandidateTestScoresModel from '../models/CandidateTestScores.mjs';

/**
 * Get candidate profile
 * GET /api/candidates/profile
 */
export const getProfile = asyncHandler(async (req, res) => {
  const candidateId = req.user.id;
  console.log('🔍 GET /api/candidates/profile - Fetching data for authenticated user ID:', candidateId);

  // Fetch all candidate data in parallel
  const [candidate, profile, availability, contractPrefs, testScores] = await Promise.all([
    CandidateModel.findById(candidateId),
    CandidateProfileModel.findByCandidateId(candidateId),
    CandidateAvailabilityModel.findByCandidateId(candidateId),
    CandidateContractPreferencesModel.findByCandidateId(candidateId),
    CandidateTestScoresModel.findByCandidateId(candidateId)
  ]);

  if (!candidate) {
    console.log('❌ Candidate not found for ID:', candidateId);
    return errorResponse(res, 'Candidate not found', 404);
  }

  console.log('✅ Profile data fetched successfully for user ID:', candidateId);
  console.log('📊 Availability data from DB:', availability ? { 
    work_mode: availability.work_mode, 
    work_time: availability.work_time, 
    work_park: availability.work_park,
    blacklist: availability.blacklist,
    whitelist: availability.whitelist
  } : 'null');
  console.log('📊 Contract preferences from DB:', contractPrefs);
  console.log('📊 TestScores notification settings from DB:', testScores ? {
    email_notifications: testScores.email_notifications,
    sms_notifications: testScores.sms_notifications,
    profile_visibility: testScores.profile_visibility,
    secure_data_sharing: testScores.secure_data_sharing
  } : 'null');
  
  return successResponse(res, {
    candidate,
    profile,
    availability,
    contractPreferences: contractPrefs,
    testScores
  });
});

/**
 * Get all candidates with complete information
 * GET /api/candidates/all
 */
export const getAllCandidates = asyncHandler(async (req, res) => {
  console.log('🔍 GET /api/candidates/all - Fetching all candidates');

  // Get pagination parameters
  const page = parseInt(req.query.page) || 1;
  const limit = parseInt(req.query.limit) || 50;
  const skip = (page - 1) * limit;

  // Get all candidates with pagination
  const candidates = await CandidateModel.findAll({}, { skip, limit });
  
  // Fetch complete data for each candidate
  const candidatesWithDetails = await Promise.all(
    candidates.map(async (candidate) => {
      const [profile, availability, contractPrefs, testScores] = await Promise.all([
        CandidateProfileModel.findByCandidateId(candidate._id),
        CandidateAvailabilityModel.findByCandidateId(candidate._id),
        CandidateContractPreferencesModel.findByCandidateId(candidate._id),
        CandidateTestScoresModel.findByCandidateId(candidate._id)
      ]);

      return {
        candidate,
        profile,
        availability,
        contractPreferences: contractPrefs,
        testScores
      };
    })
  );

  console.log(`✅ Fetched ${candidatesWithDetails.length} candidates successfully`);
  
  return successResponse(res, {
    candidates: candidatesWithDetails,
    pagination: {
      page,
      limit,
      total: candidatesWithDetails.length
    }
  });
});

/**
 * Get candidate by ID with complete information
 * GET /api/candidates/:id
 */
export const getCandidateById = asyncHandler(async (req, res) => {
  const { id } = req.params;
  console.log('🔍 GET /api/candidates/:id - Fetching data for candidate ID:', id);

  // Fetch all candidate data in parallel
  const [candidate, profile, availability, contractPrefs, testScores] = await Promise.all([
    CandidateModel.findById(id),
    CandidateProfileModel.findByCandidateId(id),
    CandidateAvailabilityModel.findByCandidateId(id),
    CandidateContractPreferencesModel.findByCandidateId(id),
    CandidateTestScoresModel.findByCandidateId(id)
  ]);

  if (!candidate) {
    console.log('❌ Candidate not found for ID:', id);
    return errorResponse(res, 'Candidate not found', 404);
  }

  console.log('✅ Candidate data fetched successfully for ID:', id);
  return successResponse(res, {
    candidate,
    profile,
    availability,
    contractPreferences: contractPrefs,
    testScores
  });
});
