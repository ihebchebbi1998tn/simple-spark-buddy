import { motion } from "framer-motion";
import { Users, Headphones, Clock, DollarSign, Star, Bell } from "lucide-react";
import { useScrollAnimation } from "@/hooks/useScrollAnimation";
import { useTranslation } from "react-i18next";

const AdditionalFeaturesSection = () => {
  const { ref, isVisible } = useScrollAnimation();
  const { t } = useTranslation('hero');

  const features = [
    { icon: Users, titleKey: "features.community", descKey: "features.communityDesc", delay: 0.1 },
    { icon: Clock, titleKey: "features.hiringSpeed", descKey: "features.hiringSpeedDesc", delay: 0.2 },
    { icon: Headphones, titleKey: "features.support", descKey: "features.supportDesc", delay: 0.3 },
    { icon: Star, titleKey: "features.freeSignup", descKey: "features.freeSignupDesc", delay: 0.4 },
    { icon: DollarSign, titleKey: "features.costOptimization", descKey: "features.costOptimizationDesc", delay: 0.5 },
    { icon: Bell, titleKey: "features.realTimeNotifs", descKey: "features.realTimeNotifsDesc", delay: 0.6 },
  ];

  return (
    <motion.section 
      ref={ref}
      className="relative py-16 lg:py-20 bg-muted/30"
      initial={{ opacity: 0 }}
      animate={isVisible ? { opacity: 1 } : { opacity: 0 }}
      transition={{ duration: 0.8 }}
    >
      <div className="container mx-auto px-4 lg:px-8 max-w-7xl">
        <motion.div 
          className="text-center mb-12 lg:mb-16"
          initial={{ opacity: 0, y: 30 }}
          animate={isVisible ? { opacity: 1, y: 0 } : { opacity: 0, y: 30 }}
          transition={{ duration: 0.8, delay: 0.2 }}
        >
          <h2 className="text-3xl lg:text-4xl font-bold text-brand-dark mb-4">
            {t('features.title').split('<1>').map((part, i) => 
              i === 0 ? part : <span key={i}><span className="text-primary">{part.split('</1>')[0]}</span>{part.split('</1>')[1]}</span>
            )}
          </h2>
        </motion.div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {features.map((feature, index) => {
            const IconComponent = feature.icon;
            return (
              <motion.div
                key={index}
                className="flex items-start space-x-4 p-6 bg-background/50 rounded-lg border border-border/50 hover:border-primary/20 transition-colors"
                initial={{ opacity: 0, y: 50 }}
                animate={isVisible ? { opacity: 1, y: 0 } : { opacity: 0, y: 50 }}
                transition={{ duration: 0.6, delay: feature.delay }}
              >
                <div className="flex-shrink-0">
                  <div className="w-12 h-12 bg-primary/10 rounded-lg flex items-center justify-center">
                    <IconComponent className="w-6 h-6 text-primary" />
                  </div>
                </div>
                <div className="flex-1">
                  <h3 className="text-lg font-semibold text-brand-dark mb-1">{t(feature.titleKey)}</h3>
                  <p className="text-muted-foreground">{t(feature.descKey)}</p>
                </div>
              </motion.div>
            );
          })}
        </div>
      </div>
    </motion.section>
  );
};

export default AdditionalFeaturesSection;
