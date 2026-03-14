# Backend Documentation Index

**Generated:** February 16, 2026  
**API Version:** 2.0.0  
**Status:** ✅ Complete

---

## Documentation Files

### 1. 📖 [API_DOCUMENTATION.md](./API_DOCUMENTATION.md)
**Complete API Reference Guide**

The most comprehensive documentation covering:
- ✅ System overview and architecture
- ✅ Getting started guide
- ✅ Authentication and JWT
- ✅ All 7 core modules with complete endpoint references
- ✅ Data models and schemas (with examples)
- ✅ Error handling and status codes
- ✅ Rate limiting and security
- ✅ Response format standards
- ✅ Complete endpoint reference with examples
- ✅ Testing information
- ✅ Troubleshooting guide

**Best for:** Developers needing detailed endpoint information, request/response examples, data schema understanding.

**Sections:**
1. Overview & Architecture
2. Getting Started
3. Authentication
4. Base Configuration
5. Core Modules (7 modules × ~5 endpoints each)
6. Data Models (7 schemas)
7. Error Handling
8. Rate Limiting
9. Response Format
10. Complete Endpoint Reference
11. Testing
12. Troubleshooting

---

### 2. ⚡ [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md)
**Quick Lookup Guide**

Fast reference for common tasks:
- ✅ Quick start commands
- ✅ Authentication flow summary
- ✅ All endpoints in table format
- ✅ Status codes
- ✅ Field constraints and enums
- ✅ Error examples
- ✅ Common cURL examples
- ✅ Environment variables
- ✅ Rate limits summary
- ✅ Common issues & solutions

**Best for:** Quick lookups during development, finding endpoints fast, copy-paste examples.

**Key Tables:**
- Essential Endpoints (3 formats: Auth, Leads, Skills, Profiles, Notifications, Settings)
- Status Codes Reference
- Field Constraints
- Common Issues & Solutions

---

### 3. 🏗️ [ARCHITECTURE.md](./ARCHITECTURE.md)
**Technical Architecture Guide**

In-depth documentation for developers and architects:
- ✅ Directory structure
- ✅ Core components (6 layers)
- ✅ Design patterns and philosophy
- ✅ Request-response flow diagrams
- ✅ Database relationships
- ✅ Performance optimization
- ✅ Security features
- ✅ Development workflow
- ✅ Testing strategy
- ✅ Deployment checklist
- ✅ Troubleshooting guide
- ✅ Future enhancements

**Best for:** Understanding system design, making architectural decisions, onboarding developers, code reviews.

**Key Topics:**
- MVC Pattern Explanation
- Mongoose Model Design
- Validation Strategy
- Error Handling Hierarchy
- Security Implementation
- Development Workflow

---

## Quick Navigation

### By Use Case

#### "I want to implement a new endpoint"
1. Start with [API_DOCUMENTATION.md](./API_DOCUMENTATION.md) → Data Models section
2. Reference [ARCHITECTURE.md](./ARCHITECTURE.md) → Development Workflow
3. Check [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md) → Field Constraints

#### "I need to call an API endpoint"
1. Check [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md) → Essential Endpoints
2. See examples in [API_DOCUMENTATION.md](./API_DOCUMENTATION.md) → Complete Endpoint Reference
3. Copy cURL example from Quick Reference

#### "I'm getting an error"
1. Look up status code in [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md) → Status Codes
2. Check error examples in [API_DOCUMENTATION.md](./API_DOCUMENTATION.md) → Error Handling
3. See troubleshooting in [ARCHITECTURE.md](./ARCHITECTURE.md) → Troubleshooting

#### "I need to understand the system"
1. Start with [ARCHITECTURE.md](./ARCHITECTURE.md) → Overview
2. Review directory structure
3. Read component layers explanation
4. Check request-response flow diagrams

#### "I'm onboarding a new developer"
1. Read [API_DOCUMENTATION.md](./API_DOCUMENTATION.md) → Getting Started
2. Have them bookmark [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md)
3. Review [ARCHITECTURE.md](./ARCHITECTURE.md) → Development Workflow
4. Run test suite: `npm run test:api`

---

## Documentation Map

```
📚 Complete Documentation (3 files)
│
├─ 📖 API_DOCUMENTATION.md (1,000+ lines)
│  ├─ Overview
│  ├─ Getting Started
│  ├─ Authentication
│  ├─ 7 Modules with 50+ endpoints
│  ├─ 7 Data Models with examples
│  ├─ Error Handling
│  ├─ Testing
│  └─ Troubleshooting
│
├─ ⚡ API_QUICK_REFERENCE.md (400+ lines)
│  ├─ Quick Start
│  ├─ Endpoint Tables
│  ├─ Status Codes
│  ├─ Field Constraints
│  ├─ cURL Examples
│  ├─ Common Issues
│  └─ Environment Vars
│
└─ 🏗️ ARCHITECTURE.md (800+ lines)
   ├─ System Design
   ├─ 6 Layer Architecture
   ├─ Data Models
   ├─ Request Flow
   ├─ Security
   ├─ Development Guide
   ├─ Deployment
   └─ Troubleshooting
```

---

## Key Information Summary

### System Stats

| Metric         | Details                                                                     |
| :------------- | :-------------------------------------------------------------------------- |
| Modules        | 7 — Leads, Profiles, Skills, Scoring, Notifications, ActivityLogs, Settings |
| Endpoints      | 179+ API endpoints                                                          |
| Data Models    | 7 Mongoose schemas                                                          |
| Validators     | 7 module validators + shared validators                                     |
| Services       | 7 module services + helper utilities                                        |
| Controllers    | 7 module controllers + shared functions                                     |
| Routes         | 8 top-level route groups                                                    |
| Authentication | JWT tokens (24-hour expiration)                                             |
| Database       | MongoDB 4.4 or newer                                                        |
| Framework      | Express.js 4.18.2                                                           |
| Rate Limit     | 100 requests per 15 minutes                                                 |


### Core Features

✅ **Authentication**
- JWT tokens (24h expiration)
- Password hashing (bcrypt)
- Account lockout after 5 failed attempts

✅ **Authorization**
- Field-level access control
- User vs Admin settings
- Protected endpoints

✅ **Data Validation**
- Multi-layer validation
- Field-level constraints
- Custom business logic

✅ **Security**
- Helmet.js (security headers)
- Rate limiting
- SQL injection prevention
- XSS protection

✅ **Testing**
- Automated test suite (12 steps)
- Phase 3 comprehensive testing
- 100% pass rate validation

✅ **Monitoring**
- Activity logging (6-month TTL)
- Failed login tracking
- Audit trail

### Quick Commands

```bash
# Start server
npm run start-v2

# Development mode
npm run dev-v2

# Run tests
npm run test:api

# View test report
cat test-report.json
```

---

## Authentication Quick Start

```javascript
// 1. Register
POST /api/v2/leads/register
{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "first_name": "Jean",
  "last_name": "Dupont",
  "phone": "+33612345678",
  "date_of_birth": "1990-05-15"
}

// 2. Verify Email
POST /api/v2/leads/{leadId}/verify-email

// 3. Login
POST /api/v2/leads/login
{
  "email": "user@example.com",
  "password": "SecurePass123!"
}
→ Receive JWT token

// 4. Use token
GET /api/v2/leads/{leadId}
Headers: Authorization: Bearer {token}
```

---

## Common Endpoints Cheat Sheet

### CRUD Operations

```
CREATE (POST)    → Status 201 Created
READ (GET)       → Status 200 OK
UPDATE (PUT)     → Status 200 OK
DELETE (DELETE)  → Status 200 OK / 204 No Content
```

### Module Endpoints Pattern

```
GET     /api/v2/leads/:leadId/{module}            → List all
POST    /api/v2/leads/:leadId/{module}            → Create
GET     /api/v2/leads/:leadId/{module}/:itemId    → Get one
PUT     /api/v2/leads/:leadId/{module}/:itemId    → Update
DELETE  /api/v2/leads/:leadId/{module}/:itemId    → Delete
```

### Special Endpoints

```
POST    /api/v2/leads/register                    → Register (public)
POST    /api/v2/leads/login                       → Login (public)
POST    /api/v2/leads/:id/verify-email            → Verify (public)
POST    /api/v2/leads/:id/notifications/:nId/mark-read  → Action
GET     /api/v2/leads?page=1&limit=20            → Paginated list
```

---

## Status Codes At-a-Glance

```
2xx Success
  200 OK              → GET/PUT/DELETE successful
  201 Created         → POST successful
  204 No Content      → DELETE with no body

4xx Client Error
  400 Bad Request     → Validation failed
  401 Unauthorized    → Invalid token
  403 Forbidden       → Not active / No permission
  404 Not Found       → Resource missing
  409 Conflict        → Email exists
  429 Too Many Req.   → Rate limited

5xx Server Error
  500 Server Error    → Unexpected error
```

---

## Field Validation Rules

### Email
- Required, unique
- Valid email format
- Case-insensitive

### Password
- Min 8 characters
- Must be confirmed
- Bcrypt hashed

### Phone
- International format
- Min 10 digits
- Example: +33612345678

### Enums
- Account Status: 1=Active, 2=Inactive, 3=Suspended, 4=Pending
- Work Mode: 1=Remote, 2=On-site, 3=Hybrid
- Contract: 1=CDI, 2=CDD, 3=Freelance

---

## Testing

### Run Tests
```bash
npm run test:api
```

### Test Coverage
- ✅ Registration & verification
- ✅ Login & JWT generation
- ✅ Language, technical, certification skills
- ✅ Work preferences & compensation
- ✅ Notifications (CRUD + actions)
- ✅ Settings updates
- ✅ Skill deletion
- ✅ Error handling

### Expected Output
```
✅ Step 0: Lead registered
✅ Step 0.25: Email verified, account activated
✅ Step 0.5: Lead logged in, token received
✅ Step 1-10: Various CRUD operations

TEST SUMMARY
Total Tests: 12
✅ Passed: 12
❌ Failed: 0
Pass Rate: 100.00%
```

---

## File Locations

| Item                | Location                                           |
| :------------------ | :------------------------------------------------- |
| API Documentation   | [API_DOCUMENTATION.md](./API_DOCUMENTATION.md)     |
| Quick Reference     | [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md) |
| System Architecture | [ARCHITECTURE.md](./ARCHITECTURE.md)               |
| Server Entry Point  | `index-v2.mjs`                                     |
| Data Models         | `src-v2/modules/*/models/`                         |
| Validators          | `src-v2/modules/*/validators/`                     |
| Services            | `src-v2/modules/*/services/`                       |
| Controllers         | `src-v2/modules/*/controllers/`                    |
| Routes              | `src-v2/modules/*/routes/`                         |
| Test Runner         | `scripts/runPhase3Tests.mjs`                       |
| Environment Config  | `.env`                                             |


---

## Environment Setup

```bash
# .env file
MONGODB_URI=mongodb://localhost:27017/ccm
DB_NAME_V2=CCM_DEV
PORT=5050
SECRET_KEY=your_jwt_secret_key
NODE_ENV=development
```

---

## Support & Resources

- **Swagger Docs:** `http://localhost:5050/api/v2/docs` (interactive)
- **Git Repository:** [Your repo URL]
- **Issue Tracking:** [Your issue tracker]
- **Development Team:** dev@ccm.com

---

## Version Information

| Component | Version Requirement |
| :-------- | :------------------ |
| API       | v2.0.0              |
| Node.js   | 16 or newer         |
| Express   | 4.18.2              |
| Mongoose  | 8.0.0               |
| MongoDB   | 4.4 or newer        |


---

## Next Steps

### For New Developers
1. Read [Getting Started](./API_DOCUMENTATION.md#getting-started)
2. Bookmark [Quick Reference](./API_QUICK_REFERENCE.md)
3. Review [Architecture](./ARCHITECTURE.md)
4. Run test suite: `npm run test:api`
5. Start with a simple endpoint call

### For DevOps/Deployment
1. Review [Deployment Checklist](./ARCHITECTURE.md#deployment-checklist)
2. Configure environment variables
3. Set up MongoDB
4. Run health check: `curl http://localhost:5050/api/v2/health`
5. Run tests: `npm run test:api`

### For Product/Requirements
1. Read [Modules Overview](./API_DOCUMENTATION.md#core-modules)
2. Check [Data Models](./API_DOCUMENTATION.md#data-models)
3. Review [Status Codes](./API_QUICK_REFERENCE.md#status-codes)
4. Understand error cases

---

**Documentation Complete**  
**Last Updated:** February 16, 2026  
**Status:** ✅ Production Ready
