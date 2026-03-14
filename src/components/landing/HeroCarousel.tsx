import { Button } from "@/components/ui/button"
import { useNavigate } from "react-router-dom"
import { ArrowRight, Users, Building2, CheckCircle2, TrendingUp, Award } from "lucide-react"
import { motion } from "framer-motion"
import { useTranslation } from "react-i18next"

const HeroCarousel = () => {
  const navigate = useNavigate()
  const { t } = useTranslation('hero')

  const statCards = [
    { number: "12k+", label: t('stat1'), icon: Users },
    { number: "15+", label: t('stat2'), icon: Building2 },
    { number: "4.8★", label: t('stat3'), icon: Award, highlight: true },
    { number: "200+", label: t('stat4'), icon: TrendingUp },
  ]

  const trustItems = [t('trust1'), t('trust2'), t('trust3')]

  return (
    <section className="bg-background relative overflow-hidden">
      <div className="container mx-auto px-4 lg:px-8 max-w-[1280px]">
        <div className="grid lg:grid-cols-2 gap-12 lg:gap-16 items-center py-16 lg:py-0 lg:min-h-[calc(100vh-68px)]">
          {/* Left Content */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, ease: "easeOut" }}
            className="max-w-[560px]"
          >
            <motion.div
              className="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full border border-border bg-muted mb-6"
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ delay: 0.15, duration: 0.3 }}
            >
              <span className="text-[11px] font-semibold text-primary tracking-wide uppercase">{t('badge')}</span>
            </motion.div>

            <h1 className="text-[1.85rem] sm:text-[2rem] lg:text-[2.75rem] xl:text-[3.15rem] font-extrabold text-foreground mb-5 leading-[1.12] tracking-[-0.02em]">
              {t('title1')}{" "}
              <span className="text-primary">{t('titleHighlight')}</span>.
              <br />
              <span className="text-foreground/85">{t('title2')}</span>
            </h1>

            <p className="text-[15px] lg:text-base text-muted-foreground mb-9 leading-[1.75] max-w-[480px]">
              {t('description')}
            </p>

            <motion.div
              className="flex flex-wrap gap-3"
              initial={{ opacity: 0, y: 10 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: 0.25, duration: 0.35 }}
            >
              <Button
                size="lg"
                className="rounded-xl px-7 h-12 text-sm font-semibold shadow-sm transition-colors gap-2"
                onClick={() => navigate("/candidats")}
              >
                <Users className="w-4 h-4" />
                {t('btnCandidate')}
                <ArrowRight className="w-3.5 h-3.5" />
              </Button>
              <Button
                size="lg"
                variant="outline"
                className="rounded-xl border-border text-foreground hover:text-primary hover:border-primary/30 hover:bg-primary/5 px-7 h-12 text-sm font-semibold transition-colors gap-2"
                onClick={() => navigate("/entreprises")}
              >
                <Building2 className="w-4 h-4" />
                {t('btnRecruiter')}
                <ArrowRight className="w-3.5 h-3.5" />
              </Button>
            </motion.div>

            {/* Trust indicators */}
            <motion.div
              className="mt-10 flex flex-wrap items-center gap-x-6 gap-y-2.5"
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ delay: 0.5, duration: 0.4 }}
            >
              {trustItems.map((item) => (
                <span key={item} className="flex items-center gap-2 text-[12px] text-muted-foreground font-medium">
                  <CheckCircle2 className="w-3.5 h-3.5 text-primary/40" />
                  {item}
                </span>
              ))}
            </motion.div>
          </motion.div>

          {/* Right - Stats grid */}
          <motion.div
            className="hidden lg:block"
            initial={{ opacity: 0, x: 16 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.5, delay: 0.15 }}
          >
            <div className="relative h-[440px] w-full rounded-2xl overflow-hidden bg-primary shadow-lg">
              <img
                src="https://images.unsplash.com/photo-1556740738-b6a63e27c4df?w=800&auto=format&fit=crop"
                alt={t('heroImageAlt')}
                className="absolute inset-0 w-full h-full object-cover opacity-15"
                loading="lazy"
              />
              <div className="relative z-10 h-full w-full grid grid-cols-2 gap-4 p-6">
                {statCards.map((stat, i) => {
                  const Icon = stat.icon;
                  return (
                    <motion.div
                      key={stat.label}
                      className={`rounded-2xl p-6 flex flex-col items-center justify-center text-center ${
                        stat.highlight
                          ? "border border-primary-foreground/20 bg-primary-foreground/10 backdrop-blur-sm"
                          : "bg-card border border-border"
                      }`}
                      initial={{ opacity: 0, y: 12 }}
                      animate={{ opacity: 1, y: 0 }}
                      transition={{ delay: 0.3 + i * 0.08, duration: 0.35 }}
                    >
                      <div className={`w-10 h-10 rounded-xl flex items-center justify-center mb-3 ${
                        stat.highlight ? "bg-primary-foreground/10" : "bg-primary/8"
                      }`}>
                        <Icon className={`w-5 h-5 ${stat.highlight ? "text-primary-foreground/80" : "text-primary/60"}`} />
                      </div>
                      <span className={`text-[1.7rem] font-extrabold leading-none ${stat.highlight ? "text-primary-foreground" : "text-primary"}`}>
                        {stat.number}
                      </span>
                      <span className={`text-[0.6rem] uppercase tracking-[0.14em] font-semibold mt-2.5 ${stat.highlight ? "text-primary-foreground/60" : "text-muted-foreground/70"}`}>
                        {stat.label}
                      </span>
                    </motion.div>
                  );
                })}
              </div>
            </div>
          </motion.div>

          {/* Mobile stats */}
          <div className="lg:hidden grid grid-cols-2 gap-3">
            {statCards.map((stat) => {
              const Icon = stat.icon;
              return (
                <div
                  key={stat.label}
                  className={`rounded-2xl p-5 text-center border ${
                    stat.highlight
                      ? "bg-primary border-primary text-primary-foreground"
                      : "bg-card border-border shadow-sm"
                  }`}
                >
                  <div className={`w-8 h-8 rounded-lg flex items-center justify-center mx-auto mb-2 ${
                    stat.highlight ? "bg-primary-foreground/15" : "bg-primary/8"
                  }`}>
                    <Icon className={`w-4 h-4 ${stat.highlight ? "text-primary-foreground/80" : "text-primary/50"}`} />
                  </div>
                  <div className={`text-xl font-extrabold ${stat.highlight ? "" : "text-primary"}`}>{stat.number}</div>
                  <div className={`text-[0.6rem] uppercase font-semibold tracking-wider mt-1 ${stat.highlight ? "text-primary-foreground/60" : "text-muted-foreground/70"}`}>
                    {stat.label}
                  </div>
                </div>
              );
            })}
          </div>
        </div>
      </div>
    </section>
  )
}

export default HeroCarousel
