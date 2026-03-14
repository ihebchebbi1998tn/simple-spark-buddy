/**
 * @file index.mjs
 * @description Prospects module entry point - exports all prospect routes
 * @todo Implement prospect models, controllers, and routes
 */

import express from 'express';

const router = express.Router();

// Health check for prospects module
router.get('/health', (req, res) => {
  res.json({ 
    success: true, 
    message: 'Prospects module is ready',
    status: 'Not implemented yet'
  });
});

// TODO: Add prospect routes here
// Example:
// import prospectRoutes from './routes/prospects.mjs';
// router.use('/prospects', prospectRoutes);

export default router;
