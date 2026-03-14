/**
 * @file CandidateAlert.mjs
 * @description MongoDB model for candidate alerts/notifications using native driver
 */

import { ObjectId } from "mongodb";
import dbConnection from "../../../config/database.mjs";

class CandidateAlertModel {
  static collectionName = "candidate_alerts";

  static async getCollection() {
    const db = dbConnection.getDb();
    return db.collection(this.collectionName);
  }

  static async createIndexes() {
    const collection = await this.getCollection();
    await collection.createIndex({ candidate_id: 1, created_at: -1 });
    await collection.createIndex({ candidate_id: 1, read: 1 });
  }

  static validate(data) {
    if (!data.candidate_id) {
      throw new Error('candidate_id is required');
    }
    if (!data.type || !['success', 'warning', 'info', 'error'].includes(data.type)) {
      throw new Error('Valid type is required (success, warning, info, error)');
    }
    if (!data.title || data.title.length > 200) {
      throw new Error('Title is required and must be less than 200 characters');
    }
    if (!data.message || data.message.length > 500) {
      throw new Error('Message is required and must be less than 500 characters');
    }
    return true;
  }

  static async create(alertData) {
    this.validate(alertData);
    
    const collection = await this.getCollection();
    const document = {
      candidate_id: new ObjectId(alertData.candidate_id),
      type: alertData.type,
      title: alertData.title.trim(),
      message: alertData.message.trim(),
      action_type: alertData.action_type || 'system',
      read: false,
      created_at: new Date()
    };

    const result = await collection.insertOne(document);
    return { ...document, _id: result.insertedId };
  }

  static async findByCandidateId(candidateId, limit = 50) {
    const collection = await this.getCollection();
    return await collection
      .find({ candidate_id: new ObjectId(candidateId) })
      .sort({ created_at: -1 })
      .limit(limit)
      .toArray();
  }

  static async markAsRead(alertId, candidateId) {
    const collection = await this.getCollection();
    return await collection.findOneAndUpdate(
      { _id: new ObjectId(alertId), candidate_id: new ObjectId(candidateId) },
      { $set: { read: true } },
      { returnDocument: 'after' }
    );
  }

  static async markAllAsRead(candidateId) {
    const collection = await this.getCollection();
    return await collection.updateMany(
      { candidate_id: new ObjectId(candidateId), read: false },
      { $set: { read: true } }
    );
  }

  static async deleteOld(candidateId, daysToKeep = 30) {
    const collection = await this.getCollection();
    const cutoffDate = new Date();
    cutoffDate.setDate(cutoffDate.getDate() - daysToKeep);
    
    return await collection.deleteMany({
      candidate_id: new ObjectId(candidateId),
      created_at: { $lt: cutoffDate }
    });
  }

  static async countUnread(candidateId) {
    const collection = await this.getCollection();
    return await collection.countDocuments({
      candidate_id: new ObjectId(candidateId),
      read: false
    });
  }
}

export default CandidateAlertModel;
