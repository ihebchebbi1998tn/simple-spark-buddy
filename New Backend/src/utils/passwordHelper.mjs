/**
 * @file passwordHelper.mjs
 * @description Password hashing and verification utilities
 */

import bcrypt from 'bcrypt';

const SALT_ROUNDS = 10;

/**
 * Hash password
 */
export const hashPassword = async (password) => {
  return await bcrypt.hash(password, SALT_ROUNDS);
};

/**
 * Compare password with hash
 */
export const comparePassword = async (password, hash) => {
  return await bcrypt.compare(password, hash);
};

/**
 * Validate password strength
 * Requirements: minimum 6 characters, at least one uppercase, one lowercase, one digit
 */
export const validatePasswordStrength = (password) => {
  const minLength = 6;
  const hasUpperCase = /[A-Z]/.test(password);
  const hasLowerCase = /[a-z]/.test(password);
  const hasNumbers = /\d/.test(password);

  if (password.length < minLength) {
    throw new Error('Le mot de passe doit contenir au moins 6 caractères');
  }

  if (!hasUpperCase || !hasLowerCase || !hasNumbers) {
    throw new Error('Le mot de passe doit contenir au moins une majuscule, une minuscule et un chiffre');
  }

  return true;
};
