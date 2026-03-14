/**
 * @file CandidateSystemInfo.mjs
 * @description MongoDB model for candidate_system_info table (System & auth info)
 */

import { ObjectId } from "mongodb";
import dbConnection from "../../../config/database.mjs";

class CandidateSystemInfoModel {
  static collectionName = "candidate_system_info";

  static async getCollection() {
    const db = dbConnection.getDb();
    return db.collection(this.collectionName);
  }

  static async createIndexes() {
    const collection = await this.getCollection();
    await collection.createIndex({ candidate_id: 1 }, { unique: true });
    await collection.createIndex({ enabled: 1 });
    await collection.createIndex({ deleted: 1 });
    await collection.createIndex({ last_login: -1 });
  }

  static validate(data) {
    if (!data.candidate_id) {
      throw new Error('candidate_id is required');
    }
    return true;
  }

  static async create(systemData) {
    this.validate(systemData);
    
    const collection = await this.getCollection();
    const document = {
      enabled: true,
      deleted: false,
      failed_login_attempts: 0,
      ...systemData,
      candidate_id: new ObjectId(systemData.candidate_id),
      created_at: new Date(),
      updated_at: new Date()
    };

    const result = await collection.insertOne(document);
    return { ...document, _id: result.insertedId };
  }

  static async findByCandidateId(candidateId) {
    const collection = await this.getCollection();
    return await collection.findOne({ candidate_id: new ObjectId(candidateId) });
  }

  static async update(candidateId, updateData) {
    const collection = await this.getCollection();
    return await collection.updateOne(
      { candidate_id: new ObjectId(candidateId) },
      { 
        $set: {
          ...updateData,
          updated_at: new Date()
        }
      },
      { upsert: true }
    );
  }

  static async recordLogin(candidateId, ipAddress, device) {
    const collection = await this.getCollection();
    return await collection.updateOne(
      { candidate_id: new ObjectId(candidateId) },
      { 
        $set: {
          last_login: new Date(),
          ip_address: ipAddress,
          device: device,
          failed_login_attempts: 0,
          updated_at: new Date()
        }
      },
      { upsert: true }
    );
  }

  static async recordFailedLogin(candidateId) {
    const collection = await this.getCollection();
    return await collection.updateOne(
      { candidate_id: new ObjectId(candidateId) },
      { 
        $inc: { failed_login_attempts: 1 },
        $set: {
          last_failed_login: new Date(),
          updated_at: new Date()
        }
      },
      { upsert: true }
    );
  }

  static async delete(candidateId) {
    const collection = await this.getCollection();
    return await collection.deleteOne({ candidate_id: new ObjectId(candidateId) });
  }
}

export default CandidateSystemInfoModel;
