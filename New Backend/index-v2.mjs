import express from 'express';
import cors from 'cors';
import helmet from 'helmet';
import cookieParser from 'cookie-parser';
import swaggerUi from 'swagger-ui-express';
import swaggerJsdoc from 'swagger-jsdoc';
import dotenv from 'dotenv';
import mongoose from 'mongoose';
import { fileURLToPath } from 'url';
import { dirname } from 'path';

// Import v2 config
import dbConnection from './src-v2/config/database.mjs';
import { config } from './src-v2/config/env.mjs';
import { swaggerDefinition } from './src-v2/config/swagger.mjs';

// Get current directory for ESM
const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

// Import middleware
import { errorHandler } from './src/middleware/errorHandler.mjs';
import { apiLimiter } from './src/middleware/rateLimiter.mjs';

// Import v2 routes
import leadsRoutes from './src-v2/modules/leads/routes/index.mjs';

// Import v1 routes (for backward compatibility - password reset, etc.)
import v1ApiRoutes from './src/routes/index.mjs';

// Load environment variables
dotenv.config();

// Initialize Express app
const app = express();
const PORT = config.port;
const API_BASE = '/api/v2';

/**
 * ============================================
 * MIDDLEWARE SETUP
 * ============================================
 */

// Security middleware
app.use(helmet());

// Trust proxy for rate limiting behind reverse proxy (Render, etc.)
app.set('trust proxy', 1);

// CORS configuration
app.use(cors(config.cors));

// Body parsing middleware
app.use(express.json({ limit: '10mb' }));
app.use(express.urlencoded({ limit: '10mb', extended: true }));

// Cookie parser
app.use(cookieParser());

// Rate limiting
app.use('/api/', apiLimiter);

// Swagger Documentation
const swaggerSpec = swaggerJsdoc({
    definition: swaggerDefinition,
    apis: [
        './src-v2/modules/leads/routes/*.mjs',
        './src-v2/modules/leads/controllers/*.mjs',
        './src-v2/modules/callcenters/routes/*.mjs',
        './src-v2/modules/callcenters/controllers/*.mjs',
    ]
});

app.use('/swagger', swaggerUi.serve, swaggerUi.setup(swaggerSpec, {
    customCss: '.swagger-ui .topbar { display: none }',
    customSiteTitle: 'CCM Backend API V2 Documentation',
}));

// Swagger JSON endpoint
app.get('/swagger.json', (req, res) => {
    res.setHeader('Content-Type', 'application/json');
    res.send(swaggerSpec);
});

/**
 * ============================================
 * HEALTH CHECK ENDPOINTS
 * ============================================
 */

app.get('/health', (req, res) => {
    const dbHealth = dbConnection.isHealthy();
    res.status(dbHealth ? 200 : 503).json({
        success: true,
        status: dbHealth ? 'healthy' : 'unhealthy',
        timestamp: new Date().toISOString(),
        uptime: process.uptime(),
        version: '2.0.0'
    });
});

app.get('/api/health', (req, res) => {
    const dbHealth = dbConnection.isHealthy();
    res.status(dbHealth ? 200 : 503).json({
        success: true,
        status: dbHealth ? 'healthy' : 'unhealthy',
        timestamp: new Date().toISOString(),
        uptime: process.uptime(),
        version: '2.0.0'
    });
});

/**
 * ============================================
 * HOME ROUTE
 * ============================================
 */

app.get('/', (req, res) => {
    res.status(200).json({
        success: true,
        message: 'CCM Backend API v2.0',
        documentation: '/swagger',
        swagger_json: '/swagger.json',
        api_docs: '/api-docs',
        health: '/health',
        api_base: `http://localhost:${config.port}${API_BASE}`
    });
});

// API Documentation endpoint
app.get('/api-docs', (req, res) => {
    res.setHeader('Content-Type', 'text/markdown');
    res.sendFile('./docs/API_DOCUMENTATION.md', { root: '.' });
});

// API Quick Reference endpoint
app.get('/api-quick-ref', (req, res) => {
    res.setHeader('Content-Type', 'text/markdown');
    res.sendFile('./docs/API_QUICK_REFERENCE.md', { root: '.' });
});

/**
 * ============================================
 * DATABASE CONNECTION
 * ============================================
 */

async function initializeDatabase() {
    try {
        console.log('🔄 Initializing database connection...');
        await dbConnection.connect();
        console.log('✅ Database connection established successfully');
        return true;
    } catch (error) {
        console.error('❌ Database connection failed:', error.message);
        throw error;
    }
}

/**
 * ============================================
 * API ROUTES
 * ============================================
 */

// V2 Leads module routes - includes leads, profiles, skills, notifications, and activity logs
app.use(`${API_BASE}`, leadsRoutes);

// V1 routes (backward compatibility) - password reset, legacy auth endpoints
app.use('/api', v1ApiRoutes);

/**
 * ============================================
 * 404 NOT FOUND HANDLER
 * ============================================
 */

app.use((req, res) => {
    res.status(404).json({
        success: false,
        message: `Route not found: ${req.method} ${req.path}`,
        path: req.path,
        method: req.method
    });
});

/**
 * ============================================
 * ERROR HANDLING MIDDLEWARE
 * ============================================
 */

app.use(errorHandler);

/**
 * ============================================
 * GRACEFUL SHUTDOWN
 * ============================================
 */

async function gracefulShutdown(signal) {
    console.log(`\n⚠️  ${signal} received. Starting graceful shutdown...`);

    try {
        console.log('🔄 Closing database connections...');
        await dbConnection.disconnect();
        console.log('✅ Database connections closed');
        console.log('✅ Graceful shutdown completed');
        process.exit(0);
    } catch (error) {
        console.error('❌ Error during graceful shutdown:', error);
        process.exit(1);
    }
}

process.on('SIGINT', () => gracefulShutdown('SIGINT'));
process.on('SIGTERM', () => gracefulShutdown('SIGTERM'));

/**
 * ============================================
 * SERVER STARTUP
 * ============================================
 */

async function startServer() {
    try {
        // Initialize database first
        await initializeDatabase();

        // Start listening for requests
        const server = app.listen(PORT, () => {
            const baseUrl = `http://localhost:${PORT}`;

            console.log('\n' + '='.repeat(60));
            console.log('🚀 CCM BACKEND SERVER V2 STARTED SUCCESSFULLY');
            console.log('='.repeat(60));
            console.log(`\n📍 Server Info:`);
            console.log(`   Port: ${PORT}`);
            console.log(`   Environment: ${process.env.NODE_ENV || 'development'}`);
            console.log(`   Database: ${process.env.DB_NAME_V2 || 'CCM_DB_V2'}`);
            console.log(`\n🌐 Available URLs:`);
            console.log(`   Backend API:      ${baseUrl}`);
            console.log(`   Swagger Docs:     ${baseUrl}/swagger`);
            console.log(`   Swagger JSON:     ${baseUrl}/swagger.json`);
            console.log(`   Health Check:     ${baseUrl}/health`);
            console.log(`   API Health:       ${baseUrl}/api/health`);
            console.log(`   API Base v2:      ${baseUrl}${API_BASE}`);
            console.log('\n' + '='.repeat(60));
            console.log('📡 Ready to accept requests!\n');
        });

        // Handle server errors
        server.on('error', (error) => {
            if (error.code === 'EADDRINUSE') {
                console.error(`❌ Port ${PORT} is already in use`);
            } else {
                console.error('❌ Server error:', error);
            }
            process.exit(1);
        });

    } catch (error) {
        console.error('❌ Failed to start server:', error.message);
        process.exit(1);
    }
}

// Start the server if this is the main module
if (process.argv[1] && process.argv[1].includes('index-v2.mjs')) {
    startServer();
}

export default app;
export { startServer };
