"use client"
import { motion } from "framer-motion"
import { useScrollAnimation } from "@/hooks/useScrollAnimation"

const CompaniesSection = () => {
  const { ref, isVisible } = useScrollAnimation()

  const companies = [
    { name: "TeleContact", id: 1 },
    { name: "VoiceCenter", id: 2 },
    { name: "CallExpert", id: 3 },
    { name: "CustomerCare Plus", id: 4 },
    { name: "ConnectPro", id: 5 },
    { name: "TeleSolutions", id: 6 },
  ]

  // Duplicate companies for infinite scroll
  const duplicatedCompanies = [...companies, ...companies, ...companies]

  return (
    <motion.section
      ref={ref}
      className="py-8 bg-muted/30 overflow-hidden"
      initial={{ opacity: 0 }}
      animate={isVisible ? { opacity: 1 } : { opacity: 0 }}
      transition={{ duration: 0.8 }}
    >
      <div className="container mx-auto px-4 lg:px-8 max-w-6xl">
        <motion.div
          className="text-center mb-6"
          initial={{ opacity: 0, y: 30 }}
          animate={isVisible ? { opacity: 1, y: 0 } : { opacity: 0, y: 30 }}
          transition={{ duration: 0.8, delay: 0.2 }}
        >
          <p className="text-sm font-medium text-muted-foreground mb-8">
            Utilisé par plus que <span className="text-primary font-bold">10 000</span> professionnels entre chercheurs
            d'emploi et employeurs.
          </p>
        </motion.div>

        {/* Auto-scrolling carousel for all screen sizes */}
        <motion.div
          className="relative"
          initial={{ opacity: 0, y: 30 }}
          animate={isVisible ? { opacity: 1, y: 0 } : { opacity: 0, y: 30 }}
          transition={{ duration: 0.8, delay: 0.3 }}
        >
          <div className="overflow-hidden">
            <div
              className="flex gap-6 animate-scroll-left"
              style={{
                width: "calc(300% + 48px)", // Adjust for triple duplication
              }}
            >
              {duplicatedCompanies.map((company, index) => (
                <div
                  key={`${company.id}-${index}`}
                  className="bg-white rounded-lg p-4 shadow-sm flex-shrink-0 w-32 h-16 md:w-40 md:h-20 flex items-center justify-center transition-all duration-300 hover:shadow-md hover:scale-105 group"
                >
                  <img
                    src={`https://placehold.co/120x60/f8f9fa/6b7280?text=${company.name.replace(/\s+/g, "+")}`}
                    alt={`${company.name} logo`}
                    className="max-w-full max-h-full object-contain opacity-60 group-hover:opacity-80 transition-opacity"
                  />
                </div>
              ))}
            </div>
          </div>
        </motion.div>
      </div>
    </motion.section>
  )
}

export default CompaniesSection
