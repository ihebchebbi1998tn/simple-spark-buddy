import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Separator } from '@/components/ui/separator';
import {
  CheckCircle,
  Target,
  MapPin,
  Users,
  Clock,
  Shield,
  Sparkles,
  FileText,
  Package,
  AlertTriangle,
} from 'lucide-react';
import { useToast } from '@/hooks/use-toast';
import type { PPIConfig } from '@/types/recruiter';
import { ppiLabelMap, ppiOptions } from '@/data/recruiterMockData';

interface OrderConfirmationModalProps {
  config: PPIConfig | null;
  isOpen: boolean;
  onClose: () => void;
  onModify: () => void;
}

function getLabel(key: string) {
  return ppiLabelMap[key] || key || '—';
}

function InfoRow({ label, value }: { label: string; value: string }) {
  return (
    <div className="flex items-center justify-between py-1.5">
      <span className="text-sm text-muted-foreground">{label}</span>
      <span className="text-sm font-medium text-foreground text-right max-w-[55%] truncate">
        {value}
      </span>
    </div>
  );
}

export function OrderConfirmationModal({
  config,
  isOpen,
  onClose,
  onModify,
}: OrderConfirmationModalProps) {
  const { toast } = useToast();

  if (!config) return null;

  const handleConfirm = () => {
    toast({
      title: 'Commande validée !',
      description: `Votre commande de ${config.candidatesCount} candidat(s) a été enregistrée. Montant : ${config.totalEstimate.toLocaleString()} TND.`,
    });
    onClose();
  };

  return (
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent className="max-w-lg w-[95vw] sm:w-full max-h-[90vh] overflow-y-auto p-0 gap-0">
        {/* Header */}
        <div className="bg-primary p-5 sm:p-6 text-primary-foreground text-center">
          <div className="w-14 h-14 rounded-full bg-primary-foreground/20 flex items-center justify-center mx-auto mb-3">
            <CheckCircle className="w-7 h-7" />
          </div>
          <DialogHeader>
            <DialogTitle className="text-lg sm:text-xl font-bold text-primary-foreground">
              Confirmation de votre commande
            </DialogTitle>
          </DialogHeader>
          <p className="text-primary-foreground/80 text-sm mt-1">
            Vérifiez les détails avant de valider
          </p>
        </div>

        <div className="p-4 sm:p-5 space-y-4">
          {/* Récapitulatif de la commande */}
          <div>
            <div className="flex items-center gap-2 mb-2">
              <Package className="w-4 h-4 text-primary" />
              <h4 className="text-sm font-semibold text-foreground">Récapitulatif de votre commande</h4>
            </div>
            <div className="bg-primary/5 border border-primary/20 rounded-lg p-4 space-y-2">
              <div className="flex items-center justify-between">
                <span className="text-sm text-muted-foreground">Type de pack</span>
                <Badge variant="secondary" className="font-medium">Pay Per Integration</Badge>
              </div>
              <Separator className="my-1" />
              <InfoRow label="Modèle" value={getLabel(config.model)} />
              <InfoRow label="Nombre de candidats" value={`${config.candidatesCount}`} />
              <InfoRow label="Coût par candidat" value={`${config.costPerCandidate} TND`} />
              <Separator className="my-1" />
              <div className="flex items-center justify-between pt-1">
                <span className="text-sm font-semibold text-foreground">Total</span>
                <span className="text-lg font-bold text-primary">{config.totalEstimate.toLocaleString()} TND</span>
              </div>
            </div>
          </div>

          {/* Résumé financier cards */}
          <div className="grid grid-cols-3 gap-3">
            <div className="bg-muted/40 rounded-lg p-3 text-center">
              <p className="text-[11px] text-muted-foreground mb-0.5">Candidats</p>
              <p className="text-lg font-bold text-foreground">{config.candidatesCount}</p>
            </div>
            <div className="bg-muted/40 rounded-lg p-3 text-center">
              <p className="text-[11px] text-muted-foreground mb-0.5">Leads fournis</p>
              <p className="text-lg font-bold text-foreground">{config.leadsProvided}</p>
            </div>
            <div className="bg-muted/40 rounded-lg p-3 text-center">
              <p className="text-[11px] text-muted-foreground mb-0.5">Garantie</p>
              <p className="text-lg font-bold text-foreground">90 jours</p>
            </div>
          </div>

          <Separator />

          {/* Profil */}
          <div>
            <div className="flex items-center gap-2 mb-2">
              <Target className="w-4 h-4 text-primary" />
              <h4 className="text-sm font-semibold text-foreground">Profil recherché</h4>
            </div>
            <div className="bg-muted/30 rounded-lg p-3 space-y-0.5">
              <InfoRow label="Poste" value={getLabel(config.poste)} />
              <InfoRow label="Activité" value={getLabel(config.activite)} />
              <InfoRow label="Opération" value={getLabel(config.operation)} />
              <InfoRow label="Exp. globale" value={getLabel(config.expGlobale)} />
              <InfoRow label="Genre" value={getLabel(config.genre)} />
            </div>
          </div>

          {/* Langue */}
          <div>
            <div className="flex items-center gap-2 mb-2">
              <Sparkles className="w-4 h-4 text-primary" />
              <h4 className="text-sm font-semibold text-foreground">Langue</h4>
            </div>
            <div className="bg-muted/30 rounded-lg p-3 space-y-0.5">
              <InfoRow label="Langue principale" value={getLabel(config.languePrincipale)} />
              {config.bilingue && (
                <InfoRow label="Langue secondaire" value={getLabel(config.langueSecondaire)} />
              )}
            </div>
          </div>

          {/* Configuration */}
          <div>
            <div className="flex items-center gap-2 mb-2">
              <FileText className="w-4 h-4 text-primary" />
              <h4 className="text-sm font-semibold text-foreground">Configuration</h4>
            </div>
            <div className="bg-muted/30 rounded-lg p-3 space-y-0.5">
              <InfoRow label="Localisation" value={getLabel(config.location)} />
              <InfoRow label="Urgence" value={getLabel(config.urgency)} />
              <InfoRow label="Modèle PPI" value={getLabel(config.model)} />
              <InfoRow label="Leads fournis" value={`${config.leadsProvided} leads`} />
              <InfoRow label="Garantie" value="90 jours" />
            </div>
          </div>

          <Separator />

          {/* Grille de remise selon efficacité */}
          <div>
            <div className="flex items-center gap-2 mb-2">
              <Sparkles className="w-4 h-4 text-primary" />
              <h4 className="text-sm font-semibold text-foreground">Grille de remise selon votre efficacité</h4>
            </div>
            <div className="grid grid-cols-2 gap-2">
              {ppiOptions.discounts.map((d, i) => (
                <div
                  key={i}
                  className={`rounded-lg p-2.5 text-center border transition-all ${
                    d.featured
                      ? 'border-emerald-500/40 bg-emerald-500/5'
                      : 'border-border bg-muted/30'
                  }`}
                >
                  <p className={`text-base font-bold mb-0.5 ${d.featured ? 'text-emerald-600' : 'text-primary'}`}>
                    {d.percent}
                  </p>
                  <p className="text-[10px] text-muted-foreground leading-tight">{d.condition}</p>
                </div>
              ))}
            </div>
          </div>

          {/* Notice */}
          <div className="flex items-start gap-2 bg-amber-500/10 border border-amber-500/20 rounded-lg p-3">
            <AlertTriangle className="w-4 h-4 text-amber-600 mt-0.5 shrink-0" />
            <p className="text-xs text-muted-foreground">
              Veuillez vérifier les détails de votre commande avant validation. Une fois validée, la commande sera traitée par notre équipe.
            </p>
          </div>

          {/* Actions */}
          <div className="flex flex-col-reverse sm:flex-row gap-2 sm:gap-3">
            <Button variant="outline" onClick={onModify} className="flex-1">
              Modifier ma commande
            </Button>
            <Button onClick={handleConfirm} className="flex-1">
              <CheckCircle className="w-4 h-4 mr-2" />
              Valider ma commande
            </Button>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}
