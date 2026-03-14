/**
 * Profile Validator
 * Validates LeadProfile data for create and update operations
 */

export const validateProfileUpdate = (data) => {
    const errors = {};

    // Optional fields validation - only validate if provided
    if (data.desired_position !== undefined) {
        if (!Number.isInteger(data.desired_position) || data.desired_position < 1 || data.desired_position > 9) {
            errors.desired_position = 'Must be an integer between 1 and 9';
        }
    }

    if (data.call_center_experience !== undefined) {
        if (!Number.isInteger(data.call_center_experience) || data.call_center_experience < 1 || data.call_center_experience > 7) {
            errors.call_center_experience = 'Must be an integer between 1 and 7';
        }
    }

    if (data.call_center_self_assessment !== undefined) {
        if (!Number.isInteger(data.call_center_self_assessment) || data.call_center_self_assessment < 1 || data.call_center_self_assessment > 5) {
            errors.call_center_self_assessment = 'Must be an integer between 1 and 5';
        }
    }

    if (data.position_experience !== undefined) {
        if (!Number.isInteger(data.position_experience) || data.position_experience < 1 || data.position_experience > 7) {
            errors.position_experience = 'Must be an integer between 1 and 7';
        }
    }

    if (data.position_self_assessment !== undefined) {
        if (!Number.isInteger(data.position_self_assessment) || data.position_self_assessment < 1 || data.position_self_assessment > 5) {
            errors.position_self_assessment = 'Must be an integer between 1 and 5';
        }
    }

    // Activities validation
    if (data.activities !== undefined) {
        if (!Array.isArray(data.activities)) {
            errors.activities = 'Must be an array';
        } else if (data.activities.length > 3) {
            errors.activities = 'Maximum 3 activities allowed';
        } else {
            data.activities.forEach((activity, index) => {
                if (!Number.isInteger(activity.activity_type) || ![1, 2, 3, 4].includes(activity.activity_type)) {
                    errors[`activities.${index}.activity_type`] = 'Must be one of: 1, 2, 3, 4';
                }
                if (!Number.isInteger(activity.activity_experience) || activity.activity_experience < 1 || activity.activity_experience > 7) {
                    errors[`activities.${index}.activity_experience`] = 'Must be an integer between 1 and 7';
                }
                if (activity.operations && Array.isArray(activity.operations)) {
                    if (activity.operations.length > 3) {
                        errors[`activities.${index}.operations`] = 'Maximum 3 operations per activity';
                    }
                    activity.operations.forEach((op, opIndex) => {
                        if (!Number.isInteger(op.operation_type) || op.operation_type < 1 || op.operation_type > 7) {
                            errors[`activities.${index}.operations.${opIndex}.operation_type`] = 'Must be an integer between 1 and 7';
                        }
                        if (!Number.isInteger(op.operation_experience) || op.operation_experience < 1 || op.operation_experience > 7) {
                            errors[`activities.${index}.operations.${opIndex}.operation_experience`] = 'Must be an integer between 1 and 7';
                        }
                        if (op.training_needed !== undefined && typeof op.training_needed !== 'boolean') {
                            errors[`activities.${index}.operations.${opIndex}.training_needed`] = 'Must be boolean';
                        }
                    });
                }
            });
        }
    }

    // Work preferences validation
    if (data.work_preferences !== undefined) {
        if (typeof data.work_preferences !== 'object' || data.work_preferences === null) {
            errors.work_preferences = 'Must be an object';
        } else {
            if (data.work_preferences.availability !== undefined) {
                const validAvailability = ['immediately', 'within_2_weeks', 'within_1_month', 'flexible'];
                if (!validAvailability.includes(data.work_preferences.availability)) {
                    errors['work_preferences.availability'] = `Must be one of: ${validAvailability.join(', ')}`;
                }
            }
            if (data.work_preferences.contract_type !== undefined) {
                const validContractTypes = ['cdi', 'cdd', 'stage', 'freelance', 'alternance'];
                if (!Array.isArray(data.work_preferences.contract_type)) {
                    errors['work_preferences.contract_type'] = 'Must be an array';
                } else {
                    data.work_preferences.contract_type.forEach((type, idx) => {
                        if (!validContractTypes.includes(type)) {
                            errors[`work_preferences.contract_type.${idx}`] = `Invalid type. Must be one of: ${validContractTypes.join(', ')}`;
                        }
                    });
                }
            }
            if (data.work_preferences.work_schedule !== undefined) {
                const validSchedules = ['full_time', 'part_time', 'shift_work', 'flexible'];
                if (!Array.isArray(data.work_preferences.work_schedule)) {
                    errors['work_preferences.work_schedule'] = 'Must be an array';
                } else {
                    data.work_preferences.work_schedule.forEach((schedule, idx) => {
                        if (!validSchedules.includes(schedule)) {
                            errors[`work_preferences.work_schedule.${idx}`] = `Invalid schedule. Must be one of: ${validSchedules.join(', ')}`;
                        }
                    });
                }
            }
            if (data.work_preferences.remote_preference !== undefined) {
                const validRemote = ['on_site', 'hybrid', 'remote'];
                if (!validRemote.includes(data.work_preferences.remote_preference)) {
                    errors['work_preferences.remote_preference'] = `Must be one of: ${validRemote.join(', ')}`;
                }
            }
            if (data.work_preferences.travel_availability !== undefined && typeof data.work_preferences.travel_availability !== 'boolean') {
                errors['work_preferences.travel_availability'] = 'Must be boolean';
            }
        }
    }

    // Compensation validation
    if (data.compensation !== undefined) {
        if (typeof data.compensation !== 'object' || data.compensation === null) {
            errors.compensation = 'Must be an object';
        } else {
            if (data.compensation.salary_min !== undefined) {
                if (!Number.isInteger(data.compensation.salary_min) || data.compensation.salary_min < 0) {
                    errors['compensation.salary_min'] = 'Must be a non-negative integer';
                }
            }
            if (data.compensation.salary_max !== undefined) {
                if (!Number.isInteger(data.compensation.salary_max) || data.compensation.salary_max < 0) {
                    errors['compensation.salary_max'] = 'Must be a non-negative integer';
                }
            }
            if (data.compensation.salary_min !== undefined && data.compensation.salary_max !== undefined) {
                if (data.compensation.salary_min > data.compensation.salary_max) {
                    errors['compensation.salary_range'] = 'Minimum salary must be less than or equal to maximum';
                }
            }
            if (data.compensation.currency !== undefined) {
                const validCurrencies = ['USD', 'EUR', 'GBP', 'CHF', 'CAD'];
                if (!validCurrencies.includes(data.compensation.currency)) {
                    errors['compensation.currency'] = `Must be one of: ${validCurrencies.join(', ')}`;
                }
            }
            if (data.compensation.bonus_expected !== undefined && typeof data.compensation.bonus_expected !== 'boolean') {
                errors['compensation.bonus_expected'] = 'Must be boolean';
            }
        }
    }

    return {
        isValid: Object.keys(errors).length === 0,
        errors
    };
};

export const validateProfileQuery = (query) => {
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

    if (query.sortBy !== undefined) {
        const validSortFields = ['created_at', 'updated_at', 'desired_position'];
        if (!validSortFields.includes(query.sortBy)) {
            errors.sortBy = `Must be one of: ${validSortFields.join(', ')}`;
        }
    }

    if (query.sortOrder !== undefined) {
        const validOrders = ['asc', 'desc'];
        if (!validOrders.includes(query.sortOrder)) {
            errors.sortOrder = `Must be one of: ${validOrders.join(', ')}`;
        }
    }

    return {
        isValid: Object.keys(errors).length === 0,
        errors
    };
};

export default {
    validateProfileUpdate,
    validateProfileQuery
};
