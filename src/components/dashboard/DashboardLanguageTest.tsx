"use client"

import { useState, useEffect } from "react"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Progress } from "@/components/ui/progress"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import { Label } from "@/components/ui/label"
import { CheckCircle2, XCircle, Clock, Languages } from "lucide-react"
import { motion, AnimatePresence } from "framer-motion"
import { toast } from "@/hooks/use-toast"
import { questionsByLanguage, languageOptions, type Language } from "@/utils/testQuestions"
import { LANGUAGES } from "@/utils/constants"
import { candidateService, type CandidateProfile } from '@/services/candidateService';

interface DashboardLanguageTestProps {
  profile: CandidateProfile | null;
  onRefresh: () => void;
}

const DashboardLanguageTest = ({ profile, onRefresh }: DashboardLanguageTestProps) => {
  // Debug log for test scores
  console.log('📊 DashboardLanguageTest - testScores:', profile?.testScores);

  const [hasStarted, setHasStarted] = useState(false)
  const [selectedLanguage, setSelectedLanguage] = useState<Language>("francais")
  const [currentQuestion, setCurrentQuestion] = useState(0)
  const [answers, setAnswers] = useState<string[]>([])
  const [timeLeft, setTimeLeft] = useState(300) // 5 minutes
  const [isTimerActive, setIsTimerActive] = useState(false)
  const [testCompleted, setTestCompleted] = useState(false)
  const [score, setScore] = useState(0)
  const [categoryScores, setCategoryScores] = useState<{
    language: { correct: number; total: number; percentage: number }
    softskills: { correct: number; total: number; percentage: number }
    competences: { correct: number; total: number; percentage: number }
  } | null>(null)

  useEffect(() => {
    if (!isTimerActive) return

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
  }, [isTimerActive])

  const handleTimeUp = async () => {
    setIsTimerActive(false)
    await calculateScore()
    setTestCompleted(true)
    toast({
      title: "Temps écoulé !",
      description: "Le test est terminé.",
      variant: "destructive",
    })
  }

  const handleStart = () => {
    setHasStarted(true)
    setIsTimerActive(true)
  }

  const handleAnswer = (answerId: string) => {
    const newAnswers = [...answers]
    newAnswers[currentQuestion] = answerId
    setAnswers(newAnswers)
  }

  const handleNext = () => {
    if (!answers[currentQuestion]) {
      toast({
        title: "Attention",
        description: "Veuillez sélectionner une réponse",
        variant: "destructive",
      })
      return
    }

    if (currentQuestion < questions.length - 1) {
      setCurrentQuestion(currentQuestion + 1)
    }
  }

  const handlePrevious = () => {
    if (currentQuestion > 0) {
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
    await calculateScore()
    setTestCompleted(true)
  }

  const calculateScore = async () => {
    // Initialize category counters
    const categories = {
      language: { correct: 0, total: 0 },
      softskills: { correct: 0, total: 0 },
      competences: { correct: 0, total: 0 },
    }

    let totalCorrect = 0

    // Count correct answers by category
    questions.forEach((q, idx) => {
      const selectedOption = q.options.find((opt) => opt.id === answers[idx])
      const questionCategory = q.category

      categories[questionCategory].total++

      if (selectedOption?.correct) {
        categories[questionCategory].correct++
        totalCorrect++
      }
    })

    // Calculate percentages
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

    const overallPercentage = Math.round((totalCorrect / questions.length) * 100);

    setScore(totalCorrect)
    setCategoryScores(scores)

    // Save test scores to database
    try {
      // Map test language to backend format
      const languageMapping: Record<Language, string> = {
        'francais': 'french',
        'english': 'english',
        'deutsch': 'german',
        'italiano': 'italian',
        'espanol': 'spanish'
      };

      await candidateService.addTestScore(
        languageMapping[selectedLanguage],
        {
          linguistic: scores.language.percentage,
          softSkills: scores.softskills.percentage,
          jobSkills: scores.competences.percentage,
          overall: overallPercentage
        }
      );

      console.log('✅ Test scores saved successfully');
      console.log('📊 Saved test data:', {
        language: languageMapping[selectedLanguage],
        completed: true,
        scores: {
          linguistic: scores.language.percentage,
          softSkills: scores.softskills.percentage,
          jobSkills: scores.competences.percentage,
          overall: overallPercentage
        }
      });

      toast({
        title: 'Test enregistré ! 🎉',
        description: 'Vos résultats ont été sauvegardés avec succès.',
      });

      // Clear any localStorage test data
      localStorage.removeItem('languageTestResult');

      // Refresh profile data to show updated test scores
      onRefresh();

    } catch (error) {
      console.error('❌ Error saving test scores:', error);
      toast({
        title: 'Erreur',
        description: 'Impossible de sauvegarder les résultats du test',
        variant: 'destructive'
      });
    }
  }

  const formatTime = (seconds: number) => {
    const mins = Math.floor(seconds / 60)
    const secs = seconds % 60
    return `${mins}:${secs.toString().padStart(2, "0")}`
  }

  // Get questions for selected language
  const questions = questionsByLanguage[selectedLanguage]
  const progress = ((currentQuestion + 1) / questions.length) * 100
  const currentQ = questions[currentQuestion]

  const getScoreLevel = (score: number) => {
    if (score >= 90) return { level: "Excellent", color: "text-green-600", bg: "bg-green-500" }
    if (score >= 75) return { level: "Très bien", color: "text-blue-600", bg: "bg-blue-500" }
    if (score >= 60) return { level: "Bien", color: "text-yellow-600", bg: "bg-yellow-500" }
    if (score >= 50) return { level: "Passable", color: "text-orange-600", bg: "bg-orange-500" }
    return { level: "À améliorer", color: "text-red-600", bg: "bg-red-500" }
  }

  const formatScore = (score: number) => {
    return `${score}%`
  }

  if (!hasStarted) {
    return (
      <div className="space-y-6">
        <Card className="border-2 border-blue-200 shadow-md bg-gradient-to-br from-blue-50 via-indigo-50 to-blue-100">
          <CardHeader className="bg-gradient-to-r from-blue-100 to-blue-100 border-b-2 border-blue-200">
            <CardTitle className="text-2xl font-bold text-center flex items-center justify-center gap-3">
              <Languages className="w-8 h-8 text-blue-600" />
              Test d'auto-évaluation
            </CardTitle>
          </CardHeader>
          <CardContent className="p-8 space-y-6">
            <div className="bg-blue-50 border-l-4 border-blue-400 p-4 rounded">
              <div className="flex items-start gap-3">
                <Clock className="w-6 h-6 text-blue-600 flex-shrink-0 mt-0.5" />
                <div className="space-y-2">
                  <p className="font-semibold text-blue-800">Informations importantes :</p>
                  <ul className="list-disc list-inside space-y-1 text-blue-700 text-sm">
                    <li>
                      Durée : <strong>5 minutes</strong>
                    </li>
                    <li>
                      Nombre de questions : <strong>22</strong>
                    </li>
                    <li>Une seule bonne réponse par question</li>
                    <li>
                      Le test peut être passé <strong>une fois tous les 3 mois</strong>
                    </li>
                    <li>Assurez-vous d'être dans un environnement calme</li>
                  </ul>
                </div>
              </div>
            </div>

            <div className="space-y-6">
              <div className="text-center">
                <h3 className="text-xl font-bold text-slate-900 mb-2">Sélectionner la langue</h3>
                <p className="text-slate-600">Choisissez la langue que vous souhaitez évaluer</p>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-6 gap-6 max-w-5xl mx-auto [&>*:nth-child(-n+3)]:md:col-span-2 [&>*:nth-child(4)]:md:col-span-2 [&>*:nth-child(4)]:md:col-start-2 [&>*:nth-child(5)]:md:col-span-2">
                {languageOptions.map((lang) => {
                  const languageMapping: Record<Language, string> = {
                    'francais': 'Français',
                    'english': 'Anglais',
                    'deutsch': 'Allemand',
                    'italiano': 'Italien',
                    'espanol': 'Espagnol'
                  };

                  // Check if test is already completed - check both correct and legacy field names
                  const testCompleteMapping: Record<Language, string[]> = {
                    'francais': ['french_test_completed'],
                    'english': ['english_test_completed'],
                    'deutsch': ['german_test_completed', 'deutsch_test_completed'], // Check both for backwards compatibility
                    'italiano': ['italian_test_completed', 'italiano_test_completed'],
                    'espanol': ['spanish_test_completed', 'espanol_test_completed']
                  };

                  const scoreMapping: Record<Language, string[]> = {
                    'francais': ['french_overall_score'],
                    'english': ['english_overall_score'],
                    'deutsch': ['german_overall_score', 'deutsch_overall_score'],
                    'italiano': ['italian_overall_score', 'italiano_overall_score'],
                    'espanol': ['spanish_overall_score', 'espanol_overall_score']
                  };

                  // Check any of the field names for completion status
                  const isCompleted = testCompleteMapping[lang.value].some(
                    field => profile?.testScores?.[field as keyof typeof profile.testScores] === true
                  );

                  // Get score from any of the field names
                  const testScore = scoreMapping[lang.value].reduce<number | undefined>((score, field) => {
                    if (score !== undefined) return score;
                    const value = profile?.testScores?.[field as keyof typeof profile.testScores];
                    return typeof value === 'number' ? value : undefined;
                  }, undefined);

                  const languageData = LANGUAGES.find(l => l.value === languageMapping[lang.value]);
                  const FlagIcon = languageData?.flag;

                  return (
                    <Card
                      key={lang.value}
                      className={`w-full max-w-xs relative transition-all ${isCompleted
                        ? "border-2 border-green-500 shadow-lg bg-green-50/50"
                        : "border hover:border-blue-500 hover:shadow-lg"
                        }`}
                    >
                      {isCompleted && (
                        <div className="absolute -top-2 -right-2 z-10 w-8 h-8 bg-green-500 rounded-full flex items-center justify-center shadow-lg">
                          <CheckCircle2 className="w-5 h-5 text-white" />
                        </div>
                      )}
                      <CardContent className="p-6 flex flex-col items-center text-center space-y-4">
                        <div className="w-24 h-16 rounded-lg overflow-hidden shadow-md">
                          {FlagIcon && <FlagIcon className="w-full h-full object-cover" />}
                        </div>
                        <h4 className="text-xl font-semibold text-slate-900">{lang.label}</h4>

                        {isCompleted ? (
                          <div className="w-full space-y-2">
                            <div className="text-green-600 font-semibold flex items-center justify-center gap-2">
                              <CheckCircle2 className="w-5 h-5" />
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
                              setSelectedLanguage(lang.value);
                              handleStart();
                            }}
                            className="w-full bg-primary hover:bg-primary/90"
                          >
                            Commencer
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
      </div>
    )
  }

  if (testCompleted) {
    const overallPercentage = Math.round((score / questions.length) * 100)
    const overallLevel = getScoreLevel(overallPercentage)

    return (
      <div className="space-y-6">
        <Card className="border-2 border-blue-200 shadow-md bg-gradient-to-br from-blue-50 via-indigo-50 to-blue-100">
          <CardHeader className="bg-gradient-to-r from-green-100 to-emerald-50 border-b-2 border-green-200">
            <CardTitle className="text-2xl font-bold text-center">Résultats du Test 🎉</CardTitle>
          </CardHeader>
          <CardContent className="p-8 space-y-6">
            <div className="space-y-6">
              <Card className="bg-gradient-to-br from-primary/10 to-blue-500/10 border-2 border-primary/20">
                <CardContent className="p-6 text-center">
                  <motion.div
                    initial={{ scale: 0 }}
                    animate={{ scale: 1 }}
                    transition={{ type: "spring", duration: 0.6 }}
                    className="mb-4"
                  >
                    <div className="text-6xl mb-2">🎉</div>
                    <h3 className="text-2xl font-bold text-primary mb-2">Félicitations !</h3>
                    <p className="text-lg text-muted-foreground mb-4">
                      Vous avez terminé le test avec succès !
                    </p>
                  </motion.div>
                  <div className="inline-flex items-center justify-center w-32 h-32 rounded-full bg-gradient-to-br from-primary to-blue-600 text-white">
                    <div className="text-center">
                      <div className="text-4xl font-bold">{overallPercentage}%</div>
                      <div className="text-sm">Score global</div>
                    </div>
                  </div>
                </CardContent>
              </Card>
            </div>

            <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
              <p className="text-sm text-blue-700">
                <strong>Votre score a été enregistré.</strong> Il sera visible par les recruteurs et vous permettra
                d'augmenter vos chances de décrocher un poste.
              </p>
            </div>
          </CardContent>
        </Card>
      </div>
    )
  }

  return (
    <div className="space-y-6">
      {/* Timer */}
      <Card className={`border-2 ${timeLeft < 60 ? "border-red-500 animate-pulse" : "border-primary/20"}`}>
        <CardContent className="p-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-2">
              <Clock className={`w-5 h-5 ${timeLeft < 60 ? "text-red-600" : "text-primary"}`} />
              <span className="font-semibold">Temps restant:</span>
            </div>
            <div className={`text-2xl font-bold ${timeLeft < 60 ? "text-red-600" : "text-primary"}`}>
              {formatTime(timeLeft)}
            </div>
          </div>
        </CardContent>
      </Card>

      <Card className="border-2 border-primary/20 shadow-xl">
        <CardHeader className="bg-gradient-to-r from-primary/10 to-blue-500/10 border-b">
          <div className="space-y-2">
            <div className="flex justify-between items-center">
              <span className="text-sm font-medium text-muted-foreground">
                Question {currentQuestion + 1} sur {questions.length}
              </span>
              <span className="text-sm font-semibold text-primary flex items-center gap-1">
                <CheckCircle2 className="w-4 h-4" />
                {answers.filter((a) => a).length} / {questions.length}
              </span>
            </div>
            <Progress value={progress} className="h-2" />
          </div>
        </CardHeader>

        <CardContent className="p-8 space-y-6">
          <AnimatePresence mode="wait">
            <motion.div
              key={currentQuestion}
              initial={{ opacity: 0, x: 50 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -50 }}
              transition={{ duration: 0.3 }}
              className="space-y-6"
            >
              <h3 className="text-xl md:text-2xl font-semibold text-foreground leading-relaxed">{currentQ.question}</h3>

              <RadioGroup value={answers[currentQuestion]} onValueChange={handleAnswer} className="space-y-3">
                {currentQ.options.map((option) => (
                  <Label
                    key={option.id}
                    htmlFor={`q${currentQuestion}-${option.id}`}
                    className={`flex items-start gap-3 p-4 rounded-lg border-2 cursor-pointer transition-all ${answers[currentQuestion] === option.id
                        ? "border-primary bg-primary/10 shadow-lg"
                        : "border-border hover:border-primary/50 hover:bg-primary/5"
                      }`}
                  >
                    <RadioGroupItem value={option.id} id={`q${currentQuestion}-${option.id}`} className="mt-0.5" />
                    <span className="text-base leading-relaxed flex-1">{option.text}</span>
                    {answers[currentQuestion] === option.id && <CheckCircle2 className="w-5 h-5 text-primary" />}
                  </Label>
                ))}
              </RadioGroup>
            </motion.div>
          </AnimatePresence>

          <div className="flex gap-3 justify-between pt-4 border-t">
            <Button variant="outline" onClick={handlePrevious} disabled={currentQuestion === 0}>
              ← Précédent
            </Button>
            {currentQuestion === questions.length - 1 ? (
              <Button onClick={handleSubmit} className="bg-green-600 hover:bg-green-700">
                Soumettre le test
                <CheckCircle2 className="w-4 h-4 ml-2" />
              </Button>
            ) : (
              <Button onClick={handleNext} className="bg-primary hover:bg-primary/90">
                Suivant →
              </Button>
            )}
          </div>
        </CardContent>
      </Card>
    </div>
  )
}

export default DashboardLanguageTest
