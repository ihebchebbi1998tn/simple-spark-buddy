/**
 * @file leadValidator.mjs
 * @description Validation rules and functions for LEADS module
 * @version 1.0
 */

/**
 * ============================================================================
 * HELPER VALIDATION FUNCTIONS
 * ============================================================================
 */

/**
 * Validate email format
 * @param {string} email - Email to validate
 * @returns {boolean} True if valid, false otherwise
 */
export const validateEmail = (email) => {
    if (!email || typeof email !== 'string') return false;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email) && email.length <= 255;
};

/**
 * Validate password strength
 * Requirements:
 * - Minimum 8 characters
 * - At least 1 uppercase letter
 * - At least 1 lowercase letter
 * - At least 1 number
 * - At least 1 special character
 * @param {string} password - Password to validate
 * @returns {object} { isValid: boolean, errors: string[] }
 */
export const validatePassword = (password) => {
    const errors = [];

    if (!password || typeof password !== 'string') {
        return { isValid: false, errors: ['Password is required'] };
    }

    if (password.length < 8) {
        errors.push('Password must be at least 8 characters');
    }

    if (!/[A-Z]/.test(password)) {
        errors.push('Password must contain at least one uppercase letter');
    }

    if (!/[a-z]/.test(password)) {
        errors.push('Password must contain at least one lowercase letter');
    }

    if (!/[0-9]/.test(password)) {
        errors.push('Password must contain at least one number');
    }

    if (!/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(password)) {
        errors.push('Password must contain at least one special character');
    }

    if (password.length > 128) {
        errors.push('Password must not exceed 128 characters');
    }

    return {
        isValid: errors.length === 0,
        errors
    };
};

/**
 * Validate phone number (international format)
 * @param {string} phone - Phone number to validate
 * @returns {boolean} True if valid, false otherwise
 */
export const validatePhone = (phone) => {
    if (!phone || typeof phone !== 'string') return false;
    // Basic international format: +33612345678 or similar
    const phoneRegex = /^\+?[1-9]\d{1,14}$/;
    return phoneRegex.test(phone.replace(/\s/g, ''));
};

/**
 * Validate date of birth
 * @param {string|Date} dob - Date of birth
 * @returns {object} { isValid: boolean, errors: string[] }
 */
export const validateDateOfBirth = (dob) => {
    const errors = [];

    if (!dob) {
        return { isValid: false, errors: ['Date of birth is required'] };
    }

    const birthDate = new Date(dob);

    // Check if valid date
    if (isNaN(birthDate.getTime())) {
        errors.push('Invalid date format');
        return { isValid: false, errors };
    }

    // Calculate age
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();

    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }

    // Age validation: minimum 16, maximum 100
    if (age < 16) {
        errors.push('You must be at least 16 years old');
    }

    if (age > 100) {
        errors.push('Invalid date of birth');
    }

    // Date should not be in the future
    if (birthDate > today) {
        errors.push('Date of birth cannot be in the future');
    }

    return {
        isValid: errors.length === 0,
        errors,
        age
    };
};

/**
 * Validate gender code
 * @param {number} gender - Gender code (0, 1, 2)
 * @returns {boolean} True if valid
 */
export const validateGender = (gender) => {
    return [0, 1, 2].includes(gender);
};

/**
 * Validate first and last name
 * @param {string} name - Name to validate
 * @returns {boolean} True if valid
 */
export const validateName = (name) => {
    if (!name || typeof name !== 'string') return false;
    // Allow letters, hyphens, and apostrophes
    const nameRegex = /^[a-zA-ZÀ-ÿ\s\-']{2,100}$/;
    return nameRegex.test(name.trim());
};

/**
 * Sanitize string input
 * @param {string} input - Input to sanitize
 * @returns {string} Sanitized input
 */
export const sanitizeInput = (input) => {
    if (typeof input !== 'string') return '';
    return input.trim().replace(/[<>]/g, '');
};

/**
 * Sanitize email (normalize)
 * @param {string} email - Email to sanitize
 * @returns {string} Sanitized email
 */
export const sanitizeEmail = (email) => {
    if (typeof email !== 'string') return '';
    return email.trim().toLowerCase();
};

/**
 * Sanitize phone
 * @param {string} phone - Phone to sanitize
 * @returns {string} Sanitized phone
 */
export const sanitizePhone = (phone) => {
    if (typeof phone !== 'string') return '';
    return phone.trim().replace(/\s/g, '');
};

/**
 * ============================================================================
 * LEAD REGISTRATION VALIDATION
 * ============================================================================
 */

/**
 * Validate lead registration data
 * @param {object} data - Registration data
 * @returns {object} { isValid: boolean, errors: object }
 */
export const validateLeadRegistration = (data) => {
    const errors = {};

    // Email validation
    if (!data.email) {
        errors.email = 'Email is required';
    } else if (!validateEmail(data.email)) {
        errors.email = 'Invalid email format';
    } else if (data.email.length > 255) {
        errors.email = 'Email too long (max 255 characters)';
    }

    // Password validation
    const passwordValidation = validatePassword(data.password);
    if (!passwordValidation.isValid) {
        errors.password = passwordValidation.errors[0]; // Show first error
    }

    // Confirm password
    if (!data.password_confirm) {
        errors.password_confirm = 'Please confirm your password';
    } else if (data.password !== data.password_confirm) {
        errors.password_confirm = 'Passwords do not match';
    }

    // First name
    if (!data.first_name) {
        errors.first_name = 'First name is required';
    } else if (!validateName(data.first_name)) {
        errors.first_name = 'Invalid first name (2-100 characters, letters only)';
    }

    // Last name
    if (!data.last_name) {
        errors.last_name = 'Last name is required';
    } else if (!validateName(data.last_name)) {
        errors.last_name = 'Invalid last name (2-100 characters, letters only)';
    }

    // Phone
    if (!data.phone) {
        errors.phone = 'Phone number is required';
    } else if (!validatePhone(data.phone)) {
        errors.phone = 'Invalid phone number format (use international format: +33...)';
    }

    // Date of birth
    if (!data.date_of_birth) {
        errors.date_of_birth = 'Date of birth is required';
    } else {
        const dobValidation = validateDateOfBirth(data.date_of_birth);
        if (!dobValidation.isValid) {
            errors.date_of_birth = dobValidation.errors[0];
        }
    }

    // Gender (optional at registration, but if provided must be valid)
    if (data.gender !== undefined && data.gender !== null) {
        if (!validateGender(data.gender)) {
            errors.gender = 'Invalid gender value (0, 1, or 2)';
        }
    }

    // Terms acceptance (implied)
    if (!data.accept_terms) {
        errors.accept_terms = 'You must accept the terms and conditions';
    }

    return {
        isValid: Object.keys(errors).length === 0,
        errors
    };
};

/**
 * ============================================================================
 * LEAD LOGIN VALIDATION
 * ============================================================================
 */

/**
 * Validate lead login data
 * @param {object} data - Login data
 * @returns {object} { isValid: boolean, errors: object }
 */
export const validateLeadLogin = (data) => {
    const errors = {};

    // Email validation (can be email or username)
    if (!data.email) {
        errors.email = 'Email is required';
    } else if (typeof data.email !== 'string') {
        errors.email = 'Email must be a string';
    } else if (data.email.trim().length === 0) {
        errors.email = 'Email cannot be empty';
    }

    // Password validation
    if (!data.password) {
        errors.password = 'Password is required';
    } else if (typeof data.password !== 'string') {
        errors.password = 'Password must be a string';
    } else if (data.password.length === 0) {
        errors.password = 'Password cannot be empty';
    }

    return {
        isValid: Object.keys(errors).length === 0,
        errors
    };
};

/**
 * ============================================================================
 * LEAD UPDATE VALIDATION
 * ============================================================================
 */

/**
 * Validate lead profile update
 * @param {object} data - Update data
 * @param {string} updateType - Type of update (email, password, profile, etc.)
 * @returns {object} { isValid: boolean, errors: object }
 */
export const validateLeadUpdate = (data, updateType = 'profile') => {
    const errors = {};

    switch (updateType) {
        case 'email':
            if (!data.new_email) {
                errors.new_email = 'New email is required';
            } else if (!validateEmail(data.new_email)) {
                errors.new_email = 'Invalid email format';
            }
            if (!data.password) {
                errors.password = 'Password required to change email';
            }
            break;

        case 'password':
            if (!data.current_password) {
                errors.current_password = 'Current password is required';
            }
            const newPasswordValidation = validatePassword(data.new_password);
            if (!newPasswordValidation.isValid) {
                errors.new_password = newPasswordValidation.errors[0];
            }
            if (!data.password_confirm) {
                errors.password_confirm = 'Please confirm new password';
            } else if (data.new_password !== data.password_confirm) {
                errors.password_confirm = 'Passwords do not match';
            }
            break;

        case 'phone':
            if (!data.phone) {
                errors.phone = 'Phone number is required';
            } else if (!validatePhone(data.phone)) {
                errors.phone = 'Invalid phone number format';
            }
            break;

        case 'profile':
            // Optional: first_name, last_name, gender
            if (data.first_name && !validateName(data.first_name)) {
                errors.first_name = 'Invalid first name';
            }
            if (data.last_name && !validateName(data.last_name)) {
                errors.last_name = 'Invalid last name';
            }
            if (data.gender !== undefined && data.gender !== null && !validateGender(data.gender)) {
                errors.gender = 'Invalid gender value';
            }
            break;

        default:
            errors.updateType = 'Invalid update type';
    }

    return {
        isValid: Object.keys(errors).length === 0,
        errors
    };
};

/**
 * ============================================================================
 * LEAD PROFILE VALIDATION
 * ============================================================================
 */

/**
 * Validate lead profile data (LEAD_PROFILES)
 * @param {object} data - Profile data
 * @returns {object} { isValid: boolean, errors: object }
 */
export const validateLeadProfile = (data) => {
    const errors = {};

    // Position
    if (!data.desired_position) {
        errors.desired_position = 'Desired position is required';
    } else if (!Number.isInteger(data.desired_position) || data.desired_position < 1 || data.desired_position > 9) {
        errors.desired_position = 'Invalid position code (1-9)';
    }

    // Call center experience
    if (!data.call_center_experience) {
        errors.call_center_experience = 'Call center experience is required';
    } else if (!Number.isInteger(data.call_center_experience) || data.call_center_experience < 1 || data.call_center_experience > 7) {
        errors.call_center_experience = 'Invalid experience level (1-7)';
    }

    // Call center self assessment
    if (!data.call_center_self_assessment) {
        errors.call_center_self_assessment = 'Self assessment is required';
    } else if (!Number.isInteger(data.call_center_self_assessment) || data.call_center_self_assessment < 1 || data.call_center_self_assessment > 5) {
        errors.call_center_self_assessment = 'Invalid assessment (1-5)';
    }

    // Activities validation
    if (!data.activities || !Array.isArray(data.activities) || data.activities.length === 0) {
        errors.activities = 'At least one activity is required';
    } else if (data.activities.length > 3) {
        errors.activities = 'Maximum 3 activities allowed';
    } else {
        data.activities.forEach((activity, index) => {
            if (!activity.activity_type || activity.activity_type < 1 || activity.activity_type > 4) {
                errors[`activities[${index}].activity_type`] = 'Invalid activity type (1-4)';
            }
            if (!activity.activity_experience || activity.activity_experience < 1 || activity.activity_experience > 7) {
                errors[`activities[${index}].activity_experience`] = 'Invalid experience level (1-7)';
            }
            if (activity.operations && Array.isArray(activity.operations) && activity.operations.length > 3) {
                errors[`activities[${index}].operations`] = 'Maximum 3 operations per activity';
            }
        });
    }

    // Work preferences
    if (data.work_preferences) {
        if (data.work_preferences.mode && ![1, 2, 3].includes(data.work_preferences.mode)) {
            errors['work_preferences.mode'] = 'Invalid work mode (1-3)';
        }
        if (data.work_preferences.time && ![1, 2, 3, 4, 5].includes(data.work_preferences.time)) {
            errors['work_preferences.time'] = 'Invalid work time (1-5)';
        }
        if (data.work_preferences.shift && ![1, 2, 3].includes(data.work_preferences.shift)) {
            errors['work_preferences.shift'] = 'Invalid shift (1-3)';
        }
        if (data.work_preferences.preferred_cities && Array.isArray(data.work_preferences.preferred_cities)) {
            if (data.work_preferences.preferred_cities.length > 5) {
                errors['work_preferences.preferred_cities'] = 'Maximum 5 preferred cities';
            }
        }
    }

    // Bio (optional)
    if (data.bio && typeof data.bio === 'string' && data.bio.length > 500) {
        errors.bio = 'Bio must not exceed 500 characters';
    }

    return {
        isValid: Object.keys(errors).length === 0,
        errors
    };
};

/**
 * ============================================================================
 * SUMMARY VALIDATION FUNCTION
 * ============================================================================
 */

/**
 * Main validation dispatcher
 * @param {object} data - Data to validate
 * @param {string} validationType - Type of validation to perform
 * @returns {object} { isValid: boolean, errors: object }
 */
export const validateLeadData = (data, validationType) => {
    switch (validationType) {
        case 'registration':
            return validateLeadRegistration(data);
        case 'login':
            return validateLeadLogin(data);
        case 'update':
            return validateLeadUpdate(data, data.updateType || 'profile');
        case 'profile':
            return validateLeadProfile(data);
        default:
            return { isValid: false, errors: { type: 'Unknown validation type' } };
    }
};

export default {
    validateEmail,
    validatePassword,
    validatePhone,
    validateDateOfBirth,
    validateGender,
    validateName,
    sanitizeInput,
    sanitizeEmail,
    sanitizePhone,
    validateLeadRegistration,
    validateLeadLogin,
    validateLeadUpdate,
    validateLeadProfile,
    validateLeadData
};
