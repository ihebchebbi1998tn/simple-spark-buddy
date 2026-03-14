import { useState, useEffect } from 'react';
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Checkbox } from '@/components/ui/checkbox';
import { Edit3, Save, MapPin, Globe, X, Loader2 } from 'lucide-react';
import { toast } from '@/hooks/use-toast';
import { candidateService, type CandidateProfile } from '@/services/candidateService';

const tunisianCities = ['Tunis', 'Ariana', 'Ben Arous', 'Manouba', 'Nabeul', 'Zaghouan', 'Bizerte', 'Béja', 'Jendouba', 'Le Kef', 'Siliana', 'Kairouan', 'Kasserine', 'Sidi Bouzid', 'Sousse', 'Monastir', 'Mahdia', 'Sfax', 'Gabès', 'Médenine', 'Tataouine', 'Gafsa', 'Tozeur', 'Kébili'];

interface ProximitySectionProps {
  profile: CandidateProfile | null;
  onRefresh: () => void;
  onProfileUpdate?: (updates: Partial<CandidateProfile>) => void;
}

const ProximitySection = ({ profile, onRefresh, onProfileUpdate }: ProximitySectionProps) => {
  // Saved state (from database)
  const [savedData, setSavedData] = useState({
    villeResidence: 'Tunis',
    autresVilles: [] as string[],
    offresEtranger: false
  });
  
  // Working state (during editing)
  const [workingData, setWorkingData] = useState({
    villeResidence: 'Tunis',
    autresVilles: [] as string[],
    offresEtranger: false
  });
  
  const [isEditing, setIsEditing] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  
  // Sync with profile data when not editing
  useEffect(() => {
    if (profile && !isEditing) {
      console.log("📊 Syncing proximity data from profile");
      const loadedData = {
        villeResidence: profile.candidate?.city || 'Tunis',
        autresVilles: profile.availability?.nearby_cities || [],
        offresEtranger: profile.availability?.international_offers || false
      };
      setSavedData(loadedData);
      setWorkingData(loadedData);
    }
  }, [profile, isEditing]);

  const handleEdit = () => {
    // Copy saved data to working data when entering edit mode
    setWorkingData({ ...savedData });
    setIsEditing(true);
  };

  const handleCancel = () => {
    // Revert to saved data
    setWorkingData({ ...savedData });
    setIsEditing(false);
  };

  const handleSave = async () => {
    if (isSaving) return;
    
    try {
      setIsSaving(true);
      console.log("💾 Saving proximity preferences:", workingData);
      
      const response = await candidateService.updateProximityPreferences({
        villeResidence: workingData.villeResidence,
        villesProximite: workingData.autresVilles,
        offresEtranger: workingData.offresEtranger
      });
      
      console.log("✅ Proximity saved successfully:", response);
      
      // Update saved state with what we just saved
      setSavedData({ ...workingData });
      
      toast({
        title: "Proximité mise à jour",
        description: "Vos préférences géographiques ont été sauvegardées.",
        className: "bg-green-50 border-green-200 text-green-800",
        duration: 3000,
      });
      
      setIsEditing(false);
      onRefresh();
    } catch (error: any) {
      console.error("❌ Error saving proximity:", error);
      toast({
        title: "Erreur",
        description: "Impossible de sauvegarder vos préférences. Veuillez réessayer.",
        variant: "destructive"
      });
    } finally {
      setIsSaving(false);
    }
  };
  
  const toggleCity = (city: string) => {
    if (workingData.autresVilles.includes(city)) {
      setWorkingData(prev => ({
        ...prev,
        autresVilles: prev.autresVilles.filter(c => c !== city)
      }));
    } else {
      setWorkingData(prev => ({
        ...prev,
        autresVilles: [...prev.autresVilles, city]
      }));
    }
  };
  
  const setOffresEtranger = (value: boolean) => {
    setWorkingData(prev => ({ ...prev, offresEtranger: value }));
  };

  // Use working data when editing, saved data when not
  const displayData = isEditing ? workingData : savedData;
  const availableCities = tunisianCities.filter(city => city !== displayData.villeResidence);

  return (
    <div className="space-y-6">
      <Card className="border border-border bg-card">
        <div className="flex items-center justify-between p-6 border-b border-border">
          <h2 className="text-lg font-semibold text-foreground">Proximité Géographique</h2>
          <div className="flex gap-2">
            {isEditing ? (
              <>
                <Button variant="outline" size="sm" onClick={handleCancel} disabled={isSaving}>
                  <X className="h-4 w-4 mr-2" /> Annuler
                </Button>
                <Button variant="default" size="sm" onClick={handleSave} disabled={isSaving}>
                  {isSaving ? <Loader2 className="h-4 w-4 mr-2 animate-spin" /> : <Save className="h-4 w-4 mr-2" />}
                  {isSaving ? 'Sauvegarde...' : 'Sauvegarder'}
                </Button>
              </>
            ) : (
              <Button variant="outline" size="sm" onClick={handleEdit}>
                <Edit3 className="h-4 w-4 mr-2" /> Modifier
              </Button>
            )}
          </div>
        </div>

        <CardContent className="p-6 space-y-6">
          {/* Ville de résidence */}
          <div className="bg-muted border border-border rounded-lg p-5">
            <div className="flex items-center gap-3 mb-3">
              <div className="p-2 bg-primary rounded-lg">
                <MapPin className="h-5 w-5 text-primary-foreground" />
              </div>
              <Label className="font-semibold text-foreground text-base">Votre ville de résidence</Label>
            </div>
            <p className="text-foreground font-bold text-xl mb-2">{displayData.villeResidence}</p>
            <div className="flex items-center gap-2 bg-muted rounded-lg px-3 py-2 mt-3 border border-border">
              <span className="inline-block w-2 h-2 bg-primary rounded-full"></span>
              <p className="text-sm text-muted-foreground font-medium">Information pré-remplie lors de l'inscription</p>
            </div>
          </div>

          {/* Autres villes */}
          <div className="space-y-4">
            <div className="flex items-start gap-3 bg-muted border border-border rounded-lg p-4">
              <div className="p-2 bg-primary rounded-lg shrink-0">
                <MapPin className="h-5 w-5 text-primary-foreground" />
              </div>
              <div className="flex-1">
                <Label className="font-semibold text-foreground mb-2 block text-base">
                  Souhaitez-vous travailler dans d'autres villes ?
                </Label>
                <p className="text-sm text-muted-foreground leading-relaxed">
                  Élargissez vos opportunités professionnelles en sélectionnant d'autres villes tunisiennes où vous seriez disposé(e) à travailler. Plus vous sélectionnez de villes, plus vous augmentez vos chances de recevoir des offres adaptées.
                </p>
              </div>
            </div>

            {isEditing ? (
              <div className="grid grid-cols-2 sm:grid-cols-3 gap-3 max-h-96 overflow-y-auto p-4 border border-border rounded-lg bg-card">
                {availableCities.map(city => (
                  <div key={city} className="flex items-center space-x-2 p-2 hover:bg-muted rounded-lg transition-colors">
                    <Checkbox 
                      id={`city-${city}`} 
                      checked={workingData.autresVilles.includes(city)} 
                      onCheckedChange={() => toggleCity(city)} 
                    />
                    <Label htmlFor={`city-${city}`} className="text-sm cursor-pointer text-foreground font-medium">
                      {city}
                    </Label>
                  </div>
                ))}
              </div>
            ) : (
              <div className="p-5 border border-border rounded-lg bg-card">
                {displayData.autresVilles.length > 0 ? (
                  <div className="flex flex-wrap gap-2">
                    {displayData.autresVilles.map(city => (
                      <span key={city} className="px-4 py-2 bg-primary text-primary-foreground rounded-lg text-sm font-semibold">
                        {city}
                      </span>
                    ))}
                  </div>
                ) : (
                  <div className="flex items-center gap-3 text-muted-foreground">
                    <div className="p-2 bg-muted rounded-lg">
                      <MapPin className="h-5 w-5 text-muted-foreground" />
                    </div>
                    <p className="text-sm">
                      Aucune autre ville sélectionnée. Cliquez sur "Modifier" pour élargir votre zone de recherche.
                    </p>
                  </div>
                )}
              </div>
            )}
          </div>

          {/* Offres de l'étranger */}
          <div className="space-y-4">
            <div className="flex items-start gap-3 bg-muted border border-border rounded-lg p-4">
              <div className="p-2 bg-primary rounded-lg shrink-0">
                <Globe className="h-5 w-5 text-primary-foreground" />
              </div>
              <div className="flex-1">
                <Label className="font-semibold text-foreground mb-2 block text-base">
                  Souhaitez-vous recevoir des offres de l'étranger ?
                </Label>
                <p className="text-sm text-muted-foreground leading-relaxed">
                  Ouvrez-vous aux opportunités internationales ! De nombreux centres d'appels à l'étranger recherchent des profils francophones et offrent des conditions attractives (télétravail, salaires compétitifs, évolution de carrière).
                </p>
              </div>
            </div>

            {isEditing ? (
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <button
                  type="button"
                  onClick={() => setOffresEtranger(true)}
                  className={`h-auto py-5 px-4 flex flex-col items-center gap-3 rounded-lg transition-all duration-200 ${
                    workingData.offresEtranger
                      ? 'bg-green-50 border-2 border-green-400'
                      : 'bg-card border-2 border-border hover:border-green-300'
                  }`}
                >
                  <span className="text-3xl">✅</span>
                  <span className="font-semibold text-green-900 text-base">Oui, je suis intéressé(e)</span>
                  <span className="text-xs text-green-700">Maximiser mes opportunités</span>
                  {workingData.offresEtranger && (
                    <div className="mt-2 px-3 py-1 bg-green-600 text-white text-xs rounded-full font-medium">
                      ✓ Sélectionné
                    </div>
                  )}
                </button>
                <button
                  type="button"
                  onClick={() => setOffresEtranger(false)}
                  className={`h-auto py-5 px-4 flex flex-col items-center gap-3 rounded-lg transition-all duration-200 ${
                    !workingData.offresEtranger
                      ? 'bg-orange-50 border-2 border-orange-400'
                      : 'bg-card border-2 border-border hover:border-orange-300'
                  }`}
                >
                  <span className="text-3xl">🇹🇳</span>
                  <span className="font-semibold text-orange-900 text-base">Non, uniquement Tunisie</span>
                  <span className="text-xs text-orange-700">Offres locales seulement</span>
                  {!workingData.offresEtranger && (
                    <div className="mt-2 px-3 py-1 bg-orange-600 text-white text-xs rounded-full font-medium">
                      ✓ Sélectionné
                    </div>
                  )}
                </button>
              </div>
            ) : (
              <div className={`p-5 border-2 rounded-lg ${displayData.offresEtranger ? 'bg-green-50 border-green-400' : 'bg-orange-50 border-orange-400'}`}>
                <div className="flex items-start gap-4">
                  <span className="text-4xl">{displayData.offresEtranger ? '🌍' : '🇹🇳'}</span>
                  <div className="flex-1">
                    <div className="flex items-center gap-2 mb-2">
                      <p className={`font-bold text-lg ${displayData.offresEtranger ? 'text-green-900' : 'text-orange-900'}`}>
                        {displayData.offresEtranger ? 'Ouvert aux opportunités internationales' : 'Recherche locale uniquement (Tunisie)'}
                      </p>
                      <div className={`px-2 py-1 rounded-full text-xs font-bold ${displayData.offresEtranger ? 'bg-green-600 text-white' : 'bg-orange-600 text-white'}`}>
                        ✓ Actif
                      </div>
                    </div>
                    <p className={`text-sm ${displayData.offresEtranger ? 'text-green-800' : 'text-orange-800'} leading-relaxed`}>
                      {displayData.offresEtranger 
                        ? 'Vous recevrez des offres de centres d\'appels tunisiens et internationaux' 
                        : 'Vous recevrez uniquement des offres de centres d\'appels basés en Tunisie'}
                    </p>
                  </div>
                </div>
              </div>
            )}
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default ProximitySection;
