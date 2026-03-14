import type { ReactNode } from "react";
import type { PasswordStrength } from "@/utils/formValidation";

export interface RegistrationFormData {
  activitePrincipale: string;
  experienceActivitePrincipale: string;
  operation1: string;
  experienceOperation1: string;
  operation2: string;
  experienceOperation2: string;
  langueMaternelle: string;
  langueForeign1: string;
  niveauLangueForeign1: string;
  hasSecondForeignLanguage: string;
  langueForeign2: string;
  niveauLangueForeign2: string;
  bilingue: string;
  modeTravail: string;
  tempsTravail: string;
  parcTravail: string;
  email: string;
  motDePasse: string;
  confirmationMotDePasse: string;
  dateNaissance: string;
  acceptCGU: boolean;
}

export interface StepProps {
  formData: RegistrationFormData;
  handleInputChange: (field: string, value: string | boolean) => void;
}

export interface ExperienceStepProps extends StepProps {
  hasMinimalExperience: boolean;
  filteredOperations: Array<{ value: string; label: string }>;
  filteredActivityExperience: Array<{ value: string; label: string }>;
  filteredOperation1Experience: Array<{ value: string; label: string }>;
  filteredOperation2Experience: Array<{ value: string; label: string }>;
  isOperationsExperienceExceeded: boolean;
}

export interface LanguagesStepProps extends StepProps {
  renderStars: (count: number) => ReactNode[];
}

export interface AccountStepProps extends StepProps {
  fieldValidation: Record<string, { isValid: boolean; error?: string }>;
  emailSuggestion?: string;
  setEmailSuggestion: (s: string | undefined) => void;
  emailExists: boolean | null;
  isCheckingEmail: boolean;
  passwordStrength: PasswordStrength | null;
  backendPasswordError: string | null;
  showPassword: boolean;
  setShowPassword: (v: boolean) => void;
  showConfirmPassword: boolean;
  setShowConfirmPassword: (v: boolean) => void;
}
