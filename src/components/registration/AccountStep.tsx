import { motion, AnimatePresence } from "framer-motion";
import { Label } from "@/components/ui/label";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import { Calendar } from "@/components/ui/calendar";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import { CheckCircle2, Mail, Lock, Shield, Check, CircleAlert, Loader2, Eye, EyeOff, Sparkles, Calendar as CalendarIcon } from "lucide-react";
import { format, parse } from "date-fns";
import { fr } from "date-fns/locale";
import { cn } from "@/lib/utils";
import type { AccountStepProps } from "./types";

export function AccountStep({
  formData,
  handleInputChange,
  fieldValidation,
  emailSuggestion,
  setEmailSuggestion,
  emailExists,
  isCheckingEmail,
  passwordStrength,
  backendPasswordError,
  showPassword,
  setShowPassword,
  showConfirmPassword,
  setShowConfirmPassword,
}: AccountStepProps) {
  return (
    <div className="space-y-4 sm:space-y-6">
      {/* Final Step Message */}
      <motion.div initial={{ opacity: 0, scale: 0.95, y: 20 }} animate={{ opacity: 1, scale: 1, y: 0 }} transition={{ duration: 0.5, type: "spring", stiffness: 200 }}
        className="relative overflow-hidden rounded-xl bg-gradient-to-br from-primary/20 via-primary/15 to-primary/10 border-2 border-primary/30 p-4 sm:p-6 text-center shadow-lg"
      >
        <motion.h3 initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.2 }}
          className="text-lg sm:text-2xl font-bold text-primary mb-2 flex items-center justify-center gap-2"
        >
          <CheckCircle2 className="w-5 h-5 sm:w-6 sm:h-6" /> Une dernière étape ! 🎉
        </motion.h3>
        <motion.p initial={{ opacity: 0, y: 10 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.4 }} className="text-sm sm:text-base text-muted-foreground">
          Obtenez vos codes d'accès et commencez votre nouvelle aventure professionnelle !
        </motion.p>
      </motion.div>

      {/* Email */}
      <motion.div initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: 0.1 }} className="space-y-2">
        <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
          <Mail size={18} /> Email <span className="text-destructive">*</span>
          {formData.email && fieldValidation.email?.isValid && (
            <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500, damping: 30 }}>
              <Check className="w-4 h-4 text-green-500 ml-auto" />
            </motion.div>
          )}
        </Label>
        <div className="relative">
          <Input type="email" placeholder="votre.email@exemple.com" value={formData.email}
            onChange={(e) => handleInputChange("email", e.target.value)}
            className={`h-12 transition-all duration-300 ${emailExists ? "border-destructive bg-destructive/5" : formData.email && fieldValidation.email?.isValid && emailExists === false ? "border-green-500 bg-green-50 dark:bg-green-950/20" : formData.email && fieldValidation.email?.error ? "border-destructive bg-destructive/5" : ""}`}
          />
          {formData.email && (
            <div className="absolute right-3 top-1/2 -translate-y-1/2">
              {isCheckingEmail ? <Loader2 className="h-5 w-5 text-muted-foreground animate-spin" /> :
                emailExists ? <CircleAlert className="h-5 w-5 text-destructive" /> :
                fieldValidation.email?.isValid && emailExists === false ? <Check className="h-5 w-5 text-green-500" /> :
                fieldValidation.email?.error ? <CircleAlert className="h-5 w-5 text-destructive" /> : null}
            </div>
          )}
        </div>
        <AnimatePresence mode="wait">
          {emailExists && (
            <motion.p key="email-exists" initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0, y: -10 }} className="text-xs text-destructive flex items-center gap-1">
              <CircleAlert className="w-3 h-3" /> Cet email est déjà utilisé. Veuillez vous connecter ou utiliser un autre email.
            </motion.p>
          )}
          {!emailExists && formData.email && fieldValidation.email?.error && (
            <motion.p key="email-error" initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0, y: -10 }} className="text-xs text-destructive flex items-center gap-1">
              <CircleAlert className="w-3 h-3" /> {fieldValidation.email.error}
            </motion.p>
          )}
          {emailSuggestion && !emailExists && (
            <motion.button key="email-suggestion" initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0, y: -10 }}
              onClick={() => { handleInputChange("email", emailSuggestion); setEmailSuggestion(undefined); }}
              className="text-xs text-primary hover:underline flex items-center gap-1"
            >
              💡 Vouliez-vous dire <strong>{emailSuggestion}</strong> ?
            </motion.button>
          )}
          {formData.email && fieldValidation.email?.isValid && emailExists === false && !isCheckingEmail && (
            <motion.p key="email-valid" initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0, y: -10 }} className="text-xs text-green-600 dark:text-green-500 flex items-center gap-1">
              <Check className="w-3 h-3" /> Email disponible ! Continuez
            </motion.p>
          )}
          {isCheckingEmail && (
            <motion.p key="email-checking" initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0, y: -10 }} className="text-xs text-muted-foreground flex items-center gap-1">
              <Loader2 className="w-3 h-3 animate-spin" /> Vérification en cours...
            </motion.p>
          )}
        </AnimatePresence>
      </motion.div>

      {/* Password */}
      <motion.div initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: 0.2 }} className="space-y-2">
        <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
          <Lock size={18} /> Mot de passe <span className="text-destructive">*</span>
        </Label>
        <div className="relative">
          <Input type={showPassword ? "text" : "password"} placeholder="Minimum 6 caractères" value={formData.motDePasse}
            onChange={(e) => handleInputChange("motDePasse", e.target.value)} className="h-12 pr-12"
          />
          <button type="button" onClick={() => setShowPassword(!showPassword)} className="absolute right-3 top-1/2 -translate-y-1/2 p-1 text-muted-foreground hover:text-foreground transition-colors">
            {showPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
          </button>
        </div>
        <AnimatePresence mode="wait">
          {backendPasswordError && (
            <motion.div key="backend-error" initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0, y: -10 }} className="p-3 rounded-lg bg-destructive/10 border border-destructive/30">
              <p className="text-xs text-destructive font-medium mb-1">Le mot de passe ne respecte pas les critères :</p>
              <ul className="text-xs text-destructive/80 list-disc pl-4 space-y-0.5">
                {backendPasswordError.split('.').filter(Boolean).map((err, i) => <li key={i}>{err.trim()}</li>)}
              </ul>
            </motion.div>
          )}
          {!backendPasswordError && formData.motDePasse && passwordStrength && (
            <motion.div key="strength" initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0, y: -10 }} className="space-y-2">
              <div className="flex items-center gap-2">
                <div className="flex-1 flex gap-1">
                  {[0, 1, 2, 3].map(i => (
                    <motion.div key={i} initial={{ scaleX: 0 }} animate={{ scaleX: 1 }} transition={{ delay: i * 0.05 }}
                      className="flex-1 h-2 rounded-full transition-colors duration-300"
                      style={{ backgroundColor: i < passwordStrength.score ? passwordStrength.color : 'hsl(var(--muted))' }}
                    />
                  ))}
                </div>
                <span className="text-xs font-semibold min-w-[80px] text-right" style={{ color: passwordStrength.color }}>{passwordStrength.label}</span>
              </div>
              {(() => {
                const pwd = formData.motDePasse;
                const items = [
                  { met: pwd.length >= 6, label: '6+ caractères' },
                  { met: /[A-Z]/.test(pwd), label: 'Une majuscule (A-Z)' },
                  { met: /[a-z]/.test(pwd), label: 'Une minuscule (a-z)' },
                  { met: /\d/.test(pwd), label: 'Un chiffre (0-9)' },
                ];
                const allMet = items.every(i => i.met);
                return (
                  <div className={`grid grid-cols-2 gap-x-4 gap-y-1 p-2 rounded-lg ${allMet ? 'bg-green-50 dark:bg-green-950/20 border border-green-200 dark:border-green-800/30' : 'bg-muted/30'}`}>
                    {items.map((item, i) => (
                      <motion.span key={i} initial={{ opacity: 0, x: -5 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: i * 0.05 }}
                        className={`text-xs flex items-center gap-1.5 ${item.met ? 'text-green-600 dark:text-green-500' : 'text-muted-foreground'}`}
                      >
                        {item.met ? <Check className="w-3 h-3 flex-shrink-0" /> : <span className="w-3 h-3 flex-shrink-0 rounded-full border border-muted-foreground/40 inline-block" />}
                        {item.label}
                      </motion.span>
                    ))}
                  </div>
                );
              })()}
              {passwordStrength.score === 4 && (
                <p className="text-xs text-green-600 dark:text-green-500 flex items-center gap-1">
                  <Shield className="w-3 h-3" /> Mot de passe très sécurisé !
                </p>
              )}
            </motion.div>
          )}
        </AnimatePresence>
      </motion.div>

      {/* Confirm Password */}
      <motion.div initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: 0.3 }} className="space-y-2">
        <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
          <Lock size={18} /> Confirmation du mot de passe <span className="text-destructive">*</span>
          {formData.confirmationMotDePasse && formData.motDePasse === formData.confirmationMotDePasse && (
            <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500, damping: 30 }}>
              <Check className="w-4 h-4 text-green-500 ml-auto" />
            </motion.div>
          )}
        </Label>
        <div className="relative">
          <Input type={showConfirmPassword ? "text" : "password"} placeholder="Confirmez votre mot de passe" value={formData.confirmationMotDePasse}
            onChange={(e) => handleInputChange("confirmationMotDePasse", e.target.value)}
            className={`h-12 pr-20 transition-all duration-300 ${formData.confirmationMotDePasse && formData.motDePasse === formData.confirmationMotDePasse ? "border-green-500 bg-green-50 dark:bg-green-950/20" : formData.confirmationMotDePasse ? "border-destructive bg-destructive/5" : ""}`}
          />
          <div className="absolute right-3 top-1/2 -translate-y-1/2 flex items-center gap-1">
            <button type="button" onClick={() => setShowConfirmPassword(!showConfirmPassword)} className="p-1 text-muted-foreground hover:text-foreground transition-colors">
              {showConfirmPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
            </button>
            {formData.confirmationMotDePasse && (formData.motDePasse === formData.confirmationMotDePasse ? <Check className="h-5 w-5 text-green-500" /> : <CircleAlert className="h-5 w-5 text-destructive" />)}
          </div>
        </div>
        <AnimatePresence mode="wait">
          {formData.confirmationMotDePasse && (
            <motion.p initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0, y: -10 }}
              className={`text-xs flex items-center gap-1 ${formData.motDePasse === formData.confirmationMotDePasse ? "text-green-600 dark:text-green-500" : "text-destructive"}`}
            >
              {formData.motDePasse === formData.confirmationMotDePasse ? <><Check className="w-3 h-3" /> Les mots de passe correspondent parfaitement !</> : <><CircleAlert className="w-3 h-3" /> Les mots de passe ne correspondent pas</>}
            </motion.p>
          )}
        </AnimatePresence>
      </motion.div>

      {/* Date of Birth */}
      <motion.div initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: 0.35 }} className="space-y-2">
        <Label className="flex items-center gap-2 font-bold text-base sm:text-lg">
          <CalendarIcon size={18} /> Date de naissance <span className="text-destructive">*</span>
          {formData.dateNaissance && (
            <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500, damping: 30 }}>
              <CheckCircle2 className="w-4 h-4 text-primary ml-auto" />
            </motion.div>
          )}
        </Label>
        <Popover>
          <PopoverTrigger asChild>
            <Button variant="outline" className={cn("w-full h-12 justify-start text-left font-normal transition-all duration-300", !formData.dateNaissance && "text-muted-foreground")}>
              <CalendarIcon className="mr-2 h-4 w-4" />
              {formData.dateNaissance ? format(parse(formData.dateNaissance, "yyyy-MM-dd", new Date()), "dd MMMM yyyy", { locale: fr }) : "Sélectionnez votre date de naissance"}
            </Button>
          </PopoverTrigger>
          <PopoverContent className="w-auto p-0" align="start">
            <Calendar mode="single"
              selected={formData.dateNaissance ? parse(formData.dateNaissance, "yyyy-MM-dd", new Date()) : undefined}
              onSelect={(date) => { if (date) handleInputChange("dateNaissance", format(date, "yyyy-MM-dd")); }}
              disabled={(date) => date > new Date(new Date().getFullYear() - 16, new Date().getMonth(), new Date().getDate()) || date < new Date("1925-01-01")}
              defaultMonth={formData.dateNaissance ? parse(formData.dateNaissance, "yyyy-MM-dd", new Date()) : new Date(2000, 0)}
              captionLayout="dropdown" fromYear={1925} toYear={new Date().getFullYear() - 16}
              className={cn("p-3 pointer-events-auto")} initialFocus
            />
          </PopoverContent>
        </Popover>
        <p className="text-xs text-muted-foreground">Vous devez avoir au moins 16 ans pour vous inscrire</p>
      </motion.div>

      {/* CGU Checkbox */}
      <motion.div initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: 0.4 }} className="flex items-center space-x-2 pt-4">
        <Checkbox id="acceptCGU" checked={formData.acceptCGU} onCheckedChange={(checked) => handleInputChange("acceptCGU", checked as boolean)} />
        <label htmlFor="acceptCGU" className="text-sm leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70 flex items-center gap-2">
          <span>
            J'accepte les{" "}<a href="/terms-of-service" className="text-primary hover:underline">Conditions Générales d'Utilisation</a>{" "}et la{" "}<a href="/privacy-policy" className="text-primary hover:underline">Politique de Confidentialité</a>
            <span className="text-destructive">*</span>
          </span>
          {formData.acceptCGU && (
            <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ type: "spring", stiffness: 500, damping: 30 }}>
              <CheckCircle2 className="w-4 h-4 text-primary" />
            </motion.div>
          )}
        </label>
      </motion.div>

      <AnimatePresence>
        {formData.acceptCGU && (
          <motion.div initial={{ opacity: 0, y: 10 }} animate={{ opacity: 1, y: 0 }} exit={{ opacity: 0, y: 10 }} className="bg-primary/10 border border-primary/20 rounded-lg p-4 text-center">
            <p className="text-sm text-primary font-medium flex items-center justify-center gap-2">
              <Sparkles className="w-4 h-4" /> Bravo ! Vous êtes prêt à finaliser votre inscription
            </p>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
}
