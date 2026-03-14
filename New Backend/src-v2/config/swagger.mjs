/**
 * @file swagger.mjs
 * @description Swagger/OpenAPI configuration for V2 API documentation
 */

export const swaggerDefinition = {
    openapi: '3.0.0',
    info: {
        title: 'CCM Backend API - V2',
        version: '2.0.0',
        description: 'Call Center Management - Backend API V2 (Leads Module Documentation)',
        contact: {
            name: 'CCM Dev Team',
            email: 'dev@ccm.com',
        },
    },
    externalDocs: {
        url: 'http://localhost:5050/api-docs',
        description: 'Full API Documentation',
    },
    servers: [
        {
            url: 'http://localhost:5050/api/v2',
            description: 'Development server (V2)',
        },
        {
            url: 'https://api.ccm.com/api/v2',
            description: 'Production server (V2)',
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
                    errors: {
                        type: 'object',
                        description: 'Validation errors detailed by field',
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
                        description: 'Response data',
                    },
                },
            },
            Lead: {
                type: 'object',
                properties: {
                    _id: { type: 'string', description: 'Lead unique identifier' },
                    email: { type: 'string', format: 'email' },
                    name: { type: 'string' },
                    phone: { type: 'string' },
                    status: { type: 'string', enum: ['active', 'inactive', 'archived'] },
                    created_at: { type: 'string', format: 'date-time' },
                    updated_at: { type: 'string', format: 'date-time' },
                },
            },
            LeadProfile: {
                type: 'object',
                properties: {
                    _id: { type: 'string' },
                    lead_id: { type: 'string' },
                    desired_position: { type: 'integer' },
                    call_center_experience: { type: 'integer' },
                    call_center_self_assessment: { type: 'integer' },
                    activities: {
                        type: 'array',
                        items: { type: 'object' },
                    },
                },
            },
            LeadSkill: {
                type: 'object',
                properties: {
                    _id: { type: 'string' },
                    lead_id: { type: 'string' },
                    qualification_type: { type: 'integer', description: '1=Language, 2=Technical, 3=Certification' },
                    language_code: { type: 'string', description: 'ISO language code' },
                    completed: { type: 'boolean' },
                    scores: {
                        type: 'object',
                        properties: {
                            linguistic: { type: 'integer' },
                            soft_skills: { type: 'integer' },
                            job_skills: { type: 'integer' },
                            global: { type: 'integer' },
                        },
                    },
                },
            },
            LeadSettings: {
                type: 'object',
                properties: {
                    _id: { type: 'string' },
                    lead_id: { type: 'string' },
                    notifications: {
                        type: 'object',
                        properties: {
                            email: { type: 'boolean' },
                            sms: { type: 'boolean' },
                            push: { type: 'boolean' },
                            in_app: { type: 'boolean' },
                        },
                    },
                    account_status: { type: 'string', enum: ['active', 'suspended', 'banned', 'pending_review'] },
                    two_factor_enabled: { type: 'boolean' },
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
    definition: swaggerDefinition,
    apis: [
        './src-v2/modules/*/routes/*.mjs',
        './src-v2/modules/*/controllers/*.mjs',
    ],
};
