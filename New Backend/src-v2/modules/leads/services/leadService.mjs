/**
 * @file leadService.mjs
 * @description Business logic for LEADS CRUD operations
 * @version 1.0
 */

import bcrypt from 'bcryptjs';
import { Lead, LeadProfile, LeadScoring, LeadSettings, LeadSkill } from '../models/index.mjs';

/**
 * ============================================================================
 * HELPER FUNCTIONS
 * ============================================================================
 */

/**
 * Generate public ID for lead (LD-2026-00001, etc.)
 * @returns {Promise<string>} Generated public ID
 */
const generatePublicId = async () => {
    const count = await Lead.countDocuments();
    const year = new Date().getFullYear();
    return `LD-${year}-${String(count + 1).padStart(5, '0')}`;
};

/**
 * Hash password using bcrypt
 * @param {string} password - Plain text password
 * @returns {Promise<string>} Hashed password
 */
export const hashPassword = async (password) => {
    const saltRounds = 10;
    return bcrypt.hash(password, saltRounds);
};

/**
 * Compare plain password with hashed password
 * @param {string} plainPassword - Plain text password
 * @param {string} hashedPassword - Hashed password from DB
 * @returns {Promise<boolean>} True if match, false otherwise
 */
export const comparePassword = async (plainPassword, hashedPassword) => {
    return bcrypt.compare(plainPassword, hashedPassword);
};

/**
 * ============================================================================
 * LEAD CREATION
 * ============================================================================
 */

/**
 * Create a new lead (register)
 * @param {object} leadData - Lead registration data (validated)
 * @returns {Promise<object>} Created lead with id
 * @throws {Error} If email already exists or DB error
 */
export const createLead = async (leadData) => {
    try {
        // Check if email already exists
        const existingLead = await Lead.findOne({
            email_normalized: leadData.email.toLowerCase()
        });

        if (existingLead) {
            throw new Error('EMAIL_ALREADY_EXISTS');
        }

        // Hash password
        const passwordHash = await hashPassword(leadData.password);

        // Generate public ID
        const publicId = await generatePublicId();

        // Create lead document
        const lead = new Lead({
            public_id: publicId,
            email: leadData.email,
            email_normalized: leadData.email.toLowerCase(),
            password_hash: passwordHash,
            first_name: leadData.first_name,
            last_name: leadData.last_name,
            phone: leadData.phone,
            date_of_birth: leadData.date_of_birth,
            gender: leadData.gender || null,
            location: {
                country_code: leadData.country_code || null,
                city_id: leadData.city_id || null,
                postal_code: leadData.postal_code || null
            },
            channel: leadData.channel || 2, // 2 = organic (default)
            account_status: 4, // 4 = En attente
            lead_stage: 1, // 1 = Just registered
            verification: {
                email_verified: false,
                phone_verified: false,
                identity_verified: false
            },
            security: {
                two_factor_enabled: false,
                failed_login_attempts: 0,
                account_locked: false
            },
            summary_metrics: {
                skills_count: 0,
                languages_count: 0
            }
        });

        // Save lead
        await lead.save();

        // Create related documents
        await Promise.all([
            createLeadProfile(lead._id),
            createLeadScoring(lead._id),
            createLeadSettings(lead._id, leadData)
        ]);

        return {
            _id: lead._id,
            public_id: lead.public_id,
            email: lead.email,
            first_name: lead.first_name,
            last_name: lead.last_name,
            message: 'Lead created successfully'
        };
    } catch (error) {
        throw error;
    }
};

/**
 * Create lead profile document
 * @param {ObjectId} leadId - Lead ID
 * @returns {Promise<object>} Created LeadProfile
 */
const createLeadProfile = async (leadId) => {
    return LeadProfile.create({
        lead_id: leadId,
        desired_position: null,
        call_center_experience: null,
        call_center_self_assessment: null,
        position_experience: null,
        position_self_assessment: null,
        activities: [],
        work_preferences: {
            mode: null,
            time: null,
            shift: null,
            willing_to_relocate: false,
            preferred_cities: []
        },
        availability: {
            availability_status: null,
            contract_types: [],
            international_offers: false
        },
        company_preferences: {
            blacklisted_companies: [],
            whitelisted_companies: []
        },
        compensation: {
            salary_expectation: {},
            benefits_preferences: {
                health_insurance: false,
                tickets_restaurant: false,
                transport_allowance: false,
                performance_bonus: false
            }
        },
        bio: null
    });
};

/**
 * Create lead scoring document
 * @param {ObjectId} leadId - Lead ID
 * @returns {Promise<object>} Created LeadScoring
 */
const createLeadScoring = async (leadId) => {
    return LeadScoring.create({
        lead_id: leadId,
        profile_completion: {
            is_complete: false,
            completion_percentage: 0,
            incomplete_sections: []
        },
        pre_qualification: {
            internal_score: 0,
            ai_score: 0,
            ai_assessment: null,
            ai_recommendation: null
        },
        qualification_score: 0,
        post_qualification: {
            internal_score: 0,
            ai_score: 0,
            ai_assessment: null,
            ai_recommendation: null
        }
    });
};

/**
 * Create lead settings document
 * @param {ObjectId} leadId - Lead ID
 * @param {object} leadData - Lead data for settings
 * @returns {Promise<object>} Created LeadSettings
 */
const createLeadSettings = async (leadId, leadData) => {
    return LeadSettings.create({
        lead_id: leadId,
        account_state_audit: {
            account_confirmed: false,
            account_enabled: true,
            account_disabled: false,
            account_deleted: false
        },
        security_extended: {
            ip_address: leadData.ip_address || null,
            device_type: leadData.device_type || null
        },
        activity_extended: {
            inactivity_duration_days: 0
        },
        change_history: {
            email_updated: false,
            phone_updated: false
        },
        legal_consents: {
            cgu_accepted: false,
            cookies_accepted: false,
            marketing_opt_in: false
        },
        ui_preferences: {
            language: leadData.language || 'fr',
            theme: 'auto',
            browser: null
        },
        notification_preferences: {
            email_enabled: true,
            sms_enabled: true,
            push_enabled: true,
            marketing_email_enabled: false,
            newsletter_unsubscribed: false,
            sms_disabled: false,
            allow_internal_messaging: true
        },
        acquisition: {
            campaign_id: leadData.campaign_id || null,
            referral_code: leadData.referral_code || null,
            source_type: leadData.source_type || 'platform_registration',
            registration_date: new Date()
        }
    });
};

/**
 * ============================================================================
 * LEAD RETRIEVAL
 * ============================================================================
 */

/**
 * Get lead by ID with optional related documents
 * @param {string} leadId - Lead MongoDB ID
 * @param {object} options - Query options
 * @param {boolean} options.includeProfile - Include LeadProfile
 * @param {boolean} options.includeScoring - Include LeadScoring
 * @param {boolean} options.includeSettings - Include LeadSettings
 * @returns {Promise<object|null>} Lead document or null
 */
export const getLeadById = async (leadId, options = {}) => {
    try {
        const lead = await Lead.findById(leadId).select('-password_hash');

        if (!lead) {
            return null;
        }

        const result = { ...lead.toObject() };

        // Include related documents if requested
        if (options.includeProfile) {
            result.profile = await LeadProfile.findOne({ lead_id: leadId });
        }

        if (options.includeScoring) {
            result.scoring = await LeadScoring.findOne({ lead_id: leadId });
        }

        if (options.includeSettings) {
            result.settings = await LeadSettings.findOne({ lead_id: leadId });
        }

        return result;
    } catch (error) {
        throw error;
    }
};

/**
 * Get lead by email
 * @param {string} email - Email address
 * @param {object} options - Query options (same as getLeadById)
 * @returns {Promise<object|null>} Lead document or null
 */
export const getLeadByEmail = async (email, options = {}) => {
    try {
        const lead = await Lead.findOne({
            email_normalized: email.toLowerCase()
        }).select('-password_hash');

        if (!lead) {
            return null;
        }

        const result = { ...lead.toObject() };

        if (options.includeProfile) {
            result.profile = await LeadProfile.findOne({ lead_id: lead._id });
        }

        if (options.includeScoring) {
            result.scoring = await LeadScoring.findOne({ lead_id: lead._id });
        }

        if (options.includeSettings) {
            result.settings = await LeadSettings.findOne({ lead_id: lead._id });
        }

        return result;
    } catch (error) {
        throw error;
    }
};

/**
 * Get lead by public ID
 * @param {string} publicId - Public ID (LD-2026-00001)
 * @param {object} options - Query options
 * @returns {Promise<object|null>} Lead document or null
 */
export const getLeadByPublicId = async (publicId, options = {}) => {
    try {
        const lead = await Lead.findOne({ public_id: publicId }).select('-password_hash');

        if (!lead) {
            return null;
        }

        const result = { ...lead.toObject() };

        if (options.includeProfile) {
            result.profile = await LeadProfile.findOne({ lead_id: lead._id });
        }

        if (options.includeScoring) {
            result.scoring = await LeadScoring.findOne({ lead_id: lead._id });
        }

        if (options.includeSettings) {
            result.settings = await LeadSettings.findOne({ lead_id: lead._id });
        }

        return result;
    } catch (error) {
        throw error;
    }
};

/**
 * Get leads for login (return with password hash)
 * @param {string} email - Email address
 * @returns {Promise<object|null>} Lead with password_hash or null
 */
export const getLeadForLogin = async (email) => {
    try {
        return await Lead.findOne({
            email_normalized: email.toLowerCase()
        });
    } catch (error) {
        throw error;
    }
};

/**
 * ============================================================================
 * LEAD UPDATES
 * ============================================================================
 */

/**
 * Update lead profile fields
 * @param {string} leadId - Lead ID
 * @param {object} updateData - Fields to update
 * @returns {Promise<object>} Updated lead
 */
export const updateLeadProfile = async (leadId, updateData) => {
    try {
        const allowedFields = ['first_name', 'last_name', 'phone', 'gender', 'location'];
        const safeUpdateData = {};

        // Only allow specific fields to be updated
        allowedFields.forEach(field => {
            if (updateData[field] !== undefined) {
                safeUpdateData[field] = updateData[field];
            }
        });

        // Activity tracking
        safeUpdateData.last_activity_at = new Date();

        const updatedLead = await Lead.findByIdAndUpdate(
            leadId,
            safeUpdateData,
            { new: true, runValidators: true }
        ).select('-password_hash');

        return updatedLead;
    } catch (error) {
        throw error;
    }
};

/**
 * Update lead email
 * @param {string} leadId - Lead ID
 * @param {string} newEmail - New email address
 * @returns {Promise<object>} Updated lead
 * @throws {Error} If email already exists
 */
export const updateLeadEmail = async (leadId, newEmail) => {
    try {
        // Check if new email already exists
        const existingLead = await Lead.findOne({
            email_normalized: newEmail.toLowerCase(),
            _id: { $ne: leadId }
        });

        if (existingLead) {
            throw new Error('EMAIL_ALREADY_EXISTS');
        }

        const updatedLead = await Lead.findByIdAndUpdate(
            leadId,
            {
                email: newEmail,
                email_normalized: newEmail.toLowerCase(),
                'change_history.email_updated': true,
                'change_history.email_updated_at': new Date(),
                last_activity_at: new Date()
            },
            { new: true, runValidators: true }
        ).select('-password_hash');

        return updatedLead;
    } catch (error) {
        throw error;
    }
};

/**
 * Update lead password
 * @param {string} leadId - Lead ID
 * @param {string} newPassword - New plain password
 * @returns {Promise<object>} Updated lead
 */
export const updateLeadPassword = async (leadId, newPassword) => {
    try {
        const passwordHash = await hashPassword(newPassword);

        const updatedLead = await Lead.findByIdAndUpdate(
            leadId,
            {
                password_hash: passwordHash,
                'security.password_updated_at': new Date(),
                'security.failed_login_attempts': 0,
                last_activity_at: new Date()
            },
            { new: true, runValidators: true }
        ).select('-password_hash');

        return updatedLead;
    } catch (error) {
        throw error;
    }
};

/**
 * Update lead security (lock/unlock account)
 * @param {string} leadId - Lead ID
 * @param {object} securityUpdate - Security fields
 * @returns {Promise<object>} Updated lead
 */
export const updateLeadSecurity = async (leadId, securityUpdate) => {
    try {
        const updateData = {
            'security.failed_login_attempts': securityUpdate.failed_login_attempts || 0,
            'security.last_failed_login': securityUpdate.last_failed_login || null,
            'security.account_locked': securityUpdate.account_locked || false,
            'security.lock_until': securityUpdate.lock_until || null
        };

        const updatedLead = await Lead.findByIdAndUpdate(
            leadId,
            updateData,
            { new: true }
        ).select('-password_hash');

        return updatedLead;
    } catch (error) {
        throw error;
    }
};

/**
 * Update lead activity timestamp
 * @param {string} leadId - Lead ID
 * @returns {Promise<object>} Updated lead
 */
export const updateLeadActivity = async (leadId) => {
    try {
        return await Lead.findByIdAndUpdate(
            leadId,
            { last_activity_at: new Date() },
            { new: true }
        ).select('-password_hash');
    } catch (error) {
        throw error;
    }
};

/**
 * ============================================================================
 * LEAD DELETION
 * ============================================================================
 */

/**
 * Delete lead (soft delete - mark as deleted)
 * @param {string} leadId - Lead ID
 * @param {string} deletedBy - Who deleted (admin/system)
 * @returns {Promise<object>} Deleted lead
 */
export const deleteLead = async (leadId, deletedBy = 'system') => {
    try {
        const deletedLead = await Lead.findByIdAndUpdate(
            leadId,
            {
                account_status: 2, // Inactif
                'security.account_deleted': true,
                'security.account_deleted_at': new Date(),
                'security.account_deleted_by': deletedBy
            },
            { new: true }
        ).select('-password_hash');

        return deletedLead;
    } catch (error) {
        throw error;
    }
};

/**
 * Permanently delete lead (hard delete - remove from DB)
 * @param {string} leadId - Lead ID
 * @returns {Promise<object>} Deleted lead
 */
export const hardDeleteLead = async (leadId) => {
    try {
        // Delete all related documents
        await Promise.all([
            Lead.findByIdAndDelete(leadId),
            LeadProfile.deleteOne({ lead_id: leadId }),
            LeadScoring.deleteOne({ lead_id: leadId }),
            LeadSettings.deleteOne({ lead_id: leadId }),
            LeadSkill.deleteMany({ lead_id: leadId })
        ]);

        return { message: 'Lead permanently deleted', leadId };
    } catch (error) {
        throw error;
    }
};

/**
 * ============================================================================
 * LEAD VERIFICATION
 * ============================================================================
 */

/**
 * Check if email exists
 * @param {string} email - Email to check
 * @returns {Promise<boolean>} True if exists, false otherwise
 */
export const emailExists = async (email) => {
    try {
        const lead = await Lead.findOne({
            email_normalized: email.toLowerCase()
        });
        return !!lead;
    } catch (error) {
        throw error;
    }
};

/**
 * Verify email
 * @param {string} leadId - Lead ID
 * @returns {Promise<object>} Updated lead
 */
export const verifyEmail = async (leadId) => {
    try {
        const updatedLead = await Lead.findByIdAndUpdate(
            leadId,
            {
                'verification.email_verified': true,
                'verification.email_verified_at': new Date(),
                'account_state_audit.account_confirmed': true,
                'account_state_audit.account_confirmed_at': new Date(),
                account_status: 1, // Actif
                last_activity_at: new Date()
            },
            { new: true }
        ).select('-password_hash');

        return updatedLead;
    } catch (error) {
        throw error;
    }
};

/**
 * Verify phone
 * @param {string} leadId - Lead ID
 * @returns {Promise<object>} Updated lead
 */
export const verifyPhone = async (leadId) => {
    try {
        return await Lead.findByIdAndUpdate(
            leadId,
            {
                'verification.phone_verified': true,
                'verification.phone_verified_at': new Date(),
                last_activity_at: new Date()
            },
            { new: true }
        ).select('-password_hash');
    } catch (error) {
        throw error;
    }
};

/**
 * ============================================================================
 * LEAD QUERIES
 * ============================================================================
 */

/**
 * Get leads with pagination
 * @param {object} options - Query options
 * @param {number} options.page - Page number (default 1)
 * @param {number} options.limit - Page size (default 20)
 * @param {string} options.search - Search term (email, name)
 * @param {number} options.status - Filter by account status
 * @returns {Promise<object>} Paginated leads with metadata
 */
export const getLeads = async (options = {}) => {
    try {
        const page = Math.max(1, options.page || 1);
        const limit = Math.min(100, options.limit || 20);
        const skip = (page - 1) * limit;

        // Build query
        const query = {};

        if (options.search) {
            query.$or = [
                { email: { $regex: options.search, $options: 'i' } },
                { first_name: { $regex: options.search, $options: 'i' } },
                { last_name: { $regex: options.search, $options: 'i' } }
            ];
        }

        if (options.status) {
            query.account_status = options.status;
        }

        // Execute query
        const [leads, total] = await Promise.all([
            Lead.find(query)
                .select('-password_hash')
                .limit(limit)
                .skip(skip)
                .sort({ created_at: -1 }),
            Lead.countDocuments(query)
        ]);

        return {
            data: leads,
            pagination: {
                page,
                limit,
                total,
                pages: Math.ceil(total / limit)
            }
        };
    } catch (error) {
        throw error;
    }
};

/**
 * ============================================================================
 * EXPORT
 * ============================================================================
 */

export default {
    hashPassword,
    comparePassword,
    createLead,
    getLeadById,
    getLeadByEmail,
    getLeadByPublicId,
    getLeadForLogin,
    updateLeadProfile,
    updateLeadEmail,
    updateLeadPassword,
    updateLeadSecurity,
    updateLeadActivity,
    deleteLead,
    hardDeleteLead,
    emailExists,
    verifyEmail,
    verifyPhone,
    getLeads
};
