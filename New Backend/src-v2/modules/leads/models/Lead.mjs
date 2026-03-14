/**
 * @file Lead.mjs
 * @description MongoDB schema for LEADS table
 * @version 2.0
 */

import mongoose from 'mongoose';

const leadSchema = new mongoose.Schema(
    {
        // 🆔 IDENTITY SECTION (9 fields)
        public_id: {
            type: String,
            unique: true,
            required: true,
            description: 'Human-readable reference ID (LD-2026-00001)'
        },
        email: {
            type: String,
            unique: true,
            required: true,
            lowercase: true,
            trim: true,
            description: 'User email address'
        },
        email_normalized: {
            type: String,
            required: true,
            lowercase: true,
            description: 'Normalized email for case-insensitive search'
        },
        password_hash: {
            type: String,
            required: true,
            description: 'Encrypted password (bcrypt)'
        },
        first_name: {
            type: String,
            required: true,
            trim: true,
            description: 'First name'
        },
        last_name: {
            type: String,
            required: true,
            trim: true,
            description: 'Last name'
        },
        phone: {
            type: String,
            required: true,
            description: 'Phone number in international format'
        },
        date_of_birth: {
            type: Date,
            required: true,
            description: 'Birth date'
        },
        gender: {
            type: Number,
            enum: [0, 1, 2], // 0=Male, 1=Female, 2=Other
            description: 'Gender (0=Male, 1=Female, 2=Other)'
        },

        // 📍 LOCATION SECTION (3 fields)
        location: {
            country_code: {
                type: String,
                description: 'ISO country code (FR, ES, etc.)'
            },
            city_id: {
                type: Number,
                description: 'Reference to cities (handled by frontend)'
            },
            postal_code: {
                type: String,
                description: 'ZIP or postal code'
            }
        },

        // ⚙️ ACCOUNT STATUS SECTION (3 fields)
        account_status: {
            type: Number,
            enum: [1, 2, 3, 4], // 1=Actif, 2=Inactif, 3=Suspendu, 4=En attente
            default: 4,
            description: 'Current account state'
        },
        lead_stage: {
            type: Number,
            enum: [1, 2, 3, 4, 5], // 1=Just registered, 2=Profile completed, etc.
            default: 1,
            description: 'Lifecycle stage'
        },
        profile_completion: {
            type: Number,
            min: 0,
            max: 100,
            default: 0,
            description: 'Profile completion percentage'
        },

        // ✅ VERIFICATION SECTION (5 fields)
        verification: {
            email_verified: {
                type: Boolean,
                default: false,
                description: 'Email confirmed'
            },
            email_verified_at: {
                type: Date,
                default: null,
                description: 'Email verification timestamp'
            },
            phone_verified: {
                type: Boolean,
                default: false,
                description: 'Phone confirmed'
            },
            phone_verified_at: {
                type: Date,
                default: null,
                description: 'Phone verification timestamp'
            },
            identity_verified: {
                type: Boolean,
                default: false,
                description: 'ID verified by admin'
            }
        },

        // 🔒 SECURITY SECTION (7 fields)
        security: {
            two_factor_enabled: {
                type: Boolean,
                default: false,
                description: 'Is 2FA active?'
            },
            two_factor_method: {
                type: String,
                enum: ['sms', 'email', 'authenticator'],
                default: null,
                description: '2FA delivery method'
            },
            failed_login_attempts: {
                type: Number,
                default: 0,
                min: 0,
                max: 5,
                description: 'Failed login counter'
            },
            last_failed_login: {
                type: Date,
                default: null,
                description: 'Last failed login attempt'
            },
            lock_until: {
                type: Date,
                default: null,
                description: 'Account unlock timestamp'
            },
            account_locked: {
                type: Boolean,
                default: false,
                description: 'Is account locked?'
            },
            password_updated_at: {
                type: Date,
                default: null,
                description: 'Last password change timestamp'
            }
        },

        // 📊 SOURCE SECTION (1 field)
        channel: {
            type: Number,
            enum: [1, 2, 3, 4], // 1=facebook_ads, 2=organic, 3=referral, 4=api
            default: 2,
            description: 'Registration source channel (1=facebook_ads, 2=organic, 3=referral, 4=api)'
        },

        // 🎯 SUMMARY METRICS SECTION (20+ fields)
        summary_metrics: {
            // Position & Experience
            desired_position: {
                type: Number,
                required: false,
                description: 'Target job position code'
            },
            call_center_experience: {
                type: Number,
                required: false,
                description: 'Experience level in call centers (1-7)'
            },
            desired_position_experience: {
                type: Number,
                required: false,
                description: 'Experience in desired position (1-7)'
            },

            // Activities Array (1-3 activities with nested operations)
            activities: {
                type: [
                    {
                        activity_type: {
                            type: Number,
                            required: true,
                            description: 'Activity code (1=Télévente, 2=Téléprospection, etc.)'
                        },
                        activity_experience: {
                            type: Number,
                            required: true,
                            description: 'Experience level (1-7)'
                        },
                        operations: {
                            type: [
                                {
                                    operation_type: {
                                        type: Number,
                                        required: true,
                                        description: 'Operation code'
                                    },
                                    operation_experience: {
                                        type: Number,
                                        required: true,
                                        description: 'Experience level (1-7)'
                                    }
                                }
                            ],
                            default: [],
                            maxLength: 3,
                            description: 'Operations (max 3)'
                        }
                    }
                ],
                default: [],
                maxLength: 3,
                description: 'Activities (1-3, priority order)'
            },

            // Languages (primary, secondary, tertiary)
            primary_language_code: {
                type: String,
                default: null,
                description: 'ISO language code (FR, EN, etc.)'
            },
            primary_language_level: {
                type: Number,
                min: 1,
                max: 7, // A1-C2 + Native
                default: null,
                description: 'CEFR level (1-7)'
            },
            secondary_language_code: {
                type: String,
                default: null,
                description: 'ISO language code'
            },
            secondary_language_level: {
                type: Number,
                min: 1,
                max: 7,
                default: null,
                description: 'CEFR level'
            },
            tertiary_language_code: {
                type: String,
                default: null,
                description: 'ISO language code'
            },
            tertiary_language_level: {
                type: Number,
                min: 1,
                max: 7,
                default: null,
                description: 'CEFR level'
            },

            // Work Preferences
            work_mode: {
                type: Number,
                enum: [1, 2, 3], // 1=Remote, 2=On-site, 3=Hybrid
                default: null,
                description: 'Work location preference'
            },
            work_time: {
                type: Number,
                enum: [1, 2, 3, 4, 5],
                default: null,
                description: 'Schedule type'
            },
            work_shift: {
                type: Number,
                enum: [1, 2, 3],
                default: null,
                description: 'Shift preference'
            },

            // Availability
            availability: {
                type: Number,
                enum: [1, 2, 3, 4, 5, 6], // 1=Immediately, 2=1 week, etc.
                default: null,
                description: 'Availability timeframe'
            },

            // Counters
            skills_count: {
                type: Number,
                default: 0,
                description: 'Total qualifications count'
            },
            languages_count: {
                type: Number,
                min: 0,
                max: 3,
                default: 0,
                description: 'Number of languages (1-3)'
            },

            // Sync tracking
            last_sync_at: {
                type: Date,
                default: null,
                description: 'Last auto-sync from LEAD_PROFILES'
            }
        },

        // ⏰ TIMESTAMPS (5 fields)
        last_login: {
            type: Date,
            default: null,
            description: 'Last successful login'
        },
        last_activity_at: {
            type: Date,
            default: null,
            description: 'Last activity timestamp (login, profile view, update, test, etc.)'
        },
        created_at: {
            type: Date,
            default: Date.now,
            description: 'Record creation timestamp'
        },
        updated_at: {
            type: Date,
            default: Date.now,
            description: 'Last update timestamp'
        }
    },
    {
        timestamps: true,
        collection: 'leads',
        strict: true
    }
);

// Indexes for performance
// NOTE: email, email_normalized, and public_id already have unique indexes from field definitions
leadSchema.index({ account_status: 1 });
leadSchema.index({ lead_stage: 1 });
leadSchema.index({ 'location.country_code': 1 });
leadSchema.index({ 'location.city_id': 1 });
leadSchema.index({ channel: 1 });
leadSchema.index({ created_at: -1 });
leadSchema.index({ updated_at: -1 });
leadSchema.index({ last_login: -1 });
leadSchema.index({ last_activity_at: -1 });

// Pre-save middleware to update updated_at and last_activity_at
leadSchema.pre('save', function (next) {
    this.updated_at = new Date();
    this.last_activity_at = new Date();
    next();
});

// Create and export model
const Lead = mongoose.model('Lead', leadSchema);

export default Lead;
