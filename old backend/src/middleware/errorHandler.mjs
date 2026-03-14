/**
 * @file errorHandler.mjs
 * @description Global error handling middleware
 */

import { config } from '../config/env.mjs';

/**
 * Global error handler
 */
export const errorHandler = (err, req, res, next) => {
  // Don't log expected errors during tests or JSON parse errors
  const isTestMode = process.argv.includes('--test');
  const isJsonParseError = err.type === 'entity.parse.failed';
  
  if (!isJsonParseError || !isTestMode) {
    console.error('Error:', err.message);
  }

  // JSON parse errors (malformed JSON)
  if (isJsonParseError) {
    return res.status(400).json({
      success: false,
      message: 'Invalid JSON format'
    });
  }

  // MongoDB duplicate key error
  if (err.code === 11000) {
    const field = Object.keys(err.keyPattern)[0];
    return res.status(409).json({
      success: false,
      message: `${field} already exists`,
      error: config.nodeEnv === 'development' ? err.message : undefined
    });
  }

  // JWT errors
  if (err.name === 'JsonWebTokenError') {
    return res.status(401).json({
      success: false,
      message: 'Invalid token'
    });
  }

  if (err.name === 'TokenExpiredError') {
    return res.status(401).json({
      success: false,
      message: 'Token expired'
    });
  }

  // Validation errors
  if (err.name === 'ValidationError') {
    return res.status(400).json({
      success: false,
      message: 'Validation error',
      errors: err.errors
    });
  }

  // Default error
  res.status(err.statusCode || 500).json({
    success: false,
    message: err.message || 'Internal server error',
    error: config.nodeEnv === 'development' ? err.stack : undefined
  });
};

/**
 * 404 handler
 */
export const notFound = (req, res) => {
  res.status(404).json({
    success: false,
    message: `Route ${req.originalUrl} not found`
  });
};

/**
 * Async handler wrapper
 */
export const asyncHandler = (fn) => (req, res, next) => {
  Promise.resolve(fn(req, res, next)).catch(next);
};
