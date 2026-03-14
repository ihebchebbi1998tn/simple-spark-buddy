import mongoose from 'mongoose';

const activityLogSchema = new mongoose.Schema(
    {
        /**
         * ============================================
         * CORE FIELDS
         * ============================================
         */
        lead_id: {
            type: mongoose.Schema.Types.ObjectId,
            ref: 'Lead',
            sparse: true, // nullable for anonymous users
            index: true,
            description: 'Foreign key to LEADS - who performed the action (null if anonymous)'
        },

        is_authenticated: {
            type: Boolean,
            required: true,
            default: false,
            description: 'Whether user was logged in when action occurred'
        },

        /**
         * ============================================
         * ACTION DETAILS
         * ============================================
         */
        action_type: {
            type: Number,
            required: true,
            enum: [
                // Auth (1-11)
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
                // Profile (12-16)
                12, 13, 14, 15, 16,
                // Skills (17-23)
                17, 18, 19, 20, 21, 22, 23,
                // Settings (24-28)
                24, 25, 26, 27, 28,
                // Job/Matching (29-36)
                29, 30, 31, 32, 33, 34, 35, 36,
                // Communication (37-40)
                37, 38, 39, 40,
                // Platform (41-47)
                41, 42, 43, 44, 45, 46, 47,
                // Errors (48-50)
                48, 49, 50,
                // Admin (51-53)
                51, 52, 53
            ],
            index: true,
            description: 'Action type (1-53)`'
        },

        action_details: {
            type: mongoose.Schema.Types.Mixed,
            description: 'Contextual metadata for this action'
        },

        /**
         * ============================================
         * SESSION TRACKING
         * ============================================
         */
        session_id: {
            type: String,
            required: true,
            index: true,
            description: 'Session identifier for grouping actions'
        },

        /**
         * ============================================
         * SECURITY & CONTEXT
         * ============================================
         */
        ip_address: {
            type: String,
            maxlength: 45,
            required: true,
            description: 'User IP address (for security, removed after 30 days)'
        },

        ip_hash: {
            type: String,
            maxlength: 256,
            description: 'SHA256 hash of IP (for GDPR compliance)'
        },

        user_agent: {
            type: String,
            maxlength: 500,
            required: true,
            description: 'Browser/device user agent string'
        },

        device_type: {
            type: Number,
            required: true,
            enum: [1, 2, 3, 4],
            description: 'Device: 1=Desktop, 2=Mobile, 3=Tablet, 4=Unknown'
        },

        /**
         * ============================================
         * PERFORMANCE
         * ============================================
         */
        response_time_ms: {
            type: Number,
            min: 0,
            description: 'Action execution time in milliseconds'
        },

        /**
         * ============================================
         * SOURCE
         * ============================================
         */
        source: {
            type: String,
            required: true,
            enum: ['web', 'ios_app', 'android_app', 'api', 'admin_panel', 'system', 'webhook'],
            description: 'Where action originated'
        },

        /**
         * ============================================
         * TIMESTAMP
         * ============================================
         */
        timestamp: {
            type: Date,
            required: true,
            default: Date.now
        }
    },
    {
        collection: 'activity_logs',
        timestamps: false,
        strict: true
    }
);

/**
 * ============================================
 * INDEXES
 * ============================================
 */
activityLogSchema.index({ lead_id: 1, timestamp: -1 });
activityLogSchema.index({ action_type: 1, timestamp: -1 });
activityLogSchema.index({ session_id: 1, timestamp: 1 });
activityLogSchema.index({ ip_address: 1, timestamp: -1, action_type: 1 });
activityLogSchema.index({ response_time_ms: 1, timestamp: -1 });
activityLogSchema.index({ source: 1, timestamp: -1 });
activityLogSchema.index({ device_type: 1, timestamp: -1 });
activityLogSchema.index({ lead_id: 1, action_type: 1, timestamp: -1 });
// NOTE: timestamp already has index from field definition
// TTL: Delete after 180 days
activityLogSchema.index({ timestamp: 1 }, { expireAfterSeconds: 15552000 });

export default mongoose.model('LeadActivityLog', activityLogSchema);
