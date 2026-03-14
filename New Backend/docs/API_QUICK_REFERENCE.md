# API Quick Reference Guide

**Last Updated:** February 16, 2026  
**Version:** 2.0.0

---

## Quick Start

```bash
# Start server
npm run start-v2

# Development mode (auto-reload)
npm run dev-v2

# Run tests
npm run test:api
```

---

## Available Documentation

| Resource              | Link                                                                       | Description                            |
| :-------------------- | :------------------------------------------------------------------------- | :------------------------------------- |
| Swagger UI            | [http://localhost:5050/swagger](http://localhost:5050/swagger)             | Interactive API explorer (recommended) |
| Full Documentation    | [http://localhost:5050/api-docs](http://localhost:5050/api-docs)           | Complete Markdown documentation        |
| Quick Reference       | [http://localhost:5050/api-quick-ref](http://localhost:5050/api-quick-ref) | Endpoint cheat sheet                   |
| OpenAPI Specification | [http://localhost:5050/swagger.json](http://localhost:5050/swagger.json)   | OpenAPI 3.0 machine-readable spec      |


---

## Health Check

```bash
# Check if server is running
curl http://localhost:5050/health

# Check specific API health
curl http://localhost:5050/api/health
```

---

## Authentication Flow

```
1. POST /api/v2/leads/register              → Create account (status: 4 = pending)
2. POST /api/v2/leads/{leadId}/verify-email → Activate account (status: 1 = active)
3. POST /api/v2/leads/login                 → Get JWT token
4. Use token in Authorization header        → Access protected endpoints
```

---

## Essential Endpoints

### Registration & Auth
| Endpoint                  | Method | Auth Required | Description                           |
| :------------------------ | :----- | :-----------: | :------------------------------------ |
| `/leads/register`         | POST   |       No      | Register a new user account           |
| `/leads/login`            | POST   |       No      | Authenticate and receive a JWT token  |
| `/leads/:id/verify-email` | POST   |       No      | Verify email and activate the account |


### Leads
| Endpoint              | Method | Auth Required | Description                |
| :-------------------- | :----- | :-----------: | :------------------------- |
| `/leads/:id`          | GET    |      Yes      | Retrieve lead profile      |
| `/leads/:id/profile`  | PUT    |      Yes      | Update profile information |
| `/leads/:id/email`    | PUT    |      Yes      | Change email address       |
| `/leads/:id/password` | PUT    |      Yes      | Change account password    |


### Skills
| Endpoint                          | Method | Auth Required | Description              |
| :-------------------------------- | :----- | :-----------: | :----------------------- |
| `/leads/:id/skills`               | GET    |      Yes      | List skills for a lead   |
| `/leads/:id/skills/language`      | POST   |      Yes      | Create a language skill  |
| `/leads/:id/skills/technical`     | POST   |      Yes      | Create a technical skill |
| `/leads/:id/skills/certification` | POST   |      Yes      | Create a certification   |
| `/leads/:id/skills/:skillId`      | DELETE |      Yes      | Delete a skill           |


### Work Preferences
| Endpoint                              | Method | Auth Required | Description                     |
| :------------------------------------ | :----- | :-----------: | :------------------------------ |
| `/leads/:id/profile/work-preferences` | PUT    |      Yes      | Update work preferences         |
| `/leads/:id/profile/compensation`     | POST   |      Yes      | Create compensation preferences |


### Notifications
| Endpoint                                      | Method | Auth Required | Description                   |
| :-------------------------------------------- | :----- | :-----------: | :---------------------------- |
| `/leads/:id/notifications`                    | GET    |      Yes      | List notifications for a lead |
| `/leads/:id/notifications`                    | POST   |      Yes      | Create a notification         |
| `/leads/:id/notifications/:notifId/mark-read` | POST   |      Yes      | Mark a notification as read   |
| `/leads/:id/notifications/:notifId`           | DELETE |      Yes      | Delete a notification         |


### Settings
| Endpoint              | Method | Auth Required | Description            |
| :-------------------- | :----- | :-----------: | :--------------------- |
| `/leads/:id/settings` | GET    |      Yes      | Retrieve lead settings |
| `/leads/:id/settings` | PUT    |      Yes      | Update lead settings   |


---

## Common Request Headers

```
Content-Type: application/json
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## Common Response Format

```json
{
  "success": true,
  "message": "Optional message",
  "data": { /* response data */ },
  "pagination": { /* if applicable */ }
}
```

---

## Status Codes

| Code | Name                  | Description                                            |
| :--: | :-------------------- | :----------------------------------------------------- |
|  200 | OK                    | Request successful                                     |
|  201 | Created               | Resource successfully created                          |
|  400 | Bad Request           | Invalid or malformed request data                      |
|  401 | Unauthorized          | Missing or invalid authentication token                |
|  403 | Forbidden             | Authenticated but not allowed (e.g., inactive account) |
|  404 | Not Found             | Requested resource does not exist                      |
|  409 | Conflict              | Resource already exists (e.g., email already taken)    |
|  429 | Too Many Requests     | Rate limit exceeded                                    |
|  500 | Internal Server Error | Unexpected server-side error                           |


---

## Field Constraints

### Email
- Required, unique
- Valid email format
- Case-insensitive storage

### Password
- Min 8 characters
- Must confirm match
- Bcrypt hashed storage

### Phone
- International format (+33...)
- Min 10 digits
- Stored as-is

### Skill Proficiency
- Scale: 1-5
- 1 = Basic, 5 = Expert

### Account Status
- 1 = Active (can login)
- 2 = Inactive
- 3 = Suspended
- 4 = Pending (needs verification)

### Work Modes
- 1 = Télétravail (Remote)
- 2 = Sur site (On-site)
- 3 = Hybrid

### Contract Types
- 1 = CDI (Permanent)
- 2 = CDD (Fixed term)
- 3 = Freelance

### Shift Preferences
- 1 = Matin (Morning)
- 2 = Après-midi (Afternoon)
- 3 = Nuit (Night)

---

## Error Examples

### Validation Error
```json
{
  "success": false,
  "message": "Validation failed",
  "errors": {
    "email": "Invalid email format",
    "password": "Password must be at least 8 characters"
  }
}
```

### Not Found
```json
{
  "success": false,
  "message": "Lead not found"
}
```

### Unauthorized
```json
{
  "success": false,
  "message": "Unauthorized",
  "error": "Invalid or expired token"
}
```

---

## Testing

```bash
# Run full test suite
npm run test:api

# Expected output:
# ✅ Step 0: Lead registered with ID
# ✅ Step 0.25: Email verified, account activated
# ✅ Step 0.5: Lead logged in, token received
# ✅ Step 1: Language skill created
# ... (10 more steps)
# Pass Rate: 100.00%
```

---

## Environment Variables

```bash
# .env file
MONGODB_URI=mongodb://localhost:27017/ccm
DB_NAME_V2=CCM_DEV
PORT=5050
SECRET_KEY=your_jwt_secret_key
NODE_ENV=development
```

---

## Rate Limits

- **Default:** 100 requests per 15 minutes per IP
- **Account Lock:** 5 failed logins = 30 min lock
- **Headers:** `X-RateLimit-*` in response

---

## Example Requests

### Register
```bash
curl -X POST http://localhost:5050/api/v2/leads/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "SecurePass123!",
    "password_confirm": "SecurePass123!",
    "first_name": "Jean",
    "last_name": "Dupont",
    "phone": "+33612345678",
    "date_of_birth": "1990-05-15",
    "accept_terms": true
  }'
```

### Login
```bash
curl -X POST http://localhost:5050/api/v2/leads/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "SecurePass123!"
  }'
```

### Get Profile (with token)
```bash
curl -X GET http://localhost:5050/api/v2/leads/607f1f77bcf86cd799439011 \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Add Skill
```bash
curl -X POST http://localhost:5050/api/v2/leads/607f1f77bcf86cd799439011/skills/language \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -d '{
    "skill_name": "English",
    "proficiency_level": 4,
    "proficiency_test_score": 92
  }'
```

---

## Files & Locations

| Path                         | Description                         |
| :--------------------------- | :---------------------------------- |
| `API_DOCUMENTATION.md`       | Full API reference documentation    |
| `API_QUICK_REFERENCE.md`     | Condensed quick reference guide     |
| `src-v2/modules/leads/`      | Lead-related routes and controllers |
| `src-v2/modules/`            | All application modules             |
| `scripts/runPhase3Tests.mjs` | Automated test runner               |
| `.env`                       | Environment configuration variables |


---

## Useful Commands

```bash
# Start development server with watch mode
npm run dev-v2

# Start production server
npm run start-v2

# Run all tests
npm run test:api

# View test report
cat test-report.json

# Monitor MongoDB (in another terminal)
mongosh
> use ccm
> db.leads.countDocuments()
```

---

## Common Issues

### Account Not Active
**Error:** "Account is not active"  
**Solution:** Call `/verify-email` endpoint first

### Email Already Exists
**Error:** 409 Conflict  
**Solution:** Use different email or recover existing account

### Invalid Token
**Error:** 401 Unauthorized  
**Solution:** Login again to get new token, tokens expire after 24h

### Rate Limited
**Error:** 429 Too Many Requests  
**Solution:** Wait 15 minutes or reduce request frequency

---

**Reference:** Full documentation in `API_DOCUMENTATION.md`
