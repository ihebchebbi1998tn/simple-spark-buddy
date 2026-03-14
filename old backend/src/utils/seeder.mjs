/**
 * @file seeder.mjs
 * @description Database seeder for test candidate data
 */

import bcrypt from 'bcrypt';
import { ObjectId } from 'mongodb';
import dbConnection from '../config/database.mjs';

const SALT_ROUNDS = 10;

export async function seedCandidates(force = false) {
  try {
    const db = dbConnection.getDb();
    
    // Check if data already exists
    const existingCount = await db.collection('candidates').countDocuments();
    if (existingCount > 0 && !force) {
      console.log('⚠ Database already contains candidates. Skipping seed.');
      return;
    }

    if (force && existingCount > 0) {
      console.log('\n🗑️  Clearing existing test data...');
      
      // Get all test candidate IDs to delete related records
      const testCandidates = await db.collection('candidates')
        .find({ email: /test\.com$/i })
        .toArray();
      
      const testCandidateIds = testCandidates.map(c => c._id);
      
      // Delete all related data for test candidates
      if (testCandidateIds.length > 0) {
        await Promise.all([
          db.collection('candidate_system_info').deleteMany({ candidate_id: { $in: testCandidateIds } }),
          db.collection('candidate_profiles').deleteMany({ candidate_id: { $in: testCandidateIds } }),
          db.collection('candidate_availability').deleteMany({ candidate_id: { $in: testCandidateIds } }),
          db.collection('candidate_contract_preferences').deleteMany({ candidate_id: { $in: testCandidateIds } }),
          db.collection('candidate_test_scores').deleteMany({ candidate_id: { $in: testCandidateIds } })
        ]);
      }
      
      // Finally delete the candidates themselves
      await db.collection('candidates').deleteMany({ email: /test\.com$/i });
      console.log(`  Deleted ${testCandidateIds.length} test candidates and related data`);
    }

    console.log('\n📦 Seeding test candidates...');

    // Candidate 1: Just signed up - minimal data
    const candidate1Id = new ObjectId();
    const password1 = await bcrypt.hash('Test123!', SALT_ROUNDS);
    
    await db.collection('candidates').insertOne({
      _id: candidate1Id,
      email: 'candidate1@test.com',
      phone: '+33600000001',
      name: 'Test',
      surname: 'User1',
      gender: 'M.',
      city: 'Paris',
      registration_source: 'platform',
      registration_date: new Date(),
      is_deleted: false,
      created_at: new Date(),
      updated_at: new Date()
    });

    await db.collection('candidate_system_info').insertOne({
      candidate_id: candidate1Id,
      password_hash: password1,
      is_active: true,
      last_login: new Date(),
      registration_ip: '127.0.0.1',
      failed_login_attempts: 0,
      is_locked: false,
      created_at: new Date(),
      updated_at: new Date()
    });

    await db.collection('candidate_test_scores').insertOne({
      candidate_id: candidate1Id,
      profile_complete: false,
      cgu_accepted: false,
      newsletter_subscription: false,
      email_notifications: true,
      sms_notifications: false,
      profile_visibility: 'public',
      secure_data_sharing: false,
      french_test_completed: false,
      french_linguistic_score: null,
      french_soft_skills_score: null,
      french_job_skills_score: null,
      french_overall_score: null,
      english_test_completed: false,
      english_linguistic_score: null,
      english_soft_skills_score: null,
      english_job_skills_score: null,
      english_overall_score: null,
      italian_test_completed: false,
      italian_linguistic_score: null,
      italian_soft_skills_score: null,
      italian_job_skills_score: null,
      italian_overall_score: null,
      spanish_test_completed: false,
      spanish_linguistic_score: null,
      spanish_soft_skills_score: null,
      spanish_job_skills_score: null,
      spanish_overall_score: null,
      german_test_completed: false,
      german_linguistic_score: null,
      german_soft_skills_score: null,
      german_job_skills_score: null,
      german_overall_score: null,
      created_at: new Date(),
      updated_at: new Date()
    });

    console.log('  ✓ Candidate 1: Basic signup (candidate1@test.com / Test123!)');

    // Candidate 2: Completed forms + language test
    const candidate2Id = new ObjectId();
    const password2 = await bcrypt.hash('Test123!', SALT_ROUNDS);
    
    await db.collection('candidates').insertOne({
      _id: candidate2Id,
      email: 'candidate2@test.com',
      phone: '+33612345678',
      name: 'Dupont',
      surname: 'Jean',
      gender: 'M.',
      city: 'Paris',
      registration_source: 'platform',
      registration_date: new Date(),
      is_deleted: false,
      created_at: new Date(),
      updated_at: new Date()
    });

    await db.collection('candidate_system_info').insertOne({
      candidate_id: candidate2Id,
      password_hash: password2,
      is_active: true,
      last_login: new Date(),
      registration_ip: '127.0.0.1',
      failed_login_attempts: 0,
      is_locked: false,
      created_at: new Date(),
      updated_at: new Date()
    });

    await db.collection('candidate_profiles').insertOne({
      candidate_id: candidate2Id,
      desired_position: 'Téléconseiller',
      call_center_experience: '3 ans',
      position_experience: '2 ans',
      primary_activity: 'Service Client',
      primary_activity_experience: '2 ans',
      operation_1: 'Ventes',
      operation_1_experience: '1 an',
      operation_2: null,
      operation_2_experience: null,
      native_language: 'Français',
      foreign_language_1: 'Anglais',
      foreign_language_1_level: 'Avancé',
      foreign_language_2: null,
      foreign_language_2_level: null,
      is_bilingual: false,
      created_at: new Date(),
      updated_at: new Date()
    });

    await db.collection('candidate_availability').insertOne({
      candidate_id: candidate2Id,
      work_mode: 'Remote',
      work_time: 'Temps plein',
      work_park: 'Paris',
      created_at: new Date(),
      updated_at: new Date()
    });

    await db.collection('candidate_contract_preferences').insertOne({
      candidate_id: candidate2Id,
      created_at: new Date(),
      updated_at: new Date()
    });

    await db.collection('candidate_test_scores').insertOne({
      candidate_id: candidate2Id,
      profile_complete: true,
      cgu_accepted: true,
      cgu_accepted_at: new Date(),
      newsletter: true,
      email_notifications: true,
      sms_notifications: true,
      profile_visibility: 'public',
      secure_data_sharing: true,
      french_test_completed: true,
      french_linguistic_score: 85,
      french_soft_skills_score: 78,
      french_job_skills_score: 82,
      french_overall_score: 82,
      english_test_completed: true,
      english_linguistic_score: 75,
      english_soft_skills_score: 70,
      english_job_skills_score: 73,
      english_overall_score: 73,
      italian_test_completed: false,
      italian_linguistic_score: null,
      italian_soft_skills_score: null,
      italian_job_skills_score: null,
      italian_overall_score: null,
      spanish_test_completed: false,
      spanish_linguistic_score: null,
      spanish_soft_skills_score: null,
      spanish_job_skills_score: null,
      spanish_overall_score: null,
      german_test_completed: false,
      german_linguistic_score: null,
      german_soft_skills_score: null,
      german_job_skills_score: null,
      german_overall_score: null,
      created_at: new Date(),
      updated_at: new Date()
    });

    console.log('  ✓ Candidate 2: Full profile + language tests (candidate2@test.com / Test123!)');

    // Candidate 3: Fully completed - all fields and multiple language tests
    const candidate3Id = new ObjectId();
    const password3 = await bcrypt.hash('Test123!', SALT_ROUNDS);
    
    await db.collection('candidates').insertOne({
      _id: candidate3Id,
      email: 'candidate3@test.com',
      phone: '+34612345678',
      name: 'Garcia',
      surname: 'Maria',
      gender: 'Mme',
      city: 'Madrid',
      registration_source: 'platform',
      registration_date: new Date(),
      is_deleted: false,
      created_at: new Date(),
      updated_at: new Date()
    });

    await db.collection('candidate_system_info').insertOne({
      candidate_id: candidate3Id,
      password_hash: password3,
      is_active: true,
      last_login: new Date(),
      registration_ip: '127.0.0.1',
      failed_login_attempts: 0,
      is_locked: false,
      created_at: new Date(),
      updated_at: new Date()
    });

    await db.collection('candidate_profiles').insertOne({
      candidate_id: candidate3Id,
      desired_position: 'Superviseur Centre d\'Appels',
      call_center_experience: '7 ans',
      position_experience: '5 ans',
      primary_activity: 'Management d\'équipe',
      primary_activity_experience: '5 ans',
      operation_1: 'Support technique',
      operation_1_experience: '3 ans',
      operation_2: 'Ventes',
      operation_2_experience: '2 ans',
      native_language: 'Espagnol',
      foreign_language_1: 'Anglais',
      foreign_language_1_level: 'Avancé',
      foreign_language_2: 'Français',
      foreign_language_2_level: 'Intermédiaire',
      is_bilingual: true,
      created_at: new Date(),
      updated_at: new Date()
    });

    await db.collection('candidate_availability').insertOne({
      candidate_id: candidate3Id,
      work_mode: 'Hybride',
      work_time: 'Temps plein',
      work_park: 'Madrid',
      created_at: new Date(),
      updated_at: new Date()
    });

    await db.collection('candidate_contract_preferences').insertOne({
      candidate_id: candidate3Id,
      created_at: new Date(),
      updated_at: new Date()
    });

    await db.collection('candidate_test_scores').insertOne({
      candidate_id: candidate3Id,
      profile_complete: true,
      cgu_accepted: true,
      cgu_accepted_at: new Date(),
      newsletter: true,
      email_notifications: true,
      sms_notifications: true,
      profile_visibility: 'public',
      secure_data_sharing: true,
      french_test_completed: true,
      french_linguistic_score: 92,
      french_soft_skills_score: 88,
      french_job_skills_score: 90,
      french_overall_score: 90,
      english_test_completed: true,
      english_linguistic_score: 95,
      english_soft_skills_score: 93,
      english_job_skills_score: 94,
      english_overall_score: 94,
      italian_test_completed: true,
      italian_linguistic_score: 68,
      italian_soft_skills_score: 72,
      italian_job_skills_score: 70,
      italian_overall_score: 70,
      spanish_test_completed: true,
      spanish_linguistic_score: 98,
      spanish_soft_skills_score: 96,
      spanish_job_skills_score: 97,
      spanish_overall_score: 97,
      german_test_completed: true,
      german_linguistic_score: 55,
      german_soft_skills_score: 58,
      german_job_skills_score: 56,
      german_overall_score: 56,
      created_at: new Date(),
      updated_at: new Date()
    });

    console.log('  ✓ Candidate 3: Fully completed profile (candidate3@test.com / Test123!)');
    console.log('✓ Successfully seeded 3 test candidates\n');

  } catch (error) {
    console.error('✗ Error seeding candidates:', error.message);
    throw error;
  }
}
