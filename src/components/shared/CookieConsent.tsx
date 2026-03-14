import { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { Button } from "@/components/ui/button";
import { Switch } from "@/components/ui/switch";
import { Cookie, X, Settings, Shield, ChevronUp } from "lucide-react";
import { Link } from "react-router-dom";
import { useCookieConsent, CookiePreferences } from "@/hooks/useCookieConsent";

const CookieConsent = () => {
  const {
    hasConsent,
    isLoaded,
    saveConsent,
    acceptAll,
    rejectAll,
  } = useCookieConsent();

  const [isVisible, setIsVisible] = useState(false);
  const [showDetails, setShowDetails] = useState(false);
  const [localPreferences, setLocalPreferences] = useState<CookiePreferences>({
    necessary: true,
    analytics: false,
    marketing: false,
    preferences: false,
  });

  useEffect(() => {
    if (isLoaded && !hasConsent) {
      // Show banner after a short delay for better UX
      const timer = setTimeout(() => setIsVisible(true), 1500);
      return () => clearTimeout(timer);
    }
  }, [isLoaded, hasConsent]);

  const handleAcceptAll = () => {
    acceptAll();
    setIsVisible(false);
  };

  const handleRejectAll = () => {
    rejectAll();
    setIsVisible(false);
  };

  const handleSavePreferences = () => {
    saveConsent(localPreferences);
    setIsVisible(false);
  };

  const cookieTypes = [
    {
      id: "necessary" as keyof CookiePreferences,
      name: "Cookies essentiels",
      description: "Nécessaires au fonctionnement du site (authentification, sécurité). Ne peuvent pas être désactivés.",
      required: true,
    },
    {
      id: "analytics" as keyof CookiePreferences,
      name: "Cookies analytiques",
      description: "Nous aident à comprendre comment vous utilisez le site pour l'améliorer (pages visitées, temps passé).",
      required: false,
    },
    {
      id: "marketing" as keyof CookiePreferences,
      name: "Cookies marketing",
      description: "Utilisés pour afficher des publicités pertinentes basées sur vos centres d'intérêt.",
      required: false,
    },
    {
      id: "preferences" as keyof CookiePreferences,
      name: "Cookies de préférences",
      description: "Mémorisent vos choix (langue, région, paramètres d'affichage) pour une meilleure expérience.",
      required: false,
    },
  ];

  // Don't render anything until we've checked localStorage
  if (!isLoaded) return null;

  return (
    <AnimatePresence>
      {isVisible && (
        <motion.div
          initial={{ y: 100, opacity: 0 }}
          animate={{ y: 0, opacity: 1 }}
          exit={{ y: 100, opacity: 0 }}
          transition={{ type: "spring", damping: 25, stiffness: 300 }}
          className="fixed bottom-0 left-0 right-0 z-[100] p-4 md:p-6"
        >
          <div className="max-w-4xl mx-auto bg-card border border-border rounded-2xl shadow-2xl overflow-hidden">
            {/* Header */}
            <div className="p-6 pb-4">
              <div className="flex items-start justify-between gap-4">
                <div className="flex items-center gap-3">
                  <div className="w-12 h-12 rounded-full bg-primary/10 flex items-center justify-center flex-shrink-0">
                    <Cookie className="w-6 h-6 text-primary" />
                  </div>
                  <div>
                    <h3 className="text-lg font-semibold text-foreground">
                      Gestion des cookies
                    </h3>
                    <p className="text-sm text-muted-foreground">
                      Nous respectons votre vie privée
                    </p>
                  </div>
                </div>
                <button
                  onClick={handleRejectAll}
                  className="text-muted-foreground hover:text-foreground transition-colors p-1"
                  aria-label="Fermer et refuser"
                >
                  <X className="w-5 h-5" />
                </button>
              </div>

              <p className="mt-4 text-sm text-muted-foreground leading-relaxed">
                Nous utilisons des cookies pour améliorer votre expérience, analyser le trafic 
                et personnaliser le contenu. Vos préférences sont sauvegardées localement et 
                respectées lors de chaque visite.{" "}
                <Link 
                  to="/privacy" 
                  className="text-primary hover:underline font-medium"
                >
                  Politique de confidentialité
                </Link>
              </p>
            </div>

            {/* Cookie Details (Expandable) */}
            <AnimatePresence>
              {showDetails && (
                <motion.div
                  initial={{ height: 0, opacity: 0 }}
                  animate={{ height: "auto", opacity: 1 }}
                  exit={{ height: 0, opacity: 0 }}
                  transition={{ duration: 0.3 }}
                  className="overflow-hidden"
                >
                  <div className="px-6 pb-4 space-y-3">
                    {cookieTypes.map((cookie) => (
                      <div
                        key={cookie.id}
                        className="flex items-center justify-between p-4 bg-secondary/30 rounded-lg border border-border/50"
                      >
                        <div className="flex-1 pr-4">
                          <div className="flex items-center gap-2">
                            <span className="font-medium text-foreground text-sm">
                              {cookie.name}
                            </span>
                            {cookie.required && (
                              <span className="text-xs bg-primary/10 text-primary px-2 py-0.5 rounded-full">
                                Toujours actif
                              </span>
                            )}
                          </div>
                          <p className="text-xs text-muted-foreground mt-1">
                            {cookie.description}
                          </p>
                        </div>
                        <Switch
                          checked={localPreferences[cookie.id]}
                          onCheckedChange={(checked) => {
                            if (!cookie.required) {
                              setLocalPreferences((prev) => ({
                                ...prev,
                                [cookie.id]: checked,
                              }));
                            }
                          }}
                          disabled={cookie.required}
                          className="data-[state=checked]:bg-primary"
                          aria-label={`${cookie.required ? 'Toujours actif' : localPreferences[cookie.id] ? 'Désactiver' : 'Activer'} ${cookie.name}`}
                        />
                      </div>
                    ))}
                  </div>
                </motion.div>
              )}
            </AnimatePresence>

            {/* Actions */}
            <div className="p-6 pt-4 bg-secondary/20 border-t border-border/50">
              <div className="flex flex-col sm:flex-row items-center gap-3">
                <Button
                  variant="outline"
                  size="sm"
                  onClick={() => setShowDetails(!showDetails)}
                  className="w-full sm:w-auto order-3 sm:order-1"
                >
                  {showDetails ? (
                    <>
                      <ChevronUp className="w-4 h-4 mr-2" />
                      Masquer les options
                    </>
                  ) : (
                    <>
                      <Settings className="w-4 h-4 mr-2" />
                      Personnaliser
                    </>
                  )}
                </Button>
                
                <div className="flex-1" />
                
                <Button
                  variant="outline"
                  size="sm"
                  onClick={handleRejectAll}
                  className="w-full sm:w-auto order-2"
                >
                  Refuser tout
                </Button>
                
                {showDetails ? (
                  <Button
                    size="sm"
                    onClick={handleSavePreferences}
                    className="w-full sm:w-auto order-1 sm:order-3 bg-primary hover:bg-primary/90"
                  >
                    <Shield className="w-4 h-4 mr-2" />
                    Enregistrer mes choix
                  </Button>
                ) : (
                  <Button
                    size="sm"
                    onClick={handleAcceptAll}
                    className="w-full sm:w-auto order-1 sm:order-3 bg-primary hover:bg-primary/90"
                  >
                    Accepter tout
                  </Button>
                )}
              </div>
            </div>
          </div>
        </motion.div>
      )}
    </AnimatePresence>
  );
};

export default CookieConsent;
