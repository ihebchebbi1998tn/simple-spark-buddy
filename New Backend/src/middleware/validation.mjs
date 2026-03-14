/**
 * @file validation.mjs
 * @description Request validation middleware
 */

import validator from 'validator';

/**
 * Validate email format
 */
export const validateEmail = (email) => {
  if (!email || !validator.isEmail(email)) {
    throw new Error('Invalid email format');
  }
  return validator.normalizeEmail(email);
};

/**
 * Validate phone number
 */
export const validatePhone = (phone) => {
  if (!phone || !validator.isMobilePhone(phone, 'any')) {
    throw new Error('Invalid phone number');
  }
  return phone;
};

/**
 * Sanitize string input
 */
export const sanitizeString = (str, maxLength = 255) => {
  if (!str) return '';
  return validator.escape(validator.trim(str.substring(0, maxLength)));
};

/**
 * Validate required fields
 */
export const validateRequired = (fields) => {
  return (req, res, next) => {
    const missing = [];
    
    for (const field of fields) {
      if (!req.body[field]) {
        missing.push(field);
      }
    }

    if (missing.length > 0) {
      return res.status(400).json({
        success: false,
        message: `Missing required fields: ${missing.join(', ')}`
      });
    }

    next();
  };
};

/**
 * Validate ObjectId format
 */
export const validateObjectId = (id) => {
  const objectIdRegex = /^[0-9a-fA-F]{24}$/;
  if (!objectIdRegex.test(id)) {
    throw new Error('Invalid ID format');
  }
  return true;
};
