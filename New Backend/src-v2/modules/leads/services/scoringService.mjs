/**
 * Scoring Service
 * Handles scoring data operations - both auto-calculated and manual fields
 */

import LeadScoring from '../models/LeadScoring.mjs';

export const scoringService = {
    /**
     * Get scoring data for a lead
     */
    async getScoringByLeadId(leadId) {
        const scoring = await LeadScoring.findOne({ lead_id: leadId })
            .populate('lead_id', 'email name status');

        if (!scoring) {
            const error = new Error('Scoring data not found');
            error.code = 'SCORING_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }
        return scoring;
    },

    /**
     * Get scoring summary (calculated fields only)
     */
    async getScoringOverview(leadId) {
        const scoring = await LeadScoring.findOne({ lead_id: leadId })
            .select('lead_id overall_score skills_score experience_score profile_completeness availability_score language_score test_results match_quality compatibility_score');

        if (!scoring) {
            const error = new Error('Scoring data not found');
            error.code = 'SCORING_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return {
            lead_id: scoring.lead_id,
            overall_score: scoring.overall_score,
            breakdown: {
                skills: scoring.skills_score,
                experience: scoring.experience_score,
                profile_completeness: scoring.profile_completeness,
                availability: scoring.availability_score,
                language: scoring.language_score,
                test_results: scoring.test_results,
                match_quality: scoring.match_quality,
                compatibility: scoring.compatibility_score
            }
        };
    },

    /**
     * Update manual scoring fields only
     * Auto-calculated fields are protected and cannot be updated directly
     */
    async updateScoringManuals(leadId, updateData) {
        // List of auto-calculated fields that cannot be updated
        const autoCalculatedFields = [
            'overall_score',
            'skills_score',
            'experience_score',
            'profile_completeness',
            'availability_score',
            'language_score',
            'test_results',
            'match_quality',
            'compatibility_score',
            'last_calculated_at'
        ];

        // Filter out any attempt to update auto-calculated fields
        const manualUpdates = {};
        const allowedManualFields = ['notes', 'scoring_adjustments', 'flags', 'manual_ranking', 'status'];

        for (const field of allowedManualFields) {
            if (field in updateData) {
                manualUpdates[field] = updateData[field];
            }
        }

        // Add system timestamp
        manualUpdates.updated_at = new Date();

        const scoring = await LeadScoring.findOneAndUpdate(
            { lead_id: leadId },
            { $set: manualUpdates },
            { new: true, runValidators: true }
        );

        if (!scoring) {
            const error = new Error('Scoring data not found');
            error.code = 'SCORING_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return scoring;
    },

    /**
     * Get detailed scoring breakdown
     */
    async getScoringBreakdown(leadId) {
        const scoring = await LeadScoring.findOne({ lead_id: leadId });

        if (!scoring) {
            const error = new Error('Scoring data not found');
            error.code = 'SCORING_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return {
            overall: {
                score: scoring.overall_score,
                last_calculated: scoring.last_calculated_at
            },
            components: {
                skills: {
                    score: scoring.skills_score,
                    details: scoring.skills_assessment || {}
                },
                experience: {
                    score: scoring.experience_score,
                    details: scoring.experience_assessment || {}
                },
                profile: {
                    completeness: scoring.profile_completeness,
                    status: scoring.profile_status || 'incomplete'
                },
                availability: {
                    score: scoring.availability_score,
                    status: scoring.availability_status || 'unknown'
                },
                language: {
                    score: scoring.language_score,
                    fluency_level: scoring.fluency_level || 'unknown'
                }
            },
            tests: scoring.test_results || {},
            match_quality: scoring.match_quality || 0,
            compatibility: scoring.compatibility_score || 0,
            manual_overrides: {
                manual_ranking: scoring.manual_ranking,
                notes: scoring.notes,
                adjustments: scoring.scoring_adjustments || {},
                flags: scoring.flags || []
            }
        };
    },

    /**
     * Get comparisons with other leads (for ranking context)
     */
    async getScoringComparison(leadId, limit = 10) {
        const currentScoring = await LeadScoring.findOne({ lead_id: leadId })
            .select('overall_score skills_score experience_score');

        if (!currentScoring) {
            const error = new Error('Scoring data not found');
            error.code = 'SCORING_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        // Get rankings relative to others
        const betterThanCount = await LeadScoring.countDocuments({
            lead_id: { $ne: leadId },
            overall_score: { $gt: currentScoring.overall_score }
        });

        const sameScoreCount = await LeadScoring.countDocuments({
            lead_id: { $ne: leadId },
            overall_score: currentScoring.overall_score
        });

        const topLeads = await LeadScoring.find()
            .select('lead_id overall_score')
            .sort({ overall_score: -1 })
            .limit(limit)
            .lean();

        return {
            current_score: currentScoring.overall_score,
            ranking: {
                better_than: betterThanCount,
                tied_with: sameScoreCount,
                percentile: Math.round(((betterThanCount) / (betterThanCount + sameScoreCount)) * 100)
            },
            top_leads: topLeads
        };
    },

    /**
     * Flag scoring for manual review
     */
    async flagForReview(leadId, reason = null) {
        const scoring = await LeadScoring.findOneAndUpdate(
            { lead_id: leadId },
            {
                $addToSet: { flags: 'needs_review' },
                $set: {
                    updated_at: new Date(),
                    review_reason: reason
                }
            },
            { new: true }
        );

        if (!scoring) {
            const error = new Error('Scoring data not found');
            error.code = 'SCORING_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return scoring;
    },

    /**
     * Clear review flag
     */
    async clearReviewFlag(leadId) {
        const scoring = await LeadScoring.findOneAndUpdate(
            { lead_id: leadId },
            {
                $pull: { flags: 'needs_review' },
                $set: { updated_at: new Date() }
            },
            { new: true }
        );

        if (!scoring) {
            const error = new Error('Scoring data not found');
            error.code = 'SCORING_NOT_FOUND';
            error.statusCode = 404;
            throw error;
        }

        return scoring;
    },

    /**
     * Get all leads flagged for review
     */
    async getFlaggedForReview(page = 1, limit = 50) {
        const skip = (page - 1) * limit;

        const [scores, total] = await Promise.all([
            LeadScoring.find({ flags: 'needs_review' })
                .populate('lead_id', 'email name')
                .sort({ updated_at: -1 })
                .skip(skip)
                .limit(limit)
                .lean(),
            LeadScoring.countDocuments({ flags: 'needs_review' })
        ]);

        return {
            data: scores,
            pagination: {
                page,
                limit,
                total,
                pages: Math.ceil(total / limit)
            }
        };
    },

    /**
     * Get analytics across all scores
     */
    async getScoringAnalytics() {
        const pipeline = [
            {
                $group: {
                    _id: null,
                    avg_overall: { $avg: '$overall_score' },
                    max_overall: { $max: '$overall_score' },
                    min_overall: { $min: '$overall_score' },
                    avg_skills: { $avg: '$skills_score' },
                    avg_experience: { $avg: '$experience_score' },
                    avg_profile: { $avg: '$profile_completeness' },
                    avg_availability: { $avg: '$availability_score' },
                    avg_language: { $avg: '$language_score' },
                    total_leads: { $sum: 1 },
                    flagged_count: {
                        $sum: { $cond: [{ $in: ['needs_review', '$flags'] }, 1, 0] }
                    },
                    high_potential_count: {
                        $sum: { $cond: [{ $in: ['high_potential', '$flags'] }, 1, 0] }
                    }
                }
            }
        ];

        const result = await LeadScoring.aggregate(pipeline);

        return result[0] || {
            avg_overall: 0,
            total_leads: 0
        };
    }
};

export default scoringService;
