import { useState, useMemo } from 'react';
import {
  Users,
  Scale,
  Sliders,
  Save,
  RotateCcw,
  CheckCircle,
  AlertTriangle,
  Info,
  UserCheck,
  ArrowRightLeft,
} from 'lucide-react';
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Switch } from '@/components/ui/switch';
import { Slider } from '@/components/ui/slider';
import { Progress } from '@/components/ui/progress';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { useToast } from '@/hooks/use-toast';
import { mockUsers } from '@/data/recruiterMockData';
import { LeadRedistributionModal } from './LeadRedistributionModal';
import type { RecruiterRole } from '@/types/recruiter';

type DistributionMode = 'balanced' | 'custom';

interface Intervenant {
  id: string;
  name: string;
  initials: string;
  active: boolean;
  percentage: number;
  leadsAssigned: number;
}

interface LeadDistributionConfigProps {
  totalLeads: number;
  orderId: string;
  userRole?: RecruiterRole;
}

export function LeadDistributionConfig({ totalLeads, orderId, userRole }: LeadDistributionConfigProps) {
  const { toast } = useToast();

  // Only chargés who participate in distribution
  const eligibleUsers = useMemo(() =>
    mockUsers
      .filter(u => u.role === 'charge' && u.actif && u.permissions.participerDistribution)
      .map(u => ({
        id: u.id,
        name: `${u.prenom} ${u.nom}`,
        initials: `${u.prenom[0]}${u.nom[0]}`,
        active: true,
        percentage: 0,
        leadsAssigned: 0,
      })),
    []
  );

  const [mode, setMode] = useState<DistributionMode>('balanced');
  const [isRedistributionOpen, setIsRedistributionOpen] = useState(false);
  const [intervenants, setIntervenants] = useState<Intervenant[]>(() => {
    const share = eligibleUsers.length > 0 ? Math.floor(100 / eligibleUsers.length) : 0;
    const remainder = eligibleUsers.length > 0 ? 100 - share * eligibleUsers.length : 0;
    return eligibleUsers.map((u, i) => ({
      ...u,
      percentage: share + (i < remainder ? 1 : 0),
      leadsAssigned: Math.round(totalLeads * (share + (i < remainder ? 1 : 0)) / 100),
    }));
  });
  const [saved, setSaved] = useState(false);

  const activeIntervenants = intervenants.filter(i => i.active);
  const totalPercentage = activeIntervenants.reduce((sum, i) => sum + i.percentage, 0);
  const isValid = totalPercentage === 100 && activeIntervenants.length > 0;

  const recalculateBalanced = (list: Intervenant[]) => {
    const active = list.filter(i => i.active);
    if (active.length === 0) return list;
    const share = Math.floor(100 / active.length);
    const remainder = 100 - share * active.length;
    let idx = 0;
    return list.map(i => {
      if (!i.active) return { ...i, percentage: 0, leadsAssigned: 0 };
      const pct = share + (idx < remainder ? 1 : 0);
      idx++;
      return { ...i, percentage: pct, leadsAssigned: Math.round(totalLeads * pct / 100) };
    });
  };

  const handleModeChange = (newMode: DistributionMode) => {
    setMode(newMode);
    setSaved(false);
    if (newMode === 'balanced') {
      setIntervenants(prev => recalculateBalanced(prev));
    }
  };

  const handleToggleIntervenant = (id: string) => {
    setSaved(false);
    setIntervenants(prev => {
      const updated = prev.map(i => i.id === id ? { ...i, active: !i.active } : i);
      return mode === 'balanced' ? recalculateBalanced(updated) : updated.map(i =>
        !i.active ? { ...i, percentage: 0, leadsAssigned: 0 } : i
      );
    });
  };

  const handlePercentageChange = (id: string, value: number) => {
    if (mode !== 'custom') return;
    setSaved(false);
    setIntervenants(prev => prev.map(i =>
      i.id === id
        ? { ...i, percentage: value, leadsAssigned: Math.round(totalLeads * value / 100) }
        : i
    ));
  };

  const handleReset = () => {
    setSaved(false);
    setIntervenants(prev => recalculateBalanced(prev.map(i => ({ ...i, active: true }))));
    setMode('balanced');
  };

  const handleSave = () => {
    if (!isValid) {
      toast({
        title: 'Configuration invalide',
        description: `Le total doit être exactement 100%. Actuellement: ${totalPercentage}%`,
        variant: 'destructive',
      });
      return;
    }
    setSaved(true);
    toast({
      title: 'Distribution enregistrée',
      description: `${activeIntervenants.length} intervenant(s) configuré(s) pour ${totalLeads} leads.`,
    });
  };

  if (eligibleUsers.length === 0) {
    return (
      <Card className="border shadow-sm">
        <CardContent className="p-6 text-center space-y-3">
          <Users className="w-10 h-10 text-muted-foreground mx-auto" />
          <h3 className="font-semibold text-foreground">Aucun intervenant éligible</h3>
          <p className="text-sm text-muted-foreground max-w-md mx-auto">
            Aucun chargé de recrutement actif n'a la permission "Distribution leads" activée.
            Activez cette option dans la gestion des utilisateurs.
          </p>
        </CardContent>
      </Card>
    );
  }

  return (
    <div className="space-y-4">
      {/* Mode selector */}
      <Card className="border shadow-sm">
        <CardContent className="p-4 sm:p-5">
          <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
            <div>
              <h3 className="font-semibold text-foreground flex items-center gap-2 text-sm">
                <Sliders className="w-4 h-4 text-primary" />
                Mode de distribution
              </h3>
              <p className="text-xs text-muted-foreground mt-1">
                Choisissez comment répartir les leads entre les intervenants
              </p>
            </div>
            <Select value={mode} onValueChange={(v) => handleModeChange(v as DistributionMode)}>
              <SelectTrigger className="w-full sm:w-[260px]">
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="balanced">
                  <div className="flex items-center gap-2">
                    <Scale className="w-4 h-4 text-primary" />
                    <span>Dispatching équilibré</span>
                  </div>
                </SelectItem>
                <SelectItem value="custom">
                  <div className="flex items-center gap-2">
                    <Sliders className="w-4 h-4 text-violet-600" />
                    <span>Dispatching par intervenant</span>
                  </div>
                </SelectItem>
              </SelectContent>
            </Select>
          </div>

          {/* Mode description */}
          <div className={`mt-4 p-3 rounded-lg border text-xs flex items-start gap-2 ${
            mode === 'balanced'
              ? 'bg-primary/5 border-primary/20 text-primary'
              : 'bg-violet-500/5 border-violet-500/20 text-violet-700 dark:text-violet-400'
          }`}>
            <Info className="w-4 h-4 shrink-0 mt-0.5" />
            <span>
              {mode === 'balanced'
                ? 'Les leads seront répartis équitablement entre tous les intervenants actifs. Le volume est automatiquement ajusté.'
                : 'Attribuez un pourcentage spécifique à chaque intervenant. Le total doit atteindre 100%.'}
            </span>
          </div>
        </CardContent>
      </Card>

      {/* Status bar */}
      <div className="flex flex-wrap items-center gap-3">
        <Badge variant="outline" className="gap-1.5 text-xs px-2.5 py-1">
          <Users className="w-3 h-3" />
          {activeIntervenants.length} intervenant(s) actif(s)
        </Badge>
        <Badge variant="outline" className="gap-1.5 text-xs px-2.5 py-1">
          <UserCheck className="w-3 h-3" />
          {totalLeads} leads à distribuer
        </Badge>
        <Badge
          className={`gap-1.5 text-xs px-2.5 py-1 ${
            isValid
              ? 'bg-emerald-500/10 text-emerald-600 border-emerald-500/30'
              : 'bg-destructive/10 text-destructive border-destructive/30'
          }`}
        >
          {isValid ? <CheckCircle className="w-3 h-3" /> : <AlertTriangle className="w-3 h-3" />}
          Total: {totalPercentage}%
        </Badge>
        {saved && (
          <Badge className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30 gap-1.5 text-xs px-2.5 py-1">
            <CheckCircle className="w-3 h-3" /> Enregistré
          </Badge>
        )}
      </div>

      {/* Intervenants */}
      <div className="grid gap-3">
        {intervenants.map((intervenant) => (
          <Card
            key={intervenant.id}
            className={`border shadow-sm transition-all ${
              !intervenant.active ? 'opacity-50 bg-muted/30' : ''
            }`}
          >
            <CardContent className="p-4">
              <div className="flex items-center gap-4">
                {/* Avatar + info */}
                <div className="flex items-center gap-3 min-w-0 flex-1">
                  <Avatar className="h-10 w-10 shrink-0">
                    <AvatarFallback className="bg-primary/10 text-primary text-sm font-semibold">
                      {intervenant.initials}
                    </AvatarFallback>
                  </Avatar>
                  <div className="min-w-0">
                    <p className="text-sm font-medium text-foreground truncate">{intervenant.name}</p>
                    <p className="text-xs text-muted-foreground">
                      {intervenant.active
                        ? `${intervenant.leadsAssigned} lead(s) assigné(s)`
                        : 'Désactivé'}
                    </p>
                  </div>
                </div>

                {/* Toggle */}
                <Switch
                  checked={intervenant.active}
                  onCheckedChange={() => handleToggleIntervenant(intervenant.id)}
                  className="shrink-0"
                />

                {/* Percentage display */}
                <div className="text-right shrink-0 w-16">
                  <p className={`text-lg font-bold tabular-nums ${
                    intervenant.active ? 'text-primary' : 'text-muted-foreground'
                  }`}>
                    {intervenant.percentage}%
                  </p>
                </div>
              </div>

              {/* Slider for custom mode */}
              {intervenant.active && (
                <div className="mt-3 space-y-2">
                  {mode === 'custom' ? (
                    <Slider
                      value={[intervenant.percentage]}
                      onValueChange={([v]) => handlePercentageChange(intervenant.id, v)}
                      max={100}
                      min={0}
                      step={5}
                      className="w-full"
                    />
                  ) : (
                    <Progress value={intervenant.percentage} className="h-2" />
                  )}
                  <div className="flex justify-between text-[10px] text-muted-foreground">
                    <span>{intervenant.leadsAssigned} leads</span>
                    <span>{intervenant.percentage}% du volume total</span>
                  </div>
                </div>
              )}
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Actions */}
      <div className="flex flex-col sm:flex-row gap-2 sm:justify-end">
        {(userRole === 'admin' || userRole === 'super_admin') && (
          <Button variant="outline" onClick={() => setIsRedistributionOpen(true)} className="gap-1.5 mr-auto border-primary/30 text-primary hover:bg-primary/5">
            <ArrowRightLeft className="w-4 h-4" />
            Redistribuer (absence)
          </Button>
        )}
        <Button variant="outline" onClick={handleReset} className="gap-1.5">
          <RotateCcw className="w-4 h-4" />
          Réinitialiser
        </Button>
        <Button onClick={handleSave} disabled={!isValid} className="gap-1.5">
          <Save className="w-4 h-4" />
          Enregistrer la distribution
        </Button>
      </div>

      <LeadRedistributionModal
        isOpen={isRedistributionOpen}
        onClose={() => setIsRedistributionOpen(false)}
        orderId={orderId}
        totalLeads={totalLeads}
      />
    </div>
  );
}
