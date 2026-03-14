import { motion } from "framer-motion";
import { useScrollAnimation } from "@/hooks/useScrollAnimation";
import Header from "@/components/shared/Header";
import Footer from "@/components/shared/Footer";
import { Button } from "@/components/ui/button";
import CandidateInlineForm from "@/components/CandidateInlineForm";
import {
  UserCheck,
  UserPen,
  Bot,
  Handshake,
  Briefcase,
  ChevronDown,
} from "lucide-react";
import { useState } from "react";
import { useTranslation } from "react-i18next";

const stepIcons = [UserPen, Bot, Handshake, Briefcase];

const FAQItem = ({ q, a }: { q: string; a: string }) => {
  const [open, setOpen] = useState(false);
  return (
    <div
      className="bg-card border border-border rounded-2xl p-5 cursor-pointer transition-all hover:border-primary"
      onClick={() => setOpen(!open)}
    >
      <div className="flex items-center justify-between gap-3">
        <span className="font-semibold text-foreground">{q}</span>
        <ChevronDown
          className={`w-4 h-4 text-primary shrink-0 transition-transform ${open ? "rotate-180" : ""}`}
        />
      </div>
      {open && (
        <p className="mt-3 text-muted-foreground text-sm">{a}</p>
      )}
    </div>
  );
};

const Candidats = () => {
  const { ref: heroRef, isVisible: heroVisible } = useScrollAnimation();
  const { ref: stepsRef, isVisible: stepsVisible } = useScrollAnimation();
  const { ref: faqRef, isVisible: faqVisible } = useScrollAnimation();
  const { t } = useTranslation('candidates');

  const metrics = t('metrics', { returnObjects: true }) as { number: string; label: string }[];
  const steps = t('steps', { returnObjects: true }) as { title: string; description: string }[];
  const faqs = t('faqs', { returnObjects: true }) as { q: string; a: string }[];

  return (
    <div className="min-h-screen flex flex-col bg-background">
      <Header />

      <main className="flex-1">
        {/* ===== HERO ===== */}
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
                  {t('heroBadge')}
                </span>
                <h1 className="text-4xl lg:text-5xl font-extrabold text-foreground leading-tight mb-6">
                  {t('heroTitle')}
                </h1>
                <p className="text-lg text-muted-foreground max-w-lg mb-8">
                  {t('heroSubtitle')}
                </p>
                <Button
                  size="lg"
                  className="rounded-full px-8 font-semibold shadow-md"
                  onClick={() => document.getElementById('formulaire')?.scrollIntoView({ behavior: 'smooth' })}
                >
                  {t('heroBtn')}
                </Button>
              </motion.div>

              <motion.div
                initial={{ opacity: 0, x: 30 }}
                animate={heroVisible ? { opacity: 1, x: 0 } : {}}
                transition={{ duration: 0.6, delay: 0.2 }}
                className="bg-gradient-to-br from-primary/10 to-primary/5 rounded-3xl p-8 flex flex-col items-center justify-center border border-primary/10 min-h-[300px]"
              >
                <UserCheck className="w-20 h-20 text-primary mb-6" />
                <div className="flex gap-6 w-full justify-center">
                  {metrics.map((m) => (
                    <div key={m.label} className="text-center flex-1">
                      <div className="text-2xl lg:text-3xl font-extrabold text-foreground">
                        {m.number}
                      </div>
                      <div className="text-xs text-muted-foreground mt-1">
                        {m.label}
                      </div>
                    </div>
                  ))}
                </div>
              </motion.div>
            </div>
          </div>
        </motion.section>

        {/* ===== COMMENT ÇA MARCHE ===== */}
        <motion.section
          ref={stepsRef}
          className="py-16 bg-muted/30"
          initial={{ opacity: 0 }}
          animate={stepsVisible ? { opacity: 1 } : {}}
          transition={{ duration: 0.6 }}
        >
          <div className="container mx-auto px-4 max-w-[1200px]">
            <h2 className="text-3xl lg:text-4xl font-bold text-center text-foreground mb-6">
              {t('howItWorksTitle')}
            </h2>

            <div className="bg-card border border-border rounded-2xl p-6 text-center mb-10 max-w-3xl mx-auto">
              <h3 className="text-xl font-bold text-foreground mb-2">
                {t('howItWorksBlock')}
              </h3>
              <p className="text-muted-foreground">
                {t('howItWorksBlockDesc')}
              </p>
            </div>

            <div className="grid grid-cols-2 lg:grid-cols-4 gap-6">
              {steps.map((step, i) => {
                const Icon = stepIcons[i] || UserPen;
                return (
                  <motion.div
                    key={i}
                    className="bg-card border border-border rounded-2xl p-6 text-center transition-all hover:border-primary hover:shadow-md hover:-translate-y-1"
                    initial={{ opacity: 0, y: 30 }}
                    animate={stepsVisible ? { opacity: 1, y: 0 } : {}}
                    transition={{ duration: 0.5, delay: 0.1 * i }}
                  >
                    <div className="w-14 h-14 bg-primary/10 text-primary rounded-full flex items-center justify-center mx-auto mb-4">
                      <Icon className="w-6 h-6" />
                    </div>
                    <h4 className="font-bold text-foreground mb-2">
                      {step.title}
                    </h4>
                    <p className="text-sm text-muted-foreground">
                      {step.description}
                    </p>
                  </motion.div>
                );
              })}
            </div>
          </div>
        </motion.section>

        {/* ===== INLINE 6-STEP FORM ===== */}
        <section id="formulaire" className="py-16">
          <div className="container mx-auto px-4 max-w-[1200px]">
            <h2 className="text-3xl lg:text-4xl font-bold text-center text-foreground mb-4">
              {t('formTitle')}
            </h2>
            <p className="text-lg text-muted-foreground text-center mb-10 max-w-xl mx-auto">
              {t('formSubtitle')}
            </p>

            <CandidateInlineForm />
          </div>
        </section>

        {/* ===== FAQ ===== */}
        <motion.section
          ref={faqRef}
          className="py-16 bg-muted/30"
          initial={{ opacity: 0 }}
          animate={faqVisible ? { opacity: 1 } : {}}
          transition={{ duration: 0.6 }}
        >
          <div className="container mx-auto px-4 max-w-[1200px]">
            <h2 className="text-3xl lg:text-4xl font-bold text-center text-foreground mb-10">
              {t('faqTitle')}
            </h2>
            <div className="grid md:grid-cols-2 gap-4 max-w-4xl mx-auto">
              {faqs.map((f, i) => (
                <FAQItem key={i} q={f.q} a={f.a} />
              ))}
            </div>
          </div>
        </motion.section>

        {/* ===== CTA FINAL ===== */}
        <section className="py-16">
          <div className="container mx-auto px-4 max-w-[1200px]">
            <div className="bg-primary/5 rounded-3xl p-10 lg:p-16 text-center">
              <h3 className="text-2xl lg:text-3xl font-bold text-foreground mb-4">
                {t('ctaTitle')}
              </h3>
              <p className="text-lg text-muted-foreground mb-8 max-w-xl mx-auto">
                {t('ctaSubtitle')}
              </p>
              <Button
                size="lg"
                className="rounded-full px-10 font-semibold shadow-md"
                onClick={() => document.getElementById('formulaire')?.scrollIntoView({ behavior: 'smooth' })}
              >
                {t('ctaBtn')}
              </Button>
            </div>
          </div>
        </section>
      </main>

      <Footer />
    </div>
  );
};

export default Candidats;
