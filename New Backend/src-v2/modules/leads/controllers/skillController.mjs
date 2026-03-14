/**
 * @file skillController.mjs
 * @description API handlers for LEAD_SKILLS (languages, certifications, skills)
 * @version 1.0
 */

import { validateSkillCreate, validateSkillUpdate, validateSkillQuery } from '../validators/skillValidator.mjs';
import { updateLeadActivity } from '../services/leadService.mjs';
import {
    getSkillsByLeadId,
    getLanguageTestsByLeadId,
    getLanguageTest,
    getCompletedSkills,
    createOrUpdateLanguageTest,
    createTechnicalSkill,
    createCertification,
    updateSkill,
    deleteSkill,
    getSkillStats
} from '../services/skillService.mjs';

/**
 * ============================================================================
 * GET SKILLS ENDPOINTS
 * ============================================================================
 */

/**
 * GET /api/v2/leads/:leadId/skills
 * Get all skills for a lead
 */
export const getAllSkills = async (req, res) => {
    try {
        const skills = await getSkillsByLeadId(req.params.leadId);

        await updateLeadActivity(req.params.leadId);

        return res.status(200).json({
            success: true,
            data: skills,
            count: skills.length
        });
    } catch (error) {
        console.error('Get all skills error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to retrieve skills',
            error: error.message
        });
    }
};

/**
 * GET /api/v2/leads/:leadId/skills/language-tests
 * Get all language tests for a lead
 */
export const getLanguageTests = async (req, res) => {
    try {
        const tests = await getLanguageTestsByLeadId(req.params.leadId);

        await updateLeadActivity(req.params.leadId);

        return res.status(200).json({
            success: true,
            data: tests,
            count: tests.length
        });
    } catch (error) {
        console.error('Get language tests error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to retrieve language tests',
            error: error.message
        });
    }
};

/**
 * GET /api/v2/leads/:leadId/skills/language/:languageCode/method/:testMethod
 * Get specific language test
 */
export const getLanguageTestByCode = async (req, res) => {
    try {
        const test = await getLanguageTest(
            req.params.leadId,
            req.params.languageCode,
            parseInt(req.params.testMethod)
        );

        if (!test) {
            return res.status(404).json({
                success: false,
                message: 'Language test not found'
            });
        }

        await updateLeadActivity(req.params.leadId);

        return res.status(200).json({
            success: true,
            data: test
        });
    } catch (error) {
        console.error('Get language test error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to retrieve language test',
            error: error.message
        });
    }
};

/**
 * GET /api/v2/leads/:leadId/skills/completed
 * Get completed skills
 */
export const getCompletedSkillsEndpoint = async (req, res) => {
    try {
        const skills = await getCompletedSkills(req.params.leadId);

        await updateLeadActivity(req.params.leadId);

        return res.status(200).json({
            success: true,
            data: skills,
            count: skills.length
        });
    } catch (error) {
        console.error('Get completed skills error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to retrieve completed skills',
            error: error.message
        });
    }
};

/**
 * GET /api/v2/leads/:leadId/skills/stats
 * Get skill statistics
 */
export const getSkillsStats = async (req, res) => {
    try {
        const stats = await getSkillStats(req.params.leadId);

        await updateLeadActivity(req.params.leadId);

        return res.status(200).json({
            success: true,
            data: stats
        });
    } catch (error) {
        console.error('Get skill stats error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to retrieve skill statistics',
            error: error.message
        });
    }
};

/**
 * ============================================================================
 * CREATE/UPDATE SKILLS ENDPOINTS
 * ============================================================================
 */

/**
 * POST /api/v2/leads/:leadId/skills/language
 * Create or update language test result
 */
export const createUpdateLanguageTest = async (req, res) => {
    try {
        const { language_code, test_method, completed, scores, test_duration_seconds } = req.body;

        // Validate required fields
        if (!language_code) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: { language_code: 'Language code required' }
            });
        }

        if (!test_method) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: { test_method: 'Test method required' }
            });
        }

        // Validate scores if test is completed
        if (completed && scores) {
            if (typeof scores.global !== 'number' || scores.global < 0 || scores.global > 100) {
                return res.status(400).json({
                    success: false,
                    message: 'Invalid scores',
                    errors: { 'scores.global': 'Score must be 0-100' }
                });
            }
        }

        // Create or update test
        const result = await createOrUpdateLanguageTest(req.params.leadId, {
            language_code,
            test_method,
            completed: completed || false,
            scores: scores || null,
            test_duration_seconds: test_duration_seconds || null,
            is_verified: req.body.is_verified || false,
            verified_by: req.body.verified_by || null,
            source: req.body.source || 1,
            is_native: req.body.is_native || false,
            proficiency_level: req.body.proficiency_level || null,
            language_count: req.body.language_count
        });

        await updateLeadActivity(req.params.leadId);

        return res.status(200).json({
            success: true,
            message: 'Language test saved successfully',
            data: result
        });
    } catch (error) {
        console.error('Language test error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to save language test',
            error: error.message
        });
    }
};

/**
 * POST /api/v2/leads/:leadId/skills/technical
 * Create technical skill
 */
export const createTechnicalSkillEndpoint = async (req, res) => {
    try {
        // Basic validation
        if (!req.body.skill_name) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: { skill_name: 'Skill name is required' }
            });
        }

        if (req.body.test_method && (!Number.isInteger(req.body.test_method) || req.body.test_method < 1 || req.body.test_method > 10)) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: { test_method: 'Test method must be integer 1-10' }
            });
        }

        // Create skill with all fields
        const skill = await createTechnicalSkill(req.params.leadId, {
            skill_name: req.body.skill_name,
            skill_category: req.body.skill_category || null,
            test_method: req.body.test_method || 1,
            completed: req.body.completed || false,
            test_date: req.body.test_date || null,
            test_duration_seconds: req.body.test_duration_seconds || null,
            scores: req.body.scores || null,
            is_verified: req.body.is_verified || false,
            verified_by: req.body.verified_by || null,
            source: req.body.source || 1
        });

        await updateLeadActivity(req.params.leadId);

        return res.status(201).json({
            success: true,
            message: 'Technical skill created successfully',
            data: skill
        });
    } catch (error) {
        console.error('Create technical skill error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to create technical skill',
            error: error.message
        });
    }
};

/**
 * POST /api/v2/leads/:leadId/skills/certification
 * Create certification
 */
export const createCertificationEndpoint = async (req, res) => {
    try {
        // Basic validation
        if (!req.body.certification_name) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: { certification_name: 'Certification name is required' }
            });
        }

        if (!req.body.certification_authority) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: { certification_authority: 'Certification authority is required' }
            });
        }

        if (req.body.test_method && (!Number.isInteger(req.body.test_method) || req.body.test_method < 1 || req.body.test_method > 10)) {
            return res.status(400).json({
                success: false,
                message: 'Validation failed',
                errors: { test_method: 'Test method must be integer 1-10' }
            });
        }

        // Create certification with all fields
        const cert = await createCertification(req.params.leadId, {
            certification_name: req.body.certification_name,
            certification_authority: req.body.certification_authority,
            certification_number: req.body.certification_number || null,
            certification_expiry_date: req.body.certification_expiry_date || null,
            test_method: req.body.test_method || 1,
            completed: req.body.completed || false,
            test_date: req.body.test_date || null,
            test_duration_seconds: req.body.test_duration_seconds || null,
            scores: req.body.scores || null,
            file_url: req.body.file_url || null,
            file_type: req.body.file_type || null,
            is_verified: req.body.is_verified || false,
            verified_by: req.body.verified_by || null,
            source: req.body.source || 2 // 2 = upload
        });

        await updateLeadActivity(req.params.leadId);

        return res.status(201).json({
            success: true,
            message: 'Certification created successfully',
            data: cert
        });
    } catch (error) {
        console.error('Create certification error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to create certification',
            error: error.message
        });
    }
};

/**
 * PUT /api/v2/leads/skills/:skillId
 * Update skill
 */
export const updateSkillEndpoint = async (req, res) => {
    try {
        const updateData = req.body;

        // Prevent updating lead_id and qualification_type
        delete updateData.lead_id;
        delete updateData.qualification_type;

        const updatedSkill = await updateSkill(req.params.skillId, updateData);

        if (!updatedSkill) {
            return res.status(404).json({
                success: false,
                message: 'Skill not found'
            });
        }

        await updateLeadActivity(updatedSkill.lead_id);

        return res.status(200).json({
            success: true,
            message: 'Skill updated successfully',
            data: updatedSkill
        });
    } catch (error) {
        console.error('Update skill error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to update skill',
            error: error.message
        });
    }
};

/**
 * ============================================================================
 * DELETE ENDPOINTS
 * ============================================================================
 */

/**
 * DELETE /api/v2/leads/skills/:skillId
 * Delete skill
 */
export const deleteSkillEndpoint = async (req, res) => {
    try {
        const deletedSkill = await deleteSkill(req.params.skillId);

        if (!deletedSkill) {
            return res.status(404).json({
                success: false,
                message: 'Skill not found'
            });
        }

        await updateLeadActivity(deletedSkill.lead_id);

        return res.status(200).json({
            success: true,
            message: 'Skill deleted successfully',
            data: {
                skill_id: deletedSkill._id,
                lead_id: deletedSkill.lead_id
            }
        });
    } catch (error) {
        console.error('Delete skill error:', error);
        return res.status(500).json({
            success: false,
            message: 'Failed to delete skill',
            error: error.message
        });
    }
};

/**
 * ============================================================================
 * EXPORT
 * ============================================================================
 */

export default {
    getAllSkills,
    getLanguageTests,
    getLanguageTestByCode,
    getCompletedSkillsEndpoint,
    getSkillsStats,
    createUpdateLanguageTest,
    createTechnicalSkillEndpoint,
    createCertificationEndpoint,
    updateSkillEndpoint,
    deleteSkillEndpoint
};
