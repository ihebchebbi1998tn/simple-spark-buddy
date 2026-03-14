/**
 * @file candidateProximityController.mjs
 * @description Controller for managing candidate geographic proximity preferences
 */

import { asyncHandler } from '../../../middleware/errorHandler.mjs';
import { successResponse, errorResponse } from '../../../utils/responseHelper.mjs';
import CandidateModel from '../models/Candidate.mjs';
import CandidateAvailabilityModel from '../models/CandidateAvailability.mjs';
import { createAlert } from './candidateAlertController.mjs';

/**
 * Update candidate's geographic proximity preferences
 * Updates city preferences in candidate and availability collections
 */
export const updateProximityPreferences = asyncHandler(async (req, res) => {
  const candidateId = req.user.id;
  const { 
    villeResidence,
    villesProximite,
    offresEtranger 
  } = req.body;

  console.log('💾 PUT /api/candidates/proximity - Updating proximity for user ID:', candidateId);
  console.log('📋 Update data:', { villeResidence, villesProximite, offresEtranger });

  // Update main candidate city if provided
  // Don't sanitize - city names contain special characters (é, è, etc.)
  if (villeResidence) {
    await CandidateModel.update(candidateId, {
      city: villeResidence.trim()
    });
    console.log('✅ Updated candidate city to:', villeResidence.trim());
  }

  // Build availability update object
  const availabilityUpdate = {};

  // Update proximity cities in availability collection
  // Don't sanitize - city names contain special characters (Béja, Médenine, etc.)
  if (villesProximite !== undefined) {
    const proximityArray = Array.isArray(villesProximite) 
      ? villesProximite.map(v => typeof v === 'string' ? v.trim() : v)
      : [];
    availabilityUpdate.nearby_cities = proximityArray;
    console.log('📍 Will save nearby_cities:', proximityArray);
  }

  // Update international offers preference
  if (offresEtranger !== undefined) {
    availabilityUpdate.international_offers = Boolean(offresEtranger);
    console.log('🌍 Will save international_offers:', Boolean(offresEtranger));
  }

  // Only update availability if there's something to update
  if (Object.keys(availabilityUpdate).length > 0) {
    await CandidateAvailabilityModel.update(candidateId, availabilityUpdate);
    console.log('✅ Availability updated with:', availabilityUpdate);
    
    // Verify the save
    const verifyData = await CandidateAvailabilityModel.findByCandidateId(candidateId);
    console.log('🔍 Verified saved data:', {
      nearby_cities: verifyData?.nearby_cities,
      international_offers: verifyData?.international_offers
    });
  }

  // Create success alert
  await createAlert(candidateId, {
    type: 'success',
    title: 'Préférences de proximité mises à jour',
    message: 'Vos préférences de proximité géographique ont été mises à jour avec succès.',
    action_type: 'proximity_update'
  });

  return successResponse(res, null, 'Préférences de proximité géographique mises à jour avec succès');
});
