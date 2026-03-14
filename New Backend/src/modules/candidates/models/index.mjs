/**
 * @file index.mjs
 * @description Export all candidate models and initialize database indexes
 */

import CandidateModel from './Candidate.mjs';
import CandidateProfileModel from './CandidateProfile.mjs';
import CandidateAvailabilityModel from './CandidateAvailability.mjs';
import CandidateContractPreferencesModel from './CandidateContractPreferences.mjs';
import CandidateTestScoresModel from './CandidateTestScores.mjs';
import CandidateSystemInfoModel from './CandidateSystemInfo.mjs';
import CandidateAlertModel from './CandidateAlert.mjs';

/**
 * Initialize all candidate database indexes
 */
export async function initializeCandidateIndexes() {
  try {
    console.log('\n📊 Initializing Candidate Collections...\n');
    
    const collections = [
      { name: 'candidates', model: CandidateModel },
      { name: 'candidate_profiles', model: CandidateProfileModel },
      { name: 'candidate_availability', model: CandidateAvailabilityModel },
      { name: 'candidate_contract_preferences', model: CandidateContractPreferencesModel },
      { name: 'candidate_test_scores', model: CandidateTestScoresModel },
      { name: 'candidate_system_info', model: CandidateSystemInfoModel },
      { name: 'candidate_alerts', model: CandidateAlertModel }
    ];

    for (const collection of collections) {
      await collection.model.createIndexes();
      console.log(`\x1b[32m✓\x1b[0m ${collection.name.padEnd(35)} - Created`);
    }
    
    console.log('\n\x1b[32m✓\x1b[0m All candidate collections initialized successfully\n');
  } catch (error) {
    console.error('\n\x1b[31m✗\x1b[0m Error creating candidate collections:', error.message);
    throw error;
  }
}

export {
  CandidateModel,
  CandidateProfileModel,
  CandidateAvailabilityModel,
  CandidateContractPreferencesModel,
  CandidateTestScoresModel,
  CandidateSystemInfoModel,
  CandidateAlertModel
};
