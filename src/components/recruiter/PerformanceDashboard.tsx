import { useState } from 'react';
import { Badge } from '@/components/ui/badge';
import { CampaignDetailView } from './CampaignDetailView';
import { AdminTeamPerformance } from './AdminTeamPerformance';
import { ChargeView } from './performance/ChargeView';
import { AdminView } from './performance/AdminView';
import { mockCampaigns as campaigns } from '@/data/recruiterMockData';

interface PerformanceDashboardProps {
  userRole: 'super_admin' | 'admin' | 'charge';
  userName?: string;
}

export function PerformanceDashboard({ userRole, userName = 'Chargé' }: PerformanceDashboardProps) {
  const isChargeView = userRole === 'charge';
  const [selectedCampaign, setSelectedCampaign] = useState<typeof campaigns[0] | null>(null);
  const [showTeamPerformance, setShowTeamPerformance] = useState(false);

  const getStatusBadge = (statut: string) => {
    const map: Record<string, { label: string; cls: string }> = {
      a_traiter: { label: 'À traiter', cls: 'bg-amber-500/10 text-amber-600 border-amber-500/30' },
      entretien_programme: { label: 'Entretien', cls: 'bg-primary/10 text-primary border-primary/30' },
      retenu: { label: 'Retenu', cls: 'bg-violet-500/10 text-violet-600 border-violet-500/30' },
      integre: { label: 'Intégré', cls: 'bg-emerald-500/10 text-emerald-600 border-emerald-500/30' },
      non_joignable: { label: 'Non joignable', cls: 'bg-muted text-muted-foreground border-muted' },
      en_cours: { label: 'En cours', cls: 'bg-primary/10 text-primary border-primary/30' },
      terminee: { label: 'Terminée', cls: 'bg-emerald-500/10 text-emerald-600 border-emerald-500/30' },
    };
    const cfg = map[statut] || { label: statut, cls: '' };
    return <Badge className={`${cfg.cls} text-xs`}>{cfg.label}</Badge>;
  };

  if (showTeamPerformance && !isChargeView) {
    return <AdminTeamPerformance onBack={() => setShowTeamPerformance(false)} />;
  }

  if (selectedCampaign) {
    return <CampaignDetailView campaign={selectedCampaign} onBack={() => setSelectedCampaign(null)} />;
  }

  if (isChargeView) {
    return <ChargeView userName={userName} getStatusBadge={getStatusBadge} />;
  }

  return (
    <AdminView
      getStatusBadge={getStatusBadge}
      onShowTeamPerformance={() => setShowTeamPerformance(true)}
      onSelectCampaign={setSelectedCampaign}
    />
  );
}
