import { useState, useMemo } from 'react';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Separator } from '@/components/ui/separator';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import {
  CheckCircle,
  Users,
  MapPin,
  Clock,
  Sparkles,
} from 'lucide-react';
import { useToast } from '@/hooks/use-toast';
import { ppiOptions } from '@/data/recruiterMockData';

interface LeadOffer {
  id: string;
  name: string;
  price: string;
  priceNote: string;
  description: string;
  limits: { title: string; description: string };
}

interface LeadOrderModalProps {
  offer: LeadOffer | null;
  isOpen: boolean;
  onClose: () => void;
}

export function LeadOrderModal({ offer, isOpen, onClose }: LeadOrderModalProps) {
  const { toast } = useToast();
  const [step, setStep] = useState<'config' | 'confirm'>('config');
  const [quantity, setQuantity] = useState(20);
  const [location, setLocation] = useState('tunis');
  const [urgency, setUrgency] = useState('normal');
  const [languePrincipale, setLanguePrincipale] = useState('');
  const [activite, setActivite] = useState('');

  if (!offer) return null;

  const unitPrice = parseInt(offer.price);
  const minOrder = offer.id === 'lead-agent' ? 10 : offer.id === 'lead-responsable' ? 5 : 20;

  const locationMult = ppiOptions.locations.find(l => l.value === location)?.multiplier ?? 1;
  const urgencyMult = ppiOptions.urgencies.find(u => u.value === urgency)?.multiplier ?? 1;
  const adjustedPrice = Math.round(unitPrice * locationMult * urgencyMult);
  const volumeDiscount = quantity >= 100 ? 0.15 : quantity >= 50 ? 0.1 : quantity >= 30 ? 0.05 : 0;
  const finalUnitPrice = Math.round(adjustedPrice * (1 - volumeDiscount));
  const total = finalUnitPrice * quantity;

  const handleConfirm = () => {
    toast({
      title: 'Commande de leads enregistrée !',
      description: `${quantity} leads "${offer.name}" commandés. Total : ${total.toLocaleString()} TND.`,
    });
    setStep('config');
    onClose();
  };

  const handleClose = () => {
    setStep('config');
    onClose();
  };

  return (
    <Dialog open={isOpen} onOpenChange={handleClose}>
      <DialogContent className="max-w-lg w-[95vw] sm:w-full max-h-[90vh] overflow-y-auto p-0 gap-0">
        {/* Header */}
        <div className="bg-primary p-5 sm:p-6 text-primary-foreground text-center">
          <div className="w-14 h-14 rounded-full bg-primary-foreground/20 flex items-center justify-center mx-auto mb-3">
            {step === 'confirm' ? (
              <CheckCircle className="w-7 h-7" />
            ) : (
              <Users className="w-7 h-7" />
            )}
          </div>
          <DialogHeader>
            <DialogTitle className="text-lg sm:text-xl font-bold text-primary-foreground">
              {step === 'confirm' ? 'Confirmer votre commande' : `Commander — ${offer.name}`}
            </DialogTitle>
          </DialogHeader>
          <p className="text-primary-foreground/80 text-sm mt-1">
            {step === 'confirm' ? 'Vérifiez avant validation' : offer.description}
          </p>
        </div>

        <div className="p-4 sm:p-5 space-y-4">
          {step === 'config' ? (
            <>
              {/* Quantity */}
              <div className="space-y-1.5">
                <Label className="text-sm font-medium">Nombre de leads souhaités</Label>
                <Input
                  type="number"
                  min={minOrder}
                  max={500}
                  value={quantity}
                  onChange={(e) => setQuantity(Math.max(minOrder, Math.min(500, Number(e.target.value))))}
                  className="h-10"
                />
                <p className="text-xs text-muted-foreground">Minimum : {minOrder} leads</p>
              </div>

              {/* Criteria */}
              <div className="grid sm:grid-cols-2 gap-3">
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">Localisation</Label>
                  <Select value={location} onValueChange={setLocation}>
                    <SelectTrigger className="h-10">
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      {ppiOptions.locations.map(l => (
                        <SelectItem key={l.value} value={l.value}>{l.label}</SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">Urgence</Label>
                  <Select value={urgency} onValueChange={setUrgency}>
                    <SelectTrigger className="h-10">
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      {ppiOptions.urgencies.map(u => (
                        <SelectItem key={u.value} value={u.value}>{u.label}</SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">Langue principale</Label>
                  <Select value={languePrincipale} onValueChange={setLanguePrincipale}>
                    <SelectTrigger className="h-10">
                      <SelectValue placeholder="Sélectionnez" />
                    </SelectTrigger>
                    <SelectContent>
                      {ppiOptions.langues.map(l => (
                        <SelectItem key={l.value} value={l.value}>{l.label}</SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-1.5">
                  <Label className="text-sm font-medium">Activité</Label>
                  <Select value={activite} onValueChange={setActivite}>
                    <SelectTrigger className="h-10">
                      <SelectValue placeholder="Sélectionnez" />
                    </SelectTrigger>
                    <SelectContent>
                      {ppiOptions.activites.map(a => (
                        <SelectItem key={a.value} value={a.value}>{a.label}</SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
              </div>

              {/* Volume discount info */}
              <div className="bg-muted/30 rounded-lg p-3">
                <p className="text-xs font-medium text-foreground mb-2">Remises volume</p>
                <div className="grid grid-cols-3 gap-2 text-center">
                  {[
                    { min: 30, discount: '5%' },
                    { min: 50, discount: '10%' },
                    { min: 100, discount: '15%' },
                  ].map(v => (
                    <div
                      key={v.min}
                      className={`rounded-md p-2 border text-xs ${
                        quantity >= v.min
                          ? 'border-emerald-500/50 bg-emerald-500/5 text-emerald-600 font-medium'
                          : 'border-border text-muted-foreground'
                      }`}
                    >
                      <p className="font-bold">{v.discount}</p>
                      <p>≥ {v.min} leads</p>
                    </div>
                  ))}
                </div>
              </div>

              <Separator />

              {/* Price summary */}
              <div className="bg-primary/5 border border-primary/20 rounded-lg p-4 space-y-2">
                <div className="flex justify-between text-sm">
                  <span className="text-muted-foreground">Prix unitaire de base</span>
                  <span>{offer.price} TND</span>
                </div>
                <div className="flex justify-between text-sm">
                  <span className="text-muted-foreground">Prix ajusté</span>
                  <span>{adjustedPrice} TND</span>
                </div>
                {volumeDiscount > 0 && (
                  <div className="flex justify-between text-sm text-emerald-600">
                    <span>Remise volume</span>
                    <span>-{Math.round(volumeDiscount * 100)}%</span>
                  </div>
                )}
                <Separator />
                <div className="flex justify-between font-bold text-lg">
                  <span>Total</span>
                  <span className="text-primary">{total.toLocaleString()} TND</span>
                </div>
                <p className="text-xs text-muted-foreground text-center">
                  {quantity} leads × {finalUnitPrice} TND
                </p>
              </div>

              <div className="flex gap-3">
                <Button variant="outline" onClick={handleClose} className="flex-1">Annuler</Button>
                <Button onClick={() => setStep('confirm')} className="flex-1">Continuer</Button>
              </div>
            </>
          ) : (
            <>
              <div className="grid grid-cols-3 gap-3">
                <div className="bg-primary/5 border border-primary/20 rounded-lg p-3 text-center">
                  <p className="text-[11px] text-muted-foreground mb-0.5">Type</p>
                  <p className="text-sm font-bold text-primary">{offer.name}</p>
                </div>
                <div className="bg-primary/5 border border-primary/20 rounded-lg p-3 text-center">
                  <p className="text-[11px] text-muted-foreground mb-0.5">Quantité</p>
                  <p className="text-lg font-bold text-primary">{quantity}</p>
                </div>
                <div className="bg-primary/5 border border-primary/20 rounded-lg p-3 text-center">
                  <p className="text-[11px] text-muted-foreground mb-0.5">Total</p>
                  <p className="text-lg font-bold text-primary">{total.toLocaleString()} TND</p>
                </div>
              </div>

              <div className="bg-muted/30 rounded-lg p-3 space-y-1.5">
                {languePrincipale && (
                  <div className="flex justify-between text-sm">
                    <span className="text-muted-foreground">Langue</span>
                    <span className="font-medium">{ppiOptions.langues.find(l => l.value === languePrincipale)?.label}</span>
                  </div>
                )}
                {activite && (
                  <div className="flex justify-between text-sm">
                    <span className="text-muted-foreground">Activité</span>
                    <span className="font-medium">{ppiOptions.activites.find(a => a.value === activite)?.label}</span>
                  </div>
                )}
                <div className="flex justify-between text-sm">
                  <span className="text-muted-foreground">Localisation</span>
                  <span className="font-medium">{ppiOptions.locations.find(l => l.value === location)?.label}</span>
                </div>
                <div className="flex justify-between text-sm">
                  <span className="text-muted-foreground">Livraison</span>
                  <span className="font-medium">{ppiOptions.urgencies.find(u => u.value === urgency)?.label}</span>
                </div>
              </div>

              <div className="flex flex-col-reverse sm:flex-row gap-2 sm:gap-3">
                <Button variant="outline" onClick={() => setStep('config')} className="flex-1">Modifier</Button>
                <Button onClick={handleConfirm} className="flex-1">
                  <CheckCircle className="w-4 h-4 mr-2" />
                  Confirmer la commande
                </Button>
              </div>
            </>
          )}
        </div>
      </DialogContent>
    </Dialog>
  );
}
