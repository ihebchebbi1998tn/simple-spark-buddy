/**
 * @file CandidateAvailability.mjs
 * @description MongoDB model for candidate_availability table (Work preferences)
 */

import { ObjectId } from "mongodb";
import dbConnection from "../../../config/database.mjs";

class CandidateAvailabilityModel {
  static collectionName = "candidate_availability";

  static async getCollection() {
    const db = dbConnection.getDb();
    return db.collection(this.collectionName);
  }

  static async createIndexes() {
    const collection = await this.getCollection();
    await collection.createIndex({ candidate_id: 1 }, { unique: true });
    await collection.createIndex({ availability: 1 });
    await collection.createIndex({ work_mode: 1 });
  }

  static validate(data) {
    if (!data.candidate_id) {
      throw new Error('candidate_id is required');
    }
    return true;
  }

  static async create(availabilityData) {
    this.validate(availabilityData);
    
    const collection = await this.getCollection();
    const document = {
      ...availabilityData,
      candidate_id: new ObjectId(availabilityData.candidate_id),
      availability: availabilityData.availability || 'immédiate', // When can start: immédiate, en_poste, 1-2_semaines, etc.
      work_mode: availabilityData.work_mode || '',
      work_time: availabilityData.work_time || '',
      work_park: availabilityData.work_park || '',
      nearby_cities: availabilityData.nearby_cities || [],
      blacklist: availabilityData.blacklist || [],
      whitelist: availabilityData.whitelist || [],
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

export default CandidateAvailabilityModel;
