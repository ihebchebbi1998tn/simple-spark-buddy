import express from 'express';
import * as profileController from '../controllers/profileController.mjs';

const router = express.Router({ mergeParams: true });

/**
 * @route   GET /api/v2/leads/:leadId/profile
 * @desc    Get full lead profile information
 * @access  Private (should add auth middleware)
 */
router.get('/', profileController.getProfile);

/**
 * @route   PUT /api/v2/leads/:leadId/profile
 * @desc    Update full lead profile (all sections)
 * @access  Private (should add auth middleware)
 */
router.put('/', profileController.updateLeadProfile);

/**
 * @route   PUT /api/v2/leads/:leadId/profile/activities
 * @desc    Update lead work activities (1-3 activities with operations)
 * @body    {
 *            activities: [
 *              {
 *                activity_type: 1-4,
 *                activity_experience: 1-7,
 *                operations: [{operation_type: 1-10, operation_experience: 1-7}, ...]
 *              }
 *            ]
 *          }
 * @access  Private (should add auth middleware)
 */
router.put('/activities', profileController.updateActivitiesEndpoint);

/**
 * @route   PUT /api/v2/leads/:leadId/profile/work-preferences
 * @desc    Update lead work mode, schedule, shift, relocation, and city preferences
 * @body    {
 *            mode: 1|2|3,           // 1=Remote, 2=On-site, 3=Hybrid
 *            time: 1-5,             // Schedule type
 *            shift: 1-3,            // Shift type
 *            willing_to_relocate: boolean,
 *            preferred_cities: [cityId1, cityId2, ...] // max 5
 *          }
 * @access  Private (should add auth middleware)
 */
router.put('/work-preferences', profileController.updateWorkPreferencesEndpoint);

/**
 * @route   PUT /api/v2/leads/:leadId/profile/compensation
 * @desc    Update lead salary expectations and benefits preferences
 * @body    {
 *            salary_expectation: {
 *              min_cents: 100000,
 *              max_cents: 150000,
 *              currency_code: 1-10,  // 1=EUR, 2=USD, etc.
 *              period: 1-4            // 1=hourly, 2=daily, 3=monthly, 4=annual
 *            },
 *            benefits_preferences: {
 *              health_insurance: boolean,
 *              tickets_restaurant: boolean,
 *              transport_allowance: boolean,
 *              performance_bonus: boolean
 *            }
 *          }
 * @access  Private (should add auth middleware)
 */
router.put('/compensation', profileController.updateCompensationEndpoint);

export default router;
