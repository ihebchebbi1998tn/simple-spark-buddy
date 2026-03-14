# Swagger API Documentation Setup

## 🚀 Quick Start

### 1. Install Dependencies
```bash
cd CCM_Backend_Dev-NEW
npm install swagger-ui-express swagger-jsdoc
```

### 2. Start the Server
```bash
npm run dev
```

### 3. Access Swagger UI
Open your browser and navigate to:
```
http://localhost:5050/swagger
```

## 📚 What's Included

### Swagger Endpoints
- **Swagger UI**: `http://localhost:5050/swagger` - Interactive API documentation
- **Swagger JSON**: `http://localhost:5050/swagger.json` - Raw OpenAPI specification

### Documented APIs

#### Authentication (`/api/auth`)
- `POST /api/auth/login` - Login with email and password
- `POST /api/auth/logout` - Logout user
- `GET /api/auth/me` - Get current authenticated user (requires JWT)

#### Candidates (`/api/candidates`)
- `POST /api/candidates/register` - Register new candidate
- `GET /api/candidates/profile` - Get complete profile (requires JWT)
- `PUT /api/candidates/info` - Update basic information (requires JWT)
- `PUT /api/candidates/profile` - Update CV/profile (requires JWT)
- `PUT /api/candidates/availability` - Update availability (requires JWT)
- `PUT /api/candidates/contract-preferences` - Update contract preferences (requires JWT)
- `PUT /api/candidates/call-centers` - Update call center preferences (requires JWT)
- `PUT /api/candidates/proximity` - Update geographic preferences (requires JWT)
- `POST /api/candidates/test-scores` - Add language test scores (requires JWT)
- `PUT /api/candidates/password` - Change password (requires JWT)
- `PUT /api/candidates/email` - Change email (requires JWT)
- `DELETE /api/candidates/account` - Delete account (requires JWT)

## 🔐 Authentication in Swagger

### Testing Authenticated Endpoints

1. **Login First**: Use the `POST /api/auth/login` endpoint to get a JWT token
2. **Authorize**: Click the "Authorize" button (🔒) at the top of Swagger UI
3. **Enter Token**: Paste your JWT token in the format: `Bearer <your-token>`
4. **Test APIs**: Now you can test all authenticated endpoints

### Example Login Request
```json
{
  "email": "candidate@example.com",
  "password": "YourPassword123"
}
```

The response will include a JWT token:
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "candidate": { ... }
  }
}
```

Copy the token value and use it for authentication.

## 📝 Adding Documentation to New APIs

When creating new API endpoints, add Swagger annotations above your route definitions:

```javascript
/**
 * @swagger
 * /api/your-endpoint:
 *   post:
 *     summary: Brief description
 *     tags: [YourTag]
 *     security:
 *       - bearerAuth: []  # If authentication required
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               field1:
 *                 type: string
 *               field2:
 *                 type: number
 *     responses:
 *       200:
 *         description: Success response
 *       400:
 *         description: Bad request
 */
router.post('/your-endpoint', yourController);
```

## 🎨 Customization

### Update API Information
Edit `CCM_Backend_Dev-NEW/src/config/swagger.mjs`:
- API title and description
- Server URLs (development, production)
- Contact information
- API version

### Add New Schema Definitions
Add reusable schemas in the `components.schemas` section of `swagger.mjs`:

```javascript
components: {
  schemas: {
    YourModel: {
      type: 'object',
      properties: {
        id: { type: 'string' },
        name: { type: 'string' }
      }
    }
  }
}
```

## 🔧 Configuration Files

- **`src/config/swagger.mjs`**: Main Swagger configuration
- **`index.mjs`**: Swagger UI setup and routes
- **Route files**: JSDoc comments for endpoint documentation

## 📖 Benefits

✅ **Interactive Testing**: Test all APIs directly from the browser
✅ **Auto-Generated Docs**: Documentation stays in sync with code
✅ **Easy Onboarding**: New developers can quickly understand the API
✅ **Type Safety**: Clear request/response schemas
✅ **Authentication Support**: Built-in JWT token management
✅ **Export Options**: Download OpenAPI spec as JSON

## 🚀 Best Practices

1. **Document as You Build**: Add Swagger annotations when creating new endpoints
2. **Use Examples**: Include example request/response payloads
3. **Group by Tags**: Organize endpoints with meaningful tags
4. **Describe Parameters**: Add clear descriptions for all parameters
5. **Error Responses**: Document all possible error responses
6. **Keep Updated**: Update docs when changing API behavior

## 📚 Resources

- [Swagger Documentation](https://swagger.io/docs/)
- [OpenAPI Specification](https://spec.openapis.org/oas/latest.html)
- [swagger-ui-express](https://www.npmjs.com/package/swagger-ui-express)
- [swagger-jsdoc](https://www.npmjs.com/package/swagger-jsdoc)
