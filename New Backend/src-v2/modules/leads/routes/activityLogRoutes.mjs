/**
 * Activity Log Routes
 * Nested under lead resource
 * 
 * POST   /activity-logs                       - Log activity
 * GET    /leads/:leadId/activity-logs         - Get lead activities (paginated)
 * GET    /leads/:leadId/activity-logs/:id     - Get single activity
 * GET    /activity-logs/session/:sessionId    - Get by session
 * GET    /leads/:leadId/activity-logs/stats   - Get statistics
 * GET    /security/failed-logins/:leadId      - Failed logins
 * GET    /security/suspicious/:leadId         - Suspicious activity
 * GET    /security/ip/:ipAddress              - By IP address
 * GET    /security/suspicious-ips             - Suspicious IPs
 * GET    /analytics                           - Analytics
 * GET    /funnel/conversion/:leadId           - Conversion funnel
 * POST   /cleanup                             - Cleanup old logs
 * POST   /remove-ips                          - Remove old IPs
 */

import express from 'express';
import activityLogController from '../controllers/activityLogController.mjs';

const router = express.Router({ mergeParams: true });

// POST / - Log activity
router.post('/', activityLogController.log);

// GET / - List by lead
router.get('/', activityLogController.getByLeadId);

// GET /session/:sessionId - By session
router.get('/session/:sessionId', activityLogController.getBySessionId);

// GET /stats - Lead statistics
router.get('/stats', activityLogController.getStats);

// GET /security/failed-logins/:leadId
router.get('/security/failed-logins/:leadId', activityLogController.getFailedLogins);

// GET /security/suspicious/:leadId
router.get('/security/suspicious/:leadId', activityLogController.detectSuspicious);

// GET /security/ip/:ipAddress
router.get('/security/ip/:ipAddress', activityLogController.getByIP);

// GET /security/suspicious-ips
router.get('/security/suspicious-ips', activityLogController.getSuspiciousIPs);

// GET /analytics
router.get('/analytics', activityLogController.getAnalytics);

// GET /funnel/conversion/:leadId
router.get('/funnel/conversion/:leadId', activityLogController.getConversionFunnel);

// POST /cleanup
router.post('/cleanup', activityLogController.cleanup);

// POST /remove-ips
router.post('/remove-ips', activityLogController.removeOldIPs);

// GET /:id - Get single activity (must be last to avoid route conflicts)
router.get('/:id', activityLogController.getById);

export default router;
