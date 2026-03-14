/**
 * @file PasswordReset.mjs
 * @description Password reset token/code model
 */

import { ObjectId } from 'mongodb';
import dbConnection from '../../../config/database.mjs';

const COLLECTION_NAME = 'password_resets';
const CODE_EXPIRY_MINUTES = 15;
const MAX_ATTEMPTS = 5;

class PasswordResetModel {
  static async getCollection() {
    const db = dbConnection.getDb();
    return db.collection(COLLECTION_NAME);
  }

  /**
   * Create indexes for the collection
   */
  static async createIndexes() {
    const collection = await this.getCollection();
    await collection.createIndex({ email: 1 });
    await collection.createIndex({ code: 1 });
    await collection.createIndex({ expires_at: 1 }, { expireAfterSeconds: 0 }); // TTL index
    console.log('✅ PasswordReset indexes created');
  }

  /**
   * Generate a 5-digit random code
   */
  static generateCode() {
    return Math.floor(10000 + Math.random() * 90000).toString();
  }

  /**
   * Create a new password reset request
   * @param {string} email - User email
   * @param {ObjectId} candidateId - Candidate ID
   */
  static async create(email, candidateId) {
    const collection = await this.getCollection();
    
    // Delete any existing reset requests for this email
    await collection.deleteMany({ email: email.toLowerCase() });
    
    const code = this.generateCode();
    const now = new Date();
    const expiresAt = new Date(now.getTime() + CODE_EXPIRY_MINUTES * 60 * 1000);
    
    const resetData = {
      email: email.toLowerCase(),
      candidate_id: candidateId,
      code,
      attempts: 0,
      created_at: now,
      expires_at: expiresAt,
      used: false
    };
    
    const result = await collection.insertOne(resetData);
    
    console.log('🔑 Password reset code created:', { 
      email, 
      code, 
      expiresAt: expiresAt.toISOString() 
    });
    
    return { ...resetData, _id: result.insertedId };
  }

  /**
   * Find reset request by email
   * @param {string} email
   */
  static async findByEmail(email) {
    const collection = await this.getCollection();
    return collection.findOne({ 
      email: email.toLowerCase(),
      used: false,
      expires_at: { $gt: new Date() }
    });
  }

  /**
   * Verify reset code
   * @param {string} email
   * @param {string} code
   * @returns {Object} { valid: boolean, error?: string, candidateId?: ObjectId }
   */
  static async verifyCode(email, code) {
    const collection = await this.getCollection();
    
    const resetRequest = await collection.findOne({ 
      email: email.toLowerCase(),
      used: false
    });
    
    if (!resetRequest) {
      return { valid: false, error: 'Aucune demande de réinitialisation trouvée' };
    }
    
    // Check if expired
    if (new Date() > resetRequest.expires_at) {
      await collection.deleteOne({ _id: resetRequest._id });
      return { valid: false, error: 'Le code a expiré. Veuillez en demander un nouveau.' };
    }
    
    // Check attempts
    if (resetRequest.attempts >= MAX_ATTEMPTS) {
      await collection.deleteOne({ _id: resetRequest._id });
      return { valid: false, error: 'Trop de tentatives. Veuillez demander un nouveau code.' };
    }
    
    // Verify code
    if (resetRequest.code !== code) {
      // Increment attempts
      await collection.updateOne(
        { _id: resetRequest._id },
        { $inc: { attempts: 1 } }
      );
      
      const remainingAttempts = MAX_ATTEMPTS - resetRequest.attempts - 1;
      return { 
        valid: false, 
        error: `Code incorrect. ${remainingAttempts} tentative(s) restante(s).`
      };
    }
    
    // Code is valid - mark as verified but not yet used
    await collection.updateOne(
      { _id: resetRequest._id },
      { $set: { verified: true, verified_at: new Date() } }
    );
    
    return { 
      valid: true, 
      candidateId: resetRequest.candidate_id,
      resetId: resetRequest._id
    };
  }

  /**
   * Mark reset as used after password change
   * @param {string} email
   */
  static async markAsUsed(email) {
    const collection = await this.getCollection();
    await collection.updateOne(
      { email: email.toLowerCase(), verified: true },
      { $set: { used: true, used_at: new Date() } }
    );
  }

  /**
   * Delete reset request
   * @param {string} email
   */
  static async deleteByEmail(email) {
    const collection = await this.getCollection();
    await collection.deleteMany({ email: email.toLowerCase() });
  }
}

export default PasswordResetModel;
