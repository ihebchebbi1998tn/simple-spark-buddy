/**
 * @file Candidate.mjs
 * @description MongoDB model for candidates table (Main contact information)
 */

import { ObjectId } from "mongodb";
import dbConnection from "../../../config/database.mjs";

class CandidateModel {
  static collectionName = "candidates";

  /**
   * Get the candidates collection
   */
  static async getCollection() {
    const db = dbConnection.getDb();
    return db.collection(this.collectionName);
  }

  /**
   * Create indexes for the collection
   */
  static async createIndexes() {
    const collection = await this.getCollection();
    await collection.createIndex({ email: 1 }, { unique: true });
    await collection.createIndex({ phone: 1 });
    await collection.createIndex({ city: 1 });
    await collection.createIndex({ registration_date: -1 });
  }

  /**
   * Validate candidate data structure
   */
  static validate(data) {
    const required = ['email', 'phone', 'name', 'surname'];
    const missing = required.filter(field => !data[field]);
    
    if (missing.length > 0) {
      throw new Error(`Missing required fields: ${missing.join(', ')}`);
    }

    return true;
  }

  /**
   * Create a new candidate
   */
  static async create(candidateData) {
    this.validate(candidateData);
    
    const collection = await this.getCollection();
    const document = {
      ...candidateData,
      created_at: new Date(),
      updated_at: new Date()
    };

    const result = await collection.insertOne(document);
    return { ...document, _id: result.insertedId };
  }

  /**
   * Find candidate by ID
   */
  static async findById(id) {
    const collection = await this.getCollection();
    return await collection.findOne({ _id: new ObjectId(id) });
  }

  /**
   * Find candidate by email
   */
  static async findByEmail(email) {
    const collection = await this.getCollection();
    return await collection.findOne({ email });
  }

  /**
   * Update candidate
   */
  static async update(id, updateData) {
    const collection = await this.getCollection();
    const result = await collection.updateOne(
      { _id: new ObjectId(id) },
      { 
        $set: {
          ...updateData,
          updated_at: new Date()
        }
      }
    );
    return result;
  }

  /**
   * Delete candidate (soft delete)
   */
  static async delete(id) {
    const collection = await this.getCollection();
    return await collection.updateOne(
      { _id: new ObjectId(id) },
      { 
        $set: { 
          deleted: true,
          deleted_date: new Date(),
          updated_at: new Date()
        }
      }
    );
  }

  /**
   * Find all candidates with filters
   */
  static async findAll(filters = {}, options = {}) {
    const collection = await this.getCollection();
    const { limit = 100, skip = 0, sort = { created_at: -1 } } = options;
    
    return await collection
      .find(filters)
      .sort(sort)
      .skip(skip)
      .limit(limit)
      .toArray();
  }
}

export default CandidateModel;
