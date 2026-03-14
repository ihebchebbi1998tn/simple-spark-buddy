import express from 'express';
import leadRoutes from './leadRoutes.mjs';
import profileRoutes from './profileRoutes.mjs';
import skillRoutes from './skillRoutes.mjs';
import scoringRoutes from './scoringRoutes.mjs';
import notificationRoutes from './notificationRoutes.mjs';
import activityLogRoutes from './activityLogRoutes.mjs';
import settingsRoutes from './settingsRoutes.mjs';
import adminSettingsRoutes from './adminSettingsRoutes.mjs';

const router = express.Router();

/**
 * Lead module routes with nested profile and skill resources
 * 
 * Base routes:
 * - POST   /leads/register              - Register new lead
 * - POST   /leads/login                 - Login lead
 * - GET    /leads/:leadId               - Get lead by ID
 * - GET    /leads/email/:email          - Get lead by email
 * - GET    /leads/public/:publicId      - Get lead by public ID
 * - GET    /leads                       - List leads (pagination, search, filter)
 * - PUT    /leads/:leadId/profile       - Update lead profile
 * - PUT    /leads/:leadId/email         - Update lead email
 * - PUT    /leads/:leadId/password      - Update lead password
 * - POST   /leads/:leadId/verify-email  - Verify email
 * - POST   /leads/:leadId/verify-phone  - Verify phone
 * - DELETE /leads/:leadId               - Delete lead
 * 
 * Profile routes (nested under lead):
 * - GET    /leads/:leadId/profile                    - Get profile
 * - PUT    /leads/:leadId/profile                    - Update full profile
 * - PUT    /leads/:leadId/profile/activities         - Update activities
 * - PUT    /leads/:leadId/profile/work-preferences   - Update work preferences
 * - PUT    /leads/:leadId/profile/compensation       - Update compensation
 * 
 * Skills routes (nested under lead):
 * - GET    /leads/:leadId/skills                            - Get all skills
 * - GET    /leads/:leadId/skills/language-tests            - Get language tests
 * - GET    /leads/:leadId/skills/language/:code/method/:method - Get specific language test
 * - GET    /leads/:leadId/skills/completed                 - Get completed skills
 * - GET    /leads/:leadId/skills/stats                     - Get skill statistics
 * - POST   /leads/:leadId/skills/language                  - Create/update language test
 * - POST   /leads/:leadId/skills/technical                 - Create technical skill
 * - POST   /leads/:leadId/skills/certification             - Create certification
 * - PUT    /skills/:skillId                         - Update skill
 * - DELETE /skills/:skillId                         - Delete skill
 * 
 * Settings routes (nested under lead):
 * - GET    /leads/:leadId/settings                         - Get user settings
 * - GET    /leads/:leadId/settings/accessibility          - Get accessible fields
 * - PUT    /leads/:leadId/settings                         - Update user-editable settings
 * - PUT    /leads/:leadId/settings/notifications/:channel  - Update notification channel
 * - POST   /leads/:leadId/settings/two-factor              - Toggle 2FA
 * - POST   /leads/:leadId/settings/reset                   - Reset settings
 * - PUT    /leads/:leadId/settings/admin                   - [Admin] Update admin fields
 * - POST   /leads/:leadId/settings/suspend                 - [Admin] Suspend account
 * - POST   /leads/:leadId/settings/restore                 - [Admin] Restore account
 * 
 * Admin settings routes (platform-level):
 * - GET    /settings/analytics                     - [Admin] Get analytics
 * - GET    /settings/by-status/:status             - [Admin] Get settings by account status
 */

// Lead routes (primary)
router.use('/leads', leadRoutes);

// Profile routes (nested under /leads/:leadId/profile)
router.use('/leads/:leadId/profile', profileRoutes);

// Skills routes (nested under /leads/:leadId/skills and /skills)
router.use('/leads/:leadId/skills', skillRoutes);
router.use('/skills', skillRoutes);

// Scoring routes (nested under /leads/:leadId/scoring)
router.use('/leads/:leadId/scoring', scoringRoutes);

// Notification routes (nested under /leads/:leadId/notifications)
router.use('/leads/:leadId/notifications', notificationRoutes);

// Activity log routes (nested under /activity-logs and /leads/:leadId/activity-logs)
router.use('/leads/:leadId/activity-logs', activityLogRoutes);
router.use('/activity-logs', activityLogRoutes);

// Settings routes (nested under /leads/:leadId/settings)
router.use('/leads/:leadId/settings', settingsRoutes);

// Admin settings routes (platform-level /settings routes)
router.use('/settings', adminSettingsRoutes);

export default router;
