import { useEffect, useState } from "react";
import { motion } from "framer-motion";
import { Sparkles, ArrowRight } from "lucide-react";

interface LoadingTransitionProps {
  onComplete: () => void;
}

const LoadingTransition = ({ onComplete }: LoadingTransitionProps) => {
  const [progress, setProgress] = useState(0);
  const [currentText, setCurrentText] = useState("Initialisation de votre profil...");

  const texts = [
    "Initialisation de votre profil...",
    "Préparation de votre parcours...",
    "Optimisation de vos opportunités...",
    "Finalisation de votre inscription..."
  ];

  useEffect(() => {
    const interval = setInterval(() => {
      setProgress(prev => {
        const newProgress = prev + 2;
        
        // Change text based on progress
        if (newProgress >= 25 && newProgress < 50) {
          setCurrentText(texts[1]);
        } else if (newProgress >= 50 && newProgress < 75) {
          setCurrentText(texts[2]);
        } else if (newProgress >= 75) {
          setCurrentText(texts[3]);
        }
        
        if (newProgress >= 100) {
          clearInterval(interval);
          setTimeout(onComplete, 800);
          return 100;
        }
        return newProgress;
      });
    }, 50);

    return () => clearInterval(interval);
  }, [onComplete]);

  return (
    <div className="min-h-screen bg-background flex items-center justify-center px-4">
      <div className="text-center space-y-6 sm:space-y-8 p-6 sm:p-8 max-w-sm sm:max-w-md mx-auto">
        <motion.div
          initial={{ scale: 0.5, opacity: 0 }}
          animate={{ scale: 1, opacity: 1 }}
          transition={{ duration: 0.6 }}
          className="relative"
        >
          <motion.div
            animate={{ rotate: 360 }}
            transition={{ duration: 3, repeat: Infinity, ease: "linear" }}
            className="w-24 h-24 mx-auto mb-6 relative"
          >
            <div className="absolute inset-0 bg-primary rounded-full"></div>
            <div className="absolute inset-2 bg-card rounded-full flex items-center justify-center">
              <Sparkles className="w-8 h-8 text-primary" />
            </div>
          </motion.div>
          
          <motion.h2
            initial={{ y: 20, opacity: 0 }}
            animate={{ y: 0, opacity: 1 }}
            transition={{ delay: 0.3 }}
            className="text-2xl sm:text-3xl font-bold text-foreground mb-2"
          >
            Finalisez votre inscription
          </motion.h2>
          
          <motion.p
            initial={{ y: 20, opacity: 0 }}
            animate={{ y: 0, opacity: 1 }}
            transition={{ delay: 0.5 }}
            className="text-base sm:text-lg text-muted-foreground mb-6 sm:mb-8 px-2"
          >
            {currentText}
          </motion.p>
        </motion.div>

        <div className="w-72 sm:w-80 mx-auto space-y-3">
          <div className="flex justify-between text-sm text-muted-foreground">
            <span>Progression</span>
            <span>{progress}%</span>
          </div>
          <div className="h-2 bg-muted rounded-full overflow-hidden">
            <motion.div
              className="h-full bg-primary rounded-full relative"
              initial={{ width: 0 }}
              animate={{ width: `${progress}%` }}
              transition={{ duration: 0.1 }}
            >
              <motion.div
                className="absolute inset-0 bg-gradient-to-r from-transparent via-foreground/10 to-transparent"
                animate={{ x: [-100, 320] }}
                transition={{ duration: 1.5, repeat: Infinity, ease: "easeInOut" }}
              />
            </motion.div>
          </div>
        </div>

        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: progress > 90 ? 1 : 0 }}
          className="flex items-center justify-center gap-2 text-primary font-medium"
        >
          <span>Redirection en cours</span>
          <ArrowRight className="w-5 h-5" />
        </motion.div>
      </div>
    </div>
  );
};

export default LoadingTransition;
