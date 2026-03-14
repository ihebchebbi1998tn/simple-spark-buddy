/**
 * Settings Routes
 * Nested under /leads/:leadId/settings
 */

import express from 'express';
import { settingsController } from '../controllers/settingsController.mjs';
import { authenticate, authorize } from '../../../middleware/auth.mjs';

const router = express.Router({ mergeParams: true });

// All routes require authentication
router.use(authenticate);

/**
 * USER ROUTES (user-editable settings)
 */

// Get user accessibility info - shows what fields they can edit
router.get('/accessibility', settingsController.getAccessibility);

// Get user's own settings (excludes admin fields)
router.get('/', settingsController.getSettings);

// Update user-editable settings
router.put('/', settingsController.updateUserSettings);

// Update notification preference for a channel
router.put('/notifications/:channel', settingsController.updateNotificationPreference);

// Toggle 2FA
router.post('/two-factor', settingsController.toggle2FA);

// Reset settings to defaults
router.post('/reset', settingsController.resetToDefaults);

/**
 * ADMIN ROUTES (admin-controlled settings)
 */

// Update admin-controlled settings (e.g., account_status, subscription_status)
router.put('/admin', authorize('admin'), settingsController.updateAdminSettings);

// Suspend account
router.post('/suspend', authorize('admin'), settingsController.suspendAccount);

// Restore account
router.post('/restore', authorize('admin'), settingsController.restoreAccount);

export default router;
