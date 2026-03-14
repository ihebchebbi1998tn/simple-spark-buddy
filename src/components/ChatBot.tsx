"use client"

import type React from "react"
import { useState, useEffect, useRef } from "react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { X, Send, Bot, Eye, EyeOff } from "lucide-react"
import { motion, AnimatePresence } from "framer-motion"
import { useToast } from "@/hooks/use-toast"
import { useNotificationSound } from "@/hooks/useNotificationSound"
import { TUNISIAN_CITIES, OPERATION_ACTIVITY_MAPPING, EXPERIENCE_LEVELS, filterExperienceLevelsByGlobal } from "@/utils/constants"
import { validatePasswordRequirements, checkPasswordStrength } from "@/utils/formValidation"
import { useTranslation } from "react-i18next"

interface ChatMessage {
  id: string
  type: "bot" | "user"
  content: string
  timestamp: Date
}

interface FullFormData {
  experienceGlobale: string
  posteRecherche: string
  experiencePosteRecherche: string
  civilite: string
  nom: string
  prenom: string
  telephone: string
  villeResidence: string
  activitePrincipale: string
  experienceActivitePrincipale: string
  operation1: string
  experienceOperation1: string
  operation2: string
  experienceOperation2: string
  langueMaternelle: string
  langueForeign1: string
  niveauLangueForeign1: string
  hasSecondForeignLanguage: string
  langueForeign2: string
  niveauLangueForeign2: string
  bilingue: string
  modeTravail: string
  tempsTravail: string
  parcTravail: string
  email: string
  motDePasse: string
  confirmationMotDePasse: string
  dateNaissance: string
  acceptCGU: boolean
}

type StepType = "buttons" | "text"

interface Step {
  key: string
  questionKey: string
  type: StepType
  getOptions?: () => Array<{ value: string; labelKey: string }>
  shouldSkip?: () => boolean
  validation?: (v: string) => string | null
  inputType?: string
  onSelect?: (value: string) => void
}

const ChatBot = () => {
  const { t } = useTranslation('chatbot')
  const [isOpen, setIsOpen] = useState(false)
  const [showInitialPopup, setShowInitialPopup] = useState(false)
  const [showPromoPopup, setShowPromoPopup] = useState(false)
  const [messages, setMessages] = useState<ChatMessage[]>([])
  const [currentInput, setCurrentInput] = useState("")
  const [currentStep, setCurrentStep] = useState(0)
  const [formData, setFormData] = useState<FullFormData>({
    experienceGlobale: "", posteRecherche: "", experiencePosteRecherche: "",
    civilite: "", nom: "", prenom: "", telephone: "", villeResidence: "",
    activitePrincipale: "", experienceActivitePrincipale: "",
    operation1: "", experienceOperation1: "", operation2: "", experienceOperation2: "",
    langueMaternelle: "", langueForeign1: "", niveauLangueForeign1: "",
    hasSecondForeignLanguage: "", langueForeign2: "", niveauLangueForeign2: "",
    bilingue: "", modeTravail: "", tempsTravail: "", parcTravail: "",
    email: "", motDePasse: "", confirmationMotDePasse: "", dateNaissance: "", acceptCGU: false,
  })
  const formDataRef = useRef(formData)
  formDataRef.current = formData
  const [isTyping, setIsTyping] = useState(false)
  const [isCompleted, setIsCompleted] = useState(false)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [hasScrolledAndClosed, setHasScrolledAndClosed] = useState(false)
  const [showButtons, setShowButtons] = useState(false)
  const [showPassword, setShowPassword] = useState(false)
  const messagesEndRef = useRef<HTMLDivElement>(null)
  const promoTimerRef = useRef<NodeJS.Timeout | null>(null)
  const autoCloseTimerRef = useRef<NodeJS.Timeout | null>(null)
  const { toast } = useToast()
  const { playNotification } = useNotificationSound()

  // --- Data options: values stay the same for backend, labels are i18n keys ---
  const experienceLevels = [
    { value: "0", labelKey: "options.noExperience" },
    { value: "1", labelKey: "options.months0to6" },
    { value: "2", labelKey: "options.months6to12" },
    { value: "3", labelKey: "options.years1to2" },
    { value: "4", labelKey: "options.years2to3" },
    { value: "5", labelKey: "options.years3to5" },
    { value: "6", labelKey: "options.years5to7" },
    { value: "7", labelKey: "options.years7plus" },
  ]

  const positions = [
    { value: "1", labelKey: "options.agent" },
    { value: "2", labelKey: "options.teamLeader" },
    { value: "3", labelKey: "options.responsableActivite" },
    { value: "4", labelKey: "options.responsableQualite" },
    { value: "5", labelKey: "options.responsableServiceClient" },
    { value: "6", labelKey: "options.responsableRecrutement" },
    { value: "7", labelKey: "options.responsablePlateau" },
    { value: "8", labelKey: "options.formateur" },
    { value: "9", labelKey: "options.chargeRecrutement" },
  ]

  const activities = [
    { value: "1", labelKey: "options.televente" },
    { value: "2", labelKey: "options.teleprospection" },
    { value: "3", labelKey: "options.priseRDV" },
    { value: "4", labelKey: "options.serviceClient" },
  ]

  const allOperations = [
    { value: "1", labelKey: "options.energie" },
    { value: "2", labelKey: "options.photovoltaique" },
    { value: "3", labelKey: "options.telecom" },
    { value: "4", labelKey: "options.mutuelle" },
    { value: "5", labelKey: "options.voyance" },
    { value: "6", labelKey: "options.complements" },
    { value: "7", labelKey: "options.sitesWeb" },
    { value: "8", labelKey: "options.telesecretariat" },
    { value: "9", labelKey: "options.dons" },
    { value: "10", labelKey: "options.qualification" },
    { value: "11", labelKey: "options.jeux" },
    { value: "12", labelKey: "options.tourismeMedical" },
    { value: "13", labelKey: "options.dossiersAdmin" },
    { value: "14", labelKey: "options.cpf" },
    { value: "15", labelKey: "options.gestionCommandes" },
    { value: "16", labelKey: "options.moderation" },
    { value: "17", labelKey: "options.reservations" },
  ]

  const languages = [
    { value: "Français", labelKey: "options.french" },
    { value: "Arabe", labelKey: "options.arabic" },
    { value: "Anglais", labelKey: "options.english" },
    { value: "Espagnol", labelKey: "options.spanish" },
    { value: "Allemand", labelKey: "options.german" },
    { value: "Italien", labelKey: "options.italian" },
    { value: "Portugais", labelKey: "options.portuguese" },
    { value: "Néerlandais", labelKey: "options.dutch" },
    { value: "Turc", labelKey: "options.turkish" },
    { value: "Russe", labelKey: "options.russian" },
    { value: "Chinois", labelKey: "options.chinese" },
  ]

  const languageLevels = [
    { value: "debutant", labelKey: "options.beginner" },
    { value: "elementaire", labelKey: "options.elementary" },
    { value: "intermediaire", labelKey: "options.intermediate" },
    { value: "avance", labelKey: "options.advanced" },
    { value: "courant", labelKey: "options.fluent" },
    { value: "natif", labelKey: "options.native" },
  ]

  // --- Business logic helpers ---
  const getHasMinimalExperience = () => ["0", "1"].includes(formDataRef.current.experienceGlobale)
  const getIsLowExperience = () => ["0", "1", "2", "3", "4"].includes(formDataRef.current.experienceGlobale)

  const experienceToMonths = (v: string): number => {
    const m: Record<string, number> = { "1": 6, "2": 12, "3": 24, "4": 36, "5": 60, "6": 84, "7": 120 }
    return m[v] || 0
  }

  const getFilteredExpOptions = (cap?: string) => {
    if (!cap) return experienceLevels.filter(e => e.value !== "0")
    const filtered = filterExperienceLevelsByGlobal(cap, EXPERIENCE_LEVELS.map(e => ({ value: e.value, label: e.label })))
    return experienceLevels.filter(e => e.value !== "0" && filtered.some(f => f.value === e.value))
  }

  const getFilteredOperations = () => {
    const fd = formDataRef.current
    if (!fd.activitePrincipale) return allOperations
    return allOperations.filter(op => OPERATION_ACTIVITY_MAPPING[op.value]?.includes(fd.activitePrincipale))
  }

  const getOperationExpOptions = (isOp2: boolean) => {
    const fd = formDataRef.current
    if (!fd.experienceActivitePrincipale) return experienceLevels.filter(e => e.value !== "0")
    const budget = experienceToMonths(fd.experienceActivitePrincipale)
    const used = isOp2 ? experienceToMonths(fd.experienceOperation1 || "0") : experienceToMonths(fd.experienceOperation2 || "0")
    const remaining = budget - used
    return experienceLevels.filter(e => e.value !== "0" && experienceToMonths(e.value) <= remaining)
  }

  // --- Steps definition ---
  const steps: Step[] = [
    {
      key: "experienceGlobale",
      questionKey: "questions.experienceGlobale",
      type: "buttons",
      getOptions: () => experienceLevels,
      onSelect: (v) => {
        if (["0", "1", "2", "3", "4"].includes(v)) {
          setFormData(prev => ({ ...prev, posteRecherche: "1" }))
        }
        if (["0", "1"].includes(v)) {
          setFormData(prev => ({
            ...prev, experienceActivitePrincipale: "1", experienceOperation1: "1"
          }))
        }
      }
    },
    {
      key: "posteRecherche",
      questionKey: "questions.posteRecherche",
      type: "buttons",
      shouldSkip: () => getIsLowExperience(),
      getOptions: () => positions,
    },
    {
      key: "experiencePosteRecherche",
      questionKey: "questions.experiencePosteRecherche",
      type: "buttons",
      shouldSkip: () => formDataRef.current.posteRecherche === "1" || getIsLowExperience(),
      getOptions: () => getFilteredExpOptions(formDataRef.current.experienceGlobale),
    },
    {
      key: "civilite",
      questionKey: "questions.civilite",
      type: "buttons",
      getOptions: () => [{ value: "madame", labelKey: "options.mrs" }, { value: "monsieur", labelKey: "options.mr" }],
    },
    {
      key: "nom",
      questionKey: "questions.nom",
      type: "text",
      validation: (v) => v.trim().length >= 2 ? null : t("validation.nameTooShort"),
    },
    {
      key: "prenom",
      questionKey: "questions.prenom",
      type: "text",
      validation: (v) => v.trim().length >= 2 ? null : t("validation.firstNameTooShort"),
    },
    {
      key: "telephone",
      questionKey: "questions.telephone",
      type: "text",
      validation: (v) => {
        const cleaned = v.replace(/\D/g, '')
        if (cleaned.length !== 8) return t("validation.phoneDigits")
        if (!['2', '3', '4', '5', '7', '9'].includes(cleaned[0])) return t("validation.phoneInvalid")
        return null
      },
    },
    {
      key: "villeResidence",
      questionKey: "questions.villeResidence",
      type: "buttons",
      getOptions: () => TUNISIAN_CITIES.map(c => ({ value: c.value, labelKey: c.label })),
    },
    {
      key: "activitePrincipale",
      questionKey: "questions.activitePrincipale",
      type: "buttons",
      getOptions: () => activities,
    },
    {
      key: "experienceActivitePrincipale",
      questionKey: "questions.experienceActivitePrincipale",
      type: "buttons",
      shouldSkip: () => getHasMinimalExperience(),
      getOptions: () => getFilteredExpOptions(formDataRef.current.experienceGlobale),
    },
    {
      key: "operation1",
      questionKey: "questions.operation1",
      type: "buttons",
      getOptions: () => getFilteredOperations(),
    },
    {
      key: "experienceOperation1",
      questionKey: "questions.experienceOperation1",
      type: "buttons",
      shouldSkip: () => getHasMinimalExperience(),
      getOptions: () => getOperationExpOptions(false),
    },
    {
      key: "operation2",
      questionKey: "questions.operation2",
      type: "buttons",
      shouldSkip: () => !formDataRef.current.operation1,
      getOptions: () => [
        { value: "", labelKey: "options.skipOperation2" },
        ...getFilteredOperations().filter(op => op.value !== formDataRef.current.operation1)
      ],
      onSelect: (v) => {
        if (v && getHasMinimalExperience()) {
          setFormData(prev => ({ ...prev, experienceOperation2: "1" }))
        }
      }
    },
    {
      key: "experienceOperation2",
      questionKey: "questions.experienceOperation2",
      type: "buttons",
      shouldSkip: () => !formDataRef.current.operation2 || getHasMinimalExperience(),
      getOptions: () => getOperationExpOptions(true),
    },
    {
      key: "langueMaternelle",
      questionKey: "questions.langueMaternelle",
      type: "buttons",
      getOptions: () => languages,
    },
    {
      key: "langueForeign1",
      questionKey: "questions.langueForeign1",
      type: "buttons",
      getOptions: () => languages.filter(l => l.value !== formDataRef.current.langueMaternelle),
    },
    {
      key: "niveauLangueForeign1",
      questionKey: "questions.niveauLangueForeign1",
      type: "buttons",
      getOptions: () => languageLevels,
    },
    {
      key: "hasSecondForeignLanguage",
      questionKey: "questions.hasSecondForeignLanguage",
      type: "buttons",
      getOptions: () => [{ value: "oui", labelKey: "options.yes" }, { value: "non", labelKey: "options.no" }],
    },
    {
      key: "langueForeign2",
      questionKey: "questions.langueForeign2",
      type: "buttons",
      shouldSkip: () => formDataRef.current.hasSecondForeignLanguage !== "oui",
      getOptions: () => languages.filter(l => l.value !== formDataRef.current.langueMaternelle && l.value !== formDataRef.current.langueForeign1),
    },
    {
      key: "niveauLangueForeign2",
      questionKey: "questions.niveauLangueForeign2",
      type: "buttons",
      shouldSkip: () => formDataRef.current.hasSecondForeignLanguage !== "oui" || !formDataRef.current.langueForeign2,
      getOptions: () => languageLevels,
    },
    {
      key: "bilingue",
      questionKey: "questions.bilingue",
      type: "buttons",
      getOptions: () => [{ value: "oui", labelKey: "options.yes" }, { value: "non", labelKey: "options.no" }],
    },
    {
      key: "modeTravail",
      questionKey: "questions.modeTravail",
      type: "buttons",
      getOptions: () => [
        { value: "presentiel", labelKey: "options.onSite" },
        { value: "teletravail", labelKey: "options.remote" },
        { value: "peu_importe", labelKey: "options.noPreference" },
      ],
    },
    {
      key: "tempsTravail",
      questionKey: "questions.tempsTravail",
      type: "buttons",
      getOptions: () => [
        { value: "plein_temps", labelKey: "options.fullTime" },
        { value: "mi_temps", labelKey: "options.partTime" },
        { value: "peu_importe", labelKey: "options.noPreference" },
      ],
    },
    {
      key: "parcTravail",
      questionKey: "questions.parcTravail",
      type: "buttons",
      getOptions: () => [
        { value: "jour", labelKey: "options.dayShift" },
        { value: "nuit", labelKey: "options.nightShift" },
        { value: "peu_importe", labelKey: "options.noPreference" },
      ],
    },
    {
      key: "email",
      questionKey: "questions.email",
      type: "text",
      validation: (v) => {
        if (!v.trim()) return t("validation.emailRequired")
        if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v)) return t("validation.emailInvalid")
        return null
      },
    },
    {
      key: "motDePasse",
      questionKey: "questions.motDePasse",
      type: "text",
      inputType: "password",
      validation: (v) => {
        const { isValid, errors } = validatePasswordRequirements(v)
        return isValid ? null : errors.join(', ')
      },
    },
    {
      key: "confirmationMotDePasse",
      questionKey: "questions.confirmationMotDePasse",
      type: "text",
      inputType: "password",
      validation: (v) => v === formDataRef.current.motDePasse ? null : t("validation.passwordMismatch"),
    },
    {
      key: "dateNaissance",
      questionKey: "questions.dateNaissance",
      type: "text" as StepType,
      validation: (v: string) => {
        const dateRegex = /^(\d{2})[\/\-](\d{2})[\/\-](\d{4})$/
        const isoRegex = /^\d{4}-\d{2}-\d{2}$/
        if (isoRegex.test(v)) {
          const date = new Date(v)
          if (isNaN(date.getTime())) return t("validation.dateInvalid")
          const age = Math.floor((Date.now() - date.getTime()) / (365.25 * 24 * 60 * 60 * 1000))
          if (age < 16) return t("validation.ageMinimum")
          if (age > 100) return t("validation.dateInvalid")
          return null
        }
        const match = v.match(dateRegex)
        if (!match) return t("validation.dateFormat")
        const [, day, month, year] = match
        const date = new Date(`${year}-${month}-${day}`)
        if (isNaN(date.getTime())) return t("validation.dateInvalid")
        const age = Math.floor((Date.now() - date.getTime()) / (365.25 * 24 * 60 * 60 * 1000))
        if (age < 16) return t("validation.ageMinimum")
        if (age > 100) return t("validation.dateInvalid")
        return null
      },
    },
    {
      key: "confirmCGU",
      questionKey: "questions.confirmCGU",
      type: "buttons",
      getOptions: () => [
        { value: "oui", labelKey: "options.acceptCGU" },
        { value: "non", labelKey: "options.refuseCGU" },
      ],
    },
  ]

  // --- Effects ---
  useEffect(() => {
    const timer = setTimeout(() => setShowInitialPopup(true), 4000)
    return () => clearTimeout(timer)
  }, [])

  useEffect(() => {
    let lastScrollY = window.scrollY
    const handleScroll = () => {
      const currentScrollY = window.scrollY
      if (currentScrollY > lastScrollY && currentScrollY > 100) {
        if (isOpen || showInitialPopup) {
          setIsOpen(false)
          setShowInitialPopup(false)
          setHasScrolledAndClosed(true)
        }
      }
      lastScrollY = currentScrollY
    }
    window.addEventListener("scroll", handleScroll, { passive: true })
    return () => window.removeEventListener("scroll", handleScroll)
  }, [isOpen, showInitialPopup])

  useEffect(() => {
    if (hasScrolledAndClosed && !isOpen && !showPromoPopup) {
      promoTimerRef.current = setTimeout(() => {
        setShowPromoPopup(true)
        playNotification()
        autoCloseTimerRef.current = setTimeout(() => setShowPromoPopup(false), 5000)
      }, 10000)
    }
    return () => {
      if (promoTimerRef.current) clearTimeout(promoTimerRef.current)
      if (autoCloseTimerRef.current) clearTimeout(autoCloseTimerRef.current)
    }
  }, [hasScrolledAndClosed, isOpen, showPromoPopup, playNotification])

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" })
  }, [messages, isTyping])

  // --- Message helpers ---
  const addBotMessage = (content: string, showOptionsAfter: boolean = false) => {
    setIsTyping(true)
    setShowButtons(false)
    setTimeout(() => {
      setMessages(prev => [...prev, {
        id: Date.now().toString(),
        type: 'bot',
        content,
        timestamp: new Date(),
      }])
      setIsTyping(false)
      if (showOptionsAfter) setShowButtons(true)
      playNotification()
    }, 800 + Math.random() * 600)
  }

  const addUserMessage = (content: string) => {
    setMessages(prev => [...prev, {
      id: Date.now().toString(),
      type: 'user',
      content,
      timestamp: new Date(),
    }])
  }

  // --- Navigation ---
  const startConversation = () => {
    setIsOpen(true)
    setShowInitialPopup(false)
    setShowPromoPopup(false)
    setHasScrolledAndClosed(false)
    if (messages.length === 0) {
      setTimeout(() => addBotMessage(t(steps[0].questionKey), steps[0].type === "buttons"), 500)
    }
  }

  const findNextStep = (fromIndex: number): number => {
    let next = fromIndex + 1
    while (next < steps.length) {
      const step = steps[next]
      if (step.shouldSkip && step.shouldSkip()) {
        next++
        continue
      }
      break
    }
    return next
  }

  const advanceToNextStep = () => {
    setTimeout(() => {
      const nextIdx = findNextStep(currentStep)
      if (nextIdx >= steps.length) {
        submitRegistration()
        return
      }
      setCurrentStep(nextIdx)
      setShowPassword(false)
      const nextStep = steps[nextIdx]
      const isButtons = nextStep.type === "buttons"
      setTimeout(() => addBotMessage(t(nextStep.questionKey), isButtons), 100)
    }, 50)
  }

  // --- Handlers ---
  const handleOptionSelect = (value: string) => {
    const step = steps[currentStep]
    const options = step.getOptions?.() || []
    const selected = options.find(o => o.value === value)
    // Display translated label to user, but send value to backend
    addUserMessage(selected ? t(selected.labelKey) : value)

    if (step.key === "confirmCGU") {
      if (value === "non") {
        addBotMessage(t("errors.cguRequired"), true)
        return
      }
      setFormData(prev => ({ ...prev, acceptCGU: true }))
      step.onSelect?.(value)
      advanceToNextStep()
      return
    }

    if (step.key !== "welcome" && step.key !== "confirmCGU") {
      setFormData(prev => ({ ...prev, [step.key]: value }))
    }
    step.onSelect?.(value)

    setTimeout(() => advanceToNextStep(), 400)
  }

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    if (!currentInput.trim()) return
    processTextInput(currentInput.trim())
    setCurrentInput("")
  }

  const processTextInput = async (input: string) => {
    const step = steps[currentStep]

    if (step.validation) {
      const error = step.validation(input)
      if (error) {
        if (step.key === "confirmationMotDePasse") {
          addBotMessage(`❌ ${error}. ${t("validation.retryPassword")}`)
          setFormData(prev => ({ ...prev, motDePasse: "", confirmationMotDePasse: "" }))
          const passwordStepIdx = steps.findIndex(s => s.key === "motDePasse")
          if (passwordStepIdx >= 0) {
            setTimeout(() => {
              setCurrentStep(passwordStepIdx)
              setShowPassword(false)
              setTimeout(() => addBotMessage(t(steps[passwordStepIdx].questionKey), false), 100)
            }, 800)
          }
          return
        }
        addBotMessage(`❌ ${error}. ${t("validation.retry")}`)
        return
      }
    }

    addUserMessage(step.inputType === "password" ? "••••••••" : input)

    if (step.key === "telephone") {
      try {
        const { candidateService } = await import('@/services/candidateService')
        const { formatPhoneForBackend } = await import('@/utils/formValidation')
        const cleaned = input.replace(/\D/g, '')
        const formatted = formatPhoneForBackend(cleaned)
        const result = await candidateService.checkAvailability({ phone: formatted })
        if (result.data.phoneExists) {
          addBotMessage(`❌ ${t("errors.phoneExists")}`)
          return
        }
      } catch (err) {
        console.error('Phone check error:', err)
      }
    }

    if (step.key === "email") {
      try {
        const { candidateService } = await import('@/services/candidateService')
        const result = await candidateService.checkAvailability({ email: input })
        if (result.data.emailExists) {
          addBotMessage(`❌ ${t("errors.emailExists")}`)
          return
        }
      } catch (err) {
        console.error('Email check error:', err)
      }
    }

    const saveValue = step.key === "telephone" ? input.replace(/\D/g, '') : input.trim()
    setFormData(prev => ({ ...prev, [step.key]: saveValue }))

    setTimeout(() => advanceToNextStep(), 400)
  }

  // --- Submit registration ---
  const submitRegistration = async () => {
    setIsCompleted(true)
    setIsSubmitting(true)

    try {
      const { mapToBackendFormat } = await import('@/services/fieldMapping')
      const { sanitizeObject } = await import('@/utils/sanitization')
      const { leadService } = await import('@/services/leadService')
      const { authService } = await import('@/services/authService')

      const fd = formDataRef.current
      const combinedData = {
        ...fd,
        hasSecondForeignLanguage: fd.hasSecondForeignLanguage || "non",
        bilingue: fd.bilingue || "non",
        dateNaissance: (() => {
          const dob = fd.dateNaissance
          if (!dob) return null
          const match = dob.match(/^(\d{2})[\/\-](\d{2})[\/\-](\d{4})$/)
          if (match) return `${match[3]}-${match[2]}-${match[1]}`
          return dob
        })(),
        testLanguage: null,
        testScore: null,
      }

      const sanitized: any = sanitizeObject(combinedData)
      sanitized.registrationSource = 'chatbot'

      console.log('📤 ChatBot - Final payload:', { ...sanitized, motDePasse: '[REDACTED]' })

      const response = await leadService.register(sanitized)

      if (response.success || response.message?.includes('success')) {
        const hasToken = !!response.data?.token;
        
        if (hasToken) {
          authService.storeAuthToken(response.data!.token)
          authService.storeUserData({ lead_id: response.data!.lead_id, email: response.data!.email, name: response.data!.last_name, surname: response.data!.first_name })
        }

        addBotMessage(hasToken ? t("success.registrationComplete") : t("success.registrationComplete") + " Votre compte est en cours d'activation.")
        toast({ title: t("success.registrationTitle"), description: hasToken ? t("success.welcome") : "Votre compte est en cours d'activation. Veuillez vous connecter." })

        try {
          localStorage.removeItem('candidateRegistrationData')
          localStorage.removeItem('languageTestResult')
        } catch (e) {
          console.error('Failed to clear localStorage:', e)
        }

        setTimeout(() => { window.location.href = hasToken ? '/dashboard' : '/espace-candidats' }, 3000)
      } else {
        throw new Error(response.message || 'Erreur')
      }
    } catch (error: any) {
      console.error('❌ ChatBot registration error:', error)
      setIsCompleted(false)
      setIsSubmitting(false)

      const errMsg = error?.message?.toLowerCase() || error?.response?.data?.message?.toLowerCase() || ''
      let msg = t("errors.genericError")
      let goBackToStep: number | null = null

      if (errMsg.includes('email') && (errMsg.includes('exist') || errMsg.includes('déjà') || errMsg.includes('duplicate') || errMsg.includes('already'))) {
        msg = t("errors.emailDuplicate")
        goBackToStep = steps.findIndex(s => s.key === "email")
        setFormData(prev => ({ ...prev, email: "" }))
      } else if (errMsg.includes('phone') || errMsg.includes('téléphone') || errMsg.includes('telephone')) {
        msg = t("errors.phoneDuplicate")
        goBackToStep = steps.findIndex(s => s.key === "telephone")
        setFormData(prev => ({ ...prev, telephone: "" }))
      } else if (errMsg.includes('password') || errMsg.includes('mot de passe') || errMsg.includes('passe')) {
        msg = t("errors.passwordError")
        goBackToStep = steps.findIndex(s => s.key === "motDePasse")
        setFormData(prev => ({ ...prev, motDePasse: "", confirmationMotDePasse: "" }))
      } else if (errMsg.includes('date') || errMsg.includes('birth') || errMsg.includes('naissance')) {
        msg = t("errors.dateError")
        goBackToStep = steps.findIndex(s => s.key === "dateNaissance")
        setFormData(prev => ({ ...prev, dateNaissance: "" }))
      }

      addBotMessage(`❌ ${msg}`)
      toast({ title: t("errors.registrationError"), description: msg, variant: "destructive" })

      if (goBackToStep !== null && goBackToStep >= 0) {
        setTimeout(() => {
          setCurrentStep(goBackToStep!)
          setShowPassword(false)
          setTimeout(() => addBotMessage(t(steps[goBackToStep!].questionKey), steps[goBackToStep!].type === "buttons"), 800)
        }, 1500)
      }
    }
  }

  // --- Determine current step UI ---
  const currentStepDef = steps[currentStep]
  const isTextStep = currentStepDef?.type === "text"
  const isButtonStep = currentStepDef?.type === "buttons"
  const currentOptions = isButtonStep ? (currentStepDef.getOptions?.() || []) : []

  // --- Render ---
  return (
    <>
      {/* Initial popup */}
      <AnimatePresence>
        {showInitialPopup && !isOpen && (
          <motion.div
            initial={{ opacity: 0, y: 20, scale: 0.9 }}
            animate={{ opacity: 1, y: 0, scale: 1 }}
            exit={{ opacity: 0, y: 20, scale: 0.9 }}
            className="fixed bottom-24 right-6 z-50 max-w-xs"
          >
            <div className="bg-card rounded-2xl shadow-2xl border border-border p-4 relative">
              <button onClick={() => setShowInitialPopup(false)} className="absolute -top-2 -right-2 bg-muted hover:bg-muted/80 rounded-full p-1 transition-colors">
                <X className="w-4 h-4" />
              </button>
              <div className="flex items-start gap-3">
                <div className="bg-primary rounded-full p-2 flex-shrink-0">
                  <Bot className="w-4 h-4 text-primary-foreground" />
                </div>
                <div className="flex-1">
                  <p className="text-sm font-medium text-foreground mb-2">{t("popup.quickRegistration")}</p>
                  <Button onClick={startConversation} size="sm" className="w-full text-xs">{t("popup.start")}</Button>
                </div>
              </div>
            </div>
          </motion.div>
        )}
      </AnimatePresence>

      {/* Promo popup */}
      <AnimatePresence>
        {showPromoPopup && !isOpen && (
          <motion.div
            initial={{ opacity: 0, y: 20, scale: 0.9 }}
            animate={{ opacity: 1, y: 0, scale: 1 }}
            exit={{ opacity: 0, y: 20, scale: 0.9 }}
            className="fixed bottom-24 right-6 z-50 max-w-xs"
          >
            <div className="bg-card rounded-2xl shadow-2xl border border-border p-4 relative">
              <button onClick={() => setShowPromoPopup(false)} className="absolute -top-2 -right-2 bg-muted hover:bg-muted/80 rounded-full p-1 transition-colors">
                <X className="w-4 h-4" />
              </button>
              <div className="flex items-start gap-3">
                <div className="bg-primary rounded-full p-2 flex-shrink-0">
                  <Bot className="w-4 h-4 text-primary-foreground" />
                </div>
                <div className="flex-1">
                  <p className="text-sm font-medium text-foreground mb-2">{t("popup.promoMessage")}</p>
                  <Button onClick={startConversation} size="sm" className="w-full text-xs">{t("popup.start")}</Button>
                </div>
              </div>
            </div>
          </motion.div>
        )}
      </AnimatePresence>

      {/* Toggle button */}
      <motion.button
        onClick={() => isOpen ? setIsOpen(false) : startConversation()}
        whileHover={{ scale: 1.1 }}
        whileTap={{ scale: 0.9 }}
        className="fixed bottom-6 right-6 z-50 bg-primary hover:bg-primary/90 text-primary-foreground rounded-full p-4 shadow-2xl transition-all duration-300 border-2 border-primary/20"
      >
        {isOpen ? <X className="w-6 h-6" /> : <Bot className="w-6 h-6" />}
      </motion.button>

      {/* Chat window */}
      <AnimatePresence>
        {isOpen && (
          <motion.div
            initial={{ opacity: 0, y: 100, scale: 0.9 }}
            animate={{ opacity: 1, y: 0, scale: 1 }}
            exit={{ opacity: 0, y: 100, scale: 0.9 }}
            className="fixed bottom-24 right-6 z-50 w-80 sm:w-96 h-[28rem] bg-card rounded-2xl shadow-2xl border border-border flex flex-col overflow-hidden"
          >
            {/* Header */}
            <div className="bg-primary text-primary-foreground p-4 flex items-center justify-between">
              <div className="flex items-center gap-3">
                <div className="bg-primary-foreground/20 rounded-full p-2">
                  <Bot className="w-4 h-4" />
                </div>
                <div>
                  <h3 className="font-semibold text-sm">{t("header.title")}</h3>
                  <p className="text-xs text-primary-foreground/80">{t("header.subtitle")}</p>
                </div>
              </div>
              <button onClick={() => setIsOpen(false)} className="hover:bg-primary-foreground/20 rounded-full p-1 transition-colors">
                <X className="w-4 h-4" />
              </button>
            </div>

            {/* Messages */}
            <div className="flex-1 overflow-y-auto p-4 space-y-3">
              {messages.map((message) => (
                <motion.div
                  key={message.id}
                  initial={{ opacity: 0, y: 10 }}
                  animate={{ opacity: 1, y: 0 }}
                  className={`flex ${message.type === "user" ? "justify-end" : "justify-start"}`}
                >
                  <div className={`max-w-[80%] p-3 rounded-2xl text-sm ${
                    message.type === "user"
                      ? "bg-primary text-primary-foreground rounded-br-md"
                      : "bg-muted text-foreground rounded-bl-md"
                  }`}>
                    {message.content}
                  </div>
                </motion.div>
              ))}
              {isTyping && (
                <motion.div initial={{ opacity: 0, y: 10 }} animate={{ opacity: 1, y: 0 }} className="flex justify-start">
                  <div className="bg-muted p-3 rounded-2xl rounded-bl-md">
                    <div className="flex gap-1">
                      <div className="w-2 h-2 bg-muted-foreground/40 rounded-full animate-bounce" style={{ animationDelay: "0ms" }} />
                      <div className="w-2 h-2 bg-muted-foreground/40 rounded-full animate-bounce" style={{ animationDelay: "150ms" }} />
                      <div className="w-2 h-2 bg-muted-foreground/40 rounded-full animate-bounce" style={{ animationDelay: "300ms" }} />
                    </div>
                  </div>
                </motion.div>
              )}
              <div ref={messagesEndRef} />
            </div>

            {/* Button options */}
            {!isCompleted && isButtonStep && !isTyping && showButtons && currentOptions.length > 0 && (
              <div className="flex flex-wrap gap-2 p-3 border-t border-border max-h-40 overflow-y-auto">
                {currentOptions.map((opt) => (
                  <Button
                    key={opt.value}
                    variant="outline"
                    size="sm"
                    onClick={() => handleOptionSelect(opt.value)}
                    className="text-xs hover:bg-primary/10 hover:border-primary/30"
                  >
                    {t(opt.labelKey)}
                  </Button>
                ))}
              </div>
            )}

            {/* Text input */}
            {!isCompleted && isTextStep && !isTyping && (
              <form onSubmit={handleSubmit} className="p-3 border-t border-border">
                <div className="flex gap-2">
                  <div className="relative flex-1">
                    <Input
                      value={currentInput}
                      onChange={(e) => setCurrentInput(e.target.value)}
                      placeholder={t("input.placeholder")}
                      className="flex-1 pr-9"
                      type={currentStepDef.inputType === "password" && !showPassword ? "password" : "text"}
                      disabled={isTyping || isSubmitting}
                      autoFocus
                    />
                    {currentStepDef.inputType === "password" && (
                      <button
                        type="button"
                        onClick={() => setShowPassword(prev => !prev)}
                        className="absolute right-2 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground transition-colors"
                      >
                        {showPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                      </button>
                    )}
                  </div>
                  <Button
                    type="submit"
                    size="sm"
                    disabled={!currentInput.trim() || isTyping || isSubmitting}
                    className="px-3"
                  >
                    <Send className="w-4 h-4" />
                  </Button>
                </div>
                {/* Password strength indicator */}
                {currentStepDef.key === "motDePasse" && currentInput.length > 0 && (() => {
                  const strength = checkPasswordStrength(currentInput)
                  const hasLower = /[a-z]/.test(currentInput)
                  const hasUpper = /[A-Z]/.test(currentInput)
                  const hasDigit = /\d/.test(currentInput)
                  const hasLength = currentInput.length >= 6
                  return (
                    <div className="mt-2 space-y-1.5">
                      <div className="flex items-center gap-2">
                        <div className="flex-1 h-1.5 bg-muted rounded-full overflow-hidden flex gap-0.5">
                          {[0, 1, 2, 3].map(i => (
                            <div
                              key={i}
                              className="flex-1 rounded-full transition-all duration-300"
                              style={{ backgroundColor: i < strength.score ? strength.color : 'hsl(var(--muted))' }}
                            />
                          ))}
                        </div>
                        <span className="text-[10px] font-medium whitespace-nowrap" style={{ color: strength.color }}>
                          {strength.label}
                        </span>
                      </div>
                      <div className="grid grid-cols-2 gap-x-2 gap-y-0.5">
                        <span className={`text-[10px] ${hasLength ? 'text-green-500' : 'text-muted-foreground'}`}>
                          {hasLength ? '✓' : '○'} {t("passwordStrength.chars6")}
                        </span>
                        <span className={`text-[10px] ${hasUpper ? 'text-green-500' : 'text-muted-foreground'}`}>
                          {hasUpper ? '✓' : '○'} {t("passwordStrength.uppercase")}
                        </span>
                        <span className={`text-[10px] ${hasLower ? 'text-green-500' : 'text-muted-foreground'}`}>
                          {hasLower ? '✓' : '○'} {t("passwordStrength.lowercase")}
                        </span>
                        <span className={`text-[10px] ${hasDigit ? 'text-green-500' : 'text-muted-foreground'}`}>
                          {hasDigit ? '✓' : '○'} {t("passwordStrength.digit")}
                        </span>
                      </div>
                    </div>
                  )
                })()}
              </form>
            )}

            {/* Submitting indicator */}
            {isSubmitting && (
              <div className="p-3 border-t border-border text-center text-xs text-muted-foreground">
                {t("input.submitting")}
              </div>
            )}
          </motion.div>
        )}
      </AnimatePresence>
    </>
  )
}

export default ChatBot
