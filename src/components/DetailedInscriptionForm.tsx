"use client"

import { useState, useMemo, useEffect } from "react"
import { useLocation, useNavigate } from "react-router-dom"
import { useRegistrationStorage } from "@/hooks/useRegistrationStorage"
import { useAvailabilityCheck } from "@/hooks/useAvailabilityCheck"
import { motion } from "framer-motion"
import { Button } from "@/components/ui/button"
import { Progress } from "@/components/ui/progress"
import {
  ChevronLeft,
  Briefcase,
  Languages,
  MapPin,
  Mail,
  Lock,
  CheckCircle,
  ArrowRight,
  Star,
} from "lucide-react"
import { filterExperienceLevelsByGlobal, OPERATIONS, getOperationsForActivity } from "@/utils/constants"
import ParticleBackground from "@/components/shared/ParticleBackground"
import { useToast } from "@/hooks/use-toast"
import {
  formatPhoneNumber,
  validateEmail,
  checkPasswordStrength,
  validateField,
  validatePasswordRequirements,
  type PasswordStrength,
} from "@/utils/formValidation"
import { ExperienceStep } from "@/components/registration/ExperienceStep"
import { LanguagesStep } from "@/components/registration/LanguagesStep"
import { WorkPreferencesStep } from "@/components/registration/WorkPreferencesStep"
import { AccountStep } from "@/components/registration/AccountStep"
import type { RegistrationFormData } from "@/components/registration/types"

interface Step1Data {
  posteRecherche: string
  experienceGlobale: string
  experiencePosteRecherche?: string
  civilite: string
  nom: string
  prenom: string
  telephone: string
  villeResidence: string
}

const DetailedInscriptionForm = () => {
  const location = useLocation()
  const navigate = useNavigate()
  const { data: storedData, updateData, clearData } = useRegistrationStorage()

  // Load language test data from location, stored data, or localStorage (languageTestResult)
  const getTestDataFromLocalStorage = () => {
    try {
      const stored = localStorage.getItem('languageTestResult');
      if (stored) {
        const testResult = JSON.parse(stored);
        return {
          fromLanguageTest: true,
          testLanguage: testResult.language,
          testScore: testResult.totalScore && testResult.totalQuestions
            ? Math.round((testResult.totalScore / testResult.totalQuestions) * 100)
            : null,
          detailedScores: {
            linguistic: testResult.scores?.language?.percentage || null,
            softSkills: testResult.scores?.softskills?.percentage || null,
            jobSkills: testResult.scores?.competences?.percentage || null,
            overall: testResult.totalScore && testResult.totalQuestions
              ? Math.round((testResult.totalScore / testResult.totalQuestions) * 100)
              : null
          }
        };
      }
    } catch (e) {
      console.error('Error reading languageTestResult from localStorage:', e);
    }
    return null;
  };

  const localStorageTestData = getTestDataFromLocalStorage();
  const fromLanguageTest = location.state?.fromLanguageTest ?? storedData.fromLanguageTest ?? localStorageTestData?.fromLanguageTest ?? false
  const testScore = location.state?.testScore ?? storedData.testScore ?? localStorageTestData?.testScore ?? localStorageTestData?.detailedScores ?? null
  const testLanguage = location.state?.testLanguage ?? storedData.testLanguage ?? localStorageTestData?.testLanguage ?? null

  const detailedTestScores = location.state?.testScore && typeof location.state.testScore === 'object'
    ? location.state.testScore
    : storedData.testScore && typeof storedData.testScore === 'object'
      ? storedData.testScore
      : localStorageTestData?.detailedScores ?? null;

  const [currentStep, setCurrentStep] = useState(0)
  const [fieldValidation, setFieldValidation] = useState<Record<string, { isValid: boolean; error?: string }>>({})
  const [emailSuggestion, setEmailSuggestion] = useState<string | undefined>()
  const [passwordStrength, setPasswordStrength] = useState<PasswordStrength | null>(null)
  const [backendPasswordError, setBackendPasswordError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [formData, setFormData] = useState<RegistrationFormData>({
    activitePrincipale: storedData.activitePrincipale || "",
    experienceActivitePrincipale: storedData.experienceActivitePrincipale || "",
    operation1: storedData.operation1 || "",
    experienceOperation1: storedData.experienceOperation1 || "",
    operation2: storedData.operation2 || "",
    experienceOperation2: storedData.experienceOperation2 || "",
    langueMaternelle: storedData.langueMaternelle || "",
    langueForeign1: storedData.langueForeign1 || "",
    niveauLangueForeign1: storedData.niveauLangueForeign1 || "",
    hasSecondForeignLanguage: storedData.hasSecondForeignLanguage || "",
    langueForeign2: storedData.langueForeign2 || "",
    niveauLangueForeign2: storedData.niveauLangueForeign2 || "",
    bilingue: storedData.bilingue || "",
    modeTravail: storedData.modeTravail || "",
    tempsTravail: storedData.tempsTravail || "",
    parcTravail: storedData.parcTravail || "",
    email: storedData.email || "",
    motDePasse: "",
    confirmationMotDePasse: "",
    dateNaissance: storedData.dateNaissance || "",
    acceptCGU: storedData.acceptCGU || false,
  })
  const { toast } = useToast()
  const [showPassword, setShowPassword] = useState(false)
  const [showConfirmPassword, setShowConfirmPassword] = useState(false)

  const { emailExists, isCheckingEmail } = useAvailabilityCheck(formData.email, '', 600)

  const hasMinimalExperience = ["0", "1"].includes(storedData?.experienceGlobale || "0");

  const experienceActivite = [
    { value: "1", label: "0 - 6 mois" },
    { value: "2", label: "6 mois - 12 mois" },
    { value: "3", label: "1 an - 2 ans" },
    { value: "4", label: "2 ans - 3 ans" },
    { value: "5", label: "3 ans - 5 ans" },
    { value: "6", label: "5 ans - 7 ans" },
    { value: "7", label: "Plus de 7 ans" },
  ];

  const experienceToMonths = (value: string): number => {
    const mapping: Record<string, number> = { "1": 6, "2": 12, "3": 24, "4": 36, "5": 60, "6": 84, "7": 120 };
    return mapping[value] || 0;
  };

  const activityExperienceMonths = experienceToMonths(formData.experienceActivitePrincipale || "0");

  const usedOperationsExperienceMonths = useMemo(() => {
    let used = 0;
    if (formData.experienceOperation1) used += experienceToMonths(formData.experienceOperation1);
    if (formData.experienceOperation2) used += experienceToMonths(formData.experienceOperation2);
    return used;
  }, [formData.experienceOperation1, formData.experienceOperation2]);

  const filteredActivityExperience = useMemo(() => {
    return filterExperienceLevelsByGlobal(storedData?.experienceGlobale || "0", experienceActivite);
  }, [storedData?.experienceGlobale]);

  const filteredOperation1Experience = useMemo(() => {
    if (!formData.experienceActivitePrincipale) return [];
    const usedByOp2 = formData.experienceOperation2 ? experienceToMonths(formData.experienceOperation2) : 0;
    const remainingBudget = activityExperienceMonths - usedByOp2;
    const filtered = experienceActivite.filter(exp => experienceToMonths(exp.value) <= remainingBudget);
    return filtered.length === 0 && experienceActivite.length > 0 ? [experienceActivite[0]] : filtered;
  }, [formData.experienceActivitePrincipale, formData.experienceOperation2, activityExperienceMonths]);

  const filteredOperation2Experience = useMemo(() => {
    if (!formData.experienceActivitePrincipale) return [];
    const usedByOp1 = formData.experienceOperation1 ? experienceToMonths(formData.experienceOperation1) : 0;
    const remainingBudget = activityExperienceMonths - usedByOp1;
    const filtered = experienceActivite.filter(exp => experienceToMonths(exp.value) <= remainingBudget);
    return filtered.length === 0 && experienceActivite.length > 0 ? [experienceActivite[0]] : filtered;
  }, [formData.experienceActivitePrincipale, formData.experienceOperation1, activityExperienceMonths]);

  const isOperationsExperienceExceeded = usedOperationsExperienceMonths > activityExperienceMonths;

  const filteredOperations = useMemo(() => {
    return getOperationsForActivity(formData.activitePrincipale, OPERATIONS);
  }, [formData.activitePrincipale]);

  // Auto-set experience for minimal experience users
  useEffect(() => {
    if (hasMinimalExperience) {
      if (formData.activitePrincipale && !formData.experienceActivitePrincipale) {
        setFormData(prev => ({ ...prev, experienceActivitePrincipale: "1" }));
        updateData({ ...formData, experienceActivitePrincipale: "1" });
      }
      if (formData.operation1 && !formData.experienceOperation1) {
        setFormData(prev => ({ ...prev, experienceOperation1: "1" }));
        updateData({ ...formData, experienceOperation1: "1" });
      }
      if (formData.operation2 && !formData.experienceOperation2) {
        setFormData(prev => ({ ...prev, experienceOperation2: "1" }));
        updateData({ ...formData, experienceOperation2: "1" });
      }
    }
  }, [formData.activitePrincipale, formData.operation1, formData.operation2, hasMinimalExperience]);

  // Reset operations experience if exceeding budget
  useEffect(() => {
    if (!hasMinimalExperience && formData.experienceActivitePrincipale) {
      const activityBudget = experienceToMonths(formData.experienceActivitePrincipale);
      if (formData.experienceOperation1) {
        const usedByOp2 = formData.experienceOperation2 ? experienceToMonths(formData.experienceOperation2) : 0;
        if (experienceToMonths(formData.experienceOperation1) > activityBudget - usedByOp2) {
          setFormData(prev => ({ ...prev, experienceOperation1: "" }));
          updateData({ experienceOperation1: "" });
        }
      }
      if (formData.experienceOperation2) {
        const usedByOp1 = formData.experienceOperation1 ? experienceToMonths(formData.experienceOperation1) : 0;
        if (experienceToMonths(formData.experienceOperation2) > activityBudget - usedByOp1) {
          setFormData(prev => ({ ...prev, experienceOperation2: "" }));
          updateData({ experienceOperation2: "" });
        }
      }
    }
  }, [formData.experienceActivitePrincipale, hasMinimalExperience]);

  // Reset operations when activity changes
  useEffect(() => {
    if (formData.activitePrincipale) {
      const validOperationValues = filteredOperations.map(op => op.value);
      if (formData.operation1 && !validOperationValues.includes(formData.operation1)) {
        setFormData(prev => ({ ...prev, operation1: "", experienceOperation1: "" }));
        updateData({ operation1: "", experienceOperation1: "" });
      }
      if (formData.operation2 && !validOperationValues.includes(formData.operation2)) {
        setFormData(prev => ({ ...prev, operation2: "", experienceOperation2: "" }));
        updateData({ operation2: "", experienceOperation2: "" });
      }
    }
  }, [formData.activitePrincipale, filteredOperations]);

  const renderStars = (count: number) => {
    return Array.from({ length: 5 }, (_, i) => (
      <Star key={i} className={`h-3 w-3 ${i < count ? "fill-yellow-400 text-yellow-400" : "text-gray-300"}`} />
    ))
  }

  const steps = [
    { id: "experience", title: "Parlez-nous de votre besoin.", subtitle: "Renseignez les détails du poste recherché et aidez-nous à mieux comprendre vos attentes.", icon: Briefcase },
    { id: "languages", title: "Parlez-nous de vos Compétences Linguistiques", subtitle: "Indiquez les langues que vous maîtrisez", icon: Languages },
    { id: "work-preferences", title: "Vos Préférences de Travail", subtitle: "Présentiel ou Télétravail, Temps plein ou Mi-temps, c'est à vous de choisir !", icon: MapPin },
    { id: "account", title: "Création du Compte", subtitle: "C'est presque fini !", icon: Mail },
  ]

  // Step completion
  const stepCompletion = useMemo(() => {
    const getStepFields = (step: number) => {
      switch (step) {
        case 0:
          if (hasMinimalExperience) {
            const base = [formData.activitePrincipale, formData.operation1];
            if (formData.operation2) base.push(formData.operation2);
            return base;
          }
          const baseFields = [formData.activitePrincipale, formData.experienceActivitePrincipale, formData.operation1, formData.experienceOperation1];
          if (formData.operation2) baseFields.push(formData.operation2, formData.experienceOperation2);
          return baseFields;
        case 1:
          const langFields = [formData.langueMaternelle, formData.langueForeign1, formData.niveauLangueForeign1, formData.hasSecondForeignLanguage];
          if (formData.hasSecondForeignLanguage === "oui") langFields.push(formData.langueForeign2, formData.niveauLangueForeign2);
          return langFields;
        case 2: return [formData.modeTravail, formData.tempsTravail, formData.parcTravail];
        case 3: return [formData.email, formData.motDePasse, formData.confirmationMotDePasse, formData.dateNaissance, formData.acceptCGU];
        default: return [];
      }
    };
    const fields = getStepFields(currentStep);
    const completed = fields.filter(f => f).length;
    const total = fields.length;
    return { completed, total, percentage: total > 0 ? (completed / total) * 100 : 0, isComplete: completed === total };
  }, [currentStep, formData, hasMinimalExperience]);

  const getMotivationalMessage = () => {
    const { percentage } = stepCompletion;
    if (percentage === 100) return "Parfait ! Passez à l'étape suivante✅";
    if (percentage >= 75) return "Vous y êtes presque ! Continuez comme ça 💪";
    if (percentage >= 50) return "Excellent progrès ! Plus que quelques champs ✨";
    if (percentage >= 25) return "Bon départ ! Continuez à remplir le formulaire 🚀";
    return "Commencez en remplissant les champs ci-dessous 👇";
  };

  const handleInputChange = (field: string, value: string | boolean) => {
    let finalValue = value;
    if (field === "telephone" && typeof value === "string") finalValue = formatPhoneNumber(value);
    if (field === "email" && typeof value === "string") {
      const emailCheck = validateEmail(value);
      setEmailSuggestion(emailCheck.suggestion || undefined);
    }
    if (field === "motDePasse" && typeof value === "string") {
      setPasswordStrength(checkPasswordStrength(value));
      setBackendPasswordError(null);
    }
    if (typeof value === "string") {
      setFieldValidation(prev => ({ ...prev, [field]: validateField(field, value) }));
    }
    const updatedFormData = { ...formData, [field]: finalValue };
    setFormData(updatedFormData);
    if (field !== "motDePasse" && field !== "confirmationMotDePasse") {
      updateData({ [field]: finalValue });
    }
  };

  const validateStep = (): boolean => {
    if (currentStep === 0) {
      if (!formData.activitePrincipale || !formData.operation1) {
        toast({ title: "Champs manquants", description: "Veuillez remplir tous les champs obligatoires.", variant: "destructive" });
        return false;
      }
      if (!hasMinimalExperience) {
        if (!formData.experienceActivitePrincipale || !formData.experienceOperation1) {
          toast({ title: "Champs manquants", description: "Veuillez indiquer votre expérience dans les activités sélectionnées.", variant: "destructive" });
          return false;
        }
        if (formData.operation2 && !formData.experienceOperation2) {
          toast({ title: "Champ manquant", description: "Veuillez indiquer votre expérience pour l'opération 2.", variant: "destructive" });
          return false;
        }
        if (isOperationsExperienceExceeded) {
          toast({ title: "Expérience incohérente", description: "L'expérience dans une opération ne peut pas être supérieure à celle de l'activité choisie.", variant: "destructive" });
          return false;
        }
      }
    } else if (currentStep === 1) {
      if (!formData.langueMaternelle || !formData.langueForeign1 || !formData.niveauLangueForeign1 || !formData.hasSecondForeignLanguage) {
        toast({ title: "Champs manquants", description: "Veuillez remplir tous les champs obligatoires.", variant: "destructive" });
        return false;
      }
      if (formData.hasSecondForeignLanguage === "oui" && (!formData.langueForeign2 || !formData.niveauLangueForeign2)) {
        toast({ title: "Champs manquants", description: "Veuillez compléter les informations sur votre deuxième langue étrangère.", variant: "destructive" });
        return false;
      }
    } else if (currentStep === 2) {
      if (!formData.modeTravail || !formData.tempsTravail || !formData.parcTravail) {
        toast({ title: "Champs manquants", description: "Veuillez définir vos préférences de travail.", variant: "destructive" });
        return false;
      }
    } else if (currentStep === 3) {
      if (!formData.email || !formData.motDePasse || !formData.confirmationMotDePasse || !formData.dateNaissance) {
        toast({ title: "Champs manquants", description: "Veuillez remplir tous les champs.", variant: "destructive" });
        return false;
      }
      if (emailExists) {
        toast({ title: "Email déjà utilisé", description: "Cet email est déjà associé à un compte.", variant: "destructive" });
        return false;
      }
      const passwordValidation = validatePasswordRequirements(formData.motDePasse);
      if (!passwordValidation.isValid) {
        toast({ title: "Mot de passe invalide", description: passwordValidation.errors.join('. '), variant: "destructive" });
        return false;
      }
      if (formData.motDePasse !== formData.confirmationMotDePasse) {
        toast({ title: "Erreur", description: "Les mots de passe ne correspondent pas.", variant: "destructive" });
        return false;
      }
      if (!formData.acceptCGU) {
        toast({ title: "Acceptation requise", description: "Veuillez accepter les CGU et la politique de confidentialité.", variant: "destructive" });
        return false;
      }
    }
    return true;
  };

  const handleNext = () => {
    if (validateStep() && currentStep < steps.length - 1) {
      setCurrentStep(currentStep + 1);
    }
  };

  const handlePrevious = () => {
    if (currentStep > 0) setCurrentStep(currentStep - 1);
  };

  const handleSubmit = async () => {
    if (!validateStep()) return;
    setIsSubmitting(true);
    try {
      const { leadService } = await import('@/services/leadService');
      const { authService } = await import('@/services/authService');
      const { sanitizeObject } = await import('@/utils/sanitization');

      let finalTestScores = detailedTestScores;
      if (!finalTestScores && (fromLanguageTest || testLanguage)) {
        try {
          const storedTest = localStorage.getItem('languageTestResult');
          if (storedTest) {
            const testResult = JSON.parse(storedTest);
            finalTestScores = {
              linguistic: testResult.scores?.language?.percentage || null,
              softSkills: testResult.scores?.softskills?.percentage || null,
              jobSkills: testResult.scores?.competences?.percentage || null,
              overall: testResult.totalScore && testResult.totalQuestions ? Math.round((testResult.totalScore / testResult.totalQuestions) * 100) : (typeof testScore === 'number' ? testScore : null)
            };
          }
        } catch (e) { console.error('Error parsing test scores:', e); }
      }
      if (!finalTestScores && testScore) {
        finalTestScores = typeof testScore === 'object' ? testScore : { overall: testScore };
      }

      const completeData = { ...storedData, ...formData, testLanguage, testScore: finalTestScores || testScore, fromLanguageTest };
      const sanitizedData = sanitizeObject(completeData);
      const response = await leadService.register(sanitizedData);

      if (response.success || response.message?.includes('success')) {
        const hasToken = !!response.data?.token;
        if (hasToken) {
          authService.storeAuthToken(response.data!.token);
          authService.storeUserData({ lead_id: response.data!.lead_id, email: response.data!.email, name: response.data!.last_name, surname: response.data!.first_name });
        }
        clearData();
        localStorage.removeItem('languageTestResult');

        if (hasToken) {
          toast({ title: "Inscription réussie ! 🎉", description: "Bienvenue sur votre tableau de bord." });
          if (fromLanguageTest && testLanguage && (finalTestScores || testScore)) {
            setTimeout(() => navigate("/test-langue", { state: { showResults: true, testScore: finalTestScores || testScore, testLanguage, totalQuestions: 22, userRegistered: true } }), 1500);
          } else {
            setTimeout(() => navigate("/dashboard"), 1500);
          }
        } else {
          toast({ title: "Inscription réussie ! 🎉", description: "Votre compte est en cours d'activation.", duration: 6000 });
          setTimeout(() => navigate("/espace-candidats", { state: { registrationSuccess: true, email: response.data?.email } }), 2000);
        }
      }
    } catch (error: any) {
      console.error("❌ Registration error:", error);
      let errorMessage = "Une erreur s'est produite lors de l'inscription. Veuillez réessayer.";
      let shouldShowToast = true;
      if (error.message?.includes('Password must contain') || error.message?.includes('password')) {
        setBackendPasswordError(error.message);
        shouldShowToast = false;
        setCurrentStep(3);
      } else if (error.message?.includes('Email already exists')) {
        errorMessage = "Cette adresse e-mail est déjà enregistrée.";
        setCurrentStep(3);
      } else if (error.message?.includes('network') || error.message?.includes('fetch')) {
        errorMessage = "Impossible de se connecter au serveur.";
      }
      if (shouldShowToast) toast({ title: "Erreur d'inscription", description: errorMessage, variant: "destructive" });
    } finally {
      setIsSubmitting(false);
    }
  };

  const renderStep = () => {
    switch (currentStep) {
      case 0:
        return (
          <ExperienceStep
            formData={formData}
            handleInputChange={handleInputChange}
            hasMinimalExperience={hasMinimalExperience}
            filteredOperations={filteredOperations}
            filteredActivityExperience={filteredActivityExperience}
            filteredOperation1Experience={filteredOperation1Experience}
            filteredOperation2Experience={filteredOperation2Experience}
            isOperationsExperienceExceeded={isOperationsExperienceExceeded}
          />
        );
      case 1:
        return <LanguagesStep formData={formData} handleInputChange={handleInputChange} renderStars={renderStars} />;
      case 2:
        return <WorkPreferencesStep formData={formData} handleInputChange={handleInputChange} />;
      case 3:
        return (
          <AccountStep
            formData={formData}
            handleInputChange={handleInputChange}
            fieldValidation={fieldValidation}
            emailSuggestion={emailSuggestion}
            setEmailSuggestion={setEmailSuggestion}
            emailExists={emailExists}
            isCheckingEmail={isCheckingEmail}
            passwordStrength={passwordStrength}
            backendPasswordError={backendPasswordError}
            showPassword={showPassword}
            setShowPassword={setShowPassword}
            showConfirmPassword={showConfirmPassword}
            setShowConfirmPassword={setShowConfirmPassword}
          />
        );
      default:
        return null;
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-primary/5 via-background to-primary/10 relative overflow-hidden">
      <ParticleBackground />
      <div className="container mx-auto px-3 sm:px-4 py-4 sm:py-8 relative z-10">
        <motion.div initial={{ opacity: 0, y: 30 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.8 }} className="max-w-3xl mx-auto">
          <div className="text-center pb-4 sm:pb-8">
            <h1 className="text-xl sm:text-2xl md:text-3xl font-bold bg-gradient-to-r from-primary to-primary/70 bg-clip-text text-transparent px-2">
              {steps[currentStep].title}
            </h1>
            <p className="text-sm sm:text-base text-muted-foreground mt-2 px-2">{steps[currentStep].subtitle}</p>
            <div className="mt-4 space-y-2">
              <Progress value={((currentStep + 1) / steps.length) * 100} className="h-2" />
              <div className="flex items-center justify-between text-xs text-muted-foreground">
                <span>Étape {currentStep + 2} sur {steps.length + 1}</span>
                <motion.span key={getMotivationalMessage()} initial={{ opacity: 0, y: -5 }} animate={{ opacity: 1, y: 0 }} className="text-primary font-medium">
                  {getMotivationalMessage()}
                </motion.span>
              </div>
            </div>
          </div>

          <motion.div key={currentStep} initial={{ opacity: 0, x: 20 }} animate={{ opacity: 1, x: 0 }} exit={{ opacity: 0, x: -20 }}
            className="bg-gradient-to-br from-card via-card to-primary/5 rounded-lg border-2 border-primary/20 p-4 sm:p-6 space-y-4 sm:space-y-6 shadow-[var(--shadow-card)] relative overflow-hidden"
          >
            <div className="absolute top-0 right-0 w-20 sm:w-32 h-20 sm:h-32 bg-gradient-to-bl from-primary/10 to-transparent rounded-bl-full" />
            <div className="absolute bottom-0 left-0 w-16 sm:w-24 h-16 sm:h-24 bg-gradient-to-tr from-primary/5 to-transparent rounded-tr-full" />

            {renderStep()}

            {/* Navigation Buttons */}
            <div className="flex justify-between items-center pt-4 sm:pt-6 border-t border-primary/10 relative z-10">
              <motion.div whileHover={{ scale: 1.05 }} whileTap={{ scale: 0.95 }}>
                <Button variant="outline" onClick={() => currentStep === 0 ? navigate('/') : handlePrevious()}
                  className="flex items-center gap-1.5 sm:gap-2 transition-all duration-300 border-2 hover:border-primary/50 hover:bg-primary/5 h-10 sm:h-11 text-sm sm:text-base px-3 sm:px-4 bg-transparent"
                >
                  <ChevronLeft className="h-4 w-4" />
                  <span className="hidden sm:inline">{currentStep === 0 ? "Retour à l'accueil" : "Précédent"}</span>
                  <span className="sm:hidden">Retour</span>
                </Button>
              </motion.div>

              <div className="flex items-center gap-3">
                {currentStep < steps.length - 1 ? (
                  <motion.div whileHover={{ scale: stepCompletion.isComplete ? 1.05 : 1 }} whileTap={{ scale: stepCompletion.isComplete ? 0.95 : 1 }}>
                    <Button onClick={handleNext} disabled={!stepCompletion.isComplete}
                      className={`flex items-center gap-1.5 sm:gap-2 transition-all duration-300 relative overflow-hidden h-10 sm:h-11 text-sm sm:text-base px-3 sm:px-4 ${stepCompletion.isComplete ? "bg-gradient-to-r from-primary to-blue-600 shadow-lg" : ""}`}
                    >
                      {stepCompletion.isComplete && (
                        <motion.div className="absolute inset-0 bg-white/20" initial={{ x: "-100%" }} animate={{ x: "100%" }} transition={{ repeat: Infinity, duration: 2, ease: "linear" }} />
                      )}
                      <span className="relative z-10">
                        <span className="hidden sm:inline">{stepCompletion.isComplete ? "Étape suivante" : "Continuer"}</span>
                        <span className="sm:hidden">{stepCompletion.isComplete ? "Suivant" : "Continuer"}</span>
                      </span>
                      {stepCompletion.isComplete ? (
                        <motion.div animate={{ x: [0, 3, 0] }} transition={{ repeat: Infinity, duration: 1.5 }} className="relative z-10">
                          <ArrowRight className="h-4 w-4" />
                        </motion.div>
                      ) : (
                        <Lock className="h-4 w-4 relative z-10" />
                      )}
                    </Button>
                  </motion.div>
                ) : (
                  <motion.div whileHover={{ scale: stepCompletion.isComplete && !isSubmitting ? 1.05 : 1 }} whileTap={{ scale: stepCompletion.isComplete && !isSubmitting ? 0.95 : 1 }}>
                    <Button onClick={handleSubmit} disabled={!stepCompletion.isComplete || isSubmitting}
                      className={`flex items-center gap-1.5 sm:gap-2 transition-all duration-300 relative overflow-hidden h-10 sm:h-11 text-sm sm:text-base px-3 sm:px-4 ${stepCompletion.isComplete ? "bg-gradient-to-r from-green-600 to-emerald-600 shadow-[var(--shadow-success)]" : ""}`}
                    >
                      {stepCompletion.isComplete && !isSubmitting && (
                        <motion.div className="absolute inset-0 bg-white/20" initial={{ x: "-100%" }} animate={{ x: "100%" }} transition={{ repeat: Infinity, duration: 2, ease: "linear" }} />
                      )}
                      <span className="relative z-10">
                        {isSubmitting ? (
                          <><motion.div animate={{ rotate: 360 }} transition={{ repeat: Infinity, duration: 1, ease: "linear" }} className="h-4 w-4 border-2 border-white border-t-transparent rounded-full inline-block mr-1.5 sm:mr-2" /><span className="hidden sm:inline">Inscription en cours...</span><span className="sm:hidden">Chargement...</span></>
                        ) : stepCompletion.isComplete ? (
                          <><CheckCircle className="h-4 w-4 inline mr-1.5 sm:mr-2" /><span className="hidden sm:inline">Finaliser l'inscription</span><span className="sm:hidden">Finaliser</span></>
                        ) : (
                          <><Lock className="h-4 w-4 inline mr-1.5 sm:mr-2" /><span className="hidden sm:inline">Complétez les champs</span><span className="sm:hidden">Compléter</span></>
                        )}
                      </span>
                    </Button>
                  </motion.div>
                )}
              </div>
            </div>
          </motion.div>
        </motion.div>
      </div>
    </div>
  )
}

export default DetailedInscriptionForm
