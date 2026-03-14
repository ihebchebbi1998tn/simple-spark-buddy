import { useState, useRef } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Progress } from '@/components/ui/progress';

import {
  Table, TableBody, TableCell, TableHead, TableHeader, TableRow
} from '@/components/ui/table';
import {
  ArrowLeft, Users, Phone, UserCheck, CheckCircle, Clock, Eye, RefreshCw,
  PhoneOff, CircleDot, PhoneCall
} from 'lucide-react';
import type { Campaign } from '@/types/recruiter';
import { mockCampaignLeads, type CampaignLead } from '@/data/recruiterMockData';
import { LeadDetailPopup } from './LeadDetailPopup';

import { useToast } from '@/hooks/use-toast';

interface CampaignDetailViewProps {
  campaign: Campaign;
  onBack: () => void;
}

const STATUS_CONFIG: Record<string, { label: string; className: string }> = {
  pending: { label: 'À traiter', className: 'bg-amber-500/10 text-amber-600 border-amber-500/30' },
  contacted: { label: 'Contacté', className: 'bg-primary/10 text-primary border-primary/30' },
  qualified: { label: 'Qualifié', className: 'bg-violet-500/10 text-violet-600 border-violet-500/30' },
  rejected: { label: 'Rejeté', className: 'bg-destructive/10 text-destructive border-destructive/30' },
  integrated: { label: 'Intégré', className: 'bg-emerald-500/10 text-emerald-600 border-emerald-500/30' },
};


export function CampaignDetailView({ campaign, onBack }: CampaignDetailViewProps) {
  const { toast } = useToast();
  const [leads, setLeads] = useState<CampaignLead[]>(
    () => mockCampaignLeads[campaign.id] || []
  );
  const [selectedLead, setSelectedLead] = useState<CampaignLead | null>(null);
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const [statusFilter, setStatusFilter] = useState<string>('all');
  
  const [autoCallOnOpen, setAutoCallOnOpen] = useState(false);
  const ringingAudioRef = useRef<HTMLAudioElement | null>(null);

  const filteredLeads = statusFilter === 'all'
    ? leads
    : leads.filter(l => l.status === statusFilter);

  const stats = {
    total: leads.length,
    pending: leads.filter(l => l.status === 'pending').length,
    contacted: leads.filter(l => l.status === 'contacted').length,
    qualified: leads.filter(l => l.status === 'qualified').length,
    integrated: leads.filter(l => l.status === 'integrated').length,
    rejected: leads.filter(l => l.status === 'rejected').length,
  };

  const handleLeadSave = (updatedLead: CampaignLead) => {
    setLeads(prev => prev.map(l => l.id === updatedLead.id ? updatedLead : l));
  };

  const stopRinging = () => {
    if (ringingAudioRef.current) {
      ringingAudioRef.current.pause();
      ringingAudioRef.current.currentTime = 0;
      ringingAudioRef.current = null;
    }
  };

  const handleCall = (lead: CampaignLead) => {
    try {
      stopRinging();
      const audio = new Audio('/sounds/phone-ringing.mp3');
      audio.volume = 0.5;
      ringingAudioRef.current = audio;
      audio.play().catch(() => console.log('Could not play ringing sound'));
    } catch (e) {
      console.log('Audio not available');
    }
    setSelectedLead(lead);
    setAutoCallOnOpen(true);
    setIsPopupOpen(true);
  };

  const renderLeadRow = (lead: CampaignLead, showCharge = false) => {
    const statusCfg = STATUS_CONFIG[lead.status] || STATUS_CONFIG.pending;
    return (
      <TableRow key={lead.id} className="group">
        <TableCell className="font-mono text-xs">{lead.id}</TableCell>
        <TableCell>
          <div className="flex items-center gap-2.5">
            <div className="relative shrink-0">
              <div className="w-8 h-8 rounded-full bg-primary/10 flex items-center justify-center text-xs font-semibold text-primary">
                {lead.prenom[0]}{lead.nom[0]}
              </div>
              <div className={`absolute -top-0.5 -right-0.5 w-2.5 h-2.5 rounded-full border-2 border-background ${
                lead.status === 'pending' ? 'bg-amber-400' :
                lead.status === 'rejected' ? 'bg-destructive' :
                lead.joignabilite === 'Non joignable' ? 'bg-muted-foreground' :
                'bg-emerald-500'
              }`} title={lead.status === 'pending' ? 'Jamais contacté' : lead.joignabilite === 'Non joignable' ? 'Non joignable' : 'Déjà contacté'} />
            </div>
            <div className="min-w-0">
              <p className="text-sm font-medium truncate">{lead.prenom} {lead.nom}</p>
              <p className="text-xs text-muted-foreground">{lead.civilite === 'M' ? 'Monsieur' : 'Madame'}</p>
            </div>
          </div>
        </TableCell>
        <TableCell>
          <span className="font-mono text-sm text-muted-foreground">{lead.telephone}</span>
        </TableCell>
        {showCharge && (
          <TableCell>
            <span className="text-xs text-muted-foreground">{lead.charge}</span>
          </TableCell>
        )}
        <TableCell>
          <Badge className={`${statusCfg.className} text-xs`}>{statusCfg.label}</Badge>
        </TableCell>
        <TableCell className="hidden sm:table-cell text-xs text-muted-foreground">
          {lead.lastAction}
        </TableCell>
        {!showCharge && (
          <TableCell className="hidden md:table-cell">
            {lead.joignabilite === 'Non joignable' ? (
              <span className="inline-flex items-center gap-1 text-xs text-destructive">
                <PhoneOff className="w-3 h-3" /> Non joignable
              </span>
            ) : (
              <span className="inline-flex items-center gap-1 text-xs text-emerald-600">
                <CircleDot className="w-3 h-3" /> Joignable
              </span>
            )}
          </TableCell>
        )}
        <TableCell>
          <div className="flex items-center gap-1 justify-end opacity-70 group-hover:opacity-100 transition-opacity">
            {!showCharge && (
              <Button
                size="sm"
                className="h-7 px-2.5 text-xs"
                onClick={() => handleCall(lead)}
              >
                <PhoneCall className="w-3 h-3 mr-1" /> {lead.status === 'pending' ? 'Appeler' : 'Rappeler'}
              </Button>
            )}
            <Button
              variant="ghost"
              size="sm"
              className="h-7 px-2 text-xs"
              onClick={() => { setSelectedLead(lead); setAutoCallOnOpen(false); setIsPopupOpen(true); }}
            >
              <Eye className="w-3 h-3 mr-1" /> Voir
            </Button>
          </div>
        </TableCell>
      </TableRow>
    );
  };

  const reste = campaign.leadsLivres - campaign.leadsTraites;

  return (
    <div className="space-y-5">
      {/* Header with back + campaign info */}
      <div className="flex items-center gap-3">
        <Button variant="ghost" size="icon" onClick={onBack} className="shrink-0">
          <ArrowLeft className="w-5 h-5" />
        </Button>
        <div className="flex-1 min-w-0">
          <div className="flex items-center gap-2 flex-wrap">
            <h2 className="text-lg sm:text-xl font-bold truncate">
              {campaign.name}
            </h2>
            <Badge variant="secondary" className="text-[10px] bg-emerald-500/10 text-emerald-600 border-emerald-500/30">
              Active
            </Badge>
          </div>
          <p className="text-xs text-muted-foreground font-mono">{campaign.id}</p>
        </div>
        <Button variant="outline" size="sm" className="shrink-0">
          <RefreshCw className="w-3.5 h-3.5 mr-1.5" /> Actualiser
        </Button>
      </div>

      {/* Compact stats bar — single row with key numbers + progress */}
      <Card>
        <CardContent className="p-4">
          <div className="flex flex-col sm:flex-row sm:items-center gap-4">
            {/* Key numbers inline */}
            <div className="flex items-center gap-5 flex-wrap flex-1">
              {[
                { label: 'Livrés', value: campaign.leadsLivres, icon: Users },
                { label: 'Traités', value: campaign.leadsTraites, icon: Phone },
                { label: 'Retenus', value: campaign.retenus, icon: UserCheck },
                { label: 'Intégrés', value: campaign.integres, icon: CheckCircle },
              ].map((item, idx) => (
                <div key={idx} className="flex items-center gap-2">
                  <item.icon className="w-4 h-4 text-primary" />
                  <div>
                    <span className="text-lg font-bold text-foreground">{item.value}</span>
                    <span className="text-xs text-muted-foreground ml-1">{item.label}</span>
                  </div>
                </div>
              ))}
              {reste > 0 && (
                <div className="flex items-center gap-1.5 text-xs text-muted-foreground">
                  <Clock className="w-3.5 h-3.5" />
                  <span>{reste} restant{reste > 1 ? 's' : ''}</span>
                </div>
              )}
            </div>
            {/* Progress */}
            <div className="sm:w-40 shrink-0">
              <div className="flex items-center justify-between text-xs mb-1">
                <span className="text-muted-foreground">Progression</span>
                <span className="font-semibold">{campaign.tauxTraitement}%</span>
              </div>
              <Progress value={campaign.tauxTraitement} className="h-2" />
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Status Filters — directly above the table */}
      <div className="flex flex-wrap items-center gap-1.5">
        {[
          { key: 'all', label: 'Tous', count: stats.total },
          { key: 'pending', label: 'À traiter', count: stats.pending },
          { key: 'contacted', label: 'Contactés', count: stats.contacted },
          { key: 'qualified', label: 'Qualifiés', count: stats.qualified },
          { key: 'integrated', label: 'Intégrés', count: stats.integrated },
          { key: 'rejected', label: 'Rejetés', count: stats.rejected },
        ].map(f => (
          <Button
            key={f.key}
            variant={statusFilter === f.key ? 'default' : 'outline'}
            size="sm"
            className="text-xs h-8"
            onClick={() => setStatusFilter(f.key)}
          >
            {f.label} ({f.count})
          </Button>
        ))}
      </div>

      {/* Leads Table */}
      <Card>
        <CardContent className="px-0 py-0">
          <div className="overflow-x-auto">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead className="text-xs">ID</TableHead>
                  <TableHead className="text-xs">Nom</TableHead>
                  <TableHead className="text-xs">Téléphone</TableHead>
                  <TableHead className="text-xs">Statut</TableHead>
                  <TableHead className="text-xs hidden sm:table-cell">Dernière action</TableHead>
                  <TableHead className="text-xs hidden md:table-cell">Joignabilité</TableHead>
                  <TableHead className="text-xs text-right">Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredLeads.map((lead) => renderLeadRow(lead))}
                {filteredLeads.length === 0 && (
                  <TableRow>
                    <TableCell colSpan={7} className="text-center py-8 text-muted-foreground">
                      Aucun lead pour ce filtre.
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>

      {/* Modals */}
      <LeadDetailPopup
        lead={selectedLead ? {
          id: selectedLead.id,
          nom: selectedLead.nom,
          prenom: selectedLead.prenom,
          email: `${selectedLead.prenom.toLowerCase()}.${selectedLead.nom.toLowerCase()}@email.com`,
          telephone: selectedLead.telephone,
          ville: 'Tunis',
          statut: selectedLead.status,
          score: Math.floor(Math.random() * 30) + 70,
          dateCreation: selectedLead.dateAppel,
          langues: ['Français', 'Arabe'],
          experience: '2 ans',
          activites: ['Service Client'],
          disponibilite: 'Immédiate',
          source: 'Campagne',
          campagne: campaign.name,
        } : null}
        isOpen={isPopupOpen}
        onClose={() => { stopRinging(); setIsPopupOpen(false); setAutoCallOnOpen(false); }}
        campaignLead={selectedLead}
        onCampaignLeadSave={handleLeadSave}
        autoCall={autoCallOnOpen}
      />
    </div>
  );
}
