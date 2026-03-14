import { motion, AnimatePresence } from "framer-motion";
import { Label } from "@/components/ui/label";
import { Button } from "@/components/ui/button";
import { CheckCircle2, Languages } from "lucide-react";
import { LANGUAGES, LANGUAGE_LEVELS } from "@/utils/constants";
import type { LanguagesStepProps } from "./types";

export function LanguagesStep({ formData, handleInputChange, renderStars }: LanguagesStepProps) {
  return (
    <div className="space-y-4 sm:space-y-6">
      {/* Mother Tongue */}
      <motion.div initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: 0.1 }} className="space-y-3">
        <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
          <Languages size={18} className="text-primary" />
          Langue maternelle
          <span className="text-destructive">*</span>
          {formData.langueMaternelle && (
            <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500, damping: 30 }}>
              <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
            </motion.div>
          )}
        </Label>
        <p className="text-sm text-muted-foreground">Indiquez la langue de votre pays d'origine</p>
        <div className="grid grid-cols-2 md:grid-cols-3 gap-3">
          {LANGUAGES.map((lang, index) => {
            const FlagIcon = lang.flag;
            const isSelected = formData.langueMaternelle === lang.value;
            return (
              <motion.div key={lang.value} initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.1 + index * 0.05 }} whileHover={{ scale: 1.05, y: -3 }} whileTap={{ scale: 0.95 }}>
                <Button type="button" variant={isSelected ? "default" : "outline"}
                  className={`h-20 w-full transition-all duration-300 relative overflow-hidden group ${isSelected ? "shadow-lg shadow-primary/30 border-primary/50 bg-gradient-to-br from-primary to-primary/80" : "hover:border-primary/40 hover:shadow-md"}`}
                  onClick={() => handleInputChange("langueMaternelle", lang.value)}
                >
                  <div className="relative flex flex-col items-center gap-2">
                    <FlagIcon className="w-12 h-8 rounded-md shadow-md" />
                    <span className="font-medium text-sm">{lang.label}</span>
                  </div>
                </Button>
              </motion.div>
            );
          })}
        </div>
      </motion.div>

      {/* First Foreign Language */}
      <AnimatePresence>
        {formData.langueMaternelle && (
          <motion.div initial={{ opacity: 0, height: 0, y: -20 }} animate={{ opacity: 1, height: "auto", y: 0 }} exit={{ opacity: 0, height: 0 }} transition={{ duration: 0.5 }} className="space-y-3">
            <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
              <Languages size={18} className="text-primary" />
              Langue étrangère principale
              <span className="text-destructive">*</span>
              {formData.langueForeign1 && (
                <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500 }}>
                  <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
                </motion.div>
              )}
            </Label>
            <p className="text-sm text-muted-foreground">Sélectionnez la première langue étrangère que vous maîtrisez</p>
            <div className="grid grid-cols-2 md:grid-cols-3 gap-3">
              {LANGUAGES.map((lang, index) => {
                const FlagIcon = lang.flag;
                const isSelected = formData.langueForeign1 === lang.value;
                const isDisabled = formData.langueMaternelle === lang.value;
                return (
                  <motion.div key={lang.value} initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: index * 0.05 }} whileHover={!isDisabled ? { scale: 1.05, y: -3 } : {}} whileTap={!isDisabled ? { scale: 0.95 } : {}}>
                    <Button type="button" variant={isSelected ? "default" : "outline"} disabled={isDisabled}
                      className={`h-20 w-full transition-all duration-300 relative overflow-hidden group ${isSelected ? "shadow-lg shadow-primary/30 border-primary/50 bg-gradient-to-br from-primary to-primary/80" : isDisabled ? "opacity-40 cursor-not-allowed" : "hover:border-primary/40 hover:shadow-md"}`}
                      onClick={() => !isDisabled && handleInputChange("langueForeign1", lang.value)}
                    >
                      <div className="relative flex flex-col items-center gap-2">
                        <FlagIcon className="w-12 h-8 rounded-md shadow-md" />
                        <span className="font-medium text-sm">{lang.label}</span>
                      </div>
                    </Button>
                  </motion.div>
                );
              })}
            </div>
            <AnimatePresence>
              {formData.langueForeign1 && (
                <motion.p initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0 }} className="text-sm text-primary font-medium flex items-center gap-1 bg-primary/10 rounded-lg p-3">
                  <CheckCircle2 className="w-4 h-4" /> Très bien ! Indiquez votre niveau ✨
                </motion.p>
              )}
            </AnimatePresence>
          </motion.div>
        )}
      </AnimatePresence>

      {/* First Foreign Language Level */}
      <AnimatePresence>
        {formData.langueForeign1 && (
          <motion.div initial={{ opacity: 0, height: 0, y: -20 }} animate={{ opacity: 1, height: "auto", y: 0 }} exit={{ opacity: 0, height: 0 }} transition={{ duration: 0.5 }} className="space-y-3">
            <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
              Parlez-nous de votre niveau
              <span className="text-destructive">*</span>
              {formData.niveauLangueForeign1 && (
                <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500 }}>
                  <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
                </motion.div>
              )}
            </Label>
            <div className="grid grid-cols-2 gap-3">
              {LANGUAGE_LEVELS.map((level, index) => {
                const isSelected = formData.niveauLangueForeign1 === level.value;
                return (
                  <motion.div key={level.value} initial={{ opacity: 0, scale: 0.9 }} animate={{ opacity: 1, scale: 1 }} transition={{ delay: index * 0.05 }} whileHover={{ scale: 1.03 }} whileTap={{ scale: 0.97 }}>
                    <Button type="button" variant={isSelected ? "default" : "outline"}
                      className={`h-16 w-full transition-all duration-300 flex flex-col gap-1 ${isSelected ? "shadow-lg shadow-primary/30 border-primary/50" : "hover:border-primary/40 hover:shadow-md"}`}
                      onClick={() => handleInputChange("niveauLangueForeign1", level.value)}
                    >
                      <span className="font-medium text-sm">{level.label}</span>
                      <div className="flex gap-0.5">{renderStars(level.stars)}</div>
                    </Button>
                  </motion.div>
                );
              })}
            </div>
            <AnimatePresence>
              {formData.niveauLangueForeign1 && (
                <motion.p initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0 }} className="text-sm text-primary font-medium flex items-center gap-1 bg-primary/10 rounded-lg p-3">
                  <CheckCircle2 className="w-4 h-4" /> Parfait ! Continuons ✨
                </motion.p>
              )}
            </AnimatePresence>
          </motion.div>
        )}
      </AnimatePresence>

      {/* Second Language Question */}
      <AnimatePresence>
        {formData.niveauLangueForeign1 && (
          <motion.div initial={{ opacity: 0, height: 0, y: -20 }} animate={{ opacity: 1, height: "auto", y: 0 }} exit={{ opacity: 0, height: 0 }} transition={{ duration: 0.5 }} className="space-y-3">
            <Label className="flex items-center gap-2 font-semibold text-base">
              Maitrisez-vous une autre langue étrangère ?
              <span className="text-destructive">*</span>
              {formData.hasSecondForeignLanguage && (
                <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500 }}>
                  <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
                </motion.div>
              )}
            </Label>
            <div className="grid grid-cols-2 gap-4">
              <Button type="button" variant={formData.hasSecondForeignLanguage === "oui" ? "default" : "outline"} className="h-14 transition-all duration-300 hover:scale-105" onClick={() => handleInputChange("hasSecondForeignLanguage", "oui")}>
                <span className="font-medium text-base">Oui</span>
              </Button>
              <Button type="button" variant={formData.hasSecondForeignLanguage === "non" ? "default" : "outline"} className="h-14 transition-all duration-300 hover:scale-105" onClick={() => handleInputChange("hasSecondForeignLanguage", "non")}>
                <span className="font-medium text-base">Non</span>
              </Button>
            </div>
            <AnimatePresence>
              {formData.hasSecondForeignLanguage === "oui" && (
                <motion.p initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0 }} className="text-sm text-primary font-medium flex items-center gap-1 bg-primary/10 rounded-lg p-3">
                  <CheckCircle2 className="w-4 h-4" /> Fantastique ! Ajoutez votre deuxième langue étrangère 🌟
                </motion.p>
              )}
              {formData.hasSecondForeignLanguage === "non" && (
                <motion.p initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0 }} className="text-sm text-primary font-medium flex items-center gap-1 bg-primary/10 rounded-lg p-3">
                  <CheckCircle2 className="w-4 h-4" /> Pas de problème ! Une langue étrangère suffit pour commencer 👍
                </motion.p>
              )}
            </AnimatePresence>
          </motion.div>
        )}
      </AnimatePresence>

      {/* Second Foreign Language */}
      <AnimatePresence>
        {formData.hasSecondForeignLanguage === "oui" && (
          <motion.div initial={{ opacity: 0, height: 0, y: -20 }} animate={{ opacity: 1, height: "auto", y: 0 }} exit={{ opacity: 0, height: 0 }} transition={{ duration: 0.5 }} className="space-y-3">
            <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
              <Languages size={18} className="text-primary" />
              Langue étrangère secondaire
              <span className="text-destructive">*</span>
              {formData.langueForeign2 && (
                <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500 }}>
                  <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
                </motion.div>
              )}
            </Label>
            <p className="text-sm text-muted-foreground">Sélectionnez la deuxième langue étrangère que vous maîtrisez</p>
            <div className="grid grid-cols-2 md:grid-cols-3 gap-3">
              {LANGUAGES.map((lang, index) => {
                const FlagIcon = lang.flag;
                const isSelected = formData.langueForeign2 === lang.value;
                const isDisabled = formData.langueMaternelle === lang.value || formData.langueForeign1 === lang.value;
                return (
                  <motion.div key={lang.value} initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: index * 0.05 }} whileHover={!isDisabled ? { scale: 1.05, y: -3 } : {}} whileTap={!isDisabled ? { scale: 0.95 } : {}}>
                    <Button type="button" variant={isSelected ? "default" : "outline"} disabled={isDisabled}
                      className={`h-20 w-full transition-all duration-300 relative overflow-hidden group ${isSelected ? "shadow-lg shadow-primary/30 border-primary/50 bg-gradient-to-br from-primary to-primary/80" : isDisabled ? "opacity-40 cursor-not-allowed" : "hover:border-primary/40 hover:shadow-md"}`}
                      onClick={() => !isDisabled && handleInputChange("langueForeign2", lang.value)}
                    >
                      <div className="relative flex flex-col items-center gap-2">
                        <FlagIcon className="w-12 h-8 rounded-md shadow-md" />
                        <span className="font-medium text-sm">{lang.label}</span>
                      </div>
                    </Button>
                  </motion.div>
                );
              })}
            </div>
            <AnimatePresence>
              {formData.langueForeign2 && (
                <motion.p initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0 }} className="text-sm text-primary font-medium flex items-center gap-1 bg-primary/10 rounded-lg p-3">
                  <CheckCircle2 className="w-4 h-4" /> Impressionnant ! Indiquez votre niveau ✨
                </motion.p>
              )}
            </AnimatePresence>
          </motion.div>
        )}
      </AnimatePresence>

      {/* Second Foreign Language Level */}
      <AnimatePresence>
        {formData.langueForeign2 && (
          <motion.div initial={{ opacity: 0, height: 0, y: -20 }} animate={{ opacity: 1, height: "auto", y: 0 }} exit={{ opacity: 0, height: 0 }} transition={{ duration: 0.5 }} className="space-y-3">
            <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
              Parlez-nous de votre niveau
              <span className="text-destructive">*</span>
              {formData.niveauLangueForeign2 && (
                <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500 }}>
                  <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
                </motion.div>
              )}
            </Label>
            <div className="grid grid-cols-2 gap-3">
              {LANGUAGE_LEVELS.map((level, index) => {
                const isSelected = formData.niveauLangueForeign2 === level.value;
                return (
                  <motion.div key={level.value} initial={{ opacity: 0, scale: 0.9 }} animate={{ opacity: 1, scale: 1 }} transition={{ delay: index * 0.05 }} whileHover={{ scale: 1.03 }} whileTap={{ scale: 0.97 }}>
                    <Button type="button" variant={isSelected ? "default" : "outline"}
                      className={`h-16 w-full transition-all duration-300 flex flex-col gap-1 ${isSelected ? "shadow-lg shadow-primary/30 border-primary/50" : "hover:border-primary/40 hover:shadow-md"}`}
                      onClick={() => handleInputChange("niveauLangueForeign2", level.value)}
                    >
                      <span className="font-medium text-sm">{level.label}</span>
                      <div className="flex gap-0.5">{renderStars(level.stars)}</div>
                    </Button>
                  </motion.div>
                );
              })}
            </div>
            <AnimatePresence>
              {formData.niveauLangueForeign2 && (
                <motion.p initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0 }} className="text-sm text-primary font-medium flex items-center gap-1 bg-primary/10 rounded-lg p-3">
                  <CheckCircle2 className="w-4 h-4" /> Excellent ! Une dernière question 🌟
                </motion.p>
              )}
            </AnimatePresence>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
}
