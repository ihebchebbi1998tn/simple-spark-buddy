import { useScrollAnimation } from "@/hooks/useScrollAnimation"
import { Quote } from "lucide-react"
import { motion } from "framer-motion"
import { useTranslation } from "react-i18next"

const TestimonialsSection = () => {
  const { ref, isVisible } = useScrollAnimation()
  const { t } = useTranslation('testimonials')

  const testimonials = t('testimonials', { returnObjects: true }) as { name: string; role: string; initial: string; content: string }[]

  return (
    <section ref={ref} className="py-20 lg:py-28 bg-background">
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
          <p className="text-muted-foreground text-[15px] max-w-md mx-auto">
            {t('subtitle')}
          </p>
        </motion.div>

        <div className="grid md:grid-cols-3 gap-5">
          {testimonials.map((testimonial, i) => (
            <motion.div
              key={i}
              className="bg-card border border-border rounded-2xl p-7 flex flex-col h-full transition-colors hover:border-primary/15 relative"
              initial={{ opacity: 0, y: 14 }}
              animate={isVisible ? { opacity: 1, y: 0 } : {}}
              transition={{ delay: 0.08 * i, duration: 0.35 }}
            >
              <Quote className="w-8 h-8 text-primary/10 mb-4" />
              <p className="text-sm text-foreground/70 leading-[1.8] flex-1 mb-6">
                "{testimonial.content}"
              </p>
              <div className="flex items-center gap-3 pt-5 border-t border-border/40">
                <div className="w-10 h-10 rounded-xl flex items-center justify-center text-primary-foreground font-semibold text-sm flex-shrink-0 bg-primary">
                  {testimonial.initial}
                </div>
                <div>
                  <div className="font-semibold text-foreground text-sm">{testimonial.name}</div>
                  <div className="text-[12px] text-muted-foreground mt-0.5">{testimonial.role}</div>
                </div>
              </div>
            </motion.div>
          ))}
        </div>
      </div>
    </section>
  )
}

export default TestimonialsSection
