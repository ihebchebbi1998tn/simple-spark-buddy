import express from 'express';
import * as skillController from '../controllers/skillController.mjs';

const router = express.Router({ mergeParams: true });

/**
 * @route   GET /api/v2/leads/:leadId/skills
 * @desc    Get all skills/qualifications for a lead
 * @access  Private (should add auth middleware)
 */
router.get('/', skillController.getAllSkills);

/**
 * @route   GET /api/v2/leads/:leadId/skills/language-tests
 * @desc    Get all language tests for a lead
 * @access  Private (should add auth middleware)
 */
router.get('/language-tests', skillController.getLanguageTests);

/**
 * @route   GET /api/v2/leads/:leadId/skills/language/:code/method/:method
 * @desc    Get specific language test by language code and test method
 * @params  code=FR|EN|ES|IT|DE (language code)
 * @params  method=1-10 (test delivery method)
 * @access  Private (should add auth middleware)
 */
router.get('/language/:code/method/:method', skillController.getLanguageTestByCode);

/**
 * @route   GET /api/v2/leads/:leadId/skills/completed
 * @desc    Get only completed skills with scores
 * @access  Private (should add auth middleware)
 */
router.get('/completed', skillController.getCompletedSkillsEndpoint);

/**
 * @route   GET /api/v2/leads/:leadId/skills/stats
 * @desc    Get skill statistics (total, completed, completion rate, breakdown)
 * @access  Private (should add auth middleware)
 */
router.get('/stats', skillController.getSkillsStats);

/**
 * @route   POST /api/v2/leads/:leadId/skills/language
 * @desc    Create or update a language test (upsert pattern)
 * @body    {
 *            language_code: "FR"|"EN"|"ES"|"IT"|"DE",
 *            is_native: boolean,
 *            proficiency_level: 1-7,     // CEFR: A1-C2
 *            test_method: 1-10,
 *            completed: boolean,
 *            test_date: "2026-01-15",    // optional
 *            test_duration_seconds: 1800, // optional
 *            scores: {                    // required if completed=true
 *              linguistic: 0-100,
 *              soft_skills: 0-100,
 *              job_skills: 0-100,
 *              global: 0-100
 *            },
 *            is_verified: boolean,
 *            verified_by: 1-4,           // 1=system, 2=AI, 3=admin, 4=partner
 *            verified_at: "2026-01-15T10:30:00Z",
 *            source: 1-4,                // 1=platform, 2=upload, 3=partner, 4=migration
 *            file_url: "https://..."
 *          }
 * @access  Private (should add auth middleware)
 */
router.post('/language', skillController.createUpdateLanguageTest);

/**
 * @route   POST /api/v2/leads/:leadId/skills/technical
 * @desc    Create a technical skill
 * @body    {
 *            skill_name: "JavaScript",
 *            skill_category: 1-20,
 *            test_method: 1-10,
 *            proficiency_level: 1-7,
 *            completed: boolean,
 *            test_date: "2026-01-15",
 *            test_duration_seconds: 3600,
 *            scores: {
 *              linguistic: 0-100,
 *              soft_skills: 0-100,
 *              job_skills: 0-100,
 *              global: 0-100
 *            },
 *            source: 1-4,
 *            file_url: "https://..."
 *          }
 * @access  Private (should add auth middleware)
 */
router.post('/technical', skillController.createTechnicalSkillEndpoint);

/**
 * @route   POST /api/v2/leads/:leadId/skills/certification
 * @desc    Create a certification
 * @body    {
 *            certification_name: "AWS Solutions Architect",
 *            certification_authority: "Amazon",
 *            certification_number: "ABC-123-DEF",
 *            certification_expiry_date: "2028-01-15",
 *            test_date: "2024-01-15",
 *            is_verified: boolean,
 *            verified_by: 1-4,
 *            source: 1-4,
 *            file_url: "https://..."
 *          }
 * @access  Private (should add auth middleware)
 */
router.post('/certification', skillController.createCertificationEndpoint);

/**
 * @route   PUT /api/v2/skills/:skillId
 * @desc    Update a skill (language test, technical skill, certification, software)
 * @note    Cannot change lead_id or qualification_type
 * @access  Private (should add auth middleware)
 */
router.put('/:skillId', skillController.updateSkillEndpoint);

/**
 * @route   DELETE /api/v2/skills/:skillId
 * @desc    Delete a skill permanently
 * @access  Private (should add auth middleware)
 */
router.delete('/:skillId', skillController.deleteSkillEndpoint);

export default router;
