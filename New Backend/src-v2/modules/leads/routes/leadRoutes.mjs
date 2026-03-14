import express from 'express';
import * as leadController from '../controllers/leadController.mjs';

const router = express.Router();

/**
 * @route   POST /api/v2/leads/register
 * @desc    Register a new lead with email, password, and basic info
 * @access  Public
 */
router.post('/register', leadController.registerLead);

/**
 * @route   POST /api/v2/leads/login
 * @desc    Authenticate a lead with email and password
 * @access  Public
 * @note    Tracks failed login attempts, locks account after 5 attempts for 30 min
 */
router.post('/login', leadController.loginLead);

/**
 * @route   GET /api/v2/leads/:leadId
 * @desc    Get a lead by ID
 * @access  Private (should add auth middleware)
 */
router.get('/:leadId', leadController.getLeadProfile);

/**
 * @route   GET /api/v2/leads/email/:email
 * @desc    Get a lead by email address
 * @access  Private (should add auth middleware)
 */
router.get('/email/:email', leadController.getLeadByEmailEndpoint);

/**
 * @route   GET /api/v2/leads/public/:publicId
 * @desc    Get a lead by public ID (human-readable format LD-YYYY-#####)
 * @access  Private (should add auth middleware)
 */
router.get('/public/:publicId', leadController.getLeadByPublicIdEndpoint);

/**
 * @route   GET /api/v2/leads
 * @desc    List all leads with pagination, search, and filtering
 * @query   page=1
 * @query   limit=20
 * @query   search=email|firstName|lastName
 * @query   status=1|2|3 (Attente confirmation|Actif|Inactif)
 * @access  Private (should add auth middleware)
 */
router.get('/', leadController.listLeads);

/**
 * @route   PUT /api/v2/leads/:leadId/profile
 * @desc    Update lead profile information
 * @access  Private (should add auth middleware)
 */
router.put('/:leadId/profile', leadController.updateLeadProfileEndpoint);

/**
 * @route   PUT /api/v2/leads/:leadId/email
 * @desc    Update lead email address
 * @access  Private (should add auth middleware)
 */
router.put('/:leadId/email', leadController.updateLeadEmailEndpoint);

/**
 * @route   PUT /api/v2/leads/:leadId/password
 * @desc    Update lead password
 * @access  Private (should add auth middleware)
 */
router.put('/:leadId/password', leadController.updateLeadPasswordEndpoint);

/**
 * @route   POST /api/v2/leads/:leadId/verify-email
 * @desc    Verify lead email address
 * @access  Public (via email verification link token)
 */
router.post('/:leadId/verify-email', leadController.verifyEmailEndpoint);

/**
 * @route   POST /api/v2/leads/:leadId/verify-phone
 * @desc    Verify lead phone number
 * @access  Public (via OTP or verification code)
 */
router.post('/:leadId/verify-phone', leadController.verifyPhoneEndpoint);

/**
 * @route   DELETE /api/v2/leads/:leadId
 * @desc    Soft delete a lead (marks as inactive)
 * @access  Private (should add auth middleware)
 */
router.delete('/:leadId', leadController.deleteLeadEndpoint);

export default router;
