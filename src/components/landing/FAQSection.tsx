import { useScrollAnimation } from "@/hooks/useScrollAnimation"
import { ChevronDown } from "lucide-react"
import { motion } from "framer-motion"
import { useTranslation } from "react-i18next"

const FAQSection = () => {
  const { ref, isVisible } = useScrollAnimation()
  const { t } = useTranslation('faq')

  const faqs = t('faqs', { returnObjects: true }) as { question: string; answer: string }[]

  return (
    <section ref={ref} id="faq" className="py-20 lg:py-28 bg-muted/40">
      <div className="container mx-auto px-4 lg:px-8 max-w-[800px]">
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
          <p className="text-muted-foreground text-[15px] max-w-md mx-auto">
            {t('subtitle')}
          </p>
        </motion.div>

        <div className="space-y-3">
          {faqs.map((faq, index) => (
            <motion.details
              key={index}
              className="bg-card border border-border rounded-2xl px-6 py-5 cursor-pointer transition-colors hover:border-primary/15 group [&[open]]:border-primary/15"
              initial={{ opacity: 0, y: 10 }}
              animate={isVisible ? { opacity: 1, y: 0 } : {}}
              transition={{ delay: index * 0.04, duration: 0.25 }}
            >
              <summary className="font-semibold text-foreground text-sm list-none flex items-center justify-between cursor-pointer gap-4 py-0.5">
                {faq.question}
                <ChevronDown className="w-4 h-4 text-muted-foreground/60 flex-shrink-0 transition-transform duration-200 group-open:rotate-180" />
              </summary>
              <p className="mt-3 text-[13px] text-muted-foreground leading-[1.7] pb-1">
                {faq.answer}
              </p>
            </motion.details>
          ))}
        </div>
      </div>
    </section>
  )
}

export default FAQSection
