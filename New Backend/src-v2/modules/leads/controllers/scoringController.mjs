/**
 * Scoring Controller
 * Handles HTTP requests for scoring operations
 * Supports both auto-calculated (read-only) and manual (update) fields
 */

import { validateScoringUpdate, validateScoringQuery } from '../validators/scoringValidator.mjs';
import scoringService from '../services/scoringService.mjs';

export const scoringController = {
    /**
     * Get full scoring data for a lead
     * GET /leads/:leadId/scoring
     */
    async getScoringByLeadId(req, res) {
        try {
            const scoring = await scoringService.getScoringByLeadId(req.params.leadId);

            return res.status(200).json({
                success: true,
                data: scoring
            });
        } catch (error) {
            if (error.code === 'SCORING_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Scoring data not found'
                });
            }

            console.error('[Scoring Get Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch scoring data',
                error: error.message
            });
        }
    },

    /**
     * Get scoring overview (auto-calculated fields only)
     * GET /leads/:leadId/scoring/overview
     */
    async getOverview(req, res) {
        try {
            const overview = await scoringService.getScoringOverview(req.params.leadId);

            return res.status(200).json({
                success: true,
                data: overview
            });
        } catch (error) {
            if (error.code === 'SCORING_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Scoring data not found'
                });
            }

            console.error('[Overview Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch overview',
                error: error.message
            });
        }
    },

    /**
     * Get detailed scoring breakdown
     * GET /leads/:leadId/scoring/breakdown
     */
    async getBreakdown(req, res) {
        try {
            const breakdown = await scoringService.getScoringBreakdown(req.params.leadId);

            return res.status(200).json({
                success: true,
                data: breakdown
            });
        } catch (error) {
            if (error.code === 'SCORING_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Scoring data not found'
                });
            }

            console.error('[Breakdown Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch breakdown',
                error: error.message
            });
        }
    },

    /**
     * Update manual scoring fields only
     * Auto-calculated fields are protected
     * PUT /leads/:leadId/scoring
     */
    async updateManuals(req, res) {
        try {
            const { isValid, errors } = validateScoringUpdate(req.body);

            if (!isValid) {
                return res.status(400).json({
                    success: false,
                    message: 'Validation error',
                    errors
                });
            }

            const scoring = await scoringService.updateScoringManuals(
                req.params.leadId,
                req.body
            );

            return res.status(200).json({
                success: true,
                message: 'Scoring updated',
                data: scoring
            });
        } catch (error) {
            if (error.code === 'SCORING_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Scoring data not found'
                });
            }

            console.error('[Scoring Update Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to update scoring',
                error: error.message
            });
        }
    },

    /**
     * Get scoring comparison with other leads
     * GET /leads/:leadId/scoring/comparison
     */
    async getComparison(req, res) {
        try {
            const limit = parseInt(req.query.limit) || 10;
            const comparison = await scoringService.getScoringComparison(
                req.params.leadId,
                limit
            );

            return res.status(200).json({
                success: true,
                data: comparison
            });
        } catch (error) {
            if (error.code === 'SCORING_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Scoring data not found'
                });
            }

            console.error('[Comparison Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch comparison',
                error: error.message
            });
        }
    },

    /**
     * Flag scoring for manual review
     * POST /leads/:leadId/scoring/flag-review
     */
    async flagForReview(req, res) {
        try {
            const scoring = await scoringService.flagForReview(
                req.params.leadId,
                req.body.reason
            );

            return res.status(200).json({
                success: true,
                message: 'Scoring flagged for review',
                data: scoring
            });
        } catch (error) {
            if (error.code === 'SCORING_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Scoring data not found'
                });
            }

            console.error('[Flag Review Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to flag for review',
                error: error.message
            });
        }
    },

    /**
     * Clear review flag
     * POST /leads/:leadId/scoring/clear-flag
     */
    async clearReviewFlag(req, res) {
        try {
            const scoring = await scoringService.clearReviewFlag(req.params.leadId);

            return res.status(200).json({
                success: true,
                message: 'Review flag cleared',
                data: scoring
            });
        } catch (error) {
            if (error.code === 'SCORING_NOT_FOUND') {
                return res.status(404).json({
                    success: false,
                    message: 'Scoring data not found'
                });
            }

            console.error('[Clear Flag Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to clear flag',
                error: error.message
            });
        }
    },

    /**
     * Get all leads flagged for review
     * GET /scoring/flagged-for-review
     */
    async getFlaggedForReview(req, res) {
        try {
            const { page, limit, isValid, errors } = validateScoringQuery(req.query);

            if (!isValid) {
                return res.status(400).json({
                    success: false,
                    message: 'Invalid query parameters',
                    errors
                });
            }

            const result = await scoringService.getFlaggedForReview(page, limit);

            return res.status(200).json({
                success: true,
                data: result.data,
                pagination: result.pagination
            });
        } catch (error) {
            console.error('[Flagged Review Error]', error);
            return res.status(500).json({
                success: false,
                message: 'Failed to fetch flagged scores',
                error: error.message
            });
        }
    },

    /**
     * Get scoring analytics
     * GET /scoring/analytics
     */
    async getAnalytics(req, res) {
        try {
            const analytics = await scoringService.getScoringAnalytics();

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
    }
};

export default scoringController;
