import { CheckCircle2, ArrowRight, Users, Building2 } from "lucide-react"
import { useScrollAnimation } from "@/hooks/useScrollAnimation"
import { Button } from "@/components/ui/button"
import { useNavigate } from "react-router-dom"
import { motion } from "framer-motion"
import { useTranslation } from "react-i18next"

const ComparisonSection = () => {
  const { ref, isVisible } = useScrollAnimation()
  const navigate = useNavigate()
  const { t } = useTranslation('comparison')

  const candidateFeatures = t('candidateFeatures', { returnObjects: true }) as string[]
  const recruiterFeatures = t('recruiterFeatures', { returnObjects: true }) as string[]

  return (
    <section ref={ref} className="py-20 lg:py-28 bg-background relative">
      <div className="container mx-auto px-4 lg:px-8 max-w-[1100px]">
        <motion.div
          className="text-center mb-14"
          initial={{ opacity: 0, y: 16 }}
          animate={isVisible ? { opacity: 1, y: 0 } : {}}
          transition={{ duration: 0.4 }}
        >
          <span className="inline-block text-[11px] font-semibold text-primary uppercase tracking-widest mb-3">{t('badge')}</span>
          <h2 className="text-2xl lg:text-[2rem] font-bold text-foreground mb-3 leading-tight tracking-[-0.01em]">
            {t('title')}
          </h2>
          <p className="text-muted-foreground text-[15px] max-w-[520px] mx-auto leading-relaxed">
            {t('subtitle')}
          </p>
        </motion.div>

        <div className="grid md:grid-cols-2 gap-6">
          {/* Candidate Column */}
          <motion.div
            className="bg-card border border-border rounded-2xl p-8 lg:p-9 transition-colors hover:border-primary/20"
            initial={{ opacity: 0, y: 16 }}
            animate={isVisible ? { opacity: 1, y: 0 } : {}}
            transition={{ duration: 0.4, delay: 0.1 }}
          >
            <div className="flex items-center gap-3 mb-7">
              <div className="w-10 h-10 rounded-xl bg-primary/8 flex items-center justify-center">
                <Users className="w-5 h-5 text-primary/60" />
              </div>
              <h3 className="text-lg font-bold text-foreground">{t('candidateTitle')}</h3>
            </div>

            <ul className="space-y-0 mb-8">
              {candidateFeatures.map((feature, i) => (
                <li key={i} className="flex items-center gap-3 text-sm text-muted-foreground py-2.5 border-b border-border/40 last:border-0">
                  <CheckCircle2 className="w-4 h-4 text-primary/50 flex-shrink-0" />
                  <span className="font-medium">{feature}</span>
                </li>
              ))}
            </ul>

            <Button
              variant="outline"
              className="rounded-xl px-6 h-10 font-semibold text-[13px] border-border text-foreground hover:text-primary hover:border-primary/30 hover:bg-primary/5 transition-colors gap-2"
              onClick={() => navigate("/candidats")}
            >
              {t('btnCandidate')}
              <ArrowRight className="w-3.5 h-3.5" />
            </Button>
          </motion.div>

          {/* Recruiter Column */}
          <motion.div
            className="rounded-2xl p-8 lg:p-9 bg-primary text-primary-foreground"
            initial={{ opacity: 0, y: 16 }}
            animate={isVisible ? { opacity: 1, y: 0 } : {}}
            transition={{ duration: 0.4, delay: 0.2 }}
          >
            <div className="flex items-center gap-3 mb-7">
              <div className="w-10 h-10 rounded-xl bg-primary-foreground/10 flex items-center justify-center">
                <Building2 className="w-5 h-5 text-primary-foreground/70" />
              </div>
              <h3 className="text-lg font-bold">{t('recruiterTitle')}</h3>
            </div>

            <ul className="space-y-0 mb-8">
              {recruiterFeatures.map((feature, i) => (
                <li key={i} className="flex items-center gap-3 text-sm text-primary-foreground/80 py-2.5 border-b border-primary-foreground/10 last:border-0">
                  <CheckCircle2 className="w-4 h-4 text-primary-foreground/50 flex-shrink-0" />
                  <span className="font-medium">{feature}</span>
                </li>
              ))}
            </ul>

            <Button
              className="rounded-xl px-6 h-10 font-semibold text-[13px] bg-primary-foreground/12 text-primary-foreground hover:bg-primary-foreground/20 border border-primary-foreground/15 shadow-none transition-colors gap-2"
              onClick={() => navigate("/entreprises")}
            >
              {t('btnRecruiter')}
              <ArrowRight className="w-3.5 h-3.5" />
            </Button>
          </motion.div>
        </div>
      </div>
    </section>
  )
}

export default ComparisonSection
