# CCM Backend API Endpoints

## 📚 Base URL
```
Development: http://localhost:5050
Production: https://api.ccm.com
```

## 🔐 Authentication

All authenticated endpoints require a JWT token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

---

## 🔑 Authentication Endpoints (`/api/auth`)

### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "candidate@example.com",
  "password": "YourPassword123"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "candidate": {
      "_id": "507f1f77bcf86cd799439011",
      "email": "candidate@example.com",
      "name": "John",
      "surname": "Doe"
    }
  }
}
```

### Logout
```http
POST /api/auth/logout
```

### Get Current User
```http
GET /api/auth/me
Authorization: Bearer <token>
```

---

## 👤 Candidate Endpoints (`/api/candidates`)

### 1. Register New Candidate
```http
POST /api/candidates/register
Content-Type: application/json

{
  "civilite": "M.",
  "nom": "Doe",
  "prenom": "John",
  "telephone": "+21612345678",
  "villeResidence": "Tunis",
  "email": "john.doe@example.com",
  "motDePasse": "SecurePassword123",
  "posteRecherche": "Téléconseiller",
  "experienceGlobale": "2-5 ans",
  "langueMaternelle": "Français",
  "langueForeign1": "Anglais",
  "niveauLangueForeign1": "B2",
  "modeTravail": "Présentiel",
  "tempsTravail": "Temps plein",
  "parcTravail": "Oui"
}
```

### 2. Get Own Profile (Authenticated User)
```http
GET /api/candidates/profile
Authorization: Bearer <token>
```

**Response:**
```json
{
  "success": true,
  "data": {
    "candidate": {
      "_id": "507f1f77bcf86cd799439011",
      "email": "john.doe@example.com",
      "phone": "+21612345678",
      "name": "John",
      "surname": "Doe",
      "gender": "M.",
      "city": "Tunis",
      "registration_date": "2024-01-15T10:30:00.000Z"
    },
    "profile": {
      "desired_position": "Téléconseiller",
      "call_center_experience": "2-5 ans",
      "native_language": "Français",
      "foreign_language_1": "Anglais",
      "foreign_language_1_level": "B2"
    },
    "availability": {
      "work_mode": "Présentiel",
      "work_time": "Temps plein",
      "work_park": "Oui"
    },
    "contractPreferences": { ... },
    "testScores": { ... }
  }
}
```

### 3. Get All Candidates ⭐ NEW
```http
GET /api/candidates/all?page=1&limit=50
Authorization: Bearer <token>
```

**Query Parameters:**
- `page` (optional): Page number, default = 1
- `limit` (optional): Results per page, default = 50

**Response:**
```json
{
  "success": true,
  "data": {
    "candidates": [
      {
        "candidate": { ... },
        "profile": { ... },
        "availability": { ... },
        "contractPreferences": { ... },
        "testScores": { ... }
      },
      // ... more candidates
    ],
    "pagination": {
      "page": 1,
      "limit": 50,
      "total": 45
    }
  }
}
```

**Use Cases:**
- Admin dashboard showing all registered candidates
- Recruiter viewing candidate database
- Analytics and reporting
- Bulk candidate management

### 4. Get Candidate by ID ⭐ NEW
```http
GET /api/candidates/507f1f77bcf86cd799439011
Authorization: Bearer <token>
```

**Response:**
```json
{
  "success": true,
  "data": {
    "candidate": {
      "_id": "507f1f77bcf86cd799439011",
      "email": "john.doe@example.com",
      "phone": "+21612345678",
      "name": "John",
      "surname": "Doe",
      "gender": "M.",
      "city": "Tunis",
      "registration_date": "2024-01-15T10:30:00.000Z"
    },
    "profile": {
      "desired_position": "Téléconseiller",
      "call_center_experience": "2-5 ans",
      "position_experience": "Vente inbound",
      "primary_activity": "Service client",
      "native_language": "Français",
      "foreign_language_1": "Anglais",
      "foreign_language_1_level": "B2",
      "foreign_language_2": "Espagnol",
      "foreign_language_2_level": "A2",
      "is_bilingual": true
    },
    "availability": {
      "work_mode": "Présentiel",
      "work_time": "Temps plein",
      "work_park": "Oui",
      "nearby_cities": ["Ariana", "Ben Arous"]
    },
    "contractPreferences": {
      "contract_types": ["CDI", "CDD"]
    },
    "testScores": {
      "french_test_completed": true,
      "french_linguistic_score": 85,
      "french_soft_skills_score": 90,
      "french_job_skills_score": 88,
      "french_overall_score": 87.67
    }
  }
}
```

**Use Cases:**
- View detailed candidate profile
- Recruiter reviewing specific candidate
- Candidate matching algorithms
- Profile verification

### 5. Update Basic Info
```http
PUT /api/candidates/info
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "John",
  "surname": "Doe",
  "phone": "+21612345678",
  "city": "Tunis",
  "gender": "M."
}
```

### 6. Update Profile (CV Details)
```http
PUT /api/candidates/profile
Authorization: Bearer <token>
Content-Type: application/json

{
  "desired_position": "Téléconseiller senior",
  "call_center_experience": "5+ ans",
  "foreign_language_2": "Espagnol",
  "foreign_language_2_level": "B1"
}
```

### 7. Update Availability
```http
PUT /api/candidates/availability
Authorization: Bearer <token>
Content-Type: application/json

{
  "work_mode": "Hybride",
  "work_time": "Temps partiel",
  "work_park": "Non"
}
```

### 8. Update Contract Preferences
```http
PUT /api/candidates/contract-preferences
Authorization: Bearer <token>
Content-Type: application/json

{
  "contract_types": ["CDI", "CDD", "Freelance"]
}
```

### 9. Update Call Center Preferences
```http
PUT /api/candidates/call-centers
Authorization: Bearer <token>
Content-Type: application/json

{
  "blacklist": ["Teleperformance France", "Webhelp"],
  "whitelist": ["Orange Business Services", "Capgemini"]
}
```

### 10. Update Geographic Proximity
```http
PUT /api/candidates/proximity
Authorization: Bearer <token>
Content-Type: application/json

{
  "villeResidence": "Tunis",
  "villesProximite": ["Ariana", "Ben Arous", "Manouba"]
}
```

### 11. Add Language Test Scores
```http
POST /api/candidates/test-scores
Authorization: Bearer <token>
Content-Type: application/json

{
  "language": "french",
  "scores": {
    "linguistic": 85,
    "softSkills": 90,
    "jobSkills": 88,
    "overall": 87.67
  }
}
```

### 12. Change Password
```http
PUT /api/candidates/password
Authorization: Bearer <token>
Content-Type: application/json

{
  "currentPassword": "OldPassword123",
  "newPassword": "NewSecurePassword456"
}
```

### 13. Change Email
```http
PUT /api/candidates/email
Authorization: Bearer <token>
Content-Type: application/json

{
  "newEmail": "newemail@example.com",
  "password": "YourPassword123"
}
```

### 14. Delete Account
```http
DELETE /api/candidates/account
Authorization: Bearer <token>
Content-Type: application/json

{
  "password": "YourPassword123",
  "confirmation": "SUPPRIMER"
}
```

---

## 📊 Response Format

### Success Response
```json
{
  "success": true,
  "message": "Operation successful",
  "data": { ... }
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error description"
}
```

---

## ⚡ Quick Testing Guide

### 1. Test with cURL

**Login:**
```bash
curl -X POST http://localhost:5050/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"password123"}'
```

**Get All Candidates:**
```bash
curl -X GET http://localhost:5050/api/candidates/all?page=1&limit=10 \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**Get Candidate by ID:**
```bash
curl -X GET http://localhost:5050/api/candidates/507f1f77bcf86cd799439011 \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### 2. Test with Swagger UI

Visit: `http://localhost:5050/swagger`

1. Use `/api/auth/login` to get a token
2. Click "Authorize" 🔒 and enter: `Bearer <token>`
3. Test any endpoint interactively!

---

## 🔒 Security Notes

- All sensitive endpoints require JWT authentication
- Passwords must be at least 8 characters
- JWT tokens are signed with HS256 algorithm
- Account deletion requires password confirmation
- Email changes require password verification

---

## 📈 Rate Limits

- Global API rate limit: Applied per IP
- Configured in `src/middleware/rateLimiter.mjs`

---

## 🐛 Error Codes

| Code | Description |
|------|-------------|
| 200 | Success |
| 201 | Created |
| 400 | Bad Request (validation error) |
| 401 | Unauthorized (invalid or missing token) |
| 404 | Not Found |
| 409 | Conflict (duplicate email/phone) |
| 500 | Internal Server Error |

---

## 📞 Support

For API support, contact: dev@ccm.com
