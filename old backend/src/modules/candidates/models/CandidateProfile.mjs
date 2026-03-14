/**
 * @file CandidateProfile.mjs
 * @description MongoDB model for candidate_profiles table (CV/Profile information)
 */

import { ObjectId } from "mongodb";
import dbConnection from "../../../config/database.mjs";

class CandidateProfileModel {
  static collectionName = "candidate_profiles";

  static async getCollection() {
    const db = dbConnection.getDb();
    return db.collection(this.collectionName);
  }

  static async createIndexes() {
    const collection = await this.getCollection();
    await collection.createIndex({ candidate_id: 1 }, { unique: true });
    await collection.createIndex({ desired_position: 1 });
    await collection.createIndex({ primary_activity: 1 });
    await collection.createIndex({ call_center_experience: 1 });
  }

  static validate(data) {
    if (!data.candidate_id) {
      throw new Error('candidate_id is required');
    }
    return true;
  }

  static async create(profileData) {
    this.validate(profileData);
    
    const collection = await this.getCollection();
    const document = {
      ...profileData,
      candidate_id: new ObjectId(profileData.candidate_id),
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

  static async delete(candidateId) {
    const collection = await this.getCollection();
    return await collection.deleteOne({ candidate_id: new ObjectId(candidateId) });
  }
}

export default CandidateProfileModel;
