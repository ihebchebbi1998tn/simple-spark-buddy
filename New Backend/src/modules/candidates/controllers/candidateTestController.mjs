/**
 * @file candidateTestController.mjs
 * @description Candidate language test scores controller
 */

import { asyncHandler } from '../../../middleware/errorHandler.mjs';
import { successResponse, errorResponse } from '../../../utils/responseHelper.mjs';
import CandidateTestScoresModel from '../models/CandidateTestScores.mjs';
import { createAlert } from './candidateAlertController.mjs';

/**
 * Add/Update language test scores (for existing logged-in users)
 * POST /api/candidates/test-scores
 */
export const addTestScore = asyncHandler(async (req, res) => {
  const candidateId = req.user.id;
  const { language, scores } = req.body;

  console.log('📝 POST /api/candidates/test-scores - Request received:', {
    candidateId,
    language,
    scores
  });

  if (!language || !scores) {
    console.log('❌ Missing language or scores');
    return errorResponse(res, 'Language and scores are required', 400);
  }

  // Validate language
  const validLanguages = ['french', 'english', 'german', 'italian', 'spanish'];
  if (!validLanguages.includes(language.toLowerCase())) {
    console.log('❌ Invalid language:', language);
    return errorResponse(res, 'Invalid language', 400);
  }

  try {
    // Update test scores
    console.log('💾 Updating test scores for candidate:', candidateId);
    const result = await CandidateTestScoresModel.updateTestScores(candidateId, language.toLowerCase(), scores);
    console.log('✅ Test scores updated:', result);

    // Create success alert
    const languageNames = {
      french: 'français',
      english: 'anglais',
      german: 'allemand',
      italian: 'italien',
      spanish: 'espagnol'
    };

    await createAlert(candidateId, {
      type: 'success',
      title: 'Test de langue validé',
      message: `Votre test de ${languageNames[language.toLowerCase()]} a été validé avec succès. Score: ${scores.overall}/100`,
      action_type: 'test_completion'
    });

    return successResponse(res, null, 'Test scores saved successfully');
  } catch (error) {
    // Create error alert
    await createAlert(candidateId, {
      type: 'error',
      title: 'Échec du test',
      message: 'Une erreur est survenue lors de la sauvegarde de votre test.',
      action_type: 'test_completion'
    });
    throw error;
  }
});
