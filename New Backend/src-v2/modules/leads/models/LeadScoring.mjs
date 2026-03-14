/**
 * @file LeadScoring.mjs
 * @description MongoDB schema for LEAD_SCORING table
 * @version 1.0
 */

import mongoose from 'mongoose';

const leadScoringSchema = new mongoose.Schema(
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

        // ✅ PROFILE COMPLETION SECTION
        profile_completion: {
            is_complete: {
                type: Boolean,
                default: false,
                description: 'Is profile 100% complete?'
            },
            completion_percentage: {
                type: Number,
                min: 0,
                max: 100,
                default: 0,
                description: 'Completion percentage (0-100)'
            },
            incomplete_sections: {
                type: [Number],
                default: [],
                description: 'Missing section codes'
            }
        },

        // 📊 PRE-QUALIFICATION SCORES SECTION
        pre_qualification: {
            internal_score: {
                type: Number,
                min: 0,
                max: 100,
                default: 0,
                description: 'Internal algorithm score'
            },
            ai_score: {
                type: Number,
                min: 0,
                max: 100,
                default: 0,
                description: 'AI evaluation score'
            },
            ai_assessment: {
                type: Number,
                min: 1,
                max: 10,
                description: 'AI assessment code (1-10)'
            },
            ai_recommendation: {
                type: Number,
                min: 1,
                max: 10,
                description: 'AI recommendation code (1-10)'
            },
            ai_score_testAI_liveWritten: {
                type: Number,
                min: 1,
                max: 10,
                default: null,
                description: 'AI score for live written test'
            },
            ai_score_testAI_liveAudio: {
                type: Number,
                min: 1,
                max: 10,
                default: null,
                description: 'AI score for live audio test'
            },
            ai_score_testAI_recordedAudio: {
                type: Number,
                min: 1,
                max: 10,
                default: null,
                description: 'AI score for recorded audio test'
            },
            ai_score_globalAI: {
                type: Number,
                min: 1,
                max: 10,
                default: null,
                description: 'Aggregated AI score'
            },
            ai_assessment_liveTests: {
                type: Number,
                min: 1,
                max: 10,
                default: null,
                description: 'AI assessment from live tests'
            },
            ai_recommendation_liveTests: {
                type: Number,
                min: 1,
                max: 10,
                default: null,
                description: 'AI recommendation from live tests'
            }
        },

        // 🎓 QUALIFICATION SCORE SECTION
        qualification_score: {
            type: Number,
            min: 0,
            max: 100,
            default: 0,
            description: 'Average score from all qualification tests'
        },

        // 📈 POST-QUALIFICATION SCORES SECTION
        post_qualification: {
            internal_score: {
                type: Number,
                min: 0,
                max: 100,
                default: 0,
                description: 'Internal score after tests'
            },
            ai_score: {
                type: Number,
                min: 0,
                max: 100,
                default: 0,
                description: 'AI score after tests'
            },
            ai_assessment: {
                type: Number,
                min: 1,
                max: 10,
                description: 'AI assessment after tests'
            },
            ai_recommendation: {
                type: Number,
                min: 1,
                max: 10,
                description: 'AI recommendation after tests'
            }
        },

        // ⏰ METADATA
        last_calculated_at: {
            type: Date,
            default: null,
            description: 'Last calculation timestamp'
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
        collection: 'lead_scoring',
        strict: true
    }
);

// Indexes
// NOTE: lead_id already has unique index from field definition
leadScoringSchema.index({ 'pre_qualification.internal_score': -1 });
leadScoringSchema.index({ 'pre_qualification.ai_score': -1 });
leadScoringSchema.index({ qualification_score: -1 });
leadScoringSchema.index({ updated_at: -1 });

// Pre-save middleware
leadScoringSchema.pre('save', function (next) {
    this.updated_at = new Date();
    this.last_calculated_at = new Date();
    next();
});

const LeadScoring = mongoose.model('LeadScoring', leadScoringSchema);

export default LeadScoring;
