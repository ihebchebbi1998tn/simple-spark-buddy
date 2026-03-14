"use client"

import type React from "react"

import { useState, useEffect, useRef } from "react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import { Progress } from "@/components/ui/progress"
import { motion, AnimatePresence } from "framer-motion"
import {
  Clock,
  AlertTriangle,
  CheckCircle2,
  XCircle,
  Languages,
  ArrowRight,
  Sparkles,
  ArrowLeft,
  User,
  Briefcase,
  Phone,
  MapPin,
  Lock,
  Check,
} from "lucide-react"
import { useToast } from "@/hooks/use-toast"
import { Link, useNavigate, useLocation } from "react-router-dom"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import ParticleBackground from "@/components/shared/ParticleBackground"
import { LANGUAGES } from "@/utils/constants"
import { authService } from "@/services/authService"
import { candidateService } from "@/services/candidateService"

import { questionsByLanguage, languageOptions, type Language } from "@/utils/testQuestions"

type TestStep = "intro" | "warning" | "test" | "registration" | "results"

// Questions are now loaded dynamically based on selected language

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

const LanguageTest = () => {
  // Force scroll to top on mount (after render)
  useEffect(() => {
    setTimeout(() => {
      window.scrollTo({ top: 0, left: 0, behavior: 'auto' });
    }, 0);
  }, []);
  const location = useLocation()
  const navigate = useNavigate()

  // Check if user just completed registration from language test
  const showResultsFromRegistration = location.state?.showResults
  const savedTestScore = location.state?.testScore
  const savedLanguage = location.state?.testLanguage
  const savedTotalQuestions = location.state?.totalQuestions
  const userRegistered = location.state?.userRegistered

  // Also try to get data from localStorage if not in state
  const getTestDataFromStorage = () => {
    try {
      const stored = localStorage.getItem('languageTestResult');
      if (stored) {
        const testResult = JSON.parse(stored);
        return {
          score: testResult.totalScore,
          totalQuestions: testResult.totalQuestions,
          language: testResult.language,
          overallPercentage: testResult.scores?.language?.percentage !== undefined
            ? Math.round((testResult.totalScore / testResult.totalQuestions) * 100)
            : null
        };
      }
    } catch (e) {
      console.error('Error reading test data from storage:', e);
    }
    return null;
  };

  const storageData = getTestDataFromStorage();

  // Handle savedScore - it can be a number (raw score) or an object with detailed scores
  const getSavedScoreValue = () => {
    if (savedTestScore === null || savedTestScore === undefined) {
      return storageData?.score ?? 0;
    }
    // If it's an object with overall percentage, return raw score calculated from it
    if (typeof savedTestScore === 'object' && savedTestScore.overall !== undefined) {
      // savedTestScore.overall is already a percentage (0-100)
      // We need to store it as-is for display, but also handle totalQuestions
      return savedTestScore.overall; // This is already a percentage
    }
    return savedTestScore;
  };

  // Get overall percentage directly if available
  const getSavedOverallPercentage = () => {
    if (savedTestScore && typeof savedTestScore === 'object' && savedTestScore.overall !== undefined) {
      return savedTestScore.overall;
    }
    return null;
  };

  const savedOverallPercentage = getSavedOverallPercentage();

  const [step, setStep] = useState<TestStep>(showResultsFromRegistration ? "results" : "intro")
  const [selectedLanguage, setSelectedLanguage] = useState<Language>(savedLanguage || storageData?.language || "francais")
  const [currentQuestion, setCurrentQuestion] = useState(0)
  const [answers, setAnswers] = useState<string[]>([])
  const [timeLeft, setTimeLeft] = useState(300) // 5 minutes in seconds
  const [isTimerActive, setIsTimerActive] = useState(false)
  const [score, setScore] = useState(getSavedScoreValue())
  const [totalQuestions, setTotalQuestions] = useState(savedTotalQuestions ?? storageData?.totalQuestions ?? 22)
  const [categoryScores, setCategoryScores] = useState<{
    language: { correct: number; total: number; percentage: number }
    softskills: { correct: number; total: number; percentage: number }
    competences: { correct: number; total: number; percentage: number }
  } | null>(null)
  const [showConfetti, setShowConfetti] = useState(false)
  const [questionDirection, setQuestionDirection] = useState(1)
  const [registrationData, setRegistrationData] = useState({
    experienceGlobale: "",
    posteRecherche: "",
    civilite: "",
    nom: "",
    telephone: "",
    villeResidence: "",
  })
  const cardRef = useRef<HTMLDivElement>(null)
  const { toast } = useToast()

  // Authentication state
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  const [completedTests, setCompletedTests] = useState<Record<string, { completed: boolean; score?: number }>>({})
  const [loading, setLoading] = useState(true)

  // Check authentication and load completed tests on mount
  useEffect(() => {
    const checkAuthAndLoadTests = async () => {
      const authenticated = authService.isAuthenticated()
      setIsAuthenticated(authenticated)

      if (authenticated) {
        try {
          const response = await candidateService.getProfile()
          if (response.success && response.data.testScores) {
            const testScores = response.data.testScores

            // Map completed tests with language names - check both correct and legacy field names
            const completed: Record<string, { completed: boolean; score?: number }> = {
              francais: {
                completed: !!(testScores.french_test_completed),
                score: typeof testScores.french_overall_score === 'number' ? testScores.french_overall_score : undefined
              },
              english: {
                completed: !!(testScores.english_test_completed),
                score: typeof testScores.english_overall_score === 'number' ? testScores.english_overall_score : undefined
              },
              deutsch: {
                completed: !!(testScores.german_test_completed || testScores.deutsch_test_completed),
                score: typeof testScores.german_overall_score === 'number' ? testScores.german_overall_score : typeof testScores.deutsch_overall_score === 'number' ? testScores.deutsch_overall_score : undefined
              },
              italiano: {
                completed: !!(testScores.italian_test_completed || testScores.italiano_test_completed),
                score: typeof testScores.italian_overall_score === 'number' ? testScores.italian_overall_score : typeof testScores.italiano_overall_score === 'number' ? testScores.italiano_overall_score : undefined
              },
              espanol: {
                completed: !!(testScores.spanish_test_completed || testScores.espanol_test_completed),
                score: typeof testScores.spanish_overall_score === 'number' ? testScores.spanish_overall_score : typeof testScores.espanol_overall_score === 'number' ? testScores.espanol_overall_score : undefined
              }
            }

            setCompletedTests(completed)
          }
        } catch (error) {
          console.error('Error loading test scores:', error)
        }
      }

      setLoading(false)
    }

    checkAuthAndLoadTests()
  }, [])

  // Show welcome message when user returns after registration/login
  useEffect(() => {
    if (userRegistered) {
      toast({
        title: "Félicitations ! 🎊",
        description: "Votre inscription est complète. Découvrez vos résultats ci-dessous.",
        duration: 5000,
      })
    } else if (location.state?.userLoggedIn) {
      toast({
        title: "Content de vous revoir ! 👋",
        description: "Voici vos résultats de test.",
        duration: 4000,
      })
    }
  }, [userRegistered, toast])

  // Timer effect
  useEffect(() => {
    if (!isTimerActive || step !== "test") return

    const timer = setInterval(() => {
      setTimeLeft((prev) => {
        if (prev <= 1) {
          clearInterval(timer)
          handleTimeUp()
          return 0
        }
        return prev - 1
      })
    }, 1000)

    return () => clearInterval(timer)
  }, [isTimerActive, step])

  // Prevent page reload during test
  useEffect(() => {
    if (step === "test" && isTimerActive) {
      const handleBeforeUnload = (e: BeforeUnloadEvent) => {
        e.preventDefault()
        e.returnValue = ""
      }
      window.addEventListener("beforeunload", handleBeforeUnload)
      return () => window.removeEventListener("beforeunload", handleBeforeUnload)
    }
  }, [step, isTimerActive])

  const handleTimeUp = async () => {
    setIsTimerActive(false)

    // Calculate scores inline
    const categories = {
      language: { correct: 0, total: 0 },
      softskills: { correct: 0, total: 0 },
      competences: { correct: 0, total: 0 },
    }

    let totalCorrect = 0

    questions.forEach((q, idx) => {
      const selectedOption = q.options.find((opt) => opt.id === answers[idx])
      if (selectedOption) {
        const questionCategory = q.category
        categories[questionCategory].total++
        if (selectedOption.correct) {
          categories[questionCategory].correct++
          totalCorrect++
        }
      }
    })

    const scores = {
      language: {
        ...categories.language,
        percentage: categories.language.total > 0
          ? Math.round((categories.language.correct / categories.language.total) * 100)
          : 0
      },
      softskills: {
        ...categories.softskills,
        percentage: categories.softskills.total > 0
          ? Math.round((categories.softskills.correct / categories.softskills.total) * 100)
          : 0
      },
      competences: {
        ...categories.competences,
        percentage: categories.competences.total > 0
          ? Math.round((categories.competences.correct / categories.competences.total) * 100)
          : 0
      }
    }

    setScore(totalCorrect)
    setCategoryScores(scores)

    // Save for authenticated users or store in localStorage for non-authenticated
    if (isAuthenticated) {
      try {
        const languageMapping: Record<Language, string> = {
          'francais': 'french',
          'english': 'english',
          'deutsch': 'german',
          'italiano': 'italian',
          'espanol': 'spanish'
        };

        const overallPercentage = Math.round((totalCorrect / questions.length) * 100);

        await candidateService.addTestScore(
          languageMapping[selectedLanguage],
          {
            linguistic: scores.language.percentage,
            softSkills: scores.softskills.percentage,
            jobSkills: scores.competences.percentage,
            overall: overallPercentage
          }
        );

        // Update local completedTests state
        setCompletedTests(prev => ({
          ...prev,
          [selectedLanguage]: { completed: true, score: overallPercentage }
        }));

        // Clear localStorage to prevent duplicate submissions
        localStorage.removeItem('languageTestResult');

        toast({
          title: "Temps écoulé !",
          description: "Le test est terminé. Vos résultats ont été sauvegardés.",
          variant: "destructive",
        });

        setStep("results");
      } catch (error) {
        console.error('Error saving test scores:', error);
        toast({
          title: "Erreur",
          description: "Impossible de sauvegarder vos résultats.",
          variant: "destructive",
        });
      }
    } else {
      const testResult = {
        language: selectedLanguage,
        date: new Date().toISOString(),
        scores: scores,
        totalScore: totalCorrect,
        totalQuestions: questions.length
      }

      localStorage.setItem('languageTestResult', JSON.stringify(testResult))

      toast({
        title: "Temps écoulé !",
        description: "Le test est terminé. Inscrivez-vous pour voir vos résultats.",
        variant: "destructive",
      })

      setStep("registration")
    }
  }

  const handleStartWarning = (language?: Language) => {
    const langToCheck = language || selectedLanguage

    if (!langToCheck) {
      toast({
        title: "Attention",
        description: "Veuillez sélectionner une langue",
        variant: "destructive",
      })
      return
    }

    // Check if test already completed
    if (completedTests[langToCheck]?.completed) {
      toast({
        title: "Test déjà complété",
        description: `Vous avez déjà passé le test de ${languageOptions.find(l => l.value === langToCheck)?.label}. Score: ${completedTests[langToCheck]?.score}%`,
        variant: "destructive",
      })
      return
    }

    setStep("warning")
  }

  const handleStartTest = () => {
    setStep("test")
    setIsTimerActive(true)
  }

  const handleAnswer = (answerId: string) => {
    const newAnswers = [...answers]
    newAnswers[currentQuestion] = answerId
    setAnswers(newAnswers)

    // Visual feedback
    if (cardRef.current) {
      cardRef.current.classList.add("scale-[1.01]")
      setTimeout(() => {
        cardRef.current?.classList.remove("scale-[1.01]")
      }, 200)
    }

    // Show sparkle effect for answered questions
    if (!answers[currentQuestion]) {
      setShowConfetti(true)
      setTimeout(() => setShowConfetti(false), 800)
    }
  }

  const handleNext = () => {
    if (!answers[currentQuestion]) {
      toast({
        title: "Attention",
        description: "Veuillez sélectionner une réponse",
        variant: "destructive",
      })
      // Shake animation
      if (cardRef.current) {
        cardRef.current.classList.add("animate-shake")
        setTimeout(() => {
          cardRef.current?.classList.remove("animate-shake")
        }, 500)
      }
      return
    }

    if (currentQuestion < questions.length - 1) {
      setQuestionDirection(1)
      setCurrentQuestion(currentQuestion + 1)
    }
  }

  const handlePrevious = () => {
    if (currentQuestion > 0) {
      setQuestionDirection(-1)
      setCurrentQuestion(currentQuestion - 1)
    }
  }

  const handleSubmit = async () => {
    if (answers.filter((a) => a).length < questions.length) {
      toast({
        title: "Attention",
        description: `Il reste ${questions.length - answers.filter((a) => a).length} question(s) sans réponse`,
        variant: "destructive",
      })
      return
    }

    setIsTimerActive(false)

    // Calculate scores
    const categories = {
      language: { correct: 0, total: 0 },
      softskills: { correct: 0, total: 0 },
      competences: { correct: 0, total: 0 },
    }

    let totalCorrect = 0

    questions.forEach((q, idx) => {
      const selectedOption = q.options.find((opt) => opt.id === answers[idx])
      const questionCategory = q.category

      categories[questionCategory].total++

      if (selectedOption?.correct) {
        categories[questionCategory].correct++
        totalCorrect++
      }
    })

    const scores = {
      language: {
        ...categories.language,
        percentage: categories.language.total > 0
          ? Math.round((categories.language.correct / categories.language.total) * 100)
          : 0
      },
      softskills: {
        ...categories.softskills,
        percentage: categories.softskills.total > 0
          ? Math.round((categories.softskills.correct / categories.softskills.total) * 100)
          : 0
      },
      competences: {
        ...categories.competences,
        percentage: categories.competences.total > 0
          ? Math.round((categories.competences.correct / categories.competences.total) * 100)
          : 0
      }
    }

    setScore(totalCorrect)
    setCategoryScores(scores)

    // If authenticated, save test scores directly to backend
    if (isAuthenticated) {
      try {
        const languageMapping: Record<Language, string> = {
          'francais': 'french',
          'english': 'english',
          'deutsch': 'german',
          'italiano': 'italian',
          'espanol': 'spanish'
        };

        const overallPercentage = Math.round((totalCorrect / questions.length) * 100);

        await candidateService.addTestScore(
          languageMapping[selectedLanguage],
          {
            linguistic: scores.language.percentage,
            softSkills: scores.softskills.percentage,
            jobSkills: scores.competences.percentage,
            overall: overallPercentage
          }
        );

        // Update local completedTests state to reflect the saved test
        setCompletedTests(prev => ({
          ...prev,
          [selectedLanguage]: { completed: true, score: overallPercentage }
        }));

        // Clear localStorage to prevent any duplicate submissions
        localStorage.removeItem('languageTestResult');

        console.log('✅ Test scores saved to database for', selectedLanguage);

        toast({
          title: "Test enregistré ! 🎉",
          description: "Vos résultats ont été sauvegardés avec succès.",
        });

        setStep("results");
      } catch (error) {
        console.error('Error saving test scores:', error);
        toast({
          title: "Erreur",
          description: "Impossible de sauvegarder vos résultats. Veuillez réessayer.",
          variant: "destructive",
        });
      }
    } else {
      // Not authenticated - store in localStorage and go to registration
      const testResult = {
        language: selectedLanguage,
        date: new Date().toISOString(),
        scores: scores,
        totalScore: totalCorrect,
        totalQuestions: questions.length
      }

      localStorage.setItem('languageTestResult', JSON.stringify(testResult))
      setStep("registration");
    }
  }

  const calculateScore = () => {
    // This function is kept for backward compatibility
    // but the actual calculation is now done in handleSubmit
    // to avoid async state update issues
  }

  const handleRegistrationChange = (field: string, value: string) => {
    setRegistrationData((prev) => ({
      ...prev,
      [field]: value,
    }))
  }

  const handleRegistrationSubmit = (e: React.FormEvent) => {
    e.preventDefault()

    const requiredFields = ["experienceGlobale", "posteRecherche", "civilite", "nom", "telephone", "villeResidence"]
    const missingFields = requiredFields.filter((field) => !registrationData[field as keyof typeof registrationData])

    if (missingFields.length > 0) {
      toast({
        title: "Informations manquantes",
        description: "Veuillez remplir tous les champs obligatoires pour voir vos résultats.",
        variant: "destructive",
      })
      return
    }

    toast({
      title: "Inscription réussie ! 🎉",
      description: "Découvrez maintenant vos résultats.",
    })

    setStep("results")
  }

  const registrationProgress = Object.values(registrationData).filter((v) => v).length
  const registrationPercentage = (registrationProgress / 6) * 100

  const getScoreLevel = (score: number) => {
    const percentage = (score / questions.length) * 100
    if (percentage >= 90) return { level: "Excellent", color: "text-green-600", bg: "bg-green-500" }
    if (percentage >= 75) return { level: "Très bien", color: "text-blue-600", bg: "bg-blue-500" }
    if (percentage >= 60) return { level: "Bien", color: "text-yellow-600", bg: "bg-yellow-500" }
    if (percentage >= 50) return { level: "Passable", color: "text-orange-600", bg: "bg-orange-500" }
    return { level: "À améliorer", color: "text-red-600", bg: "bg-red-500" }
  }

  const formatTime = (seconds: number) => {
    const mins = Math.floor(seconds / 60)
    const secs = seconds % 60
    return `${mins}:${secs.toString().padStart(2, "0")}`
  }

  // Get questions for the selected language
  const questions = questionsByLanguage[selectedLanguage] || []

  const progress = questions.length > 0 ? ((currentQuestion + 1) / questions.length) * 100 : 0
  const currentQ = questions[currentQuestion]
  const result = getScoreLevel(score)

  // Calculate the final score percentage
  // Priority: savedOverallPercentage (already computed %) > compute from raw score
  const scorePercentage = savedOverallPercentage !== null
    ? savedOverallPercentage
    : totalQuestions > 0
      ? Math.round((score / totalQuestions) * 100)
      : questions.length > 0
        ? Math.round((score / questions.length) * 100)
        : 0

  return (
    <div className="min-h-screen relative bg-gradient-to-br from-slate-50 via-indigo-50/30 to-slate-50 dark:from-slate-950 dark:via-indigo-950/20 dark:to-slate-950">
      <ParticleBackground />
      {/* Header with back button */}
      <div className="relative z-10 bg-white/80 backdrop-blur-sm border-b border-border/50">
        <div className="container mx-auto px-4 lg:px-8 max-w-7xl">
          <div className="flex items-center justify-between h-16">
            <Link
              to="/"
              className="flex items-center gap-2 text-muted-foreground hover:text-foreground transition-colors group"
            >
              <ArrowLeft className="h-4 w-4 group-hover:-translate-x-1 transition-transform" />
              <span className="font-medium">Retour à l'accueil</span>
            </Link>

            <div className="flex items-center">
              <img
                src="/uploads/488f38af-4f42-45f8-a54f-4c1b46a2dfff.png"
                alt="Call Center Match"
                className="h-12 w-auto"
              />
            </div>
          </div>
        </div>
      </div>
      <div className="relative z-10 container mx-auto px-4 py-8 max-w-4xl">
        <AnimatePresence mode="wait">
          {/* STEP 1: Introduction */}
          {step === "intro" && (
            <motion.div
              key="intro"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              exit={{ opacity: 0, y: -20 }}
            >
              <Card className="border border-indigo-200/60 dark:border-indigo-800/60 shadow-xl backdrop-blur-sm bg-white/95 dark:bg-slate-900/95">
                <CardHeader className="bg-gradient-to-r from-indigo-50/50 to-blue-50/50 dark:from-indigo-950/50 dark:to-blue-950/50 border-b border-indigo-100 dark:border-indigo-900">
                  <CardTitle className="text-xl sm:text-3xl font-bold text-center flex items-center justify-center gap-3 text-slate-900 dark:text-white">
                    <Languages className="w-8 h-8 text-indigo-600 dark:text-indigo-400" />
                    Test d'auto-évaluation
                  </CardTitle>
                </CardHeader>
                <CardContent className="p-8 space-y-6">
                  <div className="relative overflow-hidden bg-gradient-to-br from-amber-50 via-orange-50 to-amber-50 dark:from-amber-950/30 dark:via-orange-950/30 dark:to-amber-950/30 border-2 border-amber-300 dark:border-amber-700 p-5 sm:p-6 rounded-xl shadow-sm">
                    <div className="absolute top-0 right-0 w-32 h-32 bg-amber-200/20 dark:bg-amber-800/10 rounded-full -mr-16 -mt-16"></div>
                    <div className="absolute bottom-0 left-0 w-24 h-24 bg-orange-200/20 dark:bg-orange-800/10 rounded-full -ml-12 -mb-12"></div>
                    <div className="relative flex items-start gap-3 sm:gap-4">
                      <div className="bg-amber-500 dark:bg-amber-600 p-2.5 rounded-lg flex-shrink-0">
                        <AlertTriangle className="w-5 h-5 sm:w-6 sm:h-6 text-white" />
                      </div>
                      <div className="space-y-3 flex-1">
                        <h4 className="font-bold text-amber-900 dark:text-amber-100 text-base sm:text-lg">Informations importantes</h4>
                        <ul className="space-y-2 text-amber-900 dark:text-amber-100 text-sm sm:text-base">
                          <li className="flex items-start gap-2">
                            <span className="text-amber-600 dark:text-amber-400 font-bold mt-0.5">•</span>
                            <span>Veuillez vous assurer de disposer de <strong className="font-bold">5 minutes ininterrompues</strong> pour réaliser ce test</span>
                          </li>
                          <li className="flex items-start gap-2">
                            <span className="text-amber-600 dark:text-amber-400 font-bold mt-0.5">•</span>
                            <span>Aucune pause ne sera possible</span>
                          </li>
                          <li className="flex items-start gap-2">
                            <span className="text-amber-600 dark:text-amber-400 font-bold mt-0.5">•</span>
                            <span>Le test comporte <strong className="font-bold">22 questions</strong>, avec toujours une seule bonne réponse par question</span>
                          </li>
                          <li className="flex items-start gap-2">
                            <span className="text-amber-600 dark:text-amber-400 font-bold mt-0.5">•</span>
                            <span>Ce test ne peut être passé qu'<strong className="font-bold">une seule fois tous les 3 mois</strong></span>
                          </li>
                        </ul>
                      </div>
                    </div>
                  </div>

                  <div className="space-y-6">
                    <div className="text-center">
                      <h3 className="text-2xl font-bold text-slate-900 dark:text-white mb-2">Sélectionner la langue</h3>
                      <p className="text-slate-600 dark:text-slate-400">Choisissez la langue que vous souhaitez évaluer</p>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-6 gap-6 max-w-5xl mx-auto [&>*:nth-child(-n+3)]:md:col-span-2 [&>*:nth-child(4)]:md:col-span-2 [&>*:nth-child(4)]:md:col-start-2 [&>*:nth-child(5)]:md:col-span-2">
                      {languageOptions.map((lang) => {
                        // Map language values to LANGUAGES constant
                        const languageMapping: Record<Language, string> = {
                          'francais': 'Français',
                          'english': 'Anglais',
                          'deutsch': 'Allemand',
                          'italiano': 'Italien',
                          'espanol': 'Espagnol'
                        };

                        const languageData = LANGUAGES.find(l => l.value === languageMapping[lang.value]);
                        const FlagIcon = languageData?.flag;
                        const isCompleted = completedTests[lang.value]?.completed || false;
                        const testScore = completedTests[lang.value]?.score;

                        return (
                          <Card
                            key={lang.value}
                            className={`w-full max-w-xs relative transition-all ${isCompleted
                              ? "border-2 border-green-500 shadow-lg bg-green-50/50 dark:bg-green-950/20"
                              : selectedLanguage === lang.value
                                ? "border-2 border-indigo-600 shadow-md hover:border-indigo-500 hover:shadow-lg cursor-pointer"
                                : "border hover:border-indigo-500 hover:shadow-lg cursor-pointer"
                              }`}
                          >
                            {isCompleted && (
                              <div className="absolute -top-2 -right-2 z-10 w-8 h-8 bg-green-500 rounded-full flex items-center justify-center shadow-lg">
                                <Check className="w-5 h-5 text-white" />
                              </div>
                            )}
                            <CardContent className="p-6 flex flex-col items-center text-center space-y-4">
                              <div className="w-24 h-16 rounded-lg overflow-hidden shadow-md">
                                {FlagIcon && <FlagIcon className="w-full h-full object-cover" />}
                              </div>
                              <h4 className="text-xl font-semibold text-slate-900 dark:text-white">{lang.label}</h4>

                              {isCompleted ? (
                                <div className="w-full space-y-2">
                                  <div className="text-green-600 dark:text-green-400 font-semibold flex items-center justify-center gap-2">
                                    <Check className="w-5 h-5" />
                                    Test complété
                                  </div>
                                  {testScore !== undefined && (
                                    <div className="text-sm text-muted-foreground">
                                      Score: {testScore}%
                                    </div>
                                  )}
                                  <Button
                                    variant="outline"
                                    disabled
                                    className="w-full"
                                  >
                                    Déjà passé
                                  </Button>
                                </div>
                              ) : (
                                <Button
                                  onClick={() => {
                                    setSelectedLanguage(lang.value)
                                    handleStartWarning(lang.value)
                                  }}
                                  className="w-full bg-indigo-600 hover:bg-indigo-700 dark:bg-indigo-500 dark:hover:bg-indigo-600"
                                  disabled={loading}
                                >
                                  {loading ? "Chargement..." : "Commencer"}
                                </Button>
                              )}
                            </CardContent>
                          </Card>
                        );
                      })}
                    </div>
                  </div>

                </CardContent>
              </Card>
            </motion.div>
          )}

          {/* STEP 2: Warning */}
          {step === "warning" && (
            <motion.div
              key="warning"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              exit={{ opacity: 0, y: -20 }}
            >
              <Card className="border border-orange-200 dark:border-orange-900/60 shadow-xl backdrop-blur-sm bg-white/95 dark:bg-slate-900/95">
                <CardHeader className="bg-gradient-to-r from-orange-50/50 to-amber-50/50 dark:from-orange-950/30 dark:to-amber-950/30 border-b border-orange-100 dark:border-orange-900">
                  <CardTitle className="text-xl sm:text-3xl font-bold text-center flex items-center justify-center gap-2 sm:gap-3 text-orange-700 dark:text-orange-400">
                    <AlertTriangle className="w-6 h-6 sm:w-8 sm:h-8" />
                    Dernier Avertissement
                  </CardTitle>
                </CardHeader>
                <CardContent className="p-4 sm:p-8 space-y-4 sm:space-y-6">
                  <div className="bg-orange-50/80 dark:bg-orange-950/20 border-2 border-orange-300 dark:border-orange-700 p-3 sm:p-6 rounded-lg space-y-3 sm:space-y-4">
                    <p className="text-sm sm:text-lg font-bold text-orange-900 dark:text-orange-100 text-left">
                      ⚠️ ATTENTION : Une fois que vous cliquez sur &quot;Commencer&quot;, le test démarrera immédiatement !
                    </p>
                    <ul className="space-y-2 sm:space-y-3 text-orange-800 dark:text-orange-200 text-sm sm:text-base">
                      <li className="flex items-start gap-2">
                        <Clock className="w-4 h-4 sm:w-5 sm:h-5 flex-shrink-0 mt-0.5" />
                        <span>
                          Vous aurez exactement <strong>5 minutes</strong> pour terminer
                        </span>
                      </li>
                      <li className="flex items-start gap-2">
                        <XCircle className="w-4 h-4 sm:w-5 sm:h-5 flex-shrink-0 mt-0.5" />
                        <span>Aucune pause ne sera possible</span>
                      </li>
                      <li className="flex items-start gap-2">
                        <AlertTriangle className="w-4 h-4 sm:w-5 sm:h-5 flex-shrink-0 mt-0.5" />
                        <span>
                          Vous ne pourrez repasser ce test que dans <strong>3 mois</strong>
                        </span>
                      </li>
                      <li className="flex items-start gap-2">
                        <CheckCircle2 className="w-4 h-4 sm:w-5 sm:h-5 flex-shrink-0 mt-0.5" />
                        <span>Assurez-vous d'être dans un environnement calme</span>
                      </li>
                      <li className="font-bold">
                        <span>⚠️ NE RECHARGEZ PAS la page une fois le test commencé !</span>
                      </li>
                    </ul>
                  </div>

                  <p className="text-base sm:text-xl font-semibold text-center text-slate-900 dark:text-white px-2">
                    Êtes-vous absolument prêt(e) à commencer maintenant ?
                  </p>

                  <div className="flex flex-col sm:flex-row gap-3 sm:gap-4 justify-center pt-4 px-2">
                    <Button variant="outline" size="lg" onClick={() => setStep("intro")} className="w-full sm:w-auto">
                      Non, revenir en arrière
                    </Button>
                    <Button
                      size="lg"
                      onClick={handleStartTest}
                      className="bg-orange-600 hover:bg-orange-700 dark:bg-orange-500 dark:hover:bg-orange-600 text-white w-full sm:w-auto sm:px-8 shadow-lg"
                    >
                      Oui, je suis prêt(e) !
                    </Button>
                  </div>
                </CardContent>
              </Card>
            </motion.div>
          )}

          {/* STEP 3: Test */}
          {step === "test" && (
            <motion.div
              key="test"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              exit={{ opacity: 0, y: -20 }}
            >
              {/* Timer */}
              <Card className={`mb-3 border ${timeLeft < 60 ? "border-orange-400 dark:border-orange-600 animate-pulse bg-orange-50/50 dark:bg-orange-950/20" : "border-indigo-200 dark:border-indigo-800 bg-white/95 dark:bg-slate-900/95"} shadow-lg backdrop-blur-sm`}>
                <CardContent className="p-2.5">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <Clock className={`w-4 h-4 ${timeLeft < 60 ? "text-orange-600 dark:text-orange-400" : "text-indigo-600 dark:text-indigo-400"}`} />
                      <span className={`text-sm font-semibold ${timeLeft < 60 ? "text-orange-900 dark:text-orange-100" : "text-slate-900 dark:text-white"}`}>Temps restant:</span>
                    </div>
                    <div className={`text-xl font-bold ${timeLeft < 60 ? "text-orange-600 dark:text-orange-400" : "text-indigo-600 dark:text-indigo-400"}`}>
                      {formatTime(timeLeft)}
                    </div>
                  </div>
                </CardContent>
              </Card>

              <Card
                ref={cardRef}
                className="border border-indigo-200 dark:border-indigo-800 shadow-xl transition-all duration-200 relative overflow-hidden backdrop-blur-sm bg-white/95 dark:bg-slate-900/95"
              >
                {showConfetti && (
                  <motion.div
                    initial={{ opacity: 0, scale: 0 }}
                    animate={{ opacity: 1, scale: 1 }}
                    exit={{ opacity: 0, scale: 0 }}
                    className="absolute top-4 right-4 z-10"
                  >
                    <Sparkles className="w-8 h-8 text-yellow-500 animate-pulse" />
                  </motion.div>
                )}

                <CardHeader className="bg-gradient-to-r from-indigo-50/50 to-blue-50/50 dark:from-indigo-950/50 dark:to-blue-950/50 border-b border-indigo-100 dark:border-indigo-900">
                  <div className="space-y-2">
                    <div className="flex justify-between items-center">
                      <motion.span
                        key={currentQuestion}
                        initial={{ opacity: 0, y: -10 }}
                        animate={{ opacity: 1, y: 0 }}
                        className="text-sm font-medium text-slate-600 dark:text-slate-400"
                      >
                        Question {currentQuestion + 1} sur {questions.length}
                      </motion.span>
                      <motion.span
                        className="text-sm font-semibold text-indigo-600 dark:text-indigo-400 flex items-center gap-1"
                        animate={{ scale: answers[currentQuestion] ? [1, 1.1, 1] : 1 }}
                      >
                        <CheckCircle2 className="w-4 h-4" />
                        {answers.filter((a) => a).length} / {questions.length}
                      </motion.span>
                    </div>
                    <Progress value={progress} className="h-2 transition-all duration-500" />
                  </div>
                </CardHeader>

                <CardContent className="p-4 md:p-5 space-y-4">
                  <AnimatePresence mode="wait">
                    <motion.div
                      key={currentQuestion}
                      initial={{ opacity: 0, x: questionDirection * 50 }}
                      animate={{ opacity: 1, x: 0 }}
                      exit={{ opacity: 0, x: questionDirection * -50 }}
                      transition={{ duration: 0.3 }}
                      className="space-y-4"
                    >
                      <h3 className="text-base md:text-lg font-semibold text-slate-900 dark:text-white leading-snug">
                        {currentQ.question}
                      </h3>

                      <RadioGroup value={answers[currentQuestion]} onValueChange={handleAnswer} className="space-y-2">
                        {currentQ.options.map((option, idx) => (
                          <motion.div
                            key={option.id}
                            initial={{ opacity: 0, x: -20 }}
                            animate={{ opacity: 1, x: 0 }}
                            transition={{ delay: idx * 0.1 }}
                          >
                            <Label
                              htmlFor={`q${currentQuestion}-${option.id}`}
                              className={`flex items-start gap-2.5 p-3 rounded-lg border-2 cursor-pointer transition-all duration-300 ${answers[currentQuestion] === option.id
                                ? "border-indigo-600 dark:border-indigo-500 bg-indigo-50 dark:bg-indigo-950/40 shadow-lg scale-[1.02]"
                                : "border-slate-200 dark:border-slate-700 hover:border-indigo-400 dark:hover:border-indigo-600 hover:bg-indigo-50/50 dark:hover:bg-indigo-950/20 hover:shadow-md hover:scale-[1.01]"
                                }`}
                            >
                              <RadioGroupItem
                                value={option.id}
                                id={`q${currentQuestion}-${option.id}`}
                                className="mt-0.5"
                              />
                              <span className="text-sm leading-snug flex-1 text-slate-900 dark:text-white">{option.text}</span>
                              {answers[currentQuestion] === option.id && (
                                <motion.div
                                  initial={{ scale: 0 }}
                                  animate={{ scale: 1 }}
                                  transition={{ type: "spring", stiffness: 500, damping: 15 }}
                                >
                                  <CheckCircle2 className="w-5 h-5 text-indigo-600 dark:text-indigo-400" />
                                </motion.div>
                              )}
                            </Label>
                          </motion.div>
                        ))}
                      </RadioGroup>
                    </motion.div>
                  </AnimatePresence>

                  <div className="flex flex-col-reverse sm:flex-row gap-2 justify-between pt-2 border-t border-slate-200 dark:border-slate-700">
                    <Button
                      variant="outline"
                      onClick={handlePrevious}
                      disabled={currentQuestion === 0}
                      className="w-full sm:w-auto text-sm py-2"
                    >
                      ← Précédent
                    </Button>
                    {currentQuestion === questions.length - 1 ? (
                      <Button onClick={handleSubmit} className="bg-emerald-600 hover:bg-emerald-700 dark:bg-emerald-500 dark:hover:bg-emerald-600 text-white w-full sm:w-auto shadow-lg text-sm py-2">
                        Soumettre le test
                        <CheckCircle2 className="w-4 h-4 ml-2" />
                      </Button>
                    ) : (
                      <Button onClick={handleNext} className="bg-indigo-600 hover:bg-indigo-700 dark:bg-indigo-500 dark:hover:bg-indigo-600 text-white w-full sm:w-auto shadow-lg text-sm py-2">
                        Suivant →
                      </Button>
                    )}
                  </div>
                </CardContent>
              </Card>
            </motion.div>
          )}

          {/* STEP 4: Registration */}
          {step === "registration" && (
            <motion.div
              key="registration"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              exit={{ opacity: 0, y: -20 }}
            >
              <Card className="border-2 border-primary/20 shadow-2xl relative overflow-hidden">
                <div className="absolute top-0 left-0 w-full h-1 bg-gradient-to-r from-primary via-blue-500 to-primary" />

                <CardHeader className="bg-gradient-to-r from-primary/10 to-blue-500/10 border-b-2 border-primary/20">
                  <CardTitle className="text-2xl sm:text-3xl font-bold text-center">
                    <span className="bg-gradient-to-r from-primary to-blue-600 bg-clip-text text-transparent">
                      Inscription Requise
                    </span>
                  </CardTitle>
                  <p className="text-sm sm:text-base text-center text-muted-foreground mt-2 px-4">
                    Inscrivez-vous pour découvrir vos résultats et accéder aux meilleures opportunités
                  </p>
                </CardHeader>

                <CardContent className="p-6 sm:p-8 space-y-6">
                  <div className="text-center space-y-4">
                    <motion.div
                      className="w-16 h-16 mx-auto bg-gradient-to-br from-primary/10 to-blue-500/10 rounded-full flex items-center justify-center border-2 border-primary/30"
                      animate={{ scale: [1, 1.05, 1] }}
                      transition={{ duration: 2, repeat: Infinity }}
                    >
                      <Sparkles className="w-8 h-8 text-primary" />
                    </motion.div>

                    <div className="space-y-2">
                      <h3 className="text-lg sm:text-xl font-semibold text-foreground">
                        Prêt(e) à découvrir vos résultats ?
                      </h3>
                      <p className="text-sm sm:text-base text-muted-foreground max-w-md mx-auto">
                        Complétez votre inscription en quelques étapes pour accéder à votre score détaillé et aux opportunités qui vous correspondent.
                      </p>
                    </div>

                    <div className="flex flex-wrap items-center justify-center gap-4 text-xs sm:text-sm text-muted-foreground pt-2">
                      <div className="flex items-center gap-2">
                        <CheckCircle2 className="w-4 h-4 text-primary" />
                        <span>Sans CV</span>
                      </div>
                      <div className="flex items-center gap-2">
                        <CheckCircle2 className="w-4 h-4 text-primary" />
                        <span>2 minutes</span>
                      </div>
                      <div className="flex items-center gap-2">
                        <CheckCircle2 className="w-4 h-4 text-primary" />
                        <span>Gratuit</span>
                      </div>
                    </div>
                  </div>

                  <div className="flex flex-col gap-3 pt-4">
                    <Button
                      size="lg"
                      onClick={() => {
                        // Prepare detailed test scores for backend
                        const totalQs = totalQuestions > 0 ? totalQuestions : questions.length;
                        const detailedScores = {
                          linguistic: categoryScores?.language?.percentage || 0,
                          softSkills: categoryScores?.softskills?.percentage || 0,
                          jobSkills: categoryScores?.competences?.percentage || 0,
                          overall: totalQs > 0 ? Math.round((score / totalQs) * 100) : 0
                        };

                        navigate("/inscription-detaillee", {
                          state: {
                            fromLanguageTest: true,
                            testScore: detailedScores,
                            testLanguage: selectedLanguage
                          }
                        });
                      }}
                      className="w-full bg-gradient-to-r from-primary to-blue-600 hover:from-primary/90 hover:to-blue-600/90 text-white px-6 py-6 text-base sm:text-lg font-semibold shadow-lg hover:shadow-xl transition-all group"
                    >
                      Commencer l'inscription
                      <ArrowRight className="w-5 h-5 ml-2 group-hover:translate-x-1 transition-transform" />
                    </Button>

                    <div className="relative">
                      <div className="absolute inset-0 flex items-center">
                        <div className="w-full border-t border-border"></div>
                      </div>
                      <div className="relative flex justify-center text-xs uppercase">
                        <span className="bg-card px-2 text-muted-foreground">ou</span>
                      </div>
                    </div>

                    <Button
                      variant="outline"
                      size="lg"
                      onClick={() => {
                        // Prepare detailed test scores for backend
                        const totalQs = totalQuestions > 0 ? totalQuestions : questions.length;
                        const detailedScores = {
                          linguistic: categoryScores?.language?.percentage || 0,
                          softSkills: categoryScores?.softskills?.percentage || 0,
                          jobSkills: categoryScores?.competences?.percentage || 0,
                          overall: totalQs > 0 ? Math.round((score / totalQs) * 100) : 0
                        };

                        navigate("/candidate-login", {
                          state: {
                            fromLanguageTest: true,
                            testScore: detailedScores,
                            testLanguage: selectedLanguage
                          }
                        });
                      }}
                      className="w-full border-2 border-primary/30 hover:bg-primary/5 px-6 py-6 text-base sm:text-lg font-semibold transition-all"
                    >
                      <User className="w-5 h-5 mr-2" />
                      Déjà inscrit ? Se connecter
                    </Button>
                  </div>

                  <div className="pt-4 text-center">
                    <p className="text-xs text-muted-foreground">
                      En vous inscrivant, vous acceptez nos{" "}
                      <a href="/terms" className="text-primary hover:underline">Conditions Générales</a>
                      {" "}et notre{" "}
                      <a href="/privacy" className="text-primary hover:underline">Politique de Confidentialité</a>
                    </p>
                  </div>
                </CardContent>
              </Card>
            </motion.div>
          )}

          {/* STEP 6: Results */}
          {step === "results" && (
            <motion.div
              key="results"
              initial={{ opacity: 0, scale: 0.9 }}
              animate={{ opacity: 1, scale: 1 }}
              exit={{ opacity: 0, scale: 0.9 }}
            >
              <Card className="border-2 border-primary/20 shadow-2xl">
                <CardHeader className="bg-gradient-to-r from-primary/10 to-blue-500/10 border-b py-4">
                  <CardTitle className="text-2xl font-bold text-center">Résultats de Votre Évaluation</CardTitle>
                  <p className="text-sm text-center text-muted-foreground mt-1">Merci d'avoir passé le test</p>
                </CardHeader>
                <CardContent className="p-6 space-y-6">
                  {/* Score Global */}
                  <Card className="bg-gradient-to-br from-primary/10 to-blue-500/10 border-2 border-primary/20">
                    <CardContent className="p-6 text-center">
                      <motion.div
                        initial={{ scale: 0 }}
                        animate={{ scale: 1 }}
                        transition={{ type: "spring", duration: 0.6 }}
                      >
                        <div className="text-5xl mb-3">🎉</div>
                        <h3 className="text-2xl font-bold text-primary mb-2">Félicitations !</h3>
                        <p className="text-base text-muted-foreground mb-4">
                          Votre profil sera évalué par nos recruteurs
                        </p>
                        <div className="inline-flex items-center justify-center w-32 h-32 rounded-full bg-gradient-to-br from-primary to-blue-600 text-white shadow-lg">
                          <div className="text-center">
                            <div className="text-4xl font-bold">{scorePercentage}%</div>
                            <div className="text-sm">Score global</div>
                          </div>
                        </div>
                      </motion.div>
                    </CardContent>
                  </Card>

                  {/* Info message */}
                  <Card className="bg-gradient-to-br from-blue-50 to-indigo-50 dark:from-blue-950/20 dark:to-indigo-950/20 border-2 border-blue-200 dark:border-blue-800">
                    <CardContent className="p-4">
                      <div className="flex items-start gap-3">
                        <CheckCircle2 className="w-6 h-6 text-blue-600 dark:text-blue-400 flex-shrink-0 mt-1" />
                        <div className="space-y-1">
                          <h4 className="text-base font-semibold text-blue-900 dark:text-blue-100">
                            Votre évaluation est enregistrée
                          </h4>
                          <p className="text-sm text-blue-700 dark:text-blue-300">
                            Votre profil complet sera examiné par nos recruteurs qui vous contacteront si votre profil correspond à nos opportunités.
                          </p>
                        </div>
                      </div>
                    </CardContent>
                  </Card>

                  {/* Call to action */}
                  <div className="flex justify-center pt-2">
                    <Button
                      size="lg"
                      onClick={() => {
                        if (isAuthenticated) {
                          navigate("/dashboard");
                        } else {
                          // Build detailed scores object for backend
                          const totalQs = totalQuestions > 0 ? totalQuestions : questions.length;
                          const detailedScores = {
                            linguistic: categoryScores?.language?.percentage || 0,
                            softSkills: categoryScores?.softskills?.percentage || 0,
                            jobSkills: categoryScores?.competences?.percentage || 0,
                            overall: totalQs > 0 ? Math.round((score / totalQs) * 100) : scorePercentage
                          };

                          // Pass test data in navigation state for inscription form
                          navigate("/inscription-detaillee", {
                            state: {
                              fromLanguageTest: true,
                              testLanguage: selectedLanguage,
                              testScore: detailedScores,
                              totalQuestions: totalQs
                            }
                          });
                        }
                      }}
                      className="gap-2 bg-primary hover:bg-primary/90 h-12 px-8 text-base"
                    >
                      <Briefcase className="w-5 h-5" />
                      {isAuthenticated ? "Aller au tableau de bord" : "Continuer mon inscription"}
                    </Button>
                  </div>
                </CardContent>
              </Card>
            </motion.div>
          )}
        </AnimatePresence>
      </div>
    </div>
  )
}

export default LanguageTest
