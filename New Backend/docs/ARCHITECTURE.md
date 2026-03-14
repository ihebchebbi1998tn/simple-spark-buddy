# Backend V2 Architecture Guide

**Version:** 2.0.0  
**Date:** February 16, 2026  
**Status:** Production Ready

---

## Overview

The Backend V2 represents a complete architectural overhaul from V1, introducing a modern MVC pattern with Mongoose ODM, comprehensive validation, and nested resource management.

### Design Philosophy

- **Separation of Concerns:** Models ↔ Validators ↔ Services ↔ Controllers → Routes
- **Nested Resources:** Hierarchical relationship structure (leads with 6 child modules)
- **Field-Level Control:** Granular access control for user vs admin operations
- **Immutable Defaults:** Auto-generated IDs, timestamps, and derived fields
- **Fail-Safe Validation:** Multi-layer validation at validator and model levels

---

## Directory Structure

```
backend/
├── index-v2.mjs                    # V2 server entry point
├── .env                            # Environment vars
├── package.json                    # Dependencies
│
├── src-v2/                         # V2 application code
│   ├── config/
│   │   ├── database.mjs            # Mongoose connection
│   │   ├── env.mjs                 # Config from ../src/
│   │   └── swagger.mjs             # OpenAPI 3.0 docs
│   │
│   ├── middleware/
│   │   ├── auth.mjs                # JWT verification
│   │   ├── errorHandler.mjs        # Global error handling
│   │   ├── rateLimiter.mjs         # Rate limiting
│   │   └── validation.mjs          # Request validation
│   │
│   └── modules/
│       ├── leads/
│       │   ├── models/
│       │   │   └── Lead.mjs
│       │   ├── validators/
│       │   │   └── leadValidator.mjs
│       │   ├── services/
│       │   │   └── leadService.mjs
│       │   ├── controllers/
│       │   │   └── leadController.mjs
│       │   ├── routes/
│       │   │   └── leadRoutes.mjs
│       │   └── tests/
│       │       └── leadTests.mjs
│       │
│       ├── profiles/          # LeadProfile module
│       ├── skills/            # LeadSkill module
│       ├── scoring/           # LeadScoring module
│       ├── notifications/     # LeadNotification module
│       ├── activityLogs/      # LeadActivityLog module
│       └── settings/          # LeadSettings module
│
├── scripts/
│   ├── runPhase3Tests.mjs      # Automated test suite
│   └── checkAndTest.mjs        # Installation checker (optional)
│
├── API_DOCUMENTATION.md        # Complete API reference
├── API_QUICK_REFERENCE.md      # Quick lookup guide
└── NEW_ARCHITECTURE.md         # This file

```

---

## Core Components

### 1. Database Connection

**File:** `src-v2/config/database.mjs`

```javascript
// Singleton pattern for connection
class DatabaseConnection {
  async connect() {
    // Reuses existing connection
  }
}

export const db = new DatabaseConnection();
```

**Features:**
- Singleton pattern prevents multiple connections
- Connection pooling via Mongoose
- Error handling and reconnection logic

---

### 2. Mongoose Models

**Pattern:** One model per entity

```
Lead.mjs          → User accounts (identity)
LeadProfile.mjs   → Extended profile (work history, preferences)
LeadSkill.mjs     → Skills (languages, technical, certs)
LeadScoring.mjs   → Qualification scores
LeadNotification.mjs → Multi-channel notifications
LeadActivityLog.mjs → Audit trail (auto-cleanup after 6 months)
LeadSettings.mjs  → User and admin settings
```

**Schema Design:**
- Flat structure with nested sub-documents where logical
- Indexes on frequently queried fields
- TTL indexes for auto-deletion (activity logs)
- Timestamps on all records (createdAt, updatedAt)

---

### 3. Validators

**Layer:** Input validation before database operations

**File Pattern:** `{module}Validator.mjs`

```javascript
// Example: leadValidator.mjs
export const validateLeadRegistration = (data) => {
  const errors = {};
  const isValid = true; // Or false with errors
  return { isValid, errors };
};
```

**Validation Types:**
- Email uniqueness check
- Password strength (min 8 chars)
- Phone number format (international)
- Field type validation
- Enum validation (statuses, types)
- Custom business logic validation

---

### 4. Services

**Layer:** Business logic, database operations

**File Pattern:** `{module}Service.mjs`

```javascript
// Example: leadService.mjs
export const createLead = async (data) => {
  // Business logic
  const lead = new Lead(data);
  await lead.save();
  return lead;
};

export const getLeadById = async (id) => {
  return Lead.findById(id).select('-password_hash');
};
```

**Responsibilities:**
- Create, read, update, delete operations
- Data transformation
- Password hashing (bcrypt)
- Token generation (JWT)
- Relationship management
- Field-level access control

---

### 5. Controllers

**Layer:** HTTP request/response handling

**File Pattern:** `{module}Controller.mjs`

```javascript
// Example: leadController.mjs
export const registerLead = async (req, res) => {
  try {
    // 1. Validate input
    const validation = validateLeadRegistration(req.body);
    if (!validation.isValid) {
      return res.status(400).json({
        success: false,
        errors: validation.errors
      });
    }

    // 2. Call service
    const lead = await createLead(req.body);

    // 3. Return response
    return res.status(201).json({
      success: true,
      data: lead
    });
  } catch (error) {
    return res.status(500).json({
      success: false,
      error: error.message
    });
  }
};
```

**Responsibilities:**
- Parse HTTP requests
- Call services/validators
- Format responses
- Error handling and status codes
- Response serialization

---

### 6. Routes

**Layer:** URL pattern matching

**File Pattern:** `{module}Routes.mjs`

```javascript
// Example: leadRoutes.mjs
router.post('/register', leadController.registerLead);
router.post('/login', leadController.loginLead);
router.get('/:leadId', authMiddleware, leadController.getLeadProfile);
router.put('/:leadId/profile', authMiddleware, leadController.updateLeadProfileEndpoint);
```

**Nested Pattern:**
```
/api/v2/leads/:leadId/
  ├── /profile (profiles routes)
  ├── /skills (skills routes)
  ├── /scoring (scoring routes)
  ├── /notifications (notifications routes)
  ├── /activity-logs (activity logs routes)
  └── /settings (settings routes)
```

---

## Key Features

### Authentication

**JWT Implementation:**
```javascript
// Generated on login
const token = jwt.sign(
  {
    id: lead._id,
    email: lead.email,
    role: 'user'
  },
  config.jwt.secret,
  { expiresIn: '24h' }
);
```

**Verification Middleware:**
```javascript
// Applied to protected routes
export const authMiddleware = (req, res, next) => {
  const token = req.headers.authorization?.split(' ')[1];
  if (!token) return res.status(401).json({ message: 'No token' });
  
  try {
    const decoded = jwt.verify(token, config.jwt.secret);
    req.userId = decoded.id;
    next();
  } catch (error) {
    res.status(401).json({ message: 'Invalid token' });
  }
};
```

### Field-Level Access Control

**Example: LeadSettings**

```javascript
// User-editable via validator
export const validateUserSettings = (data) => {
  const allowed = ['theme', 'language', 'notification_frequency'];
  // Only allow whitelisted fields
};

// Admin-only fields require different validator
export const validateAdminSettings = (data) => {
  const allowed = ['verified', 'approved', 'blacklisted'];
  // Admin-specific fields
};
```

### Data Validation

**Multi-Layer Approach:**

1. **Route Level** - Early rejection of malformed requests
2. **Controller Level** - Business rule validation
3. **Service Level** - Database constraint validation
4. **Model Level** - Schema-level validation (Mongoose)

### Auto-Generated Fields

```javascript
// Public ID (human-readable)
public_id: {
  type: String,
  unique: true,
  default: () => `LD-${new Date().getFullYear()}-${Math.random()...}`
}

// Timestamps
createdAt: { type: Date, default: Date.now }
updatedAt: { type: Date, default: Date.now }

// TTL Index (auto-delete after 6 months)
expireAt: {
  type: Date,
  default: () => new Date(Date.now() + 180 * 24 * 60 * 60 * 1000),
  index: { expireAfterSeconds: 0 }
}
```

---

## Request-Response Flow

### Registration Flow

```
Client
  ↓ POST /leads/register
Server (index-v2.mjs)
  ↓ Route matching
leadRoutes.mjs
  ↓ Handler assignment
leadController.registerLead()
  ↓ Validation
leadValidator.validateLeadRegistration()
  ↓ Business logic
leadService.createLead()
  ↓ Schema validation & save
Mongoose → Lead.save()
  ↓ Database
MongoDB (insert lead document)
  ↓ Response
res.status(201).json({ success: true, data: lead })
  ↓ Client
```

### Authentication Flow

```
Client
  ↓ POST /leads/login
leadController.loginLead()
  ↓ Fetch user
leadService.getLeadForLogin()
  ↓ Verify password
bcrypt.compare()
  ↓ Generate token
jwt.sign()
  ↓ Response with token
res.json({ token, data })
  ↓ Client stores token

Subsequent Requests
Client
  ↓ GET /leads/:id + "Authorization: Bearer {token}"
Server
  ↓ authMiddleware
jwt.verify()
  ↓ Validate token
  ↓ Extract user ID
req.userId = decoded.id
  ↓ Proceed to controller
```

---

## Error Handling

### Hierarchy

```
Global Error Handler Middleware
  ↓ Catches all errors
errorHandler.mjs
  ↓ Logs error
  ↓ Formats response
  ↓ Sends HTTP response
```

### Error Categories

| Type           | HTTP Status | Cause                                            | Example Message           |
| :------------- | :---------: | :----------------------------------------------- | :------------------------ |
| Validation     |     400     | Invalid request input or failed field validation | `"Invalid email format"`  |
| Authentication |     401     | Missing or invalid authentication token          | `"No token provided"`     |
| Authorization  |     403     | Authenticated but not permitted                  | `"Account not active"`    |
| Not Found      |     404     | Requested resource does not exist                | `"Lead not found"`        |
| Conflict       |     409     | Unique constraint violation                      | `"Email already exists"`  |
| Rate Limit     |     429     | Too many requests in a short time                | `"Too many requests"`     |
| Server Error   |     500     | Unexpected internal failure                      | `"Internal server error"` |


---

## Database Schema Relationships

### Lead-Centric Model

```
Lead (root)
├── 1:1 → LeadProfile (extended info)
├── 1:N → LeadSkill (many skills per lead)
├── 1:N → LeadScoring (multiple scores)
├── 1:N → LeadNotification (notification history)
├── 1:N → LeadActivityLog (audit trail)
└── 1:1 → LeadSettings (user preferences)
```

### Query Pattern

```javascript
// Get lead with all related data
const lead = await Lead.findById(leadId)
  .populate('profiles')
  .populate('skills')
  .populate('notifications');
```

---

## Performance Optimization

### Indexes

```javascript
// Field-level indexes (automatic from schema)
email: { index: true }

// Compound indexes (for common queries)
db.leads.createIndex({ account_status: 1, created_at: -1 })

// TTL index (auto-delete old records)
expireAt: { index: { expireAfterSeconds: 0 } }
```

### Query Optimization

```javascript
// Exclude password from all responses
Lead.find().select('-password_hash');

// Pagination for large result sets
Lead.find()
  .skip((page - 1) * limit)
  .limit(limit);

// Lean queries (read-only, faster)
Lead.findById(id).lean();
```

---

## Testing Strategy

### Unit Tests (Per Module)

```javascript
// Test: leadValidator.mjs
- testValidateRegistration()
  ✓ Valid input
  ✓ Invalid email
  ✓ Short password
  ✓ Email exists
```

### Integration Tests (Phase 3)

```javascript
// Test: Full workflow
Step 0: Register lead
Step 0.25: Verify email
Step 0.5: Login
Step 1-10: Full CRUD operations
```

### Test Execution

```bash
npm run test:api
```

---

## Security Features

### Password Security

```javascript
// Hashing with bcrypt
const hash = await bcrypt.hash(password, 10); // 10 salt rounds

// Verification
const match = await bcrypt.compare(inputPassword, hash);

// Storage (never plain text)
password_hash: String (hashed, required)
password: Not in schema (immediate hash on create)
```

### JWT Security

```javascript
// Token stored on client (not in database)
// Contains: id, email, role
// Expires after 24 hours
// Verified with SECRET_KEY
```

### Account Lockout

```javascript
// After 5 failed login attempts
failed_login_attempts: 5
account_locked: true
lock_until: Date (now + 30 minutes)

// Auto-unlock after timeout
// Reset on successful login
```

### Data Validation

```javascript
// All inputs validated
// XSS prevention via sanitization
// SQL injection not possible (MongoDB)
// CSRF tokens (future enhancement)
```

---

## Monitoring & Logging

### Activity Logging

```javascript
// Automatic on lead updates
LeadActivityLog {
  lead_id,
  type: 'login' | 'profile_update' | 'skill_added',
  timestamp,
  details
}

// Auto-cleanup after 6 months
```

### Error Logging

```javascript
// All errors logged to console (production → file)
console.error('Error in leadService:', error);
```

---

## Development Workflow

### Making Changes

1. **Update Schema** → Model file
2. **Update Validation** → Validator file
3. **Update Business Logic** → Service file
4. **Update Handler** → Controller file
5. **Update Route** (if needed) → Routes file
6. **Test** → npm run test:api

Example: Add new skill field

```javascript
// 1. Model (LeadSkill.mjs)
skill_version: { type: String, default: '1.0' }

// 2. Validator (skillValidator.mjs)
if (data.skill_version && data.skill_version.length > 20) {
  errors.skill_version = 'Version too long';
}

// 3. Service (skillService.mjs)
export const createSkill = async (leadId, data) => {
  const skill = new LeadSkill({
    lead_id: leadId,
    skill_version: data.skill_version,
    ...
  });
  await skill.save();
};

// 4. Controller (skillController.mjs)
export const createSkillEndpoint = async (req, res) => {
  const validation = validateSkill(req.body);
  if (!validation.isValid) return res.status(400).json({ errors: validation.errors });
  
  const skill = await createSkill(req.params.leadId, req.body);
  res.status(201).json({ success: true, data: skill });
};

// 5. Test in Phase 3
// npm run test:api
```

---

## Deployment Checklist

- [ ] Environment variables configured (.env)
- [ ] MongoDB connection verified
- [ ] Tests passing (npm run test:api)
- [ ] JWT_SECRET set in production
- [ ] CORS configured for frontend domain
- [ ] Rate limits reviewed
- [ ] Add logging to file (production)
- [ ] Monitor endpoints set up
- [ ] Backup strategy in place
- [ ] SSL/TLS certificate ready

---

## Troubleshooting

### Database Schema Mismatch

**Problem:** "Path `field` is required"  
**Solution:** Field marked required but null on creation

```javascript
// Change to optional
field: { type: String, default: null }
```

### Duplicate Index Error

**Problem:** "Cannot create index on field, already indexed"  
**Solution:** Remove duplicate index definitions

```javascript
// Keep ONE approach:
// Option A: Field-level
field: { type: String, index: true }

// Option B: Direct index call
schema.index({ field: 1 })
```

### Model Import Error

**Problem:** "Model not registered"  
**Solution:** Ensure model imported before use

```javascript
import '../models/Lead.mjs'; // Must be imported first
const Lead = mongoose.model('Lead');
```

---

## Future Enhancements

- [ ] Caching layer (Redis)
- [ ] Message queue (RabbitMQ)
- [ ] File upload handling
- [ ] Advanced search (Elasticsearch)
- [ ] Real-time notifications (WebSockets)
- [ ] Audit logging (separate database)
- [ ] API versioning strategy
- [ ] Rate limiting per endpoint

---

**Documentation Version:** 2.0.0  
**Last Updated:** February 16, 2026
