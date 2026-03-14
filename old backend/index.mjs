/**
 * @file index.mjs
 * @description Main entry point for CCM Backend API
 */

import express from "express";
import cors from "cors";
import helmet from "helmet";
import cookieParser from "cookie-parser";
import swaggerUi from "swagger-ui-express";
import swaggerJsdoc from "swagger-jsdoc";
import { config } from "./src/config/env.mjs";
import dbConnection from "./src/config/database.mjs";
import { initializeCandidateIndexes } from "./src/modules/candidates/models/index.mjs";
import { errorHandler, notFound } from "./src/middleware/errorHandler.mjs";
import { apiLimiter } from "./src/middleware/rateLimiter.mjs";
import apiRoutes from "./src/routes/index.mjs";
import { seedCandidates } from "./src/utils/seeder.mjs";
import { runAPITests } from "./src/tests/apiTester.mjs";
import { swaggerOptions } from "./src/config/swagger.mjs";

const app = express();

// Trust proxy for rate limiting behind reverse proxy (Render, etc.)
app.set('trust proxy', 1);

// Security & Middleware
app.use(helmet());
app.use(cors(config.cors));
app.use(express.json({ limit: '10mb' }));
app.use(express.urlencoded({ extended: true, limit: '10mb' }));
app.use(cookieParser());

// Rate limiting
app.use('/api/', apiLimiter);

// Swagger Documentation
const swaggerSpec = swaggerJsdoc(swaggerOptions);
app.use('/swagger', swaggerUi.serve, swaggerUi.setup(swaggerSpec, {
  customCss: '.swagger-ui .topbar { display: none }',
  customSiteTitle: 'CCM Backend API Documentation',
}));

// Swagger JSON endpoint
app.get('/swagger.json', (req, res) => {
  res.setHeader('Content-Type', 'application/json');
  res.send(swaggerSpec);
});

// Health check
app.get('/health', (req, res) => {
  const dbHealth = dbConnection.isHealthy();
  res.status(dbHealth ? 200 : 503).json({
    success: true,
    status: dbHealth ? 'healthy' : 'unhealthy',
    timestamp: new Date().toISOString(),
    uptime: process.uptime()
  });
});

// Home route
app.get('/', (req, res) => {
  res.status(200).json({
    success: true,
    message: 'CCM Backend API v2.0',
    documentation: '/swagger',
    swagger_json: '/swagger.json',
    health: '/health'
  });
});

// API Routes
app.use('/api', apiRoutes);

// 404 handler
app.use(notFound);

// Error handler (must be last)
app.use(errorHandler);

// Check for test flag
const shouldRunTests = process.argv.includes('--test');

// Start server
const startServer = async () => {
  try {
    console.log('\n🚀 Starting CCM Backend API...\n');
    
    // Connect to database
    await dbConnection.connect();
    
    // Initialize indexes
    await initializeCandidateIndexes();
    // TODO: Add initializeProspectIndexes() when prospect models are created
    
    // Seed test data (only if database is empty or force re-seed for tests)
    await seedCandidates(shouldRunTests);
    
    // Start listening
    app.listen(config.port, async () => {
      const baseUrl = `http://localhost:${config.port}`;
      
      console.log('\n' + '='.repeat(60));
      console.log('🚀 CCM BACKEND SERVER STARTED SUCCESSFULLY');
      console.log('='.repeat(60));
      console.log(`\n📍 Server Info:`);
      console.log(`   Port: ${config.port}`);
      console.log(`   Environment: ${config.nodeEnv}`);
      console.log(`   Database: ${config.mongodb.dbName}`);
      console.log(`\n🌐 Available URLs:`);
      console.log(`   Backend API:      ${baseUrl}`);
      console.log(`   Swagger Docs:     ${baseUrl}/swagger`);
      console.log(`   Swagger JSON:     ${baseUrl}/swagger.json`);
      console.log(`   Health Check:     ${baseUrl}/health`);
      console.log(`   API Health:       ${baseUrl}/api/health`);
      console.log('\n' + '='.repeat(60));
      console.log('📡 Ready to accept requests!\n');

      // Run tests if --test flag is present
      if (shouldRunTests) {
        await runAPITests();
        console.log('🏁 Tests completed. Server will continue running.\n');
      }
    });
  } catch (error) {
    console.error('✗ Failed to start server:', error);
    process.exit(1);
  }
};

startServer();
