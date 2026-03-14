"use client"

import { Button } from "@/components/ui/button"
import { motion } from "framer-motion"
import { useScrollAnimation } from "@/hooks/useScrollAnimation"
import { Languages, CheckCircle2, Sparkles, Trophy, Target, Users, Briefcase } from "lucide-react"
import { useNavigate } from "react-router-dom"
import { useTranslation } from "react-i18next"

const FrenchTestSection = () => {
  const { ref, isVisible } = useScrollAnimation()
  const navigate = useNavigate()
  const { t } = useTranslation('hero')

  const benefits = [
    t('test.benefit1'),
    t('test.benefit2'),
    t('test.benefit3'),
    t('test.benefit4'),
    t('test.benefit5'),
  ]

  return (
    <>
      <motion.section
        id="test-niveau"
        ref={ref}
        className="py-20 lg:py-32 relative overflow-hidden"
        initial={{ opacity: 0 }}
        animate={isVisible ? { opacity: 1 } : { opacity: 0 }}
        transition={{ duration: 0.8 }}
      >
        <div className="absolute inset-0 bg-gradient-to-br from-primary/5 via-transparent to-blue-500/5" />
        <div className="absolute top-10 right-10 w-72 h-72 bg-primary/10 rounded-full blur-3xl" />
        <div className="absolute bottom-10 left-10 w-96 h-96 bg-blue-500/10 rounded-full blur-3xl" />

        <div className="container mx-auto px-4 lg:px-8 max-w-6xl relative z-10">
          <div className="grid lg:grid-cols-2 gap-12 items-center">
            <motion.div
              initial={{ opacity: 0, x: -50 }}
              animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: -50 }}
              transition={{ duration: 0.8, delay: 0.2 }}
            >
              <h2 className="text-3xl lg:text-5xl font-bold text-brand-dark mb-6 leading-tight">
                {t('test.title').split('<1>').map((part, i) => 
                  i === 0 ? part : <span key={i}><span className="text-primary">{part.split('</1>')[0]}</span>{part.split('</1>')[1]}</span>
                )}
              </h2>

              <p className="text-lg text-muted-foreground mb-8 leading-relaxed">
                {t('test.description')}
              </p>

              <div className="space-y-4 mb-8">
                {benefits.map((benefit, index) => (
                  <motion.div
                    key={index}
                    className="flex items-center gap-3"
                    initial={{ opacity: 0, x: -20 }}
                    animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: -20 }}
                    transition={{ duration: 0.5, delay: 0.3 + index * 0.1 }}
                  >
                    <div className="w-6 h-6 rounded-full bg-green-500/20 flex items-center justify-center flex-shrink-0">
                      <CheckCircle2 className="w-4 h-4 text-green-600" />
                    </div>
                    <span className="text-brand-dark font-medium">{benefit}</span>
                  </motion.div>
                ))}
              </div>

              <motion.div
                initial={{ opacity: 0, y: 20 }}
                animate={isVisible ? { opacity: 1, y: 0 } : { opacity: 0, y: 20 }}
                transition={{ duration: 0.6, delay: 0.7 }}
              >
                <Button
                  size="lg"
                  onClick={() => navigate("/test-langue")}
                  className="bg-primary hover:bg-primary/90 text-white px-8 py-6 text-lg font-semibold rounded-xl shadow-lg hover:shadow-xl transition-all duration-300 hover:scale-105"
                >
                  <Languages className="w-5 h-5 mr-2" />
                  {t('test.startTest')}
                </Button>
                <p className="text-sm text-muted-foreground mt-3">{t('test.duration')}</p>
              </motion.div>
            </motion.div>

            <motion.div
              className="relative"
              initial={{ opacity: 0, scale: 0.9 }}
              animate={isVisible ? { opacity: 1, scale: 1 } : { opacity: 0, scale: 0.9 }}
              transition={{ duration: 0.8, delay: 0.4 }}
            >
              <div className="relative bg-gradient-to-br from-background via-card to-primary/5 rounded-3xl p-6 shadow-2xl border border-primary/20">
                <div className="space-y-3">
                  {[
                    { label: t('test.langLevel'), rating: t('test.excellent'), score: 90, icon: Languages, color: "from-green-500 to-emerald-600" },
                    { label: t('test.softSkills'), rating: t('test.excellent'), score: 85, icon: Users, color: "from-blue-500 to-cyan-600" },
                    { label: t('test.jobSkills'), rating: t('test.acceptable'), score: 65, icon: Briefcase, color: "from-orange-500 to-amber-600" },
                    { label: t('test.results'), rating: t('test.congrats'), score: 80, icon: Target, color: "from-purple-500 to-pink-600" },
                  ].map((item, index) => {
                    const Icon = item.icon
                    return (
                      <motion.div
                        key={item.label}
                        className="group relative p-4 bg-card/80 backdrop-blur-sm rounded-xl shadow-lg border border-primary/10 hover:border-primary/30 hover:shadow-xl transition-all duration-300"
                        initial={{ opacity: 0, x: 50 }}
                        animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: 50 }}
                        transition={{ duration: 0.5, delay: 0.5 + index * 0.1 }}
                      >
                        <div className="flex items-center gap-3 mb-2">
                          <div className={`w-10 h-10 rounded-lg bg-gradient-to-br ${item.color} flex items-center justify-center shadow-lg`}>
                            <Icon className="w-5 h-5 text-white" />
                          </div>
                          <div className="flex-1">
                            <div className="text-sm font-semibold text-foreground">{item.label}</div>
                            <div className="text-xs text-muted-foreground">{item.rating}</div>
                          </div>
                          <span className="text-lg font-bold text-primary">{item.score}%</span>
                        </div>
                        <div className="h-2 bg-muted rounded-full overflow-hidden">
                          <motion.div
                            className={`h-full bg-gradient-to-r ${item.color}`}
                            initial={{ width: 0 }}
                            animate={isVisible ? { width: `${item.score}%` } : { width: 0 }}
                            transition={{ duration: 0.8, delay: 0.8 + index * 0.1 }}
                          />
                        </div>
                      </motion.div>
                    )
                  })}
                </div>

                <motion.div
                  className="mt-4 p-4 bg-gradient-to-r from-green-500/10 to-emerald-500/10 border border-green-500/20 rounded-xl"
                  initial={{ opacity: 0, y: 20 }}
                  animate={isVisible ? { opacity: 1, y: 0 } : { opacity: 0, y: 20 }}
                  transition={{ duration: 0.6, delay: 1.2 }}
                >
                  <p className="text-sm text-center text-foreground font-medium">
                    {t('test.profileAppreciated')}
                  </p>
                </motion.div>

                <motion.div
                  className="absolute -top-3 -right-3 sm:-top-5 sm:-right-5 bg-gradient-to-br from-yellow-400 via-orange-500 to-red-500 text-white p-3 sm:p-5 rounded-xl sm:rounded-2xl shadow-2xl"
                  animate={{ y: [0, -10, 0], rotate: [0, 5, 0] }}
                  transition={{ duration: 3, repeat: Number.POSITIVE_INFINITY, ease: "easeInOut" }}
                >
                  <Trophy className="w-5 h-5 sm:w-7 sm:h-7 mb-1" />
                  <Sparkles className="w-3 h-3 sm:w-4 sm:h-4 absolute top-1 right-1 sm:top-2 sm:right-2" />
                </motion.div>
              </div>
            </motion.div>
          </div>
        </div>
      </motion.section>
    </>
  )
}

export default FrenchTestSection
