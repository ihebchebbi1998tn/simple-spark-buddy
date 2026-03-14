/**
 * Settings Validators
 */

export const validateUserSettingsUpdate = (data) => {
    const errors = {};

    // User-editable fields only (admin-only fields excluded)
    if (data.notifications !== undefined) {
        if (typeof data.notifications !== 'object' || Array.isArray(data.notifications)) {
            errors.notifications = 'Notifications must be an object';
        } else {
            // Validate notification sub-fields
            if (data.notifications.email !== undefined && typeof data.notifications.email !== 'boolean') {
                errors.notifications_email = 'Email notifications must be boolean';
            }
            if (data.notifications.sms !== undefined && typeof data.notifications.sms !== 'boolean') {
                errors.notifications_sms = 'SMS notifications must be boolean';
            }
            if (data.notifications.push !== undefined && typeof data.notifications.push !== 'boolean') {
                errors.notifications_push = 'Push notifications must be boolean';
            }
            if (data.notifications.in_app !== undefined && typeof data.notifications.in_app !== 'boolean') {
                errors.notifications_in_app = 'In-app notifications must be boolean';
            }
        }
    }

    if (data.privacy !== undefined) {
        if (typeof data.privacy !== 'object' || Array.isArray(data.privacy)) {
            errors.privacy = 'Privacy settings must be an object';
        } else {
            if (data.privacy.profile_visibility !== undefined) {
                const validVisibility = ['public', 'private', 'recruiters_only'];
                if (!validVisibility.includes(data.privacy.profile_visibility)) {
                    errors.privacy_visibility = `Profile visibility must be one of: ${validVisibility.join(', ')}`;
                }
            }
            if (data.privacy.search_indexing !== undefined && typeof data.privacy.search_indexing !== 'boolean') {
                errors.privacy_search = 'Search indexing must be boolean';
            }
        }
    }

    if (data.language !== undefined) {
        const validLanguages = ['en', 'fr', 'es', 'de', 'it', 'pt', 'nl', 'ja', 'zh'];
        if (!validLanguages.includes(data.language)) {
            errors.language = `Language must be one of: ${validLanguages.join(', ')}`;
        }
    }

    if (data.theme !== undefined) {
        const validThemes = ['light', 'dark', 'auto'];
        if (!validThemes.includes(data.theme)) {
            errors.theme = `Theme must be one of: ${validThemes.join(', ')}`;
        }
    }

    if (data.preferences !== undefined) {
        if (typeof data.preferences !== 'object' || Array.isArray(data.preferences)) {
            errors.preferences = 'Preferences must be an object';
        } else {
            if (data.preferences.marketing_emails !== undefined && typeof data.preferences.marketing_emails !== 'boolean') {
                errors.preferences_marketing = 'Marketing emails must be boolean';
            }
            if (data.preferences.newsletter !== undefined && typeof data.preferences.newsletter !== 'boolean') {
                errors.preferences_newsletter = 'Newsletter subscription must be boolean';
            }
            if (data.preferences.two_factor_enabled !== undefined && typeof data.preferences.two_factor_enabled !== 'boolean') {
                errors.preferences_2fa = '2FA enabled must be boolean';
            }
        }
    }

    return { isValid: Object.keys(errors).length === 0, errors };
};

export const validateAdminSettingsUpdate = (data) => {
    const errors = {};

    // All fields allowed for admins
    if (data.email_verified !== undefined && typeof data.email_verified !== 'boolean') {
        errors.email_verified = 'Email verified must be boolean';
    }

    if (data.phone_verified !== undefined && typeof data.phone_verified !== 'boolean') {
        errors.phone_verified = 'Phone verified must be boolean';
    }

    if (data.account_status !== undefined) {
        const validStatus = ['active', 'suspended', 'banned', 'pending_review'];
        if (!validStatus.includes(data.account_status)) {
            errors.account_status = `Account status must be one of: ${validStatus.join(', ')}`;
        }
    }

    if (data.subscription_status !== undefined) {
        const validStatus = ['free', 'premium', 'enterprise'];
        if (!validStatus.includes(data.subscription_status)) {
            errors.subscription_status = `Subscription status must be one of: ${validStatus.join(', ')}`;
        }
    }

    if (data.account_flags !== undefined) {
        if (!Array.isArray(data.account_flags)) {
            errors.account_flags = 'Account flags must be an array';
        } else {
            const validFlags = ['verified', 'vip', 'flagged', 'restricted', 'legacy'];
            const invalidFlags = data.account_flags.filter(f => !validFlags.includes(f));
            if (invalidFlags.length > 0) {
                errors.account_flags = `Invalid flags: ${invalidFlags.join(', ')}`;
            }
        }
    }

    // Also allow user-editable fields
    const userValidation = validateUserSettingsUpdate(data);
    if (!userValidation.isValid) {
        Object.assign(errors, userValidation.errors);
    }

    return { isValid: Object.keys(errors).length === 0, errors };
};

export const validateSettingsQuery = (query) => {
    const errors = {};
    let page = 1, limit = 50;

    if (query.page) {
        page = parseInt(query.page);
        if (isNaN(page) || page < 1) errors.page = 'Page must be positive';
        else page = Math.min(page, 1000);
    }

    if (query.limit) {
        limit = parseInt(query.limit);
        if (isNaN(limit) || limit < 1 || limit > 200) errors.limit = 'Limit must be 1-200';
    }

    return { isValid: Object.keys(errors).length === 0, errors, page, limit };
};
