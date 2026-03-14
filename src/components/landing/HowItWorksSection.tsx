import { useState } from "react"
import { motion, AnimatePresence } from "framer-motion"
import { useScrollAnimation } from "@/hooks/useScrollAnimation"
import { Button } from "@/components/ui/button"
import { useNavigate } from "react-router-dom"
import { ArrowRight } from "lucide-react"
import { useTranslation } from "react-i18next"

const HowItWorksSection = () => {
  const { ref, isVisible } = useScrollAnimation()
  const [activeTab, setActiveTab] = useState<"candidat" | "recruteur">("candidat")
  const navigate = useNavigate()
  const { t } = useTranslation('howItWorks')

  const candidateSteps = t('candidateSteps', { returnObjects: true }) as { title: string; description: string }[]
  const recruiterSteps = t('recruiterSteps', { returnObjects: true }) as { title: string; description: string }[]

  const steps = activeTab === "candidat" ? candidateSteps : recruiterSteps

  return (
    <motion.section
      ref={ref}
      className="py-20 lg:py-28 bg-muted/40"
      initial={{ opacity: 0 }}
      animate={isVisible ? { opacity: 1 } : { opacity: 0 }}
      transition={{ duration: 0.5 }}
    >
      <div className="container mx-auto px-4 lg:px-8 max-w-[1100px]">
        {/* Header */}
        <motion.div
          className="text-center mb-10"
          initial={{ opacity: 0, y: 16 }}
          animate={isVisible ? { opacity: 1, y: 0 } : {}}
          transition={{ duration: 0.4 }}
        >
          <span className="inline-block text-[11px] font-semibold text-primary uppercase tracking-widest mb-3">{t('badge')}</span>
          <h2 className="text-2xl lg:text-[2rem] font-bold text-foreground mb-3 leading-tight tracking-[-0.01em]">
            {t('title')}
          </h2>
          <p className="text-muted-foreground text-[15px]">{t('subtitle')}</p>
        </motion.div>

        {/* Tabs */}
        <div className="flex justify-center mb-12">
          <div className="inline-flex bg-muted rounded-xl p-1 gap-0.5 border border-border/50">
            {(["candidat", "recruteur"] as const).map((tab) => (
              <button
                key={tab}
                onClick={() => setActiveTab(tab)}
                className={`px-6 py-2.5 rounded-lg font-medium text-sm transition-colors ${
                  activeTab === tab
                    ? "bg-background text-foreground shadow-sm border border-border/60"
                    : "text-muted-foreground hover:text-foreground"
                }`}
              >
                {tab === "candidat" ? t('tabCandidate') : t('tabRecruiter')}
              </button>
            ))}
          </div>
        </div>

        {/* Steps Grid */}
        <AnimatePresence mode="wait">
          <motion.div
            key={activeTab}
            className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4"
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            exit={{ opacity: 0, y: -8 }}
            transition={{ duration: 0.3 }}
          >
            {steps.map((step, i) => (
              <motion.div
                key={i}
                className="bg-card border border-border rounded-2xl p-6 transition-colors hover:border-primary/20 group relative"
                initial={{ opacity: 0, y: 12 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: i * 0.06, duration: 0.3 }}
              >
                {i < steps.length - 1 && (
                  <div className="hidden lg:block absolute top-[28px] -right-2 w-4 h-px bg-border" />
                )}
                <div className="w-11 h-11 rounded-xl flex items-center justify-center text-lg font-bold text-primary-foreground bg-primary mb-4">
                  {i + 1}
                </div>
                <h4 className="text-[15px] font-bold text-foreground mb-2">{step.title}</h4>
                <p className="text-[13px] text-muted-foreground leading-relaxed">{step.description}</p>
              </motion.div>
            ))}
          </motion.div>
        </AnimatePresence>

        {/* CTA row */}
        <motion.div
          className="flex flex-wrap justify-center gap-3 mt-12"
          initial={{ opacity: 0 }}
          animate={isVisible ? { opacity: 1 } : {}}
          transition={{ delay: 0.3, duration: 0.3 }}
        >
          <Button
            variant="outline"
            className="rounded-xl px-6 h-10 text-[13px] font-semibold border-border text-foreground/70 hover:text-primary hover:border-primary/30 hover:bg-primary/5 transition-colors gap-2"
            onClick={() => navigate("/candidats")}
          >
            {t('btnCandidate')} <ArrowRight className="w-3.5 h-3.5" />
          </Button>
          <Button
            variant="outline"
            className="rounded-xl px-6 h-10 text-[13px] font-semibold border-border text-foreground/70 hover:text-primary hover:border-primary/30 hover:bg-primary/5 transition-colors gap-2"
            onClick={() => navigate("/entreprises")}
          >
            {t('btnRecruiter')} <ArrowRight className="w-3.5 h-3.5" />
          </Button>
        </motion.div>
      </div>
    </motion.section>
  )
}

export default HowItWorksSection
