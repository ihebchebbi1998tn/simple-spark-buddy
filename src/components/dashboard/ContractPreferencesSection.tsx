import { useState, useEffect } from 'react';
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Checkbox } from '@/components/ui/checkbox';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Edit3, Save, Euro, Gift, Briefcase, Clock, Moon, Sun, X, Loader2 } from 'lucide-react';
import { toast } from '@/hooks/use-toast';
import { candidateService, type CandidateProfile } from '@/services/candidateService';

interface ContractPreferencesSectionProps {
  profile: CandidateProfile | null;
  onRefresh: () => void;
}

interface PreferencesData {
  modePresence: string[];
  typeTemps: string[];
  typeParc: string[];
  typeContrat: string[];
  salaireMin: string;
  salaireMax: string;
  ticketsRestaurant: boolean;
  mutuelle: boolean;
  transport: boolean;
  primes: boolean;
  commentaires: string;
}

const defaultPreferences: PreferencesData = {
  modePresence: ['presentiel'],
  typeTemps: ['temps_plein'],
  typeParc: ['jour'],
  typeContrat: [],
  salaireMin: '700',
  salaireMax: '1000',
  ticketsRestaurant: false,
  mutuelle: false,
  transport: false,
  primes: false,
  commentaires: ''
};

const ContractPreferencesSection = ({ profile, onRefresh }: ContractPreferencesSectionProps) => {
  const [isSaving, setIsSaving] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [lastProfileVersion, setLastProfileVersion] = useState<string | null>(null);
  
  // Saved state (from database)
  const [savedPreferences, setSavedPreferences] = useState<PreferencesData>({ ...defaultPreferences });
  
  // Working state (during editing)
  const [workingPreferences, setWorkingPreferences] = useState<PreferencesData>({ ...defaultPreferences });
  
  const decodeHtmlEntities = (str: string) => {
    if (!str) return str;
    return str
      .replace(/&eacute;/g, 'é')
      .replace(/&egrave;/g, 'è')
      .replace(/&agrave;/g, 'à')
      .replace(/&apos;/g, "'")
      .replace(/&quot;/g, '"');
  };
  
  const mapWorkModeToFrontend = (mode: string) => {
    const decoded = decodeHtmlEntities(mode);
    const modes: string[] = [];
    if (decoded?.includes('presentiel') || decoded?.includes('pr&eacute;sentiel') || decoded === '1') modes.push('presentiel');
    if (decoded?.includes('teletravail') || decoded?.includes('t&eacute;l&eacute;travail') || decoded === '2') modes.push('teletravail');
    return modes.length > 0 ? modes : ['presentiel'];
  };
  
  const mapWorkTimeToFrontend = (time: string) => {
    const decoded = decodeHtmlEntities(time);
    const times: string[] = [];
    if (decoded?.includes('plein') || decoded?.includes('mi_temps') === false && decoded === '1') times.push('temps_plein');
    if (decoded?.includes('partiel') || decoded?.includes('mi_temps') || decoded === '2') times.push('temps_partiel');
    return times.length > 0 ? times : ['temps_plein'];
  };
  
  const mapWorkParkToFrontend = (park: string) => {
    const decoded = decodeHtmlEntities(park);
    const parks: string[] = [];
    if (decoded?.includes('jour') || decoded?.includes('peu_importe') || decoded === '1') parks.push('jour');
    if (decoded?.includes('nuit') || decoded === '2') parks.push('nuit');
    return parks.length > 0 ? parks : ['jour'];
  };

  // Load data from profile when profile changes (using updated_at as version)
  useEffect(() => {
    if (!profile?.availability) return;
    
    // Create a version string from contractPreferences updated_at
    const currentVersion = profile.contractPreferences?.updated_at || (profile.candidate as any)?.updated_at || profile.candidate?._id || '';
    
    // Only reload if version changed and we're not editing
    if (currentVersion !== lastProfileVersion && !isEditing) {
      console.log("📊 [ContractPrefs] Loading from profile, version:", currentVersion);
      console.log("📊 [ContractPrefs] contractPreferences from profile:", profile.contractPreferences);
      
      const loadedData: PreferencesData = {
        ...defaultPreferences,
        modePresence: mapWorkModeToFrontend(profile.availability.work_mode),
        typeTemps: mapWorkTimeToFrontend(profile.availability.work_time),
        typeParc: mapWorkParkToFrontend(profile.availability.work_park)
      };
      
      if (profile.contractPreferences) {
        const cp = profile.contractPreferences;
        console.log("📊 [ContractPrefs] Loading contract_types:", cp.contract_types);
        if (cp.contract_types && Array.isArray(cp.contract_types)) {
          loadedData.typeContrat = [...cp.contract_types];
        }
        if (cp.salary_min !== undefined && cp.salary_min !== null) loadedData.salaireMin = String(cp.salary_min);
        if (cp.salary_max !== undefined && cp.salary_max !== null) loadedData.salaireMax = String(cp.salary_max);
        if (cp.tickets_restaurant !== undefined) loadedData.ticketsRestaurant = Boolean(cp.tickets_restaurant);
        if (cp.health_insurance !== undefined) loadedData.mutuelle = Boolean(cp.health_insurance);
        if (cp.transport !== undefined) loadedData.transport = Boolean(cp.transport);
        if (cp.bonuses !== undefined) loadedData.primes = Boolean(cp.bonuses);
        if (cp.comments !== undefined && cp.comments !== null) loadedData.commentaires = String(cp.comments);
      }
      
      console.log("📊 [ContractPrefs] Final loadedData:", loadedData);
      setSavedPreferences(loadedData);
      setWorkingPreferences(loadedData);
      setLastProfileVersion(currentVersion);
    }
  }, [profile, lastProfileVersion, isEditing]);

  const handleEdit = () => {
    // Copy saved data to working data when entering edit mode
    setWorkingPreferences({ ...savedPreferences });
    setIsEditing(true);
  };

  const handleCancel = () => {
    // Revert to saved data
    setWorkingPreferences({ ...savedPreferences });
    setIsEditing(false);
  };

  const handleSave = async () => {
    if (isSaving) return;
    
    setIsSaving(true);
    console.log("💾 Saving contract preferences:", workingPreferences);
    
    try {
      const availabilityData = {
        work_mode: workingPreferences.modePresence.join(', '),
        work_time: workingPreferences.typeTemps.join(', '),
        work_park: workingPreferences.typeParc.join(', ')
      };
      await candidateService.updateAvailability(availabilityData);
      
      const contractData = {
        contract_types: workingPreferences.typeContrat,
        salary_min: workingPreferences.salaireMin,
        salary_max: workingPreferences.salaireMax,
        tickets_restaurant: workingPreferences.ticketsRestaurant,
        health_insurance: workingPreferences.mutuelle,
        transport: workingPreferences.transport,
        bonuses: workingPreferences.primes,
        comments: workingPreferences.commentaires
      };
      const response = await candidateService.updateContractPreferences(contractData);
      
      console.log("✅ [ContractPrefs] Saved successfully, response:", response);
      
      // Update saved state with what we just saved
      setSavedPreferences({ ...workingPreferences });
      
      toast({
        title: "Préférences mises à jour",
        description: "Vos préférences contractuelles ont été sauvegardées.",
        className: "bg-green-50 border-green-200 text-green-800",
        duration: 3000,
      });
      
      setIsEditing(false);
      
      // Refresh profile to sync all dashboard components
      onRefresh();
    } catch (error) {
      console.error("❌ Error saving contract preferences:", error);
      toast({
        title: "Erreur de sauvegarde",
        description: error instanceof Error ? error.message : "Une erreur s'est produite lors de la sauvegarde",
        variant: "destructive",
      });
    } finally {
      setIsSaving(false);
    }
  };

  const toggleArrayValue = (array: string[], value: string) => {
    if (array.includes(value)) {
      return array.filter(item => item !== value);
    } else {
      return [...array, value];
    }
  };

  const updatePreferences = (field: keyof PreferencesData, value: any) => {
    setWorkingPreferences(prev => ({ ...prev, [field]: value }));
  };

  // Use working data when editing, saved data when not
  const displayPreferences = isEditing ? workingPreferences : savedPreferences;

  return (
    <div className="space-y-6">
      <Card className="border border-border bg-card">
        <CardContent className="pt-6">
          <div className="flex items-center justify-between mb-6">
            <h2 className="text-lg font-semibold text-foreground">Mes Préférences Contractuelles</h2>
            <div className="flex gap-2">
              {isEditing ? (
                <>
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={handleCancel}
                    disabled={isSaving}
                  >
                    <X className="h-4 w-4 mr-2" />
                    Annuler
                  </Button>
                  <Button
                    variant="default"
                    size="sm"
                    onClick={handleSave}
                    disabled={isSaving}
                  >
                    {isSaving ? (
                      <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                    ) : (
                      <Save className="h-4 w-4 mr-2" />
                    )}
                    {isSaving ? 'Sauvegarde...' : 'Sauvegarder'}
                  </Button>
                </>
              ) : (
                <Button
                  variant="outline"
                  size="sm"
                  onClick={handleEdit}
                >
                  <Edit3 className="h-4 w-4 mr-2" />
                  Modifier
                </Button>
              )}
            </div>
          </div>
          
          <div className="space-y-8">
            {/* Pre-filled from registration */}
            <div className="bg-muted border border-border rounded-lg p-5">
              <p className="text-sm text-foreground font-semibold mb-4 flex items-center gap-2">
                <span>Informations pré-remplies lors de l'inscription</span>
                <span className="text-muted-foreground text-xs font-normal">(modifiables à tout moment)</span>
              </p>
              
              <div className="space-y-4">
                {/* Mode de présence */}
                <div className="space-y-2">
                  <Label className="flex items-center gap-2 font-bold text-foreground">
                    <Briefcase className="h-5 w-5 text-primary" />
                    Mode de présence
                  </Label>
                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                    <div className="flex items-center space-x-2">
                      <Checkbox
                        id="presentiel"
                        checked={displayPreferences.modePresence.includes('presentiel')}
                        disabled={!isEditing}
                        onCheckedChange={() => 
                          updatePreferences('modePresence', toggleArrayValue(workingPreferences.modePresence, 'presentiel'))
                        }
                      />
                      <Label htmlFor="presentiel" className="text-sm">En présentiel</Label>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Checkbox
                        id="teletravail"
                        checked={displayPreferences.modePresence.includes('teletravail')}
                        disabled={!isEditing}
                        onCheckedChange={() => 
                          updatePreferences('modePresence', toggleArrayValue(workingPreferences.modePresence, 'teletravail'))
                        }
                      />
                      <Label htmlFor="teletravail" className="text-sm">Télétravail</Label>
                    </div>
                  </div>
                </div>

                {/* Type de temps */}
                <div className="space-y-2">
                  <Label className="flex items-center gap-2 font-bold text-foreground">
                    <Clock className="h-5 w-5 text-primary" />
                    Type de temps
                  </Label>
                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                    <div className="flex items-center space-x-2">
                      <Checkbox
                        id="temps_plein"
                        checked={displayPreferences.typeTemps.includes('temps_plein')}
                        disabled={!isEditing}
                        onCheckedChange={() => 
                          updatePreferences('typeTemps', toggleArrayValue(workingPreferences.typeTemps, 'temps_plein'))
                        }
                      />
                      <Label htmlFor="temps_plein" className="text-sm">Temps plein</Label>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Checkbox
                        id="temps_partiel"
                        checked={displayPreferences.typeTemps.includes('temps_partiel')}
                        disabled={!isEditing}
                        onCheckedChange={() => 
                          updatePreferences('typeTemps', toggleArrayValue(workingPreferences.typeTemps, 'temps_partiel'))
                        }
                      />
                      <Label htmlFor="temps_partiel" className="text-sm">Temps partiel</Label>
                    </div>
                  </div>
                </div>

                {/* Type de parc */}
                <div className="space-y-2">
                  <Label className="flex items-center gap-2 font-bold text-foreground">
                    <Sun className="h-5 w-5 text-primary" />
                    Type de parc
                  </Label>
                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                    <div className="flex items-center space-x-2">
                      <Checkbox
                        id="parc_jour"
                        checked={displayPreferences.typeParc.includes('jour')}
                        disabled={!isEditing}
                        onCheckedChange={() => 
                          updatePreferences('typeParc', toggleArrayValue(workingPreferences.typeParc, 'jour'))
                        }
                      />
                      <Label htmlFor="parc_jour" className="text-sm">Parc de jour</Label>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Checkbox
                        id="parc_nuit"
                        checked={displayPreferences.typeParc.includes('nuit')}
                        disabled={!isEditing}
                        onCheckedChange={() => 
                          updatePreferences('typeParc', toggleArrayValue(workingPreferences.typeParc, 'nuit'))
                        }
                      />
                      <Label htmlFor="parc_nuit" className="text-sm flex items-center gap-1">
                        Parc de nuit <Moon className="h-3 w-3" />
                      </Label>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            {/* Type de contrat souhaité */}
            <div className="space-y-4">
              <h3 className="text-base font-semibold text-foreground border-b border-border pb-2">
                Type de contrat souhaité <span className="text-muted-foreground text-sm font-normal">(Optionnel)</span>
              </h3>
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
                {['CDI / CIVP', 'CDD', 'Freelance', 'Auto-entrepreneur'].map((type) => (
                  <div key={type} className="flex items-center space-x-2">
                    <Checkbox
                      id={`contract-${type}`}
                      checked={displayPreferences.typeContrat.includes(type)}
                      disabled={!isEditing}
                      onCheckedChange={() => 
                        updatePreferences('typeContrat', toggleArrayValue(workingPreferences.typeContrat, type))
                      }
                    />
                    <Label htmlFor={`contract-${type}`} className="text-sm">{type}</Label>
                  </div>
                ))}
              </div>
            </div>

            {/* Avantages souhaités */}
            <div className="space-y-4">
              <h3 className="text-base font-semibold text-foreground border-b border-border pb-2 flex items-center">
                <Gift className="h-5 w-5 mr-2" />
                Avantages souhaités <span className="text-muted-foreground text-sm font-normal ml-2">(Optionnel - Valeurs par défaut)</span>
              </h3>

              {/* Prétentions salariales */}
              <div className="space-y-2">
                <Label className="flex items-center gap-2 font-medium">
                  <Euro className="h-4 w-4 text-muted-foreground" />
                  Prétentions salariales (TND/mois)
                </Label>
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                  <div className="space-y-1">
                    <Label htmlFor="salaire-min" className="text-sm text-muted-foreground">Minimum</Label>
                    <Input
                      id="salaire-min"
                      type="number"
                      value={displayPreferences.salaireMin}
                      onChange={(e) => updatePreferences('salaireMin', e.target.value)}
                      disabled={!isEditing}
                      placeholder="700"
                    />
                  </div>
                  <div className="space-y-1">
                    <Label htmlFor="salaire-max" className="text-sm text-muted-foreground">Maximum</Label>
                    <Input
                      id="salaire-max"
                      type="number"
                      value={displayPreferences.salaireMax}
                      onChange={(e) => updatePreferences('salaireMax', e.target.value)}
                      disabled={!isEditing}
                      placeholder="1000"
                    />
                  </div>
                </div>
              </div>

              {/* Autres avantages */}
              <div className="space-y-3 pl-4">
                <div className="flex items-center space-x-2">
                  <Checkbox
                    id="tickets-restaurant"
                    checked={displayPreferences.ticketsRestaurant}
                    disabled={!isEditing}
                    onCheckedChange={(checked) => updatePreferences('ticketsRestaurant', checked)}
                  />
                  <Label htmlFor="tickets-restaurant" className="text-sm">Tickets restaurant</Label>
                </div>

                <div className="flex items-center space-x-2">
                  <Checkbox
                    id="mutuelle"
                    checked={displayPreferences.mutuelle}
                    disabled={!isEditing}
                    onCheckedChange={(checked) => updatePreferences('mutuelle', checked)}
                  />
                  <Label htmlFor="mutuelle" className="text-sm">Mutuelle santé</Label>
                </div>

                <div className="flex items-center space-x-2">
                  <Checkbox
                    id="transport"
                    checked={displayPreferences.transport}
                    disabled={!isEditing}
                    onCheckedChange={(checked) => updatePreferences('transport', checked)}
                  />
                  <Label htmlFor="transport" className="text-sm">Prise en charge transport</Label>
                </div>

                <div className="flex items-center space-x-2">
                  <Checkbox
                    id="primes"
                    checked={displayPreferences.primes}
                    disabled={!isEditing}
                    onCheckedChange={(checked) => updatePreferences('primes', checked)}
                  />
                  <Label htmlFor="primes" className="text-sm">Primes de performance</Label>
                </div>
              </div>
            </div>

            {/* Commentaires */}
            <div className="space-y-2">
              <Label htmlFor="commentaires" className="text-base font-semibold text-foreground">
                Commentaires additionnels <span className="text-muted-foreground text-sm font-normal">(Optionnel)</span>
              </Label>
              <Textarea
                id="commentaires"
                placeholder="Ajoutez des informations supplémentaires sur vos attentes..."
                className="min-h-[100px]"
                value={displayPreferences.commentaires}
                onChange={(e) => updatePreferences('commentaires', e.target.value)}
                disabled={!isEditing}
              />
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default ContractPreferencesSection;