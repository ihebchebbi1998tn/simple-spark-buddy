/**
 * @file CandidateContractPreferences.mjs
 * @description MongoDB model for candidate_contract_preferences table
 */

import { ObjectId } from "mongodb";
import dbConnection from "../../../config/database.mjs";

class CandidateContractPreferencesModel {
  static collectionName = "candidate_contract_preferences";

  static async getCollection() {
    const db = dbConnection.getDb();
    return db.collection(this.collectionName);
  }

  static async createIndexes() {
    const collection = await this.getCollection();
    await collection.createIndex({ candidate_id: 1 }, { unique: true });
  }

  static validate(data) {
    if (!data.candidate_id) {
      throw new Error('candidate_id is required');
    }
    return true;
  }

  static async create(preferencesData) {
    this.validate(preferencesData);
    
    const collection = await this.getCollection();
    const document = {
      ...preferencesData,
      candidate_id: new ObjectId(preferencesData.candidate_id),
      contract_types: preferencesData.contract_types || [],
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

export default CandidateContractPreferencesModel;
