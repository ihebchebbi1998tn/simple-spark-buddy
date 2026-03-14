/**
 * Notification Validators
 */

export const validateNotificationCreation = (data) => {
    const errors = {};

    if (!data.lead_id) errors.lead_id = 'Lead ID is required';
    if (!data.notification_type || ![1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36].includes(data.notification_type)) {
        errors.notification_type = 'Valid notification type (1-36) is required';
    }
    if (!data.channel || ![1, 2, 3, 4, 5].includes(data.channel)) {
        errors.channel = 'Valid channel (1=Email, 2=SMS, 3=Push, 4=In-app, 5=WhatsApp) is required';
    }
    if (!data.priority || ![1, 2, 3, 4].includes(data.priority)) {
        errors.priority = 'Valid priority (1=Low, 2=Normal, 3=High, 4=Urgent) is required';
    }
    if (!data.title || data.title.trim().length === 0) {
        errors.title = 'Title is required';
    } else if (data.title.length > 255) {
        errors.title = 'Title cannot exceed 255 characters';
    }
    if (!data.message || data.message.trim().length === 0) {
        errors.message = 'Message is required';
    } else if (data.message.length > 10000) {
        errors.message = 'Message cannot exceed 10000 characters';
    }
    if (!data.provider || !['sendgrid', 'mailgun', 'ses', 'twilio', 'vonage', 'firebase', 'apns', 'onesignal', 'internal'].includes(data.provider)) {
        errors.provider = 'Valid provider is required';
    }

    // Conditional validations
    if (data.channel === 1 && !data.email_address) {
        errors.email_address = 'Email address required for email channel';
    }
    if (data.channel === 2 && !data.phone_number) {
        errors.phone_number = 'Phone number required for SMS channel';
    }
    if (data.channel === 3 && !data.device_token) {
        errors.device_token = 'Device token required for push channel';
    }

    return { isValid: Object.keys(errors).length === 0, errors };
};

export const validateNotificationQuery = (query) => {
    const errors = {};
    let page = 1, limit = 20;

    if (query.page) {
        page = parseInt(query.page);
        if (isNaN(page) || page < 1) errors.page = 'Page must be positive';
        else page = Math.min(page, 1000);
    }

    if (query.limit) {
        limit = parseInt(query.limit);
        if (isNaN(limit) || limit < 1 || limit > 100) errors.limit = 'Limit must be 1-100';
    }

    return { isValid: Object.keys(errors).length === 0, errors, page, limit };
};
