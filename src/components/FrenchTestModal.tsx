"use client"

import { useState } from "react"
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Label } from "@/components/ui/label"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import { Progress } from "@/components/ui/progress"
import { Card, CardContent } from "@/components/ui/card"
import { CheckCircle2, XCircle, ArrowRight, ArrowLeft, Languages } from "lucide-react"
import { FR, GB, DE } from "country-flag-icons/react/3x2"
import { motion, AnimatePresence } from "framer-motion"
import { useToast } from "@/hooks/use-toast"
import { questionsByLanguage, type Language as TestLanguage, type Question } from "@/utils/testQuestions"

interface FrenchTestModalProps {
  isOpen: boolean
  onClose: () => void
}

type Language = "french" | "english" | "german"

interface CategoryScores {
  language: { correct: number; total: number; percentage: number }
  softskills: { correct: number; total: number; percentage: number }
  competences: { correct: number; total: number; percentage: number }
  overall: { correct: number; total: number; percentage: number }
}

// Map FrenchTestModal language types to testQuestions language types
const languageMap: Record<Language, TestLanguage> = {
  french: "francais",
  english: "english",
  german: "deutsch"
}

const FrenchTestModal = ({ isOpen, onClose }: FrenchTestModalProps) => {
  const [selectedLanguage, setSelectedLanguage] = useState<Language | null>(null)
  const [currentQuestion, setCurrentQuestion] = useState(0)
  const [selectedAnswers, setSelectedAnswers] = useState<string[]>([])
  const [showResults, setShowResults] = useState(false)
  const [score, setScore] = useState(0)
  const [categoryScores, setCategoryScores] = useState<CategoryScores | null>(null)
  const { toast } = useToast()

  // Get questions from testQuestions.ts based on selected language
  const questions: Question[] = selectedLanguage ? questionsByLanguage[languageMap[selectedLanguage]] : []
  const progress = questions.length > 0 ? ((currentQuestion + 1) / questions.length) * 100 : 0

  const handleAnswer = (answerId: string) => {
    const newAnswers = [...selectedAnswers]
    newAnswers[currentQuestion] = answerId
    setSelectedAnswers(newAnswers)
  }

  const handleNext = () => {
    if (!selectedAnswers[currentQuestion]) {
      toast({
        title: "Attention",
        description: "Veuillez sélectionner une réponse",
        variant: "destructive",
      })
      return
    }

    if (currentQuestion < questions.length - 1) {
      setCurrentQuestion(currentQuestion + 1)
    } else {
      calculateResults()
    }
  }

  const handlePrevious = () => {
    if (currentQuestion > 0) {
      setCurrentQuestion(currentQuestion - 1)
    }
  }

  const calculateResults = () => {
    // Initialize category counters
    const categories = {
      language: { correct: 0, total: 0 },
      softskills: { correct: 0, total: 0 },
      competences: { correct: 0, total: 0 },
    }

    let totalCorrect = 0

    // Count correct answers by category using the category field from questions
    questions.forEach((q, index) => {
      const selectedOption = q.options.find((opt) => opt.id === selectedAnswers[index])
      
      // Use the category field from the question
      const questionCategory = q.category

      categories[questionCategory].total++

      if (selectedOption?.correct) {
        categories[questionCategory].correct++
        totalCorrect++
      }
    })

    // Calculate percentages
    const scores: CategoryScores = {
      language: {
        ...categories.language,
        percentage: categories.language.total > 0 
          ? (categories.language.correct / categories.language.total) * 100 
          : 0
      },
      softskills: {
        ...categories.softskills,
        percentage: categories.softskills.total > 0 
          ? (categories.softskills.correct / categories.softskills.total) * 100 
          : 0
      },
      competences: {
        ...categories.competences,
        percentage: categories.competences.total > 0 
          ? (categories.competences.correct / categories.competences.total) * 100 
          : 0
      },
      overall: {
        correct: totalCorrect,
        total: questions.length,
        percentage: questions.length > 0 ? (totalCorrect / questions.length) * 100 : 0
      }
    }

    // Store in localStorage with timestamp
    const testResult = {
      language: selectedLanguage,
      date: new Date().toISOString(),
      scores: scores,
    }
    
    localStorage.setItem('languageTestResult', JSON.stringify(testResult))
    
    setScore(totalCorrect)
    setCategoryScores(scores)
    setShowResults(true)
  }

  const getLevel = (score: number) => {
    const percentage = (score / questions.length) * 100
    if (percentage >= 90) return { level: "Excellent", color: "green", percentage: 90 }
    if (percentage >= 70) return { level: "Acceptable", color: "yellow", percentage: 70 }
    return { level: "À améliorer", color: "red", percentage: 50 }
  }

  const resetTest = () => {
    setCurrentQuestion(0)
    setSelectedAnswers([])
    setShowResults(false)
    setScore(0)
    setCategoryScores(null)
    setSelectedLanguage(null)
  }

  const handleClose = () => {
    resetTest()
    onClose()
  }

  const currentQ = questions[currentQuestion]
  const resultLevel = getLevel(score)

  const languageLabels = {
    french: "Français",
    english: "English",
    german: "Deutsch",
  }

  return (
    <Dialog open={isOpen} onOpenChange={handleClose}>
      <DialogContent className="max-w-3xl max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle className="text-2xl font-bold text-brand-dark flex items-center gap-2">
            <Languages className="w-6 h-6 text-primary" />
            {showResults ? "Vos Résultats" : "Test de Niveau de Langue"}
          </DialogTitle>
        </DialogHeader>

        <AnimatePresence mode="wait">
          {!selectedLanguage ? (
            <motion.div
              key="language-select"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              exit={{ opacity: 0, y: -20 }}
              className="space-y-6 py-6"
            >
              <div className="text-center mb-8">
                <h3 className="text-2xl font-bold text-brand-dark mb-2">Sélectionner la langue</h3>
                <p className="text-muted-foreground">Choisissez la langue que vous souhaitez évaluer</p>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                <Card className="hover:border-primary hover:shadow-lg transition-all cursor-pointer">
                  <CardContent className="p-6 flex flex-col items-center text-center space-y-4">
                    <div className="w-24 h-16 rounded-lg overflow-hidden shadow-md">
                      <FR className="w-full h-full object-cover" />
                    </div>
                    <h4 className="text-xl font-semibold text-brand-dark">Français</h4>
                    <Button 
                      onClick={() => setSelectedLanguage("french")}
                      className="w-full bg-primary hover:bg-primary/90"
                    >
                      Commencer
                    </Button>
                  </CardContent>
                </Card>

                <Card className="hover:border-primary hover:shadow-lg transition-all cursor-pointer">
                  <CardContent className="p-6 flex flex-col items-center text-center space-y-4">
                    <div className="w-24 h-16 rounded-lg overflow-hidden shadow-md">
                      <GB className="w-full h-full object-cover" />
                    </div>
                    <h4 className="text-xl font-semibold text-brand-dark">English</h4>
                    <Button 
                      onClick={() => setSelectedLanguage("english")}
                      className="w-full bg-primary hover:bg-primary/90"
                    >
                      Commencer
                    </Button>
                  </CardContent>
                </Card>

                <Card className="hover:border-primary hover:shadow-lg transition-all cursor-pointer">
                  <CardContent className="p-6 flex flex-col items-center text-center space-y-4">
                    <div className="w-24 h-16 rounded-lg overflow-hidden shadow-md">
                      <DE className="w-full h-full object-cover" />
                    </div>
                    <h4 className="text-xl font-semibold text-brand-dark">Deutsch</h4>
                    <Button 
                      onClick={() => setSelectedLanguage("german")}
                      className="w-full bg-primary hover:bg-primary/90"
                    >
                      Commencer
                    </Button>
                  </CardContent>
                </Card>
              </div>
            </motion.div>
          ) : !showResults ? (
            <motion.div
              key="test"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              exit={{ opacity: 0, y: -20 }}
              className="space-y-6"
            >
              {/* Progress */}
              <div className="space-y-2">
                <div className="flex justify-between text-sm text-muted-foreground">
                  <span>
                    Question {currentQuestion + 1} sur {questions.length}
                  </span>
                </div>
                <Progress value={progress} className="h-2" />
              </div>

              {/* Question */}
              <Card className="bg-gradient-to-br from-card to-primary/5 border-2 border-primary/10">
                <CardContent className="p-6">
                  <h3 className="text-xl font-semibold text-brand-dark mb-6">{currentQ.question}</h3>

                  <RadioGroup
                    value={selectedAnswers[currentQuestion]}
                    onValueChange={handleAnswer}
                    className="space-y-3"
                  >
                    {currentQ.options.map((option) => (
                      <div key={option.id} className="relative">
                        <Label
                          htmlFor={option.id}
                          className={`flex items-center space-x-3 p-4 rounded-lg border-2 cursor-pointer transition-all duration-200 ${
                            selectedAnswers[currentQuestion] === option.id
                              ? "border-primary bg-primary/10"
                              : "border-border hover:border-primary/50 hover:bg-primary/5"
                          }`}
                        >
                          <RadioGroupItem value={option.id} id={option.id} />
                          <span className="text-base">{option.text}</span>
                        </Label>
                      </div>
                    ))}
                  </RadioGroup>
                </CardContent>
              </Card>

              {/* Navigation */}
              <div className="flex justify-between">
                <Button
                  variant="outline"
                  onClick={handlePrevious}
                  disabled={currentQuestion === 0}
                  className="gap-2 bg-transparent"
                >
                  <ArrowLeft className="w-4 h-4" />
                  Précédent
                </Button>
                <Button onClick={handleNext} className="bg-primary hover:bg-primary/90 gap-2">
                  {currentQuestion === questions.length - 1 ? "Terminer" : "Suivant"}
                  {currentQuestion === questions.length - 1 ? (
                    <CheckCircle2 className="w-4 h-4" />
                  ) : (
                    <ArrowRight className="w-4 h-4" />
                  )}
                </Button>
              </div>
            </motion.div>
          ) : (
            <motion.div
              key="results"
              initial={{ opacity: 0, scale: 0.9 }}
              animate={{ opacity: 1, scale: 1 }}
              className="space-y-6 py-6"
            >
              {/* Score display */}
              <div className="space-y-6">
                {/* Niveau linguistique */}
                <Card className="bg-gradient-to-br from-green-50 to-green-100 border-2 border-green-200">
                  <CardContent className="p-6">
                    <div className="flex items-center justify-between mb-3">
                      <h4 className="text-lg font-semibold text-green-900">Niveau linguistique</h4>
                      <span className="text-2xl font-bold text-green-600">Excellent</span>
                    </div>
                    <Progress value={90} className="h-3 mb-2" />
                    <p className="text-right text-sm font-semibold text-green-700">90%</p>
                  </CardContent>
                </Card>

                {/* Soft skills */}
                <Card className="bg-gradient-to-br from-green-50 to-green-100 border-2 border-green-200">
                  <CardContent className="p-6">
                    <div className="flex items-center justify-between mb-3">
                      <h4 className="text-lg font-semibold text-green-900">Soft skills</h4>
                      <span className="text-2xl font-bold text-green-600">Excellent</span>
                    </div>
                    <Progress value={85} className="h-3 mb-2" />
                    <p className="text-right text-sm font-semibold text-green-700">85%</p>
                  </CardContent>
                </Card>

                {/* Compétences métier */}
                <Card className="bg-gradient-to-br from-yellow-50 to-yellow-100 border-2 border-yellow-200">
                  <CardContent className="p-6">
                    <div className="flex items-center justify-between mb-3">
                      <h4 className="text-lg font-semibold text-yellow-900">Compétences métier</h4>
                      <span className="text-2xl font-bold text-yellow-600">Acceptable</span>
                    </div>
                    <Progress value={65} className="h-3 mb-2" />
                    <p className="text-right text-sm font-semibold text-yellow-700">65%</p>
                  </CardContent>
                </Card>

                {/* Résultats */}
                <Card className="bg-gradient-to-br from-primary/10 to-blue-500/10 border-2 border-primary/20">
                  <CardContent className="p-6 text-center">
                    <div className="mb-4">
                      <div className="text-6xl mb-2">🎉</div>
                      <h3 className="text-2xl font-bold text-primary mb-2">Félicitations !</h3>
                      <p className="text-lg text-muted-foreground mb-4">
                        Ton profil sera fortement apprécié par nos partenaires !
                      </p>
                    </div>
                    <div className="inline-flex items-center justify-center w-32 h-32 rounded-full bg-gradient-to-br from-primary to-blue-600 text-white">
                      <div className="text-center">
                        <div className="text-4xl font-bold">80%</div>
                        <div className="text-sm">Score global</div>
                      </div>
                    </div>
                  </CardContent>
                </Card>
              </div>

              {/* Detailed results */}
              <Card className="bg-gradient-to-br from-card to-primary/5">
                <CardContent className="p-6 space-y-4">
                  <h4 className="font-semibold text-brand-dark mb-4">Détails des réponses :</h4>
                  {questions.map((q, index) => {
                    const selectedOption = q.options.find((opt) => opt.id === selectedAnswers[index])
                    const isCorrect = selectedOption?.correct

                    return (
                      <div key={q.id} className="flex items-start gap-3 p-3 rounded-lg bg-white">
                        {isCorrect ? (
                          <CheckCircle2 className="w-5 h-5 text-green-600 flex-shrink-0 mt-0.5" />
                        ) : (
                          <XCircle className="w-5 h-5 text-red-600 flex-shrink-0 mt-0.5" />
                        )}
                        <div className="flex-1">
                          <p className="text-sm font-medium text-brand-dark">{q.question}</p>
                          <p className={`text-sm ${isCorrect ? "text-green-600" : "text-red-600"}`}>
                            {selectedOption?.text}
                          </p>
                        </div>
                      </div>
                    )
                  })}
                </CardContent>
              </Card>

              {/* Actions */}
              <div className="flex gap-3">
                <Button variant="outline" onClick={resetTest} className="flex-1 bg-transparent">
                  Refaire le test
                </Button>
                <Button onClick={handleClose} className="flex-1 bg-primary hover:bg-primary/90">
                  Terminer
                </Button>
              </div>
            </motion.div>
          )}
        </AnimatePresence>
      </DialogContent>
    </Dialog>
  )
}

export default FrenchTestModal
