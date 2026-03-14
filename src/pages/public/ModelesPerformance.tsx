import { useState } from "react";
import { motion } from "framer-motion";
import { useScrollAnimation } from "@/hooks/useScrollAnimation";
import Header from "@/components/shared/Header";
import Footer from "@/components/shared/Footer";
import { Button } from "@/components/ui/button";
import { Link } from "react-router-dom";
import {
  Shield,
  TrendingUp,
  AlertTriangle,
  ChevronDown,
  CreditCard,
  Users,
  Award,
  Target,
  BarChart3,
  Zap,
  CheckCircle2,
  Eye,
  DollarSign,
  ArrowRight,
} from "lucide-react";
import { useTranslation } from "react-i18next";

/* ──────────────────────────────────────── DATA ──────────────────────────────────────── */

const heroStats = [
  { titleKey: "Paiement", labelKey: "AU RÉSULTAT" },
  { titleKey: "Alignement", labelKey: "SUR VOS OBJECTIFS" },
  { titleKey: "Transparence", labelKey: "FINANCIÈRE", highlight: true },
  { titleKey: "Rentabilité", labelKey: "SÉCURISÉE" },
];

const risks = [
  {
    icon: AlertTriangle,
    title: "Coûts RH cachés",
    items: [
      "Temps RH et managers mobilisés à répétition",
      "Dépenses engagées sur des candidatures inexploitables",
      "Frais de formation et d'encadrement sans retour",
      "Postes vacants qui pèsent durablement sur l'exploitation",
    ],
  },
  {
    icon: DollarSign,
    title: "Coût d'intégration non sécurisé",
    items: [
      "Budget engagé sur un sourcing brut, sans garantie de transformation",
      "Aucune visibilité en amont sur le coût réel d'intégration",
      "Frais RH additionnels et imprévisibles liés aux No-show",
      "Gap financier non maîtrisé entre sourcing et intégration réussie",
    ],
  },
  {
    icon: BarChart3,
    title: "Rentabilité peu pilotée du budget engagé",
    items: [
      "Productivité réelle difficile à anticiper avant plusieurs semaines",
      "Aucune alternative de compensation rapide en cas d'abandon précoce",
      "Faible marge de manœuvre sur les investissements formation et encadrement",
      "Leviers d'ajustement stratégiques limités par manque d'options de sécurisation adaptées",
    ],
  },
];

const pppModels = [
  {
    icon: Target,
    title: "PPP — Retenu à l'entretien",
    items: [
      "Paiement déclenché lorsque le candidat est confirmé « retenu » à l'étape d'entretien physique",
      "Idéal si votre process aval est déjà bien maîtrisé",
    ],
  },
  {
    icon: Users,
    title: "PPP — Présent à la formation",
    items: [
      "Paiement déclenché sur une présence constatée au démarrage de formation",
      "Utile quand le no-show est un vrai frein",
    ],
  },
  {
    icon: CheckCircle2,
    title: "PPP — Intégré",
    items: [
      "Paiement déclenché lorsque l'intégration est effective",
      "Le plus aligné sur l'objectif final : sécuriser la performance jusqu'au bout",
    ],
  },
];

const packs = [
  {
    icon: Zap,
    title: "Pack crédits Agents",
    items: [
      "Format flexible pour gérer des viviers de langues rares (IT, ES, DE, NL, TR et PR).",
      "Cadre d'exploitation (validité, consommation, priorisation)",
    ],
  },
  {
    icon: CreditCard,
    title: "Pack crédits Cadres",
    items: [
      "Pour des profils plus rares (superviseurs, team leaders)",
      "Accès ciblé sur des fonctions support",
    ],
  },
  {
    icon: Award,
    title: "Pack Excellence",
    items: [
      "Alternative premium pour recrutements récurrents",
      "Cadre optimisé, suivi et options avancées",
    ],
  },
];

const perfFaqs = [
  {
    q: "Pourquoi ne pas afficher les prix et paramètres publiquement ?",
    a: "Parce qu'ils dépendent du contexte (volumes, langues, profils). La logique est publique, les détails sont consultables par les centres authentifiés.",
  },
  {
    q: "Comment choisir entre PPP et un pack crédits ?",
    a: "PPP est le modèle phare orienté performance. Les packs sont complémentaires pour des besoins spécifiques.",
  },
  {
    q: "Est-ce que le PPP remplace nos process internes ?",
    a: "Non. Il s'aligne sur vos étapes et vise à rendre la performance plus lisible, pour mieux piloter et réduire les pertes.",
  },
  {
    q: 'Que signifie "centres authentifiés" ?',
    a: "Un espace recruteur validé permettant de consulter les détails et d'utiliser les outils de la plateforme.",
  },
  {
    q: "Les modèles PPP sont-ils adaptés à tous les types de centres ?",
    a: "Oui, ils s'adaptent à votre volume, vos langues et votre type d'opération.",
  },
  {
    q: "Comment sont calculés les seuils de rentabilité dans vos modèles ?",
    a: "Ils sont basés sur des données terrain consolidées. Les détails sont accessibles dans l'espace recruteur.",
  },
];

/* ──────────────────────────────────────── SUB‑COMPONENTS ──────────────────────────────────────── */

const fadeUp = { hidden: { opacity: 0, y: 30 }, visible: { opacity: 1, y: 0 } };

function SectionHeading({ children, sub }: { children: React.ReactNode; sub?: string }) {
  return (
    <div className="text-center mb-10 lg:mb-14">
      <h2 className="text-3xl lg:text-4xl font-extrabold text-foreground tracking-tight">{children}</h2>
      {sub && <p className="mt-4 text-muted-foreground text-lg max-w-2xl mx-auto leading-relaxed">{sub}</p>}
    </div>
  );
}

function NumberCard({
  index, icon: Icon, title, items, delay = 0, visible,
}: { index: number; icon: React.ElementType; title: string; items: string[]; delay?: number; visible: boolean }) {
  return (
    <motion.div
      className="group bg-card border border-border rounded-3xl p-7 flex flex-col transition-all hover:border-primary/40 hover:shadow-lg hover:-translate-y-1"
      variants={fadeUp}
      initial="hidden"
      animate={visible ? "visible" : "hidden"}
      transition={{ duration: 0.5, delay }}
    >
      <div className="flex items-center gap-4 mb-5">
        <div className="w-12 h-12 rounded-2xl bg-gradient-to-br from-primary to-primary/80 flex items-center justify-center text-primary-foreground font-extrabold text-lg shadow-sm shrink-0">
          {index}
        </div>
        <Icon className="w-5 h-5 text-primary" />
      </div>
      <h4 className="text-lg font-bold text-foreground mb-4 pb-3 border-b-2 border-primary/15">{title}</h4>
      <ul className="space-y-3 flex-1">
        {items.map((item, i) => (
          <li key={i} className="flex items-start gap-2.5 text-sm text-muted-foreground leading-relaxed">
            <span className="mt-1.5 w-1.5 h-1.5 rounded-full bg-primary shrink-0" />
            {item}
          </li>
        ))}
      </ul>
    </motion.div>
  );
}

function FAQItem({ q, a }: { q: string; a: string }) {
  const [open, setOpen] = useState(false);
  return (
    <div
      className="bg-card border border-border rounded-2xl p-5 cursor-pointer transition-all hover:border-primary/40 hover:shadow-sm"
      onClick={() => setOpen(!open)}
    >
      <div className="flex items-center justify-between gap-3">
        <span className="font-semibold text-foreground text-sm lg:text-base">{q}</span>
        <ChevronDown className={`w-4 h-4 text-primary shrink-0 transition-transform duration-300 ${open ? "rotate-180" : ""}`} />
      </div>
      <motion.div
        initial={false}
        animate={{ height: open ? "auto" : 0, opacity: open ? 1 : 0 }}
        transition={{ duration: 0.3 }}
        className="overflow-hidden"
      >
        <p className="pt-3 text-muted-foreground text-sm leading-relaxed">{a}</p>
      </motion.div>
    </div>
  );
}

/* ──────────────────────────────────────── PAGE ──────────────────────────────────────── */

export default function ModelesPerformance() {
  const { ref: heroRef, isVisible: heroVisible } = useScrollAnimation();
  const { ref: risksRef, isVisible: risksVisible } = useScrollAnimation();
  const { ref: pppRef, isVisible: pppVisible } = useScrollAnimation();
  const { ref: packsRef, isVisible: packsVisible } = useScrollAnimation();
  const { ref: faqRef, isVisible: faqVisible } = useScrollAnimation();
  const { t } = useTranslation('performance');

  return (
    <div className="min-h-screen flex flex-col bg-background">
      <Header />

      <main className="flex-1">
        {/* ═══════ HERO ═══════ */}
        <motion.section
          ref={heroRef}
          className="py-14 lg:py-24"
          initial={{ opacity: 0 }}
          animate={heroVisible ? { opacity: 1 } : {}}
          transition={{ duration: 0.6 }}
        >
          <div className="container mx-auto px-4 max-w-[1280px]">
            <div className="grid lg:grid-cols-2 gap-12 lg:gap-16 items-center">
              <motion.div
                initial={{ opacity: 0, x: -30 }}
                animate={heroVisible ? { opacity: 1, x: 0 } : {}}
                transition={{ duration: 0.6, delay: 0.1 }}
              >

                <h1 className="text-3xl lg:text-4xl xl:text-[2.75rem] font-extrabold text-foreground leading-[1.15] mb-6 tracking-tight">
                  {t('title')}{" "}
                  <span className="text-primary">{t('titleHighlight')}</span>
                </h1>

                <p className="text-lg text-muted-foreground leading-relaxed max-w-xl mb-8">
                  {t('subtitle')}
                </p>

                <div className="flex flex-wrap gap-4">
                  <Button
                    size="lg"
                    className="rounded-full px-8 font-semibold shadow-md"
                    onClick={() => document.getElementById("modeles")?.scrollIntoView({ behavior: "smooth" })}
                  >
                    {t('discoverBtn')}
                  </Button>
                  <Link to="/entreprises">
                    <Button size="lg" variant="outline" className="rounded-full px-8 font-semibold border-primary text-primary hover:bg-primary/5">
                      {t('simulatorBtn')}
                    </Button>
                  </Link>
                </div>
              </motion.div>

              <motion.div
                initial={{ opacity: 0, x: 30 }}
                animate={heroVisible ? { opacity: 1, x: 0 } : {}}
                transition={{ duration: 0.6, delay: 0.25 }}
                className="relative"
              >
                <div className="absolute inset-0 rounded-3xl bg-gradient-to-br from-primary/8 to-primary/3 blur-3xl -z-10" />
                <div className="grid grid-cols-2 gap-4">
                  {heroStats.map((s, i) => (
                    <motion.div
                      key={i}
                      className={`rounded-2xl p-6 text-center border transition-all hover:-translate-y-1 hover:shadow-md ${
                        s.highlight
                          ? "bg-gradient-to-br from-primary to-primary/85 text-primary-foreground border-primary shadow-lg"
                          : "bg-card border-border"
                      }`}
                      initial={{ opacity: 0, scale: 0.9 }}
                      animate={heroVisible ? { opacity: 1, scale: 1 } : {}}
                      transition={{ duration: 0.4, delay: 0.3 + i * 0.08 }}
                    >
                      <span className={`block text-xl font-extrabold mb-1 ${s.highlight ? "" : "text-foreground"}`}>{s.titleKey}</span>
                      <span className={`text-xs font-bold tracking-widest uppercase ${s.highlight ? "text-primary-foreground/80" : "text-muted-foreground"}`}>
                        {s.labelKey}
                      </span>
                    </motion.div>
                  ))}
                </div>
              </motion.div>
            </div>
          </div>
        </motion.section>

        {/* ═══════ RISQUES FINANCIERS ═══════ */}
        <motion.section
          ref={risksRef}
          className="py-16 lg:py-20"
          initial={{ opacity: 0 }}
          animate={risksVisible ? { opacity: 1 } : {}}
          transition={{ duration: 0.6 }}
        >
          <div className="container mx-auto px-4 max-w-[1280px]">
            <SectionHeading>{t('risksTitle')}</SectionHeading>
            <div className="grid md:grid-cols-3 gap-6">
              {risks.map((r, i) => (
                <NumberCard key={i} index={i + 1} icon={r.icon} title={r.title} items={r.items} delay={0.1 * i} visible={risksVisible} />
              ))}
            </div>
          </div>
        </motion.section>

        {/* ═══════ PPP MODELS ═══════ */}
        <motion.section
          ref={pppRef}
          id="modeles"
          className="py-16 lg:py-20 bg-muted/40 scroll-mt-24"
          initial={{ opacity: 0 }}
          animate={pppVisible ? { opacity: 1 } : {}}
          transition={{ duration: 0.6 }}
        >
          <div className="container mx-auto px-4 max-w-[1280px]">
            <SectionHeading sub={t('pppSubtitle')}>
              {t('pppTitle')}
            </SectionHeading>

            <div className="grid md:grid-cols-3 gap-6">
              {pppModels.map((m, i) => (
                <NumberCard key={i} index={i + 1} icon={m.icon} title={m.title} items={m.items} delay={0.1 * i} visible={pppVisible} />
              ))}
            </div>

            <div className="text-center mt-10">
              <Link to="/demande-acces-recruteur">
                <Button size="lg" className="rounded-full px-10 font-semibold shadow-md gap-2">
                  <Eye className="w-4 h-4" /> {t('discoverDetails')}
                </Button>
              </Link>
            </div>
          </div>
        </motion.section>

        {/* ═══════ PACKS COMPLÉMENTAIRES ═══════ */}
        <motion.section
          ref={packsRef}
          className="py-16 lg:py-20"
          initial={{ opacity: 0 }}
          animate={packsVisible ? { opacity: 1 } : {}}
          transition={{ duration: 0.6 }}
        >
          <div className="container mx-auto px-4 max-w-[1280px]">
            <SectionHeading sub={t('packsSubtitle')}>
              {t('packsTitle')}
            </SectionHeading>

            <div className="grid md:grid-cols-3 gap-6">
              {packs.map((p, i) => (
                <NumberCard key={i} index={i + 1} icon={p.icon} title={p.title} items={p.items} delay={0.1 * i} visible={packsVisible} />
              ))}
            </div>

            <div className="text-center mt-10">
              <Link to="/demande-acces-recruteur">
                <Button size="lg" className="rounded-full px-10 font-semibold shadow-md gap-2">
                  <Eye className="w-4 h-4" /> {t('discoverDetails')}
                </Button>
              </Link>
            </div>
          </div>
        </motion.section>

        {/* ═══════ FAQ ═══════ */}
        <motion.section
          ref={faqRef}
          className="py-16 lg:py-20 bg-muted/30"
          initial={{ opacity: 0 }}
          animate={faqVisible ? { opacity: 1 } : {}}
          transition={{ duration: 0.6 }}
        >
          <div className="container mx-auto px-4 max-w-[1280px]">
            <SectionHeading sub={t('faqSubtitle')}>
              {t('faqTitle')}
            </SectionHeading>

            <div className="grid md:grid-cols-2 gap-4 max-w-4xl mx-auto">
              {perfFaqs.map((f, i) => (
                <FAQItem key={i} q={f.q} a={f.a} />
              ))}
            </div>
          </div>
        </motion.section>

        {/* ═══════ CTA FINAL ═══════ */}
        <section className="py-16 lg:py-20">
          <div className="container mx-auto px-4 max-w-[1280px]">
            <div className="relative overflow-hidden rounded-3xl bg-gradient-to-br from-primary to-primary/85 text-primary-foreground py-14 px-6 lg:px-16 text-center">
              <div className="absolute -top-20 -right-20 w-64 h-64 rounded-full bg-primary-foreground/5 blur-2xl pointer-events-none" />
              <div className="absolute -bottom-16 -left-16 w-48 h-48 rounded-full bg-primary-foreground/5 blur-2xl pointer-events-none" />

              <h2 className="text-2xl lg:text-3xl font-extrabold mb-4 relative">
                {t('ctaTitle')}
              </h2>
              <p className="text-primary-foreground/90 text-lg max-w-2xl mx-auto mb-8 leading-relaxed relative">
                {t('ctaSubtitle')}
              </p>

              <div className="flex flex-wrap gap-4 justify-center relative">
                <Link to="/entreprises">
                  <Button size="lg" variant="secondary" className="rounded-full px-10 font-semibold bg-primary-foreground text-primary hover:bg-primary-foreground/90 shadow-md">
                    {t('simulatorBtn')}
                  </Button>
                </Link>
                <Link to="/demande-acces-recruteur">
                  <Button size="lg" variant="outline" className="rounded-full px-10 font-semibold border-primary-foreground text-primary-foreground bg-primary-foreground/10 hover:bg-primary-foreground/20">
                    {t('ctaBtn')}
                  </Button>
                </Link>
              </div>
            </div>
          </div>
        </section>
      </main>

      <Footer />
    </div>
  );
}
