import { motion, AnimatePresence } from "framer-motion";
import { Label } from "@/components/ui/label";
import { Button } from "@/components/ui/button";
import { CheckCircle2, Home, Laptop, Sun, Moon, Clock } from "lucide-react";
import type { StepProps } from "./types";

const workModes = [
  { value: "presentiel", label: "Présentiel", icon: Home, gradient: "from-blue-500/20 to-indigo-500/20" },
  { value: "teletravail", label: "Télétravail", icon: Laptop, gradient: "from-green-500/20 to-emerald-500/20" },
  { value: "peu_importe", label: "Peu importe", icon: Home, gradient: "from-purple-500/20 to-pink-500/20" },
];

const workTimes = [
  { value: "plein_temps", label: "Plein temps", gradient: "from-orange-500/20 to-red-500/20" },
  { value: "mi_temps", label: "Mi-temps", gradient: "from-yellow-500/20 to-amber-500/20" },
  { value: "peu_importe", label: "Peu importe", gradient: "from-green-500/20 to-teal-500/20" },
];

const workShifts = [
  { value: "jour", label: "Parc de jour", icon: Sun, gradient: "from-amber-500/20 to-yellow-500/20" },
  { value: "nuit", label: "Parc de nuit", icon: Moon, gradient: "from-indigo-500/20 to-blue-500/20" },
  { value: "peu_importe", label: "Peu importe", icon: Clock, gradient: "from-violet-500/20 to-purple-500/20" },
];

export function WorkPreferencesStep({ formData, handleInputChange }: StepProps) {
  return (
    <div className="space-y-4 sm:space-y-6">
      {/* Work Mode */}
      <motion.div initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: 0.1 }} className="space-y-3">
        <Label className="flex flex-wrap items-center gap-2 font-bold text-base sm:text-lg">
          <Home size={18} className="text-primary flex-shrink-0" />
          <span className="flex-1">Mode de travail :</span>
          <span className="text-destructive">*</span>
          {formData.modeTravail && (
            <motion.div initial={{ scale: 0, rotate: -180 }} animate={{ scale: 1, rotate: 0 }} transition={{ type: "spring", stiffness: 500, damping: 25 }}>
              <CheckCircle2 className="w-5 h-5 text-primary ml-auto" />
            </motion.div>
          )}
        </Label>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
          {workModes.map((option, index) => (
            <motion.div key={option.value} initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.1 + index * 0.1 }} whileHover={{ scale: 1.05, y: -5 }} whileTap={{ scale: 0.95 }}>
              <Button type="button" variant={formData.modeTravail === option.value ? "default" : "outline"}
                className={`h-16 sm:h-20 w-full transition-all duration-300 relative overflow-hidden group ${formData.modeTravail === option.value ? "shadow-lg shadow-primary/30 border-primary/50" : "hover:border-primary/40 hover:shadow-md"}`}
                onClick={() => handleInputChange("modeTravail", option.value)}
              >
                <div className={`absolute inset-0 bg-gradient-to-br ${option.gradient} opacity-0 group-hover:opacity-100 transition-opacity duration-300 ${formData.modeTravail === option.value ? "opacity-50" : ""}`} />
                <div className="relative flex flex-col items-center gap-1.5 sm:gap-2">
                  <option.icon className={`w-5 h-5 sm:w-6 sm:h-6 transition-all duration-300 ${formData.modeTravail === option.value ? "scale-110 animate-pulse" : "group-hover:scale-110"}`} />
                  <span className="font-medium text-sm sm:text-base">{option.label}</span>
                </div>
              </Button>
            </motion.div>
          ))}
        </div>
        <AnimatePresence>
          {formData.modeTravail && (
            <motion.p initial={{ opacity: 0, height: 0, y: -10 }} animate={{ opacity: 1, height: "auto", y: 0 }} exit={{ opacity: 0, height: 0 }} className="text-sm text-primary font-medium flex items-center gap-2 bg-primary/10 rounded-lg p-3">
              <CheckCircle2 className="w-4 h-4 animate-pulse" />
              <span>Parfait ! Votre choix est enregistré✅</span>
            </motion.p>
          )}
        </AnimatePresence>
      </motion.div>

      {/* Work Time */}
      <motion.div initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: 0.2 }} className="space-y-3">
        <Label className="flex flex-wrap items-center gap-2 font-bold text-base sm:text-lg">
          <Clock size={18} className="text-primary flex-shrink-0" />
          <span className="flex-1">Temps de travail :</span>
          <span className="text-destructive">*</span>
          {formData.tempsTravail && (
            <motion.div initial={{ scale: 0, rotate: -180 }} animate={{ scale: 1, rotate: 0 }} transition={{ type: "spring", stiffness: 500, damping: 25 }}>
              <CheckCircle2 className="w-5 h-5 text-primary ml-auto" />
            </motion.div>
          )}
        </Label>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
          {workTimes.map((option, index) => (
            <motion.div key={option.value} initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.2 + index * 0.1 }} whileHover={{ scale: 1.05, y: -5 }} whileTap={{ scale: 0.95 }}>
              <Button type="button" variant={formData.tempsTravail === option.value ? "default" : "outline"}
                className={`h-16 sm:h-20 w-full transition-all duration-300 relative overflow-hidden group ${formData.tempsTravail === option.value ? "shadow-lg shadow-primary/30 border-primary/50" : "hover:border-primary/40 hover:shadow-md"}`}
                onClick={() => handleInputChange("tempsTravail", option.value)}
              >
                <div className={`absolute inset-0 bg-gradient-to-br ${option.gradient} opacity-0 group-hover:opacity-100 transition-opacity duration-300 ${formData.tempsTravail === option.value ? "opacity-50" : ""}`} />
                <div className="relative flex flex-col items-center gap-1.5 sm:gap-2">
                  <Clock className={`w-5 h-5 sm:w-6 sm:h-6 transition-all duration-300 ${formData.tempsTravail === option.value ? "scale-110 animate-pulse" : "group-hover:scale-110"}`} />
                  <span className="font-medium text-sm sm:text-base">{option.label}</span>
                </div>
              </Button>
            </motion.div>
          ))}
        </div>
        <AnimatePresence>
          {formData.tempsTravail && (
            <motion.p initial={{ opacity: 0, height: 0, y: -10 }} animate={{ opacity: 1, height: "auto", y: 0 }} exit={{ opacity: 0, height: 0 }} className="text-sm text-primary font-medium flex items-center gap-2 bg-primary/10 rounded-lg p-3">
              <CheckCircle2 className="w-4 h-4 animate-pulse" />
              <span>Super choix ! Continuez 💪</span>
            </motion.p>
          )}
        </AnimatePresence>
      </motion.div>

      {/* Work Shift */}
      <motion.div initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: 0.3 }} className="space-y-3">
        <Label className="flex flex-wrap items-center gap-2 font-bold text-base sm:text-lg">
          <Sun size={18} className="text-primary flex-shrink-0" />
          <span className="flex-1">Parc de travail :</span>
          <span className="text-destructive">*</span>
          {formData.parcTravail && (
            <motion.div initial={{ scale: 0, rotate: -180 }} animate={{ scale: 1, rotate: 0 }} transition={{ type: "spring", stiffness: 500, damping: 25 }}>
              <CheckCircle2 className="w-5 h-5 text-primary ml-auto" />
            </motion.div>
          )}
        </Label>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
          {workShifts.map((option, index) => (
            <motion.div key={option.value} initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.3 + index * 0.1 }} whileHover={{ scale: 1.05, y: -5 }} whileTap={{ scale: 0.95 }}>
              <Button type="button" variant={formData.parcTravail === option.value ? "default" : "outline"}
                className={`h-16 sm:h-20 w-full transition-all duration-300 relative overflow-hidden group ${formData.parcTravail === option.value ? "shadow-lg shadow-primary/30 border-primary/50" : "hover:border-primary/40 hover:shadow-md"}`}
                onClick={() => handleInputChange("parcTravail", option.value)}
              >
                <div className={`absolute inset-0 bg-gradient-to-br ${option.gradient} opacity-0 group-hover:opacity-100 transition-opacity duration-300 ${formData.parcTravail === option.value ? "opacity-50" : ""}`} />
                <div className="relative flex flex-col items-center gap-1.5 sm:gap-2">
                  <option.icon className={`w-5 h-5 sm:w-6 sm:h-6 transition-all duration-300 ${formData.parcTravail === option.value ? "scale-110 animate-pulse" : "group-hover:scale-110"}`} />
                  <span className="font-medium text-sm sm:text-base">{option.label}</span>
                </div>
              </Button>
            </motion.div>
          ))}
        </div>
        <AnimatePresence>
          {formData.parcTravail && (
            <motion.p initial={{ opacity: 0, height: 0, y: -10 }} animate={{ opacity: 1, height: "auto", y: 0 }} exit={{ opacity: 0, height: 0 }} className="text-sm text-primary font-medium flex items-center gap-2 bg-primary/10 rounded-lg p-3">
              <CheckCircle2 className="w-4 h-4 animate-pulse" />
              <span>Excellent ! Vos préférences sont notées ✨</span>
            </motion.p>
          )}
        </AnimatePresence>
      </motion.div>
    </div>
  );
}
