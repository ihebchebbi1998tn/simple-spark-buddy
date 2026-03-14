/**
 * @file apiTester.mjs
 * @description Automated API testing bot for CCM Backend
 */

import { config } from '../config/env.mjs';

const BASE_URL = `http://localhost:${config.port}`;
let authToken = null;
let testCandidateId = null;

class APITester {
  constructor() {
    this.passedTests = 0;
    this.failedTests = 0;
    this.results = [];
  }

  async request(method, endpoint, data = null, useAuth = false) {
    const url = `${BASE_URL}${endpoint}`;
    const options = {
      method,
      headers: {
        'Content-Type': 'application/json',
      }
    };

    if (useAuth && authToken) {
      options.headers['Authorization'] = `Bearer ${authToken}`;
    }

    if (data) {
      options.body = JSON.stringify(data);
    }

    try {
      const response = await fetch(url, options);
      const text = await response.text();
      let json = null;
      
      try {
        json = text ? JSON.parse(text) : null;
      } catch {
        json = { raw: text };
      }

      return {
        status: response.status,
        ok: response.ok,
        data: json
      };
    } catch (error) {
      return {
        status: 0,
        ok: false,
        error: error.message
      };
    }
  }

  async test(name, fn) {
    try {
      await fn();
      this.passedTests++;
      this.results.push({ name, status: '✓', passed: true });
      console.log(`  ✓ ${name}`);
    } catch (error) {
      this.failedTests++;
      this.results.push({ name, status: '✗', passed: false, error: error.message });
      console.log(`  ✗ ${name}: ${error.message}`);
    }
  }

  assert(condition, message) {
    if (!condition) {
      throw new Error(message);
    }
  }

  async runAllTests() {
    console.log('\n🤖 Starting Automated API Tests...\n');
    console.log('═'.repeat(60));

    // Health Check Tests
    console.log('\n📋 Health Check Tests');
    await this.test('Server health check', async () => {
      const res = await this.request('GET', '/health');
      this.assert(res.ok, `Expected 200, got ${res.status}`);
      this.assert(res.data.status === 'healthy', 'Server not healthy');
    });

    await this.test('Root endpoint', async () => {
      const res = await this.request('GET', '/');
      this.assert(res.ok, `Expected 200, got ${res.status}`);
      this.assert(res.data.message.includes('CCM Backend'), 'Invalid root response');
    });

    // Authentication Tests
    console.log('\n🔐 Authentication Tests');
    
    const testEmail = `test_${Date.now()}@example.com`;
    const testPassword = 'TestPass123!';
    const testPhone = `+336${Math.floor(10000000 + Math.random() * 90000000)}`; // Unique phone

    await this.test('Register new candidate', async () => {
      const res = await this.request('POST', '/api/candidates/register', {
        civilite: 'M.',
        nom: 'Testeur',
        prenom: 'Candidat',
        telephone: testPhone,
        villeResidence: 'Paris',
        email: testEmail,
        motDePasse: testPassword,
        posteRecherche: 'Service Client',
        experienceGlobale: '2 ans',
        langueMaternelle: 'Français',
        langueForeign1: 'Anglais',
        niveauLangueForeign1: 'Intermédiaire',
        modeTravail: 'Remote',
        tempsTravail: 'Temps plein',
        parcTravail: 'Paris'
      });
      this.assert(res.ok, `Registration failed: ${res.status} - ${JSON.stringify(res.data)}`);
      this.assert(res.data.data?.token, 'No token returned');
      authToken = res.data.data.token;
      testCandidateId = res.data.data.user?.id;
    });

    await this.test('Login with credentials', async () => {
      const res = await this.request('POST', '/api/auth/login', {
        email: testEmail,
        password: testPassword
      });
      this.assert(res.ok, `Login failed: ${res.status}`);
      this.assert(res.data.data?.token, 'No token in login response');
      authToken = res.data.data.token;
    });

    await this.test('Login with invalid password', async () => {
      const res = await this.request('POST', '/api/auth/login', {
        email: testEmail,
        password: 'WrongPassword123!'
      });
      this.assert(!res.ok, 'Should reject invalid password');
      this.assert(res.status === 401, `Expected 401, got ${res.status}`);
    });

    await this.test('Access protected route without token', async () => {
      const tempToken = authToken;
      authToken = null;
      const res = await this.request('GET', '/api/candidates/profile', null, true);
      this.assert(!res.ok, 'Should reject request without token');
      this.assert(res.status === 401, `Expected 401, got ${res.status}`);
      authToken = tempToken;
    });

    // Profile Management Tests
    console.log('\n👤 Profile Management Tests');

    await this.test('Get candidate profile', async () => {
      const res = await this.request('GET', '/api/candidates/profile', null, true);
      this.assert(res.ok, `Failed to get profile: ${res.status}`);
      this.assert(res.data.data?.candidate || res.data.data?.profile, 'No profile data returned');
    });

    await this.test('Update candidate info', async () => {
      const res = await this.request('PUT', '/api/candidates/info', {
        name: 'Updated',
        surname: 'Name',
        city: 'Lyon'
      }, true);
      this.assert(res.ok, `Failed to update info: ${res.status}`);
    });

    await this.test('Update candidate profile', async () => {
      const res = await this.request('PUT', '/api/candidates/profile', {
        desired_position: 'Téléconseiller',
        call_center_experience: '3 ans'
      }, true);
      this.assert(res.ok, `Failed to update profile: ${res.status}`);
    });

    await this.test('Update availability', async () => {
      const res = await this.request('PUT', '/api/candidates/availability', {
        work_mode: 'Hybride',
        work_time: 'Temps plein',
        work_park: 'Paris'
      }, true);
      this.assert(res.ok, `Failed to update availability: ${res.status}`);
    });

    await this.test('Update contract preferences', async () => {
      const res = await this.request('PUT', '/api/candidates/contract-preferences', {
        expected_salary: 35000,
        contract_type: 'CDI'
      }, true);
      this.assert(res.ok, `Failed to update contract preferences: ${res.status}`);
    });

    // Language Test Tests
    console.log('\n📝 Language Test Tests');

    let firstAlertId = null;

    await this.test('Submit language test scores', async () => {
      const res = await this.request('POST', '/api/candidates/test-scores', {
        language: 'french',
        scores: {
          linguistic: 85,
          softSkills: 78,
          jobSkills: 82,
          overall: 82
        }
      }, true);
      this.assert(res.ok, `Failed to submit test scores: ${res.status}`);
    });

    // Alerts Tests (after profile updates and test submission)
    console.log('\n🔔 Alerts Tests');

    await this.test('Get candidate alerts', async () => {
      const res = await this.request('GET', '/api/candidates/alerts', null, true);
      this.assert(res.ok, `Failed to get alerts: ${res.status}`);
      this.assert(res.data.data?.alerts, 'No alerts array returned');
      this.assert(Array.isArray(res.data.data.alerts), 'Alerts should be an array');
      this.assert(res.data.data.unreadCount !== undefined, 'No unreadCount returned');
      
      // Store first alert ID for later tests
      if (res.data.data.alerts.length > 0) {
        firstAlertId = res.data.data.alerts[0]._id;
      }
    });

    await this.test('Verify alerts contain expected types', async () => {
      const res = await this.request('GET', '/api/candidates/alerts', null, true);
      this.assert(res.ok, `Failed to get alerts: ${res.status}`);
      const alerts = res.data.data.alerts;
      
      // Should have alerts from profile updates and test submission
      this.assert(alerts.length > 0, 'Should have at least one alert');
      
      // Verify alert structure
      const alert = alerts[0];
      this.assert(alert.type, 'Alert should have type');
      this.assert(alert.title, 'Alert should have title');
      this.assert(alert.message, 'Alert should have message');
      this.assert(alert.action_type, 'Alert should have action_type');
      this.assert(alert.read !== undefined, 'Alert should have read status');
    });

    await this.test('Verify unread count is accurate', async () => {
      const res = await this.request('GET', '/api/candidates/alerts', null, true);
      this.assert(res.ok, `Failed to get alerts: ${res.status}`);
      const { alerts, unreadCount } = res.data.data;
      
      const actualUnread = alerts.filter(a => !a.read).length;
      this.assert(unreadCount === actualUnread, `Unread count mismatch: ${unreadCount} vs ${actualUnread}`);
    });

    await this.test('Mark specific alert as read', async () => {
      if (!firstAlertId) {
        throw new Error('No alert ID available for testing');
      }
      
      const res = await this.request('PUT', `/api/candidates/alerts/${firstAlertId}/read`, null, true);
      this.assert(res.ok, `Failed to mark alert as read: ${res.status}`);
      
      // Verify it was marked as read
      const checkRes = await this.request('GET', '/api/candidates/alerts', null, true);
      const alert = checkRes.data.data.alerts.find(a => a._id === firstAlertId);
      this.assert(alert && alert.read === true, 'Alert was not marked as read');
    });

    await this.test('Mark all alerts as read', async () => {
      const res = await this.request('PUT', '/api/candidates/alerts/read-all', null, true);
      this.assert(res.ok, `Failed to mark all alerts as read: ${res.status}`);
      
      // Verify all are read
      const checkRes = await this.request('GET', '/api/candidates/alerts', null, true);
      this.assert(checkRes.data.data.unreadCount === 0, 'All alerts should be marked as read');
    });

    await this.test('Verify test completion alert created', async () => {
      const res = await this.request('GET', '/api/candidates/alerts', null, true);
      this.assert(res.ok, `Failed to get alerts: ${res.status}`);
      const alerts = res.data.data.alerts;
      
      const testAlert = alerts.find(a => a.action_type === 'test_completion');
      this.assert(testAlert, 'Should have alert for test completion');
      this.assert(testAlert.type === 'success', 'Test alert should be success type');
    });

    await this.test('Verify profile update alerts created', async () => {
      const res = await this.request('GET', '/api/candidates/alerts', null, true);
      this.assert(res.ok, `Failed to get alerts: ${res.status}`);
      const alerts = res.data.data.alerts;
      
      const profileUpdateAlert = alerts.find(a => a.action_type === 'profile_update');
      this.assert(profileUpdateAlert, 'Should have alert for profile update');
    });

    await this.test('Delete old alerts (cleanup)', async () => {
      const res = await this.request('DELETE', '/api/candidates/alerts/cleanup', null, true);
      this.assert(res.ok, `Failed to delete old alerts: ${res.status}`);
    });

    // Proximity Tests
    console.log('\n📍 Proximity & Access Tests');

    await this.test('Update proximity preferences', async () => {
      const res = await this.request('PUT', '/api/candidates/proximity', {
        villeResidence: 'Lyon',
        villesProximite: ['Paris', 'Marseille']
      }, true);
      this.assert(res.ok, `Failed to update proximity: ${res.status}`);
    });

    await this.test('Change password', async () => {
      const res = await this.request('PUT', '/api/candidates/password', {
        currentPassword: testPassword,
        newPassword: 'NewTestPass456!'
      }, true);
      this.assert(res.ok, `Failed to change password: ${res.status}`);
    });

    // Access Control Tests
    console.log('\n🔒 Access Control Tests');

    await this.test('Get current user', async () => {
      const res = await this.request('GET', '/api/auth/me', null, true);
      this.assert(res.ok, `Failed to get current user: ${res.status}`);
    });

    await this.test('Logout candidate', async () => {
      const res = await this.request('POST', '/api/auth/logout', null, true);
      this.assert(res.ok, `Failed to logout: ${res.status}`);
    });

    // Error Handling Tests
    console.log('\n⚠️ Error Handling Tests');

    await this.test('Handle invalid endpoint', async () => {
      const res = await this.request('GET', '/api/invalid-endpoint');
      this.assert(res.status === 404, `Expected 404, got ${res.status}`);
    });

    await this.test('Handle malformed JSON (expected error in console)', async () => {
      const url = `${BASE_URL}/api/candidates/register`;
      const response = await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: 'invalid json{{'
      });
      this.assert(response.status === 400, `Expected 400 for malformed JSON, got ${response.status}`);
    });

    // Database Tests
    console.log('\n💾 Database Tests');

    await this.test('Verify seeded candidates exist', async () => {
      const res = await this.request('POST', '/api/auth/login', {
        email: 'candidate1@test.com',
        password: 'Test123!'
      });
      this.assert(res.ok, 'Seeded candidate 1 not found');
    });

    // Summary
    console.log('\n' + '═'.repeat(60));
    console.log('\n📊 Test Summary');
    console.log(`  Total Tests: ${this.passedTests + this.failedTests}`);
    console.log(`  ✓ Passed: ${this.passedTests}`);
    console.log(`  ✗ Failed: ${this.failedTests}`);
    console.log(`  Success Rate: ${((this.passedTests / (this.passedTests + this.failedTests)) * 100).toFixed(1)}%`);
    
    if (this.failedTests > 0) {
      console.log('\n❌ Failed Tests:');
      this.results
        .filter(r => !r.passed)
        .forEach(r => console.log(`  • ${r.name}: ${r.error}`));
    }

    console.log('\n' + '═'.repeat(60));
    console.log(this.failedTests === 0 ? '✅ All tests passed!\n' : '⚠️ Some tests failed!\n');

    return this.failedTests === 0;
  }
}

export async function runAPITests() {
  const tester = new APITester();
  
  // Wait for server to be ready
  console.log('⏳ Waiting for server to start...');
  await new Promise(resolve => setTimeout(resolve, 2000));
  
  // Clean up test data before running tests
  try {
    const cleanupUrl = `${BASE_URL}/api/candidates/register`;
    // We can't directly access the database here, so we'll just run the tests
    // The tests will create new unique data each time based on timestamp
  } catch (error) {
    console.log('⚠️  Could not clean up test data:', error.message);
  }
  
  const allPassed = await tester.runAllTests();
  
  if (!allPassed) {
    process.exit(1);
  }
}
