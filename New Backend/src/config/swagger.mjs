/**
 * @file swagger.mjs
 * @description Swagger/OpenAPI configuration for API documentation
 */

export const swaggerDefinition = {
  openapi: '3.0.0',
  info: {
    title: 'CCM Backend API',
    version: '2.0.0',
    description: 'Call Center Management - Backend API Documentation',
    contact: {
      name: 'CCM Dev Team',
      email: 'dev@ccm.com',
    },
  },
  servers: [
    {
      url: 'http://localhost:5050',
      description: 'Development server',
    },
    {
      url: 'https://api.ccm.com',
      description: 'Production server',
    },
  ],
  components: {
    securitySchemes: {
      bearerAuth: {
        type: 'http',
        scheme: 'bearer',
        bearerFormat: 'JWT',
        description: 'Enter your JWT token in the format: Bearer <token>',
      },
    },
    schemas: {
      Error: {
        type: 'object',
        properties: {
          success: {
            type: 'boolean',
            example: false,
          },
          message: {
            type: 'string',
            example: 'Error message',
          },
        },
      },
      Success: {
        type: 'object',
        properties: {
          success: {
            type: 'boolean',
            example: true,
          },
          message: {
            type: 'string',
            example: 'Operation successful',
          },
          data: {
            type: 'object',
          },
        },
      },
      CandidateProfile: {
        type: 'object',
        properties: {
          candidate: {
            type: 'object',
            properties: {
              _id: { type: 'string' },
              email: { type: 'string' },
              phone: { type: 'string' },
              name: { type: 'string' },
              surname: { type: 'string' },
              gender: { type: 'string' },
              city: { type: 'string' },
              registration_date: { type: 'string', format: 'date-time' },
            },
          },
          profile: {
            type: 'object',
            properties: {
              desired_position: { type: 'string' },
              call_center_experience: { type: 'string' },
              native_language: { type: 'string' },
              foreign_language_1: { type: 'string' },
              foreign_language_1_level: { type: 'string' },
            },
          },
          availability: {
            type: 'object',
            properties: {
              work_mode: { type: 'string' },
              work_time: { type: 'string' },
              work_park: { type: 'string' },
            },
          },
        },
      },
    },
  },
  security: [
    {
      bearerAuth: [],
    },
  ],
};

export const swaggerOptions = {
  swaggerDefinition,
  apis: [
    './src/modules/*/routes/*.mjs',
    './src/modules/*/controllers/*.mjs',
  ],
};
