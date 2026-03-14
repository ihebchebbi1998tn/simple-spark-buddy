"use client"

import type React from "react"

import { useState, useMemo, useEffect } from "react"
import { useNavigate, useLocation } from "react-router-dom"
import { useRegistrationStorage } from "@/hooks/useRegistrationStorage"
import { useAvailabilityCheck } from "@/hooks/useAvailabilityCheck"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Card, CardContent } from "@/components/ui/card"
import { User, MapPin, Briefcase, Phone, CheckCircle2, ArrowRight, Lock, Loader2, CircleAlert } from "lucide-react"
import { useToast } from "@/hooks/use-toast"
import { motion, AnimatePresence } from "framer-motion"
import { useScrollAnimation } from "@/hooks/useScrollAnimation"
import { EXPERIENCE_LEVELS, POSITIONS, TUNISIAN_CITIES, filterExperienceLevelsByGlobal } from "@/utils/constants"

const CandidateInscriptionForm = () => {
  const { ref, isVisible } = useScrollAnimation()
  const location = useLocation()
  const { data: storedData, updateData } = useRegistrationStorage()
  const [formData, setFormData] = useState({
    posteRecherche: storedData.posteRecherche || "",
    experienceGlobale: storedData.experienceGlobale || "",
    experiencePosteRecherche: storedData.experiencePosteRecherche || "",
    civilite: storedData.civilite || "",
    nom: storedData.nom || "",
    prenom: storedData.prenom || "",
    telephone: storedData.telephone || "",
    villeResidence: storedData.villeResidence || "",
  })
  const navigate = useNavigate()
  const { toast } = useToast()

  // Check phone availability in real-time
  const { phoneExists, isCheckingPhone } = useAvailabilityCheck('', formData.telephone, 600)

  // Get language test data from location state (passed from LanguageTest page) OR localStorage fallback
  const locationState = location.state as {
    fromLanguageTest?: boolean;
    testScore?: any;
    testLanguage?: string;
    scrollTo?: string;
  } | null

  // Fallback to localStorage if navigation state doesn't have test data
  const getTestDataFromLocalStorage = () => {
    try {
      const stored = localStorage.getItem('languageTestResult');
      if (stored) {
        const testResult = JSON.parse(stored);
        return {
          fromLanguageTest: true,
          testLanguage: testResult.language,
          testScore: {
            linguistic: testResult.scores?.language?.percentage || 0,
            softSkills: testResult.scores?.softskills?.percentage || 0,
            jobSkills: testResult.scores?.competences?.percentage || 0,
            overall: testResult.totalScore && testResult.totalQuestions
              ? Math.round((testResult.totalScore / testResult.totalQuestions) * 100)
              : 0
          }
        };
      }
    } catch (e) {
      console.error('Error reading languageTestResult from localStorage:', e);
    }
    return null;
  };

  const localStorageTestData = getTestDataFromLocalStorage();

  // Prioritize navigation state, then stored data, then localStorage
  const testData = {
    fromLanguageTest: locationState?.fromLanguageTest || storedData.fromLanguageTest || localStorageTestData?.fromLanguageTest || false,
    testScore: locationState?.testScore || storedData.testScore || localStorageTestData?.testScore || null,
    testLanguage: locationState?.testLanguage || storedData.testLanguage || localStorageTestData?.testLanguage || null,
  };

  console.log('📊 CandidateInscriptionForm - Test data sources:', {
    fromLocationState: !!locationState?.fromLanguageTest,
    fromStoredData: !!storedData.fromLanguageTest,
    fromLocalStorage: !!localStorageTestData?.fromLanguageTest,
    finalTestData: testData
  });

  const positions = [
    { value: "1", label: "Agent" },
    { value: "2", label: "Team Leader" },
    { value: "3", label: "Responsable Activité" },
    { value: "4", label: "Responsable Qualité" },
    { value: "5", label: "Responsable Service client" },
    { value: "6", label: "Responsable Recrutement" },
    { value: "7", label: "Responsable Plateau" },
    { value: "8", label: "Formateur" },
    { value: "9", label: "Chargé Recrutement" },
  ]

  const experienceLevels = [
    { value: "0", label: "Aucune expérience" },
    { value: "1", label: "0 - 6 mois" },
    { value: "2", label: "6 mois - 12 mois" },
    { value: "3", label: "1 an - 2 ans" },
    { value: "4", label: "2 ans - 3 ans" },
    { value: "5", label: "3 ans - 5 ans" },
    { value: "6", label: "5 ans - 7 ans" },
    { value: "7", label: "Plus de 7 ans" },
  ]

  const tunisianCities = [
    { value: "tunis", label: "Tunis" },
    { value: "ariana", label: "Ariana" },
    { value: "ben_arous", label: "Ben Arous" },
    { value: "manouba", label: "Manouba" },
    { value: "nabeul", label: "Nabeul" },
    { value: "zaghouan", label: "Zaghouan" },
    { value: "bizerte", label: "Bizerte" },
    { value: "beja", label: "Béja" },
    { value: "jendouba", label: "Jendouba" },
    { value: "kef", label: "Le Kef" },
    { value: "siliana", label: "Siliana" },
    { value: "kairouan", label: "Kairouan" },
    { value: "kasserine", label: "Kasserine" },
    { value: "sidi_bouzid", label: "Sidi Bouzid" },
    { value: "sousse", label: "Sousse" },
    { value: "monastir", label: "Monastir" },
    { value: "mahdia", label: "Mahdia" },
    { value: "sfax", label: "Sfax" },
    { value: "gabes", label: "Gabès" },
    { value: "medenine", label: "Médenine" },
    { value: "tataouine", label: "Tataouine" },
    { value: "gafsa", label: "Gafsa" },
    { value: "tozeur", label: "Tozeur" },
    { value: "kebili", label: "Kébili" },
  ]

  // Always show position field when experience is selected
  const shouldShowPositionField = !!formData.experienceGlobale
  const isLowExperience = ["0", "1", "2", "3", "4"].includes(formData.experienceGlobale)
  const isAgentPosition = formData.posteRecherche === "1"
  // Only show experience in position field for high experience AND non-Agent position
  const shouldShowExperiencePoste = !isLowExperience && formData.posteRecherche && !isAgentPosition

  // Filter experience options for position - must be <= global experience (for non-Agent positions)
  const filteredPositionExperience = useMemo(() => {
    if (!formData.experienceGlobale) return [...EXPERIENCE_LEVELS];
    return filterExperienceLevelsByGlobal(formData.experienceGlobale, [...EXPERIENCE_LEVELS]);
  }, [formData.experienceGlobale]);

  // Reset experiencePosteRecherche if it exceeds the new global experience
  useEffect(() => {
    if (formData.experiencePosteRecherche && formData.experienceGlobale) {
      const globalExpInt = parseInt(formData.experienceGlobale, 10);
      const posteExpInt = parseInt(formData.experiencePosteRecherche, 10);
      // If position experience exceeds global experience, reset it
      if (posteExpInt > globalExpInt) {
        setFormData(prev => ({ ...prev, experiencePosteRecherche: "" }));
        updateData({ experiencePosteRecherche: "" });
      }
    }
  }, [formData.experienceGlobale]);
  const handleInputChange = (field: string, value: string) => {
    // Import sanitization utility
    import('@/utils/sanitization').then(({ sanitizeInput, sanitizeEmail, sanitizePhone }) => {
      let sanitizedValue = value;

      // Sanitize based on field type
      if (field === 'email') {
        sanitizedValue = sanitizeEmail(value);
      } else if (field === 'telephone') {
        sanitizedValue = sanitizePhone(value);
      } else if (typeof value === 'string') {
        sanitizedValue = sanitizeInput(value);
      }

      const newFormData = {
        ...formData,
        [field]: sanitizedValue,
      };

      setFormData(newFormData);

      // Save to localStorage
      updateData(newFormData);
    });
  }

  // Calculate completion progress
  const completedFields = useMemo(() => {
    let count = 0
    if (formData.experienceGlobale) count++
    if (shouldShowPositionField && formData.posteRecherche) count++
    if (shouldShowExperiencePoste && formData.experiencePosteRecherche) count++
    if (formData.civilite) count++
    if (formData.nom?.trim()) count++
    if (formData.prenom?.trim()) count++
    if (formData.telephone?.trim()) count++
    if (formData.villeResidence) count++

    console.log('📊 Step 1 - Completion tracking:', {
      experienceGlobale: !!formData.experienceGlobale,
      shouldShowPositionField,
      posteRecherche: !!formData.posteRecherche,
      shouldShowExperiencePoste,
      experiencePosteRecherche: !!formData.experiencePosteRecherche,
      civilite: !!formData.civilite,
      nom: !!formData.nom?.trim(),
      prenom: !!formData.prenom?.trim(),
      telephone: !!formData.telephone?.trim(),
      villeResidence: !!formData.villeResidence,
      completedCount: count
    });

    return count
  }, [formData, shouldShowPositionField, shouldShowExperiencePoste])

  const totalFields = useMemo(() => {
    let total = 6 // experience, civilite, nom, prenom, telephone, ville
    if (shouldShowPositionField) total++ // position field
    if (shouldShowExperiencePoste) total++ // experience in position field
    return total
  }, [shouldShowPositionField, shouldShowExperiencePoste])

  const progressPercentage = (completedFields / totalFields) * 100

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    console.log('📝 Step 1 Form - Attempting submission');
    console.log('📋 Current form data:', formData);

    const requiredFields = ["experienceGlobale", "civilite", "nom", "prenom", "telephone", "villeResidence"]
    if (shouldShowPositionField) requiredFields.push("posteRecherche")
    if (shouldShowExperiencePoste) requiredFields.push("experiencePosteRecherche")

    const missingFields = requiredFields.filter((field) => !formData[field as keyof typeof formData])

    if (missingFields.length > 0) {
      console.log('❌ Step 1 validation failed - Missing fields:', missingFields);
      toast({
        title: "Informations manquantes",
        description: "Veuillez remplir tous les champs obligatoires pour continuer.",
        variant: "destructive",
      })
      return
    }

    // Check if phone already exists
    if (phoneExists) {
      console.log('❌ Step 1 validation failed - Phone already exists');
      toast({
        title: "Numéro déjà utilisé",
        description: "Ce numéro est déjà associé à un compte. Veuillez vous connecter ou utiliser un autre numéro.",
        variant: "destructive",
      })
      return
    }

    console.log('✅ Step 1 validation passed');

    const dataToSave = {
      ...formData,
      // Use selected position or default to Agent if not selected
      posteRecherche: formData.posteRecherche || "1",
    }

    console.log('💾 Step 1 - Saving data to localStorage:', {
      experienceGlobale: dataToSave.experienceGlobale,
      experienceLabel: EXPERIENCE_LEVELS.find(l => l.value === dataToSave.experienceGlobale)?.label,
      isLowExperience,
      posteRecherche: dataToSave.posteRecherche,
      hasAllRequiredFields: !!(dataToSave.experienceGlobale && dataToSave.civilite && dataToSave.nom && dataToSave.prenom && dataToSave.telephone && dataToSave.villeResidence)
    });

    // Save step 1 data and test data to localStorage
    // Use testData which prioritizes navigation state, stored data, then localStorage
    const completeData = {
      ...dataToSave,
      ...(testData.fromLanguageTest && {
        fromLanguageTest: testData.fromLanguageTest,
        testScore: testData.testScore,
        testLanguage: testData.testLanguage,
      })
    };

    console.log('💾 Saving to localStorage:', {
      ...completeData,
      languageTestIncluded: !!testData.fromLanguageTest,
      testLanguage: testData.testLanguage,
      testScore: testData.testScore
    });

    updateData(completeData);

    console.log('🚀 Navigating to /processing with state:', {
      hasStep1Data: true,
      hasLanguageTest: !!testData.fromLanguageTest,
      testLanguage: testData.testLanguage
    });

    // Navigate to detailed inscription form (processing first)
    navigate("/processing", {
      state: {
        step1Data: completeData, // Pass the form data so ProcessingInscription doesn't redirect
        fromLanguageTest: testData.fromLanguageTest,
        testScore: testData.testScore,
        testLanguage: testData.testLanguage,
      }
    })
  }

  return (
    <motion.section
      ref={ref}
      id="candidate-form"
      className="py-16 px-4 relative overflow-hidden"
      initial={{ opacity: 0 }}
      animate={isVisible ? { opacity: 1 } : { opacity: 0 }}
      transition={{ duration: 0.8 }}
    >
      <div className="absolute inset-0 overflow-hidden pointer-events-none">
        <div className="absolute top-20 left-10 w-72 h-72 bg-primary/5 rounded-full blur-3xl" />
        <div className="absolute bottom-20 right-10 w-96 h-96 bg-blue-500/5 rounded-full blur-3xl" />
      </div>

      <div className="max-w-4xl mx-auto relative z-10">
        <motion.div
          className="text-center mb-8"
          initial={{ opacity: 0, y: 50 }}
          animate={isVisible ? { opacity: 1, y: 0 } : { opacity: 0, y: 50 }}
          transition={{ duration: 0.8, delay: 0.2 }}
        >
          <h2 className="text-3xl sm:text-4xl lg:text-5xl font-bold text-foreground mb-4 leading-tight">
            Prêt(e) à décrocher{" "}
            <span className="text-primary bg-gradient-to-r from-primary to-blue-600 bg-clip-text text-transparent">
              un job
            </span>{" "}
            ?
          </h2>

          <p className="text-base sm:text-lg lg:text-xl text-muted-foreground max-w-2xl mx-auto px-4 mb-6 leading-relaxed">
            Sans CV ni lettre de motivation, inscrivez-vous en <span className="text-primary font-semibold">2 minutes</span> et accédez aux meilleures opportunités des centres d'appels.
          </p>
        </motion.div>

        <motion.div
          initial={{ opacity: 0, y: 50 }}
          animate={isVisible ? { opacity: 1, y: 0 } : { opacity: 0, y: 50 }}
          transition={{ duration: 0.8, delay: 0.4 }}
        >
          <Card className="bg-gradient-to-br from-card via-card to-primary/5 backdrop-blur-sm border-2 relative shadow-[var(--shadow-card)]">
            {/* Animated progress bar with gradient */}
            <motion.div
              className="absolute top-0 left-0 h-1 bg-gradient-to-r from-primary via-blue-500 to-primary pointer-events-none"
              initial={{ width: 0 }}
              animate={{ width: `${progressPercentage}%` }}
              transition={{ duration: 0.5 }}
              style={{
                boxShadow: completedFields === totalFields ? "var(--shadow-glow)" : "none",
              }}
            />

            {/* Decorative corner accent */}
            <div className="absolute top-0 right-0 w-32 h-32 bg-gradient-to-bl from-primary/10 to-transparent rounded-bl-full pointer-events-none" />

            <CardContent className="space-y-6 px-4 sm:px-6 pb-6 pt-6">
              {/* Enhanced progress indicator with circular progress */}
              <AnimatePresence>
                {completedFields > 0 && (
                  <motion.div
                    className="flex items-center justify-between mb-6 p-4 rounded-lg bg-gradient-to-r from-primary/5 to-transparent border border-primary/10"
                    initial={{ opacity: 0, y: -20 }}
                    animate={{ opacity: 1, y: 0 }}
                    exit={{ opacity: 0, y: -20 }}
                    transition={{ duration: 0.4 }}
                  >
                    <div className="flex items-center gap-3">
                      <motion.div
                        className="relative w-12 h-12"
                        animate={completedFields === totalFields ? { rotate: [0, 360] } : {}}
                        transition={{ duration: 0.8, type: "spring" }}
                      >
                        {/* Circular progress */}
                        <svg className="w-12 h-12 transform -rotate-90">
                          <circle
                            cx="24"
                            cy="24"
                            r="20"
                            stroke="currentColor"
                            strokeWidth="4"
                            fill="none"
                            className="text-muted/20"
                          />
                          <motion.circle
                            cx="24"
                            cy="24"
                            r="20"
                            stroke="currentColor"
                            strokeWidth="4"
                            fill="none"
                            className="text-primary"
                            strokeLinecap="round"
                            initial={{ strokeDasharray: "0 125.6" }}
                            animate={{ strokeDasharray: `${progressPercentage * 1.256} 125.6` }}
                            transition={{ duration: 0.5 }}
                          />
                        </svg>
                        <div className="absolute inset-0 flex items-center justify-center">
                          <motion.span
                            key={completedFields}
                            initial={{ scale: 0.5, opacity: 0 }}
                            animate={{ scale: 1, opacity: 1 }}
                            className="text-xs font-bold text-primary"
                          >
                            {completedFields}
                          </motion.span>
                        </div>
                      </motion.div>
                      <div>
                        <p className="text-xs sm:text-sm font-semibold text-foreground">
                          {completedFields} / {totalFields} champs complétés
                        </p>
                        <p className="text-[10px] sm:text-xs text-muted-foreground">
                          {totalFields - completedFields} restant{totalFields - completedFields > 1 ? "s" : ""}
                        </p>
                      </div>
                    </div>
                    <AnimatePresence>
                      {completedFields === totalFields && (
                        <motion.div
                          initial={{ opacity: 0, scale: 0.8, x: 20 }}
                          animate={{ opacity: 1, scale: 1, x: 0 }}
                          exit={{ opacity: 0, scale: 0.8 }}
                          className="flex items-center gap-2 px-4 py-2 rounded-full bg-gradient-to-r from-green-500 to-emerald-500 text-white shadow-[var(--shadow-success)]"
                        >
                          <motion.span
                            animate={{ rotate: [0, 10, -10, 0] }}
                            transition={{ duration: 0.5, repeat: Number.POSITIVE_INFINITY, repeatDelay: 2 }}
                          >
                            🎉
                          </motion.span>
                          <span className="text-xs sm:text-sm font-semibold">Complet !</span>
                        </motion.div>
                      )}
                    </AnimatePresence>
                  </motion.div>
                )}
              </AnimatePresence>

              <form onSubmit={handleSubmit} className="space-y-6">
                {/* Expérience globale */}
                <motion.div
                  className="space-y-2"
                  initial={{ opacity: 0, x: -20 }}
                  animate={{ opacity: 1, x: 0 }}
                  transition={{ delay: 0.1 }}
                >
                  <Label
                    htmlFor="experienceGlobale"
                    className="flex items-center gap-2 text-foreground font-bold text-base sm:text-lg"
                  >
                    <Briefcase size={16} className="text-primary sm:w-5 sm:h-5" />
                    <span className="text-base sm:text-lg">
                      Parlez-nous de votre expérience dans les centres d'appels
                    </span>
                    <span className="text-destructive">*</span>
                    {formData.experienceGlobale && (
                      <motion.span initial={{ scale: 0 }} animate={{ scale: 1 }} className="ml-auto">
                        <CheckCircle2 size={14} className="text-primary sm:w-4 sm:h-4" />
                      </motion.span>
                    )}
                  </Label>
                  <Select
                    value={formData.experienceGlobale}
                    onValueChange={(value) => {
                      console.log('✅ Experience selected:', value, EXPERIENCE_LEVELS.find(l => l.value === value)?.label);
                      const isLowExp = ["0", "1", "2", "3", "4"].includes(value);
                      const newFormData = {
                        ...formData,
                        experienceGlobale: value,
                        // Reset to Agent for low experience, clear for high experience if not already set
                        posteRecherche: isLowExp ? "1" : (formData.posteRecherche === "1" ? "" : formData.posteRecherche),
                        experiencePosteRecherche: isLowExp ? "" : formData.experiencePosteRecherche,
                      };
                      setFormData(newFormData);
                      updateData(newFormData);
                    }}
                  >
                    <SelectTrigger
                      className={`h-10 sm:h-12 border-2 transition-all duration-300 input-glow text-xs sm:text-sm ${formData.experienceGlobale
                        ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md"
                        : "hover:border-primary/40 hover:bg-primary/5"
                        }`}
                    >
                      <SelectValue placeholder="Commencez par partager votre expérience" />
                    </SelectTrigger>
                    <SelectContent
                      sideOffset={8}
                      className="bg-white dark:bg-gray-800 border-2 shadow-2xl"
                      align="start"
                    >
                      {EXPERIENCE_LEVELS.map((level) => (
                        <SelectItem
                          key={level.value}
                          value={level.value}
                        >
                          {level.label}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                  {formData.experienceGlobale && (
                    <motion.p
                      initial={{ opacity: 0, y: -10 }}
                      animate={{ opacity: 1, y: 0 }}
                      className="text-xs sm:text-sm text-primary font-medium"
                    >
                      {isLowExperience
                        ? "Parfait pour débuter ! Voyons la suite."
                        : "Belle expérience ! Continuons avec le poste recherché."}
                    </motion.p>
                  )}
                  {!formData.experienceGlobale && (
                    <p className="text-xs sm:text-sm text-muted-foreground">
                      Commencez par nous parler de votre expérience
                    </p>
                  )}
                </motion.div>

                <AnimatePresence>
                  {shouldShowPositionField && (
                    <motion.div
                      className="space-y-2"
                      initial={{ opacity: 0, height: 0, y: -20 }}
                      animate={{ opacity: 1, height: "auto", y: 0 }}
                      exit={{ opacity: 0, height: 0, y: -20 }}
                      transition={{ duration: 0.6, ease: "easeOut" }}
                    >
                      <Label
                        htmlFor="posteRecherche"
                        className="flex items-center gap-2 text-foreground font-medium text-xs sm:text-sm"
                      >
                        <Briefcase size={14} className="text-primary sm:w-4 sm:h-4" />
                        <span className="text-xs sm:text-sm">Quel poste vous intéresse ?</span>
                        <span className="text-destructive">*</span>
                        {formData.posteRecherche && (
                          <motion.span initial={{ scale: 0 }} animate={{ scale: 1 }} className="ml-auto">
                            <CheckCircle2 size={14} className="text-primary sm:w-4 sm:h-4" />
                          </motion.span>
                        )}
                      </Label>

                      {isLowExperience ? (
                        // For low experience: show Agent as auto-selected (read-only display)
                        <div className="h-10 sm:h-12 px-3 flex items-center border-2 border-green-500/50 bg-gradient-to-br from-green-500/10 to-emerald-500/5 rounded-md shadow-md">
                          <CheckCircle2 size={16} className="text-green-500 mr-2" />
                          <span className="text-sm font-medium text-foreground">Agent</span>
                          <span className="ml-auto text-xs text-green-600 dark:text-green-400">Sélectionné automatiquement</span>
                        </div>
                      ) : (
                        // For high experience: show full dropdown
                        <Select
                          value={formData.posteRecherche}
                          onValueChange={(value) => {
                            handleInputChange("posteRecherche", value)
                            if (value === "1") {
                              handleInputChange("experiencePosteRecherche", "")
                            }
                          }}
                        >
                          <SelectTrigger
                            className={`h-10 sm:h-12 border-2 transition-all duration-300 input-glow text-xs sm:text-sm ${formData.posteRecherche
                              ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md"
                              : "hover:border-primary/40 hover:bg-primary/5"
                              }`}
                          >
                            <SelectValue placeholder="Choisissez le poste qui vous correspond" />
                          </SelectTrigger>
                          <SelectContent
                            sideOffset={8}
                            className="bg-white dark:bg-gray-800 border-2 shadow-2xl"
                            align="start"
                          >
                            {POSITIONS.map((position) => (
                              <SelectItem key={position.value} value={position.value}>
                                {position.label}
                              </SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                      )}

                      {formData.posteRecherche && !isLowExperience && (
                        <motion.p
                          initial={{ opacity: 0, y: -10 }}
                          animate={{ opacity: 1, y: 0 }}
                          className="text-xs sm:text-sm text-primary font-medium"
                        >
                          Excellent choix ! Continuons.
                        </motion.p>
                      )}
                    </motion.div>
                  )}
                </AnimatePresence>

                <AnimatePresence>
                  {shouldShowExperiencePoste && (
                    <motion.div
                      className="space-y-2"
                      initial={{ opacity: 0, height: 0, y: -20 }}
                      animate={{ opacity: 1, height: "auto", y: 0 }}
                      exit={{ opacity: 0, height: 0, y: -20 }}
                      transition={{ duration: 0.6, ease: "easeOut" }}
                    >
                      <Label className="flex items-center gap-2 text-foreground font-bold text-base sm:text-lg">
                        <Briefcase size={16} className="text-primary sm:w-5 sm:h-5" />
                        <span className="text-base sm:text-lg">
                          Parlez-nous de votre expérience dans le poste recherché
                        </span>
                        <span className="text-destructive">*</span>
                        {formData.experiencePosteRecherche && (
                          <motion.span initial={{ scale: 0 }} animate={{ scale: 1 }} className="ml-auto">
                            <CheckCircle2 size={14} className="text-primary sm:w-4 sm:h-4" />
                          </motion.span>
                        )}
                      </Label>
                      <Select
                        value={formData.experiencePosteRecherche}
                        onValueChange={(value) => handleInputChange("experiencePosteRecherche", value)}
                      >
                        <SelectTrigger
                          className={`h-10 sm:h-12 border-2 transition-all duration-300 input-glow text-xs sm:text-sm ${formData.experiencePosteRecherche
                            ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md"
                            : "hover:border-primary/40 hover:bg-primary/5"
                            }`}
                        >
                          <SelectValue placeholder="Depuis combien de temps ?" />
                        </SelectTrigger>
                        <SelectContent
                          sideOffset={8}
                          className="bg-white dark:bg-gray-800 border-2 shadow-2xl"
                          align="start"
                        >
                          {filteredPositionExperience.map((level) => (
                            <SelectItem key={level.value} value={level.value}>
                              {level.label}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                      {formData.experiencePosteRecherche && (
                        <motion.p
                          initial={{ opacity: 0, y: -10 }}
                          animate={{ opacity: 1, y: 0 }}
                          className="text-xs sm:text-sm text-primary font-medium"
                        >
                          Excellente expérience ! Continuons 🚀
                        </motion.p>
                      )}
                    </motion.div>
                  )}
                </AnimatePresence>

                {/* Civilité */}
                <motion.div
                  className="space-y-2"
                  initial={{ opacity: 0, x: -20 }}
                  animate={{ opacity: 1, x: 0 }}
                  transition={{ delay: 0.3 }}
                >
                  <Label className="flex items-center gap-2 text-foreground font-bold text-base sm:text-lg">
                    <User size={16} className="text-primary sm:w-5 sm:h-5" />
                    <span className="text-base sm:text-lg">Vous êtes</span>
                    <span className="text-destructive">*</span>
                    {formData.civilite && (
                      <motion.span initial={{ scale: 0 }} animate={{ scale: 1 }} className="ml-auto">
                        <CheckCircle2 size={14} className="text-primary sm:w-4 sm:h-4" />
                      </motion.span>
                    )}
                  </Label>
                  <div className="flex gap-4">
                    <motion.div className="flex-1" whileHover={{ scale: 1.02 }} whileTap={{ scale: 0.98 }}>
                      <Button
                        type="button"
                        variant={formData.civilite === "madame" ? "default" : "outline"}
                        className="w-full h-10 sm:h-12 font-medium flex items-center justify-center gap-2 text-xs sm:text-sm"
                        onClick={() => handleInputChange("civilite", "madame")}
                      >
                        <span className="text-lg sm:text-xl">👩</span>
                        Madame
                      </Button>
                    </motion.div>
                    <motion.div className="flex-1" whileHover={{ scale: 1.02 }} whileTap={{ scale: 0.98 }}>
                      <Button
                        type="button"
                        variant={formData.civilite === "monsieur" ? "default" : "outline"}
                        className="w-full h-10 sm:h-12 font-medium flex items-center justify-center gap-2 text-xs sm:text-sm"
                        onClick={() => handleInputChange("civilite", "monsieur")}
                      >
                        <span className="text-lg sm:text-xl">👨</span>
                        Monsieur
                      </Button>
                    </motion.div>
                  </div>
                  {!formData.civilite && (
                    <p className="text-xs sm:text-sm text-muted-foreground text-center">Faisons connaissance</p>
                  )}
                </motion.div>

                {/* Progressive disclosure: Show Nom and Téléphone after civility selection */}
                <AnimatePresence>
                  {formData.civilite && (
                    <motion.div
                      className="space-y-6"
                      initial={{ opacity: 0, height: 0 }}
                      animate={{ opacity: 1, height: "auto" }}
                      exit={{ opacity: 0, height: 0 }}
                      transition={{ duration: 0.6, ease: "easeOut" }}
                    >
                      <motion.div
                        initial={{ y: 20 }}
                        animate={{ y: 0 }}
                        transition={{ delay: 0.2 }}
                        className="text-center p-4 bg-gradient-to-r from-primary/5 via-primary/10 to-primary/5 rounded-lg border border-primary/20"
                      >
                        <motion.p
                          className="text-xs sm:text-sm font-semibold text-primary"
                          initial={{ scale: 0.9 }}
                          animate={{ scale: 1 }}
                          transition={{ type: "spring", stiffness: 300 }}
                        >
                          ✨ Parfait {formData.civilite === "madame" ? "Madame" : "Monsieur"} ! Continuons avec vos
                          coordonnées
                        </motion.p>
                      </motion.div>

                      {/* Nom and Prénom Row */}
                      <div className="grid sm:grid-cols-1 lg:grid-cols-2 gap-6">
                        {/* Nom */}
                        <motion.div
                          className="space-y-2"
                          initial={{ opacity: 0, x: -20 }}
                          animate={{ opacity: 1, x: 0 }}
                          transition={{ delay: 0.3 }}
                        >
                          <Label
                            htmlFor="nom"
                            className="flex items-center gap-2 text-foreground font-bold text-base sm:text-lg"
                          >
                            <User size={16} className="text-primary sm:w-5 sm:h-5" />
                            <span className="text-base sm:text-lg">Nom</span>
                            <span className="text-destructive">*</span>
                            {formData.nom && (
                              <motion.span
                                initial={{ scale: 0 }}
                                animate={{ scale: 1 }}
                                transition={{ type: "spring", stiffness: 500 }}
                                className="ml-auto"
                              >
                                <CheckCircle2 size={14} className="text-primary sm:w-4 sm:h-4" />
                              </motion.span>
                            )}
                          </Label>
                          <Input
                            id="nom"
                            type="text"
                            placeholder="Votre nom de famille"
                            value={formData.nom}
                            onChange={(e) => handleInputChange("nom", e.target.value)}
                            className={`h-10 sm:h-12 border-2 transition-all duration-300 input-glow text-xs sm:text-sm ${formData.nom
                              ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md"
                              : "hover:border-primary/40 hover:bg-primary/5"
                              }`}
                            required
                            autoFocus
                          />
                        </motion.div>

                        {/* Prénom */}
                        <motion.div
                          className="space-y-2"
                          initial={{ opacity: 0, x: 20 }}
                          animate={{ opacity: 1, x: 0 }}
                          transition={{ delay: 0.35 }}
                        >
                          <Label
                            htmlFor="prenom"
                            className="flex items-center gap-2 text-foreground font-bold text-base sm:text-lg"
                          >
                            <User size={16} className="text-primary sm:w-5 sm:h-5" />
                            <span className="text-base sm:text-lg">Prénom</span>
                            <span className="text-destructive">*</span>
                            {formData.prenom && (
                              <motion.span
                                initial={{ scale: 0 }}
                                animate={{ scale: 1 }}
                                transition={{ type: "spring", stiffness: 500 }}
                                className="ml-auto"
                              >
                                <CheckCircle2 size={14} className="text-primary sm:w-4 sm:h-4" />
                              </motion.span>
                            )}
                          </Label>
                          <Input
                            id="prenom"
                            type="text"
                            placeholder="Votre prénom"
                            value={formData.prenom}
                            onChange={(e) => handleInputChange("prenom", e.target.value)}
                            className={`h-10 sm:h-12 border-2 transition-all duration-300 input-glow text-xs sm:text-sm ${formData.prenom
                              ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md"
                              : "hover:border-primary/40 hover:bg-primary/5"
                              }`}
                            required
                          />
                        </motion.div>
                      </div>

                      <AnimatePresence>
                        {formData.nom && formData.prenom && (
                          <motion.p
                            initial={{ opacity: 0, y: -10 }}
                            animate={{ opacity: 1, y: 0 }}
                            exit={{ opacity: 0 }}
                            className="text-xs sm:text-sm text-primary font-medium text-center"
                          >
                            Ravi de vous connaître {formData.prenom} ! 👋
                          </motion.p>
                        )}
                      </AnimatePresence>

                      {/* Téléphone and Ville Row */}
                      <div className="grid sm:grid-cols-1 lg:grid-cols-2 gap-6">
                        {/* Téléphone */}
                        <motion.div
                          className="space-y-2"
                          initial={{ opacity: 0, x: -20 }}
                          animate={{ opacity: 1, x: 0 }}
                          transition={{ delay: 0.4 }}
                        >
                          <Label
                            htmlFor="telephone"
                            className="flex items-center gap-2 text-foreground font-bold text-base sm:text-lg"
                          >
                            <Phone size={16} className="text-primary sm:w-5 sm:h-5" />
                            <span className="text-base sm:text-lg">Numéro de Téléphone</span>
                            <span className="text-destructive">*</span>
                            {formData.telephone && formData.telephone.replace(/\D/g, '').length === 8 && phoneExists === false && !isCheckingPhone && (
                              <motion.span
                                initial={{ scale: 0 }}
                                animate={{ scale: 1 }}
                                transition={{ type: "spring", stiffness: 500 }}
                                className="ml-auto"
                              >
                                <CheckCircle2 size={14} className="text-primary sm:w-4 sm:h-4" />
                              </motion.span>
                            )}
                          </Label>
                          <div className="relative">
                            <Input
                              id="telephone"
                              type="tel"
                              placeholder="20 123 456 (8 chiffres)"
                              value={formData.telephone}
                              onChange={(e) => handleInputChange("telephone", e.target.value)}
                              className={`h-10 sm:h-12 border-2 transition-all duration-300 input-glow text-xs sm:text-sm ${phoneExists
                                ? "border-destructive/50 bg-destructive/5"
                                : formData.telephone && formData.telephone.replace(/\D/g, '').length === 8 && phoneExists === false
                                  ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md"
                                  : formData.telephone
                                    ? "border-destructive/50 hover:border-destructive/40"
                                    : "hover:border-primary/40 hover:bg-primary/5"
                                }`}
                              required
                            />
                            {formData.telephone && (
                              <div className="absolute right-3 top-1/2 -translate-y-1/2">
                                {isCheckingPhone ? (
                                  <Loader2 className="h-4 w-4 text-muted-foreground animate-spin" />
                                ) : phoneExists ? (
                                  <CircleAlert className="h-4 w-4 text-destructive" />
                                ) : formData.telephone.replace(/\D/g, '').length === 8 && phoneExists === false ? (
                                  <CheckCircle2 className="h-4 w-4 text-primary" />
                                ) : null}
                              </div>
                            )}
                          </div>
                          <AnimatePresence mode="wait">
                            {phoneExists && (
                              <motion.p
                                key="phone-exists"
                                initial={{ opacity: 0, y: -10 }}
                                animate={{ opacity: 1, y: 0 }}
                                exit={{ opacity: 0, y: -10 }}
                                className="text-xs text-destructive flex items-center gap-1"
                              >
                                <CircleAlert className="w-3 h-3" />
                                Ce numéro est déjà utilisé. Veuillez vous connecter ou utiliser un autre numéro.
                              </motion.p>
                            )}
                            {!phoneExists && formData.telephone && formData.telephone.replace(/\D/g, '').length !== 8 && (
                              <motion.p
                                key="phone-invalid"
                                initial={{ opacity: 0, y: -10 }}
                                animate={{ opacity: 1, y: 0 }}
                                exit={{ opacity: 0, y: -10 }}
                                className="text-xs text-destructive"
                              >
                                Le numéro doit contenir exactement 8 chiffres
                              </motion.p>
                            )}
                            {isCheckingPhone && (
                              <motion.p
                                key="phone-checking"
                                initial={{ opacity: 0, y: -10 }}
                                animate={{ opacity: 1, y: 0 }}
                                exit={{ opacity: 0, y: -10 }}
                                className="text-xs text-muted-foreground flex items-center gap-1"
                              >
                                <Loader2 className="w-3 h-3 animate-spin" />
                                Vérification en cours...
                              </motion.p>
                            )}
                            {formData.telephone && formData.telephone.replace(/\D/g, '').length === 8 && phoneExists === false && !isCheckingPhone && (
                              <motion.p
                                key="phone-available"
                                initial={{ opacity: 0, y: -10 }}
                                animate={{ opacity: 1, y: 0 }}
                                exit={{ opacity: 0, y: -10 }}
                                className="text-xs text-primary font-medium"
                              >
                                ✓ Numéro disponible
                              </motion.p>
                            )}
                          </AnimatePresence>
                        </motion.div>

                        {/* Ville de résidence */}
                        <motion.div
                          className="space-y-2"
                          initial={{ opacity: 0, x: 20 }}
                          animate={{ opacity: 1, x: 0 }}
                          transition={{ delay: 0.45 }}
                        >
                          <Label
                            htmlFor="villeResidence"
                            className="flex items-center gap-2 text-foreground font-bold text-base sm:text-lg"
                          >
                            <MapPin size={16} className="text-primary sm:w-5 sm:h-5" />
                            <span className="text-base sm:text-lg">Ville de résidence</span>
                            <span className="text-destructive">*</span>
                            {formData.villeResidence && (
                              <motion.span
                                initial={{ scale: 0 }}
                                animate={{ scale: 1 }}
                                transition={{ type: "spring", stiffness: 500 }}
                                className="ml-auto"
                              >
                                <CheckCircle2 size={14} className="text-primary sm:w-4 sm:h-4" />
                              </motion.span>
                            )}
                          </Label>
                          <Select
                            value={formData.villeResidence}
                            onValueChange={(value) => handleInputChange("villeResidence", value)}
                          >
                            <SelectTrigger
                              className={`h-10 sm:h-12 border-2 transition-all duration-300 input-glow text-xs sm:text-sm ${formData.villeResidence
                                ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md"
                                : "hover:border-primary/40 hover:bg-primary/5"
                                }`}
                            >
                              <SelectValue placeholder="Sélectionnez votre ville" />
                            </SelectTrigger>
                            <SelectContent
                              sideOffset={8}
                              className="bg-white dark:bg-gray-800 border-2 shadow-2xl max-h-[400px]"
                              align="start"
                            >
                              {TUNISIAN_CITIES.map((city) => (
                                <SelectItem key={city.value} value={city.value}>
                                  {city.label}
                                </SelectItem>
                              ))}
                            </SelectContent>
                          </Select>
                        </motion.div>
                      </div>

                      <AnimatePresence>
                        {formData.telephone && formData.villeResidence && (
                          <motion.p
                            initial={{ opacity: 0, y: -10 }}
                            animate={{ opacity: 1, y: 0 }}
                            exit={{ opacity: 0 }}
                            className="text-xs sm:text-sm text-primary font-medium text-center"
                          >
                            Parfait ! Vous serez contacté(e) bientôt 📞
                          </motion.p>
                        )}
                      </AnimatePresence>
                    </motion.div>
                  )}
                </AnimatePresence>

                {/* Enhanced Submit Button */}
                <motion.div
                  className="pt-6 border-t border-primary/10"
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.7 }}
                >
                  <motion.div
                    whileHover={{ scale: completedFields === totalFields ? 1.02 : 1 }}
                    whileTap={{ scale: completedFields === totalFields ? 0.98 : 1 }}
                  >
                    <Button
                      type="submit"
                      size="lg"
                      className={`w-full h-12 sm:h-14 font-semibold text-sm sm:text-base lg:text-lg group relative overflow-hidden ${completedFields === totalFields ? "bg-gradient-to-r from-primary to-blue-600 shadow-lg" : ""
                        }`}
                      disabled={completedFields < totalFields}
                    >
                      {completedFields === totalFields ? (
                        <>
                          Détaillez votre besoin
                          <motion.div
                            animate={{ x: [0, 5, 0] }}
                            transition={{ repeat: Number.POSITIVE_INFINITY, duration: 1.5 }}
                          >
                            <ArrowRight size={20} />
                          </motion.div>
                        </>
                      ) : (
                        <>
                          <Lock className="w-4 h-4 sm:w-5 sm:h-5" />
                          <span className="text-xs sm:text-sm lg:text-base">
                            Complétez les {totalFields - completedFields} champs restants
                          </span>
                        </>
                      )}
                    </Button>
                  </motion.div>
                  {completedFields === totalFields && (
                    <motion.p
                      initial={{ opacity: 0, y: 10 }}
                      animate={{ opacity: 1, y: 0 }}
                      className="text-center text-xs sm:text-sm font-medium mt-4 px-4 py-2 rounded-lg bg-gradient-to-r from-primary/10 to-transparent"
                    >
                      <span className="gradient-text-animated">
                        ✨ Vous êtes sur le bon chemin ! Continuons ensemble 🚀
                      </span>
                    </motion.p>
                  )}
                </motion.div>
              </form>
            </CardContent>
          </Card>
        </motion.div>
      </div>
    </motion.section>
  )
}

export default CandidateInscriptionForm
