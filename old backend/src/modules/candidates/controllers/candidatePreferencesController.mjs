/**
 * @file candidatePreferencesController.mjs
 * @description Candidate availability and contract preferences controllers
 */

import { asyncHandler } from '../../../middleware/errorHandler.mjs';
import { successResponse, errorResponse } from '../../../utils/responseHelper.mjs';
import { sanitizeString } from '../../../middleware/validation.mjs';
import CandidateAvailabilityModel from '../models/CandidateAvailability.mjs';
import CandidateContractPreferencesModel from '../models/CandidateContractPreferences.mjs';
import CandidateTestScoresModel from '../models/CandidateTestScores.mjs';
import { createAlert } from './candidateAlertController.mjs';

/**
 * Update availability preferences
 * PUT /api/candidates/availability
 */
export const updateAvailability = asyncHandler(async (req, res) => {
  const candidateId = req.user.id;
  const { availability, work_mode, work_time, work_park } = req.body;

  console.log('💾 PUT /api/candidates/availability - Updating availability for authenticated user ID:', candidateId);
  console.log('📋 Update data (before processing):', { availability, work_mode, work_time, work_park });

  const updateData = {};
  // Don't sanitize these fields - they're from controlled RadioGroups with predefined values
  // Sanitizing would convert special characters (é → &eacute;) breaking the match
  if (availability !== undefined) updateData.availability = availability.trim();
  if (work_mode !== undefined) updateData.work_mode = work_mode.trim();
  if (work_time !== undefined) updateData.work_time = work_time.trim();
  if (work_park !== undefined) updateData.work_park = work_park.trim();

  if (Object.keys(updateData).length === 0) {
    return errorResponse(res, 'No fields to update', 400);
  }

  try {
    console.log('📋 Update data (after processing):', updateData);
    const result = await CandidateAvailabilityModel.update(candidateId, updateData);
    console.log('✅ Availability updated successfully for user ID:', candidateId);
    console.log('📊 MongoDB update result:', { matchedCount: result.matchedCount, modifiedCount: result.modifiedCount, upsertedCount: result.upsertedCount });

    // Fetch and return the updated availability to confirm
    const updatedAvailability = await CandidateAvailabilityModel.findByCandidateId(candidateId);
    console.log('🔍 Fetched updated availability from DB:', updatedAvailability);

    // Create success alert
    await createAlert(candidateId, {
      type: 'success',
      title: 'Disponibilités mises à jour',
      message: 'Vos préférences de disponibilité ont été sauvegardées avec succès.',
      action_type: 'availability_update'
    });

    return successResponse(res, null, 'Availability preferences updated successfully');
  } catch (error) {
    // Create error alert
    await createAlert(candidateId, {
      type: 'error',
      title: 'Échec de la mise à jour',
      message: 'Une erreur est survenue lors de la mise à jour de vos disponibilités.',
      action_type: 'availability_update'
    });
    throw error;
  }
});

/**
 * Update contract preferences
 * PUT /api/candidates/contract-preferences
 */
export const updateContractPreferences = asyncHandler(async (req, res) => {
  const candidateId = req.user.id;
  const updateData = req.body;

  console.log('💾 PUT /api/candidates/contract-preferences - Updating for authenticated user ID:', candidateId);
  console.log('📋 Update data received:', JSON.stringify(updateData, null, 2));
  console.log('📋 contract_types received:', updateData.contract_types, 'isArray:', Array.isArray(updateData.contract_types));

  if (Object.keys(updateData).length === 0) {
    return errorResponse(res, 'No fields to update', 400);
  }

  const result = await CandidateContractPreferencesModel.update(candidateId, updateData);
  console.log('✅ Contract preferences updated successfully for user ID:', candidateId);
  console.log('📊 MongoDB update result:', { matchedCount: result.matchedCount, modifiedCount: result.modifiedCount, upsertedCount: result.upsertedCount, upsertedId: result.upsertedId });

  // Fetch and return the updated preferences to confirm save worked
  const updatedPreferences = await CandidateContractPreferencesModel.findByCandidateId(candidateId);
  console.log('🔍 Fetched updated contract preferences from DB:', JSON.stringify(updatedPreferences, null, 2));
  console.log('🔍 contract_types in DB:', updatedPreferences?.contract_types);

  // Create success alert
  await createAlert(candidateId, {
    type: 'success',
    title: 'Préférences contractuelles mises à jour',
    message: 'Vos préférences contractuelles ont été sauvegardées avec succès.',
    action_type: 'contract_update'
  });

  return successResponse(res, { contractPreferences: updatedPreferences }, 'Contract preferences updated successfully');
});

/**
 * Update call center blacklist/whitelist
 * PUT /api/candidates/call-centers
 */
export const updateCallCenterPreferences = asyncHandler(async (req, res) => {
  const candidateId = req.user.id;
  const { blacklist, whitelist } = req.body;

  console.log('💾 PUT /api/candidates/call-centers - Updating for user ID:', candidateId);
  console.log('📋 Call centers data received:', JSON.stringify({ blacklist, whitelist }));

  const updateData = {};
  if (blacklist !== undefined) updateData.blacklist = blacklist;
  if (whitelist !== undefined) updateData.whitelist = whitelist;

  if (Object.keys(updateData).length === 0) {
    return errorResponse(res, 'No fields to update', 400);
  }

  console.log('📍 Saving call centers to DB:', JSON.stringify(updateData));
  const result = await CandidateAvailabilityModel.update(candidateId, updateData);
  console.log('✅ Call centers updated. MongoDB result:', JSON.stringify({ 
    matchedCount: result.matchedCount, 
    modifiedCount: result.modifiedCount,
    upsertedCount: result.upsertedCount,
    upsertedId: result.upsertedId
  }));

  // Verify the save by reading back
  const savedData = await CandidateAvailabilityModel.findByCandidateId(candidateId);
  console.log('🔍 Verification - Data in DB after save:', JSON.stringify({
    blacklist: savedData?.blacklist,
    whitelist: savedData?.whitelist
  }));

  // Create success alert
  await createAlert(candidateId, {
    type: 'success',
    title: 'Centres d\'appel mis à jour',
    message: 'Vos préférences de centres d\'appel ont été sauvegardées avec succès.',
    action_type: 'callcenter_update'
  });

  return successResponse(res, { 
    blacklist: savedData?.blacklist || [],
    whitelist: savedData?.whitelist || []
  }, 'Call center preferences updated successfully');
});

/**
 * Update notification and privacy settings
 * PUT /api/candidates/notification-settings
 */
export const updateNotificationSettings = asyncHandler(async (req, res) => {
  const candidateId = req.user.id;
  const settingsData = req.body;

  console.log('💾 PUT /api/candidates/notification-settings - Updating for user ID:', candidateId);
  console.log('📋 Settings data received:', settingsData);

  const { email_notifications, sms_notifications, profile_visibility, secure_data_sharing } = settingsData;

  const updateData = {};
  if (email_notifications !== undefined) updateData.email_notifications = email_notifications;
  if (sms_notifications !== undefined) updateData.sms_notifications = sms_notifications;
  if (profile_visibility !== undefined) updateData.profile_visibility = profile_visibility;
  if (secure_data_sharing !== undefined) updateData.secure_data_sharing = secure_data_sharing;

  if (Object.keys(updateData).length === 0) {
    return errorResponse(res, 'No fields to update', 400);
  }

  // First check if document exists
  const existingDoc = await CandidateTestScoresModel.findByCandidateId(candidateId);
  console.log('📍 Existing testScores document:', existingDoc ? { _id: existingDoc._id, candidate_id: existingDoc.candidate_id } : 'NOT FOUND');

  console.log('📍 Saving notification settings:', updateData);
  const result = await CandidateTestScoresModel.update(candidateId, updateData);
  console.log('✅ Notification settings updated. MongoDB result:', { matchedCount: result.matchedCount, modifiedCount: result.modifiedCount });

  // Fetch updated settings
  const updatedSettings = await CandidateTestScoresModel.findByCandidateId(candidateId);

  // Create success alert
  await createAlert(candidateId, {
    type: 'success',
    title: 'Paramètres de confidentialité mis à jour',
    message: 'Vos préférences de notification et confidentialité ont été sauvegardées.',
    action_type: 'settings_update'
  });

  return successResponse(res, { 
    notificationSettings: {
      email_notifications: updatedSettings?.email_notifications,
      sms_notifications: updatedSettings?.sms_notifications,
      profile_visibility: updatedSettings?.profile_visibility,
      secure_data_sharing: updatedSettings?.secure_data_sharing
    }
  }, 'Notification settings updated successfully');
});
