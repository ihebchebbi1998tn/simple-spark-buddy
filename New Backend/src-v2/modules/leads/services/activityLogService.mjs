/**
 * Activity Log Service
 * Handles all activity log operations and analytics
 */

import LeadActivityLog from '../models/LeadActivityLog.mjs';
import crypto from 'crypto';

const hashIP = (ip) => crypto.createHash('sha256').update(ip).digest('hex');

export const activityLogService = {
    /**
     * Log activity (async, non-blocking)
     */
    async logActivity(data) {
        try {
            // Hash IP for long-term storage while keeping raw IP temporarily
            if (data.ip_address) {
                data.ip_hash = hashIP(data.ip_address);
            }

            const log = new LeadActivityLog(data);
            await log.save().catch(err => console.error('[ActivityLog Error]', err));
            return log;
        } catch (error) {
            console.error('[ActivityLog Error]', error);
            // Don't throw - logging should never break application flow
            return null;
        }
    },

    /**
     * Get activity log by ID
     */
    async getActivityLogById(id) {
        const log = await LeadActivityLog.findById(id);

        if (!log) {
            const error = new Error('Activity log not found');
            error.code = 'ACTIVITY_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }
        return log;
    },

    /**
     * Get all activities for a lead (paginated)
     */
    async getLeadActivity(leadId, page = 1, limit = 50, filters = {}) {
        const skip = (page - 1) * limit;
        const query = { lead_id: leadId };

        // Apply optional filters
        if (filters.action_type) query.action_type = filters.action_type;
        if (filters.source) query.source = filters.source;
        if (filters.device_type) query.device_type = filters.device_type;

        const [logs, total] = await Promise.all([
            LeadActivityLog.find(query)
                .sort({ timestamp: -1 })
                .skip(skip)
                .limit(limit)
                .select('-ip_address') // Don't return raw IP for privacy
                .lean(),
            LeadActivityLog.countDocuments(query)
        ]);

        return {
            data: logs,
            pagination: {
                page,
                limit,
                total,
                pages: Math.ceil(total / limit)
            }
        };
    },

    /**
     * Get activities by session
     */
    async getSessionActivity(sessionId, page = 1, limit = 100) {
        const skip = (page - 1) * limit;

        const [logs, total] = await Promise.all([
            LeadActivityLog.find({ session_id: sessionId })
                .sort({ timestamp: -1 })
                .skip(skip)
                .limit(limit)
                .select('-ip_address')
                .lean(),
            LeadActivityLog.countDocuments({ session_id: sessionId })
        ]);

        return {
            data: logs,
            pagination: {
                page,
                limit,
                total,
                pages: Math.ceil(total / limit)
            }
        };
    },

    /**
     * Get statistics for lead
     */
    async getLeadStats(leadId) {
        const pipeline = [
            {
                $match: { lead_id: leadId }
            },
            {
                $group: {
                    _id: null,
                    total_actions: { $sum: 1 },
                    avg_response_time: { $avg: '$response_time_ms' },
                    max_response_time: { $max: '$response_time_ms' },
                    by_action_type: {
                        $push: { action: '$action_type', count: { $sum: 1 } }
                    },
                    by_source: {
                        $push: { source: '$source', count: { $sum: 1 } }
                    },
                    by_device: {
                        $push: { device: '$device_type', count: { $sum: 1 } }
                    },
                    sessions: { $addToSet: '$session_id' }
                }
            }
        ];

        const result = await LeadActivityLog.aggregate(pipeline);

        if (result.length === 0) {
            return {
                total_actions: 0,
                avg_response_time: 0,
                max_response_time: 0,
                unique_sessions: 0
            };
        }

        return {
            total_actions: result[0].total_actions,
            avg_response_time: Math.round(result[0].avg_response_time || 0),
            max_response_time: result[0].max_response_time || 0,
            unique_sessions: result[0].sessions.length
        };
    },

    /**
     * Get failed login attempts for a lead
     */
    async getFailedLogins(leadId, hours = 24) {
        const cutoffTime = new Date(Date.now() - hours * 60 * 60 * 1000);

        return await LeadActivityLog.find({
            lead_id: leadId,
            action_type: 6,  // Login failed
            timestamp: { $gte: cutoffTime }
        })
            .sort({ timestamp: -1 })
            .select('-ip_address')
            .lean();
    },

    /**
     * Detect suspicious activity patterns
     * Returns suspicious activities matching multiple criteria
     */
    async detectSuspiciousActivity(leadId) {
        const oneHourAgo = new Date(Date.now() - 60 * 60 * 1000);

        // Check for multiple failed logins
        const failedLogins = await LeadActivityLog.countDocuments({
            lead_id: leadId,
            action_type: 6,
            timestamp: { $gte: oneHourAgo }
        });

        // Check for unusual activity sources
        const sources = await LeadActivityLog.distinct('source', {
            lead_id: leadId,
            timestamp: { $gte: oneHourAgo }
        });

        // Check for rapid actions (possible bot)
        const recentActions = await LeadActivityLog.find({
            lead_id: leadId,
            timestamp: { $gte: new Date(Date.now() - 5 * 60 * 1000) }
        }).countDocuments();

        return {
            suspicious: {
                excessive_failed_logins: failedLogins > 3,
                unusual_sources: sources.length > 2,
                rapid_activity: recentActions > 10
            },
            details: {
                failed_login_count: failedLogins,
                unique_sources: sources.length,
                recent_actions: recentActions
            }
        };
    },

    /**
     * Get analytics for activity logs
     */
    async getAnalytics(leadId = null, startDate = null, endDate = null) {
        const match = {};

        if (leadId) match.lead_id = leadId;
        if (startDate || endDate) {
            match.timestamp = {};
            if (startDate) match.timestamp.$gte = new Date(startDate);
            if (endDate) match.timestamp.$lte = new Date(endDate);
        }

        const pipeline = [
            { $match: match },
            {
                $group: {
                    _id: null,
                    total_actions: { $sum: 1 },
                    avg_response_time: { $avg: '$response_time_ms' },
                    by_action_type: {
                        $push: { type: '$action_type', count: { $sum: 1 } }
                    },
                    by_source: {
                        $push: { source: '$source', count: { $sum: 1 } }
                    },
                    by_device: {
                        $push: { device: '$device_type', count: { $sum: 1 } }
                    },
                    authenticated_percentage: {
                        $sum: { $cond: ['$is_authenticated', 1, 0] }
                    }
                }
            }
        ];

        const result = await LeadActivityLog.aggregate(pipeline);

        return result[0] || {
            total_actions: 0,
            avg_response_time: 0
        };
    },

    /**
     * Get conversion funnel (registration flow)
     */
    async getConversionFunnel(leadId) {
        const actions = [
            { type: 1, label: 'Registration Started' },
            { type: 3, label: 'Email Verified' },
            { type: 4, label: 'Phone Verified' },
            { type: 13, label: 'Profile Updated' },
            { type: 17, label: 'Skill Added' },
            { type: 5, label: 'Login Success' }
        ];

        const funnel = [];
        for (const action of actions) {
            const exists = await LeadActivityLog.findOne({
                lead_id: leadId,
                action_type: action.type
            });

            funnel.push({
                step: action.label,
                completed: !!exists,
                timestamp: exists ? exists.timestamp : null
            });
        }

        return funnel;
    },

    /**
     * Get activity by IP address (for security)
     */
    async getActivityByIP(ipAddress, hours = 24) {
        const ipHash = hashIP(ipAddress);
        const cutoffTime = new Date(Date.now() - hours * 60 * 60 * 1000);

        return await LeadActivityLog.find({
            ip_hash: ipHash,
            timestamp: { $gte: cutoffTime }
        })
            .sort({ timestamp: -1 })
            .select('-ip_address')
            .lean();
    },

    /**
     * Get suspicious IPs
     */
    async getSuspiciousIPs(minutes = 60) {
        const cutoffTime = new Date(Date.now() - minutes * 60 * 1000);

        const pipeline = [
            {
                $match: {
                    timestamp: { $gte: cutoffTime },
                    action_type: 6  // Failed logins
                }
            },
            {
                $group: {
                    _id: '$ip_hash',
                    count: { $sum: 1 },
                    leads: { $addToSet: '$lead_id' }
                }
            },
            {
                $match: { count: { $gte: 5 } }
            },
            {
                $sort: { count: -1 }
            }
        ];

        return await LeadActivityLog.aggregate(pipeline);
    },

    /**
     * Clean up old activity logs (for GDPR compliance)
     */
    async cleanupOldLogs(daysOld = 180) {
        const cutoffDate = new Date(Date.now() - daysOld * 24 * 60 * 60 * 1000);

        const result = await LeadActivityLog.deleteMany({
            timestamp: { $lt: cutoffDate }
        });

        return {
            deletedCount: result.deletedCount
        };
    },

    /**
     * Remove raw IPs older than specified days (GDPR compliance)
     */
    async removeOldRawIPs(daysOld = 30) {
        const cutoffDate = new Date(Date.now() - daysOld * 24 * 60 * 60 * 1000);

        const result = await LeadActivityLog.updateMany(
            { timestamp: { $lt: cutoffDate }, ip_address: { $exists: true } },
            { $unset: { ip_address: '' } }
        );

        return {
            modifiedCount: result.modifiedCount
        };
    }
};

export default activityLogService;
