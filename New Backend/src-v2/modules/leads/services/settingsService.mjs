/**
 * Settings Service
 * Handles both user-editable and admin-controlled settings
 */

import LeadSettings from '../models/LeadSettings.mjs';

export const settingsService = {
    /**
     * Get settings for a lead
     */
    async getSettingsByLeadId(leadId) {
        const settings = await LeadSettings.findOne({ lead_id: leadId })
            .populate('lead_id', 'email name status');

        if (!settings) {
            const error = new Error('Settings not found');
            error.code = 'SETTINGS_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }
        return settings;
    },

    /**
     * Get user-visible settings (exclude some admin-only fields for display)
     */
    async getUserSettings(leadId) {
        const settings = await LeadSettings.findOne({ lead_id: leadId })
            .select('-internal_notes -last_admin_review');

        if (!settings) {
            const error = new Error('Settings not found');
            error.code = 'SETTINGS_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }
        return settings;
    },

    /**
     * Get field accessibility info (what user can edit vs admin-only)
     */
    async getFieldAccessibility(leadId) {
        const settings = await LeadSettings.findOne({ lead_id: leadId });

        if (!settings) {
            const error = new Error('Settings not found');
            error.code = 'SETTINGS_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return {
            user_editable: {
                notifications: {
                    email: settings.notifications?.email,
                    sms: settings.notifications?.sms,
                    push: settings.notifications?.push,
                    in_app: settings.notifications?.in_app
                },
                privacy: {
                    profile_visibility: settings.privacy?.profile_visibility,
                    search_indexing: settings.privacy?.search_indexing
                },
                language: settings.language,
                theme: settings.theme,
                preferences: {
                    marketing_emails: settings.preferences?.marketing_emails,
                    newsletter: settings.preferences?.newsletter,
                    two_factor_enabled: settings.preferences?.two_factor_enabled
                }
            },
            admin_managed: {
                email_verified: settings.email_verified,
                phone_verified: settings.phone_verified,
                account_status: settings.account_status,
                subscription_status: settings.subscription_status,
                account_flags: settings.account_flags,
                last_admin_review: settings.last_admin_review
            }
        };
    },

    /**
     * Update user-editable settings
     */
    async updateUserSettings(leadId, updateData) {
        // Only allow specific user-editable fields
        const userEditableFields = ['notifications', 'privacy', 'language', 'theme', 'preferences'];
        const userUpdates = {};

        for (const field of userEditableFields) {
            if (field in updateData) {
                userUpdates[field] = updateData[field];
            }
        }

        userUpdates.updated_at = new Date();

        const settings = await LeadSettings.findOneAndUpdate(
            { lead_id: leadId },
            { $set: userUpdates },
            { new: true, runValidators: true }
        );

        if (!settings) {
            const error = new Error('Settings not found');
            error.code = 'SETTINGS_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return settings;
    },

    /**
     * Update admin-controlled settings
     */
    async updateAdminSettings(leadId, updateData, adminId) {
        const allUpdates = { ...updateData };
        allUpdates.updated_at = new Date();
        allUpdates.last_admin_review = new Date();
        allUpdates.admin_notes = updateData.admin_notes || '';

        const settings = await LeadSettings.findOneAndUpdate(
            { lead_id: leadId },
            { $set: allUpdates },
            { new: true, runValidators: true }
        );

        if (!settings) {
            const error = new Error('Settings not found');
            error.code = 'SETTINGS_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return settings;
    },

    /**
     * Update specific notification preference
     */
    async updateNotificationPreference(leadId, channel, enabled) {
        const updateField = `notifications.${channel}`;

        const settings = await LeadSettings.findOneAndUpdate(
            { lead_id: leadId },
            {
                $set: {
                    [updateField]: enabled,
                    updated_at: new Date()
                }
            },
            { new: true }
        );

        if (!settings) {
            const error = new Error('Settings not found');
            error.code = 'SETTINGS_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return settings;
    },

    /**
     * Enable/disable 2FA
     */
    async toggle2FA(leadId, enabled) {
        const settings = await LeadSettings.findOneAndUpdate(
            { lead_id: leadId },
            {
                $set: {
                    'preferences.two_factor_enabled': enabled,
                    updated_at: new Date()
                }
            },
            { new: true }
        );

        if (!settings) {
            const error = new Error('Settings not found');
            error.code = 'SETTINGS_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return settings;
    },

    /**
     * Reset settings to defaults
     */
    async resetToDefaults(leadId) {
        const defaults = {
            notifications: {
                email: true,
                sms: false,
                push: true,
                in_app: true
            },
            privacy: {
                profile_visibility: 'recruiters_only',
                search_indexing: true
            },
            language: 'en',
            theme: 'light',
            preferences: {
                marketing_emails: false,
                newsletter: false,
                two_factor_enabled: false
            },
            updated_at: new Date()
        };

        const settings = await LeadSettings.findOneAndUpdate(
            { lead_id: leadId },
            { $set: defaults },
            { new: true }
        );

        if (!settings) {
            const error = new Error('Settings not found');
            error.code = 'SETTINGS_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return settings;
    },

    /**
     * Suspend account (admin)
     */
    async suspendAccount(leadId, reason = null) {
        const settings = await LeadSettings.findOneAndUpdate(
            { lead_id: leadId },
            {
                $set: {
                    account_status: 'suspended',
                    updated_at: new Date(),
                    last_admin_review: new Date(),
                    admin_notes: reason || 'Account suspended'
                }
            },
            { new: true }
        );

        if (!settings) {
            const error = new Error('Settings not found');
            error.code = 'SETTINGS_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return settings;
    },

    /**
     * Restore account (admin)
     */
    async restoreAccount(leadId) {
        const settings = await LeadSettings.findOneAndUpdate(
            { lead_id: leadId },
            {
                $set: {
                    account_status: 'active',
                    updated_at: new Date(),
                    last_admin_review: new Date()
                }
            },
            { new: true }
        );

        if (!settings) {
            const error = new Error('Settings not found');
            error.code = 'SETTINGS_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return settings;
    },

    /**
     * Get all settings for a given status (admin query)
     */
    async getSettingsByAccountStatus(status, page = 1, limit = 50) {
        const skip = (page - 1) * limit;

        const [settings, total] = await Promise.all([
            LeadSettings.find({ account_status: status })
                .populate('lead_id', 'email name')
                .sort({ updated_at: -1 })
                .skip(skip)
                .limit(limit)
                .lean(),
            LeadSettings.countDocuments({ account_status: status })
        ]);

        return {
            data: settings,
            pagination: {
                page,
                limit,
                total,
                pages: Math.ceil(total / limit)
            }
        };
    },

    /**
     * Get settings analytics
     */
    async getSettingsAnalytics() {
        const pipeline = [
            {
                $group: {
                    _id: null,
                    total_leads: { $sum: 1 },
                    active_accounts: {
                        $sum: { $cond: [{ $eq: ['$account_status', 'active'] }, 1, 0] }
                    },
                    suspended_accounts: {
                        $sum: { $cond: [{ $eq: ['$account_status', 'suspended'] }, 1, 0] }
                    },
                    email_notifications_enabled: {
                        $sum: { $cond: ['$notifications.email', 1, 0] }
                    },
                    push_notifications_enabled: {
                        $sum: { $cond: ['$notifications.push', 1, 0] }
                    },
                    two_fa_enabled: {
                        $sum: { $cond: ['$preferences.two_factor_enabled', 1, 0] }
                    },
                    languages_used: { $addToSet: '$language' },
                    themes_used: { $addToSet: '$theme' }
                }
            }
        ];

        const result = await LeadSettings.aggregate(pipeline);

        return result[0] || {
            total_leads: 0,
            active_accounts: 0
        };
    }
};

export default settingsService;
