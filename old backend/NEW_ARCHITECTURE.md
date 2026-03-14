# CCM Backend v2.0 - Current Architecture

## 📁 Complete Backend Structure

```
CCM_Backend_Dev-NEW/
├── src/
│   ├── config/
│   │   ├── database.mjs              # MongoDB Atlas connection with singleton pattern
│   │   └── env.mjs                   # Centralized environment configuration
│   │
│   ├── middleware/
│   │   ├── auth.mjs                  # JWT authentication middleware
│   │   ├── errorHandler.mjs         # Global error handling with JSON parse errors
│   │   ├── rateLimiter.mjs          # Rate limiting (15 req/15min)
│   │   └── validation.mjs           # Input validation & sanitization
│   │
│   ├── modules/
│   │   ├── candidates/
│   │   │   ├── controllers/
│   │   │   │   ├── authController.mjs              # Login/Logout/GetCurrentUser
│   │   │   │   ├── candidateAuthController.mjs     # Registration
│   │   │   │   ├── candidateViewController.mjs     # Get complete profile
│   │   │   │   ├── candidateProfileController.mjs  # Update Info/Profile
│   │   │   │   ├── candidatePreferencesController.mjs  # Availability/Contract
│   │   │   │   ├── candidateProximityController.mjs    # Geographic preferences
│   │   │   │   ├── candidateTestController.mjs         # Language test scores
│   │   │   │   └── candidateAccessController.mjs       # Password/Email/Delete
│   │   │   │
│   │   │   ├── models/
│   │   │   │   ├── Candidate.mjs                    # Main contact info
│   │   │   │   ├── CandidateProfile.mjs             # CV/experience/languages
│   │   │   │   ├── CandidateAvailability.mjs        # Work preferences
│   │   │   │   ├── CandidateContractPreferences.mjs # Contract terms
│   │   │   │   ├── CandidateTestScores.mjs          # Multi-language test scores
│   │   │   │   ├── CandidateSystemInfo.mjs          # Auth & system data
│   │   │   │   └── index.mjs                        # Model initialization & indexes
│   │   │   │
│   │   │   ├── routes/
│   │   │   │   ├── auth.mjs                         # Authentication routes
│   │   │   │   └── candidates.mjs                   # Candidate management routes
│   │   │   │
│   │   │   └── index.mjs                            # Module exports
│   │   │
│   │   └── prospects/
│   │       ├── controllers/                         # Future implementation
│   │       ├── models/
│   │       ├── routes/
│   │       └── index.mjs
│   │
│   ├── routes/
│   │   └── index.mjs                 # Main routes aggregator
│   │
│   ├── tests/
│   │   └── apiTester.mjs            # Automated API testing bot (19 tests)
│   │
│   └── utils/
│       ├── jwtHelper.mjs            # JWT token generation & verification
│       ├── passwordHelper.mjs       # Password hashing & validation
│       ├── responseHelper.mjs       # Standardized API responses
│       └── seeder.mjs               # Test data seeder (3 candidates)
│
├── index.mjs                        # Main entry point & server initialization
├── .env                             # Environment variables (MongoDB, JWT secret)
├── package.json                     # Dependencies & scripts
├── TESTING.md                       # Testing guide
└── NEW_ARCHITECTURE.md             # This file
```

---

## 🔐 Security Implementation

### Authentication & Authorization
- **JWT Tokens**: 7-day expiry, signed with SECRET_KEY
- **Bearer Authentication**: Standard Authorization header
- **Token Validation**: Middleware on all protected routes
- **User Context**: Candidate ID extracted from token payload

### Password Security
- **Hashing**: Bcrypt with 10 salt rounds
- **Strength Validation**: Minimum 8 characters
- **Comparison**: Constant-time comparison via bcrypt
- **Change Protection**: Requires current password verification

### Input Validation & Sanitization
- **Email**: Format validation + normalization (lowercase, trim)
- **Phone**: French format validation +216XXXXXXXX
- **Strings**: XSS prevention via HTML entity encoding
- **Required Fields**: Validation middleware on all routes

### Rate Limiting
- **API Endpoints**: 15 requests per 15 minutes per IP
- **Headers**: X-RateLimit-* headers in responses
- **Bypass**: None (applies to all requests)

### Error Handling
- **Sensitive Data**: No password hashes in error messages
- **Stack Traces**: Only in development mode
- **Status Codes**: Proper HTTP codes (400, 401, 403, 404, 409, 500)
- **Duplicate Prevention**: Email and phone uniqueness enforced

### Additional Security
- **Helmet.js**: Security headers (XSS, clickjacking, etc.)
- **CORS**: Configurable allowed origins
- **Account Locking**: After 5 failed login attempts
- **Soft Deletes**: Account deletion marks as deleted, doesn't remove data

---

## 🧪 Testing System

### Automated Tests (19 total)
Run with: `node index.mjs --test`

#### Test Categories:
1. **Health Check** (2 tests)
   - Server health endpoint
   - Root endpoint response

2. **Authentication** (4 tests)
   - Registration with all required fields
   - Login with valid credentials
   - Login rejection with invalid password
   - Protected route access control

3. **Profile Management** (5 tests)
   - Get complete profile
   - Update personal info
   - Update profile/CV
   - Update availability
   - Update contract preferences

4. **Language Tests** (1 test)
   - Submit test scores

5. **Proximity & Access** (2 tests)
   - Update geographic preferences
   - Change password

6. **Access Control** (2 tests)
   - Get current user
   - Logout

7. **Error Handling** (2 tests)
   - 404 for invalid endpoints
   - 400 for malformed JSON

8. **Database** (1 test)
   - Verify seeded data exists

### Test Data
3 seeded candidates with varying completion levels:
- **candidate1@test.com**: Basic signup only
- **candidate2@test.com**: Full profile + 2 language tests
- **candidate3@test.com**: Complete profile + 5 language tests

---

