/**
 * Settings Controller
 * Handles both user-editable and admin-controlled settings
 * Provides role-based field access control
 */

import { validateUserSettingsUpdate, validateAdminSettingsUpdate } from '../validators/settingsValidator.mjs';
import settingsService from '../services/settingsService.mjs';

export const settingsController = {
    /**
     * Get settings for current user
     * GET /leads/:leadId/settings
     */
    async getSettings(req, res) {
        try {
            const settings = await settingsService.getUserSettings(req.params.leadId);

            return res.status(200).json({
                success: true,
                data: settings
            });
        } catch (error) {
            if (error.code === 'SETTINGS_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Settings not found'
                });
            }

            console.error('[Settings Get Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch settings',
                error: error.message
            });
        }
    },

    /**
     * Get field accessibility info
     * GET /leads/:leadId/settings/accessibility
     */
    async getAccessibility(req, res) {
        try {
            const accessibility = await settingsService.getFieldAccessibility(req.params.leadId);

            return res.status(200).json({
                success: true,
                data: accessibility
            });
        } catch (error) {
            if (error.code === 'SETTINGS_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Settings not found'
                });
            }

            console.error('[Accessibility Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch accessibility info',
                error: error.message
            });
        }
    },

    /**
     * Update user-editable settings
     * PUT /leads/:leadId/settings
     */
    async updateUserSettings(req, res) {
        try {
            const { isValid, errors } = validateUserSettingsUpdate(req.body);

            if (!isValid) {
                return res.status(400).json({
                    success: false,
                    message: 'Validation error',
                    errors
                });
            }

            const settings = await settingsService.updateUserSettings(
                req.params.leadId,
                req.body
            );

            return res.status(200).json({
                success: true,
                message: 'Settings updated',
                data: settings
            });
        } catch (error) {
            if (error.code === 'SETTINGS_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Settings not found'
                });
            }

            console.error('[Settings Update Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to update settings',
                error: error.message
            });
        }
    },

    /**
     * Update notification preference for a specific channel
     * PUT /leads/:leadId/settings/notifications/:channel
     */
    async updateNotificationPreference(req, res) {
        try {
            const { channel } = req.params;
            const { enabled } = req.body;

            const validChannels = ['email', 'sms', 'push', 'in_app'];
            if (!validChannels.includes(channel)) {
                return res.status(400).json({
                    success: false,
                    message: `Invalid channel. Must be one of: ${validChannels.join(', ')}`
                });
            }

            if (typeof enabled !== 'boolean') {
                return res.status(400).json({
                    success: false,
                    message: 'Enabled must be boolean'
                });
            }

            const settings = await settingsService.updateNotificationPreference(
                req.params.leadId,
                channel,
                enabled
            );

            return res.status(200).json({
                success: true,
                message: `${channel} notifications ${enabled ? 'enabled' : 'disabled'}`,
                data: settings.notifications
            });
        } catch (error) {
            if (error.code === 'SETTINGS_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Settings not found'
                });
            }

            console.error('[Notification Preference Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to update notification preference',
                error: error.message
            });
        }
    },

    /**
     * Toggle 2FA
     * POST /leads/:leadId/settings/two-factor
     */
    async toggle2FA(req, res) {
        try {
            const { enabled } = req.body;

            if (typeof enabled !== 'boolean') {
                return res.status(400).json({
                    success: false,
                    message: 'Enabled must be boolean'
                });
            }

            const settings = await settingsService.toggle2FA(req.params.leadId, enabled);

            return res.status(200).json({
                success: true,
                message: `2FA ${enabled ? 'enabled' : 'disabled'}`,
                data: {
                    two_factor_enabled: settings.preferences.two_factor_enabled
                }
            });
        } catch (error) {
            if (error.code === 'SETTINGS_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Settings not found'
                });
            }

            console.error('[2FA Toggle Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to toggle 2FA',
                error: error.message
            });
        }
    },

    /**
     * Reset settings to defaults
     * POST /leads/:leadId/settings/reset
     */
    async resetToDefaults(req, res) {
        try {
            const settings = await settingsService.resetToDefaults(req.params.leadId);

            return res.status(200).json({
                success: true,
                message: 'Settings reset to defaults',
                data: settings
            });
        } catch (error) {
            if (error.code === 'SETTINGS_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Settings not found'
                });
            }

            console.error('[Reset Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to reset settings',
                error: error.message
            });
        }
    },

    /**
     * [ADMIN] Update admin-controlled settings
     * PUT /leads/:leadId/settings/admin
     */
    async updateAdminSettings(req, res) {
        try {
            const { isValid, errors } = validateAdminSettingsUpdate(req.body);

            if (!isValid) {
                return res.status(400).json({
                    success: false,
                    message: 'Validation error',
                    errors
                });
            }

            const settings = await settingsService.updateAdminSettings(
                req.params.leadId,
                req.body,
                req.user?.id // Admin user ID from auth
            );

            return res.status(200).json({
                success: true,
                message: 'Admin settings updated',
                data: settings
            });
        } catch (error) {
            if (error.code === 'SETTINGS_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Settings not found'
                });
            }

            console.error('[Admin Settings Update Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to update admin settings',
                error: error.message
            });
        }
    },

    /**
     * [ADMIN] Suspend account
     * POST /leads/:leadId/settings/suspend
     */
    async suspendAccount(req, res) {
        try {
            const settings = await settingsService.suspendAccount(
                req.params.leadId,
                req.body.reason
            );

            return res.status(200).json({
                success: true,
                message: 'Account suspended',
                data: {
                    account_status: settings.account_status
                }
            });
        } catch (error) {
            if (error.code === 'SETTINGS_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Settings not found'
                });
            }

            console.error('[Suspend Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to suspend account',
                error: error.message
            });
        }
    },

    /**
     * [ADMIN] Restore account
     * POST /leads/:leadId/settings/restore
     */
    async restoreAccount(req, res) {
        try {
            const settings = await settingsService.restoreAccount(req.params.leadId);

            return res.status(200).json({
                success: true,
                message: 'Account restored',
                data: {
                    account_status: settings.account_status
                }
            });
        } catch (error) {
            if (error.code === 'SETTINGS_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Settings not found'
                });
            }

            console.error('[Restore Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to restore account',
                error: error.message
            });
        }
    },

    /**
     * [ADMIN] Get settings by account status
     * GET /settings/by-status/:status
     */
    async getByStatus(req, res) {
        try {
            const { status } = req.params;
            const { page = 1, limit = 50 } = req.query;

            const validStatus = ['active', 'suspended', 'banned', 'pending_review'];
            if (!validStatus.includes(status)) {
                return res.status(400).json({
                    success: false,
                    message: `Invalid status. Must be one of: ${validStatus.join(', ')}`
                });
            }

            const result = await settingsService.getSettingsByAccountStatus(
                status,
                parseInt(page),
                parseInt(limit)
            );

            return res.status(200).json({
                success: true,
                data: result.data,
                pagination: result.pagination
            });
        } catch (error) {
            console.error('[Get By Status Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch settings',
                error: error.message
            });
        }
    },

    /**
     * [ADMIN] Get settings analytics
     * GET /settings/analytics
     */
    async getAnalytics(req, res) {
        try {
            const analytics = await settingsService.getSettingsAnalytics();

            return res.status(200).json({
                success: true,
                data: analytics
            });
        } catch (error) {
            console.error('[Analytics Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch analytics',
                error: error.message
            });
        }
    }
};

export default settingsController;
