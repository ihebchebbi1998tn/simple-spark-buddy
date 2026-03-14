import mongoose from 'mongoose';

const notificationSchema = new mongoose.Schema(
    {
        /**
         * ============================================
         * CORE FIELDS
         * ============================================
         */
        lead_id: {
            type: mongoose.Schema.Types.ObjectId,
            ref: 'Lead',
            required: true,
            description: 'Foreign key to LEADS - who this notification is for'
        },

        /**
         * ============================================
         * NOTIFICATION DETAILS
         * ============================================
         */
        notification_type: {
            type: Number,
            required: true,
            enum: [
                1, 2, 3, 4, 5, 6, 7, 8,   // Auth & Account
                9, 10, 11, 12, 13, 14, 15, // Profile & Tests
                16, 17, 18, 19, 20, 21, 22, // Job Matching
                23, 24, 25,                  // Communication
                26, 27, 28, 29, 30,          // Marketing
                31, 32, 33, 34, 35, 36       // Administrative & Custom
            ],
            description: 'Notification type (1-36): Welcome, Email Verification, Password Reset, Job Alert, etc.'
        },

        channel: {
            type: Number,
            required: true,
            enum: [1, 2, 3, 4, 5],
            description: 'Channel: 1=Email, 2=SMS, 3=Push, 4=In-app, 5=WhatsApp'
        },

        priority: {
            type: Number,
            required: true,
            enum: [1, 2, 3, 4],
            description: 'Priority: 1=Low, 2=Normal, 3=High, 4=Urgent'
        },

        /**
         * ============================================
         * CONTENT
         * ============================================
         */
        title: {
            type: String,
            required: true,
            maxlength: 255,
            description: 'Notification title/subject'
        },

        message: {
            type: String,
            required: true,
            maxlength: 10000,
            description: 'Main notification content/body'
        },

        action_url: {
            type: String,
            maxlength: 2048,
            description: 'Optional clickable action URL'
        },

        action_button_text: {
            type: String,
            maxlength: 100,
            description: 'Action button label'
        },

        /**
         * ============================================
         * DELIVERY STATUS
         * ============================================
         */
        status: {
            type: Number,
            required: true,
            enum: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
            default: 1,
            index: true,
            description: 'Status: 1=Pending, 2=Sent, 3=Delivered, 4=Read, 5=Clicked, 6=Failed, 7=Bounced, 8=Spam, 9=Expired, 10=Cancelled'
        },

        sent_at: {
            type: Date,
            description: 'When sent to provider'
        },

        delivered_at: {
            type: Date,
            description: 'When delivered to recipient'
        },

        read_at: {
            type: Date,
            description: 'When user opened/read notification'
        },

        clicked_at: {
            type: Date,
            description: 'When user clicked action link'
        },

        /**
         * ============================================
         * CHANNEL-SPECIFIC FIELDS
         * ============================================
         */
        email_address: {
            type: String,
            maxlength: 255,
            description: 'Recipient email (stored at time of send for audit trail)'
        },

        phone_number: {
            type: String,
            maxlength: 20,
            description: 'Recipient phone (stored at time of send for audit trail)'
        },

        device_token: {
            type: String,
            maxlength: 1000,
            description: 'Push device token (Firebase, APNS, etc.)'
        },

        /**
         * ============================================
         * PROVIDER & DELIVERY TRACKING
         * ============================================
         */
        provider: {
            type: String,
            required: true,
            enum: ['sendgrid', 'mailgun', 'ses', 'twilio', 'vonage', 'firebase', 'apns', 'onesignal', 'internal'],
            description: 'Delivery provider: sendgrid, twilio, firebase, internal, etc.'
        },

        provider_message_id: {
            type: String,
            maxlength: 500,
            description: 'Provider tracking ID for this notification'
        },

        provider_response: {
            type: mongoose.Schema.Types.Mixed,
            description: 'Full provider API response for debugging'
        },

        /**
         * ============================================
         * RETRY LOGIC
         * ============================================
         */
        retry_count: {
            type: Number,
            required: true,
            default: 0,
            min: 0,
            max: 3,
            description: 'Number of delivery retries (max 3)'
        },

        last_retry_at: {
            type: Date,
            description: 'Timestamp of last retry attempt'
        },

        /**
         * ============================================
         * SCHEDULING
         * ============================================
         */
        scheduled_for: {
            type: Date,
            description: 'For scheduled sending (future date)'
        },

        expires_at: {
            type: Date,
            description: 'Expiration time (e.g., for verification codes)'
        },

        /**
         * ============================================
         * METADATA (Flexible)
         * ============================================
         */
        metadata: {
            type: mongoose.Schema.Types.Mixed,
            description: 'Flexible metadata: job_id, test_id, interview_id, etc.'
        },

        /**
         * ============================================
         * TIMESTAMPS
         * ============================================
         */
        created_at: {
            type: Date,
            required: true,
            default: Date.now
        },

        updated_at: {
            type: Date,
            required: true,
            default: Date.now
        }
    },
    {
        collection: 'notifications',
        timestamps: false,
        strict: true
    }
);

/**
 * ============================================
 * INDEXES
 * ============================================
 */
notificationSchema.index({ lead_id: 1, created_at: -1 });
notificationSchema.index({ lead_id: 1, status: 1 });
notificationSchema.index({ status: 1, retry_count: 1 });
notificationSchema.index({ scheduled_for: 1, status: 1 });
notificationSchema.index({ expires_at: 1 });
notificationSchema.index({ notification_type: 1, created_at: -1 });
notificationSchema.index({ channel: 1, created_at: -1 });
notificationSchema.index({ provider_message_id: 1 });
// TTL: Delete after 180 days
notificationSchema.index({ created_at: 1 }, { expireAfterSeconds: 15552000 });

/**
 * ============================================
 * PRE-SAVE MIDDLEWARE
 * ============================================
 */
notificationSchema.pre('save', function (next) {
    if (!this.isNew) {
        this.updated_at = new Date();
    }
    next();
});

export default mongoose.model('LeadNotification', notificationSchema);
