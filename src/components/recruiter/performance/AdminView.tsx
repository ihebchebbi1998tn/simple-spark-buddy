import { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Progress } from '@/components/ui/progress';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import {
  TrendingUp, Users, Phone, UserCheck, Target, Clock, CheckCircle, Calendar,
  BarChart3, ArrowUp, ArrowDown, Eye, Filter, FileSpreadsheet, Printer, Briefcase, Euro, Star, ChartLine
} from 'lucide-react';
import { PerformanceCharts } from '../PerformanceCharts';
import { LeadDetailPopup } from '../LeadDetailPopup';
import { downloadCSV, exportPerformanceData, exportTeamData, printReport } from '@/utils/exportUtils';
import { useToast } from '@/hooks/use-toast';
import {
  mockPerformanceMetrics as chargeMetrics,
  mockCampaigns as campaigns,
  mockRecentLeads as recentLeads,
  mockObjectives as objectives,
  mockWeeklyStats as weeklyStats,
  mockAdminOrders as adminOrders,
} from '@/data/recruiterMockData';

interface AdminViewProps {
  getStatusBadge: (statut: string) => React.ReactNode;
  onShowTeamPerformance: () => void;
  onSelectCampaign: (campaign: typeof campaigns[0]) => void;
}

export function AdminView({ getStatusBadge, onShowTeamPerformance, onSelectCampaign }: AdminViewProps) {
  const { toast } = useToast();
  const [periodFilter, setPeriodFilter] = useState('semaine');
  const [statusFilter, setStatusFilter] = useState('toutes');
  const [selectedLead, setSelectedLead] = useState<any>(null);
  const [isLeadPopupOpen, setIsLeadPopupOpen] = useState(false);

  const periodMultiplier: Record<string, number> = { jour: 0.15, semaine: 1, mois: 3.2, trimestre: 8.5 };
  const pm = periodMultiplier[periodFilter] ?? 1;
  const applyPeriod = (val: number) => Math.round(val * pm);

  const handleExportCSV = () => {
    const data = exportPerformanceData(periodFilter);
    downloadCSV(data, `performance-${periodFilter}-${new Date().toISOString().slice(0,10)}`);
    toast({ title: 'Export réussi', description: 'Le fichier CSV a été téléchargé.' });
  };

  const handlePrint = () => {
    const content = document.getElementById('performance-report');
    printReport(content, 'Rapport de Performance');
  };

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="bg-primary rounded-2xl p-6 text-primary-foreground">
        <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4">
          <div>
            <h1 className="text-xl sm:text-2xl font-bold mb-1">Tableau de bord principal</h1>
            <p className="text-primary-foreground/70 text-sm">Vue globale des performances opérationnelles</p>
          </div>
          <div className="flex flex-wrap gap-2">
            <Select value={periodFilter} onValueChange={setPeriodFilter}>
              <SelectTrigger className="w-[140px] h-9 bg-primary-foreground/10 border-primary-foreground/20 text-primary-foreground text-sm">
                <Calendar className="w-3.5 h-3.5 mr-2" /><SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="jour">Aujourd'hui</SelectItem>
                <SelectItem value="semaine">Cette semaine</SelectItem>
                <SelectItem value="mois">Ce mois</SelectItem>
                <SelectItem value="trimestre">Ce trimestre</SelectItem>
              </SelectContent>
            </Select>
            <Select value={statusFilter} onValueChange={setStatusFilter}>
              <SelectTrigger className="w-[130px] h-9 bg-primary-foreground/10 border-primary-foreground/20 text-primary-foreground text-sm">
                <Filter className="w-3.5 h-3.5 mr-2" /><SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="toutes">Toutes</SelectItem>
                <SelectItem value="en_cours">En cours</SelectItem>
                <SelectItem value="terminee">Terminées</SelectItem>
              </SelectContent>
            </Select>
            <Button variant="secondary" size="sm" className="h-9" onClick={handleExportCSV}>
              <FileSpreadsheet className="w-3.5 h-3.5 mr-1.5" /> Excel
            </Button>
            <Button variant="secondary" size="sm" className="h-9" onClick={handlePrint}>
              <Printer className="w-3.5 h-3.5 mr-1.5" /> Imprimer
            </Button>
          </div>
        </div>
      </div>

      <LeadDetailPopup lead={selectedLead} isOpen={isLeadPopupOpen} onClose={() => setIsLeadPopupOpen(false)} />

      <div id="performance-report">
        {/* Stats Overview */}
        <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-6 gap-3">
          {[
            { value: '76%', label: 'Conversion globale', icon: Target },
            { value: `${Math.round(18 / pm)}min`, label: 'Temps moyen', icon: Clock },
            { value: '94%', label: 'Satisfaction client', icon: Star },
            { value: applyPeriod(1248).toLocaleString(), label: 'Leads traités', icon: Users },
            { value: `€${applyPeriod(45620).toLocaleString()}`, label: "Chiffre d'affaires", icon: Euro },
            { value: String(applyPeriod(24)), label: 'Commandes actives', icon: Briefcase },
          ].map((s, idx) => (
            <Card key={idx} className="hover:shadow-md transition-shadow cursor-default">
              <CardContent className="p-3 text-center">
                <s.icon className="w-4 h-4 text-primary mx-auto mb-1.5" />
                <p className="text-lg font-semibold text-foreground">{s.value}</p>
                <p className="text-xs text-muted-foreground leading-snug mt-0.5">{s.label}</p>
              </CardContent>
            </Card>
          ))}
        </div>

        {/* KPI Cards with Trends */}
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
          {[
            { icon: Users, label: 'Leads traités', value: applyPeriod(chargeMetrics.leadsTraites.value), target: applyPeriod(chargeMetrics.leadsTraites.target), pct: chargeMetrics.leadsTraites.percentage, trend: '+12%', up: true },
            { icon: Phone, label: 'Entretiens', value: applyPeriod(chargeMetrics.entretiensRealises.value), target: applyPeriod(chargeMetrics.entretiensRealises.target), pct: chargeMetrics.entretiensRealises.percentage, trend: '+8%', up: true },
            { icon: UserCheck, label: 'Candidats retenus', value: applyPeriod(chargeMetrics.candidatsRetenus.value), target: applyPeriod(chargeMetrics.candidatsRetenus.target), pct: chargeMetrics.candidatsRetenus.percentage, trend: '-3%', up: false },
            { icon: CheckCircle, label: 'Intégrés', value: applyPeriod(chargeMetrics.candidatsIntegres.value), target: applyPeriod(chargeMetrics.candidatsIntegres.target), pct: chargeMetrics.candidatsIntegres.percentage, trend: '+15%', up: true },
          ].map((item, idx) => (
            <Card key={idx}>
              <CardContent className="p-4">
                <div className="flex items-center justify-between mb-3">
                  <div className="w-9 h-9 rounded-lg bg-primary/10 flex items-center justify-center">
                    <item.icon className="w-4 h-4 text-primary" />
                  </div>
                  <Badge variant="outline" className={`text-xs ${item.up ? 'text-emerald-600 border-emerald-200 bg-emerald-50' : 'text-destructive border-destructive/20 bg-destructive/10'}`}>
                    {item.up ? <ArrowUp className="w-3 h-3 mr-0.5" /> : <ArrowDown className="w-3 h-3 mr-0.5" />}
                    {item.trend}
                  </Badge>
                </div>
                <p className="text-2xl font-semibold text-foreground">{item.value}</p>
                <p className="text-sm text-muted-foreground mt-0.5">{item.label}</p>
                <Progress value={item.pct} className="mt-3 h-1" />
                <p className="text-xs text-muted-foreground mt-1.5">Objectif : {item.target}</p>
              </CardContent>
            </Card>
          ))}
        </div>

        {/* Active Campaigns */}
        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle className="text-base sm:text-lg">Campagnes actives</CardTitle>
            <Button variant="outline" size="sm">Voir tout</Button>
          </CardHeader>
          <CardContent>
            <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-4">
              {campaigns.map((campaign) => (
                <div key={campaign.id} className="p-4 rounded-xl border hover:border-primary/40 transition-all hover:shadow-sm group">
                  <div className="flex items-center justify-between mb-3">
                    <div>
                      <p className="font-semibold text-sm">{campaign.id}</p>
                      <p className="text-xs text-muted-foreground">{campaign.name}</p>
                    </div>
                    <Badge variant="secondary" className="text-xs">Active</Badge>
                  </div>
                  <div className="grid grid-cols-2 gap-2 text-center mb-3">
                    <div className="p-2.5 bg-muted/30 rounded-lg">
                      <p className="text-base font-semibold">{campaign.leadsTraites}/{campaign.leadsLivres}</p>
                      <p className="text-xs text-muted-foreground">Leads traités</p>
                    </div>
                    <div className="p-2.5 bg-muted/30 rounded-lg">
                      <p className="text-base font-semibold">{campaign.retenus}</p>
                      <p className="text-xs text-muted-foreground">Retenus</p>
                    </div>
                  </div>
                  <div className="space-y-1 text-xs mb-3">
                    <div className="flex justify-between"><span className="text-muted-foreground">Conversion retenus</span><span className="font-medium">{campaign.tauxConversionRetenus}%</span></div>
                    <div className="flex justify-between"><span className="text-muted-foreground">Intégrés</span><span className="font-medium">{campaign.integres}</span></div>
                    <div className="flex justify-between"><span className="text-muted-foreground">Taux intégrés</span><span className="font-medium">{campaign.tauxConversionIntegres}%</span></div>
                  </div>
                  <Progress value={campaign.tauxTraitement} className="h-1 mb-3" />
                  <Button size="sm" variant="outline" className="w-full group-hover:bg-primary group-hover:text-primary-foreground transition-colors"
                    onClick={() => onSelectCampaign(campaign)}
                  >
                    Continuer
                  </Button>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Secondary KPIs */}
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
          {[
            { icon: Target, value: `${chargeMetrics.tauxConversion}%`, label: 'Taux conversion' },
            { icon: Phone, value: `${chargeMetrics.tauxReponse}%`, label: 'Taux réponse' },
            { icon: Clock, value: chargeMetrics.tempsTraitementMoyen, label: 'Durée moyenne' },
            { icon: BarChart3, value: `${chargeMetrics.scoreQualite}%`, label: 'Score qualité' },
          ].map((item, idx) => (
            <Card key={idx}>
              <CardContent className="p-4 flex items-center gap-3">
                <div className="w-10 h-10 rounded-lg bg-primary/10 flex items-center justify-center shrink-0">
                  <item.icon className="w-5 h-5 text-primary" />
                </div>
                <div>
                  <p className="text-lg font-semibold text-foreground">{item.value}</p>
                  <p className="text-xs text-muted-foreground">{item.label}</p>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>

        {/* Team Performance CTA */}
        <Card className="bg-muted/30">
          <CardContent className="p-4 flex flex-col sm:flex-row items-center justify-between gap-3">
            <div className="flex items-center gap-3">
              <div className="w-10 h-10 rounded-lg bg-primary/10 flex items-center justify-center shrink-0">
                <ChartLine className="w-5 h-5 text-primary" />
              </div>
              <div>
                <p className="font-medium text-sm">Performance équipe</p>
                <p className="text-xs text-muted-foreground">Vue détaillée par chargé, analytics et rapports</p>
              </div>
            </div>
            <Button size="sm" onClick={onShowTeamPerformance}>
              <Eye className="w-3.5 h-3.5 mr-1.5" /> Voir les détails
            </Button>
          </CardContent>
        </Card>

        {/* Orders Grid */}
        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle className="text-base sm:text-lg">Gestion des commandes</CardTitle>
            <div className="flex gap-2">
              <Button variant="outline" size="sm"><FileSpreadsheet className="w-3.5 h-3.5 mr-1.5" /> Exporter</Button>
              <Button size="sm">+ Nouvelle</Button>
            </div>
          </CardHeader>
          <CardContent>
            <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-4">
              {adminOrders.map((order) => {
                const pct = Math.round((order.leadsLivres / order.leadsTotal) * 100);
                return (
                  <div key={order.id} className="p-4 rounded-xl border hover:border-primary/40 transition-all hover:shadow-sm cursor-pointer">
                    <div className="flex items-center justify-between mb-3">
                      <div>
                        <p className="font-semibold text-sm">{order.id}</p>
                        <p className="text-xs text-muted-foreground">{order.date}</p>
                      </div>
                      {getStatusBadge(order.status)}
                    </div>
                    <div className="grid grid-cols-2 gap-2 text-center mb-3">
                      <div className="p-2.5 bg-muted/30 rounded-lg">
                        <p className="text-base font-semibold">{order.leadsLivres}/{order.leadsTotal}</p>
                        <p className="text-xs text-muted-foreground">Leads traités</p>
                      </div>
                      <div className="p-2.5 bg-muted/30 rounded-lg">
                        <p className="text-base font-semibold">{order.tauxConversion}%</p>
                        <p className="text-xs text-muted-foreground">Conversion</p>
                      </div>
                    </div>
                    <Progress value={pct} className="h-1 mb-1.5" />
                    <p className="text-xs text-muted-foreground text-right">{pct}%</p>
                  </div>
                );
              })}
            </div>
          </CardContent>
        </Card>

        {/* Objectives & Weekly */}
        <div className="grid lg:grid-cols-2 gap-4">
          <Card>
            <CardHeader>
              <CardTitle className="text-base sm:text-lg flex items-center gap-2">
                <Target className="w-4 h-4 text-primary" /> Objectifs du mois
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="space-y-4">
                {objectives.map((obj, index) => (
                  <div key={index}>
                    <div className="flex items-center justify-between text-sm mb-1.5">
                      <span className="font-medium">{obj.label}</span>
                      <span className="text-muted-foreground text-xs tabular-nums">{obj.current}/{obj.target} ({Math.round((obj.current / obj.target) * 100)}%)</span>
                    </div>
                    <div className="h-1.5 bg-muted rounded-full overflow-hidden">
                      <div className={`h-full rounded-full ${obj.color}`} style={{ width: `${Math.min((obj.current / obj.target) * 100, 100)}%` }} />
                    </div>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>

          <Card>
            <CardHeader><CardTitle className="text-base sm:text-lg">Activité de la semaine</CardTitle></CardHeader>
            <CardContent>
              <div className="grid grid-cols-5 gap-2">
                {weeklyStats.map((day, index) => (
                  <div key={index} className="text-center p-2 bg-muted/30 rounded-lg">
                    <p className="text-xs font-medium text-muted-foreground mb-2">{day.jour}</p>
                    <div className="h-12 w-full bg-muted rounded relative overflow-hidden mb-1">
                      <div className="absolute bottom-0 w-full bg-primary rounded-t" style={{ height: `${(day.leads / 40) * 100}%` }} />
                    </div>
                    <p className="text-sm font-medium tabular-nums">{day.leads}</p>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>
        </div>

        {/* Recent Leads */}
        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle className="text-base sm:text-lg">Derniers leads traités</CardTitle>
            <Button variant="outline" size="sm"><Eye className="w-3.5 h-3.5 sm:mr-1.5" /><span className="hidden sm:inline">Voir tout</span></Button>
          </CardHeader>
          <CardContent>
            <div className="space-y-1.5">
              {recentLeads.map((lead) => (
                <div key={lead.id} className="flex items-center justify-between p-3 rounded-lg hover:bg-muted/40 transition-colors gap-3 cursor-pointer"
                  onClick={() => { setSelectedLead(lead); setIsLeadPopupOpen(true); }}
                >
                  <div className="flex items-center gap-3 min-w-0 flex-1">
                    <div className="w-9 h-9 rounded-full bg-primary/10 flex items-center justify-center text-sm font-semibold text-primary shrink-0">
                      {lead.nom.split(' ').map(n => n[0]).join('')}
                    </div>
                    <div className="min-w-0">
                      <p className="font-medium text-sm truncate">{lead.nom}</p>
                      <p className="text-xs text-muted-foreground">{lead.id} · {lead.date}</p>
                    </div>
                  </div>
                  <div className="flex items-center gap-2 shrink-0">
                    <span className="hidden sm:inline text-xs text-muted-foreground tabular-nums">{lead.score}%</span>
                    {getStatusBadge(lead.statut)}
                    <Button variant="ghost" size="icon" className="h-8 w-8"><Eye className="w-4 h-4" /></Button>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        <PerformanceCharts isChargeView={false} />
      </div>
    </div>
  );
}
