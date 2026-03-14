import { useState, useMemo } from 'react';
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group';
import {
  ArrowLeft,
  Users,
  MapPin,
  Clock,
  Target,
  Shield,
  Percent,
  Gift,
  Sparkles,
} from 'lucide-react';

interface PPIConfiguratorProps {
  onBack: () => void;
  onValidate: (config: PPIConfig) => void;
}

import type { PPIConfig } from '@/types/recruiter';
import { ppiOptions } from '@/data/recruiterMockData';

const { postes, experiences, activites, operations, langues, locations, urgencies, models } = ppiOptions;

const discounts = ppiOptions.discounts;

// Options are now imported from ppiOptions above

function SelectField({
  label,
  placeholder,
  options,
  value,
  onChange,
}: {
  label: string;
  placeholder: string;
  options: { value: string; label: string }[];
  value: string;
  onChange: (v: string) => void;
}) {
  return (
    <div className="space-y-1.5">
      <Label className="text-sm font-medium text-foreground">{label}</Label>
      <Select value={value} onValueChange={onChange}>
        <SelectTrigger className="h-10">
          <SelectValue placeholder={placeholder} />
        </SelectTrigger>
        <SelectContent>
          {options.map((opt) => (
            <SelectItem key={opt.value} value={opt.value}>
              {opt.label}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
    </div>
  );
}

export function PPIConfigurator({ onBack, onValidate }: PPIConfiguratorProps) {
  const [poste, setPoste] = useState('');
  const [expGlobale, setExpGlobale] = useState('');
  const [expPoste, setExpPoste] = useState('');
  const [activite, setActivite] = useState('');
  const [expActivite, setExpActivite] = useState('');
  const [operation, setOperation] = useState('');
  const [expOperation, setExpOperation] = useState('');
  const [genre, setGenre] = useState('both');
  const [languePrincipale, setLanguePrincipale] = useState('');
  const [bilingue, setBilingue] = useState('no');
  const [langueSecondaire, setLangueSecondaire] = useState('');
  const [candidatesCount, setCandidatesCount] = useState(5);
  const [location, setLocation] = useState('tunis');
  const [urgency, setUrgency] = useState('normal');
  const [model, setModel] = useState('integration');
  const [workMode, setWorkMode] = useState('both');
  const [workTime, setWorkTime] = useState('both');

  const pricing = useMemo(() => {
    const basePrice = 350;
    const locationMult = locations.find((l) => l.value === location)?.multiplier ?? 1;
    const urgencyMult = urgencies.find((u) => u.value === urgency)?.multiplier ?? 1;
    const modelMult = models.find((m) => m.value === model)?.multiplier ?? 1;
    const costPerCandidate = Math.round(basePrice * locationMult * urgencyMult * modelMult);
    const leadsProvided = candidatesCount * 50;
    const totalEstimate = costPerCandidate * candidatesCount;
    return { costPerCandidate, leadsProvided, totalEstimate };
  }, [location, urgency, model, candidatesCount]);

  const handleValidate = () => {
    onValidate({
      poste,
      expGlobale,
      expPoste,
      activite,
      expActivite,
      operation,
      expOperation,
      genre,
      languePrincipale,
      bilingue: bilingue === 'yes',
      langueSecondaire,
      candidatesCount,
      location,
      urgency,
      model,
      ...pricing,
    });
  };

  return (
    <div className="space-y-5 sm:space-y-6">
      {/* Header */}
      <div className="flex items-center gap-3">
        <Button variant="ghost" size="icon" onClick={onBack} className="shrink-0 h-9 w-9">
          <ArrowLeft className="w-4 h-4" />
        </Button>
        <div>
          <h2 className="text-lg sm:text-xl font-bold text-foreground">
            Configuration — Pay Per Integration
          </h2>
          <p className="text-sm text-muted-foreground">
            Configurez votre commande selon vos besoins spécifiques
          </p>
        </div>
      </div>

      {/* Critères des profils */}
      <Card className="border shadow-sm">
        <CardContent className="p-4 sm:p-5 space-y-4">
          <div className="flex items-center gap-2 mb-1">
            <Target className="w-4 h-4 text-primary" />
            <h3 className="font-semibold text-foreground text-sm sm:text-base">
              Critères des profils recherchés
            </h3>
          </div>
          <p className="text-xs text-muted-foreground -mt-2">
            Définissez précisément le type de candidat que vous recherchez
          </p>

          <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-4">
            <SelectField label="Poste proposé" placeholder="Sélectionnez" options={postes} value={poste} onChange={setPoste} />
            <SelectField label="Expérience globale en centres d'appels" placeholder="Sélectionnez" options={experiences} value={expGlobale} onChange={setExpGlobale} />
            <SelectField label="Expérience poste proposé" placeholder="Sélectionnez" options={experiences} value={expPoste} onChange={setExpPoste} />
            <SelectField label="Activité proposée" placeholder="Sélectionnez" options={activites} value={activite} onChange={setActivite} />
            <SelectField label="Expérience activité proposée" placeholder="Sélectionnez" options={experiences} value={expActivite} onChange={setExpActivite} />
            <SelectField label="Opération proposée" placeholder="Sélectionnez" options={operations} value={operation} onChange={setOperation} />
            <SelectField label="Expérience opération proposée" placeholder="Sélectionnez" options={experiences} value={expOperation} onChange={setExpOperation} />

            {/* Genre */}
            <div className="space-y-1.5">
              <Label className="text-sm font-medium text-foreground">Genre recherché</Label>
              <RadioGroup value={genre} onValueChange={setGenre} className="flex flex-wrap gap-3 pt-1">
                {[
                  { value: 'male', label: 'Hommes' },
                  { value: 'female', label: 'Femmes' },
                  { value: 'both', label: 'Les deux' },
                ].map((g) => (
                  <div key={g.value} className="flex items-center gap-2">
                    <RadioGroupItem value={g.value} id={`genre-${g.value}`} />
                    <Label htmlFor={`genre-${g.value}`} className="text-sm cursor-pointer">
                      {g.label}
                    </Label>
                  </div>
                ))}
              </RadioGroup>
            </div>
          </div>

          {/* Work Mode & Time */}
          <div className="grid sm:grid-cols-2 gap-4 pt-2 border-t border-border/50">
            <div className="space-y-1.5">
              <Label className="text-sm font-medium text-foreground">Mode de travail</Label>
              <div className="flex gap-2 pt-1">
                {[
                  { value: 'onsite', label: 'Présentiel' },
                  { value: 'remote', label: 'Télétravail' },
                  { value: 'both', label: 'Les deux' },
                ].map((mode) => (
                  <Button
                    key={mode.value}
                    type="button"
                    variant={workMode === mode.value ? 'default' : 'outline'}
                    size="sm"
                    onClick={() => setWorkMode(mode.value)}
                    className="flex-1 text-xs"
                  >
                    {mode.label}
                  </Button>
                ))}
              </div>
            </div>

            <div className="space-y-1.5">
              <Label className="text-sm font-medium text-foreground">Type de temps de travail</Label>
              <div className="flex gap-2 pt-1">
                {[
                  { value: 'full', label: 'Plein temps' },
                  { value: 'part', label: 'Temps partiel' },
                  { value: 'both', label: 'Les deux' },
                ].map((time) => (
                  <Button
                    key={time.value}
                    type="button"
                    variant={workTime === time.value ? 'default' : 'outline'}
                    size="sm"
                    onClick={() => setWorkTime(time.value)}
                    className="flex-1 text-xs"
                  >
                    {time.label}
                  </Button>
                ))}
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Langue */}
      <Card className="border shadow-sm">
        <CardContent className="p-4 sm:p-5 space-y-4">
          <div className="flex items-center gap-2 mb-1">
            <Sparkles className="w-4 h-4 text-primary" />
            <h3 className="font-semibold text-foreground text-sm sm:text-base">Langue</h3>
          </div>
          <p className="text-xs text-muted-foreground -mt-2">
            Indiquez la ou les langues de votre opération
          </p>

          <div className="grid sm:grid-cols-2 gap-4">
            <SelectField
              label="Langue principale"
              placeholder="Sélectionnez"
              options={langues}
              value={languePrincipale}
              onChange={setLanguePrincipale}
            />

            <div className="space-y-3">
              <Label className="text-sm font-medium text-foreground">
                Recherche de profils bilingues ?
              </Label>
              <RadioGroup value={bilingue} onValueChange={setBilingue} className="flex gap-4 pt-1">
                <div className="flex items-center gap-2">
                  <RadioGroupItem value="yes" id="bi-yes" />
                  <Label htmlFor="bi-yes" className="text-sm cursor-pointer">Oui</Label>
                </div>
                <div className="flex items-center gap-2">
                  <RadioGroupItem value="no" id="bi-no" />
                  <Label htmlFor="bi-no" className="text-sm cursor-pointer">Non</Label>
                </div>
              </RadioGroup>

              {bilingue === 'yes' && (
                <SelectField
                  label="Langue secondaire"
                  placeholder="Sélectionnez"
                  options={langues.filter((l) => l.value !== languePrincipale)}
                  value={langueSecondaire}
                  onChange={setLangueSecondaire}
                />
              )}
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Configuration de la commande */}
      <Card className="border shadow-sm">
        <CardContent className="p-4 sm:p-5 space-y-4">
          <div className="flex items-center gap-2 mb-1">
            <Users className="w-4 h-4 text-primary" />
            <h3 className="font-semibold text-foreground text-sm sm:text-base">
              Configuration de votre commande
            </h3>
          </div>
          <p className="text-xs text-muted-foreground -mt-2">
            Définissez le volume de votre recrutement
          </p>

          <div className="grid sm:grid-cols-2 gap-4">
            <div className="space-y-1.5">
              <Label className="text-sm font-medium text-foreground">
                Nombre de candidats recherchés
              </Label>
              <Input
                type="number"
                min={1}
                max={50}
                value={candidatesCount}
                onChange={(e) => setCandidatesCount(Math.max(1, Math.min(50, Number(e.target.value))))}
                className="h-10"
              />
            </div>

            <SelectField
              label="Localisation du centre"
              placeholder="Sélectionnez"
              options={locations.map((l) => ({ value: l.value, label: l.label }))}
              value={location}
              onChange={setLocation}
            />

            <SelectField
              label="Urgence du recrutement"
              placeholder="Sélectionnez"
              options={urgencies.map((u) => ({ value: u.value, label: u.label }))}
              value={urgency}
              onChange={setUrgency}
            />

            <SelectField
              label="Modèle Pay Per Integration"
              placeholder="Sélectionnez"
              options={models.map((m) => ({ value: m.value, label: m.label }))}
              value={model}
              onChange={setModel}
            />
          </div>
        </CardContent>
      </Card>

      {/* Quotation Results */}
      <div className="grid sm:grid-cols-3 gap-3 sm:gap-4">
        <Card className="border shadow-sm bg-primary/5 border-primary/20">
          <CardContent className="p-4 sm:p-5 text-center">
            <p className="text-xs sm:text-sm text-muted-foreground mb-1">Coût par candidat</p>
            <p className="text-2xl sm:text-3xl font-bold text-primary">
              {pricing.costPerCandidate} TND
            </p>
            <p className="text-[11px] text-muted-foreground mt-1">Prix de base avant remise</p>
          </CardContent>
        </Card>

        <Card className="border shadow-sm bg-primary/5 border-primary/20">
          <CardContent className="p-4 sm:p-5 text-center">
            <p className="text-xs sm:text-sm text-muted-foreground mb-1">Leads mis à disposition</p>
            <p className="text-2xl sm:text-3xl font-bold text-primary">
              {pricing.leadsProvided} leads
            </p>
            <p className="text-[11px] text-muted-foreground mt-1">
              Ratio de 50 leads par candidat
            </p>
          </CardContent>
        </Card>

        <Card className="border shadow-sm bg-primary/5 border-primary/20">
          <CardContent className="p-4 sm:p-5 text-center">
            <p className="text-xs sm:text-sm text-muted-foreground mb-1">Garantie</p>
            <p className="text-2xl sm:text-3xl font-bold text-primary">90 jours</p>
            <p className="text-[11px] text-muted-foreground mt-1">
              Remplacement gratuit si départ
            </p>
          </CardContent>
        </Card>
      </div>

      {/* Discount Grid */}
      <div>
        <div className="flex items-center gap-2 mb-3">
          <Percent className="w-4 h-4 text-primary" />
          <h3 className="font-semibold text-foreground text-sm sm:text-base">
            Grille de remise selon votre efficacité
          </h3>
        </div>
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-3">
          {discounts.map((d, i) => (
            <Card
              key={i}
              className={`border shadow-sm transition-all hover:shadow-md ${
                d.featured
                  ? 'border-emerald-500/50 bg-emerald-500/5'
                  : 'border-border'
              }`}
            >
              <CardContent className="p-3 sm:p-4 text-center">
                <p
                  className={`text-xl sm:text-2xl font-bold mb-1 ${
                    d.featured ? 'text-emerald-600' : 'text-primary'
                  }`}
                >
                  {d.percent}
                </p>
                <p className="text-xs text-muted-foreground leading-snug">{d.condition}</p>
              </CardContent>
            </Card>
          ))}
        </div>
      </div>

      {/* Total + Actions */}
      <Card className="border-2 border-primary/30 shadow-sm">
        <CardContent className="p-4 sm:p-5">
          <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
            <div className="text-center sm:text-left">
              <p className="text-sm text-muted-foreground">Estimation totale</p>
              <p className="text-2xl sm:text-3xl font-bold text-foreground">
                {pricing.totalEstimate.toLocaleString()} TND
              </p>
              <p className="text-xs text-muted-foreground">
                {candidatesCount} candidat{candidatesCount > 1 ? 's' : ''} × {pricing.costPerCandidate} TND
              </p>
            </div>
            <div className="flex gap-3">
              <Button variant="outline" onClick={onBack}>
                Retour aux offres
              </Button>
              <Button onClick={handleValidate}>
                <Shield className="w-4 h-4 mr-2" />
                Valider cette configuration
              </Button>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
