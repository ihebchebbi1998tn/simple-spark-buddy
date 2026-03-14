import { Button } from "@/components/ui/button";
import { motion } from "framer-motion";
import { useScrollAnimation } from "@/hooks/useScrollAnimation";
import heroBusinessHandshake from "@/assets/hero-business-handshake.png";
import heroMobileProfessional from "@/assets/hero-mobile-final.png";
import { useTranslation } from "react-i18next";

const HeroSection = () => {
  const { ref, isVisible } = useScrollAnimation(0.1, '0px');
  const { t } = useTranslation('hero');

  const scrollToSection = (sectionId: string) => {
    const element = document.getElementById(sectionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    }
  };

  return (
    <motion.section 
      ref={ref}
      className="overflow-hidden relative"
      initial={{ opacity: 0 }}
      animate={isVisible ? { opacity: 1 } : { opacity: 0 }}
      transition={{ duration: 0.8 }}
    >
      {/* Mobile Layout */}
      <div className="lg:hidden relative h-[600px] bg-white overflow-hidden">
        <div className="absolute right-0 -top-16 w-full h-full transform translate-x-8">
          <img src={heroMobileProfessional} alt={t('heroImageAlt')} className="w-full h-full object-cover" />
        </div>
        
        <motion.div 
          className="relative z-10 px-8 pt-6 h-full flex flex-col justify-end pb-12"
          initial={{ opacity: 0, y: 50 }}
          animate={isVisible ? { opacity: 1, y: 0 } : { opacity: 0, y: 50 }}
          transition={{ duration: 0.8, delay: 0.2 }}
        >
          <h1 className="text-2xl font-bold text-gray-900 mb-6 leading-tight -mt-20 drop-shadow-sm">
            {t('heroMobileTitle1')}{" "}
            <span className="text-primary">{t('heroMobileTalents')}</span>
            <br />
            {t('heroMobileAnd')}{" "}
            <span className="text-primary">{t('heroMobileCallCenters')}</span>
          </h1>
          
          <p className="text-lg text-gray-600 mb-8 max-w-lg drop-shadow-sm">
            {t('heroMobileSubtitle')}
          </p>

          <div className="flex flex-col items-start gap-4">
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={isVisible ? { opacity: 1, y: 0 } : { opacity: 0, y: 20 }}
              transition={{ duration: 0.4, staggerChildren: 0.1 }}
              className="flex flex-row gap-3"
            >
              <motion.div
                initial={{ opacity: 0, x: -20 }}
                animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: -20 }}
                transition={{ duration: 0.3, delay: 0.1 }}
              >
                <Button 
                  size="lg" 
                  className="bg-primary hover:bg-primary/90 text-primary-foreground px-8 py-3 text-base font-medium rounded-lg shadow-lg hover:shadow-xl transition-all duration-300"
                  onClick={() => scrollToSection('candidate-form')}
                >
                  {t('heroMobileCandidates')}
                </Button>
              </motion.div>
              <motion.div
                initial={{ opacity: 0, x: -20 }}
                animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: -20 }}
                transition={{ duration: 0.3, delay: 0.2 }}
              >
                <Button 
                  size="lg" 
                  variant="outline"
                  className="border-primary text-primary hover:bg-primary hover:text-primary-foreground px-8 py-3 text-base font-medium rounded-lg shadow-lg hover:shadow-xl transition-all duration-300"
                  onClick={() => scrollToSection('recruiter-section')}
                >
                  {t('heroMobileRecruiter')}
                </Button>
              </motion.div>
            </motion.div>
          </div>
        </motion.div>
      </div>

      {/* Desktop Layout */}
      <div className="hidden lg:block bg-white">
        <div className="grid lg:grid-cols-2 items-center h-[420px]">
          <motion.div 
            className="pl-40 pr-16 pt-4"
            initial={{ opacity: 0, x: -50 }}
            animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: -50 }}
            transition={{ duration: 0.8, delay: 0.3 }}
          >
            <h1 className="text-4xl xl:text-5xl font-bold text-gray-900 mb-6 leading-tight">
              {t('heroDesktopTitle1')}{" "}
              <span className="text-primary">{t('heroMobileTalents')}</span>
              <br />
              {t('heroDesktopTitle2')}{" "}
              <span className="text-primary">{t('heroMobileCallCenters')}</span>
            </h1>
            
            <p className="text-lg text-gray-600 mb-8 max-w-lg">
              {t('heroDesktopSubtitle')}
            </p>

            <div className="flex flex-col items-start gap-4">
              <motion.div
                initial={{ opacity: 0, y: 20 }}
                animate={isVisible ? { opacity: 1, y: 0 } : { opacity: 0, y: 20 }}
                transition={{ duration: 0.4, staggerChildren: 0.1 }}
                className="flex flex-row gap-3"
              >
                <motion.div
                  initial={{ opacity: 0, x: -20 }}
                  animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: -20 }}
                  transition={{ duration: 0.3, delay: 0.1 }}
                >
                  <Button 
                    size="lg" 
                    className="bg-primary hover:bg-primary/90 text-primary-foreground px-8 py-3 text-base font-medium rounded-lg shadow-lg hover:shadow-xl transition-all duration-300"
                    onClick={() => scrollToSection('candidate-form')}
                  >
                    {t('heroMobileCandidates')}
                  </Button>
                </motion.div>
                <motion.div
                  initial={{ opacity: 0, x: -20 }}
                  animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: -20 }}
                  transition={{ duration: 0.3, delay: 0.2 }}
                >
                  <Button 
                    size="lg" 
                    variant="outline"
                    className="border-primary text-primary hover:bg-primary hover:text-primary-foreground px-8 py-3 text-base font-medium rounded-lg shadow-lg hover:shadow-xl transition-all duration-300"
                    onClick={() => scrollToSection('recruiter-section')}
                  >
                    {t('heroMobileRecruiter')}
                  </Button>
                </motion.div>
              </motion.div>
            </div>
          </motion.div>

          <motion.div 
            className="relative h-[420px] pr-40"
            initial={{ opacity: 0, x: 50 }}
            animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: 50 }}
            transition={{ duration: 0.8, delay: 0.4 }}
          >
            <div className="absolute left-0 top-0 bottom-0 w-24 bg-white transform -skew-x-12 z-10"></div>
            <img src={heroBusinessHandshake} alt={t('heroImageAlt')} className="w-full h-full object-cover pl-12" />
          </motion.div>
        </div>
      </div>
    </motion.section>
  );
};

export default HeroSection;
