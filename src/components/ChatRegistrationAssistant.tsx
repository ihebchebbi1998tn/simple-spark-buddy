import { useState, useEffect, useRef } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Avatar, AvatarImage, AvatarFallback } from "@/components/ui/avatar";
import { Send, Bot, User, CheckCircle, AlertTriangle, Info, Sparkles, Heart, Trophy } from "lucide-react";
import { useToast } from "@/hooks/use-toast";
import { OPERATION_ACTIVITY_MAPPING, filterExperienceLevelsByGlobal, EXPERIENCE_LEVELS, TUNISIAN_CITIES } from "@/utils/constants";
import { validatePasswordRequirements } from "@/utils/formValidation";

interface FormData {
  // Step 1 - Personal Info (matches CandidateInscriptionForm)
  civilite: string;
  nom: string;
  prenom: string;
  telephone: string;
  villeResidence: string;
  // Experience BEFORE position (same order as normal form)
  experienceGlobale: string;
  posteRecherche: string;
  experiencePosteRecherche: string;

  // Step 2 - Experience (matches DetailedInscriptionForm)
  activitePrincipale: string;
  experienceActivitePrincipale: string;
  operation1: string;
  experienceOperation1: string;
  operation2: string;
  experienceOperation2: string;

  // Step 3 - Languages (matches DetailedInscriptionForm exactly)
  langueMaternelle: string;
  langueForeign1: string;
  niveauLangueForeign1: string;
  hasSecondForeignLanguage: string;
  langueForeign2: string;
  niveauLangueForeign2: string;
  bilingue: string;

  // Step 4 - Work Preferences
  modeTravail: string;
  tempsTravail: string;
  parcTravail: string;

  // Step 5 - Account
  email: string;
  motDePasse: string;
  confirmationMotDePasse: string;
  dateNaissance: string;
  acceptCGU: boolean;
}

interface ChatMessage {
  id: string;
  type: 'bot' | 'user';
  content: string;
  timestamp: Date;
  options?: Array<{ value: string; label: string; disabled?: boolean; emoji?: string }>;
  fieldKey?: string;
  isError?: boolean;
  isWarning?: boolean;
  isInfo?: boolean;
  icon?: React.ReactNode;
}

const ChatRegistrationAssistant = () => {
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [currentInput, setCurrentInput] = useState("");
  const [currentStep, setCurrentStep] = useState(0);
  const [formData, setFormData] = useState<FormData>({
    civilite: "", nom: "", prenom: "", telephone: "", villeResidence: "",
    experienceGlobale: "", posteRecherche: "", experiencePosteRecherche: "",
    activitePrincipale: "", experienceActivitePrincipale: "", operation1: "", experienceOperation1: "",
    operation2: "", experienceOperation2: "",
    langueMaternelle: "", langueForeign1: "", niveauLangueForeign1: "",
    hasSecondForeignLanguage: "", langueForeign2: "", niveauLangueForeign2: "",
    bilingue: "", modeTravail: "", tempsTravail: "", parcTravail: "",
    email: "", motDePasse: "", confirmationMotDePasse: "", dateNaissance: "", acceptCGU: false
  });
  const [isCompleted, setIsCompleted] = useState(false);
  const [isProcessing, setIsProcessing] = useState(false);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const { toast } = useToast();

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  // Business Logic - Same as DetailedInscriptionForm
  const isAgentPosition = formData.posteRecherche === "1";

  // Check if user has minimal experience (no experience or 0-6 months) - matches DetailedInscriptionForm
  const hasMinimalExperience = ["0", "1"].includes(formData.experienceGlobale);

  const isExperienceLessThan3Years = (): boolean => {
    const exp = parseInt(formData.experienceGlobale) || 0;
    return exp <= 4; // 0, 1, 2, 3, 4 = less than 3 years or no experience
  };

  const shouldShowExperiencePoste = (): boolean => {
    // Same as normal form: only show for non-Agent positions with high experience
    const isLowExperience = ["0", "1", "2", "3", "4"].includes(formData.experienceGlobale);
    return !isLowExperience && formData.posteRecherche !== "1" && formData.posteRecherche !== "";
  };

  const shouldShowOperation2 = (): boolean => {
    return formData.operation1 !== "" && formData.experienceOperation1 !== "";
  };

  // Get filtered operations based on selected activity - matches DetailedInscriptionForm
  const getFilteredOperations = () => {
    const allOperations = [
      { value: "1", label: "Énergie Électricité & Gaz", emoji: "⚡" },
      { value: "2", label: "Photovoltaïque", emoji: "☀️" },
      { value: "3", label: "Télécommunication", emoji: "📡" },
      { value: "4", label: "Mutuelle/Assurance", emoji: "💚" },
      { value: "5", label: "Voyance", emoji: "🔮" },
      { value: "6", label: "Compléments alimentaires", emoji: "🍃" },
      { value: "7", label: "Sites web & Apps", emoji: "💻" },
      { value: "8", label: "Télésecretariat", emoji: "📋" },
      { value: "9", label: "Dons humanitaires", emoji: "❤️" },
      { value: "10", label: "Qualification fichiers", emoji: "📊" },
      { value: "11", label: "Jeux/Concours", emoji: "🎮" },
      { value: "12", label: "Tourisme médical", emoji: "✈️" },
      { value: "13", label: "Dossiers administratifs", emoji: "📁" },
      { value: "14", label: "CPF/Formations", emoji: "🎓" },
      { value: "15", label: "Gestion commandes", emoji: "📦" },
      { value: "16", label: "Modération web", emoji: "🛡️" },
      { value: "17", label: "Réservations", emoji: "🏨" }
    ];
    
    if (!formData.activitePrincipale) return allOperations;
    
    return allOperations.filter(op => {
      const allowedActivities = OPERATION_ACTIVITY_MAPPING[op.value];
      return allowedActivities?.includes(formData.activitePrincipale);
    });
  };

  // Experience to months - MATCHES DetailedInscriptionForm exactly
  const experienceToMonths = (value: string): number => {
    const mapping: Record<string, number> = {
      "1": 6,    // 0-6 mois
      "2": 12,   // 6-12 mois
      "3": 24,   // 1-2 ans
      "4": 36,   // 2-3 ans
      "5": 60,   // 3-5 ans
      "6": 84,   // 5-7 ans
      "7": 120,  // Plus de 7 ans
    };
    return mapping[value] || 0;
  };

  // Get filtered experience options for position (capped by global experience)
  const getFilteredPositionExperience = () => {
    const allOptions = [
      { value: "1", label: "0 - 6 mois", emoji: "🌱" },
      { value: "2", label: "6 mois - 12 mois", emoji: "🌿" },
      { value: "3", label: "1 an - 2 ans", emoji: "🌳" },
      { value: "4", label: "2 ans - 3 ans", emoji: "🎯" },
      { value: "5", label: "3 ans - 5 ans", emoji: "⭐" },
      { value: "6", label: "5 ans - 7 ans", emoji: "🏆" },
      { value: "7", label: "Plus de 7 ans", emoji: "👑" }
    ];
    
    if (!formData.experienceGlobale) return allOptions;
    
    const filtered = filterExperienceLevelsByGlobal(
      formData.experienceGlobale,
      EXPERIENCE_LEVELS.map(exp => ({ value: exp.value, label: exp.label }))
    );
    
    return allOptions.filter(opt => filtered.some(f => f.value === opt.value));
  };

  // Get filtered experience options for activity (capped by global experience)
  const getFilteredActivityExperience = () => {
    const allOptions = [
      { value: "1", label: "0 - 6 mois", emoji: "🌱" },
      { value: "2", label: "6 mois - 12 mois", emoji: "🌿" },
      { value: "3", label: "1 an - 2 ans", emoji: "🌳" },
      { value: "4", label: "2 ans - 3 ans", emoji: "🎯" },
      { value: "5", label: "3 ans - 5 ans", emoji: "⭐" },
      { value: "6", label: "5 ans - 7 ans", emoji: "🏆" },
      { value: "7", label: "Plus de 7 ans", emoji: "👑" }
    ];
    
    if (!formData.experienceGlobale) return allOptions;
    
    const filtered = filterExperienceLevelsByGlobal(
      formData.experienceGlobale,
      EXPERIENCE_LEVELS.map(exp => ({ value: exp.value, label: exp.label }))
    );
    
    return allOptions.filter(opt => filtered.some(f => f.value === opt.value));
  };

  // Get filtered experience options for operation (capped by activity experience budget) - matches DetailedInscriptionForm
  const getFilteredOperationExperience = (isOperation2: boolean = false) => {
    const allOptions = [
      { value: "1", label: "0 - 6 mois", emoji: "🌱" },
      { value: "2", label: "6 mois - 12 mois", emoji: "🌿" },
      { value: "3", label: "1 an - 2 ans", emoji: "🌳" },
      { value: "4", label: "2 ans - 3 ans", emoji: "🎯" },
      { value: "5", label: "3 ans - 5 ans", emoji: "⭐" },
      { value: "6", label: "5 ans - 7 ans", emoji: "🏆" },
      { value: "7", label: "Plus de 7 ans", emoji: "👑" }
    ];
    
    if (!formData.experienceActivitePrincipale) return allOptions;
    
    const activityBudget = experienceToMonths(formData.experienceActivitePrincipale);
    const usedByOther = isOperation2 
      ? experienceToMonths(formData.experienceOperation1 || "0")
      : experienceToMonths(formData.experienceOperation2 || "0");
    const remainingBudget = activityBudget - usedByOther;
    
    return allOptions.filter(opt => experienceToMonths(opt.value) <= remainingBudget);
  };

  // Tunisian cities as options for chat
  const tunisianCitiesOptions = TUNISIAN_CITIES.map(city => ({
    value: city.value,
    label: city.label,
    emoji: "📍"
  }));

  // Language options matching normal form's LANGUAGES constant
  const languageOptions = [
    { value: "Français", label: "Français", emoji: "🇫🇷" },
    { value: "Arabe", label: "Arabe", emoji: "🇹🇳" },
    { value: "Anglais", label: "Anglais", emoji: "🇬🇧" },
    { value: "Espagnol", label: "Espagnol", emoji: "🇪🇸" },
    { value: "Allemand", label: "Allemand", emoji: "🇩🇪" },
    { value: "Italien", label: "Italien", emoji: "🇮🇹" },
    { value: "Portugais", label: "Portugais", emoji: "🇵🇹" },
    { value: "Néerlandais", label: "Néerlandais", emoji: "🇳🇱" },
    { value: "Turc", label: "Turc", emoji: "🇹🇷" },
    { value: "Russe", label: "Russe", emoji: "🇷🇺" },
    { value: "Chinois", label: "Chinois", emoji: "🇨🇳" },
  ];

  // Language level options matching normal form's LANGUAGE_LEVELS constant
  const languageLevelOptions = [
    { value: "debutant", label: "Débutant", emoji: "🌱" },
    { value: "elementaire", label: "Élémentaire", emoji: "📖" },
    { value: "intermediaire", label: "Intermédiaire", emoji: "🌿" },
    { value: "avance", label: "Avancé", emoji: "⭐" },
    { value: "courant", label: "Courant", emoji: "🏆" },
    { value: "natif", label: "Natif", emoji: "👑" }
  ];

  // Chat Flow Configuration - Matches CandidateInscriptionForm & DetailedInscriptionForm ORDER
  const chatSteps: Array<{
    key: string;
    question: string;
    type: 'confirmation' | 'cards' | 'text' | 'verification';
    options?: Array<{ value: string; label: string; emoji?: string; disabled?: boolean }>;
    getDynamicOptions?: () => Array<{ value: string; label: string; emoji?: string; disabled?: boolean }>;
    getCustomMessage?: () => string;
    shouldShow?: () => boolean;
    validation?: (value: string) => string | null;
  }> = [
    {
      key: "welcome",
      question: "Bonjour et bienvenue ! 👋 Je suis votre assistant personnel d'inscription. Je vais vous accompagner pour créer votre profil professionnel en seulement quelques minutes. C'est parti ?",
      type: "confirmation",
      options: [
        { value: "oui", label: "Oui, allons-y ! 🚀" },
        { value: "info", label: "J'aimerais plus d'informations 💡" }
      ]
    },
    // === STEP 1: Personal Info (same as CandidateInscriptionForm) ===
    {
      key: "civilite",
      question: "Parfait ! 🌟 Comment souhaitez-vous être appelé(e) ?",
      type: "cards",
      options: [
        { value: "madame", label: "👩 Madame" },
        { value: "monsieur", label: "👨 Monsieur" }
      ]
    },
    {
      key: "nom",
      question: "Enchanté(e) ! 💫 Quel est votre nom de famille ?",
      type: "text",
      validation: (value: string) => value.trim().length >= 2 ? null : "Le nom doit contenir au moins 2 caractères"
    },
    {
      key: "prenom",
      question: "Parfait ! 👤 Et votre prénom ?",
      type: "text",
      validation: (value: string) => value.trim().length >= 2 ? null : "Le prénom doit contenir au moins 2 caractères"
    },
    {
      key: "telephone",
      question: "Super ! 📱 Votre numéro de téléphone tunisien (8 chiffres) ?",
      type: "text",
      validation: (value: string) => {
        const cleaned = value.replace(/\D/g, '');
        if (cleaned.length !== 8) return "Le numéro doit contenir exactement 8 chiffres";
        const validPrefixes = ['2', '3', '4', '5', '7', '9'];
        if (!validPrefixes.includes(cleaned[0])) return "Numéro tunisien invalide. Doit commencer par 2, 3, 4, 5, 7 ou 9";
        return null;
      }
    },
    {
      key: "villeResidence",
      question: "Génial ! 🏡 Dans quelle ville résidez-vous ?",
      type: "cards",
      options: tunisianCitiesOptions
    },
    // Experience BEFORE position (same order as normal form)
    {
      key: "experienceGlobale",
      question: "Parlons de votre carrière ! 💼 Quelle est votre expérience globale dans les centres d'appels ?",
      type: "cards",
      options: [
        { value: "0", label: "Aucune expérience", emoji: "🆕" },
        { value: "1", label: "0 - 6 mois", emoji: "🌱" },
        { value: "2", label: "6 mois - 12 mois", emoji: "🌿" },
        { value: "3", label: "1 an - 2 ans", emoji: "🌳" },
        { value: "4", label: "2 ans - 3 ans", emoji: "🎯" },
        { value: "5", label: "3 ans - 5 ans", emoji: "⭐" },
        { value: "6", label: "5 ans - 7 ans", emoji: "🏆" },
        { value: "7", label: "Plus de 7 ans", emoji: "👑" }
      ]
    },
    {
      key: "posteRecherche",
      question: "Excellent ! 🌟 Quel poste recherchez-vous ?",
      type: "cards",
      getDynamicOptions: () => {
        if (isExperienceLessThan3Years()) {
          return [{ value: "1", label: "Agent", emoji: "🎧" }];
        }
        return [
          { value: "1", label: "Agent", emoji: "🎧" },
          { value: "2", label: "Team Leader", emoji: "👨‍💼" },
          { value: "3", label: "Responsable Activité", emoji: "📊" },
          { value: "4", label: "Responsable Qualité", emoji: "✅" },
          { value: "5", label: "Responsable Service client", emoji: "🤝" },
          { value: "6", label: "Responsable Recrutement", emoji: "👥" },
          { value: "7", label: "Responsable Plateau", emoji: "🏢" },
          { value: "8", label: "Formateur", emoji: "📚" },
          { value: "9", label: "Chargé Recrutement", emoji: "🎯" }
        ];
      },
      getCustomMessage: () => {
        if (isExperienceLessThan3Years()) {
          return "Avec votre profil actuel, le poste d'Agent est parfait pour démarrer ! 💪✨";
        }
        return "Avec votre expérience, plusieurs postes s'offrent à vous ! 🚀";
      }
    },
    {
      key: "experiencePosteRecherche",
      question: "Parfait ! ✅ Quelle est votre expérience spécifique pour ce poste ?",
      type: "cards",
      shouldShow: () => shouldShowExperiencePoste(),
      getDynamicOptions: () => getFilteredPositionExperience()
    },
    // === STEP 2: Experience (matches DetailedInscriptionForm) ===
    {
      key: "activitePrincipale",
      question: "Formidable ! 💫 Quelle activité principale vous passionne ?",
      type: "cards",
      options: [
        { value: "1", label: "Télévente", emoji: "📞" },
        { value: "2", label: "Téléprospection", emoji: "🔍" },
        { value: "3", label: "Prise de RDV", emoji: "📅" },
        { value: "4", label: "Service client", emoji: "😊" }
      ]
    },
    {
      key: "experienceActivitePrincipale",
      question: "Super choix ! ✅ Quelle est votre expérience dans cette activité ?",
      type: "cards",
      shouldShow: () => !hasMinimalExperience,
      getDynamicOptions: () => getFilteredActivityExperience()
    },
    {
      key: "operation1",
      question: "Génial ! 🎪 Dans quel secteur souhaitez-vous travailler ?",
      type: "cards",
      getDynamicOptions: () => getFilteredOperations()
    },
    {
      key: "experienceOperation1",
      question: "Excellent ! 🚀 Quelle est votre expérience dans ce secteur ?",
      type: "cards",
      shouldShow: () => !hasMinimalExperience,
      getDynamicOptions: () => getFilteredOperationExperience(false)
    },
    {
      key: "operation2",
      question: "Bravo ! 🌟 Souhaitez-vous ajouter un second secteur ? (Optionnel)",
      type: "cards",
      shouldShow: () => shouldShowOperation2() || (hasMinimalExperience && formData.operation1 !== ""),
      getDynamicOptions: () => {
        const filtered = getFilteredOperations().filter(op => op.value !== formData.operation1);
        return [{ value: "", label: "Non, je passe", emoji: "⏭️" }, ...filtered];
      }
    },
    {
      key: "experienceOperation2",
      question: "Parfait ! 💪 Votre expérience dans ce second secteur ?",
      type: "cards",
      shouldShow: () => formData.operation2 !== "" && !hasMinimalExperience,
      getDynamicOptions: () => getFilteredOperationExperience(true)
    },
    // === STEP 3: Languages (matches DetailedInscriptionForm exactly) ===
    {
      key: "langueMaternelle",
      question: "Impressionnant ! 🌍 Quelle est votre langue maternelle ?",
      type: "cards",
      options: languageOptions
    },
    {
      key: "langueForeign1",
      question: "Super ! 🗣️ Quelle est votre première langue étrangère ?",
      type: "cards",
      getDynamicOptions: () => languageOptions.filter(l => l.value !== formData.langueMaternelle)
    },
    {
      key: "niveauLangueForeign1",
      question: "Parfait ! ✅ Quel est votre niveau dans cette langue ?",
      type: "cards",
      options: languageLevelOptions
    },
    {
      key: "hasSecondForeignLanguage",
      question: "Formidable ! 💫 Parlez-vous une seconde langue étrangère ?",
      type: "cards",
      options: [
        { value: "oui", label: "Oui", emoji: "🌟" },
        { value: "non", label: "Non", emoji: "⏭️" }
      ]
    },
    {
      key: "langueForeign2",
      question: "Génial ! 🌐 Quelle est cette seconde langue ?",
      type: "cards",
      shouldShow: () => formData.hasSecondForeignLanguage === "oui",
      getDynamicOptions: () => languageOptions.filter(l => l.value !== formData.langueMaternelle && l.value !== formData.langueForeign1)
    },
    {
      key: "niveauLangueForeign2",
      question: "Excellent ! 🌟 Votre niveau dans cette seconde langue ?",
      type: "cards",
      shouldShow: () => formData.hasSecondForeignLanguage === "oui" && formData.langueForeign2 !== "",
      options: languageLevelOptions
    },
    {
      key: "bilingue",
      question: "Génial ! 🎉 Vous considérez-vous comme bilingue ?",
      type: "cards",
      options: [
        { value: "oui", label: "Oui", emoji: "🌟" },
        { value: "non", label: "Non", emoji: "🎯" }
      ]
    },
    // === STEP 4: Work Preferences ===
    {
      key: "modeTravail",
      question: "Parfait ! 🏢 Préférence de lieu de travail ?",
      type: "cards",
      options: [
        { value: "presentiel", label: "En présentiel", emoji: "🏢" },
        { value: "teletravail", label: "Télétravail", emoji: "💻" },
        { value: "peu_importe", label: "Peu importe", emoji: "🤝" }
      ]
    },
    {
      key: "tempsTravail",
      question: "Super ! ⏰ Type de contrat souhaité ?",
      type: "cards",
      options: [
        { value: "plein_temps", label: "Plein temps", emoji: "🎯" },
        { value: "mi_temps", label: "Mi-temps", emoji: "⏳" },
        { value: "peu_importe", label: "Peu importe", emoji: "🤝" }
      ]
    },
    {
      key: "parcTravail",
      question: "Excellent ! ☀️ Parc de travail préféré ?",
      type: "cards",
      options: [
        { value: "jour", label: "Parc de jour", emoji: "☀️" },
        { value: "nuit", label: "Parc de nuit", emoji: "🌙" },
        { value: "peu_importe", label: "Peu importe", emoji: "🤝" }
      ]
    },
    // === STEP 5: Account (matches DetailedInscriptionForm) ===
    {
      key: "email",
      question: "Presque terminé ! 📧 Votre adresse email ?",
      type: "text",
      validation: (value: string) => {
        if (!value.trim()) return "L'email est obligatoire";
        if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) return "Format invalide (ex: nom@exemple.com)";
        return null;
      }
    },
    {
      key: "motDePasse",
      question: "Sécurisons votre compte ! 🔐 Créez un mot de passe (min. 6 caractères, une majuscule, une minuscule, un chiffre) :",
      type: "text",
      validation: (value: string) => {
        const { isValid, errors } = validatePasswordRequirements(value);
        if (!isValid) return errors.join(', ');
        return null;
      }
    },
    {
      key: "confirmationMotDePasse",
      question: "Confirmez votre mot de passe 🔒 :",
      type: "text",
      validation: (value: string) => value === formData.motDePasse ? null : "Les mots de passe ne correspondent pas"
    },
    {
      key: "dateNaissance",
      question: "📅 Quelle est votre date de naissance ? (Format: JJ/MM/AAAA)",
      type: "text",
      validation: (value: string) => {
        // Accept various date formats
        const dateRegex = /^(\d{2})[\/\-](\d{2})[\/\-](\d{4})$/;
        const isoRegex = /^\d{4}-\d{2}-\d{2}$/;
        
        if (isoRegex.test(value)) {
          const date = new Date(value);
          if (isNaN(date.getTime())) return "Date invalide";
          const age = Math.floor((Date.now() - date.getTime()) / (365.25 * 24 * 60 * 60 * 1000));
          if (age < 16) return "Vous devez avoir au moins 16 ans";
          if (age > 100) return "Date invalide";
          return null;
        }
        
        const match = value.match(dateRegex);
        if (!match) return "Format attendu: JJ/MM/AAAA ou AAAA-MM-JJ";
        
        const [, day, month, year] = match;
        const date = new Date(`${year}-${month}-${day}`);
        if (isNaN(date.getTime())) return "Date invalide";
        
        const age = Math.floor((Date.now() - date.getTime()) / (365.25 * 24 * 60 * 60 * 1000));
        if (age < 16) return "Vous devez avoir au moins 16 ans";
        if (age > 100) return "Date invalide";
        return null;
      }
    },
    {
      key: "acceptCGU",
      question: "📋 En finalisant votre inscription, vous acceptez nos Conditions Générales d'Utilisation et notre Politique de Confidentialité. Souhaitez-vous continuer ?",
      type: "confirmation",
      options: [
        { value: "oui", label: "J'accepte et je m'inscris ✅" },
        { value: "non", label: "Je refuse ❌" }
      ]
    },
  ];

  // Initialize chat with welcome message
  useEffect(() => {
    if (messages.length === 0) {
      addBotMessage(chatSteps[0].question, chatSteps[0].options, chatSteps[0].key);
    }
  }, []);

  const addBotMessage = (content: string, options?: Array<{ value: string; label: string; disabled?: boolean }>, fieldKey?: string, isError?: boolean, isWarning?: boolean, isInfo?: boolean, icon?: React.ReactNode) => {
    const newMessage: ChatMessage = {
      id: Date.now().toString(),
      type: 'bot',
      content,
      timestamp: new Date(),
      options,
      fieldKey,
      isError,
      isWarning,
      isInfo,
      icon
    };
    setMessages(prev => [...prev, newMessage]);
  };

  const addUserMessage = (content: string) => {
    const newMessage: ChatMessage = {
      id: Date.now().toString(),
      type: 'user',
      content,
      timestamp: new Date()
    };
    setMessages(prev => [...prev, newMessage]);
  };

  const handleOptionSelect = (value: string, fieldKey?: string) => {
    if (isProcessing) return;
    setIsProcessing(true);
    const step = chatSteps[currentStep];
    const allOptions = step.getDynamicOptions?.() || step.options || [];
    const selectedOption = allOptions.find(opt => opt.value === value);

    addUserMessage(selectedOption?.label || value);

    if (fieldKey) {
      const newFormData = { ...formData, [fieldKey]: value };
      setFormData(newFormData);

      // Check if this is an operation experience field and validate against activity experience
      if ((fieldKey === 'experienceOperation1' || fieldKey === 'experienceOperation2') && newFormData.experienceActivitePrincipale) {
        const activityBudget = experienceToMonths(newFormData.experienceActivitePrincipale);
        let totalUsed = 0;
        if (newFormData.experienceOperation1) {
          totalUsed += experienceToMonths(newFormData.experienceOperation1);
        }
        if (newFormData.experienceOperation2) {
          totalUsed += experienceToMonths(newFormData.experienceOperation2);
        }
        
        if (totalUsed > activityBudget) {
          addBotMessage(
            "⚠️ Attention ! L'expérience dans une opération ne peut pas être supérieure à celle de l'activité choisie. Veuillez choisir une expérience inférieure ou égale.",
            step.getDynamicOptions?.() || step.options,
            fieldKey,
            false, true, false, <AlertTriangle className="h-4 w-4" />
          );
          setFormData(prev => ({ ...prev, [fieldKey]: '' }));
          setIsProcessing(false);
          return;
        }
      }
    }

    // Handle special cases
    if (step.key === "welcome") {
      if (value === "info") {
        addBotMessage(
          "📋 Je vais vous poser quelques questions sur votre expérience professionnelle, vos compétences linguistiques et vos préférences de travail. Cela ne prendra que 5 minutes ! Prêt(e) ?",
          [{ value: "oui", label: "Oui, c'est parti !" }],
          undefined, false, false, true, <Info className="h-4 w-4" />
        );
        setIsProcessing(false);
        return;
      }
    }

    // Auto-assign Agent position for low experience (same as normal form)
    if (step.key === "experienceGlobale") {
      if (["0", "1", "2", "3", "4"].includes(value)) {
        setFormData(prev => ({ ...prev, experienceGlobale: value, posteRecherche: "1" }));
      }
      // Auto-set experience fields for minimal experience users
      if (["0", "1"].includes(value)) {
        setFormData(prev => ({
          ...prev,
          experienceGlobale: value,
          posteRecherche: "1",
          experienceActivitePrincipale: "1",
          experienceOperation1: "1",
          experienceOperation2: "1",
        }));
      }
      
      if (parseInt(value) <= 4) {
        setTimeout(() => {
          addBotMessage(
            "🎯 Excellent ! Avec votre profil, je vais vous orienter vers les meilleures opportunités pour commencer votre carrière !",
            undefined, undefined, false, false, true, <Sparkles className="h-4 w-4" />
          );
        }, 1000);
      } else {
        setTimeout(() => {
          addBotMessage(
            "🏆 Waouh ! Votre expérience ouvre de nombreuses portes ! Vous avez accès à des postes de responsabilité !",
            undefined, undefined, false, false, true, <Trophy className="h-4 w-4" />
          );
        }, 1000);
      }
    }

    // Handle CGU acceptance
    if (step.key === "acceptCGU") {
      if (value === "oui") {
        setFormData(prev => ({ ...prev, acceptCGU: true }));
        setIsCompleted(true);
        // Pass acceptCGU=true directly to avoid stale closure (React state is async)
        submitRegistration(true);
        setIsProcessing(false);
        return;
      } else {
        addBotMessage(
          "Nous comprenons. L'acceptation des CGU est nécessaire pour finaliser votre inscription. Souhaitez-vous reconsidérer ?",
          [
            { value: "oui", label: "J'accepte et je m'inscris ✅" },
            { value: "non", label: "Je refuse ❌" }
          ],
          "acceptCGU",
          false, true, false, <Info className="h-4 w-4" />
        );
        setIsProcessing(false);
        return;
      }
    }

    // Move to next step
    setTimeout(() => {
      moveToNextStep();
      setIsProcessing(false);
    }, 500);
  };

  const handleTextSubmit = async () => {
    if (!currentInput.trim() || isProcessing) return;
    setIsProcessing(true);

    const step = chatSteps[currentStep];
    const value = currentInput.trim();

    // Validate input
    if (step.validation) {
      const error = step.validation(value);
      if (error) {
        addBotMessage(
          `❌ ${error}. Pouvez-vous réessayer ?`,
          undefined, undefined, true, false, false, <AlertTriangle className="h-4 w-4" />
        );
        setCurrentInput("");
        setIsProcessing(false);
        return;
      }
    }

    addUserMessage(value);
    setCurrentInput("");

    // Check phone availability (same as normal form)
    if (step.key === "telephone") {
      try {
        const { candidateService } = await import('@/services/candidateService');
        const { formatPhoneForBackend } = await import('@/utils/formValidation');
        const formattedPhone = formatPhoneForBackend(value.replace(/\D/g, ''));
        const result = await candidateService.checkAvailability({ phone: formattedPhone });
        if (result.data.phoneExists) {
          addBotMessage(
            "❌ Ce numéro de téléphone est déjà associé à un compte. Veuillez vous connecter ou utiliser un autre numéro.",
            undefined, undefined, true, false, false, <AlertTriangle className="h-4 w-4" />
          );
          setIsProcessing(false);
          return;
        }
      } catch (error) {
        console.error('Error checking phone availability:', error);
        // Continue even if check fails (server might be down)
      }
    }

    // Check email availability (same as normal form)
    if (step.key === "email") {
      try {
        const { candidateService } = await import('@/services/candidateService');
        const result = await candidateService.checkAvailability({ email: value });
        if (result.data.emailExists) {
          addBotMessage(
            "❌ Cette adresse e-mail est déjà enregistrée. Veuillez vous connecter ou utiliser une autre adresse.",
            undefined, undefined, true, false, false, <AlertTriangle className="h-4 w-4" />
          );
          setIsProcessing(false);
          return;
        }
      } catch (error) {
        console.error('Error checking email availability:', error);
      }
    }

    if (step.key) {
      setFormData(prev => ({ ...prev, [step.key as keyof FormData]: value }));
    }

    // Add positive feedback
    const encouragements = [
      "Parfait ! ✨", "Excellent travail ! 👍", "Très bien joué ! ✅",
      "Super réponse ! 🌟", "Merci beaucoup ! 💫", "Génial ! ⭐",
      "Formidable ! 🚀", "Bravo ! 👏", "C'est parfait ! 💎",
    ];
    const randomEncouragement = encouragements[Math.floor(Math.random() * encouragements.length)];

    setTimeout(() => {
      addBotMessage(
        randomEncouragement,
        undefined, undefined, false, false, false, <Heart className="h-4 w-4" />
      );
      setTimeout(() => {
        moveToNextStep();
        setIsProcessing(false);
      }, 800);
    }, 300);
  };

  const moveToNextStep = () => {
    let nextStepIndex = currentStep + 1;

    // Skip steps based on business logic
    while (nextStepIndex < chatSteps.length) {
      const nextStep = chatSteps[nextStepIndex];

      if (nextStep.shouldShow && !nextStep.shouldShow()) {
        nextStepIndex++;
        continue;
      }

      break;
    }

    if (nextStepIndex >= chatSteps.length) {
      setIsCompleted(true);
      addBotMessage(
        "🎉 Félicitations ! Votre profil est maintenant complet. Merci pour votre inscription !",
        undefined, undefined, false, false, false, <Trophy className="h-4 w-4" />
      );
      return;
    }

    setCurrentStep(nextStepIndex);
    const nextStep = chatSteps[nextStepIndex];

    // Add custom message if available
    const customMessage = nextStep.getCustomMessage?.();
    if (customMessage) {
      addBotMessage(
        customMessage,
        undefined, undefined, false, true, false, <Info className="h-4 w-4" />
      );
      setTimeout(() => {
        const options = nextStep.getDynamicOptions?.() || nextStep.options;
        addBotMessage(nextStep.question, options, nextStep.key);
      }, 2000);
    } else {
      setTimeout(() => {
        const options = nextStep.getDynamicOptions?.() || nextStep.options;
        addBotMessage(nextStep.question, options, nextStep.key);
      }, 800);
    }
  };

  // Submit registration to backend API - MATCHES mapToBackendFormat exactly
  const submitRegistration = async (forceAcceptCGU: boolean = false) => {
    try {
      console.log('📤 ChatBot - Submitting registration to backend');

      const { leadService } = await import('@/services/leadService');
      const { authService } = await import('@/services/authService');
      const { sanitizeObject } = await import('@/utils/sanitization');

      // Build combined data matching the normal form's structure
      const combinedData = {
        // Contact Information (same as CandidateInscriptionForm)
        civilite: formData.civilite,
        nom: formData.nom,
        prenom: formData.prenom,
        telephone: formData.telephone,
        villeResidence: formData.villeResidence,
        email: formData.email,
        motDePasse: formData.motDePasse,

        // Profile/CV Information
        posteRecherche: formData.posteRecherche || "1",
        experienceGlobale: formData.experienceGlobale,
        experiencePosteRecherche: formData.experiencePosteRecherche || "",
        activitePrincipale: formData.activitePrincipale,
        experienceActivitePrincipale: formData.experienceActivitePrincipale,
        operation1: formData.operation1,
        experienceOperation1: formData.experienceOperation1,
        operation2: formData.operation2,
        experienceOperation2: formData.experienceOperation2,

        // Languages (matching DetailedInscriptionForm field names)
        langueMaternelle: formData.langueMaternelle,
        langueForeign1: formData.langueForeign1,
        niveauLangueForeign1: formData.niveauLangueForeign1,
        hasSecondForeignLanguage: formData.hasSecondForeignLanguage || "non",
        langueForeign2: formData.langueForeign2,
        niveauLangueForeign2: formData.niveauLangueForeign2,
        bilingue: formData.bilingue,

        // Work Preferences
        modeTravail: formData.modeTravail,
        tempsTravail: formData.tempsTravail,
        parcTravail: formData.parcTravail,

        // Date of birth - convert DD/MM/YYYY to YYYY-MM-DD if needed
        dateNaissance: (() => {
          const dob = formData.dateNaissance;
          if (!dob) return null;
          const match = dob.match(/^(\d{2})[\/\-](\d{2})[\/\-](\d{4})$/);
          if (match) return `${match[3]}-${match[2]}-${match[1]}`;
          return dob; // already ISO format
        })(),

        // No test scores for chatbot registrations
        testLanguage: null,
        testScore: null,

        // CGU
        acceptCGU: forceAcceptCGU || formData.acceptCGU,
      };

      console.log('📤 ChatBot - Combined data (before sanitization):', {
        ...combinedData,
        motDePasse: '[REDACTED]'
      });

      // Sanitize all string inputs (same as normal form)
      const sanitizedData: any = sanitizeObject(combinedData);

      // Set registration source
      sanitizedData.registrationSource = 'chatbot';

      console.log('📤 ChatBot - Final backend payload:', {
        ...sanitizedData,
        motDePasse: '[REDACTED]'
      });

      // Call registration API (same endpoint as normal form)
      const response = await leadService.register(sanitizedData);

      if (response.success || response.message?.includes('success')) {
        const hasToken = !!response.data?.token;
        
        // Store authentication token and user data (same as normal form)
        if (hasToken) {
          authService.storeAuthToken(response.data!.token);
          authService.storeUserData({ lead_id: response.data!.lead_id, email: response.data!.email, name: response.data!.last_name, surname: response.data!.first_name });
        }

        console.log('✅ ChatBot - Registration successful', hasToken ? 'with token' : 'pending activation');

        addBotMessage(
          hasToken 
            ? "🏆 Parfait ! Votre inscription est enregistrée avec succès. Bienvenue ! Redirection vers votre tableau de bord..."
            : "🏆 Parfait ! Votre inscription est enregistrée avec succès. Votre compte est en cours d'activation. Redirection vers la page de connexion...",
          undefined, undefined, false, false, false, <Trophy className="h-4 w-4" />
        );

        toast({
          title: "Inscription réussie ! 🎉",
          description: hasToken ? "Bienvenue ! Votre profil a été créé avec succès." : "Votre compte est en cours d'activation. Veuillez vous connecter.",
        });

        // Redirect to dashboard if token exists, otherwise to login page
        setTimeout(() => {
          window.location.href = hasToken ? '/dashboard' : '/espace-candidats';
        }, 3000);
      }
    } catch (error: any) {
      console.error('❌ ChatBot - Registration error:', error);

      let errorMessage = "Une erreur s'est produite lors de l'inscription.";

      if (error.message?.includes('Password must contain') || error.message?.includes('password')) {
        errorMessage = "Le mot de passe doit contenir au moins une majuscule, une minuscule et un chiffre. Exemple : Azerty123";
      } else if (error.message?.includes('Email already exists') || error.message?.includes('email déjà')) {
        errorMessage = "Cette adresse e-mail est déjà enregistrée. Veuillez vous connecter.";
      } else if (error.message?.includes('Invalid phone')) {
        errorMessage = "Le numéro de téléphone est invalide. Format attendu: 8 chiffres.";
      } else if (error.message?.includes('serveur')) {
        errorMessage = "Impossible de se connecter au serveur. Veuillez démarrer le backend.";
      }

      setIsCompleted(false);

      addBotMessage(
        `❌ ${errorMessage} Voulez-vous réessayer ?`,
        [
          { value: "oui", label: "Réessayer l'inscription" }
        ],
        undefined, true, false, false, <AlertTriangle className="h-4 w-4" />
      );

      toast({
        title: "Erreur d'inscription",
        description: errorMessage,
        variant: "destructive",
      });
    }
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      e.preventDefault();
      handleTextSubmit();
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-primary/5 via-background to-primary/10 p-4">
      <div className="max-w-4xl mx-auto">
        <Card className="h-[80vh] flex flex-col bg-card/80 backdrop-blur-sm border-primary/20">
          <div className="p-4 border-b border-primary/10 bg-primary/5">
            <div className="flex items-center gap-3">
              <Avatar className="h-10 w-10">
                <AvatarImage src="/recruiter-professional.png" />
                <AvatarFallback className="bg-primary text-primary-foreground">
                  <Bot className="h-5 w-5" />
                </AvatarFallback>
              </Avatar>
              <div>
                <h3 className="font-semibold text-primary">Assistant d'Inscription</h3>
                <p className="text-sm text-muted-foreground">Inscription rapide et personnalisée</p>
              </div>
              <div className="ml-auto flex items-center gap-2">
                <Badge variant="secondary" className="bg-green-100 text-green-700">
                  En ligne
                </Badge>
                <Sparkles className="h-4 w-4 text-primary animate-pulse" />
              </div>
            </div>
          </div>

          <div className="flex-1 overflow-y-auto p-4 space-y-4">
            <AnimatePresence>
              {messages.map((message) => (
                <motion.div
                  key={message.id}
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  exit={{ opacity: 0, y: -20 }}
                  className={`flex gap-3 ${message.type === 'user' ? 'justify-end' : 'justify-start'}`}
                >
                  {message.type === 'bot' && (
                    <Avatar className="h-8 w-8 flex-shrink-0">
                      <AvatarImage src="/recruiter-professional.png" />
                      <AvatarFallback className="bg-primary text-primary-foreground text-xs">
                        <Bot className="h-4 w-4" />
                      </AvatarFallback>
                    </Avatar>
                  )}

                  <div className={`max-w-[80%] ${message.type === 'user' ? 'order-first' : ''}`}>
                    <div className={`p-3 rounded-lg ${message.type === 'user'
                        ? 'bg-primary text-primary-foreground ml-auto'
                        : message.isError
                          ? 'bg-red-50 border border-red-200 text-red-800'
                          : message.isWarning
                            ? 'bg-orange-50 border border-orange-200 text-orange-800'
                            : message.isInfo
                              ? 'bg-blue-50 border border-blue-200 text-blue-800'
                              : 'bg-muted'
                      }`}>
                      <div className="flex items-start gap-2">
                        {message.icon && (
                          <div className="flex-shrink-0 mt-0.5">
                            {message.icon}
                          </div>
                        )}
                        <p className="text-sm">{message.content}</p>
                      </div>
                    </div>

                    {message.options && message.type === 'bot' && (
                      <div className="mt-3">
                        {messages[messages.length - 1]?.id === message.id ? (
                          <>
                            {chatSteps[currentStep]?.type === 'cards' ? (
                              <div className="grid grid-cols-2 lg:grid-cols-3 gap-2">
                                {message.options.map((option) => (
                                  <motion.div
                                    key={option.value}
                                    initial={{ opacity: 0, y: 10 }}
                                    animate={{ opacity: 1, y: 0 }}
                                    transition={{ delay: 0.5 }}
                                    whileHover={{ scale: 1.02 }}
                                    whileTap={{ scale: 0.98 }}
                                  >
                                    <Button
                                      variant="outline"
                                      disabled={option.disabled || isProcessing}
                                      onClick={() => handleOptionSelect(option.value, message.fieldKey)}
                                      className="w-full p-3 h-auto text-center hover:bg-gradient-to-r hover:from-primary/10 hover:to-primary/5 hover:border-primary hover:shadow-md transition-all duration-300 group"
                                    >
                                      <div className="flex flex-col items-center space-y-1">
                                        <div className="text-lg group-hover:scale-110 transition-transform duration-200">
                                          {option.emoji || "🎯"}
                                        </div>
                                        <span className="text-xs font-medium text-center leading-tight">{option.label}</span>
                                      </div>
                                    </Button>
                                  </motion.div>
                                ))}
                              </div>
                            ) : (
                              <div className="flex flex-wrap gap-2">
                                {message.options.map((option, index) => (
                                  <motion.div
                                    key={option.value}
                                    initial={{ opacity: 0, x: -10 }}
                                    animate={{ opacity: 1, x: 0 }}
                                    transition={{ delay: 0.3 + (index * 0.1) }}
                                  >
                                    <Button
                                      variant="outline"
                                      size="sm"
                                      disabled={option.disabled || isProcessing}
                                      onClick={() => handleOptionSelect(option.value, message.fieldKey)}
                                      className="text-xs hover:bg-primary hover:text-primary-foreground transition-all"
                                    >
                                      {option.emoji && <span className="mr-1">{option.emoji}</span>}
                                      {option.label}
                                    </Button>
                                  </motion.div>
                                ))}
                              </div>
                            )}
                          </>
                        ) : null}
                      </div>
                    )}
                  </div>

                  {message.type === 'user' && (
                    <Avatar className="h-8 w-8 flex-shrink-0">
                      <AvatarFallback className="bg-secondary text-secondary-foreground text-xs">
                        <User className="h-4 w-4" />
                      </AvatarFallback>
                    </Avatar>
                  )}
                </motion.div>
              ))}
            </AnimatePresence>
            <div ref={messagesEndRef} />
          </div>

          {!isCompleted && (chatSteps[currentStep]?.type === 'text' || chatSteps[currentStep]?.type === 'verification') && (
            <div className="p-4 border-t border-primary/10">
              <div className="flex gap-2">
                <Input
                  value={currentInput}
                  onChange={(e) => setCurrentInput(e.target.value)}
                  onKeyPress={handleKeyPress}
                  placeholder="Tapez votre réponse..."
                  className="flex-1"
                  type={chatSteps[currentStep]?.key === 'motDePasse' || chatSteps[currentStep]?.key === 'confirmationMotDePasse' ? 'password' : 'text'}
                />
                <Button
                  onClick={handleTextSubmit}
                  disabled={!currentInput.trim()}
                  size="sm"
                  className="px-3"
                >
                  <Send className="h-4 w-4" />
                </Button>
              </div>
            </div>
          )}
        </Card>
      </div>
    </div>
  );
};

export default ChatRegistrationAssistant;
