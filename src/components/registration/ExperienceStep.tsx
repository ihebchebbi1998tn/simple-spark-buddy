import { motion, AnimatePresence } from "framer-motion";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Button } from "@/components/ui/button";
import { CheckCircle2, CircleAlert, X } from "lucide-react";
import type { ExperienceStepProps } from "./types";

const activities = [
  { value: "1", label: "Télévente" },
  { value: "2", label: "Téléprospection" },
  { value: "3", label: "Prise de RDV" },
  { value: "4", label: "Service client" },
];

export function ExperienceStep({
  formData,
  handleInputChange,
  hasMinimalExperience,
  filteredOperations,
  filteredActivityExperience,
  filteredOperation1Experience,
  filteredOperation2Experience,
  isOperationsExperienceExceeded,
}: ExperienceStepProps) {
  return (
    <div className="space-y-4 sm:space-y-6">
      {/* Activity Selection */}
      <motion.div
        initial={{ opacity: 0, x: -10 }}
        animate={{ opacity: 1, x: 0 }}
        transition={{ delay: 0.2 }}
        className="space-y-2"
      >
        <Label className="flex flex-wrap items-center gap-2 font-bold text-base sm:text-lg">
          <span className="flex-1">Choisissez l'activité qui vous passionne</span>
          <span className="text-destructive">*</span>
          {formData.activitePrincipale && (
            <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500, damping: 30 }}>
              <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
            </motion.div>
          )}
        </Label>
        <Select value={formData.activitePrincipale} onValueChange={(value) => handleInputChange("activitePrincipale", value)}>
          <SelectTrigger className={`h-12 border-2 transition-all duration-300 input-glow ${formData.activitePrincipale ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md" : "hover:border-primary/40 hover:bg-primary/5"}`}>
            <SelectValue placeholder="Choisissez une activité" />
          </SelectTrigger>
          <SelectContent className="bg-background border-2 shadow-lg z-[100]">
            {activities.map((activity) => (
              <SelectItem key={activity.value} value={activity.value}>{activity.label}</SelectItem>
            ))}
          </SelectContent>
        </Select>
        <AnimatePresence>
          {formData.activitePrincipale && (
            <motion.p initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0 }} className="text-sm text-primary font-medium flex items-center gap-1">
              <CheckCircle2 className="w-4 h-4" /> Parfait ! Cette activité vous correspond bien ✨
            </motion.p>
          )}
        </AnimatePresence>
      </motion.div>

      {/* Activity Experience (only for non-minimal experience) */}
      <AnimatePresence>
        {formData.activitePrincipale && !hasMinimalExperience && (
          <motion.div initial={{ opacity: 0, height: 0, y: -20 }} animate={{ opacity: 1, height: "auto", y: 0 }} exit={{ opacity: 0, height: 0 }} transition={{ duration: 0.3, delay: 0.1 }} className="space-y-2">
            <Label className="flex flex-wrap items-center gap-2 font-bold text-base sm:text-lg">
              <span className="flex-1">Indiquez l'opération qui vous intéresse</span>
              <span className="text-destructive">*</span>
              {formData.experienceActivitePrincipale && (
                <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500, damping: 30 }}>
                  <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
                </motion.div>
              )}
            </Label>
            <Select value={formData.experienceActivitePrincipale} onValueChange={(value) => handleInputChange("experienceActivitePrincipale", value)}>
              <SelectTrigger className={`h-12 border-2 transition-all duration-300 input-glow ${formData.experienceActivitePrincipale ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md" : "hover:border-primary/40 hover:bg-primary/5"}`}>
                <SelectValue placeholder="Depuis combien de temps ?" />
              </SelectTrigger>
              <SelectContent className="bg-background border-2 shadow-lg z-[100]">
                {filteredActivityExperience.map((exp) => (
                  <SelectItem key={exp.value} value={exp.value}>{exp.label}</SelectItem>
                ))}
              </SelectContent>
            </Select>
          </motion.div>
        )}
      </AnimatePresence>

      {formData.activitePrincipale && (<>
        {/* Operation 1 */}
        <motion.div initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: 0.4 }} className="space-y-2">
          <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
            Indiquez l'opération qui vous intéresse le plus
            <span className="text-destructive">*</span>
            {formData.operation1 && (
              <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500, damping: 30 }}>
                <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
              </motion.div>
            )}
          </Label>
          <Select value={formData.operation1} onValueChange={(value) => handleInputChange("operation1", value)}>
            <SelectTrigger className={`h-12 border-2 transition-all duration-300 input-glow ${formData.operation1 ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md" : "hover:border-primary/40 hover:bg-primary/5"}`}>
              <SelectValue placeholder="Choisissez une opération" />
            </SelectTrigger>
            <SelectContent className="bg-background border-2 shadow-lg z-[100] max-h-[300px]">
              {filteredOperations.map((op) => (
                <SelectItem key={op.value} value={op.value}>{op.label}</SelectItem>
              ))}
            </SelectContent>
          </Select>
        </motion.div>

        {/* Operation 1 Experience */}
        <AnimatePresence>
          {formData.operation1 && !hasMinimalExperience && (
            <motion.div initial={{ opacity: 0, height: 0, y: -20 }} animate={{ opacity: 1, height: "auto", y: 0 }} exit={{ opacity: 0, height: 0 }} transition={{ duration: 0.3, delay: 0.1 }} className="space-y-2">
              <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
                Indiquez votre expérience dans l'opération sélectionnée
                <span className="text-destructive">*</span>
                {formData.experienceOperation1 && (
                  <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500, damping: 30 }}>
                    <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
                  </motion.div>
                )}
              </Label>
              <Select value={formData.experienceOperation1} onValueChange={(value) => handleInputChange("experienceOperation1", value)}>
                <SelectTrigger className={`h-12 border-2 transition-all duration-300 input-glow ${formData.experienceOperation1 ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md" : "hover:border-primary/40 hover:bg-primary/5"}`}>
                  <SelectValue placeholder="Depuis combien de temps ?" />
                </SelectTrigger>
                <SelectContent className="bg-background border-2 shadow-lg z-[100]">
                  {filteredOperation1Experience.map((exp) => (
                    <SelectItem key={exp.value} value={exp.value}>{exp.label}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </motion.div>
          )}
        </AnimatePresence>

        {/* Operation 2 */}
        <motion.div initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: 0.6 }} className="space-y-2">
          <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
            Souhaitez-vous choisir une deuxième opération ?
            {formData.operation2 && (
              <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500, damping: 30 }}>
                <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
              </motion.div>
            )}
          </Label>
          <div className="flex gap-2 items-center">
            <Select value={formData.operation2} onValueChange={(value) => handleInputChange("operation2", value)}>
              <SelectTrigger className={`h-12 border-2 transition-all duration-300 input-glow flex-1 ${formData.operation2 ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md" : "hover:border-primary/40 hover:bg-primary/5"}`}>
                <SelectValue placeholder="Une autre opération ?" />
              </SelectTrigger>
              <SelectContent className="bg-background border-2 shadow-lg z-[100] max-h-[300px]">
                {filteredOperations.filter(op => op.value !== formData.operation1).map((op) => (
                  <SelectItem key={op.value} value={op.value}>{op.label}</SelectItem>
                ))}
              </SelectContent>
            </Select>
            {formData.operation2 && (
              <Button type="button" variant="ghost" size="icon" className="h-12 w-12 shrink-0 text-muted-foreground hover:text-destructive"
                onClick={() => { handleInputChange("operation2", ""); handleInputChange("experienceOperation2", ""); }}
                title="Retirer la 2ème opération"
              >
                <X className="w-5 h-5" />
              </Button>
            )}
          </div>
          <AnimatePresence>
            {formData.operation2 && (
              <motion.p initial={{ opacity: 0, height: 0 }} animate={{ opacity: 1, height: "auto" }} exit={{ opacity: 0, height: 0 }} className="text-xs text-primary flex items-center gap-1">
                <CheckCircle2 className="w-3 h-3" /> Super ! Vous êtes polyvalent
              </motion.p>
            )}
          </AnimatePresence>
        </motion.div>

        {/* Operation 2 Experience */}
        <AnimatePresence>
          {formData.operation2 && !hasMinimalExperience && (
            <motion.div initial={{ opacity: 0, height: 0, y: -20 }} animate={{ opacity: 1, height: "auto", y: 0 }} exit={{ opacity: 0, height: 0 }} transition={{ duration: 0.3, delay: 0.1 }} className="space-y-2">
              <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
                Expérience dans l'opération 2<span className="text-destructive">*</span>
                {formData.experienceOperation2 && (
                  <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500, damping: 30 }}>
                    <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
                  </motion.div>
                )}
              </Label>
              <Select value={formData.experienceOperation2} onValueChange={(value) => handleInputChange("experienceOperation2", value)}>
                <SelectTrigger className={`h-12 border-2 transition-all duration-300 input-glow ${formData.experienceOperation2 ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md" : "hover:border-primary/40 hover:bg-primary/5"}`}>
                  <SelectValue placeholder="Depuis combien de temps ?" />
                </SelectTrigger>
                <SelectContent className="bg-background border-2 shadow-lg z-[100]">
                  {filteredOperation2Experience.map((exp) => (
                    <SelectItem key={exp.value} value={exp.value}>{exp.label}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </motion.div>
          )}
        </AnimatePresence>

        {/* Experience exceeded error */}
        <AnimatePresence>
          {isOperationsExperienceExceeded && !hasMinimalExperience && (
            <motion.div initial={{ opacity: 0, height: 0, y: -10 }} animate={{ opacity: 1, height: "auto", y: 0 }} exit={{ opacity: 0, height: 0, y: -10 }} transition={{ duration: 0.3 }} className="p-4 rounded-lg bg-destructive/10 border border-destructive/30">
              <div className="flex items-start gap-3">
                <CircleAlert className="w-5 h-5 text-destructive flex-shrink-0 mt-0.5" />
                <div className="space-y-1">
                  <p className="text-sm font-medium text-destructive">Expérience incohérente</p>
                  <p className="text-xs text-destructive/80">
                    L'expérience dans une opération ne peut pas être supérieure à celle de l'activité choisie. 
                    Veuillez réduire l'expérience des opérations ou augmenter celle de l'activité.
                  </p>
                </div>
              </div>
            </motion.div>
          )}
        </AnimatePresence>
      </>)}
    </div>
  );
}
