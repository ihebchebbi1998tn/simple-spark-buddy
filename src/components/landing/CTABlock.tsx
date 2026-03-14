import { Button } from "@/components/ui/button"
import { useNavigate } from "react-router-dom"
import { motion } from "framer-motion"
import { useScrollAnimation } from "@/hooks/useScrollAnimation"
import { ArrowRight } from "lucide-react"
import { useTranslation } from "react-i18next"

const CTABlock = () => {
  const navigate = useNavigate()
  const { ref, isVisible } = useScrollAnimation()
  const { t } = useTranslation('cta')

  return (
    <section ref={ref} className="py-10 lg:py-16 px-4">
      <motion.div
        className="rounded-3xl py-16 lg:py-20 mx-auto max-w-[1100px] text-center relative overflow-hidden bg-primary"
        initial={{ opacity: 0, y: 16 }}
        animate={isVisible ? { opacity: 1, y: 0 } : {}}
        transition={{ duration: 0.4 }}
      >
        <div className="relative z-10 px-6 lg:px-8 max-w-[600px] mx-auto">
          <div className="inline-flex items-center gap-2 px-3 py-1.5 rounded-full bg-primary-foreground/10 border border-primary-foreground/15 mb-6">
            <span className="text-[11px] font-semibold text-primary-foreground/70 uppercase tracking-wide">{t('badge')}</span>
          </div>
          <h2 className="text-xl lg:text-[1.65rem] font-bold text-primary-foreground mb-4 leading-tight tracking-[-0.01em]">
            {t('title')}
          </h2>
          <p className="text-primary-foreground/55 mb-9 max-w-md mx-auto text-sm leading-relaxed">
            {t('subtitle')}
          </p>
          <div className="flex flex-wrap gap-3 justify-center">
            <Button
              size="lg"
              className="rounded-xl px-7 h-11 font-semibold text-sm bg-primary-foreground text-primary hover:bg-primary-foreground/90 shadow-sm transition-colors gap-2"
              onClick={() => navigate("/candidats")}
            >
              {t('btnCandidate')}
              <ArrowRight className="w-3.5 h-3.5" />
            </Button>
            <Button
              size="lg"
              className="rounded-xl px-7 h-11 font-semibold text-sm bg-transparent text-primary-foreground hover:bg-primary-foreground/10 border border-primary-foreground/25 shadow-none transition-colors gap-2"
              onClick={() => navigate("/entreprises")}
            >
              {t('btnRecruiter')}
              <ArrowRight className="w-3.5 h-3.5" />
            </Button>
          </div>
        </div>
      </motion.div>
    </section>
  )
}

export default CTABlock
