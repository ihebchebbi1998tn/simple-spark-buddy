/**
 * @file runPhase3Tests.mjs
 * @description Automated Phase 3 testing script
 * Makes API calls to test all endpoints and generates a test report
 */

import axios from 'axios';
import fs from 'fs';
import path from 'path';
import { dirname } from 'path';
import { fileURLToPath } from 'url';

const __dirname = dirname(fileURLToPath(import.meta.url));

const API_BASE = 'http://localhost:5050/api/v2';
const REPORT_FILE = `${__dirname}/../test-report.json`;

let testResults = [];
let testSummary = {
    total: 0,
    passed: 0,
    failed: 0,
    startTime: new Date(),
    endTime: null
};

// Test state
let leadId = null;
let token = null;
let skillIds = [];
let notificationId = null;
let testEmail = null;

/**
 * Log test result
 */
const logTest = (stepName, passed, message, details = null) => {
    testResults.push({
        step: stepName,
        status: passed ? '✅ PASSED' : '❌ FAILED',
        message,
        details,
        timestamp: new Date().toISOString()
    });

    testSummary.total++;
    if (passed) {
        testSummary.passed++;
        console.log(`✅ ${stepName}: ${message}`);
    } else {
        testSummary.failed++;
        console.log(`❌ ${stepName}: ${message}`);
        if (details) console.log(`   Details: ${JSON.stringify(details)}`);
    }
};

/**
 * Generate and save test report
 */
const generateReport = () => {
    testSummary.endTime = new Date();
    testSummary.duration = `${(testSummary.endTime - testSummary.startTime) / 1000}s`;

    const report = {
        summary: testSummary,
        testResults,
        passRate: `${((testSummary.passed / testSummary.total) * 100).toFixed(2)}%`
    };

    fs.writeFileSync(REPORT_FILE, JSON.stringify(report, null, 2));

    console.log('\n' + '='.repeat(60));
    console.log('TEST SUMMARY');
    console.log('='.repeat(60));
    console.log(`Total Tests: ${testSummary.total}`);
    console.log(`✅ Passed: ${testSummary.passed}`);
    console.log(`❌ Failed: ${testSummary.failed}`);
    console.log(`Pass Rate: ${report.passRate}`);
    console.log(`Duration: ${testSummary.duration}`);
    console.log(`Report saved to: ${REPORT_FILE}`);
    console.log('='.repeat(60) + '\n');

    return testSummary.failed === 0;
};

/**
 * Step 0: Register Lead
 */
const registerLead = async () => {
    try {
        testEmail = `test-${Date.now()}@example.com`;
        const response = await axios.post(`${API_BASE}/leads/register`, {
            email: testEmail,
            password: 'SecurePass123!',
            password_confirm: 'SecurePass123!',
            phone: '+33612345678',
            first_name: 'Jean',
            last_name: 'Dupont',
            date_of_birth: '1990-05-15',
            accept_terms: true
        });

        if (response.status === 201 && response.data.data.lead_id) {
            leadId = response.data.data.lead_id;
            logTest('Step 0', true, `Lead registered with ID: ${leadId}`);
            return true;
        }
    } catch (error) {
        logTest('Step 0', false, 'Failed to register lead', error.response?.data || error.message);
        return false;
    }
};

/**
 * Step 0.25: Verify Email (Activate Account)
 */
const verifyLeadEmail = async () => {
    try {
        const response = await axios.post(`${API_BASE}/leads/${leadId}/verify-email`);

        if (response.status === 200 && response.data.success) {
            logTest('Step 0.25', true, `Email verified, account activated`);
            return true;
        }
    } catch (error) {
        logTest('Step 0.25', false, 'Failed to verify email', error.response?.data || error.message);
        return false;
    }
};

/**
 * Step 0.5: Login Lead
 */
const loginLead = async () => {
    try {
        const response = await axios.post(`${API_BASE}/leads/login`, {
            email: testEmail,
            password: 'SecurePass123!'
        });

        if (response.data.data && response.data.data.token) {
            token = response.data.data.token;
            logTest('Step 0.5', true, `Lead logged in, token received`);
            return true;
        }
    } catch (error) {
        logTest('Step 0.5', false, 'Failed to login', error.response?.data || error.message);
    }
};

/**
 * Helper: Make authenticated request
 */
const authenticatedRequest = async (method, url, data = null) => {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    };

    if (method === 'GET') return axios.get(url, config);
    if (method === 'POST') return axios.post(url, data, config);
    if (method === 'PUT') return axios.put(url, data, config);
    if (method === 'DELETE') return axios.delete(url, config);
};

/**
 * Step 1: Create Language Skill
 */
const createLanguageSkill = async () => {
    if (!token) return logTest('Step 1', false, 'No token available');

    try {
        const response = await authenticatedRequest('POST',
            `${API_BASE}/leads/${leadId}/skills/language`,
            {
                language_code: 'en',
                qualification_type: 1,
                test_method: 1,
                proficiency_level: 5,
                is_native: false,
                completed: true,
                test_date: new Date().toISOString(),
                test_duration_seconds: 3600,
                scores: {
                    linguistic: 85,
                    soft_skills: 80,
                    job_skills: 75,
                    global: 80
                },
                is_verified: true,
                verified_by: 2
            }
        );

        if (response.status === 200 && response.data.data._id) {
            skillIds.push(response.data.data._id);
            logTest('Step 1', true, `Language skill created: ${response.data.data._id}`);
            return true;
        }
    } catch (error) {
        logTest('Step 1', false, 'Failed to create language skill', error.response?.data || error.message);
    }
};

/**
 * Step 2: Create Technical Skill
 */
const createTechnicalSkill = async () => {
    if (!token) return logTest('Step 2', false, 'No token available');

    try {
        const response = await authenticatedRequest('POST',
            `${API_BASE}/leads/${leadId}/skills/technical`,
            {
                skill_name: 'Microsoft Excel',
                skill_category: 5,
                test_method: 2,
                completed: true,
                test_date: new Date().toISOString(),
                test_duration_seconds: 1800,
                scores: {
                    linguistic: 70,
                    soft_skills: 75,
                    job_skills: 85,
                    global: 77
                },
                is_verified: true,
                verified_by: 2
            }
        );

        if (response.status === 201 && response.data.data._id) {
            skillIds.push(response.data.data._id);
            logTest('Step 2', true, `Technical skill created: ${response.data.data._id}`);
            return true;
        }
    } catch (error) {
        logTest('Step 2', false, 'Failed to create technical skill', error.response?.data || error.message);
    }
};

/**
 * Step 3: Create Certification
 */
const createCertification = async () => {
    if (!token) return logTest('Step 3', false, 'No token available');

    try {
        const response = await authenticatedRequest('POST',
            `${API_BASE}/leads/${leadId}/skills/certification`,
            {
                certification_name: 'Google Analytics Certified',
                certification_authority: 'Google',
                certification_number: 'GA-2024-12345',
                certification_expiry_date: new Date(Date.now() + 365 * 24 * 60 * 60 * 1000).toISOString(),
                test_method: 3,
                completed: true,
                test_date: new Date().toISOString(),
                test_duration_seconds: 3600,
                scores: {
                    linguistic: 80,
                    soft_skills: 85,
                    job_skills: 90,
                    global: 85
                },
                is_verified: true,
                verified_by: 2
            }
        );

        if (response.status === 201 && response.data.data._id) {
            skillIds.push(response.data.data._id);
            logTest('Step 3', true, `Certification created: ${response.data.data._id}`);
            return true;
        }
    } catch (error) {
        logTest('Step 3', false, 'Failed to create certification', error.response?.data || error.message);
    }
};

/**
 * Step 4: Update Work Preferences
 */
const updateWorkPreferences = async () => {
    if (!token) return logTest('Step 4', false, 'No token available');

    try {
        const response = await authenticatedRequest('PUT',
            `${API_BASE}/leads/${leadId}/profile/work-preferences`,
            {
                work_preferences: {
                    mode: 2,
                    time: 1,
                    shift: 1,
                    willing_to_relocate: true,
                    preferred_cities: [1, 2, 3]
                }
            }
        );

        if (response.status === 200) {
            logTest('Step 4', true, 'Work preferences updated');
            return true;
        }
    } catch (error) {
        logTest('Step 4', false, 'Failed to update work preferences', error.response?.data || error.message);
    }
};

/**
 * Step 5: Update Compensation
 */
const updateCompensation = async () => {
    if (!token) return logTest('Step 5', false, 'No token available');

    try {
        const response = await authenticatedRequest('PUT',
            `${API_BASE}/leads/${leadId}/profile/compensation`,
            {
                compensation: {
                    salary_expectation: {
                        min: 250000,
                        max: 350000,
                        currency: 1,
                        period: 2
                    },
                    benefits_preferences: {
                        health_insurance: true,
                        tickets_restaurant: true,
                        transport_allowance: true,
                        performance_bonus: true
                    }
                }
            }
        );

        if (response.status === 200) {
            logTest('Step 5', true, 'Compensation updated');
            return true;
        }
    } catch (error) {
        logTest('Step 5', false, 'Failed to update compensation', error.response?.data || error.message);
    }
};

/**
 * Step 6: Create Notification
 */
const createNotification = async () => {
    if (!token) return logTest('Step 6', false, 'No token available');

    try {
        const response = await authenticatedRequest('POST',
            `${API_BASE}/leads/${leadId}/notifications`,
            {
                notification_type: 22,
                channel: 1,
                priority: 2,
                title: 'New Job Match Found',
                message: 'We found a job that matches your profile!',
                action_url: 'https://example.com/job/123',
                action_button_text: 'View Job',
                provider: 'sendgrid',
                status: 1,
                email_address: `test-${Date.now() - 1000}@example.com`
            }
        );

        if (response.status === 201 && response.data.data._id) {
            notificationId = response.data.data._id;
            logTest('Step 6', true, `Notification created: ${notificationId}`);
            return true;
        }
    } catch (error) {
        logTest('Step 6', false, 'Failed to create notification', error.response?.data || error.message);
    }
};

/**
 * Step 7: Mark Notification as Read
 */
const markNotificationAsRead = async () => {
    if (!token || !notificationId) return logTest('Step 7', false, 'Missing token or notification ID');

    try {
        const response = await authenticatedRequest('POST',
            `${API_BASE}/leads/${leadId}/notifications/${notificationId}/mark-read`,
            {}
        );

        if (response.status === 200) {
            logTest('Step 7', true, 'Notification marked as read');
            return true;
        }
    } catch (error) {
        logTest('Step 7', false, 'Failed to mark notification as read', error.response?.data || error.message);
    }
};

/**
 * Step 8: Update User Settings
 */
const updateUserSettings = async () => {
    if (!token) return logTest('Step 8', false, 'No token available');

    try {
        const response = await authenticatedRequest('PUT',
            `${API_BASE}/leads/${leadId}/settings`,
            {
                ui_preferences: {
                    language: 'en',
                    theme: 'dark'
                },
                notification_preferences: {
                    email_enabled: true,
                    sms_enabled: false,
                    push_enabled: true,
                    marketing_email_enabled: false
                }
            }
        );

        if (response.status === 200) {
            logTest('Step 8', true, 'User settings updated');
            return true;
        }
    } catch (error) {
        logTest('Step 8', false, 'Failed to update user settings', error.response?.data || error.message);
    }
};

/**
 * Step 9: Update Notification Preference
 */
const updateNotificationPreference = async () => {
    if (!token) return logTest('Step 9', false, 'No token available');

    try {
        const response = await authenticatedRequest('PUT',
            `${API_BASE}/leads/${leadId}/settings/notifications/email`,
            { enabled: false }
        );

        if (response.status === 200) {
            logTest('Step 9', true, 'Notification preference updated');
            return true;
        }
    } catch (error) {
        logTest('Step 9', false, 'Failed to update notification preference', error.response?.data || error.message);
    }
};

/**
 * Step 10: Delete a Skill
 */
const deleteSkill = async () => {
    if (!token || skillIds.length === 0) return logTest('Step 10', false, 'No token or skills available');

    try {
        const skillIdToDelete = skillIds[0];
        const response = await authenticatedRequest('DELETE',
            `${API_BASE}/skills/${skillIdToDelete}`,
            null
        );

        if (response.status === 200 || response.status === 204) {
            logTest('Step 10', true, `Skill deleted: ${skillIdToDelete}`);
            return true;
        }
    } catch (error) {
        logTest('Step 10', false, 'Failed to delete skill', error.response?.data || error.message);
    }
};

/**
 * Step 10.5: Log Activity
 */
const logActivity = async () => {
    if (!token) return logTest('Step 10.5', false, 'No token available');

    try {
        const response = await authenticatedRequest('POST',
            `${API_BASE}/activity-logs`,
            {
                lead_id: leadId,
                is_authenticated: true,
                action_type: 1, // 1-53 numeric code
                description: 'Test profile update',
                session_id: `session-${Date.now()}`,
                ip_address: '192.168.1.1',
                user_agent: 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36',
                device_type: 1, // 1-4 numeric code
                source: 'web',
                metadata: {
                    fields_updated: ['first_name', 'last_name'],
                    timestamp: new Date().toISOString()
                }
            }
        );

        if (response.status === 201 || response.status === 200) {
            logTest('Step 10.5', true, `Activity logged: ${response.data.data?._id || 'success'}`);
            return true;
        }
    } catch (error) {
        logTest('Step 10.5', false, 'Failed to log activity', error.response?.data || error.message);
    }
};

/**
 * Step 10.6: Get Activity Logs
 */
const getActivityLogs = async () => {
    if (!token) return logTest('Step 10.6', false, 'No token available');

    try {
        const response = await authenticatedRequest('GET',
            `${API_BASE}/leads/${leadId}/activity-logs?page=1&limit=10`,
            null
        );

        if (response.status === 200 && response.data.success) {
            const activities = response.data.data?.activities || [];
            logTest('Step 10.6', true, `Retrieved ${activities.length} activity logs`);
            return true;
        }
    } catch (error) {
        logTest('Step 10.6', false, 'Failed to get activity logs', error.response?.data || error.message);
    }
};

/**
 * Step 10.7: Get Activity Statistics
 */
const getActivityStats = async () => {
    if (!token) return logTest('Step 10.7', false, 'No token available');

    try {
        const response = await authenticatedRequest('GET',
            `${API_BASE}/leads/${leadId}/activity-logs/stats`,
            null
        );

        if (response.status === 200 && response.data.success) {
            logTest('Step 10.7', true, `Activity stats retrieved: ${response.data.data?.total_activities || 0} activities`);
            return true;
        }
    } catch (error) {
        logTest('Step 10.7', false, 'Failed to get activity stats', error.response?.data || error.message);
    }
};

/**
 * Step 10.8: Get Failed Logins
 */
const getFailedLogins = async () => {
    if (!token) return logTest('Step 10.8', false, 'No token available');

    try {
        const response = await authenticatedRequest('GET',
            `${API_BASE}/activity-logs/security/failed-logins/${leadId}`,
            null
        );

        if (response.status === 200 && response.data.success) {
            const failedAttempts = response.data.data?.failed_attempts || [];
            logTest('Step 10.8', true, `Retrieved ${failedAttempts.length} failed login attempts`);
            return true;
        }
    } catch (error) {
        logTest('Step 10.8', false, 'Failed to get failed logins', error.response?.data || error.message);
    }
};

/**
 * Step 10.9: Get Suspicious Activity
 */
const getSuspiciousActivity = async () => {
    if (!token) return logTest('Step 10.9', false, 'No token available');

    try {
        const response = await authenticatedRequest('GET',
            `${API_BASE}/activity-logs/security/suspicious/${leadId}`,
            null
        );

        if (response.status === 200 && response.data.success) {
            const suspicious = response.data.data?.suspicious_activities || [];
            logTest('Step 10.9', true, `Retrieved ${suspicious.length} suspicious activities`);
            return true;
        }
    } catch (error) {
        logTest('Step 10.9', false, 'Failed to get suspicious activity', error.response?.data || error.message);
    }
};

/**
 * Run all tests
 */
const runAllTests = async () => {
    console.log('\n' + '='.repeat(60));
    console.log('PHASE 3: ADVANCED TESTING - API TEST SUITE');
    console.log('='.repeat(60) + '\n');

    // Registration & Login (Prerequisites)
    console.log('📝 Prerequisites...\n');
    await registerLead();
    await verifyLeadEmail();
    await loginLead();

    if (!leadId || !token) {
        console.log('\n❌ Cannot proceed without lead and token\n');
        generateReport();
        process.exit(1);
    }

    // Phase 3 Steps
    console.log('\n🧪 Running Phase 3 Tests...\n');
    await createLanguageSkill();
    await createTechnicalSkill();
    await createCertification();
    await updateWorkPreferences();
    await updateCompensation();
    await createNotification();
    await markNotificationAsRead();
    await updateUserSettings();
    await updateNotificationPreference();
    await deleteSkill();

    // Activity Log Tests
    console.log('\n📋 Running Activity Log Tests...\n');
    await logActivity();
    await getActivityLogs();
    await getActivityStats();
    await getFailedLogins();
    await getSuspiciousActivity();

    // Generate report
    const allPassed = generateReport();
    process.exit(allPassed ? 0 : 1);
};

// Run tests
runAllTests().catch(error => {
    console.error('Fatal error:', error);
    generateReport();
    process.exit(1);
});
