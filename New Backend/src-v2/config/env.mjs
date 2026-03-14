/**
 * @file env.mjs
 * @description V2 Environment Configuration - References shared config from V1
 */

import dotenv from 'dotenv';
import { config as v1Config } from '../../src/config/env.mjs';

dotenv.config();

/**
 * V2 Configuration - Extends and shares v1 config
 * Allows V1 and V2 to use the same environment variables
 */
export const config = {
    port: process.env.PORT || 3001, // V2 uses different port
    nodeEnv: process.env.NODE_ENV || 'development',

    // Shared with V1
    mongodb: v1Config.mongodb,
    jwt: v1Config.jwt,
    email: v1Config.email,

    // V2 specific CORS settings
    cors: {
        origin: process.env.CORS_ORIGIN?.split(',') || ['http://localhost:3000', 'http://localhost:5173'],
        credentials: true,
        methods: ['GET', 'POST', 'PUT', 'DELETE', 'PATCH', 'OPTIONS'],
        allowedHeaders: ['Content-Type', 'Authorization']
    }
};

// Validate required environment variables
const requiredEnvVars = ['MONGODB_URI', 'SECRET_KEY'];
const missingEnvVars = requiredEnvVars.filter(varName => !process.env[varName]);

if (missingEnvVars.length > 0) {
    throw new Error(`Missing required environment variables: ${missingEnvVars.join(', ')}`);
}

export const loadEnvConfig = () => config;
