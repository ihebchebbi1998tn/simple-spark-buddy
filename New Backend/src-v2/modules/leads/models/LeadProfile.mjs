/**
 * @file LeadProfile.mjs
 * @description MongoDB schema for LEAD_PROFILES table
 * @version 2.1
 */

import mongoose from 'mongoose';

const leadProfileSchema = new mongoose.Schema(
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

        // 💼 POSITION & EXPERIENCE SECTION
        desired_position: {
            type: Number,
            default: null,
            description: 'Target job role code (1-9)'
        },
        call_center_experience: {
            type: Number,
            min: 1,
            max: 7,
            default: null,
            description: 'Experience level in call centers'
        },
        call_center_self_assessment: {
            type: Number,
            min: 1,
            max: 5,
            default: null,
            description: 'Self-evaluation (1=Beginner, 5=Expert)'
        },
        position_experience: {
            type: Number,
            min: 1,
            max: 7,
            default: null,
            description: 'Experience in desired position'
        },
        position_self_assessment: {
            type: Number,
            min: 1,
            max: 5,
            default: null,
            description: 'Self-evaluation for position'
        },

        // 🎯 ACTIVITIES SECTION (1-3 activities with nested operations)
        activities: {
            type: [
                {
                    activity_type: {
                        type: Number,
                        required: true,
                        enum: [1, 2, 3, 4], // Télévente, Téléprospection, Prise de RDV, Service client
                        description: 'Activity type code'
                    },
                    activity_experience: {
                        type: Number,
                        required: true,
                        min: 1,
                        max: 7,
                        description: 'Experience level (1-7)'
                    },
                    operations: {
                        type: [
                            {
                                operation_type: {
                                    type: Number,
                                    required: true,
                                    description: 'Operation code (1-7)'
                                },
                                operation_experience: {
                                    type: Number,
                                    required: true,
                                    min: 1,
                                    max: 7,
                                    description: 'Experience level'
                                },
                                training_needed: {
                                    type: Boolean,
                                    default: false,
                                    description: 'Needs training for this operation'
                                }
                            }
                        ],
                        default: [],
                        maxlength: 3,
                        description: 'Operations (max 3)'
                    }
                }
            ],
            default: [],
            maxlength: 3,
            description: 'Activities (0-3, priority order) - optional during registration'
        },

        // 🏢 WORK PREFERENCES SECTION
        work_preferences: {
            mode: {
                type: Number,
                enum: [1, 2, 3], // 1=Remote, 2=On-site, 3=Hybrid
                default: null,
                description: 'Work location preference'
            },
            time: {
                type: Number,
                enum: [1, 2, 3, 4, 5],
                default: null,
                description: 'Schedule type (1=Full-time, 2=Part-time, etc.)'
            },
            shift: {
                type: Number,
                enum: [1, 2, 3], // 1=Morning, 2=Afternoon, 3=Night
                default: null,
                description: 'Shift preference'
            },
            willing_to_relocate: {
                type: Boolean,
                default: false,
                description: 'Willing to relocate?'
            },
            preferred_cities: {
                type: [Number],
                maxlength: 5,
                description: 'Preferred city IDs (max 5)'
            }
        },

        // 📅 AVAILABILITY & CONTRACTS SECTION
        availability: {
            availability_status: {
                type: Number,
                enum: [1, 2, 3, 4, 5, 6], // 1=Immediately, 2=1 week, etc.
                default: null,
                description: 'Availability timeframe'
            },
            contract_types: {
                type: [Number],
                maxlength: 8,
                description: 'Preferred contract types (1-8)'
            },
            international_offers: {
                type: Boolean,
                default: false,
                description: 'Open to international offers?'
            }
        },

        // 🏢 COMPANY PREFERENCES SECTION
        company_preferences: {
            blacklisted_companies: {
                type: [
                    {
                        company_id: {
                            type: String,
                            description: 'Company identifier'
                        },
                        reason: {
                            type: Number,
                            min: 1,
                            max: 10,
                            description: 'Reason code'
                        }
                    }
                ],
                maxlength: 10,
                default: [],
                description: 'Blacklisted companies (max 10)'
            },
            whitelisted_companies: {
                type: [String],
                maxlength: 10,
                default: [],
                description: 'Preferred companies (max 10)'
            }
        },

        // 💰 COMPENSATION SECTION
        compensation: {
            salary_expectation: {
                min: {
                    type: Number,
                    description: 'Minimum salary in cents'
                },
                max: {
                    type: Number,
                    description: 'Maximum salary in cents'
                },
                currency: {
                    type: Number,
                    enum: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], // EUR, USD, etc.
                    description: 'Currency code'
                },
                period: {
                    type: Number,
                    enum: [1, 2, 3, 4], // 1=Annual, 2=Monthly, 3=Weekly, 4=Hourly
                    description: 'Salary period'
                }
            },
            benefits_preferences: {
                health_insurance: Boolean,
                tickets_restaurant: Boolean,
                transport_allowance: Boolean,
                performance_bonus: Boolean
            }
        },

        // 📝 ADDITIONAL INFO SECTION
        bio: {
            type: String,
            maxlength: 500,
            default: null,
            description: 'Professional bio/summary'
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
        collection: 'lead_profiles',
        strict: true
    }
);

// Indexes
// NOTE: lead_id already has unique index from field definition
leadProfileSchema.index({ desired_position: 1 });
leadProfileSchema.index({ 'work_preferences.mode': 1 });
leadProfileSchema.index({ updated_at: -1 });

// Pre-save middleware
leadProfileSchema.pre('save', function (next) {
    this.updated_at = new Date();
    next();
});

const LeadProfile = mongoose.model('LeadProfile', leadProfileSchema);

export default LeadProfile;
