import { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Progress } from '@/components/ui/progress';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import {
  Users, TrendingUp, Clock, Star, Eye, Download, FileText, Settings,
  ArrowLeft, BarChart3, Activity, Euro, Zap, ThumbsUp, PhoneOff, Award
} from 'lucide-react';
import { PerformanceCharts } from './PerformanceCharts';
import { downloadCSV, exportTeamData } from '@/utils/exportUtils';
import { useToast } from '@/hooks/use-toast';

interface AdminTeamPerformanceProps {
  onBack: () => void;
}

const teamMembers = [
  { name: 'Dupont Martin', conversion: 82, leads: 285, satisfaction: 95, avgTime: '14min', status: 'Excellent' },
  { name: 'Bernard Sophie', conversion: 78, leads: 265, satisfaction: 91, avgTime: '16min', status: 'Bon' },
  { name: 'Moreau Thomas', conversion: 75, leads: 240, satisfaction: 88, avgTime: '18min', status: 'Moyen' },
  { name: 'Petit Julie', conversion: 80, leads: 270, satisfaction: 93, avgTime: '15min', status: 'Bon' },
  { name: 'Garcia Carlos', conversion: 71, leads: 210, satisfaction: 85, avgTime: '20min', status: 'À améliorer' },
];

const reports = [
  { id: 'RAPP-QUOTIDIEN', label: 'Rapport Quotidien', freq: 'Quotidien', format: 'PDF', schedule: '08:00', active: true },
  { id: 'RAPP-HEBDOMADAIRE', label: 'Rapport Hebdomadaire', freq: 'Hebdomadaire', format: 'Excel', schedule: 'Lundi', active: true },
  { id: 'RAPP-MENSUEL', label: 'Rapport Mensuel', freq: 'Mensuel', format: 'PDF + Excel', schedule: '1er du mois', active: false },
];

export function AdminTeamPerformance({ onBack }: AdminTeamPerformanceProps) {
  const { toast } = useToast();
  const [selectedMember, setSelectedMember] = useState<string | null>(null);

  const getStatusBadge = (status: string) => {
    const map: Record<string, string> = {
      Excellent: 'bg-emerald-500/10 text-emerald-600 border-emerald-500/30',
      Bon: 'bg-primary/10 text-primary border-primary/30',
      Moyen: 'bg-amber-500/10 text-amber-600 border-amber-500/30',
    };
    return <Badge className={`${map[status] || 'bg-destructive/10 text-destructive border-destructive/30'} text-xs`}>{status}</Badge>;
  };

  const handleExport = () => {
    const data = exportTeamData();
    downloadCSV(data, `equipe-performance-${new Date().toISOString().slice(0, 10)}`);
    toast({ title: 'Export réussi', description: 'Données équipe exportées.' });
  };

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="bg-primary rounded-2xl p-6 text-primary-foreground">
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div className="flex items-center gap-3">
            <Button variant="secondary" size="icon" className="h-9 w-9 shrink-0" onClick={onBack}>
              <ArrowLeft className="w-4 h-4" />
            </Button>
            <div>
              <h1 className="text-xl sm:text-2xl font-bold">Performance équipe</h1>
              <p className="text-primary-foreground/70 text-sm">Analyses des performances individuelles et collectives</p>
            </div>
          </div>
          <Button variant="secondary" size="sm" onClick={handleExport}>
            <Download className="w-3.5 h-3.5 mr-1.5" /> Exporter
          </Button>
        </div>
      </div>

      <Tabs defaultValue="globale" className="space-y-5">
        <TabsList className="w-full justify-start bg-muted/50 p-1">
          <TabsTrigger value="globale" className="text-sm">Vue globale</TabsTrigger>
          <TabsTrigger value="par-charge" className="text-sm">Par chargé</TabsTrigger>
          <TabsTrigger value="analytics" className="text-sm">Analytics</TabsTrigger>
          <TabsTrigger value="rapports" className="text-sm">Rapports</TabsTrigger>
        </TabsList>

        {/* Vue Globale */}
        <TabsContent value="globale" className="space-y-5">
          <div className="grid sm:grid-cols-3 gap-4">
            {[
              { icon: TrendingUp, title: 'Performance équipe', value: '78%', target: 'Objectif : 80%', pct: 78, detail: 'Taux conversion moyen' },
              { icon: Activity, title: 'Productivité', value: '42', target: '45 leads/jour', pct: 93, detail: 'Leads traités en moyenne' },
              { icon: Star, title: 'Satisfaction client', value: '92%', target: 'Objectif : 90%', pct: 92, detail: 'Score moyen équipe' },
            ].map((m, i) => (
              <Card key={i}>
                <CardContent className="p-4">
                  <div className="flex items-center justify-between mb-3">
                    <div className="flex items-center gap-2">
                      <div className="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center">
                        <m.icon className="w-4 h-4 text-primary" />
                      </div>
                      <span className="text-sm font-medium">{m.title}</span>
                    </div>
                    <Badge variant="secondary" className="text-xs">{m.target}</Badge>
                  </div>
                  <p className="text-2xl font-semibold text-foreground mb-0.5">{m.value}</p>
                  <p className="text-xs text-muted-foreground mb-3">{m.detail}</p>
                  <Progress value={m.pct} className="h-1.5" />
                </CardContent>
              </Card>
            ))}
          </div>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between">
              <CardTitle className="text-base sm:text-lg">Résumé mensuel</CardTitle>
              <Button variant="outline" size="sm" onClick={() => toast({ title: 'Rapport généré' })}>
                <FileText className="w-3.5 h-3.5 mr-1.5" /> Rapport PDF
              </Button>
            </CardHeader>
            <CardContent>
              <div className="grid grid-cols-2 lg:grid-cols-4 gap-3">
                {[
                  { value: '8', label: 'Chargés actifs', icon: Users },
                  { value: '1 856', label: 'Leads traités', icon: BarChart3 },
                  { value: '€68 450', label: 'CA généré', icon: Euro },
                  { value: '16min', label: 'Temps moyen', icon: Clock },
                ].map((s, i) => (
                  <div key={i} className="p-4 bg-muted/30 rounded-xl text-center">
                    <s.icon className="w-4 h-4 text-primary mx-auto mb-1.5" />
                    <p className="text-xl font-semibold">{s.value}</p>
                    <p className="text-xs text-muted-foreground mt-0.5">{s.label}</p>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>

          <PerformanceCharts isChargeView={false} />
        </TabsContent>

        {/* Par Chargé */}
        <TabsContent value="par-charge" className="space-y-5">
          <Card>
            <CardHeader className="flex flex-row items-center justify-between">
              <CardTitle className="text-base sm:text-lg">Performances individuelles</CardTitle>
              <Button variant="outline" size="sm" onClick={handleExport}>
                <Download className="w-3.5 h-3.5 mr-1.5" /> Exporter
              </Button>
            </CardHeader>
            <CardContent>
              <div className="overflow-x-auto -mx-6">
                <div className="px-6">
                  <Table>
                    <TableHeader>
                      <TableRow>
                        <TableHead>Chargé</TableHead>
                        <TableHead>Conversion</TableHead>
                        <TableHead className="hidden sm:table-cell">Leads</TableHead>
                        <TableHead className="hidden md:table-cell">Satisfaction</TableHead>
                        <TableHead className="hidden md:table-cell">Temps moy.</TableHead>
                        <TableHead>Statut</TableHead>
                        <TableHead className="w-12"></TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {teamMembers.map((m) => (
                        <TableRow key={m.name} className="cursor-pointer" onClick={() => setSelectedMember(m.name)}>
                          <TableCell>
                            <div className="flex items-center gap-2.5">
                              <div className="w-8 h-8 rounded-full bg-primary/10 flex items-center justify-center text-xs font-semibold text-primary shrink-0">
                                {m.name.split(' ').map(n => n[0]).join('')}
                              </div>
                              <span className="text-sm font-medium">{m.name}</span>
                            </div>
                          </TableCell>
                          <TableCell>
                            <div className="flex items-center gap-2">
                              <span className="text-sm font-medium tabular-nums">{m.conversion}%</span>
                              <div className="w-12 h-1.5 bg-muted rounded-full overflow-hidden hidden sm:block">
                                <div className="h-full bg-primary rounded-full" style={{ width: `${m.conversion}%` }} />
                              </div>
                            </div>
                          </TableCell>
                          <TableCell className="hidden sm:table-cell tabular-nums">{m.leads}</TableCell>
                          <TableCell className="hidden md:table-cell tabular-nums">{m.satisfaction}%</TableCell>
                          <TableCell className="hidden md:table-cell">{m.avgTime}</TableCell>
                          <TableCell>{getStatusBadge(m.status)}</TableCell>
                          <TableCell>
                            <Button variant="ghost" size="icon" className="h-8 w-8">
                              <Eye className="w-4 h-4" />
                            </Button>
                          </TableCell>
                        </TableRow>
                      ))}
                    </TableBody>
                  </Table>
                </div>
              </div>
            </CardContent>
          </Card>

          {selectedMember && (() => {
            const m = teamMembers.find(t => t.name === selectedMember)!;
            return (
              <Card className="ring-1 ring-primary/20">
                <CardHeader className="flex flex-row items-center justify-between">
                  <CardTitle className="text-base flex items-center gap-2">
                    <Award className="w-4 h-4 text-primary" />
                    {selectedMember}
                  </CardTitle>
                  <Button variant="ghost" size="sm" onClick={() => setSelectedMember(null)}>Fermer</Button>
                </CardHeader>
                <CardContent>
                  <div className="grid grid-cols-2 lg:grid-cols-5 gap-3">
                    {[
                      { label: 'Conversion', value: `${m.conversion}%` },
                      { label: 'Leads traités', value: `${m.leads}` },
                      { label: 'Satisfaction', value: `${m.satisfaction}%` },
                      { label: 'Temps moyen', value: m.avgTime },
                      { label: 'Statut', value: m.status },
                    ].map((d, i) => (
                      <div key={i} className="p-3 bg-muted/30 rounded-lg text-center">
                        <p className="text-lg font-semibold">{d.value}</p>
                        <p className="text-xs text-muted-foreground">{d.label}</p>
                      </div>
                    ))}
                  </div>
                </CardContent>
              </Card>
            );
          })()}
        </TabsContent>

        {/* Analytics */}
        <TabsContent value="analytics" className="space-y-5">
          <div className="grid sm:grid-cols-3 gap-4">
            {[
              { title: 'Tendance conversion', value: '78%', badge: '+5% vs mois dernier', detail: 'Évolution positive' },
              { title: 'Efficacité temporelle', value: '16min', badge: '-2min vs objectif', detail: 'Amélioration continue' },
              { title: 'Rentabilité', value: '€68 450', badge: '+12% CA', detail: 'Croissance mensuelle' },
            ].map((m, i) => (
              <Card key={i}>
                <CardContent className="p-4">
                  <div className="flex items-center justify-between mb-2">
                    <span className="text-sm font-medium">{m.title}</span>
                    <Badge variant="secondary" className="text-xs">{m.badge}</Badge>
                  </div>
                  <p className="text-2xl font-semibold text-foreground mb-0.5">{m.value}</p>
                  <p className="text-xs text-muted-foreground">{m.detail}</p>
                </CardContent>
              </Card>
            ))}
          </div>

          <Card>
            <CardHeader>
              <CardTitle className="text-base sm:text-lg">Analytics avancés</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="grid grid-cols-2 lg:grid-cols-4 gap-3">
                {[
                  { icon: Zap, value: '94%', label: 'Qualité appels' },
                  { icon: ThumbsUp, value: '88%', label: 'Taux de résolution' },
                  { icon: PhoneOff, value: '12%', label: "Taux d'abandon" },
                  { icon: Star, value: '4.2/5', label: 'Score NPS' },
                ].map((s, i) => (
                  <div key={i} className="p-4 bg-muted/30 rounded-xl flex items-center gap-3">
                    <div className="w-10 h-10 rounded-lg bg-primary/10 flex items-center justify-center shrink-0">
                      <s.icon className="w-5 h-5 text-primary" />
                    </div>
                    <div>
                      <p className="text-lg font-semibold">{s.value}</p>
                      <p className="text-xs text-muted-foreground">{s.label}</p>
                    </div>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>

          <PerformanceCharts isChargeView={false} />
        </TabsContent>

        {/* Rapports */}
        <TabsContent value="rapports" className="space-y-5">
          <Card>
            <CardHeader className="flex flex-row items-center justify-between">
              <CardTitle className="text-base sm:text-lg">Rapports automatisés</CardTitle>
              <Button size="sm" onClick={() => toast({ title: 'Bientôt disponible', description: 'La planification de rapports arrive prochainement.' })}>
                + Nouveau rapport
              </Button>
            </CardHeader>
            <CardContent>
              <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-4">
                {reports.map((r) => (
                  <div key={r.id} className="p-4 rounded-xl border hover:border-primary/40 transition-all hover:shadow-sm">
                    <div className="flex items-center justify-between mb-3">
                      <div>
                        <p className="font-medium text-sm">{r.label}</p>
                        <p className="text-xs text-muted-foreground">{r.freq}</p>
                      </div>
                      <Badge variant={r.active ? 'secondary' : 'outline'} className="text-xs">
                        {r.active ? 'Actif' : 'Inactif'}
                      </Badge>
                    </div>
                    <div className="grid grid-cols-2 gap-2 text-center mb-4">
                      <div className="p-2.5 bg-muted/30 rounded-lg">
                        <p className="text-sm font-medium">{r.format}</p>
                        <p className="text-xs text-muted-foreground">Format</p>
                      </div>
                      <div className="p-2.5 bg-muted/30 rounded-lg">
                        <p className="text-sm font-medium">{r.schedule}</p>
                        <p className="text-xs text-muted-foreground">{r.freq === 'Quotidien' ? 'Heure' : r.freq === 'Hebdomadaire' ? 'Jour' : 'Date'}</p>
                      </div>
                    </div>
                    <Button variant="outline" size="sm" className="w-full" onClick={() => toast({ title: 'Configuration', description: `${r.label} — bientôt disponible.` })}>
                      <Settings className="w-3.5 h-3.5 mr-1.5" /> Configurer
                    </Button>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  );
}
