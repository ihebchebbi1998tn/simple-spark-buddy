/**
 * @file profileService.mjs
 * @description Business logic for LEAD_PROFILES CRUD operations
 * @version 1.0
 */

import { LeadProfile } from '../models/index.mjs';

/**
 * ============================================================================
 * PROFILE RETRIEVAL
 * ============================================================================
 */

/**
 * Get lead profile by lead ID
 * @param {string} leadId - Lead MongoDB ID
 * @returns {Promise<object|null>} LeadProfile document or null
 */
export const getProfileByLeadId = async (leadId) => {
    try {
        return await LeadProfile.findOne({ lead_id: leadId });
    } catch (error) {
        throw error;
    }
};

/**
 * ============================================================================
 * PROFILE UPDATES
 * ============================================================================
 */

/**
 * Update lead profile
 * @param {string} leadId - Lead ID
 * @param {object} profileData - Profile data to update
 * @returns {Promise<object>} Updated profile
 */
export const updateProfile = async (leadId, profileData) => {
    try {
        const updateData = {};

        // Position & Experience
        if (profileData.desired_position !== undefined) {
            updateData.desired_position = profileData.desired_position;
        }
        if (profileData.call_center_experience !== undefined) {
            updateData.call_center_experience = profileData.call_center_experience;
        }
        if (profileData.call_center_self_assessment !== undefined) {
            updateData.call_center_self_assessment = profileData.call_center_self_assessment;
        }
        if (profileData.position_experience !== undefined) {
            updateData.position_experience = profileData.position_experience;
        }
        if (profileData.position_self_assessment !== undefined) {
            updateData.position_self_assessment = profileData.position_self_assessment;
        }

        // Activities
        if (profileData.activities !== undefined) {
            updateData.activities = profileData.activities;
        }

        // Work preferences
        if (profileData.work_preferences !== undefined) {
            updateData.work_preferences = profileData.work_preferences;
        }

        // Availability
        if (profileData.availability !== undefined) {
            updateData.availability = profileData.availability;
        }

        // Company preferences
        if (profileData.company_preferences !== undefined) {
            updateData.company_preferences = profileData.company_preferences;
        }

        // Compensation
        if (profileData.compensation !== undefined) {
            updateData.compensation = profileData.compensation;
        }

        // Bio
        if (profileData.bio !== undefined) {
            updateData.bio = profileData.bio;
        }

        const updatedProfile = await LeadProfile.findOneAndUpdate(
            { lead_id: leadId },
            updateData,
            { new: true, runValidators: true }
        );

        return updatedProfile;
    } catch (error) {
        throw error;
    }
};

/**
 * Update activities
 * @param {string} leadId - Lead ID
 * @param {array} activities - Activities array (1-3)
 * @returns {Promise<object>} Updated profile
 */
export const updateActivities = async (leadId, activities) => {
    try {
        const updatedProfile = await LeadProfile.findOneAndUpdate(
            { lead_id: leadId },
            { activities },
            { new: true, runValidators: true }
        );

        return updatedProfile;
    } catch (error) {
        throw error;
    }
};

/**
 * Update work preferences
 * @param {string} leadId - Lead ID
 * @param {object} workPreferences - Work preferences object
 * @returns {Promise<object>} Updated profile
 */
export const updateWorkPreferences = async (leadId, workPreferences) => {
    try {
        const updatedProfile = await LeadProfile.findOneAndUpdate(
            { lead_id: leadId },
            { work_preferences: workPreferences },
            { new: true, runValidators: true }
        );

        return updatedProfile;
    } catch (error) {
        throw error;
    }
};

/**
 * Update compensation
 * @param {string} leadId - Lead ID
 * @param {object} compensation - Compensation object
 * @returns {Promise<object>} Updated profile
 */
export const updateCompensation = async (leadId, compensation) => {
    try {
        const updatedProfile = await LeadProfile.findOneAndUpdate(
            { lead_id: leadId },
            { compensation },
            { new: true, runValidators: true }
        );

        return updatedProfile;
    } catch (error) {
        throw error;
    }
};

/**
 * ============================================================================
 * PROFILE EXPORT
 * ============================================================================
 */

export default {
    getProfileByLeadId,
    updateProfile,
    updateActivities,
    updateWorkPreferences,
    updateCompensation
};
