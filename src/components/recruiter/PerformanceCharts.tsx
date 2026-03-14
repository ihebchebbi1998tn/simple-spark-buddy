import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { 
  LineChart, Line, AreaChart, Area, BarChart, Bar, PieChart, Pie, Cell,
  RadarChart, Radar, PolarGrid, PolarAngleAxis, PolarRadiusAxis,
  XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Legend
} from 'recharts';
import { TrendingUp, BarChart3, PieChart as PieChartIcon, Activity, Shield } from 'lucide-react';

const weeklyEvolution = [
  { jour: 'Lun', leads: 32, entretiens: 12, integrations: 4 },
  { jour: 'Mar', leads: 28, entretiens: 15, integrations: 6 },
  { jour: 'Mer', leads: 45, entretiens: 18, integrations: 5 },
  { jour: 'Jeu', leads: 38, entretiens: 14, integrations: 7 },
  { jour: 'Ven', leads: 52, entretiens: 22, integrations: 9 },
  { jour: 'Sam', leads: 20, entretiens: 8, integrations: 3 },
  { jour: 'Dim', leads: 15, entretiens: 5, integrations: 2 },
];

const monthlyComparison = [
  { mois: 'Jan', objectif: 300, realise: 285 },
  { mois: 'Fév', objectif: 320, realise: 310 },
  { mois: 'Mar', objectif: 350, realise: 380 },
  { mois: 'Avr', objectif: 340, realise: 325 },
  { mois: 'Mai', objectif: 360, realise: 390 },
  { mois: 'Jun', objectif: 380, realise: 365 },
];

const conversionFunnel = [
  { name: 'Leads reçus', value: 500, fill: 'hsl(var(--primary))' },
  { name: 'Appelés', value: 420, fill: 'hsl(220, 70%, 55%)' },
  { name: 'Entretiens', value: 180, fill: 'hsl(262, 83%, 58%)' },
  { name: 'Retenus', value: 95, fill: 'hsl(142, 76%, 36%)' },
  { name: 'Intégrés', value: 62, fill: 'hsl(142, 71%, 45%)' },
];

const statusDistribution = [
  { name: 'Traités', value: 45, color: 'hsl(142, 76%, 36%)' },
  { name: 'En cours', value: 25, color: 'hsl(var(--primary))' },
  { name: 'En attente', value: 20, color: 'hsl(45, 93%, 47%)' },
  { name: 'Non joignables', value: 10, color: 'hsl(var(--muted-foreground))' },
];

const teamPerformance = [
  { name: 'Ahmed S.', leads: 320, conversion: 35 },
  { name: 'Fatima B.', leads: 285, conversion: 31 },
  { name: 'Karim T.', leads: 260, conversion: 28 },
  { name: 'Sara I.', leads: 245, conversion: 26 },
  { name: 'Omar M.', leads: 220, conversion: 24 },
];

const qualityMetrics = [
  { subject: 'Qualité Appels', me: 98, equipe: 92 },
  { subject: 'Résolution', me: 96, equipe: 88 },
  { subject: 'Satisfaction', me: 95, equipe: 90 },
  { subject: 'Efficacité', me: 93, equipe: 85 },
  { subject: 'Conversion', me: 87, equipe: 78 },
];

const tooltipStyle = {
  backgroundColor: 'hsl(var(--background))',
  border: '1px solid hsl(var(--border))',
  borderRadius: '8px',
  fontSize: '11px',
  padding: '8px',
};

interface PerformanceChartsProps {
  isChargeView?: boolean;
}

export function PerformanceCharts({ isChargeView = false }: PerformanceChartsProps) {
  return (
    <div className="space-y-4 sm:space-y-6">
      {/* Row 1: Evolution Chart + Funnel */}
      <div className="grid lg:grid-cols-2 gap-3 sm:gap-4">
        <Card className="border shadow-sm">
          <CardHeader className="pb-2 px-3 sm:px-6">
            <CardTitle className="text-sm sm:text-base flex items-center gap-2">
              <div className="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center shrink-0">
                <Activity className="w-4 h-4 text-primary" />
              </div>
              <span>Évolution Hebdomadaire</span>
            </CardTitle>
          </CardHeader>
          <CardContent className="px-2 sm:px-6 pb-4">
            <ResponsiveContainer width="100%" height={220}>
              <AreaChart data={weeklyEvolution} margin={{ top: 5, right: 5, left: -20, bottom: 0 }}>
                <defs>
                  <linearGradient id="colorLeads" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="hsl(var(--primary))" stopOpacity={0.3}/>
                    <stop offset="95%" stopColor="hsl(var(--primary))" stopOpacity={0}/>
                  </linearGradient>
                  <linearGradient id="colorEntretiens" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="hsl(262, 83%, 58%)" stopOpacity={0.3}/>
                    <stop offset="95%" stopColor="hsl(262, 83%, 58%)" stopOpacity={0}/>
                  </linearGradient>
                  <linearGradient id="colorIntegrations" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="hsl(142, 76%, 36%)" stopOpacity={0.3}/>
                    <stop offset="95%" stopColor="hsl(142, 76%, 36%)" stopOpacity={0}/>
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                <XAxis dataKey="jour" tick={{ fontSize: 10 }} />
                <YAxis tick={{ fontSize: 10 }} width={30} />
                <Tooltip contentStyle={tooltipStyle} />
                <Legend iconType="circle" wrapperStyle={{ fontSize: '10px', paddingTop: '8px' }} />
                <Area type="monotone" dataKey="leads" stroke="hsl(var(--primary))" fillOpacity={1} fill="url(#colorLeads)" name="Leads" strokeWidth={2} />
                <Area type="monotone" dataKey="entretiens" stroke="hsl(262, 83%, 58%)" fillOpacity={1} fill="url(#colorEntretiens)" name="Entretiens" strokeWidth={2} />
                <Area type="monotone" dataKey="integrations" stroke="hsl(142, 76%, 36%)" fillOpacity={1} fill="url(#colorIntegrations)" name="Intégrations" strokeWidth={2} />
              </AreaChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        <Card className="border shadow-sm">
          <CardHeader className="pb-2 px-3 sm:px-6">
            <CardTitle className="text-sm sm:text-base flex items-center gap-2">
              <div className="w-8 h-8 rounded-lg bg-emerald-500/10 flex items-center justify-center shrink-0">
                <TrendingUp className="w-4 h-4 text-emerald-600" />
              </div>
              <span>Tunnel de Conversion</span>
            </CardTitle>
          </CardHeader>
          <CardContent className="px-3 sm:px-6 pb-4">
            <div className="space-y-2.5 sm:space-y-3">
              {conversionFunnel.map((item, index) => {
                const percentage = (item.value / conversionFunnel[0].value) * 100;
                const prevValue = index > 0 ? conversionFunnel[index - 1].value : item.value;
                const conversionRate = index > 0 ? Math.round((item.value / prevValue) * 100) : 100;
                return (
                  <div key={item.name} className="relative">
                    <div className="flex items-center justify-between mb-1">
                      <span className="text-xs sm:text-sm font-medium">{item.name}</span>
                      <div className="flex items-center gap-1.5 sm:gap-2">
                        <span className="text-xs sm:text-sm font-bold">{item.value}</span>
                        {index > 0 && (
                          <Badge variant="outline" className="text-[10px] sm:text-xs h-5">{conversionRate}%</Badge>
                        )}
                      </div>
                    </div>
                    <div className="h-6 sm:h-8 bg-muted rounded-lg overflow-hidden">
                      <div className="h-full rounded-lg transition-all duration-500 flex items-center justify-end pr-2" style={{ width: `${percentage}%`, backgroundColor: item.fill }}>
                        <span className="text-[10px] text-white font-medium hidden sm:block">{Math.round(percentage)}%</span>
                      </div>
                    </div>
                  </div>
                );
              })}
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Row 2: Monthly + Status */}
      <div className="grid lg:grid-cols-2 gap-3 sm:gap-4">
        <Card className="border shadow-sm">
          <CardHeader className="pb-2 px-3 sm:px-6">
            <CardTitle className="text-sm sm:text-base flex items-center gap-2">
              <div className="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center shrink-0">
                <BarChart3 className="w-4 h-4 text-primary" />
              </div>
              <span>Objectifs vs Réalisé</span>
            </CardTitle>
          </CardHeader>
          <CardContent className="px-2 sm:px-6 pb-4">
            <ResponsiveContainer width="100%" height={220}>
              <BarChart data={monthlyComparison} barGap={2} margin={{ top: 5, right: 5, left: -20, bottom: 0 }}>
                <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                <XAxis dataKey="mois" tick={{ fontSize: 10 }} />
                <YAxis tick={{ fontSize: 10 }} width={30} />
                <Tooltip contentStyle={tooltipStyle} />
                <Legend iconType="circle" wrapperStyle={{ fontSize: '10px', paddingTop: '8px' }} />
                <Bar dataKey="objectif" fill="hsl(var(--muted-foreground))" name="Objectif" radius={[3, 3, 0, 0]} opacity={0.5} />
                <Bar dataKey="realise" fill="hsl(var(--primary))" name="Réalisé" radius={[3, 3, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        <Card className="border shadow-sm">
          <CardHeader className="pb-2 px-3 sm:px-6">
            <CardTitle className="text-sm sm:text-base flex items-center gap-2">
              <div className="w-8 h-8 rounded-lg bg-violet-500/10 flex items-center justify-center shrink-0">
                <PieChartIcon className="w-4 h-4 text-violet-600" />
              </div>
              <span>Répartition des Statuts</span>
            </CardTitle>
          </CardHeader>
          <CardContent className="px-3 sm:px-6 pb-4">
            <div className="flex flex-col sm:flex-row items-center gap-4 sm:gap-6">
              <ResponsiveContainer width="100%" height={180} className="sm:w-1/2">
                <PieChart>
                  <Pie data={statusDistribution} cx="50%" cy="50%" innerRadius={45} outerRadius={70} paddingAngle={4} dataKey="value">
                    {statusDistribution.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={entry.color} />
                    ))}
                  </Pie>
                  <Tooltip contentStyle={tooltipStyle} />
                </PieChart>
              </ResponsiveContainer>
              <div className="space-y-2 w-full sm:flex-1">
                {statusDistribution.map((item) => (
                  <div key={item.name} className="flex items-center justify-between p-2 rounded-lg bg-muted/30">
                    <div className="flex items-center gap-2">
                      <div className="w-3 h-3 rounded-full shrink-0" style={{ backgroundColor: item.color }} />
                      <span className="text-xs sm:text-sm">{item.name}</span>
                    </div>
                    <span className="text-xs sm:text-sm font-bold">{item.value}%</span>
                  </div>
                ))}
              </div>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Row 3: Quality Radar (Chargé) + Performance Line */}
      <div className="grid lg:grid-cols-2 gap-3 sm:gap-4">
        {/* Quality Radar Chart */}
        <Card className="border shadow-sm">
          <CardHeader className="pb-2 px-3 sm:px-6">
            <CardTitle className="text-sm sm:text-base flex items-center gap-2">
              <div className="w-8 h-8 rounded-lg bg-emerald-500/10 flex items-center justify-center shrink-0">
                <Shield className="w-4 h-4 text-emerald-600" />
              </div>
              <span>Indicateurs Qualité</span>
            </CardTitle>
          </CardHeader>
          <CardContent className="px-2 sm:px-6 pb-4">
            <ResponsiveContainer width="100%" height={250}>
              <RadarChart cx="50%" cy="50%" outerRadius="70%" data={qualityMetrics}>
                <PolarGrid className="stroke-muted" />
                <PolarAngleAxis dataKey="subject" tick={{ fontSize: 10 }} className="text-muted-foreground" />
                <PolarRadiusAxis angle={30} domain={[0, 100]} tick={{ fontSize: 9 }} />
                <Radar name="Mes Scores" dataKey="me" stroke="hsl(142, 76%, 36%)" fill="hsl(142, 76%, 36%)" fillOpacity={0.25} strokeWidth={2} />
                <Radar name="Moyenne Équipe" dataKey="equipe" stroke="hsl(var(--primary))" fill="hsl(var(--primary))" fillOpacity={0.15} strokeWidth={2} />
                <Legend iconType="circle" wrapperStyle={{ fontSize: '10px', paddingTop: '8px' }} />
                <Tooltip contentStyle={tooltipStyle} />
              </RadarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        {/* Performance Evolution Line */}
        <Card className="border shadow-sm">
          <CardHeader className="pb-2 px-3 sm:px-6">
            <CardTitle className="text-sm sm:text-base flex items-center gap-2">
              <div className="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center shrink-0">
                <TrendingUp className="w-4 h-4 text-primary" />
              </div>
              <span>Évolution des Performances</span>
            </CardTitle>
          </CardHeader>
          <CardContent className="px-2 sm:px-6 pb-4">
            <ResponsiveContainer width="100%" height={250}>
              <LineChart data={[
                { mois: 'Jan', traites: 200, retenus: 100, integres: 60 },
                { mois: 'Fév', traites: 220, retenus: 110, integres: 70 },
                { mois: 'Mar', traites: 240, retenus: 120, integres: 75 },
                { mois: 'Avr', traites: 260, retenus: 130, integres: 80 },
                { mois: 'Mai', traites: 280, retenus: 140, integres: 85 },
                { mois: 'Jun', traites: 285, retenus: 142, integres: 89 },
              ]} margin={{ top: 5, right: 5, left: -20, bottom: 0 }}>
                <CartesianGrid strokeDasharray="3 3" className="stroke-muted" />
                <XAxis dataKey="mois" tick={{ fontSize: 10 }} />
                <YAxis tick={{ fontSize: 10 }} width={30} />
                <Tooltip contentStyle={tooltipStyle} />
                <Legend iconType="circle" wrapperStyle={{ fontSize: '10px', paddingTop: '8px' }} />
                <Line type="monotone" dataKey="traites" stroke="hsl(var(--primary))" name="Leads Traités" strokeWidth={2} dot={{ r: 3 }} />
                <Line type="monotone" dataKey="retenus" stroke="hsl(142, 76%, 36%)" name="Candidats Retenus" strokeWidth={2} dot={{ r: 3 }} />
                <Line type="monotone" dataKey="integres" stroke="hsl(45, 93%, 47%)" name="Candidats Intégrés" strokeWidth={2} dot={{ r: 3 }} />
              </LineChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      </div>

      {/* Row 4: Team Performance (Admin only) */}
      {!isChargeView && (
        <Card className="border shadow-sm">
          <CardHeader className="pb-2 px-3 sm:px-6">
            <CardTitle className="text-sm sm:text-base flex items-center gap-2">
              <div className="w-8 h-8 rounded-lg bg-amber-500/10 flex items-center justify-center shrink-0">
                <BarChart3 className="w-4 h-4 text-amber-600" />
              </div>
              <span>Performance Équipe</span>
            </CardTitle>
          </CardHeader>
          <CardContent className="px-2 sm:px-6 pb-4">
            <ResponsiveContainer width="100%" height={250}>
              <BarChart data={teamPerformance} layout="vertical" barGap={6} margin={{ top: 5, right: 5, left: 5, bottom: 0 }}>
                <CartesianGrid strokeDasharray="3 3" className="stroke-muted" horizontal={false} />
                <XAxis type="number" tick={{ fontSize: 10 }} />
                <YAxis dataKey="name" type="category" tick={{ fontSize: 10 }} width={60} />
                <Tooltip contentStyle={tooltipStyle} />
                <Legend wrapperStyle={{ fontSize: '10px', paddingTop: '8px' }} />
                <Bar dataKey="leads" fill="hsl(var(--primary))" name="Leads traités" radius={[0, 3, 3, 0]} />
                <Bar dataKey="conversion" fill="hsl(142, 76%, 36%)" name="Taux conversion (%)" radius={[0, 3, 3, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>
      )}
    </div>
  );
}
