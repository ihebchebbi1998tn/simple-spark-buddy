/**
 * @file index.mjs
 * @description Candidate module entry point - exports all candidate routes
 */

import express from 'express';
import authRoutes from './routes/auth.mjs';
import candidateRoutes from './routes/candidates.mjs';

const router = express.Router();

// Mount candidate routes
router.use('/auth', authRoutes);
router.use('/candidates', candidateRoutes);

export default router;
