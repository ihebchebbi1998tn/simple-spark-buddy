/**
 * Activity Log Validators
 */

const isValidActionType = (type) => type >= 1 && type <= 53 && Number.isInteger(type);

const isValidIPAddress = (ip) => {
    const ipv4Regex = /^(\d{1,3}\.){3}\d{1,3}$/;
    if (ipv4Regex.test(ip)) {
        return ip.split('.').every(part => parseInt(part) <= 255);
    }
    return /^([0-9a-fA-F]{0,4}:){2,7}[0-9a-fA-F]{0,4}$/.test(ip);
};

export const validateActivityLogCreation = (data) => {
    const errors = {};

    if (data.is_authenticated === undefined) {
        errors.is_authenticated = 'Authentication status is required';
    } else if (typeof data.is_authenticated !== 'boolean') {
        errors.is_authenticated = 'Must be boolean';
    }

    if (!data.action_type || !isValidActionType(data.action_type)) {
        errors.action_type = 'Valid action type (1-53) is required';
    }

    if (!data.session_id || data.session_id.length < 10) {
        errors.session_id = 'Valid session ID is required (min 10 chars)';
    }

    if (!data.ip_address || !isValidIPAddress(data.ip_address)) {
        errors.ip_address = 'Valid IP address is required';
    }

    if (!data.user_agent || data.user_agent.length < 5) {
        errors.user_agent = 'Valid user agent is required';
    }

    if (!data.device_type || ![1, 2, 3, 4].includes(data.device_type)) {
        errors.device_type = 'Valid device type (1-4) is required';
    }

    if (!data.source || !['web', 'ios_app', 'android_app', 'api', 'admin_panel', 'system', 'webhook'].includes(data.source)) {
        errors.source = 'Valid source is required';
    }

    if (data.response_time_ms !== undefined && (typeof data.response_time_ms !== 'number' || data.response_time_ms < 0)) {
        errors.response_time_ms = 'Must be a positive number';
    }

    return { isValid: Object.keys(errors).length === 0, errors };
};

export const validateActivityLogQuery = (query) => {
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

export const getActionTypeLabel = (type) => {
    const labels = {
        1: 'registration_started', 2: 'registration_completed', 3: 'email_verified',
        4: 'phone_verified', 5: 'login_success', 6: 'login_failed', 7: 'logout',
        8: 'password_reset_requested', 9: 'password_reset_completed',
        10: 'two_factor_enabled', 11: 'two_factor_disabled',
        12: 'profile_viewed', 13: 'profile_updated', 14: 'profile_section_updated',
        15: 'profile_photo_uploaded', 16: 'profile_photo_deleted',
        17: 'skill_added', 18: 'skill_removed', 19: 'language_added',
        20: 'test_started', 21: 'test_completed', 22: 'test_passed', 23: 'test_failed',
        24: 'settings_viewed', 25: 'notification_preferences_updated',
        26: 'privacy_settings_updated', 27: 'language_changed', 28: 'theme_changed',
        29: 'job_search', 30: 'job_viewed', 31: 'job_applied',
        32: 'job_saved', 33: 'job_unsaved', 34: 'match_viewed',
        35: 'match_accepted', 36: 'match_declined',
        37: 'message_sent', 38: 'message_received', 39: 'message_read',
        40: 'notification_clicked', 41: 'page_view', 42: 'dashboard_viewed',
        43: 'help_accessed', 44: 'contact_support', 45: 'ai_bot_conversation',
        46: 'document_downloaded', 47: 'document_uploaded',
        48: 'error_occurred', 49: 'api_error', 50: 'validation_error',
        51: 'account_locked_by_admin', 52: 'account_unlocked_by_admin',
        53: 'profile_edited_by_admin'
    };
    return labels[type] || 'unknown';
};
