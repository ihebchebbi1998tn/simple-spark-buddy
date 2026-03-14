import { useState } from 'react';
import { motion } from 'framer-motion';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Input } from '@/components/ui/input';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Badge } from '@/components/ui/badge';
import { 
  Target, 
  Users, 
  MapPin, 
  Briefcase,
  Languages,
  Clock,
  RotateCcw,
  Rocket,
  ArrowRight,
  ArrowLeft,
  CheckCircle,
  TrendingUp,
  Lock,
  Download,
  Database,
  Bot,
  BarChart3,
  Shield,
  ChevronDown,
  PieChart,
} from 'lucide-react';
import Header from '@/components/shared/Header';
import Footer from '@/components/shared/Footer';
import AccessRequestModal from '@/components/recruiter/AccessRequestModal';
import { useScrollAnimation } from '@/hooks/useScrollAnimation';

// Options data
const cities = ['Grand Tunis', 'Sousse', 'Sfax', 'Autre'];
const positions = ['Agent', 'Conseiller', 'Télévendeur', 'Team Leader', 'Superviseur', 'Manager'];
const activities = ['Télévente', 'Prise de RDV', 'Service Client', 'SAV', 'Fidélisation', 'Recouvrement'];
const operations = ['Énergie', 'Télécom', 'Voyance', 'Banque', 'Assurance', 'E-commerce', 'Santé'];
const languages = ['Français', 'Anglais', 'Espagnol', 'Italien', 'Allemand', 'Arabe', 'Néerlandais'];
const experienceLevels = ['Débutant', '0-6 mois', '6-12 mois', '+1 an', '+2 ans', '+3 ans'];
const profileTypes = ['Équilibré', 'Priorité volume', 'Priorité qualité'];
const urgencyLevels = ['Normal', 'Rapide', 'Urgent'];
const modelTypes = ['PPI - Retenu entretien', 'PPI - Intégré'];
const serviceTypes = ['Standard', 'Plus', 'Premium'];

const recruiterFaqs = [
  { q: "D'où viennent vos candidats ?", a: "Ils postulent directement ou via nos campagnes. Tous ont une expérience en centre d'appels." },
  { q: "Comment sont-ils qualifiés ?", a: "Test d'auto‑évaluation et scoring basé sur l'expérience et la disponibilité." },
  { q: "Puis-je gérer mes équipes dans l'outil ?", a: "Oui, vous créez vos admins et chargés avec leurs droits et objectifs." },
  { q: "Comment sont répartis les leads ?", a: "Distribution équitable ou pondérée, totalement automatisée." },
  { q: "En cas d'absence ?", a: "Redistribution en un clic vers un autre membre de l'équipe." },
  { q: "À quel moment êtes-vous payés ?", a: "Uniquement quand un candidat est retenu et intégré." },
];

const valueProps = [
  { icon: Database, title: "Sourceur intelligent", desc: "Des candidats actifs, pré‑qualifiés, avec scoring transparent." },
  { icon: Bot, title: "Distribution automatisée", desc: "Les leads sont répartis chaque jour selon vos règles. Plus de fichiers à envoyer." },
  { icon: BarChart3, title: "Aide à la décision", desc: "Tableaux de bord, alertes, performances. Vous pilotez, vous ne subissez pas." },
  { icon: Shield, title: "Plateforme sécurisée", desc: "Vos données, vos équipes, vos commandes. Tout est centralisé et protégé." },
];

const RecruiterSimulator = () => {
  const navigate = useNavigate();
  const [currentStep, setCurrentStep] = useState(0);
  const [isSearching, setIsSearching] = useState(false);
  const [isUnlocked, setIsUnlocked] = useState(false);
  const [accessModalOpen, setAccessModalOpen] = useState(false);
  const { ref: heroRef, isVisible: heroVisible } = useScrollAnimation(0.1);
  const { ref: vpRef, isVisible: vpVisible } = useScrollAnimation(0.1);
  const { ref: faqRef, isVisible: faqVisible } = useScrollAnimation(0.1);

  // Step 0: Criteria
  const [formData, setFormData] = useState({
    city: 'Grand Tunis',
    workMode: 'Présentiel',
    primaryLanguage: 'Français',
    isBilingual: 'Non',
    secondaryLanguage: '',
    position: 'Conseiller',
    globalExperience: '+1 an',
    positionExperience: '+6 mois',
    activity: 'SAV',
    availability: 'Immédiate',
    profileType: 'Équilibré',
    urgencyPreview: 'Rapide',
  });

  // Step 2: Estimation form
  const [estimationData, setEstimationData] = useState({
    need: 5,
    model: 'PPI - Intégré',
    service: 'Plus',
    urgency: 'Rapide',
  });

  // Computed results
  const [pool, setPool] = useState(0);
  const [lots, setLots] = useState({ high: 0, mid: 0, low: 0, bad: 0 });

  const computePool = () => {
    let p = 4200;
    if (formData.city === 'Sousse') p = 2100;
    else if (formData.city === 'Sfax') p = 1600;
    else if (formData.city === 'Autre') p = 800;
    if (formData.primaryLanguage === 'Anglais') p *= 0.6;
    if (formData.isBilingual === 'Oui') p *= 0.5;
    if (formData.workMode === 'Télétravail') p *= 0.8;
    p = Math.max(100, Math.round(p));
    setPool(p);

    let high = 0.15, mid = 0.25, low = 0.35;
    if (formData.profileType === 'Priorité volume') { high = 0.18; mid = 0.28; }
    if (formData.profileType === 'Priorité qualité') { high = 0.12; mid = 0.22; }
    const newLots = {
      high: Math.round(p * high),
      mid: Math.round(p * mid),
      low: Math.round(p * low),
      bad: Math.round(p * (1 - high - mid - low)),
    };
    setLots(newLots);
    return { pool: p, lots: newLots };
  };

  const computePrice = (lotsData?: typeof lots) => {
    const l = lotsData || lots;
    const need = estimationData.need;
    const unit = estimationData.model.includes('Intégré') ? 180 : 140;
    const svc = estimationData.service === 'Plus' ? 1.25 : estimationData.service === 'Premium' ? 1.55 : 1.0;
    const urgm = estimationData.urgency === 'Rapide' ? 1.15 : estimationData.urgency === 'Urgent' ? 1.3 : 1.0;
    const high = l.high || 500;
    const scarcity = high < need * 6 ? 1.18 : high < need * 10 ? 1.08 : 1.0;
    return Math.round(need * unit * svc * urgm * scarcity);
  };

  const handleNextStep = (step: number) => {
    if (step === 1) {
      setIsSearching(true);
      setTimeout(() => {
        computePool();
        setIsSearching(false);
        setCurrentStep(1);
      }, 1200);
    } else if (step === 2) {
      setCurrentStep(2);
    }
  };

  const steps = [
    { label: '1. Critères', active: currentStep === 0, done: currentStep > 0 },
    { label: '2. Vivier', active: currentStep === 1, done: currentStep > 1 },
    { label: '3. Estimation', active: currentStep === 2, done: false, locked: !isUnlocked && currentStep < 2 },
  ];

  const price = computePrice();
  const quota = estimationData.need * (estimationData.service === 'Plus' ? 40 : 30);
  const bonus = estimationData.service === 'Plus' ? "jusqu'à 5%" : "jusqu'à 3%";

  return (
    <div className="min-h-screen bg-background">
      <Header />
      
      <main>
        {/* ===== V1 HERO ===== */}
        <motion.section
          ref={heroRef}
          className="py-12 lg:py-20"
          initial={{ opacity: 0 }}
          animate={heroVisible ? { opacity: 1 } : {}}
          transition={{ duration: 0.6 }}
        >
          <div className="container mx-auto px-4 max-w-[1200px]">
            <div className="grid lg:grid-cols-2 gap-12 items-center">
              <motion.div
                initial={{ opacity: 0, x: -30 }}
                animate={heroVisible ? { opacity: 1, x: 0 } : {}}
                transition={{ duration: 0.6, delay: 0.1 }}
              >
                <span className="inline-block bg-primary/10 text-primary text-sm font-semibold px-4 py-1.5 rounded-full mb-6">
                  🎯 ESPACE RECRUTEUR
                </span>
                <h1 className="text-3xl lg:text-4xl xl:text-5xl font-extrabold text-foreground leading-tight mb-6">
                  La plateforme qui transforme du sourcing en décisions.
                </h1>
                <p className="text-lg text-muted-foreground max-w-lg mb-8">
                  Nous ne livrons pas des fichiers. Nous livrons une infrastructure complète : sourcing intelligent, distribution automatisée, suivi en temps réel, et aide à la décision pour vos équipes.
                </p>
                <div className="flex flex-wrap gap-4">
                  <Button size="lg" className="rounded-full px-8 font-semibold shadow-md" onClick={() => document.getElementById('simulateur')?.scrollIntoView({ behavior: 'smooth' })}>
                    📊 Simuler mon vivier
                  </Button>
                  <Button size="lg" variant="outline" className="rounded-full px-8 font-semibold border-primary text-primary hover:bg-primary/5" onClick={() => setAccessModalOpen(true)}>
                    🔐 Accéder à la plateforme
                  </Button>
                </div>
              </motion.div>
              <motion.div className="hidden lg:flex items-center justify-center" initial={{ opacity: 0, x: 30 }} animate={heroVisible ? { opacity: 1, x: 0 } : {}} transition={{ duration: 0.6, delay: 0.2 }}>
                <div className="w-full max-w-md h-[300px] bg-primary/5 rounded-3xl flex items-center justify-center border border-primary/10">
                  <PieChart className="w-24 h-24 text-primary" />
                </div>
              </motion.div>
            </div>
          </div>
        </motion.section>

        {/* ===== V1 VALUE PROPS ===== */}
        <motion.section ref={vpRef} className="py-16" initial={{ opacity: 0 }} animate={vpVisible ? { opacity: 1 } : {}} transition={{ duration: 0.6 }}>
          <div className="container mx-auto px-4 max-w-[1200px]">
            <h2 className="text-2xl lg:text-3xl font-bold text-center text-foreground mb-10">
              CallCenterMatch.ai, c'est à la fois votre source et votre outil de pilotage.
            </h2>
            <div className="grid sm:grid-cols-2 lg:grid-cols-4 gap-6">
              {valueProps.map((vp, i) => {
                const Icon = vp.icon;
                return (
                  <motion.div key={i} className="bg-card border border-border rounded-2xl p-8 text-center transition-all hover:border-primary hover:shadow-md hover:-translate-y-1" initial={{ opacity: 0, y: 20 }} animate={vpVisible ? { opacity: 1, y: 0 } : {}} transition={{ duration: 0.4, delay: 0.1 * i }}>
                    <Icon className="w-8 h-8 text-primary mx-auto mb-5" />
                    <h3 className="font-bold text-foreground mb-2">{vp.title}</h3>
                    <p className="text-sm text-muted-foreground">{vp.desc}</p>
                  </motion.div>
                );
              })}
            </div>
          </div>
        </motion.section>

        {/* ===== SIMULATOR ===== */}
        <section id="simulateur" className="py-16 bg-muted/30">
          <div className="container mx-auto px-4 max-w-[1200px]">
            <h2 className="text-2xl lg:text-3xl font-bold text-foreground mb-2">📊 Simulateur de vivier</h2>
            <p className="text-muted-foreground mb-8">Découvrez le potentiel de notre base selon vos critères. L'estimation budgétaire est débloquée après inscription.</p>

        <div className="max-w-5xl mx-auto">
          {/* Step indicators */}
          <div className="flex gap-3 justify-center mb-6 flex-wrap">
            {steps.map((step, i) => (
              <span
                key={i}
                className={`px-5 py-2.5 rounded-full text-sm font-semibold transition-all ${
                  step.active
                    ? 'bg-primary text-primary-foreground'
                    : step.done
                    ? 'bg-green-100 text-green-700 border border-green-300'
                    : step.locked
                    ? 'bg-muted text-muted-foreground opacity-70'
                    : 'bg-muted text-muted-foreground'
                }`}
              >
                {step.label}
                {step.locked && ' 🔒'}
              </span>
            ))}
          </div>

          {/* ======== STEP 0: CRITERIA ======== */}
          {currentStep === 0 && (
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.5 }}
            >
              <Card className="border shadow-lg">
                <CardHeader className="border-b bg-muted/30">
                  <div className="flex items-center gap-3">
                    <div className="w-10 h-10 bg-primary rounded-lg flex items-center justify-center shrink-0">
                      <Users className="w-5 h-5 text-primary-foreground" />
                    </div>
                    <div>
                      <CardTitle className="text-lg">Critères des profils recherchés</CardTitle>
                      <p className="text-sm text-muted-foreground">Renseignez les détails de votre besoin</p>
                    </div>
                  </div>
                </CardHeader>
                <CardContent className="p-4 sm:p-6">
                  <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-4">
                    <div className="space-y-1.5">
                      <Label className="flex items-center gap-2 text-sm">
                        <MapPin className="w-4 h-4 text-muted-foreground" /> Ville
                      </Label>
                      <Select value={formData.city} onValueChange={(v) => setFormData({...formData, city: v})}>
                        <SelectTrigger><SelectValue /></SelectTrigger>
                        <SelectContent>
                          {cities.map((c) => <SelectItem key={c} value={c}>{c}</SelectItem>)}
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-1.5">
                      <Label className="text-sm">Mode de travail</Label>
                      <Select value={formData.workMode} onValueChange={(v) => setFormData({...formData, workMode: v})}>
                        <SelectTrigger><SelectValue /></SelectTrigger>
                        <SelectContent>
                          {['Présentiel', 'Télétravail', 'Hybride'].map((m) => <SelectItem key={m} value={m}>{m}</SelectItem>)}
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-1.5">
                      <Label className="flex items-center gap-2 text-sm">
                        <Languages className="w-4 h-4 text-muted-foreground" /> Langue principale
                      </Label>
                      <Select value={formData.primaryLanguage} onValueChange={(v) => setFormData({...formData, primaryLanguage: v})}>
                        <SelectTrigger><SelectValue /></SelectTrigger>
                        <SelectContent>
                          {languages.map((l) => <SelectItem key={l} value={l}>{l}</SelectItem>)}
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-1.5">
                      <Label className="text-sm">Bilingue ?</Label>
                      <Select value={formData.isBilingual} onValueChange={(v) => setFormData({...formData, isBilingual: v})}>
                        <SelectTrigger><SelectValue /></SelectTrigger>
                        <SelectContent>
                          <SelectItem value="Non">Non</SelectItem>
                          <SelectItem value="Oui">Oui</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>

                    {formData.isBilingual === 'Oui' && (
                      <div className="space-y-1.5">
                        <Label className="text-sm">Langue secondaire</Label>
                        <Select value={formData.secondaryLanguage} onValueChange={(v) => setFormData({...formData, secondaryLanguage: v})}>
                          <SelectTrigger><SelectValue placeholder="Sélectionnez" /></SelectTrigger>
                          <SelectContent>
                            {languages.filter(l => l !== formData.primaryLanguage).map((l) => <SelectItem key={l} value={l}>{l}</SelectItem>)}
                          </SelectContent>
                        </Select>
                      </div>
                    )}

                    <div className="space-y-1.5">
                      <Label className="flex items-center gap-2 text-sm">
                        <Briefcase className="w-4 h-4 text-muted-foreground" /> Poste
                      </Label>
                      <Select value={formData.position} onValueChange={(v) => setFormData({...formData, position: v})}>
                        <SelectTrigger><SelectValue /></SelectTrigger>
                        <SelectContent>
                          {positions.map((p) => <SelectItem key={p} value={p}>{p}</SelectItem>)}
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-1.5">
                      <Label className="flex items-center gap-2 text-sm">
                        <Clock className="w-4 h-4 text-muted-foreground" /> Exp. globale CC
                      </Label>
                      <Select value={formData.globalExperience} onValueChange={(v) => setFormData({...formData, globalExperience: v})}>
                        <SelectTrigger><SelectValue /></SelectTrigger>
                        <SelectContent>
                          {experienceLevels.map((e) => <SelectItem key={e} value={e}>{e}</SelectItem>)}
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-1.5">
                      <Label className="text-sm">Exp. au poste</Label>
                      <Select value={formData.positionExperience} onValueChange={(v) => setFormData({...formData, positionExperience: v})}>
                        <SelectTrigger><SelectValue /></SelectTrigger>
                        <SelectContent>
                          {experienceLevels.map((e) => <SelectItem key={e} value={e}>{e}</SelectItem>)}
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-1.5">
                      <Label className="text-sm">Activité</Label>
                      <Select value={formData.activity} onValueChange={(v) => setFormData({...formData, activity: v})}>
                        <SelectTrigger><SelectValue /></SelectTrigger>
                        <SelectContent>
                          {activities.map((a) => <SelectItem key={a} value={a}>{a}</SelectItem>)}
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-1.5">
                      <Label className="text-sm">Disponibilité</Label>
                      <Select value={formData.availability} onValueChange={(v) => setFormData({...formData, availability: v})}>
                        <SelectTrigger><SelectValue /></SelectTrigger>
                        <SelectContent>
                          {['Immédiate', 'Sous 2 sem.', 'Sous 1 mois'].map((a) => <SelectItem key={a} value={a}>{a}</SelectItem>)}
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-1.5">
                      <Label className="text-sm">Profil recherché</Label>
                      <Select value={formData.profileType} onValueChange={(v) => setFormData({...formData, profileType: v})}>
                        <SelectTrigger><SelectValue /></SelectTrigger>
                        <SelectContent>
                          {profileTypes.map((p) => <SelectItem key={p} value={p}>{p}</SelectItem>)}
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-1.5">
                      <Label className="text-sm">Urgence</Label>
                      <Select value={formData.urgencyPreview} onValueChange={(v) => setFormData({...formData, urgencyPreview: v})}>
                        <SelectTrigger><SelectValue /></SelectTrigger>
                        <SelectContent>
                          {urgencyLevels.map((u) => <SelectItem key={u} value={u}>{u}</SelectItem>)}
                        </SelectContent>
                      </Select>
                    </div>
                  </div>

                  <div className="flex justify-end mt-6">
                    <Button 
                      onClick={() => handleNextStep(1)}
                      disabled={isSearching}
                      size="lg"
                      className="px-8"
                    >
                      {isSearching ? (
                        <>
                          <div className="w-4 h-4 border-2 border-primary-foreground border-t-transparent rounded-full animate-spin mr-2" />
                          Analyse en cours...
                        </>
                      ) : (
                        <>
                          <Rocket className="w-4 h-4 mr-2" />
                          Voir le vivier →
                        </>
                      )}
                    </Button>
                  </div>
                </CardContent>
              </Card>
            </motion.div>
          )}

          {/* ======== STEP 1: POOL / LOTS ======== */}
          {currentStep === 1 && (
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              className="space-y-6"
            >
              <div className="text-center">
                <h2 className="text-2xl font-bold text-foreground mb-2">
                  Vivier total : <span className="text-primary">{pool.toLocaleString()}</span> profils
                </h2>
                <p className="text-muted-foreground">Répartition par lot de compatibilité</p>
              </div>

              {lots.high < 50 && (
                <div className="bg-amber-50 border-l-4 border-amber-400 p-4 rounded-r-lg">
                  <p className="text-amber-800 text-sm font-medium">
                    ⚠️ Vivier limité – La combinaison de critères réduit fortement la disponibilité. Nous recommandons d'élargir vos critères.
                  </p>
                </div>
              )}

              <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
                <Card className="border-green-200 bg-green-50">
                  <CardContent className="p-5 text-center">
                    <p className="text-3xl font-extrabold text-foreground">{lots.high}</p>
                    <p className="text-sm font-semibold text-green-700 mt-1">&gt; 80% match</p>
                    <p className="text-xs text-muted-foreground mt-1">Lot Premium</p>
                  </CardContent>
                </Card>
                <Card className="border-blue-200 bg-blue-50">
                  <CardContent className="p-5 text-center">
                    <p className="text-3xl font-extrabold text-foreground">{lots.mid}</p>
                    <p className="text-sm font-semibold text-blue-700 mt-1">60-80% match</p>
                    <p className="text-xs text-muted-foreground mt-1">Lot Standard</p>
                  </CardContent>
                </Card>
                <Card className="border-amber-200 bg-amber-50">
                  <CardContent className="p-5 text-center">
                    <p className="text-3xl font-extrabold text-foreground">{lots.low}</p>
                    <p className="text-sm font-semibold text-amber-700 mt-1">40-60% match</p>
                    <p className="text-xs text-muted-foreground mt-1">Lot Élargi</p>
                  </CardContent>
                </Card>
                <Card className="border-border">
                  <CardContent className="p-5 text-center">
                    <p className="text-3xl font-extrabold text-muted-foreground">{lots.bad}</p>
                    <p className="text-sm font-semibold text-muted-foreground mt-1">&lt; 40% match</p>
                    <p className="text-xs text-muted-foreground mt-1">Hors cible</p>
                  </CardContent>
                </Card>
              </div>

              <div className="flex justify-between">
                <Button variant="outline" onClick={() => setCurrentStep(0)}>
                  <ArrowLeft className="w-4 h-4 mr-2" /> Modifier les critères
                </Button>
                <Button onClick={() => handleNextStep(2)}>
                  Voir l'estimation → 
                </Button>
              </div>
            </motion.div>
          )}

          {/* ======== STEP 2: ESTIMATION (PROTECTED) ======== */}
          {currentStep === 2 && (
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              className="space-y-6"
            >
              <div className="relative rounded-2xl border bg-card p-6 sm:p-8 min-h-[400px]">
                {/* Estimation content (always rendered but blurred when locked) */}
                <div className={`transition-all duration-300 ${!isUnlocked ? 'blur-sm pointer-events-none select-none' : ''}`}>
                  <h3 className="text-xl font-bold text-foreground mb-6">Configurez votre estimation</h3>
                  
                  <div className="grid md:grid-cols-2 gap-8">
                    {/* Config form */}
                    <div className="space-y-4">
                      <div className="space-y-1.5">
                        <Label className="text-sm">Objectif (nombre de candidats)</Label>
                        <Input
                          type="number"
                          min={1}
                          max={50}
                          value={estimationData.need}
                          onChange={(e) => setEstimationData({...estimationData, need: parseInt(e.target.value) || 1})}
                        />
                      </div>
                      <div className="space-y-1.5">
                        <Label className="text-sm">Modèle</Label>
                        <Select value={estimationData.model} onValueChange={(v) => setEstimationData({...estimationData, model: v})}>
                          <SelectTrigger><SelectValue /></SelectTrigger>
                          <SelectContent>
                            {modelTypes.map((m) => <SelectItem key={m} value={m}>{m}</SelectItem>)}
                          </SelectContent>
                        </Select>
                      </div>
                      <div className="space-y-1.5">
                        <Label className="text-sm">Service</Label>
                        <Select value={estimationData.service} onValueChange={(v) => setEstimationData({...estimationData, service: v})}>
                          <SelectTrigger><SelectValue /></SelectTrigger>
                          <SelectContent>
                            {serviceTypes.map((s) => <SelectItem key={s} value={s}>{s}</SelectItem>)}
                          </SelectContent>
                        </Select>
                      </div>
                      <div className="space-y-1.5">
                        <Label className="text-sm">Urgence</Label>
                        <Select value={estimationData.urgency} onValueChange={(v) => setEstimationData({...estimationData, urgency: v})}>
                          <SelectTrigger><SelectValue /></SelectTrigger>
                          <SelectContent>
                            {urgencyLevels.map((u) => <SelectItem key={u} value={u}>{u}</SelectItem>)}
                          </SelectContent>
                        </Select>
                      </div>
                    </div>

                    {/* Price card */}
                    <Card className="bg-muted/30 border shadow-sm">
                      <CardContent className="p-6 text-center space-y-4">
                        <Badge className="bg-green-100 text-green-700 border-green-300">💰 Votre devis</Badge>
                        <p className="text-4xl font-extrabold text-primary">
                          {price.toLocaleString('fr-FR')} TND
                        </p>
                        <div className="space-y-2 text-sm">
                          <div className="flex justify-between">
                            <span className="text-muted-foreground">Plafond</span>
                            <strong>{quota.toLocaleString('fr-FR')} profils</strong>
                          </div>
                          <div className="flex justify-between">
                            <span className="text-muted-foreground">Bonus</span>
                            <strong className="text-green-600">{bonus}</strong>
                          </div>
                        </div>
                        <Button className="w-full bg-green-600 hover:bg-green-700 text-white mt-4">
                          <Download className="w-4 h-4 mr-2" />
                          Télécharger ma shortlist &gt;80%
                        </Button>
                      </CardContent>
                    </Card>
                  </div>
                </div>

                {/* Protected overlay */}
                {!isUnlocked && (
                  <div className="absolute inset-0 bg-background/95 backdrop-blur-sm flex flex-col items-center justify-center rounded-2xl z-10 p-8 text-center">
                    <Lock className="w-12 h-12 text-primary mb-4" />
                    <h3 className="text-xl font-bold text-foreground mb-2">Estimation protégée</h3>
                    <p className="text-muted-foreground mb-6 max-w-md">
                      Créez un compte recruteur gratuit pour débloquer votre devis et accéder à la shortlist.
                    </p>
                    <Button size="lg" onClick={() => setAccessModalOpen(true)}>
                      Créer mon compte
                    </Button>
                  </div>
                )}
              </div>

              <div className="flex justify-between">
                <Button variant="outline" onClick={() => setCurrentStep(1)}>
                  <ArrowLeft className="w-4 h-4 mr-2" /> Retour aux lots
                </Button>
              </div>
            </motion.div>
          )}

          </div>
          </div>
        </section>

        {/* ===== V1 FAQ RECRUTEURS ===== */}
        <motion.section
          ref={faqRef}
          className="py-16"
          initial={{ opacity: 0 }}
          animate={faqVisible ? { opacity: 1 } : {}}
          transition={{ duration: 0.6 }}
        >
          <div className="container mx-auto px-4 max-w-[1200px]">
            <h2 className="text-2xl lg:text-3xl font-bold text-center text-foreground mb-10">
              Questions fréquentes de recruteurs
            </h2>
            <div className="grid md:grid-cols-2 gap-4 max-w-4xl mx-auto">
              {recruiterFaqs.map((faq, i) => (
                <details
                  key={i}
                  className="bg-card border border-border rounded-2xl px-6 py-5 cursor-pointer transition-colors hover:border-primary group"
                >
                  <summary className="font-semibold text-foreground list-none flex items-center justify-between cursor-pointer">
                    {faq.q}
                    <ChevronDown className="w-4 h-4 text-primary shrink-0 transition-transform group-open:rotate-180" />
                  </summary>
                  <p className="mt-4 text-muted-foreground text-sm">{faq.a}</p>
                </details>
              ))}
            </div>
          </div>
        </motion.section>

        {/* ===== V1 CTA ===== */}
        <section className="py-16">
          <div className="container mx-auto px-4 max-w-[1200px]">
            <div className="bg-primary/5 border border-primary/10 rounded-3xl p-10 lg:p-16 text-center">
              <h2 className="text-2xl lg:text-3xl font-bold text-foreground mb-4">
                Prêt à passer sous le capot ?
              </h2>
              <p className="text-lg text-muted-foreground mb-8 max-w-xl mx-auto">
                Le simulateur vous a montré le vivier. L'estimation et la plateforme complète sont accessibles après vérification.
              </p>
              <div className="flex flex-wrap gap-4 justify-center">
                <Button size="lg" className="rounded-full px-10 font-semibold shadow-md" onClick={() => setAccessModalOpen(true)}>
                  📅 Obtenir une démo privée
                </Button>
                <Button size="lg" variant="outline" className="rounded-full px-10 font-semibold border-primary text-primary" onClick={() => setAccessModalOpen(true)}>
                  📞 Parler à un expert
                </Button>
              </div>
            </div>
          </div>
        </section>
      </main>

      <Footer />

      <AccessRequestModal
        open={accessModalOpen}
        onOpenChange={setAccessModalOpen}
        onSuccess={() => setIsUnlocked(true)}
      />
    </div>
  );
};

export default RecruiterSimulator;
