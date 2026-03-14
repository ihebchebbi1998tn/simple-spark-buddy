# CCM Backend - Automated Testing Guide

## Running Tests

The backend includes an automated API testing bot that validates all endpoints and functionality.

### Test Command

```bash
node index.mjs --test
```

### What Gets Tested

The automated test suite includes:

#### ✅ Health Check Tests
- Server health endpoint
- Root endpoint response

#### ✅ Authentication Tests
- User registration with new account
- Login with valid credentials
- Login rejection with invalid password
- Protected route access control
- Token validation

#### ✅ Profile Management Tests
- Retrieve candidate profile
- Update profile information
- Update availability settings
- Update contract preferences

#### ✅ Language Test Tests
- Submit test scores for languages
- Retrieve test scores

#### ✅ Alerts Tests
- Get candidate alerts
- Verify alerts contain expected types
- Verify unread count is accurate
- Mark specific alert as read
- Mark all alerts as read
- Verify test completion alert created
- Verify profile update alerts created
- Delete old alerts (cleanup)

#### ✅ Preference Tests
- Update notification preferences
- Update privacy settings

#### ✅ Access Control Tests
- Logout functionality
- Token expiration handling

#### ✅ Error Handling Tests
- Invalid endpoint handling (404)
- Malformed request handling
- Invalid data validation

#### ✅ Database Tests
- Verify seeded test data exists
- Database connection stability

## Test Output

The test suite provides detailed output:

```
🤖 Starting Automated API Tests...

📋 Health Check Tests
  ✓ Server health check
  ✓ Root endpoint

🔐 Authentication Tests
  ✓ Register new candidate
  ✓ Login with credentials
  ✓ Login with invalid password
  ...

📊 Test Summary
  Total Tests: 28
  ✓ Passed: 28
  ✗ Failed: 0
  Success Rate: 100%

✅ All tests passed!
```

## Test Accounts

After seeding, these test accounts are available:

1. **candidate1@test.com** / Test123! - Basic signup
2. **candidate2@test.com** / Test123! - Profile completed + language tests
3. **candidate3@test.com** / Test123! - Fully completed profile

## Continuous Integration

The test suite can be integrated into CI/CD pipelines:

```bash
# Run tests and exit with code 1 if any fail
node index.mjs --test
```

## Adding New Tests

Edit `src/tests/apiTester.mjs` and add new test cases:

```javascript
await this.test('Test name', async () => {
  const res = await this.request('GET', '/api/endpoint', null, true);
  this.assert(res.ok, 'Test failed');
});
```

## Notes

- Tests run automatically after server starts when `--test` flag is used
- Server continues running after tests complete
- Tests use real database connections and create test data
- Failed tests return exit code 1 for CI/CD integration
