/**
 * Activity Log Controller
 * Handles HTTP requests for activity log operations
 */

import { validateActivityLogCreation, validateActivityLogQuery, getActionTypeLabel } from '../validators/activityLogValidator.mjs';
import activityLogService from '../services/activityLogService.mjs';

export const activityLogController = {
    /**
     * Log activity
     * POST /activity-logs
     */
    async log(req, res) {
        try {
            const { isValid, errors } = validateActivityLogCreation(req.body);

            if (!isValid) {
                return res.status(400).json({
                    success: false,
                    message: 'Validation error',
                    errors
                });
            }

            const log = await activityLogService.logActivity(req.body);

            return res.status(201).json({
                success: true,
                message: 'Activity logged',
                data: log
            });
        } catch (error) {
            console.error('[Activity Log Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to log activity',
                error: error.message
            });
        }
    },

    /**
     * Get activity log by ID
     * GET /activity-logs/:id
     */
    async getById(req, res) {
        try {
            const log = await activityLogService.getActivityLogById(req.params.id);

            return res.status(200).json({
                success: true,
                data: log
            });
        } catch (error) {
            if (error.code === 'ACTIVITY_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Activity log not found'
                });
            }

            console.error('[Activity Get Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch activity log',
                error: error.message
            });
        }
    },

    /**
     * Get activities for a lead with pagination
     * GET /activity-logs/lead/:leadId
     */
    async getByLeadId(req, res) {
        try {
            const { page, limit, isValid, errors } = validateActivityLogQuery(req.query);

            if (!isValid) {
                return res.status(400).json({
                    success: false,
                    message: 'Invalid query parameters',
                    errors
                });
            }

            const filters = {};
            if (req.query.action_type) filters.action_type = parseInt(req.query.action_type);
            if (req.query.source) filters.source = req.query.source;
            if (req.query.device_type) filters.device_type = parseInt(req.query.device_type);

            const result = await activityLogService.getLeadActivity(
                req.params.leadId,
                page,
                limit,
                filters
            );

            return res.status(200).json({
                success: true,
                data: result.data.map(log => ({
                    ...log,
                    action_label: getActionTypeLabel(log.action_type)
                })),
                pagination: result.pagination
            });
        } catch (error) {
            console.error('[Lead Activities Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch activities',
                error: error.message
            });
        }
    },

    /**
     * Get activities by session
     * GET /activity-logs/session/:sessionId
     */
    async getBySessionId(req, res) {
        try {
            const { page, limit, isValid, errors } = validateActivityLogQuery(req.query);

            if (!isValid) {
                return res.status(400).json({
                    success: false,
                    message: 'Invalid query parameters',
                    errors
                });
            }

            const result = await activityLogService.getSessionActivity(
                req.params.sessionId,
                page,
                limit
            );

            return res.status(200).json({
                success: true,
                data: result.data.map(log => ({
                    ...log,
                    action_label: getActionTypeLabel(log.action_type)
                })),
                pagination: result.pagination
            });
        } catch (error) {
            console.error('[Session Activities Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch session activities',
                error: error.message
            });
        }
    },

    /**
     * Get lead statistics
     * GET /activity-logs/stats/:leadId
     */
    async getStats(req, res) {
        try {
            const stats = await activityLogService.getLeadStats(req.params.leadId);

            return res.status(200).json({
                success: true,
                data: stats
            });
        } catch (error) {
            console.error('[Stats Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch statistics',
                error: error.message
            });
        }
    },

    /**
     * Get failed login attempts
     * GET /activity-logs/security/failed-logins/:leadId
     */
    async getFailedLogins(req, res) {
        try {
            const hours = parseInt(req.query.hours) || 24;
            const logs = await activityLogService.getFailedLogins(req.params.leadId, hours);

            return res.status(200).json({
                success: true,
                count: logs.length,
                data: logs
            });
        } catch (error) {
            console.error('[Failed Logins Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch failed login attempts',
                error: error.message
            });
        }
    },

    /**
     * Detect suspicious activity
     * GET /activity-logs/security/suspicious/:leadId
     */
    async detectSuspicious(req, res) {
        try {
            const suspicious = await activityLogService.detectSuspiciousActivity(req.params.leadId);

            return res.status(200).json({
                success: true,
                data: suspicious
            });
        } catch (error) {
            console.error('[Suspicious Activity Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to detect suspicious activity',
                error: error.message
            });
        }
    },

    /**
     * Get analytics for activity logs
     * GET /activity-logs/analytics
     */
    async getAnalytics(req, res) {
        try {
            const { leadId, startDate, endDate } = req.query;

            const analytics = await activityLogService.getAnalytics(leadId, startDate, endDate);

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
     * Get conversion funnel
     * GET /activity-logs/funnel/conversion/:leadId
     */
    async getConversionFunnel(req, res) {
        try {
            const funnel = await activityLogService.getConversionFunnel(req.params.leadId);

            return res.status(200).json({
                success: true,
                data: funnel
            });
        } catch (error) {
            console.error('[Conversion Funnel Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch conversion funnel',
                error: error.message
            });
        }
    },

    /**
     * Get activity by IP address
     * GET /activity-logs/security/ip/:ipAddress
     */
    async getByIP(req, res) {
        try {
            const hours = parseInt(req.query.hours) || 24;
            const logs = await activityLogService.getActivityByIP(req.params.ipAddress, hours);

            return res.status(200).json({
                success: true,
                count: logs.length,
                data: logs
            });
        } catch (error) {
            console.error('[IP Activity Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch activity by IP',
                error: error.message
            });
        }
    },

    /**
     * Get suspicious IPs
     * GET /activity-logs/security/suspicious-ips
     */
    async getSuspiciousIPs(req, res) {
        try {
            const minutes = parseInt(req.query.minutes) || 60;
            const ips = await activityLogService.getSuspiciousIPs(minutes);

            return res.status(200).json({
                success: true,
                count: ips.length,
                data: ips
            });
        } catch (error) {
            console.error('[Suspicious IPs Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch suspicious IPs',
                error: error.message
            });
        }
    },

    /**
     * Cleanup old logs
     * POST /activity-logs/cleanup
     */
    async cleanup(req, res) {
        try {
            const daysOld = parseInt(req.body.daysOld) || 180;
            const result = await activityLogService.cleanupOldLogs(daysOld);

            return res.status(200).json({
                success: true,
                message: `Cleaned up ${result.deletedCount} activity logs`,
                deletedCount: result.deletedCount
            });
        } catch (error) {
            console.error('[Cleanup Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to cleanup logs',
                error: error.message
            });
        }
    },

    /**
     * Remove old raw IP addresses
     * POST /activity-logs/remove-ips
     */
    async removeOldIPs(req, res) {
        try {
            const daysOld = parseInt(req.body.daysOld) || 30;
            const result = await activityLogService.removeOldRawIPs(daysOld);

            return res.status(200).json({
                success: true,
                message: `Removed raw IPs from ${result.modifiedCount} activity logs`,
                modifiedCount: result.modifiedCount
            });
        } catch (error) {
            console.error('[Remove IPs Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to remove old IPs',
                error: error.message
            });
        }
    }
};

export default activityLogController;
