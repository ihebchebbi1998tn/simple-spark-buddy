import { useState } from 'react';
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
  CheckCircle,
  CreditCard,
  Calendar,
  Building2,
  Mail,
  Phone,
  User,
} from 'lucide-react';
import { useToast } from '@/hooks/use-toast';

interface SubscriptionOffer {
  id: string;
  name: string;
  price: string;
  priceNote: string;
  description: string;
  limits: { title: string; description: string };
}

interface SubscriptionOrderModalProps {
  offer: SubscriptionOffer | null;
  isOpen: boolean;
  onClose: () => void;
}

export function SubscriptionOrderModal({ offer, isOpen, onClose }: SubscriptionOrderModalProps) {
  const { toast } = useToast();
  const [step, setStep] = useState<'details' | 'confirm'>('details');
  const [engagement, setEngagement] = useState('3');
  const [paymentMethod, setPaymentMethod] = useState('virement');
  const [contactName, setContactName] = useState('');
  const [contactEmail, setContactEmail] = useState('');
  const [contactPhone, setContactPhone] = useState('');

  if (!offer) return null;

  const monthlyPrice = parseInt(offer.price);
  const engagementMonths = parseInt(engagement);
  const discount = engagementMonths >= 12 ? 0.15 : engagementMonths >= 6 ? 0.1 : 0;
  const discountedPrice = Math.round(monthlyPrice * (1 - discount));
  const totalEstimate = discountedPrice * engagementMonths;

  const handleConfirm = () => {
    toast({
      title: 'Abonnement souscrit !',
      description: `Votre abonnement ${offer.name} a été enregistré. Montant mensuel : ${discountedPrice} TND.`,
    });
    setStep('details');
    onClose();
  };

  const handleClose = () => {
    setStep('details');
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
              <CreditCard className="w-7 h-7" />
            )}
          </div>
          <DialogHeader>
            <DialogTitle className="text-lg sm:text-xl font-bold text-primary-foreground">
              {step === 'confirm' ? 'Confirmer votre abonnement' : `Souscrire — ${offer.name}`}
            </DialogTitle>
          </DialogHeader>
          <p className="text-primary-foreground/80 text-sm mt-1">
            {step === 'confirm' ? 'Vérifiez les détails avant de valider' : offer.description}
          </p>
        </div>

        <div className="p-4 sm:p-5 space-y-4">
          {step === 'details' ? (
            <>
              {/* Plan summary */}
              <div className="bg-primary/5 border border-primary/20 rounded-lg p-4 text-center">
                <p className="text-sm text-muted-foreground mb-1">{offer.name}</p>
                <div className="flex items-baseline justify-center gap-1">
                  <span className="text-3xl font-bold text-primary">{offer.price}</span>
                  <span className="text-sm text-muted-foreground">{offer.priceNote}</span>
                </div>
                <p className="text-xs text-muted-foreground mt-1">{offer.limits.title}</p>
              </div>

              {/* Engagement */}
              <div className="space-y-2">
                <Label className="text-sm font-medium">Durée d'engagement</Label>
                <RadioGroup value={engagement} onValueChange={setEngagement} className="grid grid-cols-3 gap-2">
                  {[
                    { value: '1', label: 'Sans engagement', discount: '' },
                    { value: '3', label: '3 mois', discount: '' },
                    { value: '6', label: '6 mois', discount: '-10%' },
                    { value: '12', label: '12 mois', discount: '-15%' },
                  ].map((opt) => (
                    <div key={opt.value} className="relative">
                      <RadioGroupItem value={opt.value} id={`eng-${opt.value}`} className="peer sr-only" />
                      <Label
                        htmlFor={`eng-${opt.value}`}
                        className="flex flex-col items-center p-3 rounded-lg border-2 cursor-pointer transition-all peer-data-[state=checked]:border-primary peer-data-[state=checked]:bg-primary/5 hover:bg-muted/50"
                      >
                        <span className="text-sm font-medium">{opt.label}</span>
                        {opt.discount && (
                          <Badge className="mt-1 bg-emerald-500/10 text-emerald-600 border-emerald-500/30 text-[10px]">
                            {opt.discount}
                          </Badge>
                        )}
                      </Label>
                    </div>
                  ))}
                </RadioGroup>
              </div>

              {/* Payment method */}
              <div className="space-y-2">
                <Label className="text-sm font-medium">Mode de paiement</Label>
                <RadioGroup value={paymentMethod} onValueChange={setPaymentMethod} className="space-y-2">
                  {[
                    { value: 'virement', label: 'Virement bancaire' },
                    { value: 'cheque', label: 'Chèque' },
                    { value: 'especes', label: 'Espèces' },
                  ].map((pm) => (
                    <div key={pm.value} className="flex items-center gap-3 p-3 rounded-lg border hover:bg-muted/30 transition-colors">
                      <RadioGroupItem value={pm.value} id={`pm-${pm.value}`} />
                      <Label htmlFor={`pm-${pm.value}`} className="text-sm cursor-pointer flex-1">{pm.label}</Label>
                    </div>
                  ))}
                </RadioGroup>
              </div>

              {/* Contact info */}
              <div className="space-y-3">
                <Label className="text-sm font-medium">Contact de facturation</Label>
                <div className="grid sm:grid-cols-2 gap-3">
                  <div className="space-y-1.5">
                    <Label className="text-xs text-muted-foreground">Nom complet</Label>
                    <Input value={contactName} onChange={(e) => setContactName(e.target.value)} placeholder="Nom et prénom" />
                  </div>
                  <div className="space-y-1.5">
                    <Label className="text-xs text-muted-foreground">Email</Label>
                    <Input type="email" value={contactEmail} onChange={(e) => setContactEmail(e.target.value)} placeholder="email@entreprise.com" />
                  </div>
                  <div className="space-y-1.5 sm:col-span-2">
                    <Label className="text-xs text-muted-foreground">Téléphone</Label>
                    <Input value={contactPhone} onChange={(e) => setContactPhone(e.target.value)} placeholder="+216 XX XXX XXX" />
                  </div>
                </div>
              </div>

              <Separator />

              {/* Price summary */}
              <div className="bg-muted/30 rounded-lg p-4 space-y-2">
                <div className="flex justify-between text-sm">
                  <span className="text-muted-foreground">Prix mensuel</span>
                  <span>{offer.price} TND</span>
                </div>
                {discount > 0 && (
                  <div className="flex justify-between text-sm text-emerald-600">
                    <span>Remise engagement</span>
                    <span>-{Math.round(discount * 100)}%</span>
                  </div>
                )}
                <div className="flex justify-between text-sm">
                  <span className="text-muted-foreground">Prix après remise</span>
                  <span className="font-medium">{discountedPrice} TND/mois</span>
                </div>
                <Separator />
                <div className="flex justify-between font-bold">
                  <span>Total ({engagementMonths} mois)</span>
                  <span className="text-primary">{totalEstimate.toLocaleString()} TND</span>
                </div>
              </div>

              <div className="flex gap-3">
                <Button variant="outline" onClick={handleClose} className="flex-1">Annuler</Button>
                <Button onClick={() => setStep('confirm')} className="flex-1">
                  Continuer
                </Button>
              </div>
            </>
          ) : (
            <>
              {/* Confirmation summary */}
              <div className="grid grid-cols-3 gap-3">
                <div className="bg-primary/5 border border-primary/20 rounded-lg p-3 text-center">
                  <p className="text-[11px] text-muted-foreground mb-0.5">Plan</p>
                  <p className="text-sm font-bold text-primary">{offer.name}</p>
                </div>
                <div className="bg-primary/5 border border-primary/20 rounded-lg p-3 text-center">
                  <p className="text-[11px] text-muted-foreground mb-0.5">Mensuel</p>
                  <p className="text-lg font-bold text-primary">{discountedPrice} TND</p>
                </div>
                <div className="bg-primary/5 border border-primary/20 rounded-lg p-3 text-center">
                  <p className="text-[11px] text-muted-foreground mb-0.5">Engagement</p>
                  <p className="text-lg font-bold text-primary">{engagementMonths} mois</p>
                </div>
              </div>

              <div className="bg-muted/30 rounded-lg p-3 space-y-1.5">
                <div className="flex justify-between text-sm">
                  <span className="text-muted-foreground">Leads inclus</span>
                  <span className="font-medium">{offer.limits.title}</span>
                </div>
                <div className="flex justify-between text-sm">
                  <span className="text-muted-foreground">Paiement</span>
                  <span className="font-medium capitalize">{paymentMethod}</span>
                </div>
                {contactName && (
                  <div className="flex justify-between text-sm">
                    <span className="text-muted-foreground">Contact</span>
                    <span className="font-medium">{contactName}</span>
                  </div>
                )}
                <Separator />
                <div className="flex justify-between font-bold pt-1">
                  <span>Total estimé</span>
                  <span className="text-primary">{totalEstimate.toLocaleString()} TND</span>
                </div>
              </div>

              <div className="flex flex-col-reverse sm:flex-row gap-2 sm:gap-3">
                <Button variant="outline" onClick={() => setStep('details')} className="flex-1">
                  Modifier
                </Button>
                <Button onClick={handleConfirm} className="flex-1">
                  <CheckCircle className="w-4 h-4 mr-2" />
                  Confirmer l'abonnement
                </Button>
              </div>
            </>
          )}
        </div>
      </DialogContent>
    </Dialog>
  );
}
