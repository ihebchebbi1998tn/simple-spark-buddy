import { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { motion, AnimatePresence } from "framer-motion";
import { Dialog, DialogContent } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Progress } from "@/components/ui/progress";
import { 
  FileText, 
  ArrowRight, 
  X, 
  Clock,
  Sparkles,
  CheckCircle2
} from "lucide-react";
import { useRegistrationStorage } from "@/hooks/useRegistrationStorage";

interface ResumeRegistrationModalProps {
  isOpen: boolean;
  onClose: () => void;
  onContinue: () => void;
  completionPercentage: number;
  hasStep1: boolean;
  hasStep2: boolean;
  hasStep3: boolean;
  hasStep4: boolean;
}

export const ResumeRegistrationModal = ({
  isOpen,
  onClose,
  onContinue,
  completionPercentage,
  hasStep1,
  hasStep2,
  hasStep3,
  hasStep4,
}: ResumeRegistrationModalProps) => {
  const [showPulse, setShowPulse] = useState(true);

  useEffect(() => {
    const timer = setTimeout(() => setShowPulse(false), 3000);
    return () => clearTimeout(timer);
  }, []);

  const getNextStepMessage = () => {
    if (!hasStep1) return "Commencez votre inscription";
    if (!hasStep2) return "Continuez avec vos expériences";
    if (!hasStep3) return "Ajoutez vos compétences linguistiques";
    if (!hasStep4) return "Finalisez votre inscription";
    return "Terminez votre inscription";
  };

  return (
    <AnimatePresence>
      {isOpen && (
        <Dialog open={isOpen} onOpenChange={onClose}>
          <DialogContent className="max-w-[95vw] sm:max-w-[500px] p-0 overflow-hidden border-2 border-primary/20 bg-gradient-to-br from-card via-card to-primary/5">
            {/* Animated background effect */}
            <div className="absolute inset-0 overflow-hidden pointer-events-none">
              <motion.div
                className="absolute -top-1/2 -right-1/2 w-96 h-96 bg-primary/10 rounded-full blur-3xl"
                animate={{
                  scale: [1, 1.2, 1],
                  opacity: [0.3, 0.5, 0.3],
                }}
                transition={{
                  duration: 4,
                  repeat: Infinity,
                  ease: "easeInOut",
                }}
              />
              <motion.div
                className="absolute -bottom-1/2 -left-1/2 w-96 h-96 bg-blue-500/10 rounded-full blur-3xl"
                animate={{
                  scale: [1.2, 1, 1.2],
                  opacity: [0.5, 0.3, 0.5],
                }}
                transition={{
                  duration: 4,
                  repeat: Infinity,
                  ease: "easeInOut",
                  delay: 2,
                }}
              />
            </div>

            <div className="relative z-10">
              {/* Header with icon */}
              <div className="relative bg-gradient-to-r from-primary to-primary/80 p-6 text-white">
                <button
                  onClick={onClose}
                  className="absolute top-4 right-4 text-white/80 hover:text-white transition-colors"
                >
                  <X className="h-5 w-5" />
                </button>
                
                <motion.div
                  initial={{ scale: 0, rotate: -180 }}
                  animate={{ scale: 1, rotate: 0 }}
                  transition={{ 
                    type: "spring", 
                    stiffness: 200, 
                    damping: 15,
                    delay: 0.1 
                  }}
                  className="flex items-center justify-center w-16 h-16 bg-white/20 backdrop-blur-sm rounded-full mx-auto mb-4"
                >
                  <FileText className="h-8 w-8" />
                </motion.div>

                <motion.h2
                  initial={{ opacity: 0, y: 10 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.2 }}
                  className="text-xl sm:text-2xl md:text-3xl font-bold text-center mb-2 px-2"
                >
                  Reprenez votre inscription
                </motion.h2>
                
                <motion.p
                  initial={{ opacity: 0, y: 10 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.3 }}
                  className="text-white/90 text-center text-sm sm:text-base px-4"
                >
                  Vous avez commencé votre inscription. Ne perdez pas vos données !
                </motion.p>
              </div>

              {/* Content */}
              <div className="p-6 space-y-6">
                {/* Progress section */}
                <motion.div
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.4 }}
                  className="space-y-3"
                >
                  <div className="flex items-center justify-between text-sm">
                    <span className="text-muted-foreground">Progression</span>
                    <span className="font-semibold text-foreground">
                      {Math.round(completionPercentage)}%
                    </span>
                  </div>
                  
                  <div className="relative">
                    <Progress 
                      value={completionPercentage} 
                      className="h-3 bg-muted"
                    />
                    {showPulse && (
                      <motion.div
                        className="absolute inset-0 h-3 bg-gradient-to-r from-primary/30 to-primary/30 rounded-full"
                        animate={{
                          opacity: [0.3, 0.6, 0.3],
                        }}
                        transition={{
                          duration: 1.5,
                          repeat: Infinity,
                          ease: "easeInOut",
                        }}
                      />
                    )}
                  </div>
                </motion.div>

                {/* Steps checklist */}
                <motion.div
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.5 }}
                  className="space-y-2 bg-muted/30 rounded-lg p-4"
                >
                  <div className="flex items-center gap-3">
                    <div className={`flex-shrink-0 w-6 h-6 rounded-full flex items-center justify-center ${
                      hasStep1 ? 'bg-green-500' : 'bg-muted'
                    }`}>
                      {hasStep1 ? (
                        <CheckCircle2 className="h-4 w-4 text-white" />
                      ) : (
                        <div className="w-2 h-2 rounded-full bg-muted-foreground/50" />
                      )}
                    </div>
                    <span className={`text-sm ${hasStep1 ? 'text-foreground' : 'text-muted-foreground'}`}>
                      Informations de base
                    </span>
                  </div>

                  <div className="flex items-center gap-3">
                    <div className={`flex-shrink-0 w-6 h-6 rounded-full flex items-center justify-center ${
                      hasStep2 ? 'bg-green-500' : 'bg-muted'
                    }`}>
                      {hasStep2 ? (
                        <CheckCircle2 className="h-4 w-4 text-white" />
                      ) : (
                        <div className="w-2 h-2 rounded-full bg-muted-foreground/50" />
                      )}
                    </div>
                    <span className={`text-sm ${hasStep2 ? 'text-foreground' : 'text-muted-foreground'}`}>
                      Expériences professionnelles
                    </span>
                  </div>

                  <div className="flex items-center gap-3">
                    <div className={`flex-shrink-0 w-6 h-6 rounded-full flex items-center justify-center ${
                      hasStep3 ? 'bg-green-500' : 'bg-muted'
                    }`}>
                      {hasStep3 ? (
                        <CheckCircle2 className="h-4 w-4 text-white" />
                      ) : (
                        <div className="w-2 h-2 rounded-full bg-muted-foreground/50" />
                      )}
                    </div>
                    <span className={`text-sm ${hasStep3 ? 'text-foreground' : 'text-muted-foreground'}`}>
                      Compétences linguistiques
                    </span>
                  </div>

                  <div className="flex items-center gap-3">
                    <div className={`flex-shrink-0 w-6 h-6 rounded-full flex items-center justify-center ${
                      hasStep4 ? 'bg-green-500' : 'bg-muted'
                    }`}>
                      {hasStep4 ? (
                        <CheckCircle2 className="h-4 w-4 text-white" />
                      ) : (
                        <div className="w-2 h-2 rounded-full bg-muted-foreground/50" />
                      )}
                    </div>
                    <span className={`text-sm ${hasStep4 ? 'text-foreground' : 'text-muted-foreground'}`}>
                      Préférences de travail
                    </span>
                  </div>
                </motion.div>

                {/* Next step message */}
                <motion.div
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.6 }}
                  className="flex items-start gap-3 p-4 bg-primary/5 border border-primary/20 rounded-lg"
                >
                  <Sparkles className="h-5 w-5 text-primary flex-shrink-0 mt-0.5" />
                  <div>
                    <p className="text-sm font-medium text-foreground mb-1">
                      Prochaine étape
                    </p>
                    <p className="text-sm text-muted-foreground">
                      {getNextStepMessage()}
                    </p>
                  </div>
                </motion.div>

                {/* Time estimate */}
                <motion.div
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.7 }}
                  className="flex items-center justify-center gap-2 text-sm text-muted-foreground"
                >
                  <Clock className="h-4 w-4" />
                  <span>Temps restant estimé : {Math.ceil((100 - completionPercentage) / 25)} minute{Math.ceil((100 - completionPercentage) / 25) > 1 ? 's' : ''}</span>
                </motion.div>

                {/* Action buttons */}
                <motion.div
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.8 }}
                  className="flex flex-col sm:flex-row gap-3"
                >
                  <Button
                    onClick={onContinue}
                    className="flex-1 h-12 bg-gradient-to-r from-primary to-primary/90 hover:from-primary/90 hover:to-primary text-white font-semibold shadow-lg hover:shadow-xl transition-all duration-300"
                  >
                    <span>Continuer mon inscription</span>
                    <ArrowRight className="ml-2 h-4 w-4" />
                  </Button>
                  
                  <Button
                    onClick={onClose}
                    variant="outline"
                    className="sm:w-auto h-12 border-2"
                  >
                    Plus tard
                  </Button>
                </motion.div>

                <p className="text-xs text-center text-muted-foreground">
                  Vos données sont automatiquement sauvegardées
                </p>
              </div>
            </div>
          </DialogContent>
        </Dialog>
      )}
    </AnimatePresence>
  );
};

// Hook to manage the modal globally
export const useResumeRegistration = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { data } = useRegistrationStorage();
  const [showModal, setShowModal] = useState(false);
  const [hasChecked, setHasChecked] = useState(false);

  useEffect(() => {
    // Don't show on registration pages, dashboard, or if already checked
    const excludedPages = [
      '/',
      '/inscription-detaillee',
      '/processing',
      '/espace-candidats',
      '/test-langue',
      '/dashboard'
    ].includes(location.pathname);

    if (excludedPages || hasChecked) {
      return;
    }

    // Check if user is authenticated
    const checkRegistration = async () => {
      const { authService } = await import('@/services/authService');
      const isAuthenticated = authService.isAuthenticated();

      // If authenticated, don't show modal
      if (isAuthenticated) {
        setHasChecked(true);
        return;
      }

      // Check if there's incomplete registration data
      const hasStep1 = !!(data.nom && data.prenom && data.telephone);
      const hasAnyData = hasStep1 || data.activitePrincipale || data.langueMaternelle || data.email;

      if (hasAnyData) {
        // Show modal after a small delay for better UX
        setTimeout(() => {
          setShowModal(true);
          setHasChecked(true);
        }, 1000);
      } else {
        setHasChecked(true);
      }
    };

    checkRegistration();
  }, [location.pathname, data, hasChecked]);

  const calculateCompletion = () => {
    let completed = 0;
    const total = 4; // 4 main steps

    // Step 1: Basic info
    if (data.nom && data.prenom && data.telephone && data.experienceGlobale) {
      completed++;
    }

    // Step 2: Experience
    if (data.activitePrincipale && data.operation1) {
      completed++;
    }

    // Step 3: Languages
    if (data.langueMaternelle && data.langueForeign1) {
      completed++;
    }

    // Step 4: Work preferences & account
    if (data.modeTravail && data.tempsTravail) {
      completed++;
    }

    return (completed / total) * 100;
  };

  const handleContinue = () => {
    setShowModal(false);
    
    // Determine where to redirect based on completion
    const hasStep1 = !!(data.nom && data.prenom && data.telephone);
    
    if (!hasStep1) {
      // Go to homepage to start fresh
      navigate('/inscription-detaillee', { replace: true });
    } else {
      // Go to detailed inscription
      navigate('/inscription-detaillee', { replace: true });
    }
  };

  const hasStep1 = !!(data.nom && data.prenom && data.telephone && data.experienceGlobale);
  const hasStep2 = !!(data.activitePrincipale && data.operation1);
  const hasStep3 = !!(data.langueMaternelle && data.langueForeign1);
  const hasStep4 = !!(data.modeTravail && data.tempsTravail);

  return {
    showModal,
    setShowModal,
    completionPercentage: calculateCompletion(),
    handleContinue,
    hasStep1,
    hasStep2,
    hasStep3,
    hasStep4,
  };
};
