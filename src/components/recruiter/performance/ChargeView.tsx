import { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Progress } from '@/components/ui/progress';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { TrendingUp, Users, UserCheck, Target, Clock, CheckCircle, Calendar, Eye, Trophy, Medal, FileSpreadsheet, ArrowRight } from 'lucide-react';
import { PerformanceCharts } from '../PerformanceCharts';
import { LeadDetailPopup } from '../LeadDetailPopup';
import { downloadCSV, exportPerformanceData } from '@/utils/exportUtils';
import { useToast } from '@/hooks/use-toast';
import {
  mockPerformanceMetrics as chargeMetrics,
  mockTeamRanking as teamRanking,
  mockRecentLeads as recentLeads,
  mockObjectives as objectives,
  mockWeeklyStats as weeklyStats,
} from '@/data/recruiterMockData';

interface ChargeViewProps {
  userName: string;
  getStatusBadge: (statut: string) => React.ReactNode;
}

export function ChargeView({ userName, getStatusBadge }: ChargeViewProps) {
  const { toast } = useToast();
  const [periodFilter, setPeriodFilter] = useState('semaine');
  const [selectedLead, setSelectedLead] = useState<any>(null);
  const [isLeadPopupOpen, setIsLeadPopupOpen] = useState(false);

  const periodMultiplier: Record<string, number> = { jour: 0.15, semaine: 1, mois: 3.2, trimestre: 8.5 };
  const pm = periodMultiplier[periodFilter] ?? 1;
  const applyPeriod = (val: number) => Math.round(val * pm);
  const periodLabel: Record<string, string> = { jour: "aujourd'hui", semaine: 'cette semaine', mois: 'ce mois', trimestre: 'ce trimestre' };

  const handleExportCSV = () => {
    const data = exportPerformanceData(periodFilter);
    downloadCSV(data, `performance-${periodFilter}-${new Date().toISOString().slice(0,10)}`);
    toast({ title: 'Export réussi', description: 'Le fichier CSV a été téléchargé.' });
  };

  const leadsTraites = applyPeriod(Number(chargeMetrics.leadsTraites.value));
  const retenus = applyPeriod(Number(chargeMetrics.candidatsRetenus.value));
  const integres = applyPeriod(Number(chargeMetrics.candidatsIntegres.value));
  const leadsTotal = applyPeriod(Number(chargeMetrics.leadsTraites.target));
  const resteATraiter = leadsTotal - leadsTraites;

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="bg-primary rounded-2xl p-6 text-primary-foreground">
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div>
            <h1 className="text-xl sm:text-2xl font-bold mb-1">Bonjour {userName} 👋</h1>
            <p className="text-primary-foreground/70 text-sm">Voici le résumé de votre activité {periodLabel[periodFilter]}</p>
          </div>
          <div className="flex gap-2">
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
            <Button variant="secondary" size="sm" className="h-9" onClick={handleExportCSV}>
              <FileSpreadsheet className="w-3.5 h-3.5 mr-1.5" /> Export
            </Button>
          </div>
        </div>
      </div>

      <LeadDetailPopup lead={selectedLead} isOpen={isLeadPopupOpen} onClose={() => setIsLeadPopupOpen(false)} />

      <div id="performance-report">
        {/* 4 Key KPIs */}
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
          {[
            { icon: Users, label: 'Leads traités', value: `${leadsTraites}/${leadsTotal}`, sub: `${resteATraiter} restants`, color: 'text-primary' },
            { icon: UserCheck, label: 'Retenus', value: `${retenus}`, sub: `${Math.round((retenus / leadsTraites) * 100)}% de conversion`, color: 'text-violet-600' },
            { icon: CheckCircle, label: 'Intégrés', value: `${integres}`, sub: `${chargeMetrics.tauxConversion}% taux final`, color: 'text-emerald-600' },
            { icon: Clock, label: 'Durée moyenne', value: chargeMetrics.tempsTraitementMoyen, sub: `Qualité: ${chargeMetrics.scoreQualite}%`, color: 'text-primary' },
          ].map((item, idx) => (
            <Card key={idx} className="hover:shadow-md transition-shadow">
              <CardContent className="p-4">
                <div className="flex items-center gap-3">
                  <div className="w-10 h-10 rounded-xl bg-primary/10 flex items-center justify-center shrink-0">
                    <item.icon className={`w-5 h-5 ${item.color}`} />
                  </div>
                  <div className="min-w-0">
                    <p className="text-xl font-bold text-foreground">{item.value}</p>
                    <p className="text-xs text-muted-foreground">{item.label}</p>
                  </div>
                </div>
                <p className="text-[11px] text-muted-foreground mt-2 pl-[52px]">{item.sub}</p>
              </CardContent>
            </Card>
          ))}
        </div>

        {/* Conversion Funnel */}
        <Card>
          <CardHeader>
            <CardTitle className="text-base sm:text-lg flex items-center gap-2">
              <TrendingUp className="w-4 h-4 text-primary" /> Entonnoir de conversion
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center justify-between gap-2">
              {[
                { label: 'Leads livrés', value: leadsTotal, pct: 100, bg: 'bg-primary/10', text: 'text-primary' },
                { label: 'Traités', value: leadsTraites, pct: Math.round((leadsTraites / leadsTotal) * 100), bg: 'bg-primary/15', text: 'text-primary' },
                { label: 'Retenus', value: retenus, pct: Math.round((retenus / leadsTotal) * 100), bg: 'bg-violet-500/10', text: 'text-violet-600' },
                { label: 'Intégrés', value: integres, pct: Math.round((integres / leadsTotal) * 100), bg: 'bg-emerald-500/10', text: 'text-emerald-600' },
              ].map((step, idx, arr) => (
                <div key={idx} className="flex items-center gap-2 flex-1">
                  <div className={`flex-1 ${step.bg} rounded-xl p-4 text-center`}>
                    <p className={`text-2xl font-bold ${step.text}`}>{step.value}</p>
                    <p className="text-xs text-muted-foreground mt-1">{step.label}</p>
                    <p className="text-[11px] font-medium text-muted-foreground/70 mt-0.5">{step.pct}%</p>
                  </div>
                  {idx < arr.length - 1 && <ArrowRight className="w-4 h-4 text-muted-foreground/40 shrink-0" />}
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Objectives + Ranking */}
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
            <CardHeader>
              <CardTitle className="text-base sm:text-lg flex items-center gap-2">
                <Trophy className="w-4 h-4 text-amber-500" /> Classement équipe
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="flex items-center gap-4 mb-4 p-3 bg-amber-500/5 rounded-xl border border-amber-500/10">
                <div className="w-12 h-12 rounded-full bg-amber-500/10 flex items-center justify-center">
                  <Medal className="w-6 h-6 text-amber-500" />
                </div>
                <div>
                  <p className="text-lg font-bold">2ème <span className="text-sm font-normal text-muted-foreground">/ 8 chargés</span></p>
                  <p className="text-xs text-muted-foreground">+15% au dessus de la moyenne</p>
                </div>
              </div>
              <div className="space-y-1">
                {teamRanking.slice(0, 5).map((member) => (
                  <div key={member.rank} className={`flex items-center gap-3 p-2.5 rounded-lg transition-colors ${member.name === 'Wael Wael' ? 'bg-primary/5 ring-1 ring-primary/20' : 'hover:bg-muted/40'}`}>
                    <div className={`w-6 h-6 rounded-full flex items-center justify-center text-xs font-semibold ${member.rank === 1 ? 'bg-amber-500 text-white' : member.rank === 2 ? 'bg-muted-foreground/60 text-white' : member.rank === 3 ? 'bg-amber-700 text-white' : 'bg-muted text-muted-foreground'}`}>{member.rank}</div>
                    <div className="flex-1 min-w-0"><p className="text-sm font-medium truncate">{member.name}</p></div>
                    <span className="text-xs text-muted-foreground">{member.tauxConversion}%</span>
                    <span className="text-sm font-semibold tabular-nums w-8 text-right">{member.score}</span>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>
        </div>

        {/* Weekly Activity */}
        <Card>
          <CardHeader><CardTitle className="text-base sm:text-lg">Activité de la semaine</CardTitle></CardHeader>
          <CardContent>
            <div className="grid grid-cols-5 gap-3">
              {weeklyStats.map((day, index) => (
                <div key={index} className="text-center p-3 bg-muted/30 rounded-xl">
                  <p className="text-xs font-medium text-muted-foreground mb-2">{day.jour}</p>
                  <div className="h-16 w-full bg-muted rounded-lg relative overflow-hidden mb-1.5">
                    <div className="absolute bottom-0 w-full bg-primary rounded-t-lg transition-all" style={{ height: `${(day.leads / 40) * 100}%` }} />
                  </div>
                  <p className="text-sm font-semibold tabular-nums">{day.leads}</p>
                  <p className="text-[10px] text-muted-foreground">leads</p>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        {/* Recent Leads */}
        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle className="text-base sm:text-lg">Derniers leads traités</CardTitle>
            <Button variant="outline" size="sm"><Eye className="w-3.5 h-3.5 sm:mr-1.5" /><span className="hidden sm:inline">Voir tout</span></Button>
          </CardHeader>
          <CardContent>
            <div className="space-y-1.5">
              {recentLeads.slice(0, 5).map((lead) => (
                <div key={lead.id} className="flex items-center justify-between p-3 rounded-lg hover:bg-muted/40 transition-colors gap-3 cursor-pointer" onClick={() => { setSelectedLead(lead); setIsLeadPopupOpen(true); }}>
                  <div className="flex items-center gap-3 min-w-0 flex-1">
                    <div className="w-9 h-9 rounded-full bg-primary/10 flex items-center justify-center text-sm font-semibold text-primary shrink-0">
                      {lead.nom.split(' ').map(n => n[0]).join('')}
                    </div>
                    <div className="min-w-0">
                      <p className="font-medium text-sm truncate">{lead.nom}</p>
                      <p className="text-xs text-muted-foreground">{lead.id} · {lead.date}</p>
                    </div>
                  </div>
                  <div className="flex items-center gap-2 shrink-0">{getStatusBadge(lead.statut)}</div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
