import { useState, useEffect, useRef } from 'react';
import {
  Phone, Save,
  PhoneCall, PhoneOff, Pause, Play, MicOff, Mic
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Textarea } from '@/components/ui/textarea';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import {
  Dialog, DialogContent, DialogHeader, DialogTitle,
} from '@/components/ui/dialog';
import { useToast } from '@/hooks/use-toast';
import type { CampaignLead } from '@/data/recruiterMockData';

interface Lead {
  id: string;
  nom: string;
  prenom: string;
  email: string;
  telephone: string;
  ville: string;
  statut: string;
  score: number;
  dateCreation: string;
  langues: string[];
  experience: string;
  activites: string[];
  disponibilite: string;
  source: string;
  campagne: string;
}

interface LeadDetailPopupProps {
  lead: Lead | null;
  isOpen: boolean;
  onClose: () => void;
  onStatusChange?: (leadId: string, newStatus: string) => void;
  campaignLead?: CampaignLead | null;
  onCampaignLeadSave?: (updatedLead: CampaignLead) => void;
  autoCall?: boolean;
}

const mockLead: Lead = {
  id: 'L-001', nom: 'Benali', prenom: 'Mohamed',
  email: 'mohamed.benali@email.com', telephone: '+216 22 *** ***',
  ville: 'Tunis', statut: 'a_traiter', score: 85,
  dateCreation: '15/01/2024', langues: ['Français', 'Arabe', 'Anglais'],
  experience: '3 ans', activites: ['Télévente', 'Service Client'],
  disponibilite: 'Immédiate', source: 'Formulaire web', campagne: 'Campagne Premium',
};

type CallState = 'idle' | 'ringing' | 'connected' | 'paused' | 'ended';

// --- Sub-components for cleaner structure ---

function CallBar({
  callState, callDuration, isPhoneRevealed, telephone, isMuted,
  onStartCall, onEndCall, onTogglePause, onToggleMute, formatDuration,
}: {
  callState: CallState;
  callDuration: number;
  isPhoneRevealed: boolean;
  telephone: string;
  isMuted: boolean;
  onStartCall: () => void;
  onEndCall: () => void;
  onTogglePause: () => void;
  onToggleMute: () => void;
  formatDuration: (s: number) => string;
}) {
  const isActive = callState === 'connected' || callState === 'paused';
  return (
    <div className={`rounded-xl border-2 p-4 transition-colors ${
      isActive ? 'border-emerald-500/40 bg-emerald-500/5' :
      callState === 'ringing' ? 'border-amber-500/40 bg-amber-500/5' :
      'border-border bg-muted/30'
    }`}>
      <div className="flex items-center justify-between gap-4 flex-wrap">
        <div className="flex items-center gap-3 min-w-0">
          <div className={`w-10 h-10 rounded-full flex items-center justify-center shrink-0 ${
            isActive ? 'bg-emerald-500/15' :
            callState === 'ringing' ? 'bg-amber-500/15' : 'bg-muted'
          }`}>
            <Phone className={`w-4.5 h-4.5 ${
              isActive ? 'text-emerald-600' :
              callState === 'ringing' ? 'text-amber-600 animate-pulse' : 'text-muted-foreground'
            }`} />
          </div>
          <div className="min-w-0">
            <p className="text-sm font-semibold">
              {callState === 'idle' && 'Appeler ce lead'}
              {callState === 'ringing' && 'Appel en cours...'}
              {callState === 'connected' && 'En communication'}
              {callState === 'paused' && 'En attente'}
              {callState === 'ended' && 'Appel terminé'}
            </p>
            <p className="text-xs text-muted-foreground font-mono mt-0.5">
              {isPhoneRevealed ? telephone.replace(/\*{3}/g, '345') : telephone}
              {isActive && (
                <span className="ml-2 tabular-nums text-foreground font-semibold">{formatDuration(callDuration)}</span>
              )}
            </p>
          </div>
        </div>
        <div className="flex items-center gap-2">
          {callState === 'idle' && (
            <Button size="sm" className="h-9 gap-1.5 px-4 rounded-lg" onClick={onStartCall}>
              <PhoneCall className="w-3.5 h-3.5" /> Appeler
            </Button>
          )}
          {isActive && (
            <>
              <Button variant="outline" size="sm" className="h-9 w-9 p-0 rounded-lg" onClick={onToggleMute}>
                {isMuted ? <MicOff className="w-3.5 h-3.5" /> : <Mic className="w-3.5 h-3.5" />}
              </Button>
              <Button variant="outline" size="sm" className="h-9 w-9 p-0 rounded-lg" onClick={onTogglePause}>
                {callState === 'paused' ? <Play className="w-3.5 h-3.5" /> : <Pause className="w-3.5 h-3.5" />}
              </Button>
              <Button variant="destructive" size="sm" className="h-9 gap-1.5 px-4 rounded-lg" onClick={onEndCall}>
                <PhoneOff className="w-3.5 h-3.5" /> Raccrocher
              </Button>
            </>
          )}
          {callState === 'ringing' && (
            <Button variant="destructive" size="sm" className="h-9 gap-1.5 px-4 rounded-lg" onClick={onEndCall}>
              <PhoneOff className="w-3.5 h-3.5" /> Annuler
            </Button>
          )}
        </div>
      </div>
    </div>
  );
}

function SectionTitle({ children }: { children: React.ReactNode }) {
  return (
    <h3 className="text-base font-bold text-primary mb-4 pb-3 border-b-[3px] border-primary/30">
      {children}
    </h3>
  );
}

function ReadOnlyField({ label, value, mono }: { label: string; value: string; mono?: boolean }) {
  return (
    <div className="space-y-1.5">
      <Label className="text-xs font-semibold text-muted-foreground">{label}</Label>
      <div className={`h-10 px-3 flex items-center rounded-lg bg-muted/50 border border-border text-sm ${mono ? 'font-mono' : ''}`}>
        {value}
      </div>
    </div>
  );
}

function FormField({ label, children }: { label: string; children: React.ReactNode }) {
  return (
    <div className="space-y-1.5">
      <Label className="text-xs font-semibold text-muted-foreground">{label}</Label>
      {children}
    </div>
  );
}

// --- Main component ---

export function LeadDetailPopup({ lead, isOpen, onClose, onStatusChange, campaignLead, onCampaignLeadSave, autoCall }: LeadDetailPopupProps) {
  const { toast } = useToast();
  const [isPhoneRevealed, setIsPhoneRevealed] = useState(false);

  const [callState, setCallState] = useState<CallState>('idle');
  const [callDuration, setCallDuration] = useState(0);
  const [isMuted, setIsMuted] = useState(false);
  const timerRef = useRef<ReturnType<typeof setInterval> | null>(null);

  const [qualForm, setQualForm] = useState({
    niveauLinguistique: '',
    experience: '',
    entretien: '',
    dateEntretien: '',
    presenceEntretien: '',
    retenu: '',
    dateFormation: '',
    presenceFormation: '',
    integration: '',
    statutTraitement: 'En cours',
    commentaire: '',
  });

  useEffect(() => {
    if (campaignLead) {
      setQualForm({
        niveauLinguistique: campaignLead.niveauLinguistique,
        experience: campaignLead.experience,
        entretien: campaignLead.entretien,
        dateEntretien: campaignLead.dateEntretien,
        presenceEntretien: campaignLead.presenceEntretien,
        retenu: campaignLead.retenu,
        dateFormation: campaignLead.dateFormation,
        presenceFormation: campaignLead.presenceFormation,
        integration: campaignLead.integration,
        statutTraitement: campaignLead.statutTraitement,
        commentaire: campaignLead.commentaire,
      });
      setIsPhoneRevealed(false);
      setCallState('idle');
      setCallDuration(0);
      setIsMuted(false);
    }
  }, [campaignLead]);

  useEffect(() => {
    if (!isOpen) {
      setCallState('idle');
      setCallDuration(0);
      setIsMuted(false);
      if (timerRef.current) clearInterval(timerRef.current);
    } else if (autoCall && callState === 'idle') {
      setIsPhoneRevealed(true);
      setCallState('ringing');
      setCallDuration(0);
      setTimeout(() => setCallState('connected'), 2000);
    }
  }, [isOpen, autoCall]);

  useEffect(() => {
    if (callState === 'connected') {
      timerRef.current = setInterval(() => setCallDuration(d => d + 1), 1000);
    } else {
      if (timerRef.current) clearInterval(timerRef.current);
    }
    return () => { if (timerRef.current) clearInterval(timerRef.current); };
  }, [callState]);

  const currentLead = lead || mockLead;

  const getStatusBadge = (statut: string) => {
    const map: Record<string, { label: string; className: string }> = {
      a_traiter: { label: 'En attente', className: 'bg-amber-500/10 text-amber-600 border-amber-500/30' },
      appele: { label: 'Contacté', className: 'bg-primary/10 text-primary border-primary/30' },
      contacted: { label: 'Contacté', className: 'bg-primary/10 text-primary border-primary/30' },
      entretien_programme: { label: 'Entretien', className: 'bg-violet-500/10 text-violet-600 border-violet-500/30' },
      entretien: { label: 'Entretien', className: 'bg-violet-500/10 text-violet-600 border-violet-500/30' },
      qualified: { label: 'Qualifié', className: 'bg-violet-500/10 text-violet-600 border-violet-500/30' },
      retenu: { label: 'Retenu', className: 'bg-emerald-500/10 text-emerald-600 border-emerald-500/30' },
      integre: { label: 'Intégré', className: 'bg-emerald-600/10 text-emerald-700 border-emerald-600/30' },
      integrated: { label: 'Intégré', className: 'bg-emerald-600/10 text-emerald-700 border-emerald-600/30' },
      non_joignable: { label: 'Non joignable', className: 'bg-muted text-muted-foreground' },
      refuse: { label: 'Refusé', className: 'bg-destructive/10 text-destructive border-destructive/30' },
      rejected: { label: 'Rejeté', className: 'bg-destructive/10 text-destructive border-destructive/30' },
      pending: { label: 'En attente', className: 'bg-amber-500/10 text-amber-600 border-amber-500/30' },
    };
    const cfg = map[statut] || { label: statut, className: '' };
    return <Badge className={`text-xs ${cfg.className}`}>{cfg.label}</Badge>;
  };

  const revealPhone = () => {
    setIsPhoneRevealed(true);
    toast({ title: 'Numéro révélé', description: 'Le numéro de téléphone est maintenant visible.' });
  };

  const startCall = () => {
    setCallState('ringing');
    setCallDuration(0);
    setTimeout(() => setCallState('connected'), 2000);
  };

  const endCall = () => {
    setCallState('ended');
    if (timerRef.current) clearInterval(timerRef.current);
    toast({
      title: 'Appel terminé',
      description: `${currentLead.prenom} ${currentLead.nom} — ${formatDuration(callDuration)}`,
    });
    setTimeout(() => setCallState('idle'), 1500);
  };

  const togglePause = () => {
    setCallState(callState === 'paused' ? 'connected' : 'paused');
  };

  const formatDuration = (s: number) => {
    const m = Math.floor(s / 60);
    const sec = s % 60;
    return `${m.toString().padStart(2, '0')}:${sec.toString().padStart(2, '0')}`;
  };

  const handleSaveQualification = () => {
    if (campaignLead && onCampaignLeadSave) {
      const updated: CampaignLead = { ...campaignLead, ...qualForm };
      if (qualForm.integration === 'Oui') updated.status = 'integrated';
      else if (qualForm.retenu === 'Oui') updated.status = 'qualified';
      else if (qualForm.niveauLinguistique) updated.status = 'contacted';
      if (qualForm.statutTraitement === 'Clôturé' && qualForm.experience === 'KO') updated.status = 'rejected';
      onCampaignLeadSave(updated);
    }
    toast({ title: 'Modifications enregistrées', description: `${currentLead.prenom} ${currentLead.nom} — qualification sauvegardée.` });
    onClose();
  };

  const selectClasses = "h-10 rounded-lg";

  return (
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent className="max-w-[920px] max-h-[92vh] p-0 gap-0 overflow-hidden w-[95vw] sm:w-full flex flex-col rounded-xl border-0 shadow-2xl">
        {/* Professional Header */}
        <div className="relative overflow-hidden shrink-0 rounded-t-xl">
          <div className="absolute inset-0 bg-gradient-to-br from-primary via-primary to-primary/80" />
          <div className="absolute inset-0 bg-[url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNDAiIGhlaWdodD0iNDAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PGNpcmNsZSBjeD0iMjAiIGN5PSIyMCIgcj0iMSIgZmlsbD0icmdiYSgyNTUsMjU1LDI1NSwwLjA1KSIvPjwvc3ZnPg==')] opacity-60" />
          <div className="relative px-6 py-5">
            <DialogHeader className="space-y-0">
              <div className="flex items-center gap-4">
                <div className="w-12 h-12 rounded-xl bg-white/15 backdrop-blur-sm flex items-center justify-center text-base font-bold text-primary-foreground shrink-0 border border-white/20 shadow-lg">
                  {currentLead.prenom.charAt(0)}{currentLead.nom.charAt(0)}
                </div>
                <div className="flex-1 min-w-0">
                  <DialogTitle className="text-lg font-bold text-primary-foreground tracking-tight">
                    {currentLead.prenom} {currentLead.nom}
                  </DialogTitle>
                  <div className="flex items-center gap-2 mt-1 flex-wrap">
                    <span className="text-xs text-primary-foreground/60 font-mono">{currentLead.id}</span>
                    <span className="w-1 h-1 rounded-full bg-primary-foreground/30" />
                    <span className="text-xs text-primary-foreground/60">{currentLead.campagne}</span>
                  </div>
                </div>
                <div className="shrink-0">
                  {getStatusBadge(currentLead.statut)}
                </div>
              </div>
            </DialogHeader>
          </div>
        </div>

        {/* Scrollable body */}
        <div className="flex-1 overflow-y-auto">
          <div className="p-6 sm:p-8 space-y-7">

            {/* Inline Call Bar */}
            <CallBar
              callState={callState}
              callDuration={callDuration}
              isPhoneRevealed={isPhoneRevealed}
              telephone={currentLead.telephone}
              isMuted={isMuted}
              onStartCall={startCall}
              onEndCall={endCall}
              onTogglePause={togglePause}
              onToggleMute={() => setIsMuted(!isMuted)}
              formatDuration={formatDuration}
            />

            {/* Two-column layout — generous spacing like HTML ref */}
            <div className="grid md:grid-cols-2 gap-8">
              {/* Left: Coordonnées du Lead (read-only) */}
              <div>
                <SectionTitle>Coordonnées du Lead</SectionTitle>
                <div className="space-y-4">
                  <ReadOnlyField label="ID" value={currentLead.id} mono />
                  <ReadOnlyField label="Civilité" value={campaignLead ? (campaignLead.civilite === 'M' ? 'Monsieur' : 'Madame') : '-'} />
                  <ReadOnlyField label="Nom" value={currentLead.nom} />
                  <ReadOnlyField label="Prénom" value={currentLead.prenom} />
                  <ReadOnlyField
                    label="Téléphone"
                    value={isPhoneRevealed ? currentLead.telephone.replace(/\*{3}/g, '345') : currentLead.telephone}
                    mono
                  />
                  <ReadOnlyField label="Joignabilité" value={campaignLead?.joignabilite || '-'} />
                  <ReadOnlyField label="Date d'appel" value={campaignLead?.dateAppel || currentLead.dateCreation} />

                  {!isPhoneRevealed && currentLead.telephone.includes('***') && (
                    <Button variant="outline" size="sm" onClick={revealPhone} className="w-full h-10 text-xs rounded-lg mt-1">
                      Révéler le numéro
                    </Button>
                  )}
                </div>
              </div>

              {/* Right: Qualification form */}
              <div>
                <SectionTitle>Qualification</SectionTitle>
                <div className="space-y-4">
                  <FormField label="Niveau Linguistique">
                    <Select value={qualForm.niveauLinguistique} onValueChange={v => setQualForm(p => ({ ...p, niveauLinguistique: v }))}>
                      <SelectTrigger className={selectClasses}><SelectValue placeholder="Sélectionner" /></SelectTrigger>
                      <SelectContent>
                        <SelectItem value="Débutant">Débutant</SelectItem>
                        <SelectItem value="Intermédiaire">Intermédiaire</SelectItem>
                        <SelectItem value="Bon">Bon</SelectItem>
                        <SelectItem value="Excellent">Excellent</SelectItem>
                      </SelectContent>
                    </Select>
                  </FormField>
                  <FormField label="Expérience & Soft Skills">
                    <Select value={qualForm.experience} onValueChange={v => setQualForm(p => ({ ...p, experience: v }))}>
                      <SelectTrigger className={selectClasses}><SelectValue placeholder="Sélectionner" /></SelectTrigger>
                      <SelectContent>
                        <SelectItem value="KO">KO</SelectItem>
                        <SelectItem value="Moyen">Moyen</SelectItem>
                        <SelectItem value="Bon">Bon</SelectItem>
                        <SelectItem value="Excellent">Excellent</SelectItem>
                      </SelectContent>
                    </Select>
                  </FormField>
                  <FormField label="Entretien physique">
                    <Select value={qualForm.entretien} onValueChange={v => setQualForm(p => ({ ...p, entretien: v }))}>
                      <SelectTrigger className={selectClasses}><SelectValue placeholder="Sélectionner" /></SelectTrigger>
                      <SelectContent>
                        <SelectItem value="Oui">Oui</SelectItem>
                        <SelectItem value="Non">Non</SelectItem>
                      </SelectContent>
                    </Select>
                  </FormField>
                  <FormField label="Date Entretien Physique">
                    <Input type="date" className="h-10 rounded-lg" value={qualForm.dateEntretien} onChange={e => setQualForm(p => ({ ...p, dateEntretien: e.target.value }))} />
                  </FormField>
                  <FormField label="Présence Entretien physique">
                    <Select value={qualForm.presenceEntretien} onValueChange={v => setQualForm(p => ({ ...p, presenceEntretien: v }))}>
                      <SelectTrigger className={selectClasses}><SelectValue placeholder="Sélectionner" /></SelectTrigger>
                      <SelectContent>
                        <SelectItem value="Oui">Oui</SelectItem>
                        <SelectItem value="Non">Non</SelectItem>
                      </SelectContent>
                    </Select>
                  </FormField>
                  <FormField label="Retenu">
                    <Select value={qualForm.retenu} onValueChange={v => setQualForm(p => ({ ...p, retenu: v }))}>
                      <SelectTrigger className={selectClasses}><SelectValue placeholder="Sélectionner" /></SelectTrigger>
                      <SelectContent>
                        <SelectItem value="Oui">Oui</SelectItem>
                        <SelectItem value="Non">Non</SelectItem>
                      </SelectContent>
                    </Select>
                  </FormField>
                  <FormField label="Date Formation">
                    <Input type="date" className="h-10 rounded-lg" value={qualForm.dateFormation} onChange={e => setQualForm(p => ({ ...p, dateFormation: e.target.value }))} />
                  </FormField>
                  <FormField label="Présence Formation">
                    <Select value={qualForm.presenceFormation} onValueChange={v => setQualForm(p => ({ ...p, presenceFormation: v }))}>
                      <SelectTrigger className={selectClasses}><SelectValue placeholder="Sélectionner" /></SelectTrigger>
                      <SelectContent>
                        <SelectItem value="Oui">Oui</SelectItem>
                        <SelectItem value="Non">Non</SelectItem>
                      </SelectContent>
                    </Select>
                  </FormField>
                  <FormField label="Intégration J+1">
                    <Select value={qualForm.integration} onValueChange={v => setQualForm(p => ({ ...p, integration: v }))}>
                      <SelectTrigger className={selectClasses}><SelectValue placeholder="Sélectionner" /></SelectTrigger>
                      <SelectContent>
                        <SelectItem value="Oui">Oui</SelectItem>
                        <SelectItem value="Non">Non</SelectItem>
                      </SelectContent>
                    </Select>
                  </FormField>
                  <FormField label="Statut Traitement">
                    <Select value={qualForm.statutTraitement} onValueChange={v => setQualForm(p => ({ ...p, statutTraitement: v }))}>
                      <SelectTrigger className={selectClasses}><SelectValue /></SelectTrigger>
                      <SelectContent>
                        <SelectItem value="En cours">En cours de traitement</SelectItem>
                        <SelectItem value="Clôturé">Traité & clôturé</SelectItem>
                        <SelectItem value="Intégré">Intégré</SelectItem>
                      </SelectContent>
                    </Select>
                  </FormField>
                  <FormField label="Commentaire">
                    <Textarea
                      value={qualForm.commentaire}
                      onChange={e => setQualForm(p => ({ ...p, commentaire: e.target.value }))}
                      placeholder="Ajoutez des commentaires ici..."
                      rows={3}
                      className="rounded-lg"
                    />
                  </FormField>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Footer — sticky, generous padding */}
        <div className="border-t bg-muted/30 px-6 py-4 flex items-center justify-end gap-3 shrink-0">
          <Button variant="outline" onClick={onClose} className="h-10 px-5 rounded-lg">
            Annuler
          </Button>
          <Button onClick={handleSaveQualification} className="h-10 px-5 rounded-lg shadow-sm">
            <Save className="w-4 h-4 mr-2" /> Enregistrer les modifications
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  );
}
