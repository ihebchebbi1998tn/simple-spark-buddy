# Call Center Match - Database Schema

---de

## Table: `candidates`

Main table storing candidate/lead contact information.kk

| Field Name | Data Type | Description |
|------------|-----------|-------------|
| `candidate_id` | ObjectId | Id lead (MongoDB primary key) |
| `gender` | Integer | Civilité (1=M., 2=Mme, 3=Mlle) |
| `surname` | String(100) | Nom |
| `name` | String(100) | Prénom |
| `phone` | String(20) | Numéro de Tel |
| `email` | String(255) | E-mail (Unique, Indexed) |
| `city` | Integer | Ville (FK to cities) |
| `age` | Integer | Age |
| `birthday` | Date | Date of birth |
| `password` | String(255) | Mot de passe (hashed) |
| `registration_source` | String | Source d'enregistrement ("platform" or "facebook") |
| `registration_date` | Date | Date d'inscription / Date d'insertion dans la BDD |
| `created_at` | Date | Record creation |
| `updated_at` | Date | DateLastupdate |

---

## Table: `candidate_profiles`

CV/Profile information for candidates.

| Field Name | Data Type | Description |
|------------|-----------|-------------|
| `profile_id` | ObjectId | Primary key |
| `candidate_id` | ObjectId | **FK to candidates.candidate_id** |
| `call_center_experience` | Integer | Expérience globale en centres d'appels (1-7 scale) |
| `call_center_self_assessment` | Integer | Auto-évaluation (1-4 scale) |
| `desired_position` | Integer | Poste (FK to positions) |
| `position_experience` | Integer | Expérience poste (1-7 scale) |
| `position_self_assessment` | Integer | Auto-évaluation (1-4 scale) |
| `primary_activity` | Integer | Activité principale (FK to activities) |
| `primary_activity_experience` | Integer | Expérience activité principale (1-7 scale) |
| `primary_operation` | Integer | Opération principale (FK to operations) |
| `primary_operation_experience` | Integer | Expérience opération principale (1-7 scale) |
| `primary_training_needs` | String[] | Besoin en formation |
| `secondary_activity` | Integer | Activité 2 (FK to activities) |
| `secondary_activity_experience` | Integer | Expérience activité 2 (1-7 scale) |
| `secondary_operation` | Integer | Opération 2 (FK to operations) |
| `secondary_operation_experience` | Integer | Expérience opération 2 (1-7 scale) |
| `secondary_training_needs` | String[] | Besoin en formation |
| `tertiary_activity` | Integer | Activité 3 (FK to activities) |
| `tertiary_activity_experience` | Integer | Expérience activité 3 (1-7 scale) |
| `tertiary_operation` | Integer | Opération 3 (FK to operations) |
| `tertiary_operation_experience` | Integer | Expérience opération 3 (1-7 scale) |
| `tertiary_training_needs` | String[] | Besoin en formation |
| `mother_tongue` | Integer | Langue maternelle (FK to languages) |
| `foreign_language_1` | Integer | Langue étrangère 1 (FK to languages) |
| `foreign_language_1_level` | Integer | Niveau langue étrangère 1 (1-5 scale) |
| `foreign_language_2` | Integer | Langue étrangère 2 (FK to languages) |
| `foreign_language_2_level` | Integer | Niveau langue étrangère 2 (1-5 scale) |
| `created_at` | Date | Record creation |
| `updated_at` | Date | Last update |

---

## Table: `candidate_availability`

Availability and work preferences for candidates.

| Field Name | Data Type | Description |
|------------|-----------|-------------|
| `availability_id` | ObjectId | Primary key |
| `candidate_id` | ObjectId | **FK to candidates.candidate_id** |
| `work_mode` | Integer[] | Mode de travail (1=présentiel, 2=télétravail, 3=hybride) |
| `work_time` | Integer[] | Temps de Travail (1=temps plein, 2=temps partiel) |
| `work_park` | Integer[] | Parc de travail (1=jour, 2=nuit) |
| `availability` | Integer | Disponibilité (1-4 scale) |
| `blacklisted_call_center_1` | Integer | Black liste centres d'appels 1 (FK to call_centers) |
| `blacklisted_call_center_2` | Integer | Black liste centres d'appels 2 |
| `blacklisted_call_center_3` | Integer | Black liste centres d'appels 3 |
| `blacklisted_call_center_4` | Integer | Black liste centres d'appels 4 |
| `blacklisted_call_center_5` | Integer | Black liste centres d'appels 5 |
| `blacklisted_call_center_6` | Integer | Black liste centres d'appels 6 |
| `blacklisted_call_center_7` | Integer | Black liste centres d'appels 7 |
| `blacklisted_call_center_8` | Integer | Black liste centres d'appels 8 |
| `blacklisted_call_center_9` | Integer | Black liste centres d'appels 9 |
| `blacklisted_call_center_10` | Integer | Black liste centres d'appels 10 |
| `preferred_call_center_1` | Integer | White liste centres d'appels 1 (FK to call_centers) |
| `preferred_call_center_2` | Integer | White liste centres d'appels 2 |
| `preferred_call_center_3` | Integer | White liste centres d'appels 3 |
| `preferred_call_center_4` | Integer | White liste centres d'appels 4 |
| `preferred_call_center_5` | Integer | White liste centres d'appels 5 |
| `preferred_call_center_6` | Integer | White liste centres d'appels 6 |
| `preferred_call_center_7` | Integer | White liste centres d'appels 7 |
| `preferred_call_center_8` | Integer | White liste centres d'appels 8 |
| `preferred_call_center_9` | Integer | White liste centres d'appels 9 |
| `preferred_call_center_10` | Integer | White liste centres d'appels 10 |
| `work_other_city` | Boolean | Travail dans une autre ville |
| `work_abroad` | Boolean | Travail à l'étranger |
| `created_at` | Date | Record creation |
| `updated_at` | Date | Last update |

---

## Table: `candidate_contract_preferences`

Contract preferences for candidates.

| Field Name | Data Type | Description |
|------------|-----------|-------------|
| `preference_id` | ObjectId | Primary key |
| `candidate_id` | ObjectId | **FK to candidates.candidate_id** |
| `contract_types` | String[] | Type de contrat |
| `salary_expectation` | String | Prétentions salariales |
| `meal_vouchers` | Boolean | Ticket restaurant |
| `health_insurance` | Boolean | Mutuelle santé |
| `transportation` | Boolean | Transport / Prime de transport |
| `performance_bonuses` | Boolean | Primes sur objectif |
| `created_at` | Date | Record creation |
| `updated_at` | Date | Last update |

---

## Table: `candidate_test_scores`

Language test scores and additional profile information for candidates.

| Field Name | Data Type | Description |
|------------|-----------|-------------|
| `test_id` | ObjectId | Primary key |
| `candidate_id` | ObjectId | **FK to candidates.candidate_id** |
| `profile_complete` | Boolean | Profil complet oui / non |
| `french_test_completed` | Boolean | Test linguistique Français : complété / non complété |
| `french_linguistic_score` | Integer | Score linguistique (0-100) |
| `french_soft_skills_score` | Integer | Score soft skills (0-100) |
| `french_job_skills_score` | Integer | Score compétences métier (0-100) |
| `french_overall_score` | Integer | Score global (0-100) |
| `english_test_completed` | Boolean | Test linguistique Anglais : complété / non complété |
| `english_linguistic_score` | Integer | Score linguistique (0-100) |
| `english_soft_skills_score` | Integer | Score soft skills (0-100) |
| `english_job_skills_score` | Integer | Score compétences métier (0-100) |
| `english_overall_score` | Integer | Score global (0-100) |
| `italian_test_completed` | Boolean | Test linguistique Italien : complété / non complété |
| `italian_linguistic_score` | Integer | Score linguistique (0-100) |
| `italian_soft_skills_score` | Integer | Score soft skills (0-100) |
| `italian_job_skills_score` | Integer | Score compétences métier (0-100) |
| `italian_overall_score` | Integer | Score global (0-100) |
| `spanish_test_completed` | Boolean | Test linguistique Espagnol : complété / non complété |
| `spanish_linguistic_score` | Integer | Score linguistique (0-100) |
| `spanish_soft_skills_score` | Integer | Score soft skills (0-100) |
| `spanish_job_skills_score` | Integer | Score compétences métier (0-100) |
| `spanish_overall_score` | Integer | Score global (0-100) |
| `german_test_completed` | Boolean | Test linguistique Allemand : complété / non complété |
| `german_linguistic_score` | Integer | Score linguistique (0-100) |
| `german_soft_skills_score` | Integer | Score soft skills (0-100) |
| `german_job_skills_score` | Integer | Score compétences métier (0-100) |
| `german_overall_score` | Integer | Score global (0-100) |
| `newsletter` | Boolean | Newsletter |
| `email_notifications` | Boolean | Notifications par email |
| `sms_notifications` | Boolean | Notifications par SMS |
| `profile_visibility` | String | Visibilité du profil |
| `secure_data_sharing` | Boolean | Partage sécurisé des données |
| `created_at` | Date | Record creation |
| `updated_at` | Date | Last update |

---

## Table: `candidate_system_info`

System and authentication information for candidates.

| Field Name | Data Type | Description |
|------------|-----------|-------------|
| `system_id` | ObjectId | Primary key |
| `candidate_id` | ObjectId | **FK to candidates.candidate_id** |
| `ip_address` | String(45) | Adresse IP |
| `device` | String(255) | Device |
| `last_login` | Date | Last login |
| `failed_login_attempts` | Integer | failedLoginAttempts |
| `last_failed_login` | Date | lastFailedLogin |
| `enabled` | Boolean | Enabled |
| `deleted` | Boolean | Deleted |
| `deleted_date` | Date | Deleted Date |
| `last_update_section` | String(100) | Last update section |
| `created_at` | Date | Record creation |
| `updated_at` | Date | Last update |

