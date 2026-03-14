/**
 * @file candidateProfileController.mjs
 * @description Candidate info and profile update controllers
 */

import { asyncHandler } from '../../../middleware/errorHandler.mjs';
import { successResponse, errorResponse } from '../../../utils/responseHelper.mjs';
import { validatePhone, sanitizeString } from '../../../middleware/validation.mjs';
import CandidateModel from '../models/Candidate.mjs';
import CandidateProfileModel from '../models/CandidateProfile.mjs';
import { createAlert } from './candidateAlertController.mjs';

/**
 * Update main candidate information
 * PUT /api/candidates/info
 */
export const updateCandidateInfo = asyncHandler(async (req, res) => {
  const candidateId = req.user.id;
  const { name, surname, phone, city, gender } = req.body;
  
  console.log('💾 PUT /api/candidates/info - Updating basic info for authenticated user ID:', candidateId);
  console.log('📋 Update data:', { name, surname, phone, city, gender });

  const updateData = {};
  if (name !== undefined) updateData.name = sanitizeString(name);
  if (surname !== undefined) updateData.surname = sanitizeString(surname);
  if (phone !== undefined) updateData.phone = validatePhone(phone);
  // Don't escape city - it comes from controlled select and may have special chars
  if (city !== undefined) updateData.city = city?.trim() || '';
  // Don't escape gender - it comes from controlled select (madame/monsieur)
  if (gender !== undefined) updateData.gender = gender?.trim() || '';

  if (Object.keys(updateData).length === 0) {
    return errorResponse(res, 'No fields to update', 400);
  }

  try {
    await CandidateModel.update(candidateId, updateData);
    console.log('✅ Basic info updated successfully for user ID:', candidateId);

    // Create success alert
    await createAlert(candidateId, {
      type: 'success',
      title: 'Informations mises à jour',
      message: 'Vos informations personnelles ont été sauvegardées avec succès.',
      action_type: 'profile_update'
    });

    return successResponse(res, null, 'Candidate information updated successfully');
  } catch (error) {
    // Create error alert
    await createAlert(candidateId, {
      type: 'error',
      title: 'Échec de la mise à jour',
      message: 'Une erreur est survenue lors de la mise à jour de vos informations.',
      action_type: 'profile_update'
    });
    throw error;
  }
});

/**
 * Update candidate profile (CV/Profile information)
 * PUT /api/candidates/profile
 */
export const updateCandidateProfile = asyncHandler(async (req, res) => {
  const candidateId = req.user.id;
  const {
    desired_position,
    call_center_experience,
    position_experience,
    primary_activity,
    primary_activity_experience,
    operation_1,
    operation_1_experience,
    operation_2,
    operation_2_experience,
    native_language,
    foreign_language_1,
    foreign_language_1_level,
    foreign_language_2,
    foreign_language_2_level,
    is_bilingual
  } = req.body;

  console.log('💾 PUT /api/candidates/profile - Updating profile for authenticated user ID:', candidateId);
  console.log('📋 Update data:', { desired_position, call_center_experience, native_language, foreign_language_1 });

  const updateData = {};
  if (desired_position !== undefined) updateData.desired_position = sanitizeString(desired_position);
  if (call_center_experience !== undefined) updateData.call_center_experience = sanitizeString(call_center_experience);
  if (position_experience !== undefined) updateData.position_experience = sanitizeString(position_experience);
  if (primary_activity !== undefined) updateData.primary_activity = sanitizeString(primary_activity);
  if (primary_activity_experience !== undefined) updateData.primary_activity_experience = sanitizeString(primary_activity_experience);
  if (operation_1 !== undefined) updateData.operation_1 = sanitizeString(operation_1);
  if (operation_1_experience !== undefined) updateData.operation_1_experience = sanitizeString(operation_1_experience);
  if (operation_2 !== undefined) updateData.operation_2 = sanitizeString(operation_2);
  if (operation_2_experience !== undefined) updateData.operation_2_experience = sanitizeString(operation_2_experience);
  if (native_language !== undefined) updateData.native_language = sanitizeString(native_language);
  if (foreign_language_1 !== undefined) updateData.foreign_language_1 = sanitizeString(foreign_language_1);
  if (foreign_language_1_level !== undefined) updateData.foreign_language_1_level = sanitizeString(foreign_language_1_level);
  if (foreign_language_2 !== undefined) updateData.foreign_language_2 = sanitizeString(foreign_language_2);
  if (foreign_language_2_level !== undefined) updateData.foreign_language_2_level = sanitizeString(foreign_language_2_level);
  if (is_bilingual !== undefined) updateData.is_bilingual = is_bilingual;

  if (Object.keys(updateData).length === 0) {
    return errorResponse(res, 'No fields to update', 400);
  }

  try {
    await CandidateProfileModel.update(candidateId, updateData);
    console.log('✅ Profile updated successfully for user ID:', candidateId);

    // Create success alert
    await createAlert(candidateId, {
      type: 'success',
      title: 'Profil mis à jour',
      message: 'Vos informations de profil ont été sauvegardées avec succès.',
      action_type: 'profile_update'
    });

    return successResponse(res, null, 'Candidate profile updated successfully');
  } catch (error) {
    // Create error alert
    await createAlert(candidateId, {
      type: 'error',
      title: 'Échec de la mise à jour',
      message: 'Une erreur est survenue lors de la mise à jour de votre profil.',
      action_type: 'profile_update'
    });
    throw error;
  }
});
