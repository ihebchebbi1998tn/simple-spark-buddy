/**
 * @file passwordResetController.mjs
 * @description Password reset functionality (forgot password flow)
 */

import { asyncHandler } from '../../../middleware/errorHandler.mjs';
import { successResponse, errorResponse } from '../../../utils/responseHelper.mjs';
import { validateEmail } from '../../../middleware/validation.mjs';
import { hashPassword, validatePasswordStrength } from '../../../utils/passwordHelper.mjs';
import { sendPasswordResetEmail } from '../../../utils/mailer.mjs';
import CandidateModel from '../models/Candidate.mjs';
import CandidateSystemInfoModel from '../models/CandidateSystemInfo.mjs';
import PasswordResetModel from '../models/PasswordReset.mjs';

/**
 * Request password reset - sends code to email
 * POST /api/auth/forgot-password
 */
export const requestPasswordReset = asyncHandler(async (req, res) => {
  const { email } = req.body;
  
  if (!email) {
    return errorResponse(res, 'Email is required', 400);
  }
  
  let normalizedEmail;
  try {
    normalizedEmail = validateEmail(email);
  } catch (e) {
    return errorResponse(res, 'Format d\'email invalide', 400);
  }
  
  // Find candidate by email
  const candidate = await CandidateModel.findByEmail(normalizedEmail);
  
  if (!candidate) {
    console.log('⚠️ Password reset requested for non-existent email:', normalizedEmail);
    return errorResponse(res, 'Aucun compte trouvé avec cette adresse email', 404);
  }
  
  // Create reset code
  const resetRequest = await PasswordResetModel.create(normalizedEmail, candidate._id);
  
  // Send email with code
  const emailResult = await sendPasswordResetEmail(
    normalizedEmail, 
    resetRequest.code,
    candidate.surname || candidate.name
  );
  
  if (!emailResult.success) {
    console.error('❌ Failed to send password reset email:', emailResult.error);
    // Return error so user knows email didn't send
    return errorResponse(res, `Impossible d'envoyer l'email: ${emailResult.error}`, 500);
  }
  
  console.log('✅ Password reset email sent to:', normalizedEmail);
  return successResponse(res, null, 'Un code de réinitialisation a été envoyé à votre adresse email.');
});

/**
 * Verify reset code
 * POST /api/auth/verify-reset-code
 */
export const verifyResetCode = asyncHandler(async (req, res) => {
  const { email, code } = req.body;
  
  if (!email || !code) {
    return errorResponse(res, 'Email et code sont requis', 400);
  }
  
  let normalizedEmail;
  try {
    normalizedEmail = validateEmail(email);
  } catch (e) {
    return errorResponse(res, 'Format d\'email invalide', 400);
  }
  
  // Verify the code
  const verification = await PasswordResetModel.verifyCode(normalizedEmail, code);
  
  if (!verification.valid) {
    return errorResponse(res, verification.error, 400);
  }
  
  return successResponse(res, { verified: true }, 'Code vérifié avec succès');
});

/**
 * Reset password with verified code
 * POST /api/auth/reset-password
 */
export const resetPassword = asyncHandler(async (req, res) => {
  const { email, code, newPassword } = req.body;
  
  if (!email || !code || !newPassword) {
    return errorResponse(res, 'Email, code et nouveau mot de passe sont requis', 400);
  }
  
  let normalizedEmail;
  try {
    normalizedEmail = validateEmail(email);
  } catch (e) {
    return errorResponse(res, 'Format d\'email invalide', 400);
  }
  
  // Validate password strength
  try {
    validatePasswordStrength(newPassword);
  } catch (e) {
    return errorResponse(res, e.message, 400);
  }
  
  // Verify code is valid and get candidate ID
  const verification = await PasswordResetModel.verifyCode(normalizedEmail, code);
  
  if (!verification.valid) {
    return errorResponse(res, verification.error, 400);
  }
  
  // Hash new password
  const passwordHash = await hashPassword(newPassword);
  
  // Update password in system info
  await CandidateSystemInfoModel.update(verification.candidateId, {
    password_hash: passwordHash,
    failed_login_attempts: 0,
    is_locked: false
  });
  
  // Mark reset as used
  await PasswordResetModel.markAsUsed(normalizedEmail);
  
  console.log('✅ Password reset successful for:', normalizedEmail);
  
  return successResponse(res, null, 'Mot de passe modifié avec succès');
});
