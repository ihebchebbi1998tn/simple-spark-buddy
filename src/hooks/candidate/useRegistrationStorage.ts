import { useState, useEffect, useCallback } from 'react';

const STORAGE_KEY = 'candidateRegistrationData';

export interface RegistrationData {
  // Step 1
  posteRecherche?: string;
  experienceGlobale?: string;
  experiencePosteRecherche?: string;
  civilite?: string;
  nom?: string;
  prenom?: string;
  telephone?: string;
  villeResidence?: string;
  // Step 2: Experience
  activitePrincipale?: string;
  experienceActivitePrincipale?: string;
  operation1?: string;
  experienceOperation1?: string;
  operation2?: string;
  experienceOperation2?: string;
  // Step 3: Languages
  langueMaternelle?: string;
  langueForeign1?: string;
  niveauLangueForeign1?: string;
  hasSecondForeignLanguage?: string;
  langueForeign2?: string;
  niveauLangueForeign2?: string;
  bilingue?: string;
  // Step 4: Work Preferences
  modeTravail?: string;
  tempsTravail?: string;
  parcTravail?: string;
  // Step 5: Authentication
  email?: string;
  motDePasse?: string;
  dateNaissance?: string;
  acceptCGU?: boolean;
  // Test data - Enhanced to support detailed scores
  testLanguage?: string;
  testScore?: number | {
    linguistic?: number;
    linguisticScore?: number;
    softSkills?: number;
    soft_skills_score?: number;
    jobSkills?: number;
    job_skills_score?: number;
    overall?: number;
    overallScore?: number;
    score?: number;
  };
  fromLanguageTest?: boolean;
}

export function useRegistrationStorage() {
  const [data, setData] = useState<RegistrationData>(() => {
    try {
      const stored = localStorage.getItem(STORAGE_KEY);
      return stored ? JSON.parse(stored) : {};
    } catch {
      return {};
    }
  });

  const updateData = useCallback((updates: Partial<RegistrationData>) => {
    setData((prev) => {
      const newData = { ...prev, ...updates };
      try {
        localStorage.setItem(STORAGE_KEY, JSON.stringify(newData));
      } catch (error) {
        console.error('Failed to save to localStorage:', error);
      }
      return newData;
    });
  }, []);

  const clearData = useCallback(() => {
    try {
      localStorage.removeItem(STORAGE_KEY);
      setData({});
    } catch (error) {
      console.error('Failed to clear localStorage:', error);
    }
  }, []);

  return {
    data,
    updateData,
    clearData,
  };
}
