/**
 * @file CandidateTestScores.mjs
 * @description MongoDB model for candidate_test_scores table (Language tests & notifications)
 */

import { ObjectId } from "mongodb";
import dbConnection from "../../../config/database.mjs";

class CandidateTestScoresModel {
  static collectionName = "candidate_test_scores";

  static async getCollection() {
    const db = dbConnection.getDb();
    return db.collection(this.collectionName);
  }

  static async createIndexes() {
    const collection = await this.getCollection();
    await collection.createIndex({ candidate_id: 1 }, { unique: true });
    await collection.createIndex({ french_test_completed: 1 });
    await collection.createIndex({ english_test_completed: 1 });
  }

  static validate(data) {
    if (!data.candidate_id) {
      throw new Error('candidate_id is required');
    }
    return true;
  }

  static async create(testData) {
    this.validate(testData);
    
    const collection = await this.getCollection();
    const document = {
      // Profile completion status
      profile_complete: false,
      
      // French test scores - all initialized to null
      french_test_completed: false,
      french_linguistic_score: null,
      french_soft_skills_score: null,
      french_job_skills_score: null,
      french_overall_score: null,
      
      // English test scores - all initialized to null
      english_test_completed: false,
      english_linguistic_score: null,
      english_soft_skills_score: null,
      english_job_skills_score: null,
      english_overall_score: null,
      
      // Italian test scores - all initialized to null
      italian_test_completed: false,
      italian_linguistic_score: null,
      italian_soft_skills_score: null,
      italian_job_skills_score: null,
      italian_overall_score: null,
      
      // Spanish test scores - all initialized to null
      spanish_test_completed: false,
      spanish_linguistic_score: null,
      spanish_soft_skills_score: null,
      spanish_job_skills_score: null,
      spanish_overall_score: null,
      
      // German test scores - all initialized to null
      german_test_completed: false,
      german_linguistic_score: null,
      german_soft_skills_score: null,
      german_job_skills_score: null,
      german_overall_score: null,
      
      // Notification preferences
      newsletter: false,
      email_notifications: true,
      sms_notifications: false,
      
      // Privacy settings
      profile_visibility: 'public',
      secure_data_sharing: true,
      
      // CGU acceptance
      cgu_accepted: false,
      cgu_accepted_at: null,
      
      // Override with provided data
      ...testData,
      candidate_id: new ObjectId(testData.candidate_id),
      created_at: new Date(),
      updated_at: new Date()
    };

    const result = await collection.insertOne(document);
    return { ...document, _id: result.insertedId };
  }

  static async findByCandidateId(candidateId) {
    const collection = await this.getCollection();
    const objectIdCandidate = new ObjectId(candidateId);
    console.log('🔍 CandidateTestScores.findByCandidateId - querying with ObjectId:', objectIdCandidate.toString());
    
    const result = await collection.findOne({ candidate_id: objectIdCandidate });
    console.log('🔍 CandidateTestScores.findByCandidateId - found:', result ? {
      _id: result._id?.toString(),
      candidate_id: result.candidate_id?.toString(),
      secure_data_sharing: result.secure_data_sharing,
      email_notifications: result.email_notifications,
      updated_at: result.updated_at
    } : 'NOT FOUND');
    
    return result;
  }

  static async update(candidateId, updateData) {
    const collection = await this.getCollection();
    const objectIdCandidate = new ObjectId(candidateId);
    console.log('🔧 CandidateTestScores.update - candidateId:', candidateId, '-> ObjectId:', objectIdCandidate.toString());
    
    const result = await collection.updateOne(
      { candidate_id: objectIdCandidate },
      { 
        $set: {
          ...updateData,
          updated_at: new Date()
        }
      },
      { upsert: false } // Don't create new document, only update existing
    );
    
    console.log('🔧 Update result:', { matchedCount: result.matchedCount, modifiedCount: result.modifiedCount });
    return result;
  }

  /**
   * Update test scores for a specific language
   * @param {ObjectId} candidateId - Candidate ID
   * @param {string} language - Language code (french, english, german, italian, spanish)
   * @param {object} scores - Scores object with linguistic, softSkills, jobSkills, overall
   */
  static async updateTestScores(candidateId, language, scores) {
    const collection = await this.getCollection();
    const updateFields = {};
    
    // Validate language
    const validLanguages = ['french', 'english', 'german', 'italian', 'spanish'];
    const lang = language.toLowerCase();
    
    if (!validLanguages.includes(lang)) {
      throw new Error(`Invalid language: ${language}. Must be one of: ${validLanguages.join(', ')}`);
    }
    
    // Set test completed flag
    updateFields[`${lang}_test_completed`] = true;
    
    // Set individual scores (null if not provided)
    updateFields[`${lang}_linguistic_score`] = scores.linguistic ?? null;
    updateFields[`${lang}_soft_skills_score`] = scores.softSkills ?? null;
    updateFields[`${lang}_job_skills_score`] = scores.jobSkills ?? null;
    updateFields[`${lang}_overall_score`] = scores.overall ?? null;
    
    updateFields.updated_at = new Date();

    return await collection.updateOne(
      { candidate_id: new ObjectId(candidateId) },
      { $set: updateFields },
      { upsert: true }
    );
  }

  /**
   * Update notification preferences
   */
  static async updateNotificationPreferences(candidateId, preferences) {
    const collection = await this.getCollection();
    const updateFields = { updated_at: new Date() };
    
    if (preferences.newsletter !== undefined) {
      updateFields.newsletter = preferences.newsletter;
    }
    if (preferences.email_notifications !== undefined) {
      updateFields.email_notifications = preferences.email_notifications;
    }
    if (preferences.sms_notifications !== undefined) {
      updateFields.sms_notifications = preferences.sms_notifications;
    }
    
    return await collection.updateOne(
      { candidate_id: new ObjectId(candidateId) },
      { $set: updateFields },
      { upsert: true }
    );
  }

  /**
   * Accept CGU (Terms and Conditions)
   */
  static async acceptCGU(candidateId) {
    const collection = await this.getCollection();
    return await collection.updateOne(
      { candidate_id: new ObjectId(candidateId) },
      { 
        $set: {
          cgu_accepted: true,
          cgu_accepted_at: new Date(),
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

export default CandidateTestScoresModel;
