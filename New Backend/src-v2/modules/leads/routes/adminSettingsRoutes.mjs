/**
 * Admin Settings Routes
 * Platform-level settings endpoints (not nested under specific leads)
 * All routes require admin authorization
 */

import express from 'express';
import { settingsController } from '../controllers/settingsController.mjs';
import { authenticate, authorize } from '../../../middleware/auth.mjs';

const router = express.Router();

// All routes require authentication and admin authorization
router.use(authenticate);
router.use(authorize('admin'));

/**
 * ADMIN ANALYTICS & QUERIES
 */

// Get settings analytics (overall statistics)
router.get('/analytics', settingsController.getAnalytics);

// Get all settings by account status
router.get('/by-status/:status', settingsController.getByStatus);

export default router;
