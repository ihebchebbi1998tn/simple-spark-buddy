import { useState, useEffect } from 'react';
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Checkbox } from '@/components/ui/checkbox';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Edit3, Save, X, Loader2 } from 'lucide-react';
import { toast } from '@/hooks/use-toast';
import { candidateService, type CandidateProfile } from '@/services/candidateService';

interface CallCentersSectionProps {
  profile: CandidateProfile | null;
  onRefresh: () => void;
  onProfileUpdate?: (updates: Partial<CandidateProfile>) => void;
}

const BLACKLIST_REASONS = [
  { value: 'mauvais_payeur', label: 'Mauvais payeur' },
  { value: 'ambiance', label: 'Ambiance non recommandée' },
  { value: 'localisation', label: 'Localisation géographique' },
];

const CallCentersSection = ({ profile, onRefresh, onProfileUpdate }: CallCentersSectionProps) => {
  const MAX_LIST_SIZE = 5;
  
  const [savedBlackList, setSavedBlackList] = useState<string[]>([]);
  const [savedWhiteList, setSavedWhiteList] = useState<string[]>([]);
  const [savedBlackListReasons, setSavedBlackListReasons] = useState<Record<string, string>>({});
  
  const [blackList, setBlackList] = useState<string[]>([]);
  const [whiteList, setWhiteList] = useState<string[]>([]);
  const [blackListReasons, setBlackListReasons] = useState<Record<string, string>>({});
  
  const [isEditing, setIsEditing] = useState(false);
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    if (profile?.availability) {
      const bl = profile.availability.blacklist || [];
      const wl = profile.availability.whitelist || [];
      const blReasons = (profile.availability as any).blacklist_reasons || {};
      setSavedBlackList(bl);
      setSavedWhiteList(wl);
      setSavedBlackListReasons(blReasons);
      setBlackList(bl);
      setWhiteList(wl);
      setBlackListReasons(blReasons);
    }
  }, [profile]);

  const callCenters = [
    'Teleperformance France', 'Webhelp (Concentrix)', 'Arvato CRM Solutions', 'Sitel Group',
    'Majorel France', 'Capgemini Business Services', 'Orange Business Services', 'Outsourcia',
    'Phone Régie', 'Servicom', 'Sellsy', 'Ubiqus', 'Expertise et Solutions',
    'Centre d\'appels Méditerranéen', 'Call Center Express', 'Phonexia Solutions',
    'DirectCall Services', 'TeleServices France', 'Customer Care Solutions',
    'Professional Call Center', 'Comdata France', 'Acticall Sitel Group',
    'Intelcia France', 'Atos Worldline', 'Armatis', 'Vivetic Group',
    'Transcom Worldwide', 'Groupe Prévoir', 'Voxens', 'Stream Global Services',
    'Convergys France', 'GEFCO Contact Center', 'Solucom Services', 'Teletech France',
    'Sykes France', 'Generix Contact', 'Calliweb', 'Phontel', 'Inbound Value',
    'Groupe SeLoger', 'Allô Services', 'Direct Assurance Centre', 'Mondial Relay Contact',
    'Veolia Contact', 'Engie Contact Center', 'EDF Centre Relation Client',
    'SNCF Centre Service', 'La Poste Contact', 'Bouygues Telecom Service',
    'SFR Centre Client', 'Free Centre Support', 'BNP Paribas Contact',
    'Société Générale Service', 'Crédit Agricole Contact', 'AXA Service Client',
    'Allianz Contact Center', 'Generali Service', 'MAIF Centre Relation',
    'MACIF Contact', 'Groupama Service Client', 'MMA Centre Contact',
    'Direct Energie Service', 'Total Energies Contact', 'Carrefour Service Client',
    'Auchan Contact', 'Leclerc Service', 'Decathlon Contact Center',
    'Fnac Darty Service', 'Boulanger Contact', 'Leroy Merlin Service',
    'Castorama Contact', 'Ikea Service Client', 'Amazon France Contact',
    'Cdiscount Service', 'Vente-privee Contact', 'Zalando Service France',
    'La Redoute Contact', '3 Suisses Service', 'Kiabi Contact Center',
    'H&M Service France', 'Zara Contact France', 'Sephora Service Client',
    'Yves Rocher Contact', 'L\'Oréal Service', 'Nocibé Contact',
    'Marionnaud Service', 'Air France Contact', 'SNCF Voyages Service',
    'Booking France Contact', 'Hotels.com Service', 'Airbnb Support France',
    'Uber Service France', 'Deliveroo Contact', 'Just Eat Service',
    'Doctolib Support', 'Ameli Service', 'CAF Contact Center',
    'Pôle Emploi Service', 'URSSAF Contact', 'Impots Service',
    'Netflix France Support'
  ];

  const handleEdit = () => setIsEditing(true);

  const handleCancel = () => {
    setBlackList(savedBlackList);
    setWhiteList(savedWhiteList);
    setBlackListReasons(savedBlackListReasons);
    setIsEditing(false);
  };

  const handleSave = async () => {
    try {
      setIsSaving(true);
      
      const response = await candidateService.updateCallCenters({
        blacklist: blackList,
        whitelist: whiteList,
        blacklist_reasons: blackListReasons
      });
      
      setSavedBlackList(blackList);
      setSavedWhiteList(whiteList);
      setSavedBlackListReasons(blackListReasons);
      
      if (onProfileUpdate && profile) {
        onProfileUpdate({
          ...profile,
          availability: { ...profile.availability, blacklist: blackList, whitelist: whiteList }
        });
      }
      
      toast({
        title: "Centres d'appels mis à jour",
        description: "Vos préférences ont été sauvegardées avec succès.",
        className: "bg-green-50 border-green-200 text-green-800",
        duration: 3000,
      });
      
      setIsEditing(false);
      onRefresh();
    } catch (error: any) {
      toast({ title: "Erreur", description: error.message || "La sauvegarde a échoué.", variant: "destructive" });
    } finally {
      setIsSaving(false);
    }
  };

  const toggleBlackList = (center: string) => {
    if (!isEditing) return;
    if (blackList.includes(center)) {
      setBlackList(prev => prev.filter(c => c !== center));
      setBlackListReasons(prev => { const n = { ...prev }; delete n[center]; return n; });
    } else {
      if (blackList.length >= MAX_LIST_SIZE) {
        toast({ title: "Limite atteinte", description: `Maximum ${MAX_LIST_SIZE} centres dans la black liste.`, className: "bg-rose-50 border-rose-200 text-rose-800" });
        return;
      }
      setBlackList(prev => [...prev, center]);
      setWhiteList(prev => prev.filter(c => c !== center));
    }
  };

  const toggleWhiteList = (center: string) => {
    if (!isEditing) return;
    if (whiteList.includes(center)) {
      setWhiteList(prev => prev.filter(c => c !== center));
    } else {
      if (whiteList.length >= MAX_LIST_SIZE) {
        toast({ title: "Limite atteinte", description: `Maximum ${MAX_LIST_SIZE} centres dans la white liste.`, className: "bg-emerald-50 border-emerald-200 text-emerald-800" });
        return;
      }
      setWhiteList(prev => [...prev, center]);
      setBlackList(prev => prev.filter(c => c !== center));
    }
  };

  const removeFromBlackList = (center: string) => {
    if (!isEditing) return;
    setBlackList(prev => prev.filter(c => c !== center));
    setBlackListReasons(prev => { const n = { ...prev }; delete n[center]; return n; });
  };

  const removeFromWhiteList = (center: string) => {
    if (!isEditing) return;
    setWhiteList(prev => prev.filter(c => c !== center));
  };

  const getAvailableCenters = () => callCenters.filter(c => !blackList.includes(c) && !whiteList.includes(c));

  const hasChanges = JSON.stringify(blackList) !== JSON.stringify(savedBlackList) || 
                     JSON.stringify(whiteList) !== JSON.stringify(savedWhiteList) ||
                     JSON.stringify(blackListReasons) !== JSON.stringify(savedBlackListReasons);

  return (
    <div className="space-y-4 sm:space-y-6">
      <Card className="border border-slate-200 shadow-sm bg-white">
        <CardContent className="pt-6">
          <div className="flex flex-col xs:flex-row xs:items-center justify-between mb-6 gap-2 xs:gap-0">
            <h2 className="text-base sm:text-lg font-semibold text-slate-800">Mes centres d'appels</h2>
            <div className="flex gap-2">
              {!isEditing ? (
                <Button variant="outline" size="sm" onClick={handleEdit} className="flex items-center gap-2">
                  <Edit3 className="h-4 w-4" /> Modifier
                </Button>
              ) : (
                <>
                  <Button variant="outline" size="sm" onClick={handleCancel} disabled={isSaving}>Annuler</Button>
                  <Button size="sm" onClick={handleSave} disabled={isSaving || !hasChanges} className="flex items-center gap-2 bg-emerald-600 hover:bg-emerald-700">
                    {isSaving ? <Loader2 className="h-4 w-4 animate-spin" /> : <Save className="h-4 w-4" />}
                    {isSaving ? 'Sauvegarde...' : 'Sauvegarder'}
                  </Button>
                </>
              )}
            </div>
          </div>

          {isEditing && (
            <div className="mb-4 p-3 bg-blue-50 border border-blue-200 rounded-lg">
              <p className="text-sm text-blue-800">✏️ Mode édition activé - Sélectionnez vos centres puis cliquez sur <strong>Sauvegarder</strong></p>
            </div>
          )}

          <div className="space-y-6 sm:space-y-8">
            {savedBlackList.length === 0 && savedWhiteList.length === 0 && (
              <div className="mb-4 p-4 bg-amber-50 border border-amber-200 rounded-xl">
                <h4 className="font-semibold text-amber-900 mb-2">⚠️ Configuration hautement recommandée</h4>
                <p className="text-amber-800 text-sm leading-relaxed">
                  Cette section vous permet de protéger votre anonymat et de nous indiquer les centres d'appels avec qui vous ne souhaitez pas être mis en relation. Cliquez sur <strong>Modifier</strong> pour commencer.
                </p>
              </div>
            )}
            
            {/* Black List */}
            <div className="space-y-4">
              <div className="flex items-center justify-between">
                <h3 className="text-lg font-bold text-rose-900">Black Liste</h3>
                <Badge className="bg-rose-100 text-rose-800">{blackList.length}/{MAX_LIST_SIZE}</Badge>
              </div>
              
              <div className="bg-rose-50 border border-rose-200 rounded-lg p-4">
                <p className="text-sm text-rose-700">
                  Centres que vous souhaitez <strong>absolument éviter</strong>. Ils ne verront jamais votre profil.
                </p>
              </div>
              
              {blackList.length > 0 && (
                <div className="space-y-2">
                  <h4 className="font-medium text-sm">Centres dans votre black liste:</h4>
                  <div className="flex flex-col gap-2">
                    {blackList.map((center) => (
                      <div key={center} className="flex flex-col sm:flex-row sm:items-center gap-2 p-3 bg-rose-50 border border-rose-200 rounded-md">
                        <div className="flex items-center gap-2 flex-1">
                          <span className="text-sm text-rose-800 font-medium">{center}</span>
                          {isEditing && (
                            <Button size="sm" variant="ghost" onClick={() => removeFromBlackList(center)} className="h-5 w-5 p-0 text-rose-600 hover:text-rose-800">
                              <X className="h-3 w-3" />
                            </Button>
                          )}
                        </div>
                        {/* Blacklist reason selector */}
                        <div className="flex items-center gap-2">
                          <span className="text-xs text-rose-600">Raison :</span>
                          <Select 
                            value={blackListReasons[center] || ''} 
                            onValueChange={(value) => setBlackListReasons(prev => ({ ...prev, [center]: value }))}
                            disabled={!isEditing}
                          >
                            <SelectTrigger className="h-8 text-xs w-[200px]">
                              <SelectValue placeholder="Sélectionnez une raison">
                                {BLACKLIST_REASONS.find(r => r.value === blackListReasons[center])?.label || 'Sélectionnez une raison'}
                              </SelectValue>
                            </SelectTrigger>
                            <SelectContent>
                              {BLACKLIST_REASONS.map((reason) => (
                                <SelectItem key={reason.value} value={reason.value}>{reason.label}</SelectItem>
                              ))}
                            </SelectContent>
                          </Select>
                        </div>
                      </div>
                    ))}
                  </div>
                </div>
              )}

              {isEditing && (
                <div className="space-y-3">
                  <h4 className="font-medium text-sm">Ajouter à la black liste:</h4>
                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-2 max-h-48 overflow-y-auto p-2 border border-border rounded-lg bg-card">
                    {getAvailableCenters().map((center) => (
                      <div key={center} className="flex items-center space-x-2">
                        <Checkbox id={`blacklist-${center}`} checked={false} onCheckedChange={() => toggleBlackList(center)} />
                        <label htmlFor={`blacklist-${center}`} className="text-sm cursor-pointer">{center}</label>
                      </div>
                    ))}
                  </div>
                </div>
              )}
            </div>

            <div className="border-t pt-6">
              {/* White List */}
              <div className="space-y-4">
                <div className="flex items-center justify-between">
                  <h3 className="text-lg font-bold text-emerald-900">White Liste</h3>
                  <Badge className="bg-emerald-100 text-emerald-800">{whiteList.length}/{MAX_LIST_SIZE}</Badge>
                </div>
                
                <div className="bg-emerald-50 border border-emerald-200 rounded-lg p-4">
                  <p className="text-sm text-emerald-700">
                    Centres avec lesquels vous souhaitez <strong>prioritairement travailler</strong>. Votre profil sera mis en avant.
                  </p>
                </div>
                
                {whiteList.length > 0 && (
                  <div className="space-y-2">
                    <h4 className="font-medium text-sm">Centres dans votre white liste:</h4>
                    <div className="flex flex-wrap gap-2">
                      {whiteList.map((center) => (
                        <div key={center} className="flex items-center space-x-2 p-2 bg-emerald-50 border border-emerald-200 rounded-md">
                          <span className="text-sm text-emerald-800">{center}</span>
                          {isEditing && (
                            <Button size="sm" variant="ghost" onClick={() => removeFromWhiteList(center)} className="h-4 w-4 p-0 text-emerald-600 hover:text-emerald-800">
                              <X className="h-3 w-3" />
                            </Button>
                          )}
                        </div>
                      ))}
                    </div>
                  </div>
                )}

                {isEditing && (
                  <div className="space-y-3">
                    <h4 className="font-medium text-sm">Ajouter à la white liste:</h4>
                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-2 max-h-48 overflow-y-auto p-2 border border-border rounded-lg bg-card">
                      {getAvailableCenters().map((center) => (
                        <div key={center} className="flex items-center space-x-2">
                          <Checkbox id={`whitelist-${center}`} checked={false} onCheckedChange={() => toggleWhiteList(center)} />
                          <label htmlFor={`whitelist-${center}`} className="text-sm cursor-pointer">{center}</label>
                        </div>
                      ))}
                    </div>
                  </div>
                )}
              </div>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default CallCentersSection;
