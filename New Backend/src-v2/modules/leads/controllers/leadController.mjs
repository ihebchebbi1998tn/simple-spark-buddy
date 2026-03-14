/**
 * @file leadController.mjs
 * @description API handlers for LEADS module
 * @version 1.0
 */

import jwt from 'jsonwebtoken';
import { config } from '../../../../src/config/env.mjs';

import {
    validateLeadRegistration,
    validateLeadLogin,
    validateLeadUpdate,
    sanitizeEmail,
    sanitizePhone
} from '../validators/leadValidator.mjs';

import {
    createLead,
    getLeadById,
    getLeadByEmail,
    getLeadByPublicId,
    getLeadForLogin,
    updateLeadProfile,
    updateLeadEmail,
    updateLeadPassword,
    updateLeadActivity,
    deleteLead,
    comparePassword,
    emailExists,
    verifyEmail,
    verifyPhone,
    getLeads
} from '../services/leadService.mjs';

/**
 * ============================================================================
 * REGISTRATION ENDPOINT
 * ============================================================================
 */

/**
 * POST /api/v2/leads/register
 * Register a new lead
 */
export const registerLead = async (req, res) => {
    try {
        // Validate input
        const validation = validateLeadRegistration(req.body);
        if (!validation.isValid) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: validation.errors
            });
        }

        // Check if email already exists (before hashing, for quick check)
        const emailExistsResult = await emailExists(sanitizeEmail(req.body.email));
        if (emailExistsResult) {
            return res.status(409).json({
                success: false,
                message: 'Email already registered',
                errors: { email: 'This email is already in use' }
            });
        }

        // Create lead
        const newLead = await createLead({
            email: sanitizeEmail(req.body.email),
            password: req.body.password,
            first_name: req.body.first_name,
            last_name: req.body.last_name,
            phone: sanitizePhone(req.body.phone),
            date_of_birth: req.body.date_of_birth,
            gender: req.body.gender,
            country_code: req.body.country_code,
            city_id: req.body.city_id,
            postal_code: req.body.postal_code,
            channel: req.body.channel || 2,
            campaign_id: req.body.campaign_id,
            referral_code: req.body.referral_code,
            source_type: req.body.source_type,
            language: req.body.language || 'fr',
            ip_address: req.ip,
            device_type: req.body.device_type
        });

        return res.status(201).json({
            success: true,
            message: 'Lead registered successfully',
            data: {
                lead_id: newLead._id,
                public_id: newLead.public_id,
                email: newLead.email,
                first_name: newLead.first_name,
                last_name: newLead.last_name
            }
        });
    } catch (error) {
        console.error('Registration error:', error);
        return res.status(500).json({
            success: false,
            message: 'Registration failed',
            error: error.message
        });
    }
};

/**
 * ============================================================================
 * LOGIN ENDPOINT
 * ============================================================================
 */

/**
 * POST /api/v2/leads/login
 * Authenticate lead and return JWT token
 */
export const loginLead = async (req, res) => {
    try {
        // Validate input
        const validation = validateLeadLogin(req.body);
        if (!validation.isValid) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: validation.errors
            });
        }

        // Get lead with password hash
        const lead = await getLeadForLogin(sanitizeEmail(req.body.email));

        if (!lead) {
            return res.status(401).json({
                success: false,
                message: 'Invalid email or password',
                errors: { credentials: 'Email or password incorrect' }
            });
        }

        // Check if account is locked
        if (lead.security.account_locked && lead.security.lock_until > new Date()) {
            return res.status(403).json({
                success: false,
                message: 'Account temporarily locked',
                error: `Please try again after ${lead.security.lock_until.toISOString()}`
            });
        }

        // Check if account is active
        if (lead.account_status !== 1) {
            return res.status(403).json({
                success: false,
                message: 'Account is not active',
                error: `Account status: ${lead.account_status}`
            });
        }

        // Verify password
        const passwordMatch = await comparePassword(req.body.password, lead.password_hash);

        if (!passwordMatch) {
            // Record failed login
            const failedAttempts = lead.security.failed_login_attempts + 1;
            let lockuntil = null;

            // Lock account after 5 failed attempts for 30 minutes
            if (failedAttempts >= 5) {
                lockuntil = new Date(Date.now() + 30 * 60 * 1000);
            }

            await updateLeadProfile(lead._id, {
                'security.failed_login_attempts': failedAttempts,
                'security.last_failed_login': new Date(),
                'security.account_locked': failedAttempts >= 5,
                'security.lock_until': lockuntil
            });

            return res.status(401).json({
                success: false,
                message: 'Invalid email or password',
                errors: { credentials: 'Email or password incorrect' }
            });
        }

        // Reset failed login attempts
        await updateLeadProfile(lead._id, {
            'security.failed_login_attempts': 0,
            'security.account_locked': false,
            'security.lock_until': null,
            last_login: new Date(),
            last_activity_at: new Date()
        });

        // Generate JWT token
        const token = jwt.sign(
            {
                id: lead._id,
                email: lead.email,
                role: 'user' // Default role for leads
            },
            config.jwt.secret,
            { expiresIn: '24h' }
        );

        return res.status(200).json({
            success: true,
            message: 'Login successful',
            data: {
                lead_id: lead._id,
                public_id: lead.public_id,
                email: lead.email,
                first_name: lead.first_name,
                last_name: lead.last_name,
                token: token
            }
        });
    } catch (error) {
        console.error('Login error:', error);
        return res.status(500).json({
            success: false,
            message: 'Login failed',
            error: error.message
        });
    }
};

/**
 * ============================================================================
 * GET LEAD ENDPOINTS
 * ============================================================================
 */

/**
 * GET /api/v2/leads/:leadId
 * Get lead profile by ID
 */
export const getLeadProfile = async (req, res) => {
    try {
        const lead = await getLeadById(req.params.leadId, {
            includeProfile: req.query.includeProfile === 'true',
            includeScoring: req.query.includeScoring === 'true',
            includeSettings: req.query.includeSettings === 'true'
        });

        if (!lead) {
            return res.status(404).json({
                success: false,
                message: 'Lead not found'
            });
        }

        // Update activity
        await updateLeadActivity(req.params.leadId);

        return res.status(200).json({
            success: true,
            data: lead
        });
    } catch (error) {
        console.error('Get lead error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to retrieve lead',
            error: error.message
        });
    }
};

/**
 * GET /api/v2/leads/email/:email
 * Get lead by email
 */
export const getLeadByEmailEndpoint = async (req, res) => {
    try {
        const lead = await getLeadByEmail(sanitizeEmail(req.params.email));

        if (!lead) {
            return res.status(404).json({
                success: false,
                message: 'Lead not found'
            });
        }

        return res.status(200).json({
            success: true,
            data: lead
        });
    } catch (error) {
        console.error('Get lead by email error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to retrieve lead',
            error: error.message
        });
    }
};

/**
 * GET /api/v2/leads/public/:publicId
 * Get lead by public ID
 */
export const getLeadByPublicIdEndpoint = async (req, res) => {
    try {
        const lead = await getLeadByPublicId(req.params.publicId);

        if (!lead) {
            return res.status(404).json({
                success: false,
                message: 'Lead not found'
            });
        }

        return res.status(200).json({
            success: true,
            data: lead
        });
    } catch (error) {
        console.error('Get lead by public ID error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to retrieve lead',
            error: error.message
        });
    }
};

/**
 * GET /api/v2/leads?page=1&limit=20&search=john&status=1
 * List leads with pagination and filtering
 */
export const listLeads = async (req, res) => {
    try {
        const result = await getLeads({
            page: parseInt(req.query.page) || 1,
            limit: parseInt(req.query.limit) || 20,
            search: req.query.search,
            status: req.query.status ? parseInt(req.query.status) : undefined
        });

        return res.status(200).json({
            success: true,
            data: result.data,
            pagination: result.pagination
        });
    } catch (error) {
        console.error('List leads error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to retrieve leads',
            error: error.message
        });
    }
};

/**
 * ============================================================================
 * UPDATE ENDPOINTS
 * ============================================================================
 */

/**
 * PUT /api/v2/leads/:leadId/profile
 * Update lead profile (name, phone, gender, location)
 */
export const updateLeadProfileEndpoint = async (req, res) => {
    try {
        // Validate update
        const validation = validateLeadUpdate(
            req.body,
            req.body.updateType || 'profile'
        );

        if (!validation.isValid) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: validation.errors
            });
        }

        // Perform update
        const updateData = {
            first_name: req.body.first_name,
            last_name: req.body.last_name,
            phone: req.body.phone ? sanitizePhone(req.body.phone) : undefined,
            gender: req.body.gender
        };

        // Remove undefined values
        Object.keys(updateData).forEach(
            key => updateData[key] === undefined && delete updateData[key]
        );

        const updatedLead = await updateLeadProfile(req.params.leadId, updateData);

        if (!updatedLead) {
            return res.status(404).json({
                success: false,
                message: 'Lead not found'
            });
        }

        return res.status(200).json({
            success: true,
            message: 'Profile updated successfully',
            data: updatedLead
        });
    } catch (error) {
        console.error('Update profile error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to update profile',
            error: error.message
        });
    }
};

/**
 * PUT /api/v2/leads/:leadId/email
 * Change email address
 */
export const updateLeadEmailEndpoint = async (req, res) => {
    try {
        // Validate
        const validation = validateLeadUpdate(
            { new_email: req.body.new_email, password: req.body.password },
            'email'
        );

        if (!validation.isValid) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: validation.errors
            });
        }

        // Verify current password
        const lead = await getLeadForLogin(req.params.leadId);
        const passwordMatch = await comparePassword(req.body.password, lead.password_hash);

        if (!passwordMatch) {
            return res.status(401).json({
                success: false,
                message: 'Invalid password',
                errors: { password: 'Current password is incorrect' }
            });
        }

        // Update email
        const updatedLead = await updateLeadEmail(
            req.params.leadId,
            sanitizeEmail(req.body.new_email)
        );

        return res.status(200).json({
            success: true,
            message: 'Email updated successfully',
            data: {
                lead_id: updatedLead._id,
                email: updatedLead.email
            }
        });
    } catch (error) {
        if (error.message === 'EMAIL_ALREADY_EXISTS') {
            return res.status(409).json({
                success: false,
                message: 'Email already in use',
                errors: { new_email: 'This email is already registered' }
            });
        }

        console.error('Update email error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to update email',
            error: error.message
        });
    }
};

/**
 * PUT /api/v2/leads/:leadId/password
 * Change password
 */
export const updateLeadPasswordEndpoint = async (req, res) => {
    try {
        // Validate
        const validation = validateLeadUpdate(
            {
                current_password: req.body.current_password,
                new_password: req.body.new_password,
                password_confirm: req.body.password_confirm
            },
            'password'
        );

        if (!validation.isValid) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: validation.errors
            });
        }

        // Verify current password
        const lead = await getLeadForLogin(req.params.leadId);
        if (!lead) {
            return res.status(404).json({
                success: false,
                message: 'Lead not found'
            });
        }

        const passwordMatch = await comparePassword(
            req.body.current_password,
            lead.password_hash
        );

        if (!passwordMatch) {
            return res.status(401).json({
                success: false,
                message: 'Invalid password',
                errors: { current_password: 'Current password is incorrect' }
            });
        }

        // Update password
        const updatedLead = await updateLeadPassword(
            req.params.leadId,
            req.body.new_password
        );

        return res.status(200).json({
            success: true,
            message: 'Password updated successfully',
            data: {
                lead_id: updatedLead._id,
                email: updatedLead.email
            }
        });
    } catch (error) {
        console.error('Update password error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to update password',
            error: error.message
        });
    }
};

/**
 * ============================================================================
 * VERIFICATION ENDPOINTS
 * ============================================================================
 */

/**
 * POST /api/v2/leads/:leadId/verify-email
 * Verify email address
 */
export const verifyEmailEndpoint = async (req, res) => {
    try {
        const updatedLead = await verifyEmail(req.params.leadId);

        if (!updatedLead) {
            return res.status(404).json({
                success: false,
                message: 'Lead not found'
            });
        }

        return res.status(200).json({
            success: true,
            message: 'Email verified successfully',
            data: {
                lead_id: updatedLead._id,
                email: updatedLead.email,
                email_verified: updatedLead.verification.email_verified
            }
        });
    } catch (error) {
        console.error('Verify email error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to verify email',
            error: error.message
        });
    }
};

/**
 * POST /api/v2/leads/:leadId/verify-phone
 * Verify phone number
 */
export const verifyPhoneEndpoint = async (req, res) => {
    try {
        const updatedLead = await verifyPhone(req.params.leadId);

        if (!updatedLead) {
            return res.status(404).json({
                success: false,
                message: 'Lead not found'
            });
        }

        return res.status(200).json({
            success: true,
            message: 'Phone verified successfully',
            data: {
                lead_id: updatedLead._id,
                phone: updatedLead.phone,
                phone_verified: updatedLead.verification.phone_verified
            }
        });
    } catch (error) {
        console.error('Verify phone error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to verify phone',
            error: error.message
        });
    }
};

/**
 * ============================================================================
 * DELETION ENDPOINTS
 * ============================================================================
 */

/**
 * DELETE /api/v2/leads/:leadId
 * Soft delete lead
 */
export const deleteLeadEndpoint = async (req, res) => {
    try {
        const deletedLead = await deleteLead(req.params.leadId);

        if (!deletedLead) {
            return res.status(404).json({
                success: false,
                message: 'Lead not found'
            });
        }

        return res.status(200).json({
            success: true,
            message: 'Lead deleted successfully',
            data: {
                lead_id: deletedLead._id,
                email: deletedLead.email
            }
        });
    } catch (error) {
        console.error('Delete lead error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to delete lead',
            error: error.message
        });
    }
};

/**
 * ============================================================================
 * EXPORT
 * ============================================================================
 */

export default {
    registerLead,
    loginLead,
    getLeadProfile,
    getLeadByEmailEndpoint,
    getLeadByPublicIdEndpoint,
    listLeads,
    updateLeadProfileEndpoint,
    updateLeadEmailEndpoint,
    updateLeadPasswordEndpoint,
    verifyEmailEndpoint,
    verifyPhoneEndpoint,
    deleteLeadEndpoint
};
