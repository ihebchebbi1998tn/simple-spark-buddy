/**
 * @file candidates.mjs
 * @description Candidate registration and profile routes
 */

/**
 * @swagger
 * tags:
 *   name: Candidates
 *   description: Candidate management and profile endpoints
 */

import express from 'express';
import { registerCandidate, checkAvailability } from '../controllers/candidateAuthController.mjs';
import { 
  getProfile, 
  getAllCandidates, 
  getCandidateById 
} from '../controllers/candidateViewController.mjs';
import { 
  updateCandidateInfo, 
  updateCandidateProfile 
} from '../controllers/candidateProfileController.mjs';
import { 
  updateAvailability, 
  updateContractPreferences, 
  updateCallCenterPreferences,
  updateNotificationSettings 
} from '../controllers/candidatePreferencesController.mjs';
import { addTestScore } from '../controllers/candidateTestController.mjs';
import { updateProximityPreferences } from '../controllers/candidateProximityController.mjs';
import { 
  changePassword, 
  changeEmail, 
  deleteAccount 
} from '../controllers/candidateAccessController.mjs';
import {
  getAlerts,
  markAlertAsRead,
  markAllAlertsAsRead,
  deleteOldAlerts
} from '../controllers/candidateAlertController.mjs';
import { authenticate } from '../../../middleware/auth.mjs';
import { validateRequired } from '../../../middleware/validation.mjs';

const router = express.Router();

/**
 * @swagger
 * /api/candidates/check-availability:
 *   post:
 *     summary: Check if email or phone already exists
 *     tags: [Candidates]
 *     security: []
 *     requestBody:
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               email:
 *                 type: string
 *                 format: email
 *               phone:
 *                 type: string
 *     responses:
 *       200:
 *         description: Availability check result
 *         content:
 *           application/json:
 *             schema:
 *               type: object
 *               properties:
 *                 success:
 *                   type: boolean
 *                 data:
 *                   type: object
 *                   properties:
 *                     emailExists:
 *                       type: boolean
 *                     phoneExists:
 *                       type: boolean
 */
router.post('/check-availability', checkAvailability);

/**
 * @swagger
 * /api/candidates/register:
 *   post:
 *     summary: Register a new candidate
 *     tags: [Candidates]
 *     security: []
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             required:
 *               - civilite
 *               - nom
 *               - prenom
 *               - telephone
 *               - villeResidence
 *               - email
 *               - motDePasse
 *               - posteRecherche
 *               - experienceGlobale
 *               - langueMaternelle
 *               - langueForeign1
 *               - niveauLangueForeign1
 *               - modeTravail
 *               - tempsTravail
 *               - parcTravail
 *             properties:
 *               civilite:
 *                 type: string
 *                 enum: [M., Mme]
 *               nom:
 *                 type: string
 *               prenom:
 *                 type: string
 *               telephone:
 *                 type: string
 *               villeResidence:
 *                 type: string
 *               email:
 *                 type: string
 *                 format: email
 *               motDePasse:
 *                 type: string
 *                 format: password
 *               posteRecherche:
 *                 type: string
 *               experienceGlobale:
 *                 type: string
 *               langueMaternelle:
 *                 type: string
 *               langueForeign1:
 *                 type: string
 *               niveauLangueForeign1:
 *                 type: string
 *               modeTravail:
 *                 type: string
 *               tempsTravail:
 *                 type: string
 *               parcTravail:
 *                 type: string
 *     responses:
 *       201:
 *         description: Registration successful
 *         content:
 *           application/json:
 *             schema:
 *               $ref: '#/components/schemas/Success'
 *       400:
 *         description: Validation error
 *       409:
 *         description: Email or phone already exists
 */
router.post(
  '/register',
  validateRequired([
    'civilite',
    'nom',
    'prenom',
    'telephone',
    'villeResidence',
    'email',
    'motDePasse',
    'posteRecherche',
    'experienceGlobale',
    'langueMaternelle',
    'langueForeign1',
    'niveauLangueForeign1',
    'modeTravail',
    'tempsTravail',
    'parcTravail'
  ]),
  registerCandidate
);

/**
 * @swagger
 * /api/candidates/profile:
 *   get:
 *     summary: Get candidate profile
 *     tags: [Candidates]
 *     security:
 *       - bearerAuth: []
 *     responses:
 *       200:
 *         description: Profile data retrieved successfully
 *         content:
 *           application/json:
 *             schema:
 *               type: object
 *               properties:
 *                 success:
 *                   type: boolean
 *                 data:
 *                   $ref: '#/components/schemas/CandidateProfile'
 *       401:
 *         description: Not authenticated
 *       404:
 *         description: Candidate not found
 */
router.get('/profile', authenticate, getProfile);

/**
 * @swagger
 * /api/candidates/all:
 *   get:
 *     summary: Get all candidates with complete information
 *     tags: [Candidates]
 *     security:
 *       - bearerAuth: []
 *     parameters:
 *       - in: query
 *         name: page
 *         schema:
 *           type: integer
 *           default: 1
 *         description: Page number for pagination
 *       - in: query
 *         name: limit
 *         schema:
 *           type: integer
 *           default: 50
 *         description: Number of candidates per page
 *     responses:
 *       200:
 *         description: List of all candidates retrieved successfully
 *         content:
 *           application/json:
 *             schema:
 *               type: object
 *               properties:
 *                 success:
 *                   type: boolean
 *                   example: true
 *                 data:
 *                   type: object
 *                   properties:
 *                     candidates:
 *                       type: array
 *                       items:
 *                         $ref: '#/components/schemas/CandidateProfile'
 *                     pagination:
 *                       type: object
 *                       properties:
 *                         page:
 *                           type: integer
 *                         limit:
 *                           type: integer
 *                         total:
 *                           type: integer
 *       401:
 *         description: Not authenticated
 */
router.get('/all', authenticate, getAllCandidates);

/**
 * @swagger
 * /api/candidates/info:
 *   put:
 *     summary: Update candidate basic information
 *     tags: [Candidates]
 *     security:
 *       - bearerAuth: []
 *     requestBody:
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               name:
 *                 type: string
 *               surname:
 *                 type: string
 *               phone:
 *                 type: string
 *               city:
 *                 type: string
 *               gender:
 *                 type: string
 *     responses:
 *       200:
 *         description: Information updated successfully
 *       401:
 *         description: Not authenticated
 */
router.put('/info', authenticate, updateCandidateInfo);

/**
 * @route   PUT /api/candidates/profile
 * @desc    Update CV/Profile information (activities, operations, languages)
 * @access  Private
 */
router.put('/profile', authenticate, updateCandidateProfile);

/**
 * @route   PUT /api/candidates/availability
 * @desc    Update availability preferences
 * @access  Private
 */
router.put('/availability', authenticate, updateAvailability);

/**
 * @route   PUT /api/candidates/contract-preferences
 * @desc    Update contract preferences
 * @access  Private
 */
router.put('/contract-preferences', authenticate, updateContractPreferences);

/**
 * @route   POST /api/candidates/test-scores
 * @desc    Add/Update language test scores (for existing logged-in users)
 * @access  Private
 */
router.post('/test-scores', authenticate, addTestScore);

/**
 * @route   PUT /api/candidates/call-centers
 * @desc    Update call center blacklist/whitelist
 * @access  Private
 */
router.put('/call-centers', authenticate, updateCallCenterPreferences);

/**
 * @route   PUT /api/candidates/proximity
 * @desc    Update geographic proximity preferences
 * @access  Private
 */
router.put('/proximity', authenticate, updateProximityPreferences);

/**
 * @route   PUT /api/candidates/notification-settings
 * @desc    Update notification and privacy settings
 * @access  Private
 */
router.put('/notification-settings', authenticate, updateNotificationSettings);

/**
 * @swagger
 * /api/candidates/password:
 *   put:
 *     summary: Change candidate password
 *     tags: [Candidates]
 *     security:
 *       - bearerAuth: []
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             required:
 *               - currentPassword
 *               - newPassword
 *             properties:
 *               currentPassword:
 *                 type: string
 *                 format: password
 *               newPassword:
 *                 type: string
 *                 format: password
 *                 minLength: 8
 *     responses:
 *       200:
 *         description: Password changed successfully
 *       401:
 *         description: Invalid current password
 */
router.put('/password', authenticate, changePassword);

/**
 * @route   PUT /api/candidates/email
 * @desc    Change candidate email
 * @access  Private
 */
router.put('/email', authenticate, changeEmail);

/**
 * @swagger
 * /api/candidates/account:
 *   delete:
 *     summary: Delete candidate account
 *     tags: [Candidates]
 *     security:
 *       - bearerAuth: []
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             required:
 *               - password
 *               - confirmation
 *             properties:
 *               password:
 *                 type: string
 *                 format: password
 *               confirmation:
 *                 type: string
 *                 enum: [SUPPRIMER]
 *                 description: Must be exactly "SUPPRIMER" to confirm
 *     responses:
 *       200:
 *         description: Account deleted successfully
 *       401:
 *         description: Invalid password
 */
router.delete('/account', authenticate, deleteAccount);

// =============================================
// ALERT ROUTES - Must come BEFORE /:id route
// =============================================

/**
 * @route   GET /api/candidates/alerts
 * @desc    Get all alerts for authenticated candidate
 * @access  Private
 */
router.get('/alerts', authenticate, getAlerts);

/**
 * @route   PUT /api/candidates/alerts/read-all
 * @desc    Mark all alerts as read
 * @access  Private
 */
router.put('/alerts/read-all', authenticate, markAllAlertsAsRead);

/**
 * @route   DELETE /api/candidates/alerts/cleanup
 * @desc    Delete old alerts (30+ days)
 * @access  Private
 */
router.delete('/alerts/cleanup', authenticate, deleteOldAlerts);

/**
 * @route   PUT /api/candidates/alerts/:alertId/read
 * @desc    Mark specific alert as read
 * @access  Private
 */
router.put('/alerts/:alertId/read', authenticate, markAlertAsRead);

// =============================================
// DYNAMIC ID ROUTE - Must come LAST
// =============================================

/**
 * @swagger
 * /api/candidates/{id}:
 *   get:
 *     summary: Get candidate by ID with complete information
 *     tags: [Candidates]
 *     security:
 *       - bearerAuth: []
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         schema:
 *           type: string
 *         description: Candidate MongoDB ObjectId
 *         example: 507f1f77bcf86cd799439011
 *     responses:
 *       200:
 *         description: Candidate data retrieved successfully
 *         content:
 *           application/json:
 *             schema:
 *               type: object
 *               properties:
 *                 success:
 *                   type: boolean
 *                 data:
 *                   $ref: '#/components/schemas/CandidateProfile'
 *       401:
 *         description: Not authenticated
 *       404:
 *         description: Candidate not found
 */
router.get('/:id', authenticate, getCandidateById);

export default router;
