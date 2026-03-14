/**
 * Notification Controller
 * Handles HTTP requests for notification operations
 */

import { validateNotificationCreation, validateNotificationQuery } from '../validators/notificationValidator.mjs';
import notificationService from '../services/notificationService.mjs';

export const notificationController = {
    /**
     * Create a new notification
     * POST /notifications
     */
    async create(req, res) {
        try {
            // Add lead_id from URL parameter
            const notificationData = {
                ...req.body,
                lead_id: req.params.leadId
            };

            const { isValid, errors } = validateNotificationCreation(notificationData);

            if (!isValid) {
                return res.status(400).json({
                    success: false,
                    message: 'Validation error',
                    errors
                });
            }

            const notification = await notificationService.createNotification(notificationData);

            return res.status(201).json({
                success: true,
                message: 'Notification created',
                data: notification
            });
        } catch (error) {
            console.error('[Notification Create Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to create notification',
                error: error.message
            });
        }
    },

    /**
     * Get notification by ID
     * GET /notifications/:notificationId
     */
    async getById(req, res) {
        try {
            const notification = await notificationService.getNotificationById(req.params.notificationId);

            return res.status(200).json({
                success: true,
                data: notification
            });
        } catch (error) {
            if (error.code === 'NOTIFICATION_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Notification not found'
                });
            }

            console.error('[Notification Get Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch notification',
                error: error.message
            });
        }
    },

    /**
     * Get notifications for a lead with pagination
     * GET /notifications/lead/:leadId
     */
    async getByLeadId(req, res) {
        try {
            const { page, limit, isValid, errors } = validateNotificationQuery(req.query);

            if (!isValid) {
                return res.status(400).json({
                    success: false,
                    message: 'Invalid query parameters',
                    errors
                });
            }

            const result = await notificationService.getNotificationsByLeadId(
                req.params.leadId,
                page,
                limit
            );

            return res.status(200).json({
                success: true,
                data: result.data,
                pagination: result.pagination
            });
        } catch (error) {
            console.error('[Lead Notifications Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch notifications',
                error: error.message
            });
        }
    },

    /**
     * Get unread notification count
     * GET /notifications/lead/:leadId/unread
     */
    async getUnreadCount(req, res) {
        try {
            const count = await notificationService.getUnreadCount(req.params.leadId);

            return res.status(200).json({
                success: true,
                count
            });
        } catch (error) {
            console.error('[Unread Count Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch unread count',
                error: error.message
            });
        }
    },

    /**
     * Update notification
     * PUT /notifications/:notificationId
     */
    async update(req, res) {
        try {
            const notification = await notificationService.updateNotification(req.params.notificationId, req.body);

            return res.status(200).json({
                success: true,
                message: 'Notification updated',
                data: notification
            });
        } catch (error) {
            if (error.code === 'NOTIFICATION_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Notification not found'
                });
            }

            console.error('[Notification Update Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to update notification',
                error: error.message
            });
        }
    },

    /**
     * Mark notification as read
     * POST /notifications/:notificationId/mark-read
     */
    async markAsRead(req, res) {
        try {
            const notification = await notificationService.markAsRead(req.params.notificationId);

            return res.status(200).json({
                success: true,
                message: 'Notification marked as read',
                data: notification
            });
        } catch (error) {
            if (error.code === 'NOTIFICATION_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Notification not found'
                });
            }

            console.error('[Mark Read Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to mark notification as read',
                error: error.message
            });
        }
    },

    /**
     * Mark notification as clicked
     * POST /notifications/:notificationId/mark-clicked
     */
    async markAsClicked(req, res) {
        try {
            const notification = await notificationService.markAsClicked(req.params.notificationId);

            return res.status(200).json({
                success: true,
                message: 'Notification marked as clicked',
                data: notification
            });
        } catch (error) {
            if (error.code === 'NOTIFICATION_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Notification not found'
                });
            }

            console.error('[Mark Clicked Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to mark notification as clicked',
                error: error.message
            });
        }
    },

    /**
     * Delete notification
     * DELETE /notifications/:notificationId
     */
    async delete(req, res) {
        try {
            await notificationService.deleteNotification(req.params.notificationId);

            return res.status(200).json({
                success: true,
                message: 'Notification deleted'
            });
        } catch (error) {
            if (error.code === 'NOTIFICATION_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Notification not found'
                });
            }

            console.error('[Notification Delete Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to delete notification',
                error: error.message
            });
        }
    },

    /**
     * Get notification analytics
     * GET /notifications/analytics
     */
    async getAnalytics(req, res) {
        try {
            const { leadId } = req.query;
            const { startDate, endDate } = req.query;

            const analytics = await notificationService.getNotificationAnalytics(
                leadId,
                startDate,
                endDate
            );

            return res.status(200).json({
                success: true,
                data: analytics
            });
        } catch (error) {
            console.error('[Analytics Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch analytics',
                error: error.message
            });
        }
    },

    /**
     * Get failed notifications for retry
     * GET /notifications/failed/retry
     */
    async getFailedForRetry(req, res) {
        try {
            const notifications = await notificationService.getFailedNotificationsForRetry();

            return res.status(200).json({
                success: true,
                count: notifications.length,
                data: notifications
            });
        } catch (error) {
            console.error('[Failed Notifications Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch failed notifications',
                error: error.message
            });
        }
    }
};

export default notificationController;
