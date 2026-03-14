# CCM Backend API V2 - Complete Documentation

**Version:** 2.0.0  
**Last Updated:** February 16, 2026  
**Status:** ✅ Production Ready  

---

## 📚 Quick Access to Documentation

| Access Point       | Link                                                                       | Description                           |
| :----------------- | :------------------------------------------------------------------------- | :------------------------------------ |
| Swagger UI         | [http://localhost:5050/swagger](http://localhost:5050/swagger)             | Interactive endpoint testing          |
| OpenAPI Spec       | [http://localhost:5050/swagger.json](http://localhost:5050/swagger.json)   | Machine-readable specification (JSON) |
| Full Documentation | [http://localhost:5050/api-docs](http://localhost:5050/api-docs)           | Detailed API documentation            |
| Quick Reference    | [http://localhost:5050/api-quick-ref](http://localhost:5050/api-quick-ref) | Endpoint lookup cheat sheet           |


---

## Table of Contents

1. [Overview](#overview)
2. [Documentation Access](#documentation-access)
3. [Getting Started](#getting-started)
4. [Authentication](#authentication)
5. [Base Configuration](#base-configuration)
6. [Core Modules](#core-modules)
7. [Data Models](#data-models)
8. [Error Handling](#error-handling)
9. [Rate Limiting](#rate-limiting)
10. [Response Format](#response-format)
11. [Complete Endpoint Reference](#complete-endpoint-reference)

---

## Overview

The CCM Backend API V2 is a RESTful service built with Express.js and MongoDB (Mongoose), designed to manage leads, candidates, and recruitment operations. The API follows modern API design principles with JWT authentication, comprehensive validation, and nested resource routing.

### Architecture

- **Framework:** Express.js 4.18.2
- **Database:** MongoDB with Mongoose 8.0.0
- **Authentication:** JWT (JSON Web Tokens)
- **Port:** 5050 (Development)
- **Language:** JavaScript (ES Modules)
- **API Docs:** Swagger UI + OpenAPI 3.0 spec
- **Testing:** Automated Phase 3 test suite with activity log coverage

### Key Features

✅ **7 Core Modules** with 179+ endpoints  
✅ **Swagger Documentation** - Interactive API testing  
✅ **Nested Resource Pattern** - Hierarchical data relationships  
✅ **Role-Based Access Control** - User vs Admin endpoints  
✅ **Comprehensive Validation** - Field-level validation on all inputs  
✅ **Security Headers** - Helmet.js integration  
✅ **Rate Limiting** - Prevent abuse (5 req/min per IP on /api)  
✅ **Activity Logging** - 6-month auto-delete TTL with security features  
✅ **Automated Testing** - Full Phase 3 test suite including activity logs  

---

## Documentation Access

### Swagger UI

The **interactive Swagger UI** is the recommended way to explore and test the API:

```bash
# Start the server
npm run dev-v2

# Open in browser
http://localhost:5050/swagger
```

In Swagger UI, you can:
- 🔍 **Browse all endpoints** by category
- 🧪 **Test endpoints** directly with "Try it out"
- 🔐 Add your JWT token to the lock icon for authenticated requests
- 📥 **See request/response examples** automatically
- 📋 **Download OpenAPI spec** (uses `/swagger.json`)

### Markdown Documentation

Full markdown documentation accessible at:
- `GET /api-docs` - This file (API_DOCUMENTATION.md)
- `GET /api-quick-ref` - Quick reference guide

---

## Getting Started

### Prerequisites

- Node.js 16+ (ES Modules support)
- MongoDB 4.4+
- npm or yarn

### Installation

```bash
# Install dependencies
npm install

# Start the server
npm run start-v2

# Development mode (auto-reload)
npm run dev-v2

# Run tests
npm run test:api
```

### Verify Installation

```bash
# Health check
curl http://localhost:5050/health

# Expected response:
{
  "success": true,
  "status": "healthy",
  "timestamp": "2026-02-16T10:30:00.000Z",
  "uptime": 123.45,
  "version": "2.0.0"
}
```

### Server Startup Output

When you run `npm run dev-v2`, you should see:

```
============================================================
🚀 CCM BACKEND SERVER V2 STARTED SUCCESSFULLY
============================================================

📍 Server Info:
   Port: 5050
   Environment: development
   Database: CCM_DB_V2

🌐 Available URLs:
   Backend API:      http://localhost:5050
   Swagger Docs:     http://localhost:5050/swagger
   Swagger JSON:     http://localhost:5050/swagger.json
   Health Check:     http://localhost:5050/health
   API Health:       http://localhost:5050/api/health
   API Base v2:      http://localhost:5050/api/v2

============================================================
📡 Ready to accept requests!
```

If the database name shows wrong, check `.env` file and verify:
- `DB_NAME_V2=CCM_DB_V2` (or your actual database name)

---

## Running Tests

Comprehensive test suite with activity log coverage:

```bash
# Run all Phase 3 tests
npm run test:api

# Tests include:
# ✅ Registration & Login
# ✅ Skills (Language, Technical, Certification)
# ✅ Profiles & Work Preferences
# ✅ Notifications
# ✅ Settings & Preferences
# ✅ Activity Logging (NEW)
# ✅ Activity Retrieval (NEW)
# ✅ Activity Statistics (NEW)
```

Tests are idempotent - each run creates unique test data with timestamps and doesn't conflict with previous runs.

---

## Authentication

### JWT Token Generation

**Endpoint:** `POST /api/v2/leads/login`

Tokens are generated during login and returned in the response.

```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "lead_id": "507f1f77bcf86cd799439011",
    "public_id": "LD-2026-00001",
    "email": "user@example.com",
    "first_name": "Jean",
    "last_name": "Dupont",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

### Using the Token

Include the token in all authenticated requests:

```bash
curl -X GET http://localhost:5050/api/v2/leads/507f1f77bcf86cd799439011 \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Token Details

- **Expiration:** 24 hours
- **Algorithm:** HS256
- **Claims:** `id`, `email`, `role`
- **Role:** `user` (for all leads)

### Public Endpoints (No Auth Required)

- `POST /api/v2/leads/register` - User registration
- `POST /api/v2/leads/login` - User login
- `POST /api/v2/leads/:leadId/verify-email` - Email verification

---

## Base Configuration

### Server URL

```
Development:  http://localhost:5050
Staging:      https://staging-api.ccm.com
Production:   https://api.ccm.com
```

### API Base Path

```
/api/v2
```

### Standard Headers

```
Content-Type: application/json
Authorization: Bearer {JWT_TOKEN}  (for protected endpoints)
```

### Rate Limiting

- **Window:** 15 minutes
- **Default Limit:** 100 requests per IP per window
- **Authentication:** 5 failed attempts trigger 30-minute account lock

---

## Core Modules

### 1. Leads Module

Root resource for user accounts and registration.

**Base Path:** `/api/v2/leads`

| Method | Endpoint                | Auth Required | Description                       |
| :----- | :---------------------- | :-----------: | :-------------------------------- |
| POST   | `/register`             |       No      | Register a new lead               |
| POST   | `/login`                |       No      | Authenticate lead                 |
| POST   | `/:leadId/verify-email` |       No      | Verify email and activate account |
| POST   | `/:leadId/verify-phone` |       No      | Verify phone number               |
| GET    | `/:leadId`              |      Yes      | Retrieve lead profile             |
| GET    | `/email/:email`         |      Yes      | Find lead by email                |
| GET    | `/public/:publicId`     |      Yes      | Find lead by public ID            |
| GET    | `/`                     |      Yes      | List leads (paginated)            |
| PUT    | `/:leadId/profile`      |      Yes      | Update profile information        |
| PUT    | `/:leadId/email`        |      Yes      | Change email address              |
| PUT    | `/:leadId/password`     |      Yes      | Change password                   |
| DELETE | `/:leadId`              |      Yes      | Soft-delete lead                  |


---

### 2. Lead Profiles Module

Extended profile information for leads (desirable positions, experience, activities).

**Base Path:** `/api/v2/leads/:leadId/profile`

| Method | Endpoint                | Auth Required | Description                      |
| :----- | :---------------------- | :-----------: | :------------------------------- |
| GET    | `/`                     |      Yes      | Retrieve current user profile    |
| PUT    | `/`                     |      Yes      | Update current user profile      |
| GET    | `/work-preferences`     |      Yes      | Retrieve work preferences        |
| PUT    | `/work-preferences`     |      Yes      | Replace all work preferences     |
| POST   | `/work-preferences/add` |      Yes      | Add a new work preference        |
| POST   | `/activities`           |      Yes      | Log a user activity              |
| GET    | `/activities`           |      Yes      | List activity history            |
| POST   | `/compensation`         |      Yes      | Create compensation expectations |
| PUT    | `/compensation/:prefId` |      Yes      | Update compensation expectation  |


**Work Preferences Fields:**
- `desired_mode` (1=Télétravail, 2=Sur site, 3=Hybrid)
- `contract_type` (1=CDI, 2=CDD, 3=Freelance)
- `shift_preference` (1=Morning, 2=Afternoon, 3=Night)
- `available_from` (date)
- `available_hours_min` / `available_hours_max` (number)

---

### 3. Skills Module

Technical and language skills with proficiency tracking.

**Base Path:** `/api/v2/leads/:leadId/skills`

| Method | Endpoint         | Auth Required | Description               |
| :----- | :--------------- | :-----------: | :------------------------ |
| GET    | `/`              |      Yes      | List all skills           |
| GET    | `/stats`         |      Yes      | Retrieve skill statistics |
| POST   | `/language`      |      Yes      | Create a language skill   |
| POST   | `/technical`     |      Yes      | Create a technical skill  |
| POST   | `/certification` |      Yes      | Create a certification    |
| GET    | `/:skillId`      |      Yes      | Retrieve a specific skill |
| PUT    | `/:skillId`      |      Yes      | Update a skill            |
| DELETE | `/:skillId`      |      Yes      | Delete a skill            |


**Skill Types:**
- **Language:** French, English, Spanish, etc. (Proficiency 1-5)
- **Technical:** Excel, Salesforce, SAP, etc.
- **Certification:** Google Analytics, Microsoft, etc. (with expiry date)

---

### 4. Scoring Module

Qualification scoring (manual and automated).

**Base Path:** `/api/v2/leads/:leadId/scoring`

| Method | Endpoint      | Auth Required | Description              |
| :----- | :------------ | :-----------: | :----------------------- |
| GET    | `/`           |      Yes      | Retrieve scoring records |
| POST   | `/`           |      Yes      | Create a manual score    |
| POST   | `/auto-score` |      Yes      | Run automatic scoring    |
| PUT    | `/:scoreId`   |      Yes      | Update a manual score    |


**Scoring Categories:**
- Technical (0-100)
- Language (0-100)
- Experience (0-100)
- Availability (0-100)
- Communication (0-100)
- Overall (0-100)

---

### 5. Notifications Module

Multi-channel notifications (email, SMS, push).

**Base Path:** `/api/v2/leads/:leadId/notifications`

| Method | Endpoint                        | Auth Required | Description                  |
| :----- | :------------------------------ | :-----------: | :--------------------------- |
| GET    | `/`                             |      Yes      | List notifications           |
| POST   | `/`                             |      Yes      | Create a notification        |
| GET    | `/:notificationId`              |      Yes      | Retrieve a notification      |
| PUT    | `/:notificationId`              |      Yes      | Update a notification        |
| POST   | `/:notificationId/mark-read`    |      Yes      | Mark notification as read    |
| POST   | `/:notificationId/mark-clicked` |      Yes      | Mark notification as clicked |
| DELETE | `/:notificationId`              |      Yes      | Delete a notification        |


**Notification Channels:**
- `1` = Email
- `2` = SMS
- `3` = Push notification
- `4` = In-app

**Notification Types (36 types):**
- 1 = Welcome
- 2 = Email Verification
- 3 = Password Reset
- 4 = Job Alert
- 5 = New Match
- ... (32 more types)

**Notification Status:**
- `1` = Pending
- `2` = Sent
- `3` = Delivered
- `4` = Read
- `5` = Clicked
- `6` = Failed
- `7` = Bounced
- `8` = Spam
- `9` = Expired
- `10` = Cancelled

---

### 6. Activity Logs Module

Comprehensive audit trail and security monitoring for all user activities.

**Base Path:** `/api/v2/activity-logs` and `/api/v2/leads/:leadId/activity-logs`

#### Core Activity Endpoints

| Method | Endpoint              | Auth Required | Description                      |
| :----- | :-------------------- | :-----------: | :------------------------------- |
| POST   | `/`                   |      Yes      | Create an activity log           |
| GET    | `/`                   |      Yes      | List activity logs (paginated)   |
| GET    | `/:logId`             |      Yes      | Retrieve an activity log         |
| GET    | `/session/:sessionId` |      Yes      | List activity logs for a session |

#### Analytics & Statistics

| Method | Endpoint                     | Auth Required | Description                                |
| :----- | :--------------------------- | :-----------: | :----------------------------------------- |
| GET    | `/stats`                     |      Yes      | Retrieve activity statistics               |
| GET    | `/analytics`                 |      Yes      | Retrieve platform analytics                |
| GET    | `/funnel/conversion/:leadId` |      Yes      | Retrieve conversion funnel data for a lead |


#### Security Endpoints

| Method | Endpoint                          | Auth Required | Description                               |
| :----- | :-------------------------------- | :-----------: | :---------------------------------------- |
| GET    | `/security/failed-logins/:leadId` |      Yes      | Retrieve failed login attempts for a lead |
| GET    | `/security/suspicious/:leadId`    |      Yes      | Retrieve suspicious activities for a lead |
| GET    | `/security/ip/:ipAddress`         |      Yes      | Retrieve activities from an IP address    |
| GET    | `/security/suspicious-ips`        |      Yes      | List suspicious IP addresses              |


#### Maintenance

| Method | Endpoint      | Auth Required | Description                        |
| :----- | :------------ | :-----------: | :--------------------------------- |
| POST   | `/cleanup`    |      Yes      | Remove expired activity logs       |
| POST   | `/remove-ips` |      Yes      | Remove outdated IP address records |


**Activity Log Fields:**
```json
{
  "lead_id": "5f...",
  "is_authenticated": true,
  "action_type": 1,                    // Numeric 1-53
  "description": "Profile updated",
  "session_id": "session-1708123456789",
  "ip_address": "192.168.1.1",
  "user_agent": "Mozilla/5.0...",
  "device_type": 1,                    // 1=Desktop, 2=Mobile, 3=Tablet, 4=Other
  "source": "web",                     // web, mobile_app, api
  "metadata": {},
  "created_at": "2026-02-16T10:30:00Z"
}
```

**Key Fields Explained:**
- `action_type:` Numeric code identifying the action (1-53)
- `device_type:` Device classification (1=Desktop, 2=Mobile, 3=Tablet, 4=Other)
- `source:` Where the action originated (web, mobile_app, api)
- `session_id:` Grouping activities by user session (min 10 chars)

**Activity Types:**
- Login/Logout
- Profile updates
- Skill additions
- Application submissions
- Document uploads
- Settings changes
- Password changes
- Email verification

**Auto-Cleanup:** Activity logs older than 6 months are automatically deleted.

**Testing Activity Logs:**

Run tests with activity log coverage:
```bash
npm run test:api
```

Tests include:
- ✅ Logging activities
- ✅ Retrieving activity logs
- ✅ Querying activity statistics
- ✅ Checking failed login attempts
- ✅ Detecting suspicious activities

---

### 7. Settings Module

User preferences and admin-managed settings.

**Base Path:** `/api/v2/leads/:leadId/settings`

#### User Settings (Editable by user)

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET    | `/`      | ✅   | Get all settings |
| PUT    | `/`      | ✅   | Update user settings |

**Editable Fields:**
- `theme` (light/dark)
- `language` (fr/en/es)
- `notification_frequency` (instant/daily/weekly)
- `email_notifications_enabled` (bool)
- `sms_notifications_enabled` (bool)
- `push_notifications_enabled` (bool)

#### Admin Settings

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET    | `/admin` | ✅   | Get admin settings |
| PUT    | `/admin` | ✅   | Update admin settings |

**Admin-Only Fields:**
- `verified` (bool)
- `approved` (bool)
- `blacklisted` (bool)
- `blacklist_reason` (string)

---

## Data Models

### Lead Schema

```javascript
{
  // Identity Section
  public_id: "LD-2026-00001",           // Auto-generated
  email: "user@example.com",             // Unique, required
  email_normalized: "user@example.com",  // For case-insensitive search
  password_hash: "bcrypt_hash",          // Encrypted
  first_name: "Jean",                    // Required
  last_name: "Dupont",                   // Required
  phone: "+33612345678",                 // International format
  date_of_birth: "1990-05-15",          // Required
  gender: 0,                             // 0=Male, 1=Female, 2=Other

  // Location Section
  location: {
    country_code: "FR",
    city_id: 75056,                      // Paris
    postal_code: "75001"
  },

  // Account Status
  account_status: 1,                     // 1=Active, 2=Inactive, 3=Suspended, 4=Pending
  lead_stage: 1,                         // 1-5, lifecycle stages
  profile_completion: 45,                // 0-100%

  // Verification
  verification: {
    email_verified: true,
    email_verified_at: "2026-02-12T10:30:00Z",
    phone_verified: false,
    phone_verified_at: null
  },

  // Security
  security: {
    failed_login_attempts: 0,
    last_failed_login: null,
    account_locked: false,
    lock_until: null,
    two_fa_enabled: false
  },

  // Timestamps
  created_at: "2026-02-12T10:00:00Z",
  updated_at: "2026-02-12T10:30:00Z",
  last_login: "2026-02-12T10:15:00Z",
  last_activity_at: "2026-02-12T10:30:00Z"
}
```

### LeadProfile Schema

```javascript
{
  lead_id: ObjectId,
  
  // Professional Info
  desired_position: "Sales Representative",
  call_center_experience: 5,             // Years
  current_role: "Team Lead",
  
  // Work Preferences
  work_preferences: [{
    mode: 2,                             // 1=Remote, 2=On-site, 3=Hybrid
    contract_type: 1,                    // 1=CDI, 2=CDD, 3=Freelance
    shift_preference: 1,                 // 1=Morning, 2=Afternoon, 3=Night
    available_from: "2026-03-01",
    available_hours_min: 35,
    available_hours_max: 40,
    status: 1                            // 1=Active, 2=Inactive
  }],

  // Activities
  activities: [{
    type: "login",
    timestamp: "2026-02-12T10:30:00Z",
    device: "Desktop",
    ip_address: "192.168.1.1"
  }],

  // Compensation
  compensation: [{
    type: "salary",                      // salary/bonus/benefits
    currency: "EUR",
    min_amount: 2500,
    max_amount: 3500,
    frequency: "monthly"
  }],

  // Timestamps
  created_at: "2026-02-12T10:00:00Z",
  updated_at: "2026-02-12T10:30:00Z"
}
```

### LeadSkill Schema

```javascript
{
  lead_id: ObjectId,
  skill_type: 1,                         // 1=Language, 2=Technical, 3=Certification
  skill_name: "English",
  skill_category: "Language",
  
  // Proficiency
  proficiency_level: 4,                  // 1-5 scale
  proficiency_test_score: 92,            // 0-100%
  
  // Certification
  certification_number: "GA-2026-001",
  expiry_date: "2027-02-12",
  issuing_organization: "Google",
  
  // Timestamps
  created_at: "2026-02-12T10:00:00Z",
  verified_at: "2026-02-12T10:05:00Z"
}
```

### LeadNotification Schema

```javascript
{
  lead_id: ObjectId,
  notification_type: 4,                  // 1-36 types
  channel: 1,                            // 1=Email, 2=SMS, 3=Push, 4=In-app
  
  subject: "New Job Match",
  message: "A new job opportunity matches your profile",
  
  // Metadata
  related_job_id: ObjectId,              // Optional
  related_recruiter_id: ObjectId,        // Optional
  
  // Status
  status: 1,                             // 1=Pending, 2=Sent, 3=Delivered, 4=Read, 5=Clicked
  sent_at: null,
  delivered_at: null,
  read_at: null,
  clicked_at: null,
  
  // Retry logic
  retry_count: 0,
  last_retry_at: null,
  
  // User engagement
  clicked_url: null,
  engagement_data: null,

  created_at: "2026-02-12T10:00:00Z"
}
```

---

## Error Handling

### Standard Error Response

```json
{
  "success": false,
  "message": "Error description",
  "errors": {
    "field_name": "Validation error detail"
  },
  "error": "Technical error message (dev only)"
}
```

### HTTP Status Codes

| Code | Name                  | When It Happens                                                              |
| :--: | :-------------------- | :--------------------------------------------------------------------------- |
|  200 | OK                    | Successful `GET` or `PUT` request                                            |
|  201 | Created               | Resource successfully created via `POST`                                     |
|  204 | No Content            | Resource successfully deleted                                                |
|  400 | Bad Request           | Request payload failed validation or is malformed                            |
|  401 | Unauthorized          | Missing or invalid authentication token                                      |
|  403 | Forbidden             | Authenticated but not allowed (inactive account or insufficient permissions) |
|  404 | Not Found             | Requested resource does not exist                                            |
|  409 | Conflict              | Resource already exists (e.g., email already registered)                     |
|  422 | Unprocessable Entity  | Field-level validation errors                                                |
|  429 | Too Many Requests     | Rate limit exceeded                                                          |
|  500 | Internal Server Error | Unexpected server-side error                                                 |


### Common Error Scenarios

#### Email Already Exists (409)
```json
{
  "success": false,
  "message": "Email already registered",
  "errors": {
    "email": "This email is already in use"
  }
}
```

#### Account Not Active (403)
```json
{
  "success": false,
  "message": "Account is not active",
  "error": "Account status: 4"
}
```

#### Invalid JWT (401)
```json
{
  "success": false,
  "message": "Unauthorized",
  "error": "Invalid or expired token"
}
```

#### Validation Errors (400)
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": {
    "email": "Invalid email format",
    "password": "Password must be at least 8 characters",
    "phone": "Invalid phone format"
  }
}
```

---

## Rate Limiting

### Global Limits

- **Default:** 100 requests per 15 minutes per IP
- **Window:** 15 minutes sliding window
- **Headers:** `X-RateLimit-*` included in responses

### Authentication Limits

- **Failed Logins:** 5 failures trigger 30-minute account lock
- **Lockout Duration:** 30 minutes (auto-reset)
- **Reset:** Successful login clears failed attempts

### Response Headers

```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 87
X-RateLimit-Reset: 1644581400
```

---

## Response Format

### Success Response (GET)

```json
{
  "success": true,
  "data": {
    "lead_id": "507f1f77bcf86cd799439011",
    "email": "user@example.com",
    "first_name": "Jean",
    "account_status": 1
  }
}
```

### Success Response (POST)

```json
{
  "success": true,
  "message": "Resource created successfully",
  "data": {
    "id": "507f1f77bcf86cd799439012",
    "created_at": "2026-02-12T10:30:00Z"
  }
}
```

### Success Response (List/Pagination)

```json
{
  "success": true,
  "data": [
    { "lead_id": "...", "email": "..." },
    { "lead_id": "...", "email": "..." }
  ],
  "pagination": {
    "page": 1,
    "limit": 20,
    "total": 150,
    "pages": 8
  }
}
```

---

## Complete Endpoint Reference

### 1. LEADS ENDPOINTS

#### Register New Lead
```
POST /api/v2/leads/register
Content-Type: application/json

{
  "email": "newuser@example.com",
  "password": "SecurePass123!",
  "password_confirm": "SecurePass123!",
  "first_name": "Jean",
  "last_name": "Dupont",
  "phone": "+33612345678",
  "date_of_birth": "1990-05-15",
  "gender": 0,
  "country_code": "FR",
  "city_id": 75056,
  "postal_code": "75001",
  "accept_terms": true
}

Response: 201 Created
{
  "success": true,
  "message": "Lead registered successfully",
  "data": {
    "lead_id": "507f1f77bcf86cd799439011",
    "public_id": "LD-2026-00001",
    "email": "newuser@example.com",
    "first_name": "Jean",
    "last_name": "Dupont"
  }
}
```

#### Login
```
POST /api/v2/leads/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePass123!"
}

Response: 200 OK
{
  "success": true,
  "message": "Login successful",
  "data": {
    "lead_id": "507f1f77bcf86cd799439011",
    "public_id": "LD-2026-00001",
    "email": "user@example.com",
    "first_name": "Jean",
    "last_name": "Dupont",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

#### Verify Email
```
POST /api/v2/leads/{leadId}/verify-email

Response: 200 OK
{
  "success": true,
  "message": "Email verified successfully",
  "data": {
    "lead_id": "507f1f77bcf86cd799439011",
    "email": "user@example.com",
    "email_verified": true
  }
}
```

#### Get Lead Profile
```
GET /api/v2/leads/{leadId}
Authorization: Bearer {token}

Response: 200 OK
{
  "success": true,
  "data": { ... lead object ... }
}
```

#### Update Lead Profile
```
PUT /api/v2/leads/{leadId}/profile
Authorization: Bearer {token}
Content-Type: application/json

{
  "first_name": "Jean",
  "last_name": "Dupont",
  "phone": "+33612345678",
  "gender": 0
}

Response: 200 OK
{
  "success": true,
  "message": "Profile updated successfully",
  "data": { ... updated lead ... }
}
```

---

### 2. SKILLS ENDPOINTS

#### Add Language Skill
```
POST /api/v2/leads/{leadId}/skills/language
Authorization: Bearer {token}
Content-Type: application/json

{
  "skill_name": "English",
  "proficiency_level": 4,
  "proficiency_test_score": 92
}

Response: 201 Created
```

#### Add Technical Skill
```
POST /api/v2/leads/{leadId}/skills/technical
Authorization: Bearer {token}
Content-Type: application/json

{
  "skill_name": "Microsoft Excel",
  "proficiency_level": 5
}

Response: 201 Created
```

#### Add Certification
```
POST /api/v2/leads/{leadId}/skills/certification
Authorization: Bearer {token}
Content-Type: application/json

{
  "skill_name": "Google Analytics",
  "proficiency_level": 4,
  "certification_number": "GA-2026-001",
  "issuing_organization": "Google",
  "expiry_date": "2027-02-12"
}

Response: 201 Created
```

#### List Skills
```
GET /api/v2/leads/{leadId}/skills
Authorization: Bearer {token}

Response: 200 OK
{
  "success": true,
  "data": [ ... skills array ... ]
}
```

#### Delete Skill
```
DELETE /api/v2/leads/{leadId}/skills/{skillId}
Authorization: Bearer {token}

Response: 200 OK
```

---

### 3. PROFILE ENDPOINTS

#### Update Work Preferences
```
PUT /api/v2/leads/{leadId}/profile/work-preferences
Authorization: Bearer {token}
Content-Type: application/json

{
  "mode": 2,
  "contract_type": 1,
  "shift_preference": 1,
  "available_from": "2026-03-01",
  "available_hours_min": 35,
  "available_hours_max": 40
}

Response: 200 OK
```

#### Update Compensation
```
POST /api/v2/leads/{leadId}/profile/compensation
Authorization: Bearer {token}
Content-Type: application/json

{
  "type": "salary",
  "currency": "EUR",
  "min_amount": 2500,
  "max_amount": 3500,
  "frequency": "monthly"
}

Response: 201 Created
```

---

### 4. NOTIFICATIONS ENDPOINTS

#### Create Notification
```
POST /api/v2/leads/{leadId}/notifications
Authorization: Bearer {token}
Content-Type: application/json

{
  "notification_type": 4,
  "channel": 1,
  "subject": "New Job Match",
  "message": "A new job opportunity matches your profile"
}

Response: 201 Created
```

#### Mark Notification as Read
```
POST /api/v2/leads/{leadId}/notifications/{notificationId}/mark-read
Authorization: Bearer {token}

Response: 200 OK
```

#### List Notifications
```
GET /api/v2/leads/{leadId}/notifications
Authorization: Bearer {token}

Response: 200 OK
{
  "success": true,
  "data": [ ... notifications array ... ]
}
```

---

### 5. SETTINGS ENDPOINTS

#### Get User Settings
```
GET /api/v2/leads/{leadId}/settings
Authorization: Bearer {token}

Response: 200 OK
{
  "success": true,
  "data": {
    "theme": "light",
    "language": "fr",
    "notification_frequency": "instant",
    "email_notifications_enabled": true,
    "sms_notifications_enabled": false
  }
}
```

#### Update User Settings
```
PUT /api/v2/leads/{leadId}/settings
Authorization: Bearer {token}
Content-Type: application/json

{
  "theme": "dark",
  "language": "en",
  "notification_frequency": "daily"
}

Response: 200 OK
```

---

## Testing

### Automated Test Suite

The backend includes a comprehensive test suite that validates all endpoints.

```bash
# Run all tests
npm run test:api

# Manual test execution
node scripts/runPhase3Tests.mjs
```

### Test Coverage

- ✅ Lead registration and login
- ✅ Account activation via email verification
- ✅ Language, technical, and certification skills
- ✅ Work preferences and compensation
- ✅ Notifications (creation, read, deletion)
- ✅ User settings updates
- ✅ Skill deletion
- ✅ Authentication and authorization
- ✅ Validation and error handling

### Test Report

After running tests, a detailed report is generated:

```bash
cat test-report.json
```

---

## Troubleshooting

### Database Connection Issues

```bash
# Verify MongoDB is running
mongod --version

# Check connection string
echo $MONGODB_URI
```

### JWT Token Expired

- Token expires after 24 hours
- Solution: Call login endpoint again to get new token

### Account Not Active

- New accounts must verify email first
- Solution: Call `/verify-email` endpoint

### Rate Limit Exceeded

- Wait 15 minutes or use different IP
- Account locked after 5 failed logins (30 min timeout)

### Validation Errors

- Check request body format
- Ensure all required fields are present
- Verify field data types match schema

---

## API Versioning

- **Current Version:** 2.0.0
- **Base Path:** `/api/v2`
- **Legacy V1:** `/api/v1` (deprecated, no longer maintained)
- **Migration Guide:** See `MIGRATION.md`

---

## Support & Contact

- **Documentation:** See `API_DOCUMENTATION.md`
- **Issues:** Report in project repository
- **Development Team:** dev@ccm.com
- **Status Page:** TBD

---

**Last Updated:** February 16, 2026  
**API Status:** ✅ Production Ready
