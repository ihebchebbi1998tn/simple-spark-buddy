/**
 * Notification Routes
 * Nested under lead resource
 * 
 * POST   /leads/:leadId/notifications                  - Create notification
 * GET    /leads/:leadId/notifications                  - Get lead notifications (paginated)
 * GET    /leads/:leadId/notifications/unread           - Get unread count
 * GET    /leads/:leadId/notifications/:notificationId  - Get single notification
 * PUT    /leads/:leadId/notifications/:notificationId  - Update notification
 * POST   /leads/:leadId/notifications/:notificationId/mark-read     - Mark as read
 * POST   /leads/:leadId/notifications/:notificationId/mark-clicked  - Mark as clicked
 * DELETE /leads/:leadId/notifications/:notificationId  - Delete notification
 * GET    /notifications/analytics                      - Get analytics
 * GET    /notifications/failed/retry                   - Get failed for retry
 */

import express from 'express';
import notificationController from '../controllers/notificationController.mjs';

const router = express.Router({ mergeParams: true });

// POST /leads/:leadId/notifications - Create
router.post('/', notificationController.create);

// GET /leads/:leadId/notifications - List by lead
router.get('/', notificationController.getByLeadId);

// GET /leads/:leadId/notifications/unread - Unread count
router.get('/unread', notificationController.getUnreadCount);

// GET /notifications/:id - Get single
router.get('/:notificationId', notificationController.getById);

// PUT /leads/:leadId/notifications/:notificationId - Update
router.put('/:notificationId', notificationController.update);

// POST /leads/:leadId/notifications/:notificationId/mark-read
router.post('/:notificationId/mark-read', notificationController.markAsRead);

// POST /leads/:leadId/notifications/:notificationId/mark-clicked
router.post('/:notificationId/mark-clicked', notificationController.markAsClicked);

// DELETE /leads/:leadId/notifications/:notificationId
router.delete('/:notificationId', notificationController.delete);

export default router;
