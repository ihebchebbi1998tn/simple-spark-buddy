# Backend Documentation

**Version:** 2.0.0  
**Last Updated:** February 16, 2026  
**Status:** ✅ Production Ready

---

## 🚀 Getting Started

### Option 1: Interactive Swagger UI (Recommended)

The easiest way to explore and test the API:

```bash
npm run dev-v2
open http://localhost:5050/swagger
```

In Swagger UI you can:
- 🔍 Browse all endpoints
- 🧪 Test endpoints with "Try it out"
- 🔐 Add JWT token for authenticated requests
- 📝 See request/response examples

### Option 2: Read Documentation

Start with any of these markdown files below.

---

## 📚 Documentation Files

All markdown documentation files are organized in this folder.

### Core Documentation

1. **[INDEX.md](./INDEX.md)** - Navigation guide and overview
2. **[API_DOCUMENTATION.md](./API_DOCUMENTATION.md)** - Complete API reference (updated with Swagger info)
3. **[API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md)** - Quick lookup guide  
4. **[ARCHITECTURE.md](./ARCHITECTURE.md)** - System design & architecture

---

## 📖 Documentation Endpoints

When your server is running, access docs at:

| URL | Type | Purpose |
|-----|------|---------|
| `http://localhost:5050/swagger` | 🎯 UI | Interactive API explorer |
| `http://localhost:5050/swagger.json` | 📋 JSON | OpenAPI 3.0 specification |
| `http://localhost:5050/api-docs` | 📖 Markdown | This documentation |
| `http://localhost:5050/api-quick-ref` | ⚡ Markdown | Quick reference |

---

## 🗂️ Folder Structure

```
backend/
├── docs/                          ← You are here
│   ├── README.md                 ← This file
│   ├── INDEX.md                  ← Start here
│   ├── API_DOCUMENTATION.md      ← Complete reference
│   ├── API_QUICK_REFERENCE.md    ← Quick lookup
│   └── ARCHITECTURE.md           ← Technical details
│
├── src-v2/                        ← Application code
│   ├── config/                    ← Database, env, swagger config
│   ├── middleware/                ← Auth, validation, error handling
│   └── modules/leads/             ← All route handlers
│
├── scripts/                       ← Test scripts (with activity log tests)
├── index-v2.mjs                  ← Server entry point
├── .env                           ← Configuration
├── .gitignore                     ← Git ignore rules
├── package.json                   ← Dependencies
└── README.md                      ← Project README
```

---

## 🚀 Quick Start

### For Developers
1. Read [INDEX.md](./INDEX.md) first
2. Bookmark [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md)
3. Use [API_DOCUMENTATION.md](./API_DOCUMENTATION.md) for details
4. Review [ARCHITECTURE.md](./ARCHITECTURE.md) when extending

### For DevOps/Deployment
1. Check [ARCHITECTURE.md](./ARCHITECTURE.md#deployment-checklist)
2. Configure `.env`
3. Start server: `npm run start-v2`
4. Run tests: `npm run test:api`

### For Product Teams
1. Review module overviews in [API_DOCUMENTATION.md](./API_DOCUMENTATION.md#core-modules)
2. Check data models in [API_DOCUMENTATION.md](./API_DOCUMENTATION.md#data-models)
3. Understand statuses in [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md#status-codes)

---

## 📖 File Descriptions

### INDEX.md
**Navigation & Overview** (400+ lines)
- Documentation map
- Quick navigation by use case
- Key information summary
- Common endpoints cheat sheet
- Support resources

**Best for:** Finding what you need quickly

---

### API_DOCUMENTATION.md
**Complete API Reference** (1000+ lines)
- System overview & architecture
- Getting started guide
- Authentication & JWT
- All 7 modules (50+ endpoints)
- 7 data models with examples
- Error handling & codes
- Rate limiting details
- Response formats
- Complete endpoint reference
- Testing guide
- Troubleshooting

**Best for:** Detailed endpoint information, examples, schemas

---

### API_QUICK_REFERENCE.md
**Quick Lookup Guide** (400+ lines)
- Quick start commands
- Authentication flow
- Essential endpoints table
- Status codes reference
- Field constraints & enums
- Error examples
- cURL examples
- Environment variables
- Common issues & solutions

**Best for:** Fast lookups during development, copy-paste examples

---

### ARCHITECTURE.md
**Technical Architecture** (800+ lines)
- Overview & design philosophy
- Directory structure
- Core components (6 layers)
- Models, Validators, Services, Controllers
- Request-response flows
- Database relationships
- Performance optimization
- Security features
- Development workflow
- Testing strategy
- Deployment checklist
- Troubleshooting

**Best for:** Understanding system design, extending functionality, code reviews

---

## 🔍 Find Information By Topic

### Authentication
- Quick flow: [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md#authentication-flow)
- Detailed: [API_DOCUMENTATION.md](./API_DOCUMENTATION.md#authentication)
- Security details: [ARCHITECTURE.md](./ARCHITECTURE.md#authentication-1)

### Endpoints
- Quick list: [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md#essential-endpoints)
- Complete reference: [API_DOCUMENTATION.md](./API_DOCUMENTATION.md#complete-endpoint-reference)
- Patterns: [ARCHITECTURE.md](./ARCHITECTURE.md#database-schema-relationships)

### Data Models
- Quick reference: [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md#field-constraints)
- Full schemas: [API_DOCUMENTATION.md](./API_DOCUMENTATION.md#data-models)
- Relationships: [ARCHITECTURE.md](./ARCHITECTURE.md#database-schema-relationships)

### Errors & Troubleshooting
- Status codes: [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md#status-codes)
- Error details: [API_DOCUMENTATION.md](./API_DOCUMENTATION.md#error-handling)
- Issues: [ARCHITECTURE.md](./ARCHITECTURE.md#troubleshooting)

### Development
- Getting started: [API_DOCUMENTATION.md](./API_DOCUMENTATION.md#getting-started)
- Architecture: [ARCHITECTURE.md](./ARCHITECTURE.md)
- Workflow: [ARCHITECTURE.md](./ARCHITECTURE.md#development-workflow)

### Deployment
- Checklist: [ARCHITECTURE.md](./ARCHITECTURE.md#deployment-checklist)
- Environment: [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md#environment-variables)

---

## 💡 Tips

- **Install Dependencies:** `npm install`
- **Start Server:** `npm run start-v2`
- **Run Tests:** `npm run test:api`
- **Dev Mode:** `npm run dev-v2` (with auto-reload)
- **View Test Report:** `cat test-report.json`

---

## 📊 System Overview

| Component      | Details                                                                     |
| :------------- | :-------------------------------------------------------------------------- |
| API Version    | v2.0.0                                                                      |
| Modules        | 7 — Leads, Profiles, Skills, Scoring, Notifications, ActivityLogs, Settings |
| Endpoints      | 179+ API endpoints                                                          |
| Authentication | JWT tokens (24-hour expiration)                                             |
| Database       | MongoDB 4.4 or newer                                                        |
| Framework      | Express.js 4.18.2                                                           |
| Rate Limit     | 100 requests per 15 minutes                                                 |


---

## 🆘 Quick Help

### Getting Started?
→ Start with [INDEX.md](./INDEX.md)

### Need an endpoint?
→ Check [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md#essential-endpoints)

### Got an error?
→ Look in [API_QUICK_REFERENCE.md](./API_QUICK_REFERENCE.md#common-issues)

### Extending the system?
→ Read [ARCHITECTURE.md](./ARCHITECTURE.md#development-workflow)

### Deploying?
→ Follow [ARCHITECTURE.md](./ARCHITECTURE.md#deployment-checklist)

---

## 📝 Documentation Info

- **Created:** February 16, 2026
- **Status:** ✅ Complete & Current
- **Maintained By:** CCM Dev Team
- **Last Reviewed:** February 16, 2026

---

**Start with [INDEX.md](./INDEX.md) →**
