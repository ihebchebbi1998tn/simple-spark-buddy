import { motion } from "framer-motion";
import { useScrollAnimation } from "@/hooks/useScrollAnimation";
import Header from "@/components/shared/Header";
import Footer from "@/components/shared/Footer";
import { Button } from "@/components/ui/button";
import { Link } from "react-router-dom";
import {
  BarChart3,
  Zap,
  Target,
  Users,
  CheckCircle2,
  ChevronDown,
  TrendingUp,
  Shield,
  Briefcase,
  ArrowRight,
  Sparkles,
} from "lucide-react";
import { useState } from "react";
import { useTranslation } from "react-i18next";

const advIcons = [Target, Zap, TrendingUp, Shield];

const faqs = [
  { q: "recruiters:faq1q", a: "recruiters:faq1a" },
];

const stats = [
  { number: "12k+", label: "stat1" },
  { number: "15+", label: "stat2" },
  { number: "98%", label: "stat3" },
  { number: "48h", label: "stat4" },
];

const FAQItem = ({ q, a, index, isVisible }: { q: string; a: string; index: number; isVisible: boolean }) => {
  const [open, setOpen] = useState(false);
  return (
    <motion.div
      className="bg-card border border-border rounded-2xl px-6 py-5 cursor-pointer transition-all duration-300 hover:border-primary/15 group"
      onClick={() => setOpen(!open)}
      initial={{ opacity: 0, y: 10 }}
      animate={isVisible ? { opacity: 1, y: 0 } : {}}
      transition={{ delay: index * 0.05, duration: 0.3 }}
    >
      <div className="flex items-center justify-between gap-3">
        <span className="font-semibold text-foreground text-sm">{q}</span>
        <ChevronDown className={`w-4 h-4 text-muted-foreground/60 shrink-0 transition-transform duration-300 ${open ? "rotate-180" : ""}`} />
      </div>
      {open && <p className="mt-3 text-muted-foreground text-[13px] leading-relaxed">{a}</p>}
    </motion.div>
  );
};

const RecruteursInfo = () => {
  const { ref: heroRef, isVisible: heroVisible } = useScrollAnimation();
  const { ref: statsRef, isVisible: statsVisible } = useScrollAnimation();
  const { ref: advRef, isVisible: advVisible } = useScrollAnimation();
  const { ref: faqRef, isVisible: faqVisible } = useScrollAnimation();
  const { t } = useTranslation(['recruiters', 'common', 'hero']);

  const advantages = t('advantages', { returnObjects: true }) as { title: string; description: string }[];
  const features = t('features', { returnObjects: true }) as string[];

  const recruiterFaqs = [
    { q: "Comment fonctionne le paiement au résultat ?", a: "Vous ne payez que lorsqu'un candidat est effectivement embauché. Pas de frais cachés, pas d'abonnement." },
    { q: "Puis-je simuler sans m'engager ?", a: "Oui, le simulateur est 100% gratuit et sans engagement." },
    { q: "Les données sont-elles protégées ?", a: "Oui, nous sommes conformes RGPD et vos données sont chiffrées." },
    { q: "Combien de candidats dans le vivier ?", a: "Plus de 12 000 profils qualifiés dans les centres d'appels." },
  ];

  const statItems = [
    { number: "12k+", label: t('hero:stat1', { ns: 'hero' }) },
    { number: "15+", label: t('hero:stat2', { ns: 'hero' }) },
    { number: "98%", label: t('hero:stat3', { ns: 'hero' }) },
    { number: "48h", label: t('recruiters:startNow') },
  ];

  return (
    <div className="min-h-screen flex flex-col bg-background">
      <Header />

      <main className="flex-1">
        {/* ===== HERO ===== */}
        <section ref={heroRef} className="relative overflow-hidden">
          <div className="absolute top-0 right-0 w-[800px] h-[800px] bg-[radial-gradient(circle,hsl(var(--primary)/0.04)_0%,transparent_60%)] pointer-events-none" />
          <div className="container mx-auto px-4 lg:px-8 max-w-[1200px] py-16 lg:py-24">
            <div className="grid lg:grid-cols-2 gap-12 lg:gap-16 items-center">
              <motion.div
                initial={{ opacity: 0, y: 24 }}
                animate={heroVisible ? { opacity: 1, y: 0 } : {}}
                transition={{ duration: 0.7, ease: [0.25, 0.46, 0.45, 0.94] }}
              >
                <motion.div
                  className="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full border border-primary/15 bg-primary/5 mb-6"
                  initial={{ opacity: 0, y: 10 }}
                  animate={heroVisible ? { opacity: 1, y: 0 } : {}}
                  transition={{ delay: 0.15, duration: 0.4 }}
                >
                  <Sparkles className="w-3.5 h-3.5 text-primary" />
                  <span className="text-[11px] font-semibold text-primary tracking-wide uppercase">{t('badge')}</span>
                </motion.div>

                <h1 className="text-[1.85rem] lg:text-[2.6rem] xl:text-[3rem] font-extrabold text-foreground leading-[1.12] tracking-[-0.02em] mb-5">
                  {t('title')}{" "}
                  <span className="text-primary">{t('titleHighlight')}</span> {t('titleEnd')}
                </h1>
                <p className="text-[15px] lg:text-base text-muted-foreground max-w-[520px] mb-9 leading-[1.75]">
                  {t('subtitle')}
                </p>
                <motion.div
                  className="flex flex-wrap gap-3"
                  initial={{ opacity: 0, y: 12 }}
                  animate={heroVisible ? { opacity: 1, y: 0 } : {}}
                  transition={{ delay: 0.35, duration: 0.45 }}
                >
                  <Link to="/entreprises">
                    <Button size="lg" className="rounded-xl px-7 h-12 text-sm font-semibold shadow-[0_4px_14px_hsl(var(--primary)/0.25)] hover:shadow-[0_6px_20px_hsl(var(--primary)/0.35)] transition-all duration-300 gap-2 hover:-translate-y-0.5">
                      <BarChart3 className="w-4 h-4" />
                      {t('simulateBtn')}
                      <ArrowRight className="w-3.5 h-3.5" />
                    </Button>
                  </Link>
                  <Link to="/recruteurs/login">
                    <Button size="lg" variant="outline" className="rounded-xl border-border/80 text-foreground hover:text-primary hover:border-primary/30 hover:bg-primary/5 px-7 h-12 text-sm font-semibold transition-all duration-300 gap-2 hover:-translate-y-0.5">
                      {t('accessBtn')}
                      <ArrowRight className="w-3.5 h-3.5" />
                    </Button>
                  </Link>
                </motion.div>
              </motion.div>

              {/* Right – features card */}
              <motion.div
                initial={{ opacity: 0, scale: 0.96, x: 20 }}
                animate={heroVisible ? { opacity: 1, scale: 1, x: 0 } : {}}
                transition={{ duration: 0.7, delay: 0.2, ease: [0.25, 0.46, 0.45, 0.94] }}
                className="relative rounded-2xl p-8 lg:p-9 text-white overflow-hidden"
                style={{
                  background: "linear-gradient(150deg, hsl(215 100% 18%), hsl(var(--primary)), hsl(215 100% 40%))",
                  boxShadow: "0 20px 60px hsl(var(--primary) / 0.2)",
                }}
              >
                <div className="absolute -top-16 -right-16 w-48 h-48 rounded-full bg-white/5 pointer-events-none" />
                <div className="absolute -bottom-12 -left-12 w-36 h-36 rounded-full bg-white/5 pointer-events-none" />
                <div className="relative z-10">
                  <div className="flex items-center gap-3 mb-7">
                    <div className="w-11 h-11 bg-white/10 rounded-xl flex items-center justify-center">
                      <Briefcase className="w-5 h-5 text-white/80" />
                    </div>
                    <h3 className="text-lg font-bold text-white">{t('included')}</h3>
                  </div>
                  <ul className="space-y-0">
                    {features.map((f, i) => (
                      <motion.li
                        key={i}
                        className="flex items-center gap-3 py-3 border-b border-white/8 last:border-0"
                        initial={{ opacity: 0, x: 10 }}
                        animate={heroVisible ? { opacity: 1, x: 0 } : {}}
                        transition={{ delay: 0.35 + i * 0.06, duration: 0.3 }}
                      >
                        <CheckCircle2 className="w-4 h-4 text-white/50 shrink-0" />
                        <span className="text-white/85 font-medium text-sm">{f}</span>
                      </motion.li>
                    ))}
                  </ul>
                </div>
              </motion.div>
            </div>
          </div>
        </section>

        {/* ===== STATS BAR ===== */}
        <section ref={statsRef} className="border-y border-border/40 bg-muted/20">
          <div className="container mx-auto px-4 lg:px-8 max-w-[1200px]">
            <div className="grid grid-cols-2 lg:grid-cols-4 gap-0">
              {statItems.map((stat, i) => (
                <motion.div
                  key={stat.label}
                  className="py-8 lg:py-10 text-center border-r border-border/30 last:border-0 rtl:border-r-0 rtl:border-l rtl:border-border/30 rtl:last:border-0"
                  initial={{ opacity: 0, y: 12 }}
                  animate={statsVisible ? { opacity: 1, y: 0 } : {}}
                  transition={{ delay: i * 0.08, duration: 0.35 }}
                >
                  <div className="text-2xl lg:text-3xl font-extrabold text-primary mb-1">{stat.number}</div>
                  <div className="text-[11px] uppercase tracking-[0.12em] font-semibold text-muted-foreground/70">{stat.label}</div>
                </motion.div>
              ))}
            </div>
          </div>
        </section>

        {/* ===== ADVANTAGES ===== */}
        <section ref={advRef} className="py-20 lg:py-28" style={{ background: "linear-gradient(180deg, hsl(var(--background)) 0%, hsl(var(--muted) / 0.3) 100%)" }}>
          <div className="container mx-auto px-4 lg:px-8 max-w-[1100px]">
            <motion.div className="text-center mb-14" initial={{ opacity: 0, y: 16 }} animate={advVisible ? { opacity: 1, y: 0 } : {}} transition={{ duration: 0.5 }}>
              <span className="inline-block text-[11px] font-semibold text-primary uppercase tracking-widest mb-3">{t('whyUs')}</span>
              <h2 className="text-2xl lg:text-[2rem] font-bold text-foreground mb-3 leading-tight tracking-[-0.01em]">{t('whyUsTitle')}</h2>
              <p className="text-muted-foreground text-[15px] max-w-[480px] mx-auto leading-relaxed">{t('whyUsSubtitle')}</p>
            </motion.div>
            <div className="grid sm:grid-cols-2 lg:grid-cols-4 gap-5">
              {advantages.map((adv, i) => {
                const Icon = advIcons[i] || Target;
                return (
                  <motion.div
                    key={i}
                    className="bg-card border border-border rounded-2xl p-7 text-center transition-all duration-300 hover:border-primary/15 hover:shadow-[0_8px_28px_hsl(var(--primary)/0.06)] hover:-translate-y-0.5"
                    initial={{ opacity: 0, y: 20 }}
                    animate={advVisible ? { opacity: 1, y: 0 } : {}}
                    transition={{ duration: 0.45, delay: 0.08 * i }}
                  >
                    <div className="w-12 h-12 rounded-xl flex items-center justify-center mx-auto mb-5" style={{ background: "linear-gradient(135deg, hsl(var(--primary) / 0.1), hsl(var(--primary) / 0.05))" }}>
                      <Icon className="w-5 h-5 text-primary/70" />
                    </div>
                    <h4 className="font-bold text-foreground text-[15px] mb-2">{adv.title}</h4>
                    <p className="text-[13px] text-muted-foreground leading-relaxed">{adv.description}</p>
                  </motion.div>
                );
              })}
            </div>
          </div>
        </section>

        {/* ===== CTA ===== */}
        <section className="py-10 lg:py-16 px-4">
          <motion.div
            className="rounded-3xl py-16 lg:py-20 mx-auto max-w-[1100px] text-center relative overflow-hidden"
            style={{ background: "linear-gradient(150deg, hsl(215 100% 16%), hsl(var(--primary)), hsl(215 100% 42%))", boxShadow: "0 16px 48px hsl(var(--primary) / 0.22)" }}
            initial={{ opacity: 0, y: 16 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            transition={{ duration: 0.5 }}
          >
            <div className="absolute -top-24 -right-24 w-72 h-72 rounded-full bg-white/5 pointer-events-none" />
            <div className="absolute -bottom-20 -left-20 w-56 h-56 rounded-full bg-white/5 pointer-events-none" />
            <div className="relative z-10 px-6 lg:px-8 max-w-[580px] mx-auto">
              <div className="inline-flex items-center gap-2 px-3 py-1.5 rounded-full bg-white/10 border border-white/15 mb-6">
                <Sparkles className="w-3.5 h-3.5 text-white/70" />
                <span className="text-[11px] font-semibold text-white/70 uppercase tracking-wide">{t('startNow')}</span>
              </div>
              <h2 className="text-xl lg:text-[1.65rem] font-bold text-white mb-4 leading-tight tracking-[-0.01em]">{t('readyToRecruit')}</h2>
              <div className="flex flex-wrap gap-3 justify-center">
                <Link to="/demande-acces-recruteur">
                  <Button size="lg" className="rounded-xl px-7 h-11 font-semibold text-sm bg-white text-primary hover:bg-white/90 shadow-[0_4px_14px_rgba(0,0,0,0.15)] transition-all duration-300 gap-2 hover:-translate-y-0.5">
                    {t('requestAccess')} <ArrowRight className="w-3.5 h-3.5" />
                  </Button>
                </Link>
                <Link to="/entreprises">
                  <Button size="lg" className="rounded-xl px-7 h-11 font-semibold text-sm bg-transparent text-white hover:bg-white/10 border border-white/25 shadow-none transition-all duration-300 gap-2 hover:-translate-y-0.5">
                    {t('trySimulator')} <ArrowRight className="w-3.5 h-3.5" />
                  </Button>
                </Link>
              </div>
            </div>
          </motion.div>
        </section>

        {/* ===== FAQ ===== */}
        <section ref={faqRef} className="py-20 lg:py-28" style={{ background: "linear-gradient(180deg, hsl(var(--muted) / 0.4) 0%, hsl(var(--background)) 100%)" }}>
          <div className="container mx-auto px-4 lg:px-8 max-w-[800px]">
            <motion.div className="text-center mb-14" initial={{ opacity: 0, y: 16 }} animate={faqVisible ? { opacity: 1, y: 0 } : {}} transition={{ duration: 0.5 }}>
              <span className="inline-block text-[11px] font-semibold text-primary uppercase tracking-widest mb-3">FAQ</span>
              <h2 className="text-2xl lg:text-[2rem] font-bold text-foreground mb-3 leading-tight tracking-[-0.01em]">{t('faqTitle')}</h2>
              <p className="text-muted-foreground text-[15px] max-w-md mx-auto">{t('faqSubtitle')}</p>
            </motion.div>
            <div className="space-y-3">
              {recruiterFaqs.map((f, i) => (
                <FAQItem key={i} q={f.q} a={f.a} index={i} isVisible={faqVisible} />
              ))}
            </div>
          </div>
        </section>

        {/* ===== FINAL CTA ===== */}
        <section className="py-16 lg:py-20">
          <div className="container mx-auto px-4 lg:px-8 max-w-[1100px]">
            <motion.div
              className="bg-card border border-border rounded-3xl p-10 lg:p-16 text-center relative overflow-hidden"
              initial={{ opacity: 0, y: 16 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{ duration: 0.5 }}
            >
              <h3 className="text-2xl lg:text-[1.75rem] font-bold text-foreground mb-3 tracking-[-0.01em]">{t('ctaTitle')}</h3>
              <p className="text-muted-foreground text-[15px] mb-9 max-w-xl mx-auto leading-relaxed">{t('ctaSubtitle')}</p>
              <div className="flex flex-wrap gap-3 justify-center">
                <Link to="/demande-acces-recruteur">
                  <Button size="lg" className="rounded-xl px-7 h-12 font-semibold text-sm shadow-[0_4px_14px_hsl(var(--primary)/0.25)] hover:shadow-[0_6px_20px_hsl(var(--primary)/0.35)] transition-all duration-300 gap-2 hover:-translate-y-0.5">
                    {t('iAmRecruiter')} <ArrowRight className="w-3.5 h-3.5" />
                  </Button>
                </Link>
                <Link to="/candidats">
                  <Button size="lg" variant="outline" className="rounded-xl border-border/80 text-foreground hover:text-primary hover:border-primary/30 hover:bg-primary/5 px-7 h-12 font-semibold text-sm transition-all duration-300 gap-2 hover:-translate-y-0.5">
                    {t('iAmCandidate')} <ArrowRight className="w-3.5 h-3.5" />
                  </Button>
                </Link>
              </div>
            </motion.div>
          </div>
        </section>
      </main>

      <Footer />
    </div>
  );
};

export default RecruteursInfo;
