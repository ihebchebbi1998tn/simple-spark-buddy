import { useState, useMemo, useCallback } from 'react';
import {
  Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Card, CardContent } from '@/components/ui/card';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import { Slider } from '@/components/ui/slider';
import { Progress } from '@/components/ui/progress';
import { Separator } from '@/components/ui/separator';
import {
  Table, TableBody, TableCell, TableHead, TableHeader, TableRow,
} from '@/components/ui/table';
import {
  Select, SelectContent, SelectItem, SelectTrigger, SelectValue,
} from '@/components/ui/select';
import {
  Users, UserMinus, UserPlus, ArrowRightLeft, Scale, Sliders,
  AlertTriangle, CheckCircle, Save, RotateCcw, ShieldAlert,
  Thermometer, Plane, Baby, GraduationCap, HelpCircle,
  ArrowRight, Info, Eye, TrendingUp, BarChart3, ArrowLeft,
} from 'lucide-react';
import { useToast } from '@/hooks/use-toast';
import { mockUsers } from '@/data/recruiterMockData';

const ABSENCE_REASONS = [
  { value: 'maladie', label: 'Maladie', icon: Thermometer },
  { value: 'conge', label: 'Congé', icon: Plane },
  { value: 'formation', label: 'Formation', icon: GraduationCap },
  { value: 'maternite', label: 'Maternité/Paternité', icon: Baby },
  { value: 'autre', label: 'Autre', icon: HelpCircle },
] as const;

type RedistributionMode = 'balanced' | 'custom';
type Step = 'select' | 'redistribute' | 'preview';

interface TeamMember {
  id: string;
  name: string;
  initials: string;
  currentPercentage: number;
  currentLeads: number;
  newPercentage: number;
  newLeads: number;
  available: boolean;
  absenceReason?: string;
}

interface LeadRedistributionModalProps {
  isOpen: boolean;
  onClose: () => void;
  orderId: string;
  totalLeads: number;
}

export function LeadRedistributionModal({
  isOpen, onClose, orderId, totalLeads,
}: LeadRedistributionModalProps) {
  const { toast } = useToast();
  const [step, setStep] = useState<Step>('select');
  const [mode, setMode] = useState<RedistributionMode>('balanced');

  const initialMembers = useMemo<TeamMember[]>(() => {
    const eligible = mockUsers.filter(
      u => u.role === 'charge' && u.actif && u.permissions.participerDistribution
    );
    const share = eligible.length > 0 ? Math.floor(100 / eligible.length) : 0;
    const remainder = eligible.length > 0 ? 100 - share * eligible.length : 0;
    return eligible.map((u, i) => {
      const pct = share + (i < remainder ? 1 : 0);
      return {
        id: u.id,
        name: `${u.prenom} ${u.nom}`,
        initials: `${u.prenom[0]}${u.nom[0]}`,
        currentPercentage: pct,
        currentLeads: Math.round(totalLeads * pct / 100),
        newPercentage: pct,
        newLeads: Math.round(totalLeads * pct / 100),
        available: true,
      };
    });
  }, [totalLeads]);

  const [members, setMembers] = useState<TeamMember[]>(initialMembers);

  const unavailableMembers = members.filter(m => !m.available);
  const availableMembers = members.filter(m => m.available);
  const totalNewPct = availableMembers.reduce((s, m) => s + m.newPercentage, 0);
  const isValid = totalNewPct === 100 && availableMembers.length > 0;
  const freedLeads = unavailableMembers.reduce((s, m) => s + m.currentLeads, 0);
  const freedPct = unavailableMembers.reduce((s, m) => s + m.currentPercentage, 0);

  const markUnavailable = (id: string, reason: string) => {
    setMembers(prev => prev.map(m =>
      m.id === id ? { ...m, available: false, absenceReason: reason, newPercentage: 0, newLeads: 0 } : m
    ));
  };

  const markAvailable = (id: string) => {
    setMembers(prev => {
      const updated = prev.map(m => m.id === id ? { ...m, available: true, absenceReason: undefined } : m);
      return redistributeBalanced(updated);
    });
  };

  const redistributeBalanced = useCallback((list: TeamMember[]): TeamMember[] => {
    const active = list.filter(m => m.available);
    if (active.length === 0) return list;
    const share = Math.floor(100 / active.length);
    const remainder = 100 - share * active.length;
    let idx = 0;
    return list.map(m => {
      if (!m.available) return { ...m, newPercentage: 0, newLeads: 0 };
      const pct = share + (idx < remainder ? 1 : 0);
      idx++;
      return { ...m, newPercentage: pct, newLeads: Math.round(totalLeads * pct / 100) };
    });
  }, [totalLeads]);

  const handleProceedToRedistribute = () => {
    if (unavailableMembers.length === 0) {
      toast({ title: 'Aucun absent', description: 'Veuillez d\'abord marquer au moins un membre comme indisponible.', variant: 'destructive' });
      return;
    }
    setMembers(prev => redistributeBalanced(prev));
    setMode('balanced');
    setStep('redistribute');
  };

  const handleModeChange = (m: RedistributionMode) => {
    setMode(m);
    if (m === 'balanced') setMembers(prev => redistributeBalanced(prev));
  };

  const handleCustomChange = (id: string, value: number) => {
    if (mode !== 'custom') return;
    setMembers(prev => prev.map(m =>
      m.id === id ? { ...m, newPercentage: value, newLeads: Math.round(totalLeads * value / 100) } : m
    ));
  };

  const handlePreview = () => {
    if (!isValid) {
      toast({ title: 'Total invalide', description: `Le total doit être 100%. Actuellement: ${totalNewPct}%`, variant: 'destructive' });
      return;
    }
    setStep('preview');
  };

  const handleApply = () => {
    toast({
      title: 'Redistribution appliquée',
      description: `${freedLeads} leads de ${unavailableMembers.length} absent(s) redistribués à ${availableMembers.length} membre(s).`,
    });
    onClose();
  };

  const handleReset = () => {
    setMembers(initialMembers);
    setStep('select');
    setMode('balanced');
  };

  const getReasonInfo = (reason?: string) => {
    const r = ABSENCE_REASONS.find(a => a.value === reason);
    return r || ABSENCE_REASONS[4];
  };

  // Simulation metrics
  const maxLoad = availableMembers.length > 0 ? Math.max(...availableMembers.map(m => m.newLeads)) : 0;
  const minLoad = availableMembers.length > 0 ? Math.min(...availableMembers.map(m => m.newLeads)) : 0;
  const avgLoad = availableMembers.length > 0 ? Math.round(availableMembers.reduce((s, m) => s + m.newLeads, 0) / availableMembers.length) : 0;
  const loadVariance = maxLoad - minLoad;

  const stepLabels: Record<Step, string> = {
    select: '1. Absences',
    redistribute: '2. Configuration',
    preview: '3. Simulation',
  };

  return (
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent className="sm:max-w-2xl max-h-[90vh] overflow-y-auto p-0">
        {/* Header */}
        <DialogHeader className="px-6 pt-6 pb-4 border-b bg-muted/30">
          <DialogTitle className="text-lg font-bold flex items-center gap-2.5">
            <div className="w-9 h-9 rounded-lg bg-primary/10 flex items-center justify-center">
              <ArrowRightLeft className="w-5 h-5 text-primary" />
            </div>
            <div>
              Redistribution des Leads
              <p className="text-xs font-normal text-muted-foreground mt-0.5">
                Commande {orderId} · {totalLeads} leads
              </p>
            </div>
          </DialogTitle>

          {/* Step indicator */}
          <div className="flex items-center gap-1 mt-4">
            {(['select', 'redistribute', 'preview'] as Step[]).map((s, i) => (
              <div key={s} className="flex items-center gap-1 flex-1">
                <div className={`flex items-center gap-1.5 px-2.5 py-1 rounded-full text-[10px] font-semibold transition-colors w-full justify-center ${
                  step === s
                    ? 'bg-primary text-primary-foreground'
                    : (['select', 'redistribute', 'preview'].indexOf(step) > i)
                      ? 'bg-primary/10 text-primary'
                      : 'bg-muted text-muted-foreground'
                }`}>
                  {stepLabels[s]}
                </div>
                {i < 2 && <ArrowRight className="w-3 h-3 text-muted-foreground shrink-0" />}
              </div>
            ))}
          </div>
        </DialogHeader>

        <div className="p-6 space-y-5">
          {step === 'select' && (
            <>
              <div className="flex items-start gap-2 p-3 bg-primary/5 border border-primary/20 rounded-lg text-sm">
                <Info className="w-4 h-4 text-primary shrink-0 mt-0.5" />
                <span className="text-primary">
                  Sélectionnez les membres indisponibles. Leurs leads seront automatiquement redistribués aux membres restants.
                </span>
              </div>

              {unavailableMembers.length > 0 && (
                <div className="flex flex-wrap gap-2">
                  <Badge className="bg-destructive/10 text-destructive border-destructive/30 text-xs gap-1">
                    <UserMinus className="w-3 h-3" />
                    {unavailableMembers.length} absent(s)
                  </Badge>
                  <Badge className="bg-primary/10 text-primary border-primary/30 text-xs gap-1">
                    <ArrowRight className="w-3 h-3" />
                    {freedLeads} leads à redistribuer ({freedPct}%)
                  </Badge>
                </div>
              )}

              <div className="space-y-2">
                {members.map((member) => {
                  const reasonInfo = getReasonInfo(member.absenceReason);
                  const ReasonIcon = reasonInfo.icon;
                  return (
                    <Card key={member.id} className={`border transition-all ${!member.available ? 'border-destructive/30 bg-destructive/5' : 'shadow-sm'}`}>
                      <CardContent className="p-4">
                        <div className="flex items-center gap-3">
                          <Avatar className="h-10 w-10 shrink-0">
                            <AvatarFallback className={`text-sm font-semibold ${member.available ? 'bg-primary/10 text-primary' : 'bg-destructive/10 text-destructive'}`}>
                              {member.initials}
                            </AvatarFallback>
                          </Avatar>
                          <div className="flex-1 min-w-0">
                            <p className="text-sm font-semibold text-foreground">{member.name}</p>
                            <p className="text-xs text-muted-foreground">{member.currentLeads} leads · {member.currentPercentage}% actuel</p>
                          </div>
                          {member.available ? (
                            <Select onValueChange={(v) => markUnavailable(member.id, v)}>
                              <SelectTrigger className="w-auto h-8 text-xs border-destructive/30 text-destructive hover:bg-destructive/5 gap-1.5">
                                <UserMinus className="w-3.5 h-3.5" />
                                <span className="hidden sm:inline">Marquer absent</span>
                              </SelectTrigger>
                              <SelectContent>
                                {ABSENCE_REASONS.map((r) => (
                                  <SelectItem key={r.value} value={r.value}>
                                    <div className="flex items-center gap-2"><r.icon className="w-3.5 h-3.5" />{r.label}</div>
                                  </SelectItem>
                                ))}
                              </SelectContent>
                            </Select>
                          ) : (
                            <div className="flex items-center gap-2">
                              <Badge variant="outline" className="text-[10px] gap-1 border-destructive/30 text-destructive">
                                <ReasonIcon className="w-3 h-3" />{reasonInfo.label}
                              </Badge>
                              <Button variant="ghost" size="sm" onClick={() => markAvailable(member.id)} className="h-8 text-xs text-primary hover:text-primary gap-1">
                                <UserPlus className="w-3.5 h-3.5" /><span className="hidden sm:inline">Rétablir</span>
                              </Button>
                            </div>
                          )}
                        </div>
                      </CardContent>
                    </Card>
                  );
                })}
              </div>

              <Button onClick={handleProceedToRedistribute} disabled={unavailableMembers.length === 0} className="w-full gap-2">
                <ArrowRightLeft className="w-4 h-4" />
                Configurer la redistribution ({freedLeads} leads)
              </Button>
            </>
          )}

          {step === 'redistribute' && (
            <>
              <div className="p-3 bg-destructive/5 border border-destructive/20 rounded-lg">
                <p className="text-xs font-semibold text-destructive flex items-center gap-1.5 mb-2">
                  <ShieldAlert className="w-3.5 h-3.5" />Membres indisponibles
                </p>
                <div className="flex flex-wrap gap-2">
                  {unavailableMembers.map(m => {
                    const r = getReasonInfo(m.absenceReason);
                    return (
                      <Badge key={m.id} variant="outline" className="text-xs gap-1 border-destructive/30 text-destructive">
                        {m.initials} · {r.label} · {m.currentLeads} leads
                      </Badge>
                    );
                  })}
                </div>
              </div>

              <div className="flex items-center gap-3">
                <Select value={mode} onValueChange={(v) => handleModeChange(v as RedistributionMode)}>
                  <SelectTrigger className="w-full sm:w-64"><SelectValue /></SelectTrigger>
                  <SelectContent>
                    <SelectItem value="balanced">
                      <div className="flex items-center gap-2"><Scale className="w-4 h-4 text-primary" /> Répartition équilibrée</div>
                    </SelectItem>
                    <SelectItem value="custom">
                      <div className="flex items-center gap-2"><Sliders className="w-4 h-4 text-primary" /> Répartition manuelle</div>
                    </SelectItem>
                  </SelectContent>
                </Select>
                <Badge className={`shrink-0 text-xs px-2.5 py-1 gap-1 ${isValid ? 'bg-primary/10 text-primary border-primary/30' : 'bg-destructive/10 text-destructive border-destructive/30'}`}>
                  {isValid ? <CheckCircle className="w-3 h-3" /> : <AlertTriangle className="w-3 h-3" />}
                  {totalNewPct}%
                </Badge>
              </div>

              <div className="space-y-2">
                {availableMembers.map((member) => {
                  const diff = member.newPercentage - member.currentPercentage;
                  return (
                    <Card key={member.id} className="border shadow-sm">
                      <CardContent className="p-4">
                        <div className="flex items-center gap-3 mb-3">
                          <Avatar className="h-9 w-9 shrink-0">
                            <AvatarFallback className="bg-primary/10 text-primary text-sm font-semibold">{member.initials}</AvatarFallback>
                          </Avatar>
                          <div className="flex-1 min-w-0">
                            <p className="text-sm font-semibold text-foreground">{member.name}</p>
                            <div className="flex items-center gap-2 text-xs text-muted-foreground">
                              <span>{member.currentPercentage}%</span>
                              <ArrowRight className="w-3 h-3 text-primary" />
                              <span className="font-bold text-primary">{member.newPercentage}%</span>
                              {diff > 0 && (
                                <Badge className="bg-primary/10 text-primary border-primary/30 text-[10px] px-1.5 py-0">+{diff}%</Badge>
                              )}
                            </div>
                          </div>
                          <div className="text-right shrink-0">
                            <p className="text-lg font-bold tabular-nums text-primary">{member.newLeads}</p>
                            <p className="text-[10px] text-muted-foreground">leads</p>
                          </div>
                        </div>
                        {mode === 'custom' ? (
                          <Slider value={[member.newPercentage]} onValueChange={([v]) => handleCustomChange(member.id, v)} max={100} min={0} step={5} className="w-full" />
                        ) : (
                          <Progress value={member.newPercentage} className="h-2" />
                        )}
                      </CardContent>
                    </Card>
                  );
                })}
              </div>

              <Button onClick={handlePreview} disabled={!isValid} className="w-full gap-2">
                <Eye className="w-4 h-4" />
                Prévisualiser l'impact
              </Button>
            </>
          )}

          {step === 'preview' && (
            <>
              {/* Simulation banner */}
              <div className="flex items-start gap-2.5 p-3 bg-accent/10 border border-accent/30 rounded-lg">
                <Eye className="w-4 h-4 text-accent shrink-0 mt-0.5" />
                <div>
                  <p className="text-sm font-semibold text-accent">Mode Simulation</p>
                  <p className="text-xs text-accent/80 mt-0.5">
                    Aucune modification n'a été appliquée. Vérifiez l'impact avant de confirmer.
                  </p>
                </div>
              </div>

              {/* Impact KPIs */}
              <div className="grid grid-cols-2 sm:grid-cols-4 gap-3">
                {[
                  { label: 'Leads redistribués', value: freedLeads, icon: ArrowRightLeft, color: 'text-destructive' },
                  { label: 'Charge max', value: maxLoad, icon: TrendingUp, color: 'text-primary' },
                  { label: 'Charge moy.', value: avgLoad, icon: BarChart3, color: 'text-primary' },
                  { label: 'Écart de charge', value: loadVariance, icon: Scale, color: loadVariance <= 2 ? 'text-primary' : 'text-accent' },
                ].map((kpi) => (
                  <Card key={kpi.label} className="border">
                    <CardContent className="p-3 text-center">
                      <kpi.icon className={`w-4 h-4 mx-auto mb-1 ${kpi.color}`} />
                      <p className="text-lg font-bold tabular-nums">{kpi.value}</p>
                      <p className="text-[10px] text-muted-foreground leading-tight">{kpi.label}</p>
                    </CardContent>
                  </Card>
                ))}
              </div>

              {/* Before / After table */}
              <Card className="border shadow-sm">
                <CardContent className="p-0">
                  <Table>
                    <TableHeader>
                      <TableRow className="bg-muted/30 hover:bg-muted/30">
                        <TableHead className="text-xs font-semibold">Membre</TableHead>
                        <TableHead className="text-xs font-semibold text-center">Statut</TableHead>
                        <TableHead className="text-xs font-semibold text-right">Avant</TableHead>
                        <TableHead className="text-xs font-semibold text-center w-10"></TableHead>
                        <TableHead className="text-xs font-semibold text-right">Après</TableHead>
                        <TableHead className="text-xs font-semibold text-right">Δ Leads</TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {members.map((m) => {
                        const delta = m.newLeads - m.currentLeads;
                        return (
                          <TableRow key={m.id} className={!m.available ? 'bg-destructive/5' : ''}>
                            <TableCell>
                              <div className="flex items-center gap-2">
                                <Avatar className="h-7 w-7 shrink-0">
                                  <AvatarFallback className={`text-[10px] font-bold ${m.available ? 'bg-primary/10 text-primary' : 'bg-destructive/10 text-destructive'}`}>
                                    {m.initials}
                                  </AvatarFallback>
                                </Avatar>
                                <span className="text-xs font-medium truncate">{m.name}</span>
                              </div>
                            </TableCell>
                            <TableCell className="text-center">
                              {m.available ? (
                                <Badge className="bg-primary/10 text-primary border-primary/30 text-[10px] px-1.5 py-0">Actif</Badge>
                              ) : (
                                <Badge className="bg-destructive/10 text-destructive border-destructive/30 text-[10px] px-1.5 py-0">
                                  {getReasonInfo(m.absenceReason).label}
                                </Badge>
                              )}
                            </TableCell>
                            <TableCell className="text-right">
                              <div>
                                <span className="text-xs font-semibold tabular-nums">{m.currentLeads}</span>
                                <span className="text-[10px] text-muted-foreground ml-1">({m.currentPercentage}%)</span>
                              </div>
                            </TableCell>
                            <TableCell className="text-center">
                              <ArrowRight className="w-3 h-3 text-muted-foreground mx-auto" />
                            </TableCell>
                            <TableCell className="text-right">
                              <div>
                                <span className={`text-xs font-semibold tabular-nums ${m.available ? 'text-primary' : 'text-muted-foreground'}`}>
                                  {m.newLeads}
                                </span>
                                <span className="text-[10px] text-muted-foreground ml-1">({m.newPercentage}%)</span>
                              </div>
                            </TableCell>
                            <TableCell className="text-right">
                              {delta !== 0 ? (
                                <Badge className={`text-[10px] px-1.5 py-0 tabular-nums ${
                                  delta > 0
                                    ? 'bg-primary/10 text-primary border-primary/30'
                                    : 'bg-destructive/10 text-destructive border-destructive/30'
                                }`}>
                                  {delta > 0 ? '+' : ''}{delta}
                                </Badge>
                              ) : (
                                <span className="text-xs text-muted-foreground">—</span>
                              )}
                            </TableCell>
                          </TableRow>
                        );
                      })}
                    </TableBody>
                  </Table>
                </CardContent>
              </Card>

              {/* Load balance visualization */}
              <Card className="border shadow-sm">
                <CardContent className="p-4 space-y-3">
                  <p className="text-xs font-semibold text-muted-foreground flex items-center gap-1.5">
                    <BarChart3 className="w-3.5 h-3.5" /> Répartition de la charge après redistribution
                  </p>
                  {availableMembers.map((m) => (
                    <div key={m.id} className="flex items-center gap-3">
                      <span className="text-xs font-medium w-20 truncate">{m.initials}</span>
                      <div className="flex-1">
                        <Progress value={(m.newLeads / Math.max(maxLoad, 1)) * 100} className="h-3" />
                      </div>
                      <span className="text-xs font-bold tabular-nums w-14 text-right">{m.newLeads} leads</span>
                    </div>
                  ))}
                  {loadVariance <= 2 ? (
                    <p className="text-[11px] text-primary flex items-center gap-1"><CheckCircle className="w-3 h-3" /> Charge bien équilibrée (écart: {loadVariance})</p>
                  ) : (
                    <p className="text-[11px] text-accent flex items-center gap-1"><AlertTriangle className="w-3 h-3" /> Charge déséquilibrée — envisagez un ajustement (écart: {loadVariance})</p>
                  )}
                </CardContent>
              </Card>
            </>
          )}
        </div>

        {/* Footer */}
        <DialogFooter className="px-6 py-4 border-t bg-muted/20 gap-2">
          {step !== 'select' && (
            <Button
              variant="outline"
              onClick={() => setStep(step === 'preview' ? 'redistribute' : 'select')}
              className="gap-1.5 mr-auto"
            >
              <ArrowLeft className="w-4 h-4" />
              Retour
            </Button>
          )}
          <Button variant="ghost" onClick={handleReset} className="text-xs">
            Réinitialiser
          </Button>
          {step === 'preview' && (
            <Button onClick={handleApply} className="gap-1.5">
              <Save className="w-4 h-4" />
              Appliquer la redistribution
            </Button>
          )}
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
