/**
 * @file env.mjs
 * @description Centralized environment configuration
 */

import dotenv from "dotenv";

dotenv.config();

export const config = {
  port: process.env.PORT || 5050,
  nodeEnv: process.env.NODE_ENV || 'development',
  
  // Database
  mongodb: {
    uri: process.env.MONGODB_URI,
    dbName: process.env.DB_NAME || 'CCM_DEV'
  },
  
  // JWT
  jwt: {
    secret: process.env.SECRET_KEY,
    expiresIn: '7d'
  },
  
  // Email
  email: {
    user: process.env.EMAIL_USER,
    pass: process.env.EMAIL_PASS
  },
  
  // CORS - Allow all origins
  cors: {
    origin: true,
    credentials: true,
    methods: ['GET', 'POST', 'PUT', 'DELETE', 'OPTIONS'],
    allowedHeaders: ['Content-Type', 'Authorization']
  }
};

// Validate required environment variables
const requiredEnvVars = ['MONGODB_URI', 'SECRET_KEY'];
const missingEnvVars = requiredEnvVars.filter(varName => !process.env[varName]);

if (missingEnvVars.length > 0) {
  throw new Error(`Missing required environment variables: ${missingEnvVars.join(', ')}`);
}
