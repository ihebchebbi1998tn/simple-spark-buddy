/**
 * Scoring Validators
 */

export const validateScoringUpdate = (data) => {
    const errors = {};

    // Validate manual fields only (auto-calculated fields should not be updatable via API)
    if (data.notes !== undefined) {
        if (typeof data.notes !== 'string') {
            errors.notes = 'Notes must be a string';
        } else if (data.notes.length > 1000) {
            errors.notes = 'Notes cannot exceed 1000 characters';
        }
    }

    if (data.scoring_adjustments !== undefined) {
        if (typeof data.scoring_adjustments !== 'object' || Array.isArray(data.scoring_adjustments)) {
            errors.scoring_adjustments = 'Scoring adjustments must be an object';
        }
    }

    if (data.flags !== undefined) {
        if (!Array.isArray(data.flags)) {
            errors.flags = 'Flags must be an array';
        } else {
            const validFlags = ['high_potential', 'needs_review', 'blocked', 'priority', 'inactive'];
            const invalidFlags = data.flags.filter(f => !validFlags.includes(f));
            if (invalidFlags.length > 0) {
                errors.flags = `Invalid flags: ${invalidFlags.join(', ')}`;
            }
        }
    }

    if (data.manual_ranking !== undefined) {
        if (typeof data.manual_ranking !== 'number' || data.manual_ranking < 1 || data.manual_ranking > 5) {
            errors.manual_ranking = 'Manual ranking must be a number between 1-5';
        }
    }

    if (data.status !== undefined) {
        const validStatus = ['pending', 'active', 'paused', 'archived'];
        if (!validStatus.includes(data.status)) {
            errors.status = `Status must be one of: ${validStatus.join(', ')}`;
        }
    }

    return { isValid: Object.keys(errors).length === 0, errors };
};

export const validateScoringQuery = (query) => {
    const errors = {};
    let page = 1, limit = 50;

    if (query.page) {
        page = parseInt(query.page);
        if (isNaN(page) || page < 1) errors.page = 'Page must be positive';
        else page = Math.min(page, 1000);
    }

    if (query.limit) {
        limit = parseInt(query.limit);
        if (isNaN(limit) || limit < 1 || limit > 200) errors.limit = 'Limit must be 1-200';
    }

    return { isValid: Object.keys(errors).length === 0, errors, page, limit };
};
