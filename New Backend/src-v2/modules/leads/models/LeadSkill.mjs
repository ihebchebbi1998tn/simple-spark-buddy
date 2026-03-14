/**
 * @file LeadSkill.mjs
 * @description MongoDB schema for LEAD_SKILLS table (Language tests, qualifications)
 * @version 2.0
 */

import mongoose from 'mongoose';

const leadSkillSchema = new mongoose.Schema(
    {
        // 🆔 CORE FIELDS
        lead_id: {
            type: mongoose.Schema.Types.ObjectId,
            ref: 'Lead',
            required: true,
            index: true,
            description: 'Foreign key to LEADS table'
        },

        // 🎯 TYPE & CATEGORY FIELDS
        qualification_type: {
            type: Number,
            enum: [1, 2, 3, 4, 5, 6, 7], // 1=Language, 2=Technical skill, 3=Certification, etc.
            required: true,
            description: 'Main qualification category'
        },
        test_method: {
            type: Number,
            enum: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
            required: true,
            description: 'Test delivery method'
        },

        // 🌍 LANGUAGE-SPECIFIC FIELDS (qualification_type = 1)
        language_code: {
            type: String,
            description: 'ISO language code (FR, EN, ES, IT, DE)'
        },
        is_native: {
            type: Boolean,
            default: false,
            description: 'Is this a native language?'
        },
        proficiency_level: {
            type: Number,
            min: 1,
            max: 7, // A1-C2 + Native
            description: 'CEFR level (1-7)'
        },

        // 💻 TECHNICAL SKILL-SPECIFIC FIELDS (qualification_type = 2)
        skill_name: {
            type: String,
            description: 'Technical skill name'
        },
        skill_category: {
            type: Number,
            description: 'Skill category code'
        },

        // 📜 CERTIFICATION-SPECIFIC FIELDS (qualification_type = 3)
        certification_name: {
            type: String,
            description: 'Certificate name'
        },
        certification_authority: {
            type: String,
            description: 'Issuing organization'
        },
        certification_number: {
            type: String,
            description: 'Certificate ID'
        },
        certification_expiry_date: {
            type: Date,
            default: null,
            description: 'Expiration date (null if no expiry)'
        },

        // 💾 SOFTWARE-SPECIFIC FIELDS (qualification_type = 4)
        software_name: {
            type: String,
            description: 'Software name'
        },
        software_version: {
            type: String,
            description: 'Software version'
        },

        // 📝 TEST STATUS FIELDS
        completed: {
            type: Boolean,
            default: false,
            required: true,
            description: 'Is test completed?'
        },
        test_date: {
            type: Date,
            default: null,
            description: 'Test date (null if incomplete)'
        },
        test_duration_seconds: {
            type: Number,
            min: 0,
            max: 10800, // 3 hours max
            default: null,
            description: 'Test duration in seconds'
        },

        // 🎯 SCORES OBJECT (only for completed tests)
        scores: {
            linguistic: {
                type: Number,
                min: 0,
                max: 100,
                description: 'Language accuracy score'
            },
            soft_skills: {
                type: Number,
                min: 0,
                max: 100,
                description: 'Communication/soft skills score'
            },
            job_skills: {
                type: Number,
                min: 0,
                max: 100,
                description: 'Job-specific skills score'
            },
            global: {
                type: Number,
                min: 0,
                max: 100,
                description: 'Overall score'
            }
        },

        // ✅ VERIFICATION & SOURCE FIELDS
        is_verified: {
            type: Boolean,
            default: false,
            required: true,
            description: 'Is qualification verified?'
        },
        verified_by: {
            type: Number,
            enum: [1, 2, 3, 4, null], // 1=system, 2=AI, 3=admin, 4=partner
            default: null,
            description: 'Verification source (1=system, 2=AI, 3=admin, 4=partner)'
        },
        verified_at: {
            type: Date,
            default: null,
            description: 'Verification timestamp'
        },
        source: {
            type: Number,
            enum: [1, 2, 3, 4], // 1=platform, 2=upload, 3=partner, 4=migration
            default: 1,
            description: 'Origin of qualification (1=platform, 2=upload, 3=partner, 4=migration)'
        },

        // 📁 FILE STORAGE FIELDS
        file_url: {
            type: String,
            default: null,
            description: 'Cloud storage URL (for certificates, diplomas, etc.)'
        },
        file_type: {
            type: String,
            default: null,
            description: 'MIME type (application/pdf, image/png, etc.)'
        },

        // ⏰ METADATA
        created_at: {
            type: Date,
            default: Date.now,
            required: true,
            description: 'Record creation timestamp'
        },
        updated_at: {
            type: Date,
            default: Date.now,
            required: true,
            description: 'Last update timestamp'
        }
    },
    {
        timestamps: true,
        collection: 'lead_skills',
        strict: true
    }
);

// Indexes
// NOTE: lead_id already has index from field definition
leadSkillSchema.index({ lead_id: 1, qualification_type: 1 });
leadSkillSchema.index({ qualification_type: 1 });
leadSkillSchema.index({ language_code: 1 });
leadSkillSchema.index({ completed: 1 });
leadSkillSchema.index({ verified_by: 1 });
leadSkillSchema.index({ source: 1 });
leadSkillSchema.index({ 'scores.global': -1 });
leadSkillSchema.index({ updated_at: -1 });

// Compound index for language test lookup
leadSkillSchema.index({ lead_id: 1, language_code: 1, qualification_type: 1 });

// Pre-save middleware
leadSkillSchema.pre('save', function (next) {
    this.updated_at = new Date();
    next();
});

const LeadSkill = mongoose.model('LeadSkill', leadSkillSchema);

export default LeadSkill;
