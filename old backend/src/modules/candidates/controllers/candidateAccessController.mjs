/**
 * @file candidateAccessController.mjs
 * @description Controller for managing candidate account access (password, email, deletion)
 */

import { asyncHandler } from '../../../middleware/errorHandler.mjs';
import { successResponse, errorResponse } from '../../../utils/responseHelper.mjs';
import { validateEmail } from '../../../middleware/validation.mjs';
import { hashPassword, comparePassword } from '../../../utils/passwordHelper.mjs';
import CandidateModel from '../models/Candidate.mjs';
import CandidateSystemInfoModel from '../models/CandidateSystemInfo.mjs';
import { createAlert } from './candidateAlertController.mjs';

/**
 * Change candidate password
 */
export const changePassword = asyncHandler(async (req, res) => {
  const candidateId = req.user.id;
  const { currentPassword, newPassword } = req.body;

  if (!currentPassword || !newPassword) {
    return errorResponse(res, 'Mot de passe actuel et nouveau mot de passe requis', 400);
  }

  if (newPassword.length < 6) {
    return errorResponse(res, 'Le nouveau mot de passe doit contenir au moins 6 caractères', 400);
  }

  // Validate password strength (uppercase, lowercase, digit)
  const hasUpperCase = /[A-Z]/.test(newPassword);
  const hasLowerCase = /[a-z]/.test(newPassword);
  const hasNumbers = /\d/.test(newPassword);
  
  if (!hasUpperCase || !hasLowerCase || !hasNumbers) {
    return errorResponse(res, 'Le mot de passe doit contenir au moins une majuscule, une minuscule et un chiffre', 400);
  }

  // Get current password hash from system info
  const systemInfo = await CandidateSystemInfoModel.findByCandidateId(candidateId);
  
  if (!systemInfo || !systemInfo.password_hash) {
    return errorResponse(res, 'Candidat introuvable', 404);
  }

  // Verify current password
  const isValidPassword = await comparePassword(currentPassword, systemInfo.password_hash);
  if (!isValidPassword) {
    return errorResponse(res, 'Mot de passe actuel incorrect', 401);
  }

  // Hash new password
  const newHash = await hashPassword(newPassword);

  // Update password in system info
  await CandidateSystemInfoModel.update(candidateId, {
    password_hash: newHash
  });

  // Create success alert
  await createAlert(candidateId, {
    type: 'success',
    title: 'Mot de passe modifié',
    message: 'Votre mot de passe a été modifié avec succès. Utilisez votre nouveau mot de passe lors de votre prochaine connexion.',
    action_type: 'password_change'
  });

  return successResponse(res, null, 'Mot de passe modifié avec succès');
});

/**
 * Change candidate email
 */
export const changeEmail = asyncHandler(async (req, res) => {
  const candidateId = req.user.id;
  const { newEmail, password } = req.body;

  if (!newEmail || !password) {
    return errorResponse(res, 'Nouvel email et mot de passe requis', 400);
  }

  // Validate email format
  const normalizedEmail = validateEmail(newEmail);

  // Verify password
  const systemInfo = await CandidateSystemInfoModel.findByCandidateId(candidateId);
  
  if (!systemInfo || !systemInfo.password_hash) {
    return errorResponse(res, 'Candidat introuvable', 404);
  }

  const isValidPassword = await comparePassword(password, systemInfo.password_hash);

  if (!isValidPassword) {
    return errorResponse(res, 'Mot de passe incorrect', 401);
  }

  // Check if email already exists
  const existingCandidate = await CandidateModel.findByEmail(normalizedEmail);
  
  if (existingCandidate && existingCandidate._id.toString() !== candidateId.toString()) {
    return errorResponse(res, 'Cet email est déjà utilisé', 409);
  }

  // Update email
  await CandidateModel.update(candidateId, {
    email: normalizedEmail
  });

  // Create success alert
  await createAlert(candidateId, {
    type: 'success',
    title: 'Email modifié',
    message: `Votre adresse email a été modifiée avec succès. Nouvelle adresse: ${normalizedEmail}`,
    action_type: 'email_change'
  });

  return successResponse(res, null, 'Email modifié avec succès');
});

/**
 * Delete candidate account (soft delete)
 */
export const deleteAccount = asyncHandler(async (req, res) => {
  const candidateId = req.user.id;
  const { password, confirmation } = req.body;

  if (!password || confirmation !== 'SUPPRIMER') {
    return errorResponse(res, 'Mot de passe et confirmation "SUPPRIMER" requis', 400);
  }

  // Verify password
  const systemInfo = await CandidateSystemInfoModel.findByCandidateId(candidateId);
  
  if (!systemInfo || !systemInfo.password_hash) {
    return errorResponse(res, 'Candidat introuvable', 404);
  }

  const isValidPassword = await comparePassword(password, systemInfo.password_hash);

  if (!isValidPassword) {
    return errorResponse(res, 'Mot de passe incorrect', 401);
  }

  // Soft delete - mark as deleted in system info
  await CandidateSystemInfoModel.update(candidateId, {
    deleted: true
  });

  return successResponse(res, null, 'Compte supprimé avec succès');
});
