/**
 * Skill Validator
 * Validates LeadSkill data for create, update, and query operations
 */

export const validateSkillCreate = (data) => {
    const errors = {};

    // Required fields
    if (!data.lead_id) {
        errors.lead_id = 'Lead ID is required';
    }

    if (!Number.isInteger(data.qualification_type) || ![1, 2, 3, 4, 5, 6, 7].includes(data.qualification_type)) {
        errors.qualification_type = 'Must be integer: 1=Language, 2=Technical, 3=Certification, 4=Software, 5=Diploma, 6=License, 7=Other';
    }

    if (!Number.isInteger(data.test_method) || data.test_method < 1 || data.test_method > 10) {
        errors.test_method = 'Must be integer between 1 and 10';
    }

    // Language-specific validation (qualification_type = 1)
    if (data.qualification_type === 1) {
        if (!data.language_code) {
            errors.language_code = 'Language code required for language qualifications';
        } else {
            const validLanguages = ['FR', 'EN', 'ES', 'IT', 'DE', 'PT', 'NL', 'JA', 'ZH'];
            if (!validLanguages.includes(data.language_code.toUpperCase())) {
                errors.language_code = `Must be one of: ${validLanguages.join(', ')}`;
            }
        }
        if (data.is_native !== undefined && typeof data.is_native !== 'boolean') {
            errors.is_native = 'Must be boolean';
        }
        if (data.proficiency_level !== undefined) {
            if (!Number.isInteger(data.proficiency_level) || data.proficiency_level < 1 || data.proficiency_level > 7) {
                errors.proficiency_level = 'CEFR level must be 1-7 (A1-C2 + Native)';
            }
        }
    }

    // Technical skill validation (qualification_type = 2)
    if (data.qualification_type === 2) {
        if (!data.skill_name) {
            errors.skill_name = 'Skill name required for technical skills';
        } else if (typeof data.skill_name !== 'string' || data.skill_name.length > 100) {
            errors.skill_name = 'Must be a string with max 100 characters';
        }
        if (data.skill_category !== undefined) {
            if (!Number.isInteger(data.skill_category) || data.skill_category < 1) {
                errors.skill_category = 'Must be a positive integer';
            }
        }
    }

    // Certification validation (qualification_type = 3)
    if (data.qualification_type === 3) {
        if (!data.certification_name) {
            errors.certification_name = 'Certification name required';
        } else if (typeof data.certification_name !== 'string' || data.certification_name.length > 200) {
            errors.certification_name = 'Must be a string with max 200 characters';
        }
        if (data.certification_authority !== undefined && typeof data.certification_authority !== 'string') {
            errors.certification_authority = 'Must be a string';
        }
        if (data.certification_number !== undefined && typeof data.certification_number !== 'string') {
            errors.certification_number = 'Must be a string';
        }
        if (data.certification_expiry_date !== undefined) {
            if (data.certification_expiry_date !== null && isNaN(new Date(data.certification_expiry_date).getTime())) {
                errors.certification_expiry_date = 'Must be a valid ISO date';
            }
        }
    }

    // Software validation (qualification_type = 4)
    if (data.qualification_type === 4) {
        if (!data.software_name) {
            errors.software_name = 'Software name required';
        } else if (typeof data.software_name !== 'string' || data.software_name.length > 100) {
            errors.software_name = 'Must be a string with max 100 characters';
        }
        if (data.software_version !== undefined && typeof data.software_version !== 'string') {
            errors.software_version = 'Must be a string';
        }
    }

    // Test metadata validation
    if (data.completed !== undefined && typeof data.completed !== 'boolean') {
        errors.completed = 'Must be boolean';
    }

    if (data.test_date !== undefined) {
        if (data.test_date !== null && isNaN(new Date(data.test_date).getTime())) {
            errors.test_date = 'Must be a valid ISO date or null';
        }
    }

    if (data.test_duration_seconds !== undefined) {
        if (!Number.isInteger(data.test_duration_seconds) || data.test_duration_seconds < 0 || data.test_duration_seconds > 10800) {
            errors.test_duration_seconds = 'Must be integer between 0 and 10800 seconds';
        }
    }

    // Scores validation (only if completed)
    if (data.scores !== undefined) {
        if (typeof data.scores !== 'object' || data.scores === null) {
            errors.scores = 'Must be an object';
        } else {
            ['linguistic', 'soft_skills', 'job_skills', 'global'].forEach(scoreKey => {
                if (data.scores[scoreKey] !== undefined) {
                    if (!Number.isInteger(data.scores[scoreKey]) || data.scores[scoreKey] < 0 || data.scores[scoreKey] > 100) {
                        errors[`scores.${scoreKey}`] = 'Must be integer between 0 and 100';
                    }
                }
            });
        }
    }

    // Verification validation
    if (data.is_verified !== undefined && typeof data.is_verified !== 'boolean') {
        errors.is_verified = 'Must be boolean';
    }

    if (data.verified_by !== undefined) {
        if (data.verified_by !== null && ![1, 2, 3, 4].includes(data.verified_by)) {
            errors.verified_by = 'Must be one of: 1=system, 2=AI, 3=admin, 4=partner, or null';
        }
    }

    return {
        isValid: Object.keys(errors).length === 0,
        errors
    };
};

export const validateSkillUpdate = (data) => {
    const errors = {};

    // Don't allow changing qualification_type or test_method on update
    if (data.qualification_type !== undefined) {
        errors.qualification_type = 'Cannot change qualification type on update';
    }

    if (data.test_method !== undefined) {
        errors.test_method = 'Cannot change test method on update';
    }

    // Validate updateable fields based on type
    if (data.is_native !== undefined && typeof data.is_native !== 'boolean') {
        errors.is_native = 'Must be boolean';
    }

    if (data.proficiency_level !== undefined) {
        if (!Number.isInteger(data.proficiency_level) || data.proficiency_level < 1 || data.proficiency_level > 7) {
            errors.proficiency_level = 'CEFR level must be 1-7';
        }
    }

    if (data.completed !== undefined && typeof data.completed !== 'boolean') {
        errors.completed = 'Must be boolean';
    }

    if (data.test_date !== undefined) {
        if (data.test_date !== null && isNaN(new Date(data.test_date).getTime())) {
            errors.test_date = 'Must be a valid ISO date or null';
        }
    }

    if (data.test_duration_seconds !== undefined) {
        if (!Number.isInteger(data.test_duration_seconds) || data.test_duration_seconds < 0 || data.test_duration_seconds > 10800) {
            errors.test_duration_seconds = 'Must be integer between 0 and 10800';
        }
    }

    // Scores validation
    if (data.scores !== undefined) {
        if (typeof data.scores !== 'object' || data.scores === null) {
            errors.scores = 'Must be an object';
        } else {
            ['linguistic', 'soft_skills', 'job_skills', 'global'].forEach(scoreKey => {
                if (data.scores[scoreKey] !== undefined) {
                    if (!Number.isInteger(data.scores[scoreKey]) || data.scores[scoreKey] < 0 || data.scores[scoreKey] > 100) {
                        errors[`scores.${scoreKey}`] = 'Must be integer between 0 and 100';
                    }
                }
            });
        }
    }

    if (data.is_verified !== undefined && typeof data.is_verified !== 'boolean') {
        errors.is_verified = 'Must be boolean';
    }

    if (data.verified_by !== undefined) {
        if (data.verified_by !== null && ![1, 2, 3, 4].includes(data.verified_by)) {
            errors.verified_by = 'Must be one of: 1=system, 2=AI, 3=admin, 4=partner, or null';
        }
    }

    return {
        isValid: Object.keys(errors).length === 0,
        errors
    };
};

export const validateSkillQuery = (query) => {
    const errors = {};

    if (query.page !== undefined) {
        const page = parseInt(query.page);
        if (isNaN(page) || page < 1) {
            errors.page = 'Page must be a positive integer';
        }
    }

    if (query.limit !== undefined) {
        const limit = parseInt(query.limit);
        if (isNaN(limit) || limit < 1 || limit > 100) {
            errors.limit = 'Limit must be between 1 and 100';
        }
    }

    if (query.qualification_type !== undefined) {
        const type = parseInt(query.qualification_type);
        if (isNaN(type) || ![1, 2, 3, 4, 5, 6, 7].includes(type)) {
            errors.qualification_type = 'Must be integer 1-7';
        }
    }

    if (query.completed !== undefined) {
        if (query.completed !== 'true' && query.completed !== 'false') {
            errors.completed = 'Must be "true" or "false"';
        }
    }

    if (query.is_verified !== undefined) {
        if (query.is_verified !== 'true' && query.is_verified !== 'false') {
            errors.is_verified = 'Must be "true" or "false"';
        }
    }

    if (query.language_code !== undefined) {
        const validLanguages = ['FR', 'EN', 'ES', 'IT', 'DE', 'PT', 'NL', 'JA', 'ZH'];
        if (!validLanguages.includes(query.language_code.toUpperCase())) {
            errors.language_code = `Must be one of: ${validLanguages.join(', ')}`;
        }
    }

    if (query.sortBy !== undefined) {
        const validSortFields = ['created_at', 'updated_at', 'test_date', 'qualification_type'];
        if (!validSortFields.includes(query.sortBy)) {
            errors.sortBy = `Must be one of: ${validSortFields.join(', ')}`;
        }
    }

    if (query.sortOrder !== undefined) {
        if (query.sortOrder !== 'asc' && query.sortOrder !== 'desc') {
            errors.sortOrder = 'Must be "asc" or "desc"';
        }
    }

    return {
        isValid: Object.keys(errors).length === 0,
        errors
    };
};

export default {
    validateSkillCreate,
    validateSkillUpdate,
    validateSkillQuery
};
