/**
 * @file profileController.mjs
 * @description API handlers for LEAD_PROFILES
 * @version 1.0
 */

import { validateProfileUpdate, validateProfileQuery } from '../validators/profileValidator.mjs';
import { updateLeadActivity } from '../services/leadService.mjs';
import {
    getProfileByLeadId,
    updateProfile,
    updateActivities,
    updateWorkPreferences,
    updateCompensation
} from '../services/profileService.mjs';

/**
 * ============================================================================
 * GET PROFILE ENDPOINTS
 * ============================================================================
 */

/**
 * GET /api/v2/leads/:leadId/profile
 * Get lead profile details
 */
export const getProfile = async (req, res) => {
    try {
        const profile = await getProfileByLeadId(req.params.leadId);

        if (!profile) {
            return res.status(404).json({
                success: false,
                message: 'Profile not found'
            });
        }

        // Update activity
        await updateLeadActivity(req.params.leadId);

        return res.status(200).json({
            success: true,
            data: profile
        });
    } catch (error) {
        console.error('Get profile error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to retrieve profile',
            error: error.message
        });
    }
};

/**
 * ============================================================================
 * UPDATE PROFILE ENDPOINTS
 * ============================================================================
 */

/**
 * PUT /api/v2/leads/:leadId/profile
 * Update complete profile
 */
export const updateLeadProfile = async (req, res) => {
    try {
        // Validate profile data
        const validation = validateLeadProfile(req.body);
        if (!validation.isValid) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: validation.errors
            });
        }

        // Update profile
        const updatedProfile = await updateProfile(req.params.leadId, req.body);

        if (!updatedProfile) {
            return res.status(404).json({
                success: false,
                message: 'Profile not found'
            });
        }

        // Update activity
        await updateLeadActivity(req.params.leadId);

        return res.status(200).json({
            success: true,
            message: 'Profile updated successfully',
            data: updatedProfile
        });
    } catch (error) {
        console.error('Update profile error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to update profile',
            error: error.message
        });
    }
};

/**
 * PUT /api/v2/leads/:leadId/profile/activities
 * Update activities (1-3)
 */
export const updateActivitiesEndpoint = async (req, res) => {
    try {
        const { activities } = req.body;

        // Validate activities
        if (
            !activities ||
            !Array.isArray(activities) ||
            activities.length === 0 ||
            activities.length > 3
        ) {
            return res.status(400).json({
                success: false,
                message: 'Invalid activities',
                errors: {
                    activities: 'Required: array of 1-3 activities'
                }
            });
        }

        // Validate each activity
        for (let i = 0; i < activities.length; i++) {
            const activity = activities[i];
            if (!activity.activity_type || activity.activity_type < 1 || activity.activity_type > 4) {
                return res.status(400).json({
                    success: false,
                    message: 'Invalid activity type',
                    errors: { [`activities[${i}].activity_type`]: 'Invalid type (1-4)' }
                });
            }
            if (
                !activity.activity_experience ||
                activity.activity_experience < 1 ||
                activity.activity_experience > 7
            ) {
                return res.status(400).json({
                    success: false,
                    message: 'Invalid experience level',
                    errors: { [`activities[${i}].activity_experience`]: 'Invalid level (1-7)' }
                });
            }
        }

        // Update activities
        const updatedProfile = await updateActivities(req.params.leadId, activities);

        if (!updatedProfile) {
            return res.status(404).json({
                success: false,
                message: 'Profile not found'
            });
        }

        await updateLeadActivity(req.params.leadId);

        return res.status(200).json({
            success: true,
            message: 'Activities updated successfully',
            data: {
                lead_id: req.params.leadId,
                activities: updatedProfile.activities
            }
        });
    } catch (error) {
        console.error('Update activities error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to update activities',
            error: error.message
        });
    }
};

/**
 * PUT /api/v2/leads/:leadId/profile/work-preferences
 * Update work preferences
 */
export const updateWorkPreferencesEndpoint = async (req, res) => {
    try {
        const { work_preferences } = req.body;

        if (!work_preferences) {
            return res.status(400).json({
                success: false,
                message: 'Work preferences required'
            });
        }

        // Validate work preferences
        if (work_preferences.mode && ![1, 2, 3].includes(work_preferences.mode)) {
            return res.status(400).json({
                success: false,
                message: 'Invalid work mode',
                errors: { 'work_preferences.mode': 'Invalid mode (1-3)' }
            });
        }

        if (work_preferences.time && ![1, 2, 3, 4, 5].includes(work_preferences.time)) {
            return res.status(400).json({
                success: false,
                message: 'Invalid work time',
                errors: { 'work_preferences.time': 'Invalid time (1-5)' }
            });
        }

        if (work_preferences.shift && ![1, 2, 3].includes(work_preferences.shift)) {
            return res.status(400).json({
                success: false,
                message: 'Invalid shift',
                errors: { 'work_preferences.shift': 'Invalid shift (1-3)' }
            });
        }

        // Update work preferences
        const updatedProfile = await updateWorkPreferences(req.params.leadId, work_preferences);

        if (!updatedProfile) {
            return res.status(404).json({
                success: false,
                message: 'Profile not found'
            });
        }

        await updateLeadActivity(req.params.leadId);

        return res.status(200).json({
            success: true,
            message: 'Work preferences updated successfully',
            data: {
                lead_id: req.params.leadId,
                work_preferences: updatedProfile.work_preferences
            }
        });
    } catch (error) {
        console.error('Update work preferences error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to update work preferences',
            error: error.message
        });
    }
};

/**
 * PUT /api/v2/leads/:leadId/profile/compensation
 * Update compensation expectations
 */
export const updateCompensationEndpoint = async (req, res) => {
    try {
        const { compensation } = req.body;

        if (!compensation) {
            return res.status(400).json({
                success: false,
                message: 'Compensation data required'
            });
        }

        // Basic validation
        if (
            compensation.salary_expectation &&
            compensation.salary_expectation.period &&
            ![1, 2, 3, 4].includes(compensation.salary_expectation.period)
        ) {
            return res.status(400).json({
                success: false,
                message: 'Invalid salary period',
                errors: { 'compensation.salary_expectation.period': 'Invalid period (1-4)' }
            });
        }

        // Update compensation
        const updatedProfile = await updateCompensation(req.params.leadId, compensation);

        if (!updatedProfile) {
            return res.status(404).json({
                success: false,
                message: 'Profile not found'
            });
        }

        await updateLeadActivity(req.params.leadId);

        return res.status(200).json({
            success: true,
            message: 'Compensation updated successfully',
            data: {
                lead_id: req.params.leadId,
                compensation: updatedProfile.compensation
            }
        });
    } catch (error) {
        console.error('Update compensation error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to update compensation',
            error: error.message
        });
    }
};

/**
 * ============================================================================
 * EXPORT
 * ============================================================================
 */

export default {
    getProfile,
    updateLeadProfile,
    updateActivitiesEndpoint,
    updateWorkPreferencesEndpoint,
    updateCompensationEndpoint
};
