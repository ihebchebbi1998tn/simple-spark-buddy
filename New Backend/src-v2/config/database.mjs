/**
 * @file database.mjs
 * @description Mongoose database connection manager (V2)
 */

import mongoose from 'mongoose';
import dotenv from 'dotenv';

dotenv.config();

const mongoUri = process.env.MONGODB_URI;
const dbName = process.env.DB_NAME_V2 || 'CCM_DEV';

if (!mongoUri) {
    throw new Error('MONGODB_URI environment variable is required');
}

class DatabaseConnection {
    constructor() {
        this.isConnected = false;
    }

    async connect() {
        if (this.isConnected) {
            console.log('✓ Already connected to MongoDB');
            return mongoose.connection;
        }

        try {
            console.log('🔄 Connecting to MongoDB with Mongoose...');

            await mongoose.connect(mongoUri, {
                dbName: dbName,
                maxPoolSize: 10,
                serverSelectionTimeoutMS: 30000,
                socketTimeoutMS: 45000,
                connectTimeoutMS: 30000,
                retryWrites: true,
                w: 'majority'
            });

            this.isConnected = true;
            console.log(`✅ Successfully connected to database: ${dbName}`);

            return mongoose.connection;
        } catch (error) {
            console.error('❌ Failed to connect to MongoDB:', error.message);
            this.isConnected = false;
            throw error;
        }
    }

    async disconnect() {
        try {
            if (this.isConnected) {
                await mongoose.disconnect();
                this.isConnected = false;
                console.log('✅ Disconnected from MongoDB');
            }
        } catch (error) {
            console.error('❌ Error disconnecting from MongoDB:', error.message);
        }
    }

    isHealthy() {
        return this.isConnected && mongoose.connection.readyState === 1;
    }
}

const dbConnection = new DatabaseConnection();

// Graceful shutdown handlers
process.on('SIGINT', async () => {
    console.log('\n⚠️  Received SIGINT. Graceful shutdown...');
    await dbConnection.disconnect();
    process.exit(0);
});

process.on('SIGTERM', async () => {
    console.log('\n⚠️  Received SIGTERM. Graceful shutdown...');
    await dbConnection.disconnect();
    process.exit(0);
});

export default dbConnection;
