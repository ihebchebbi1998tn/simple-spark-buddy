/**
 * @file index.mjs
 * @description Main routes index - combines all module routes
 */

import express from 'express';
import candidatesModule from '../modules/candidates/index.mjs';
import prospectsModule from '../modules/prospects/index.mjs';

const router = express.Router();

// Health check
router.get('/health', (req, res) => {
  res.json({ 
    success: true, 
    message: 'API is running',
    timestamp: new Date().toISOString()
  });
});

// Mount module routes
router.use('/', candidatesModule);  // Mounts /auth and /candidates
router.use('/', prospectsModule);   // Mounts /prospects (future)

export default router;
