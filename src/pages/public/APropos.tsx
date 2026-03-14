import { motion } from "framer-motion"
import { Users, Clock, Headphones, Gift, TrendingUp, Bell, CheckCircle, Handshake } from "lucide-react"
import { Link } from "react-router-dom"
import { Button } from "@/components/ui/button"
import Header from "@/components/shared/Header"
import Footer from "@/components/shared/Footer"
import { useScrollAnimation } from "@/hooks/useScrollAnimation"
import { useTranslation } from "react-i18next"

const iconMap = [Users, Clock, Headphones, Gift, TrendingUp, Bell]

const APropos = () => {
  const { ref: heroRef, isVisible: heroVisible } = useScrollAnimation()
  const { ref: valuesRef, isVisible: valuesVisible } = useScrollAnimation()
  const { ref: storyRef, isVisible: storyVisible } = useScrollAnimation()
  const { t } = useTranslation('about')

  const features = t('features', { returnObjects: true }) as string[]
  const values = t('values', { returnObjects: true }) as { title: string; desc: string }[]

  return (
    <div className="min-h-screen flex flex-col bg-background">
      <Header />

      <main className="flex-1">
        {/* Hero Section */}
        <motion.section
          ref={heroRef}
          className="py-16 lg:py-20"
          initial={{ opacity: 0 }}
          animate={heroVisible ? { opacity: 1 } : { opacity: 0 }}
          transition={{ duration: 0.6 }}
        >
          <div className="container mx-auto px-4 lg:px-6 max-w-[1200px]">
            <div className="grid lg:grid-cols-2 gap-12 items-center">
              <motion.div
                initial={{ opacity: 0, x: -40 }}
                animate={heroVisible ? { opacity: 1, x: 0 } : {}}
                transition={{ duration: 0.6, delay: 0.1 }}
              >
                <span className="inline-block px-4 py-1.5 rounded-full text-sm font-semibold bg-primary/10 text-primary mb-4">
                  {t('mission')}
                </span>
                <h1 className="text-4xl lg:text-5xl font-extrabold text-foreground leading-tight mb-6">
                  {t('title')}
                </h1>
                <p className="text-lg text-muted-foreground leading-relaxed mb-8 max-w-lg" dangerouslySetInnerHTML={{ __html: t('description') }} />

                <div className="grid grid-cols-2 gap-4">
                  {features.map((f) => (
                    <div key={f} className="flex items-center gap-2.5 text-muted-foreground">
                      <CheckCircle className="w-4 h-4 text-primary shrink-0" />
                      <span className="text-sm font-medium">{f}</span>
                    </div>
                  ))}
                </div>
              </motion.div>

              <motion.div
                className="flex items-center justify-center"
                initial={{ opacity: 0, x: 40 }}
                animate={heroVisible ? { opacity: 1, x: 0 } : {}}
                transition={{ duration: 0.6, delay: 0.2 }}
              >
                <div className="w-full h-[320px] bg-gradient-to-br from-primary/10 to-primary/5 rounded-3xl flex items-center justify-center border border-primary/10">
                  <Handshake className="w-24 h-24 text-primary" />
                </div>
              </motion.div>
            </div>
          </div>
        </motion.section>

        {/* Value Cards */}
        <motion.section
          ref={valuesRef}
          className="py-16 lg:py-20 bg-muted/30"
          initial={{ opacity: 0, y: 30 }}
          animate={valuesVisible ? { opacity: 1, y: 0 } : {}}
          transition={{ duration: 0.6 }}
        >
          <div className="container mx-auto px-4 lg:px-6 max-w-[1200px]">
            <h2 className="text-3xl lg:text-4xl font-bold text-center text-foreground mb-12">
              {t('valuesTitle')}
            </h2>

            <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-5">
              {values.map((card, i) => {
                const Icon = iconMap[i] || Users
                return (
                  <motion.div
                    key={card.title}
                    className="bg-card border border-border rounded-2xl p-7 text-center hover:border-primary hover:shadow-lg transition-all duration-200 group"
                    initial={{ opacity: 0, y: 20 }}
                    animate={valuesVisible ? { opacity: 1, y: 0 } : {}}
                    transition={{ duration: 0.4, delay: i * 0.08 }}
                    whileHover={{ y: -4 }}
                  >
                    <div className="w-14 h-14 bg-primary/10 text-primary rounded-full flex items-center justify-center mx-auto mb-4">
                      <Icon className="w-6 h-6" />
                    </div>
                    <h4 className="font-bold text-foreground mb-2">{card.title}</h4>
                    <p className="text-sm text-muted-foreground">{card.desc}</p>
                  </motion.div>
                )
              })}
            </div>
          </div>
        </motion.section>

        {/* Founder Story */}
        <motion.section
          ref={storyRef}
          className="py-16 lg:py-20"
          initial={{ opacity: 0, y: 30 }}
          animate={storyVisible ? { opacity: 1, y: 0 } : {}}
          transition={{ duration: 0.6 }}
        >
          <div className="container mx-auto px-4 lg:px-6 max-w-[1200px]">
            <h2 className="text-3xl lg:text-4xl font-bold text-center text-foreground mb-10">
              {t('storyTitle')}
            </h2>

            <div className="bg-muted/50 rounded-3xl p-10 lg:p-12 border-l-[6px] border-primary rtl:border-l-0 rtl:border-r-[6px]">
              <p className="text-lg leading-relaxed text-muted-foreground italic">
                "{t('storyQuote')}"
              </p>
              <div className="mt-6 font-semibold text-primary">
                {t('storyAuthor')}
              </div>
            </div>
          </div>
        </motion.section>

        {/* CTA */}
        <section className="pb-16 lg:pb-20">
          <div className="container mx-auto px-4 lg:px-6 max-w-[1200px]">
            <div className="bg-primary/5 rounded-3xl p-12 lg:p-16 text-center">
              <h3 className="text-2xl lg:text-3xl font-bold text-foreground mb-4">
                {t('ctaTitle')}
              </h3>
              <p className="text-lg text-muted-foreground mb-8 max-w-xl mx-auto">
                {t('ctaSubtitle')}
              </p>
              <div className="flex flex-wrap gap-4 justify-center">
                <Link to="/pre-inscription">
                  <Button size="lg" variant="default" className="rounded-full px-8 font-semibold shadow-md">
                    {t('common:iAmCandidate', { ns: 'common' })}
                  </Button>
                </Link>
                <Link to="/entreprises">
                  <Button size="lg" className="rounded-full px-8 font-semibold shadow-md">
                    {t('common:iAmRecruiter', { ns: 'common' })}
                  </Button>
                </Link>
              </div>
            </div>
          </div>
        </section>
      </main>

      <Footer />
    </div>
  )
}

export default APropos
