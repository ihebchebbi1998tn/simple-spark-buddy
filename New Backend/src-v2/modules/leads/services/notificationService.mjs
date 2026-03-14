/**
 * Notification Service
 * Handles all notification CRUD operations and lifecycle management
 */

import LeadNotification from '../models/LeadNotification.mjs';

export const notificationService = {
    /**
     * Create new notification
     */
    async createNotification(data) {
        const notification = new LeadNotification(data);
        return await notification.save();
    },

    /**
     * Get notification by ID
     */
    async getNotificationById(id) {
        const notification = await LeadNotification.findById(id)
            .populate('lead_id', 'email phone name');

        if (!notification) {
            const error = new Error('Notification not found');
            error.code = 'NOTIFICATION_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }
        return notification;
    },

    /**
     * Get all notifications for a lead (paginated)
     */
    async getNotificationsByLeadId(leadId, page = 1, limit = 50) {
        const skip = (page - 1) * limit;

        const [notifications, total] = await Promise.all([
            LeadNotification.find({ lead_id: leadId })
                .sort({ created_at: -1 })
                .skip(skip)
                .limit(limit)
                .lean(),
            LeadNotification.countDocuments({ lead_id: leadId })
        ]);

        return {
            data: notifications,
            pagination: {
                page,
                limit,
                total,
                pages: Math.ceil(total / limit)
            }
        };
    },

    /**
     * Get unread count for a lead
     */
    async getUnreadCount(leadId) {
        return await LeadNotification.countDocuments({
            lead_id: leadId,
            status: 1  // Pending
        });
    },

    /**
     * Update notification
     */
    async updateNotification(id, updateData) {
        const notification = await LeadNotification.findByIdAndUpdate(
            id,
            { $set: updateData },
            { new: true, runValidators: true }
        );

        if (!notification) {
            const error = new Error('Notification not found');
            error.code = 'NOTIFICATION_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }
        return notification;
    },

    /**
     * Mark notification as read
     */
    async markAsRead(id) {
        return await this.updateNotification(id, {
            status: 4,  // Read
            read_at: new Date()
        });
    },

    /**
     * Mark notification as clicked
     */
    async markAsClicked(id) {
        return await this.updateNotification(id, {
            status: 5,  // Clicked
            clicked_at: new Date()
        });
    },

    /**
     * Mark notification as delivered
     */
    async markAsDelivered(id) {
        return await this.updateNotification(id, {
            status: 3,  // Delivered
            delivered_at: new Date()
        });
    },

    /**
     * Mark notification as sent
     */
    async markAsSent(id, providerMessageId = null) {
        const update = {
            status: 2,  // Sent
            sent_at: new Date()
        };
        if (providerMessageId) {
            update.provider_message_id = providerMessageId;
        }
        return await this.updateNotification(id, update);
    },

    /**
     * Mark notification as failed
     */
    async markAsFailed(id, errorReason, providerResponse = null) {
        const update = {
            status: 6,  // Failed
            provider_response: providerResponse || { error: errorReason }
        };
        return await this.updateNotification(id, update);
    },

    /**
     * Mark notification as bounced
     */
    async markAsBounced(id) {
        return await this.updateNotification(id, {
            status: 7  // Bounced
        });
    },

    /**
     * Mark notification as spam
     */
    async markAsSpam(id) {
        return await this.updateNotification(id, {
            status: 8  // Spam
        });
    },

    /**
     * Mark notification as expired
     */
    async markAsExpired(id) {
        return await this.updateNotification(id, {
            status: 10  // Expired
        });
    },

    /**
     * Delete notification
     */
    async deleteNotification(id) {
        const notification = await LeadNotification.findByIdAndDelete(id);

        if (!notification) {
            const error = new Error('Notification not found');
            error.code = 'NOTIFICATION_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }
        return notification;
    },

    /**
     * Get failed notifications ready for retry
     */
    async getFailedNotificationsForRetry() {
        const maxRetries = 3;
        const fiveMinutesAgo = new Date(Date.now() - 5 * 60 * 1000);

        return await LeadNotification.find({
            $or: [
                { status: 6, retry_count: { $lt: maxRetries }, last_retry_at: { $lt: fiveMinutesAgo } },
                { status: 6, retry_count: { $lt: maxRetries }, last_retry_at: null }
            ]
        }).sort({ created_at: 1 });
    },

    /**
     * Increment retry count
     */
    async incrementRetryCount(id) {
        return await LeadNotification.findByIdAndUpdate(
            id,
            {
                $inc: { retry_count: 1 },
                $set: { last_retry_at: new Date() }
            },
            { new: true }
        );
    },

    /**
     * Get scheduled notifications ready to send
     */
    async getScheduledNotifications() {
        const now = new Date();
        return await LeadNotification.find({
            status: 1,  // Pending
            scheduled_for: { $lte: now }
        }).sort({ scheduled_for: 1 }).limit(100);
    },

    /**
     * Get notifications by type for a lead
     */
    async getNotificationsByType(leadId, notificationType, page = 1, limit = 50) {
        const skip = (page - 1) * limit;

        const [notifications, total] = await Promise.all([
            LeadNotification.find({ lead_id: leadId, notification_type: notificationType })
                .sort({ created_at: -1 })
                .skip(skip)
                .limit(limit)
                .lean(),
            LeadNotification.countDocuments({ lead_id: leadId, notification_type: notificationType })
        ]);

        return {
            data: notifications,
            pagination: {
                page,
                limit,
                total,
                pages: Math.ceil(total / limit)
            }
        };
    },

    /**
     * Get notifications by status
     */
    async getNotificationsByStatus(status, page = 1, limit = 50) {
        const skip = (page - 1) * limit;

        const [notifications, total] = await Promise.all([
            LeadNotification.find({ status })
                .sort({ created_at: -1 })
                .skip(skip)
                .limit(limit)
                .lean(),
            LeadNotification.countDocuments({ status })
        ]);

        return {
            data: notifications,
            pagination: {
                page,
                limit,
                total,
                pages: Math.ceil(total / limit)
            }
        };
    },

    /**
     * Get analytics for notifications
     */
    async getNotificationAnalytics(leadId = null, startDate = null, endDate = null) {
        const match = {};

        if (leadId) match.lead_id = leadId;
        if (startDate || endDate) {
            match.created_at = {};
            if (startDate) match.created_at.$gte = new Date(startDate);
            if (endDate) match.created_at.$lte = new Date(endDate);
        }

        const pipeline = [
            { $match: match },
            {
                $group: {
                    _id: null,
                    total: { $sum: 1 },
                    by_status: {
                        $push: {
                            status: '$status',
                            count: { $sum: 1 }
                        }
                    },
                    by_channel: {
                        $push: {
                            channel: '$channel',
                            count: { $sum: 1 }
                        }
                    },
                    by_type: {
                        $push: {
                            type: '$notification_type',
                            count: { $sum: 1 }
                        }
                    },
                    unread: {
                        $sum: { $cond: [{ $eq: ['$status', 1] }, 1, 0] }
                    },
                    failed: {
                        $sum: { $cond: [{ $in: ['$status', [6, 7, 8, 10]] }, 1, 0] }
                    },
                    read_rate: {
                        $sum: { $cond: [{ $eq: ['$status', 4] }, 1, 0] }
                    },
                    click_rate: {
                        $sum: { $cond: [{ $eq: ['$status', 5] }, 1, 0] }
                    }
                }
            }
        ];

        const result = await LeadNotification.aggregate(pipeline);
        return result[0] || {
            total: 0,
            unread: 0,
            failed: 0,
            read_rate: 0,
            click_rate: 0
        };
    },

    /**
     * Delete old notifications (for cleanup/archival)
     */
    async deleteOldNotifications(daysOld = 180) {
        const cutoffDate = new Date(Date.now() - daysOld * 24 * 60 * 60 * 1000);

        const result = await LeadNotification.deleteMany({
            created_at: { $lt: cutoffDate }
        });

        return {
            deletedCount: result.deletedCount
        };
    }
};

export default notificationService;
