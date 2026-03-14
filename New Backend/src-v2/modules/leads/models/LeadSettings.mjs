/**
 * @file LeadSettings.mjs
 * @description MongoDB schema for LEAD_SETTINGS table
 * @version 2.0
 */

import mongoose from 'mongoose';

const leadSettingsSchema = new mongoose.Schema(
    {
        // 🆔 REFERENCE SECTION
        lead_id: {
            type: mongoose.Schema.Types.ObjectId,
            ref: 'Lead',
            required: true,
            unique: true,
            index: true,
            description: 'Foreign key to LEADS table'
        },

        // 📋 ACCOUNT STATE AUDIT SECTION
        account_state_audit: {
            account_confirmed: {
                type: Boolean,
                default: false,
                description: 'Account validated by user'
            },
            account_confirmed_at: {
                type: Date,
                default: null,
                description: 'Account confirmation timestamp'
            },
            email_verified_at: {
                type: Date,
                default: null,
                description: 'Email verification timestamp'
            },
            phone_verified_at: {
                type: Date,
                default: null,
                description: 'Phone verification timestamp'
            },
            account_enabled: {
                type: Boolean,
                default: true,
                description: 'Is login allowed?'
            },
            account_enabled_at: {
                type: Date,
                default: null,
                description: 'Account enable timestamp'
            },
            account_disabled: {
                type: Boolean,
                default: false,
                description: 'Is login blocked?'
            },
            account_disabled_at: {
                type: Date,
                default: null,
                description: 'Account disable timestamp'
            },
            account_disabled_by: {
                type: String,
                enum: ['admin', 'system', null],
                default: null,
                description: 'Who disabled the account'
            },
            account_deleted: {
                type: Boolean,
                default: false,
                description: 'Is account deleted?'
            },
            account_deleted_at: {
                type: Date,
                default: null,
                description: 'Account deletion timestamp'
            },
            account_deleted_by: {
                type: String,
                enum: ['admin', 'system', null],
                default: null,
                description: 'Who deleted the account'
            },
            account_locked_at: {
                type: Date,
                default: null,
                description: 'Account lock timestamp'
            },
            account_locked_by: {
                type: String,
                enum: ['admin', 'system', null],
                default: null,
                description: 'Who locked the account'
            },
            two_factor_enabled_at: {
                type: Date,
                default: null,
                description: '2FA enable timestamp'
            }
        },

        // 🔒 SECURITY EXTENDED SECTION
        security_extended: {
            ip_address: {
                type: String,
                default: null,
                description: 'Last known IP address'
            },
            device_type: {
                type: String,
                enum: ['Desktop', 'Mobile', 'Tablet', null],
                default: null,
                description: 'Device type'
            }
        },

        // 📊 ACTIVITY EXTENDED SECTION
        activity_extended: {
            last_failed_login: {
                type: Date,
                default: null,
                description: 'Last failed login timestamp'
            },
            inactivity_duration_days: {
                type: Number,
                default: 0,
                description: 'Days since last activity'
            },
            lead_expiration_date: {
                type: Date,
                default: null,
                description: 'Profile expiration date'
            },
            last_updated_section: {
                type: String,
                enum: ['profile', 'skills', 'settings', null],
                default: null,
                description: 'Last updated section'
            },
            last_updated_at: {
                type: Date,
                default: null,
                description: 'Last update timestamp'
            }
        },

        // 🔄 CHANGE HISTORY SECTION
        change_history: {
            email_updated: {
                type: Boolean,
                default: false,
                description: 'Email changed?'
            },
            email_updated_at: {
                type: Date,
                default: null,
                description: 'Email change timestamp'
            },
            phone_updated: {
                type: Boolean,
                default: false,
                description: 'Phone changed?'
            },
            phone_updated_at: {
                type: Date,
                default: null,
                description: 'Phone change timestamp'
            }
        },

        // ⚖️ LEGAL CONSENTS SECTION
        legal_consents: {
            cgu_accepted: {
                type: Boolean,
                default: false,
                description: 'Terms of service accepted?'
            },
            cgu_version: {
                type: String,
                default: null,
                description: 'T&C version (v1.0, v2.1, etc.)'
            },
            cgu_accepted_at: {
                type: Date,
                default: null,
                description: 'T&C acceptance timestamp'
            },
            cookies_accepted: {
                type: Boolean,
                default: false,
                description: 'Cookies accepted?'
            },
            cookies_accepted_at: {
                type: Date,
                default: null,
                description: 'Cookies acceptance timestamp'
            },
            marketing_opt_in: {
                type: Boolean,
                default: false,
                description: 'Opted in for marketing?'
            }
        },

        // 🎨 UI PREFERENCES SECTION
        ui_preferences: {
            language: {
                type: String,
                enum: ['fr', 'en', 'es', 'it', 'de'],
                default: 'fr',
                description: 'Interface language'
            },
            theme: {
                type: String,
                enum: ['light', 'dark', 'auto'],
                default: 'auto',
                description: 'UI theme preference'
            },
            browser: {
                type: String,
                default: null,
                description: 'Browser name'
            }
        },

        // 🔔 NOTIFICATION PREFERENCES SECTION
        notification_preferences: {
            email_enabled: {
                type: Boolean,
                default: true,
                description: 'Email notifications enabled?'
            },
            sms_enabled: {
                type: Boolean,
                default: true,
                description: 'SMS notifications enabled?'
            },
            push_enabled: {
                type: Boolean,
                default: true,
                description: 'Push notifications enabled?'
            },
            marketing_email_enabled: {
                type: Boolean,
                default: false,
                description: 'Marketing emails enabled?'
            },
            newsletter_unsubscribed: {
                type: Boolean,
                default: false,
                description: 'Unsubscribed from newsletter?'
            },
            newsletter_unsubscribed_at: {
                type: Date,
                default: null,
                description: 'Unsubscribe timestamp'
            },
            sms_disabled: {
                type: Boolean,
                default: false,
                description: 'SMS disabled?'
            },
            sms_disabled_at: {
                type: Date,
                default: null,
                description: 'SMS disable timestamp'
            },
            allow_internal_messaging: {
                type: Boolean,
                default: true,
                description: 'Allow in-app messaging?'
            }
        },

        // 📈 ACQUISITION DETAILS SECTION
        acquisition: {
            campaign_id: {
                type: String,
                default: null,
                description: 'Campaign identifier'
            },
            referral_code: {
                type: String,
                default: null,
                description: 'Referral code'
            },
            source_type: {
                type: String,
                enum: ['social_ads', 'platform_registration'],
                default: 'platform_registration',
                description: 'Registration source type'
            },
            registration_date: {
                type: Date,
                default: Date.now,
                description: 'Registration date'
            }
        },

        // 📱 SOCIAL ADS DATA (conditional: only if source_type = 'social_ads')
        social_ads_data: {
            facebook_lead_id: String,
            facebook_created_time: Date,
            ad_id: String,
            ad_name: String,
            adset_id: String,
            adset_name: String,
            campaign_name: String,
            form_id: String,
            form_name: String,
            is_organic: Boolean,
            platform: {
                type: String,
                enum: ['facebook', 'instagram', 'linkedin']
            },
            platform_insertion_date: Date,
            auto_login_created: Boolean,
            auto_login_email: String,
            auto_password_generated: Boolean,
            activation_email_sent: Boolean,
            activation_email_sent_at: Date,
            activation_sms_sent: Boolean,
            activation_sms_last_sent_at: Date
        },

        // 🌐 PLATFORM REGISTRATION DATA (conditional: only if source_type = 'platform_registration')
        platform_registration_data: {
            registration_source: {
                type: String,
                enum: ['organic', 'external_link'],
                description: 'Organic or external'
            },
            registration_method: {
                type: String,
                enum: ['form', 'ai_bot'],
                description: 'Registration method'
            },
            referral_url: {
                type: String,
                description: 'Full referral URL'
            },
            utm_source: {
                type: String,
                description: 'UTM source'
            },
            utm_medium: {
                type: String,
                description: 'UTM medium'
            },
            utm_campaign: {
                type: String,
                description: 'UTM campaign'
            }
        },

        // ⏰ METADATA
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
        collection: 'lead_settings',
        strict: false // Allow dynamic fields for social/platform data
    }
);

// Indexes
// NOTE: lead_id unique index handled by field definition
leadSettingsSchema.index({ 'acquisition.source_type': 1 });
leadSettingsSchema.index({ updated_at: -1 });

// Pre-save middleware
leadSettingsSchema.pre('save', function (next) {
    this.updated_at = new Date();
    next();
});

const LeadSettings = mongoose.model('LeadSettings', leadSettingsSchema);

export default LeadSettings;
