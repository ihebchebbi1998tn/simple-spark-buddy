/**
 * Scoring Routes
 * Nested under lead resource
 * 
 * GET    /leads/:leadId/scoring              - Get full scoring data
 * GET    /leads/:leadId/scoring/overview     - Get auto-calculated fields only
 * GET    /leads/:leadId/scoring/breakdown    - Get detailed breakdown
 * PUT    /leads/:leadId/scoring              - Update manual fields
 * GET    /leads/:leadId/scoring/comparison   - Compare with other leads
 * POST   /leads/:leadId/scoring/flag-review  - Flag for manual review
 * POST   /leads/:leadId/scoring/clear-flag   - Clear review flag
 * GET    /scoring/flagged-for-review        - Get all flagged scores
 * GET    /scoring/analytics                 - Get analytics across all leads
 */

import express from 'express';
import scoringController from '../controllers/scoringController.mjs';

const router = express.Router({ mergeParams: true });

// GET / - Get full scoring data
router.get('/', scoringController.getScoringByLeadId);

// GET /overview - Get auto-calculated fields only
router.get('/overview', scoringController.getOverview);

// GET /breakdown - Detailed breakdown
router.get('/breakdown', scoringController.getBreakdown);

// GET /comparison - Comparison with others
router.get('/comparison', scoringController.getComparison);

// POST /flag-review - Flag for manual review
router.post('/flag-review', scoringController.flagForReview);

// POST /clear-flag - Clear review flag
router.post('/clear-flag', scoringController.clearReviewFlag);

// PUT / - Update manual fields
router.put('/', scoringController.updateManuals);

export default router;
