/**
 * @file index.mjs
 * @description V2 API routes - combines all v2 module routes
 */

import express from 'express';
import leadsRoutes from '../modules/leads/routes/index.mjs';

const router = express.Router();

/**
 * V2 API Routes
 */

// Leads module routes (includes nested: profile, skills, scoring, notifications, activity-logs, settings)
router.use('/leads', leadsRoutes);

export default router;
