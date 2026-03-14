import { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Progress } from '@/components/ui/progress';
import { CampaignDetailView } from './CampaignDetailView';
import { LeadRedistributionModal } from './LeadRedistributionModal';
import { 
  Eye, Megaphone, Users, UserCheck, CheckCircle, Clock, 
  ArrowRight, TrendingUp, FileSpreadsheet, ArrowRightLeft, RotateCcw
} from 'lucide-react';
import { mockCampaigns as campaigns } from '@/data/recruiterMockData';
import { downloadCSV, exportPerformanceData } from '@/utils/exportUtils';
import { useToast } from '@/hooks/use-toast';
import { recruiterAuthService } from '@/services/recruiterAuthService';

export function ChargeCampaigns() {
  const { toast } = useToast();
  const [selectedCampaign, setSelectedCampaign] = useState<typeof campaigns[0] | null>(null);
  const [redistributeCampaign, setRedistributeCampaign] = useState<typeof campaigns[0] | null>(null);
  const user = recruiterAuthService.getUser();
  const role = user?.role ?? 'charge';
  const canRedistribute = role === 'admin' || role === 'super_admin';

  const handleExportCSV = () => {
    const data = exportPerformanceData('semaine');
    downloadCSV(data, `campagnes-${new Date().toISOString().slice(0, 10)}`);
    toast({ title: 'Export réussi', description: 'Le fichier CSV a été téléchargé.' });
  };

  if (selectedCampaign) {
    return (
      <CampaignDetailView
        campaign={selectedCampaign}
        onBack={() => setSelectedCampaign(null)}
      />
    );
  }

  // Aggregate totals
  const totals = campaigns.reduce((acc, c) => ({
    livres: acc.livres + c.leadsLivres,
    traites: acc.traites + c.leadsTraites,
    retenus: acc.retenus + c.retenus,
    integres: acc.integres + c.integres,
  }), { livres: 0, traites: 0, retenus: 0, integres: 0 });

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="bg-emerald-600 rounded-2xl p-6 text-white">
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div className="flex items-center gap-3">
            <Megaphone className="w-6 h-6" />
            <div>
              <h1 className="text-xl sm:text-2xl font-bold">Campagnes en cours</h1>
              <p className="text-white/70 text-sm">{campaigns.length} campagne{campaigns.length > 1 ? 's' : ''} active{campaigns.length > 1 ? 's' : ''}</p>
            </div>
          </div>
          <Button variant="secondary" size="sm" onClick={handleExportCSV}>
            <FileSpreadsheet className="w-3.5 h-3.5 mr-1.5" /> Exporter
          </Button>
        </div>
      </div>

      {/* Global summary — 4 key metrics */}
      <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
        {[
          { icon: Users, label: 'Leads livrés', value: totals.livres, color: 'text-primary' },
          { icon: TrendingUp, label: 'Traités', value: totals.traites, sub: `${Math.round((totals.traites / totals.livres) * 100)}%`, color: 'text-primary' },
          { icon: UserCheck, label: 'Retenus', value: totals.retenus, sub: `${Math.round((totals.retenus / totals.traites) * 100)}%`, color: 'text-violet-600' },
          { icon: CheckCircle, label: 'Intégrés', value: totals.integres, sub: `${Math.round((totals.integres / totals.retenus) * 100)}%`, color: 'text-emerald-600' },
        ].map((item, idx) => (
          <Card key={idx}>
            <CardContent className="p-4">
              <div className="flex items-center gap-3">
                <div className="w-10 h-10 rounded-xl bg-primary/10 flex items-center justify-center shrink-0">
                  <item.icon className={`w-5 h-5 ${item.color}`} />
                </div>
                <div>
                  <p className="text-xl font-bold text-foreground">{item.value}</p>
                  <p className="text-xs text-muted-foreground">{item.label}</p>
                </div>
              </div>
              {item.sub && (
                <p className="text-[11px] text-muted-foreground mt-1.5 pl-[52px]">Taux: {item.sub}</p>
              )}
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Campaign cards — one per campaign, clean layout */}
      <div className="space-y-4">
        {campaigns.map((campaign) => {
          const reste = campaign.leadsLivres - campaign.leadsTraites;
          return (
            <Card key={campaign.id} className="overflow-hidden hover:shadow-md transition-shadow">
              <CardContent className="p-0">
                <div className="flex flex-col lg:flex-row">
                  {/* Left: Campaign info */}
                  <div className="flex-1 p-5 lg:p-6">
                    <div className="flex items-start justify-between mb-4">
                      <div>
                        <div className="flex items-center gap-2 mb-1">
                          <h3 className="text-base font-semibold text-foreground">{campaign.name}</h3>
                          <Badge variant="secondary" className="text-[10px] bg-emerald-500/10 text-emerald-600 border-emerald-500/30">
                            Active
                          </Badge>
                        </div>
                        <p className="text-xs text-muted-foreground font-mono">{campaign.id}</p>
                      </div>
                    </div>

                    {/* Funnel row */}
                    <div className="flex items-center gap-1.5 sm:gap-3 mb-4">
                      {[
                        { label: 'Livrés', value: campaign.leadsLivres },
                        { label: 'Traités', value: campaign.leadsTraites },
                        { label: 'Retenus', value: campaign.retenus },
                        { label: 'Intégrés', value: campaign.integres },
                      ].map((step, idx, arr) => (
                        <div key={idx} className="flex items-center gap-1.5 sm:gap-3 flex-1">
                          <div className="flex-1 text-center p-2 sm:p-3 bg-muted/30 rounded-lg">
                            <p className="text-lg sm:text-xl font-bold text-foreground">{step.value}</p>
                            <p className="text-[10px] sm:text-xs text-muted-foreground">{step.label}</p>
                          </div>
                          {idx < arr.length - 1 && (
                            <ArrowRight className="w-3.5 h-3.5 text-muted-foreground/30 shrink-0" />
                          )}
                        </div>
                      ))}
                    </div>

                    {/* Progress bar */}
                    <div className="mb-1">
                      <div className="flex items-center justify-between text-xs mb-1.5">
                        <span className="text-muted-foreground">Progression</span>
                        <span className="font-medium text-foreground">{campaign.tauxTraitement}%</span>
                      </div>
                      <Progress value={campaign.tauxTraitement} className="h-2" />
                    </div>
                    {reste > 0 && (
                      <p className="text-[11px] text-muted-foreground flex items-center gap-1 mt-1">
                        <Clock className="w-3 h-3" /> {reste} lead{reste > 1 ? 's' : ''} restant{reste > 1 ? 's' : ''}
                      </p>
                    )}
                  </div>

                  {/* Right: Conversion rates + CTA */}
                  <div className="lg:w-56 border-t lg:border-t-0 lg:border-l border-border bg-muted/20 p-5 lg:p-6 flex flex-col justify-between gap-4">
                    <div className="space-y-3">
                      {[
                        { label: 'Conv. retenus', value: campaign.tauxConversionRetenus },
                        { label: 'Conv. intégrés', value: campaign.tauxConversionIntegres },
                      ].map((rate, idx) => (
                        <div key={idx}>
                          <div className="flex items-center justify-between text-xs mb-1">
                            <span className="text-muted-foreground">{rate.label}</span>
                            <span className="font-semibold text-foreground">{rate.value}%</span>
                          </div>
                          <Progress value={rate.value} className="h-1" />
                        </div>
                      ))}
                    </div>
                    <div className="flex flex-col gap-2">
                      <Button
                        className="w-full bg-primary hover:bg-primary/90 text-primary-foreground"
                        onClick={() => setSelectedCampaign(campaign)}
                      >
                        <Eye className="w-4 h-4 mr-2" /> Continuer
                      </Button>
                      {canRedistribute && (
                        <Button
                          variant="outline"
                          size="sm"
                          className="w-full gap-1.5 border-primary/30 text-primary hover:bg-primary/5 text-xs"
                          onClick={() => setRedistributeCampaign(campaign)}
                        >
                          <ArrowRightLeft className="w-3.5 h-3.5" /> Redistribuer
                        </Button>
                      )}
                    </div>
                  </div>
                </div>
              </CardContent>
            </Card>
          );
        })}
      </div>

      {/* Redistribution modal */}
      {redistributeCampaign && (
        <LeadRedistributionModal
          isOpen={!!redistributeCampaign}
          onClose={() => setRedistributeCampaign(null)}
          orderId={redistributeCampaign.id}
          totalLeads={redistributeCampaign.leadsLivres}
        />
      )}
    </div>
  );
}
