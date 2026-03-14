/**
 * @file skillService.mjs
 * @description Business logic for LEAD_SKILLS CRUD operations
 * @version 1.0
 */

import { LeadSkill, Lead } from '../models/index.mjs';

/**
 * ============================================================================
 * SKILL RETRIEVAL
 * ============================================================================
 */

/**
 * Get all skills for a lead
 * @param {string} leadId - Lead ID
 * @returns {Promise<array>} Array of skills
 */
export const getSkillsByLeadId = async (leadId) => {
    try {
        return await LeadSkill.find({ lead_id: leadId }).sort({ created_at: -1 });
    } catch (error) {
        throw error;
    }
};

/**
 * Get language tests for a lead
 * @param {string} leadId - Lead ID
 * @returns {Promise<array>} Array of language tests
 */
export const getLanguageTestsByLeadId = async (leadId) => {
    try {
        return await LeadSkill.find({
            lead_id: leadId,
            qualification_type: 1 // Language tests only
        }).sort({ language_code: 1, created_at: -1 });
    } catch (error) {
        throw error;
    }
};

/**
 * Get specific language test
 * @param {string} leadId - Lead ID
 * @param {string} languageCode - Language code (FR, EN, etc.)
 * @param {number} testMethod - Test method code
 * @returns {Promise<object|null>} Language test or null
 */
export const getLanguageTest = async (leadId, languageCode, testMethod) => {
    try {
        return await LeadSkill.findOne({
            lead_id: leadId,
            qualification_type: 1,
            language_code: languageCode,
            test_method: testMethod
        });
    } catch (error) {
        throw error;
    }
};

/**
 * Get skills by type
 * @param {string} leadId - Lead ID
 * @param {number} qualificationType - Qualification type code
 * @returns {Promise<array>} Array of skills
 */
export const getSkillsByType = async (leadId, qualificationType) => {
    try {
        return await LeadSkill.find({
            lead_id: leadId,
            qualification_type: qualificationType
        }).sort({ created_at: -1 });
    } catch (error) {
        throw error;
    }
};

/**
 * Get completed skills for a lead
 * @param {string} leadId - Lead ID
 * @returns {Promise<array>} Array of completed skills
 */
export const getCompletedSkills = async (leadId) => {
    try {
        return await LeadSkill.find({
            lead_id: leadId,
            completed: true
        }).sort({ test_date: -1 });
    } catch (error) {
        throw error;
    }
};

/**
 * ============================================================================
 * SKILL CREATION & UPDATES
 * ============================================================================
 */

/**
 * Create or update language test result
 * @param {string} leadId - Lead ID
 * @param {object} testData - Test data (language_code, test_method, scores, etc.)
 * @returns {Promise<object>} Created/updated skill
 */
export const createOrUpdateLanguageTest = async (leadId, testData) => {
    try {
        const existingTest = await LeadSkill.findOne({
            lead_id: leadId,
            qualification_type: 1,
            language_code: testData.language_code,
            test_method: testData.test_method
        });

        const updateData = {
            completed: testData.completed || false,
            test_date: testData.completed ? new Date() : null,
            test_duration_seconds: testData.test_duration_seconds || null,
            scores: testData.scores || null,
            is_verified: testData.is_verified || false,
            verified_by: testData.verified_by || null,
            verified_at: testData.verified_at || null
        };

        if (existingTest) {
            // Update existing
            const updated = await LeadSkill.findByIdAndUpdate(
                existingTest._id,
                updateData,
                { new: true, runValidators: true }
            );

            // Update lead skills_count if test was completed
            if (testData.completed && !existingTest.completed) {
                await Lead.findByIdAndUpdate(leadId, {
                    $inc: { 'summary_metrics.skills_count': 1 }
                });
            }

            return updated;
        } else {
            // Create new
            const newTest = new LeadSkill({
                lead_id: leadId,
                qualification_type: 1,
                language_code: testData.language_code,
                is_native: testData.is_native || false,
                proficiency_level: testData.proficiency_level || null,
                test_method: testData.test_method,
                ...updateData,
                source: testData.source || 1 // 1 = platform
            });

            await newTest.save();

            // Update lead skills_count
            if (testData.completed) {
                await Lead.findByIdAndUpdate(leadId, {
                    $inc: { 'summary_metrics.skills_count': 1 },
                    'summary_metrics.languages_count': Math.min(
                        (testData.language_count || 1),
                        3
                    )
                });
            }

            return newTest;
        }
    } catch (error) {
        throw error;
    }
};

/**
 * Create technical skill
 * @param {string} leadId - Lead ID
 * @param {object} skillData - Skill data
 * @returns {Promise<object>} Created skill
 */
export const createTechnicalSkill = async (leadId, skillData) => {
    try {
        const skill = new LeadSkill({
            lead_id: leadId,
            qualification_type: 2, // Technical skill
            skill_name: skillData.skill_name,
            skill_category: skillData.skill_category,
            test_method: skillData.test_method || 1,
            completed: skillData.completed || false,
            test_date: skillData.test_date || new Date(),
            test_duration_seconds: skillData.test_duration_seconds || null,
            scores: skillData.scores || null,
            is_verified: skillData.is_verified || false,
            verified_by: skillData.verified_by || null,
            source: skillData.source || 1
        });

        await skill.save();

        // Update lead skills_count
        await Lead.findByIdAndUpdate(leadId, {
            $inc: { 'summary_metrics.skills_count': 1 }
        });

        return skill;
    } catch (error) {
        throw error;
    }
};

/**
 * Create certification
 * @param {string} leadId - Lead ID
 * @param {object} certData - Certification data
 * @returns {Promise<object>} Created certification
 */
export const createCertification = async (leadId, certData) => {
    try {
        const cert = new LeadSkill({
            lead_id: leadId,
            qualification_type: 3, // Certification
            certification_name: certData.certification_name,
            certification_authority: certData.certification_authority,
            certification_number: certData.certification_number || null,
            certification_expiry_date: certData.certification_expiry_date || null,
            test_method: certData.test_method || 1,
            completed: certData.completed || false,
            test_date: certData.test_date || new Date(),
            test_duration_seconds: certData.test_duration_seconds || null,
            scores: certData.scores || null,
            file_url: certData.file_url || null,
            file_type: certData.file_type || null,
            is_verified: certData.is_verified || false,
            verified_by: certData.verified_by || null,
            source: certData.source || 2 // 2 = upload
        });

        await cert.save();

        // Update lead skills_count
        await Lead.findByIdAndUpdate(leadId, {
            $inc: { 'summary_metrics.skills_count': 1 }
        });

        return cert;
    } catch (error) {
        throw error;
    }
};

/**
 * Update skill
 * @param {string} skillId - Skill ID
 * @param {object} updateData - Data to update
 * @returns {Promise<object>} Updated skill
 */
export const updateSkill = async (skillId, updateData) => {
    try {
        return await LeadSkill.findByIdAndUpdate(
            skillId,
            updateData,
            { new: true, runValidators: true }
        );
    } catch (error) {
        throw error;
    }
};

/**
 * ============================================================================
 * SKILL DELETION
 * ============================================================================
 */

/**
 * Delete skill
 * @param {string} skillId - Skill ID
 * @returns {Promise<object>} Deleted skill
 */
export const deleteSkill = async (skillId) => {
    try {
        const skill = await LeadSkill.findByIdAndDelete(skillId);

        if (skill) {
            // Update lead skills_count
            await Lead.findByIdAndUpdate(skill.lead_id, {
                $inc: { 'summary_metrics.skills_count': -1 }
            });
        }

        return skill;
    } catch (error) {
        throw error;
    }
};

/**
 * ============================================================================
 * SKILL STATISTICS
 * ============================================================================
 */

/**
 * Get skill statistics for a lead
 * @param {string} leadId - Lead ID
 * @returns {Promise<object>} Skill statistics
 */
export const getSkillStats = async (leadId) => {
    try {
        const [
            totalSkills,
            completedSkills,
            languageTests,
            completedLanguageTests,
            certifications,
            avgScore
        ] = await Promise.all([
            LeadSkill.countDocuments({ lead_id: leadId }),
            LeadSkill.countDocuments({ lead_id: leadId, completed: true }),
            LeadSkill.countDocuments({ lead_id: leadId, qualification_type: 1 }),
            LeadSkill.countDocuments({ lead_id: leadId, qualification_type: 1, completed: true }),
            LeadSkill.countDocuments({ lead_id: leadId, qualification_type: 3 }),
            LeadSkill.aggregate([
                { $match: { lead_id: leadId, completed: true } },
                { $group: { _id: null, avg: { $avg: '$scores.global' } } }
            ])
        ]);

        return {
            total_skills: totalSkills,
            completed_skills: completedSkills,
            completion_rate: totalSkills > 0 ? Math.round((completedSkills / totalSkills) * 100) : 0,
            language_tests: {
                total: languageTests,
                completed: completedLanguageTests
            },
            certifications: certifications,
            average_score: avgScore[0]?.avg || 0
        };
    } catch (error) {
        throw error;
    }
};

/**
 * ============================================================================
 * EXPORT
 * ============================================================================
 */

export default {
    getSkillsByLeadId,
    getLanguageTestsByLeadId,
    getLanguageTest,
    getSkillsByType,
    getCompletedSkills,
    createOrUpdateLanguageTest,
    createTechnicalSkill,
    createCertification,
    updateSkill,
    deleteSkill,
    getSkillStats
};
