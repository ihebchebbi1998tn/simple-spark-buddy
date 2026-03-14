/**
 * @file authController.mjs
 * @description Authentication controller for candidate login/logout
 */

import { asyncHandler } from '../../../middleware/errorHandler.mjs';
import { successResponse, errorResponse } from '../../../utils/responseHelper.mjs';
import { validateEmail } from '../../../middleware/validation.mjs';
import { comparePassword } from '../../../utils/passwordHelper.mjs';
import { generateToken } from '../../../utils/jwtHelper.mjs';
import CandidateModel from '../models/Candidate.mjs';
import CandidateSystemInfoModel from '../models/CandidateSystemInfo.mjs';

/**
 * Login candidate
 * POST /api/auth/login
 */
export const login = asyncHandler(async (req, res) => {
  const { email, password } = req.body;

  // Validate input
  if (!email || !password) {
    return errorResponse(res, 'Email and password are required', 400);
  }

  const normalizedEmail = validateEmail(email);

  // Find candidate by email
  const candidate = await CandidateModel.findByEmail(normalizedEmail);
  
  if (!candidate) {
    return errorResponse(res, 'Invalid credentials', 401);
  }

  // Get system info to check password
  const systemInfo = await CandidateSystemInfoModel.findByCandidateId(candidate._id);
  
  if (!systemInfo || !systemInfo.password_hash) {
    return errorResponse(res, 'Invalid credentials', 401);
  }

  // Check if account is locked
  if (systemInfo.is_locked) {
    return errorResponse(res, 'Account is locked due to multiple failed login attempts', 403);
  }

  // Verify password
  const isValidPassword = await comparePassword(password, systemInfo.password_hash);
  
  if (!isValidPassword) {
    // Record failed login
    await CandidateSystemInfoModel.recordFailedLogin(candidate._id);
    return errorResponse(res, 'Invalid credentials', 401);
  }

  // Record successful login
  const ipAddress = req.ip || req.connection.remoteAddress;
  const device = req.headers['user-agent'];
  await CandidateSystemInfoModel.recordLogin(candidate._id, ipAddress, device);

  // Generate JWT token
  const token = generateToken({
    id: candidate._id,
    email: candidate.email,
    role: 'candidate'
  });

  // Return success with token and user data
  return successResponse(res, {
    token,
    user: {
      id: candidate._id,
      email: candidate.email,
      name: candidate.name,
      surname: candidate.surname,
      phone: candidate.phone,
      city: candidate.city
    }
  }, 'Login successful');
});

/**
 * Logout candidate (client-side token removal)
 * POST /api/auth/logout
 */
export const logout = asyncHandler(async (req, res) => {
  // In JWT-based auth, logout is mainly client-side
  // This endpoint can be used for logging/analytics
  return successResponse(res, null, 'Logout successful');
});

/**
 * Verify token and get current user
 * GET /api/auth/me
 */
export const getCurrentUser = asyncHandler(async (req, res) => {
  const candidate = await CandidateModel.findById(req.user.id);
  
  if (!candidate) {
    return errorResponse(res, 'User not found', 404);
  }

  return successResponse(res, {
    id: candidate._id,
    email: candidate.email,
    name: candidate.name,
    surname: candidate.surname,
    phone: candidate.phone,
    city: candidate.city
  });
});
