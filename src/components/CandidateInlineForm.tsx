import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useRegistrationStorage } from "@/hooks/useRegistrationStorage";
import { motion, AnimatePresence } from "framer-motion";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { useToast } from "@/hooks/use-toast";
import { TUNISIAN_CITIES, POSITIONS, EXPERIENCE_LEVELS } from "@/utils/constants";
import { useTranslation } from "react-i18next";

const languageKeys = ["francais", "anglais", "arabe", "espagnol", "italien", "allemand"] as const;
const availabilityKeys = ["immediate", "2_weeks", "1_month"] as const;
const workTimeKeys = ["full", "part", "any"] as const;
const workModeKeys = ["onsite", "remote", "hybrid", "any"] as const;
const sectorKeys = ["telecom", "bank", "ecommerce", "energy", "any"] as const;

const CandidateInlineForm = () => {
  const navigate = useNavigate();
  const { toast } = useToast();
  const { updateData } = useRegistrationStorage();
  const { t } = useTranslation('candidateForm');
  const [currentStep, setCurrentStep] = useState(0);

  const steps = t('steps', { returnObjects: true }) as string[];

  const [form, setForm] = useState({
    prenom: "", nom: "", telephone: "", civilite: "",
    villeResidence: "", codePostal: "", langueMaternelle: "",
    bilingue: "", disponibilite: "", tempsTravail: "",
    experienceGlobale: "", posteRecherche: "", modeTravail: "", secteur: "",
  });

  const set = (field: string, value: string) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const validateCurrentStep = (): boolean => {
    switch (currentStep) {
      case 0:
        if (!form.prenom.trim() || !form.nom.trim() || !form.telephone.trim()) {
          toast({ title: t('missingFields'), description: t('identityRequired'), variant: "destructive" });
          return false;
        }
        return true;
      case 1:
        if (!form.villeResidence) {
          toast({ title: t('missingField'), description: t('cityRequired'), variant: "destructive" });
          return false;
        }
        return true;
      case 2:
        if (!form.langueMaternelle) {
          toast({ title: t('missingField'), description: t('languageRequired'), variant: "destructive" });
          return false;
        }
        return true;
      case 3:
        if (!form.disponibilite || !form.tempsTravail) {
          toast({ title: t('missingFields'), description: t('availabilityRequired'), variant: "destructive" });
          return false;
        }
        return true;
      case 4:
        if (!form.experienceGlobale || !form.posteRecherche) {
          toast({ title: t('missingFields'), description: t('experienceRequired'), variant: "destructive" });
          return false;
        }
        return true;
      case 5:
        if (!form.modeTravail) {
          toast({ title: t('missingField'), description: t('workModeRequired'), variant: "destructive" });
          return false;
        }
        return true;
      default:
        return true;
    }
  };

  const handleNext = () => { if (validateCurrentStep() && currentStep < 5) setCurrentStep(currentStep + 1); };
  const handlePrev = () => { if (currentStep > 0) setCurrentStep(currentStep - 1); };

  const handleFinalize = () => {
    if (!validateCurrentStep()) return;
    updateData({
      prenom: form.prenom.trim(), nom: form.nom.trim(), telephone: form.telephone.trim(),
      civilite: form.civilite || "mme", villeResidence: form.villeResidence,
      experienceGlobale: form.experienceGlobale, posteRecherche: form.posteRecherche,
      modeTravail: form.modeTravail, tempsTravail: form.tempsTravail,
      bilingue: form.bilingue === "oui" ? "oui" : "non", langueMaternelle: form.langueMaternelle,
    });
    toast({ title: t('dataSaved'), description: t('dataSavedDesc') });
    navigate("/inscription-detaillee");
  };

  return (
    <div className="max-w-2xl mx-auto bg-card border border-border rounded-3xl shadow-lg overflow-hidden">
      <div className="flex border-b border-border">
        {steps.map((step, i) => (
          <div key={i}
            className={`flex-1 text-center py-3 text-[10px] sm:text-sm font-medium transition-colors cursor-pointer ${
              i === currentStep ? "bg-primary text-primary-foreground"
                : i < currentStep ? "bg-green-100 text-green-700" : "text-muted-foreground bg-muted/30"
            }`}
            onClick={() => { if (i < currentStep) setCurrentStep(i); }}>
            <span className="hidden sm:inline">{i + 1}. </span>{step}
          </div>
        ))}
      </div>

      <div className="p-6">
        <AnimatePresence mode="wait">
          <motion.div key={currentStep} initial={{ opacity: 0, x: 20 }} animate={{ opacity: 1, x: 0 }}
            exit={{ opacity: 0, x: -20 }} transition={{ duration: 0.2 }} className="space-y-4">

            {currentStep === 0 && (
              <>
                <div className="grid sm:grid-cols-2 gap-4">
                  <div className="space-y-1.5">
                    <Label className="text-sm font-medium">{t('firstName')} <span className="text-destructive">*</span></Label>
                    <Input placeholder="Rania" value={form.prenom} onChange={(e) => set("prenom", e.target.value)} />
                  </div>
                  <div className="space-y-1.5">
                    <Label className="text-sm font-medium">{t('lastName')} <span className="text-destructive">*</span></Label>
                    <Input placeholder="Ben Salem" value={form.nom} onChange={(e) => set("nom", e.target.value)} />
                  </div>
                </div>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">{t('phone')} <span className="text-destructive">*</span></Label>
                  <Input type="tel" placeholder="+216 XX XXX XXX" value={form.telephone} onChange={(e) => set("telephone", e.target.value)} />
                </div>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">{t('title')}</Label>
                  <div className="flex gap-3">
                    {[{ v: "mme", l: t('mrs') }, { v: "mr", l: t('mr') }].map((c) => (
                      <button key={c.v} type="button" onClick={() => set("civilite", c.v)}
                        className={`flex-1 h-10 rounded-lg border text-sm font-medium transition-colors ${
                          form.civilite === c.v ? "bg-primary/10 border-primary text-primary" : "bg-muted/50 border-border text-muted-foreground hover:border-primary/50"
                        }`}>{c.l}</button>
                    ))}
                  </div>
                </div>
              </>
            )}

            {currentStep === 1 && (
              <>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">{t('city')} <span className="text-destructive">*</span></Label>
                  <Select value={form.villeResidence} onValueChange={(v) => set("villeResidence", v)}>
                    <SelectTrigger><SelectValue placeholder={t('selectCity')} /></SelectTrigger>
                    <SelectContent>
                      {TUNISIAN_CITIES.map((c) => <SelectItem key={c.value} value={c.value}>{c.label}</SelectItem>)}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">{t('postalCode')}</Label>
                  <Input placeholder="1000" value={form.codePostal} onChange={(e) => set("codePostal", e.target.value)} />
                </div>
              </>
            )}

            {currentStep === 2 && (
              <>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">{t('mainLanguage')} <span className="text-destructive">*</span></Label>
                  <Select value={form.langueMaternelle} onValueChange={(v) => set("langueMaternelle", v)}>
                    <SelectTrigger><SelectValue placeholder={t('select')} /></SelectTrigger>
                    <SelectContent>
                      {languageKeys.map((k) => <SelectItem key={k} value={k}>{t(`languages.${k}`)}</SelectItem>)}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">{t('bilingual')}</Label>
                  <Select value={form.bilingue} onValueChange={(v) => set("bilingue", v)}>
                    <SelectTrigger><SelectValue placeholder={t('select')} /></SelectTrigger>
                    <SelectContent>
                      <SelectItem value="non">{t('no')}</SelectItem>
                      <SelectItem value="oui">{t('yes')}</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </>
            )}

            {currentStep === 3 && (
              <>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">{t('availability')} <span className="text-destructive">*</span></Label>
                  <Select value={form.disponibilite} onValueChange={(v) => set("disponibilite", v)}>
                    <SelectTrigger><SelectValue placeholder={t('select')} /></SelectTrigger>
                    <SelectContent>
                      {availabilityKeys.map((k) => <SelectItem key={k} value={k}>{t(`availabilityOptions.${k}`)}</SelectItem>)}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">{t('jobType')} <span className="text-destructive">*</span></Label>
                  <Select value={form.tempsTravail} onValueChange={(v) => set("tempsTravail", v)}>
                    <SelectTrigger><SelectValue placeholder={t('select')} /></SelectTrigger>
                    <SelectContent>
                      {workTimeKeys.map((k) => <SelectItem key={k} value={k}>{t(`workTimeOptions.${k}`)}</SelectItem>)}
                    </SelectContent>
                  </Select>
                </div>
              </>
            )}

            {currentStep === 4 && (
              <>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">{t('experience')} <span className="text-destructive">*</span></Label>
                  <Select value={form.experienceGlobale} onValueChange={(v) => set("experienceGlobale", v)}>
                    <SelectTrigger><SelectValue placeholder={t('select')} /></SelectTrigger>
                    <SelectContent>
                      <SelectItem value="0">{t('beginner')}</SelectItem>
                      {EXPERIENCE_LEVELS.map((e) => <SelectItem key={e.value} value={e.value}>{e.label}</SelectItem>)}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">{t('desiredPosition')} <span className="text-destructive">*</span></Label>
                  <Select value={form.posteRecherche} onValueChange={(v) => set("posteRecherche", v)}>
                    <SelectTrigger><SelectValue placeholder={t('select')} /></SelectTrigger>
                    <SelectContent>
                      {POSITIONS.map((p) => <SelectItem key={p.value} value={p.value}>{p.label}</SelectItem>)}
                    </SelectContent>
                  </Select>
                </div>
              </>
            )}

            {currentStep === 5 && (
              <>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">{t('workMode')} <span className="text-destructive">*</span></Label>
                  <Select value={form.modeTravail} onValueChange={(v) => set("modeTravail", v)}>
                    <SelectTrigger><SelectValue placeholder={t('select')} /></SelectTrigger>
                    <SelectContent>
                      {workModeKeys.map((k) => <SelectItem key={k} value={k}>{t(`workModeOptions.${k}`)}</SelectItem>)}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">{t('preferredSector')}</Label>
                  <Select value={form.secteur} onValueChange={(v) => set("secteur", v)}>
                    <SelectTrigger><SelectValue placeholder={t('select')} /></SelectTrigger>
                    <SelectContent>
                      {sectorKeys.map((k) => <SelectItem key={k} value={k}>{t(`sectorOptions.${k}`)}</SelectItem>)}
                    </SelectContent>
                  </Select>
                </div>
              </>
            )}
          </motion.div>
        </AnimatePresence>

        <div className="flex justify-between items-center pt-6 border-t border-border mt-6">
          <Button variant="ghost" onClick={handlePrev} className={`${currentStep === 0 ? "opacity-0 pointer-events-none" : ""}`}>
            {t('previous')}
          </Button>
          {currentStep < 5 ? (
            <Button onClick={handleNext} className="rounded-full px-8 font-semibold shadow-md">{t('next')}</Button>
          ) : (
            <Button onClick={handleFinalize} className="rounded-full px-8 font-semibold shadow-md bg-green-600 hover:bg-green-700 text-white">
              {t('finalize')}
            </Button>
          )}
        </div>
      </div>
    </div>
  );
};

export default CandidateInlineForm;
