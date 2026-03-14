"use client"
import { MessageCircle, Users, Star, BarChart3, TrendingUp } from "lucide-react"
import { motion } from "framer-motion"
import { useScrollAnimation } from "@/hooks/useScrollAnimation"
import missionTeamImage from "/src/assets/mission-team-call-center.png"
import avatar1 from "@/assets/avatar-1.png"
import avatar2 from "@/assets/avatar-2.png"
import avatar3 from "@/assets/avatar-3.png"
import { useTranslation } from "react-i18next"

const MissionSection = () => {
  const { ref, isVisible } = useScrollAnimation()
  const { t } = useTranslation('hero')

  return (
    <motion.section
      ref={ref}
      id="about"
      className="relative py-16 lg:py-20 overflow-hidden bg-gradient-to-br from-background to-muted/30"
      initial={{ opacity: 0 }}
      animate={isVisible ? { opacity: 1 } : { opacity: 0 }}
      transition={{ duration: 0.8 }}
    >
      <div className="container mx-auto px-4 lg:px-8 max-w-7xl relative z-10">
        <div className="w-full mx-auto">
          <div>
            <div className="grid lg:grid-cols-2 gap-12 lg:gap-16 items-center">
              <motion.div
                className="order-2 lg:order-1 space-y-8"
                initial={{ opacity: 0, x: -50 }}
                animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: -50 }}
                transition={{ duration: 0.8, delay: 0.2 }}
              >
                <div>
                  <h3 className="text-3xl lg:text-4xl font-bold text-brand-dark mb-6 leading-tight">
                    {t('mission.title1')} <span className="text-primary">{t('mission.titleHighlight')}</span> {t('mission.title2')}
                  </h3>

                  <p className="text-lg text-muted-foreground leading-relaxed mb-8">
                    {t('mission.description').split('<1>').map((part, i) => 
                      i === 0 ? part : (() => {
                        const [inside, rest] = part.split('</1>')
                        const restParts = rest?.split('<2>') || [rest]
                        return (
                          <span key={i}>
                            <span className="font-bold text-primary">{inside}</span>
                            {restParts[0]}
                            {restParts[1] && (() => {
                              const [cand, afterCand] = restParts[1].split('</2>')
                              const empParts = afterCand?.split('<3>') || [afterCand]
                              return (
                                <>
                                  <a href="/inscription-detaillee" className="text-primary font-semibold hover:underline cursor-pointer transition-all">{cand}</a>
                                  {empParts[0]}
                                  {empParts[1] && (() => {
                                    const [emp, afterEmp] = empParts[1].split('</3>')
                                    return (
                                      <>
                                        <a href="#recruiter-section" className="text-primary font-semibold hover:underline cursor-pointer transition-all" onClick={(e) => { e.preventDefault(); document.getElementById("recruiter-section")?.scrollIntoView({ behavior: "smooth" }) }}>{emp}</a>
                                        {afterEmp}
                                      </>
                                    )
                                  })()}
                                </>
                              )
                            })()}
                          </span>
                        )
                      })()
                    )}
                  </p>
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                  <div className="flex items-center gap-4">
                    <div className="w-12 h-12 flex items-center justify-center">
                      <BarChart3 className="w-8 h-8 text-green-500" />
                    </div>
                    <div><h4 className="font-semibold text-brand-dark mb-1">{t('mission.feature1')}</h4></div>
                  </div>
                  <div className="flex items-center gap-4">
                    <div className="w-12 h-12 flex items-center justify-center">
                      <Users className="w-8 h-8 text-blue-500" />
                    </div>
                    <div><h4 className="font-semibold text-brand-dark mb-1">{t('mission.feature2')}</h4></div>
                  </div>
                  <div className="flex items-center gap-4">
                    <div className="w-12 h-12 flex items-center justify-center">
                      <TrendingUp className="w-8 h-8 text-purple-500" />
                    </div>
                    <div><h4 className="font-semibold text-brand-dark mb-1">{t('mission.feature3')}</h4></div>
                  </div>
                  <div className="flex items-center gap-4">
                    <div className="w-12 h-12 flex items-center justify-center">
                      <Star className="w-8 h-8 text-orange-500" />
                    </div>
                    <div><h4 className="font-semibold text-brand-dark mb-1">{t('mission.feature4')}</h4></div>
                  </div>
                </div>
              </motion.div>

              <motion.div
                className="relative order-1 lg:order-2"
                initial={{ opacity: 0, x: 50 }}
                animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: 50 }}
                transition={{ duration: 0.8, delay: 0.3 }}
              >
                <div className="relative">
                  <div className="absolute -top-8 left-10 w-3 h-3 bg-blue-400 rounded-full animate-[float_4s_ease-in-out_infinite] opacity-80"></div>
                  <div className="absolute top-20 -left-6 w-2 h-2 bg-blue-500 rounded-full animate-[float_3s_ease-in-out_infinite_1s] opacity-60"></div>
                  <div className="absolute top-1/3 right-8 w-4 h-4 bg-blue-300 rounded-full animate-[float_5s_ease-in-out_infinite_2s] opacity-70"></div>
                  <div className="absolute bottom-20 left-4 w-2.5 h-2.5 bg-blue-600 rounded-full animate-[float_3.5s_ease-in-out_infinite_0.5s] opacity-80"></div>
                  <div className="absolute -bottom-4 right-16 w-3 h-3 bg-blue-400 rounded-full animate-[float_4.5s_ease-in-out_infinite_1.5s] opacity-60"></div>
                  <div className="absolute top-2/3 -right-4 w-2 h-2 bg-blue-500 rounded-full animate-[float_3s_ease-in-out_infinite_2.5s] opacity-70"></div>

                  <div className="relative overflow-hidden">
                    <div className="relative overflow-hidden" style={{ clipPath: "polygon(0% 15%, 15% 0%, 85% 0%, 100% 15%, 100% 85%, 85% 100%, 15% 100%, 0% 85%)" }}>
                      <img src={missionTeamImage || "/placeholder.svg"} alt="Call Center Match" className="w-full h-[300px] lg:h-[500px] object-contain" />
                    </div>
                  </div>

                  <div className="absolute -bottom-4 -right-4 lg:-bottom-6 lg:-right-6 bg-white rounded-xl shadow-xl p-3 lg:p-4 animate-[float_3s_ease-in-out_infinite_1s] w-[180px] lg:w-[220px] border border-blue-100 backdrop-blur-sm">
                    <div className="flex items-center gap-2.5 mb-2.5">
                      <div className="flex -space-x-2">
                        <img src={avatar1} alt="" className="w-7 h-7 rounded-full border-2 border-white shadow-sm object-cover" />
                        <img src={avatar2} alt="" className="w-7 h-7 rounded-full border-2 border-white shadow-sm object-cover" />
                        <img src={avatar3} alt="" className="w-7 h-7 rounded-full border-2 border-white shadow-sm object-cover" />
                        <div className="w-7 h-7 bg-gradient-to-br from-orange-400 to-orange-500 rounded-full border-2 border-white shadow-sm flex items-center justify-center">
                          <span className="text-xs font-bold text-white">+</span>
                        </div>
                      </div>
                      <div>
                        <div className="text-xs lg:text-sm font-semibold text-gray-800">{t('mission.clients')}</div>
                        <div className="text-xs text-gray-600">{t('mission.satisfied')}</div>
                      </div>
                    </div>
                  </div>

                  <div className="absolute top-1/2 -left-4 lg:-left-6 bg-white rounded-lg shadow-lg p-2 lg:p-3 animate-[float_3s_ease-in-out_infinite_2s] border border-blue-100">
                    <div className="flex items-center gap-2">
                      <TrendingUp className="w-4 h-4 text-blue-500" />
                      <div>
                        <div className="text-xs lg:text-sm font-bold text-gray-800">+245%</div>
                        <div className="text-xs text-gray-600">{t('mission.growth')}</div>
                      </div>
                    </div>
                  </div>
                </div>
              </motion.div>
            </div>
          </div>
        </div>
      </div>
    </motion.section>
  )
}

export default MissionSection
