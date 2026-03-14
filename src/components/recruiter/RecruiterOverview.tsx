import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Progress } from '@/components/ui/progress';
import {
  ChevronRight,
  Search,
  ArrowUpRight,
  ArrowDownRight,
  Users,
  ShoppingCart,
  Clock,
  TrendingUp,
  Wallet,
  Building2,
  Package,
  CircleDot,
} from 'lucide-react';
import type { RecruiterUser } from '@/types/recruiter';
import { ROLE_LABELS } from '@/types/recruiter';
import { getStatsByRole, getOrdersByRole, getActivitiesByRole } from '@/data/recruiterMockData';

interface RecruiterOverviewProps {
  onSectionChange: (section: string) => void;
  user: RecruiterUser;
}

const quickActions = [
  {
    id: 'search',
    label: 'Rechercher des candidats',
    description: 'Trouvez les profils correspondant à vos critères',
    icon: Search,
  },
  {
    id: 'orders',
    label: 'Voir mes commandes',
    description: "Suivez l'état de vos commandes en cours",
    icon: ShoppingCart,
  },
  {
    id: 'offers',
    label: 'Découvrir nos offres',
    description: 'Explorez nos solutions de recrutement',
    icon: Package,
  },
];

interface StatCardProps {
  label: string;
  value: string | number;
  trend?: { value: string; up: boolean };
  progress?: number;
  icon: React.ElementType;
}

function StatCard({ label, value, trend, progress, icon: Icon }: StatCardProps) {
  return (
    <Card className="border border-border/60 hover:border-border transition-colors bg-card">
      <CardContent className="p-4 sm:p-5">
        <div className="flex items-start justify-between mb-3">
          <div className="w-9 h-9 rounded-lg flex items-center justify-center bg-primary/10">
            <Icon className="w-4 h-4 text-primary" />
          </div>
          {trend && (
            <span
              className={`inline-flex items-center text-xs font-medium px-1.5 py-0.5 rounded-md ${
                trend.up ? 'text-emerald-700 bg-emerald-500/10' : 'text-destructive bg-destructive/10'
              }`}
            >
              {trend.up ? (
                <ArrowUpRight className="w-3 h-3 mr-0.5" />
              ) : (
                <ArrowDownRight className="w-3 h-3 mr-0.5" />
              )}
              {trend.value}
            </span>
          )}
        </div>
        <p className="text-2xl font-bold text-foreground tabular-nums tracking-tight">
          {value}
        </p>
        <p className="text-xs text-muted-foreground mt-0.5">{label}</p>
        {progress !== undefined && (
          <Progress value={progress} className="mt-3 h-1.5" />
        )}
      </CardContent>
    </Card>
  );
}

export function RecruiterOverview({ onSectionChange, user }: RecruiterOverviewProps) {
  const stats = getStatsByRole(user.role);
  const recentOrders = getOrdersByRole(user.role, user.name).slice(0, 4);
  const recentActivities = getActivitiesByRole(user.role);

  const getStatusLabel = (status: string) => {
    const map: Record<string, { label: string; cls: string }> = {
      active: { label: 'En cours', cls: 'bg-primary/10 text-primary border-primary/20' },
      completed: { label: 'Terminée', cls: 'bg-emerald-500/10 text-emerald-600 border-emerald-500/20' },
      pending: { label: 'En attente', cls: 'bg-amber-500/10 text-amber-600 border-amber-500/20' },
    };
    const cfg = map[status] || { label: status, cls: '' };
    return <Badge variant="outline" className={`${cfg.cls} text-[11px] font-medium`}>{cfg.label}</Badge>;
  };

  const welcomeMessage =
    user.role === 'super_admin'
      ? 'Vue globale de tous les centres et équipes'
      : user.role === 'admin'
        ? `Gérez votre centre ${user.company}`
        : 'Votre espace de recrutement personnel';

  return (
    <div className="space-y-6 max-w-6xl">
      {/* Welcome */}
      <div>
        <div className="flex items-center gap-2.5 mb-1">
          <h1 className="text-xl sm:text-2xl font-bold text-foreground tracking-tight">
            Bonjour, {user.name}
          </h1>
          <Badge variant="outline" className="text-[10px] font-medium uppercase tracking-wider text-muted-foreground border-border">
            {ROLE_LABELS[user.role]}
          </Badge>
        </div>
        <p className="text-sm text-muted-foreground">{welcomeMessage}</p>
      </div>

      {/* Stats */}
      <div className="grid grid-cols-2 lg:grid-cols-4 gap-3">
        <StatCard
          label="Candidats consultés"
          value={stats.candidatesViewed.toLocaleString()}
          trend={{ value: '+12%', up: true }}
          icon={Users}
        />
        <StatCard
          label="Commandes actives"
          value={stats.activeOrders}
          icon={ShoppingCart}
        />
        <StatCard
          label="Intégrations en attente"
          value={stats.pendingIntegrations}
          trend={{ value: '-3', up: false }}
          icon={Clock}
        />
        <StatCard
          label="Taux de conversion"
          value={`${stats.conversionRate}%`}
          progress={stats.conversionRate}
          icon={TrendingUp}
        />
      </div>

      {/* Super-admin extra stats */}
      {user.role === 'super_admin' && (
        <div className="grid grid-cols-3 gap-3">
          <StatCard
            label="Revenu total"
            value={`${((stats.totalRevenue ?? 0) / 1000).toFixed(0)}K TND`}
            trend={{ value: '+8%', up: true }}
            icon={Wallet}
          />
          <StatCard
            label="Utilisateurs actifs"
            value={stats.totalUsers ?? 0}
            icon={Users}
          />
          <StatCard
            label="Centres actifs"
            value={stats.activeCenters ?? 0}
            icon={Building2}
          />
        </div>
      )}

      {/* Quick Actions */}
      <div className="grid sm:grid-cols-3 gap-3">
        {quickActions.map((action) => {
          const Icon = action.icon;
          return (
            <button
              key={action.id}
              className="text-left p-4 rounded-xl border border-border/60 bg-card hover:bg-muted/40 hover:border-border transition-all duration-150 group"
              onClick={() => onSectionChange(action.id)}
            >
              <div className="flex items-center gap-3 mb-2">
                <div className="w-8 h-8 rounded-lg flex items-center justify-center bg-muted">
                  <Icon className="w-4 h-4 text-muted-foreground" />
                </div>
                <span className="text-sm font-semibold text-foreground">{action.label}</span>
              </div>
              <p className="text-xs text-muted-foreground leading-relaxed pl-11">
                {action.description}
              </p>
            </button>
          );
        })}
      </div>

      <div className="grid lg:grid-cols-2 gap-4">
        {/* Recent Orders */}
        <Card className="border-border/60">
          <CardHeader className="flex flex-row items-center justify-between pb-3">
            <CardTitle className="text-xs font-semibold text-muted-foreground uppercase tracking-wider">
              Commandes récentes
            </CardTitle>
            <Button
              variant="ghost"
              size="sm"
              onClick={() => onSectionChange('orders')}
              className="text-primary h-7 px-2 text-xs"
            >
              Voir tout
              <ChevronRight className="w-3.5 h-3.5 ml-0.5" />
            </Button>
          </CardHeader>
          <CardContent className="pt-0">
            {recentOrders.length === 0 ? (
              <p className="text-sm text-muted-foreground text-center py-8">
                Aucune commande pour le moment
              </p>
            ) : (
              <div className="divide-y divide-border/50">
                {recentOrders.map((order) => (
                  <div
                    key={order.id}
                    className="flex items-center justify-between py-3 first:pt-0 last:pb-0 gap-3"
                  >
                    <div className="min-w-0">
                      <p className="text-sm font-medium text-foreground truncate">{order.id}</p>
                      <p className="text-xs text-muted-foreground truncate">
                        {order.type} · {order.candidates} candidats
                        {user.role === 'super_admin' && order.assignedTo && ` · ${order.assignedTo}`}
                      </p>
                    </div>
                    {getStatusLabel(order.status)}
                  </div>
                ))}
              </div>
            )}
          </CardContent>
        </Card>

        {/* Recent Activity */}
        <Card className="border-border/60">
          <CardHeader className="pb-3">
            <CardTitle className="text-xs font-semibold text-muted-foreground uppercase tracking-wider">
              Activité récente
            </CardTitle>
          </CardHeader>
          <CardContent className="pt-0">
            <div className="divide-y divide-border/50">
              {recentActivities.map((activity) => (
                <div
                  key={activity.id}
                  className="flex items-center gap-3 py-3 first:pt-0 last:pb-0"
                >
                  <CircleDot className="w-3 h-3 text-muted-foreground/50 shrink-0" />
                  <div className="min-w-0 flex-1">
                    <p className="text-sm font-medium text-foreground truncate">{activity.label}</p>
                    <p className="text-xs text-muted-foreground truncate">{activity.detail}</p>
                  </div>
                  <span className="text-[11px] text-muted-foreground shrink-0 tabular-nums">{activity.time}</span>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
