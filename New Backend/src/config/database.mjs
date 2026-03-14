/**
 * @file database.mjs
 * @description Database connection manager with singleton pattern and graceful shutdown
 */

import { MongoClient } from "mongodb";
import dotenv from "dotenv";

dotenv.config();

const connectionString = process.env.MONGODB_URI;
const dbName = process.env.DB_NAME || "CCM_DEV";

if (!connectionString) {
  throw new Error("MONGODB_URI environment variable is required");
}

const clientOptions = {
  maxPoolSize: 10,
  serverSelectionTimeoutMS: 30000,
  socketTimeoutMS: 45000,
  connectTimeoutMS: 30000,
  retryWrites: true,
  w: "majority",
  directConnection: false
};

class DatabaseConnection {
  constructor() {
    this.client = null;
    this.db = null;
    this.isConnected = false;
  }

  async connect() {
    if (this.isConnected && this.db) {
      return this.db;
    }

    try {
      console.log("Connecting to MongoDB Atlas...");
      this.client = new MongoClient(connectionString, clientOptions);
      await this.client.connect();
      await this.client.db("admin").command({ ping: 1 });

      this.db = this.client.db(dbName);
      this.isConnected = true;

      console.log(`✓ Successfully connected to database: ${dbName}`);
      return this.db;
    } catch (error) {
      console.error("✗ Failed to connect to MongoDB:", error.message);
      this.isConnected = false;
      throw error;
    }
  }

  async disconnect() {
    if (this.client) {
      try {
        await this.client.close();
        this.isConnected = false;
        console.log("✓ Disconnected from MongoDB");
      } catch (error) {
        console.error("✗ Error disconnecting from MongoDB:", error.message);
      }
    }
  }

  getDb() {
    if (!this.isConnected || !this.db) {
      throw new Error("Database not connected. Call connect() first.");
    }
    return this.db;
  }

  isHealthy() {
    return this.isConnected && this.client && this.db;
  }
}

const dbConnection = new DatabaseConnection();

process.on('SIGINT', async () => {
  console.log('\n⚠ Received SIGINT. Graceful shutdown...');
  await dbConnection.disconnect();
  process.exit(0);
});

process.on('SIGTERM', async () => {
  console.log('\n⚠ Received SIGTERM. Graceful shutdown...');
  await dbConnection.disconnect();
  process.exit(0);
});

export default dbConnection;
