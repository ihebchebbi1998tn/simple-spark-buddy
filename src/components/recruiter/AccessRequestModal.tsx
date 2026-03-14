import { useState } from "react";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogDescription } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { useToast } from "@/hooks/use-toast";
import { Building, Phone, Mail, Briefcase, Users } from "lucide-react";
import { z } from "zod";

const accessRequestSchema = z.object({
  email: z.string().trim().email("Email invalide").max(255),
  phone: z.string().trim().min(8, "Téléphone requis").max(20),
  company: z.string().trim().min(2, "Nom du centre requis").max(100),
  position: z.string().min(1, "Poste requis"),
  size: z.string().min(1, "Taille requise"),
});

const positions = [
  "Directeur / Responsable",
  "Responsable RH",
  "Team Leader",
  "Autre",
];

const sizes = [
  "Moins de 20 positions",
  "20–50 positions",
  "50–150 positions",
  "Plus de 150 positions",
];

interface AccessRequestModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSuccess?: () => void;
}

const AccessRequestModal = ({ open, onOpenChange, onSuccess }: AccessRequestModalProps) => {
  const { toast } = useToast();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [formData, setFormData] = useState({
    email: "",
    phone: "",
    company: "",
    position: "",
    size: "",
  });

  const handleChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    setErrors((prev) => ({ ...prev, [field]: "" }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const result = accessRequestSchema.safeParse(formData);
    if (!result.success) {
      const fieldErrors: Record<string, string> = {};
      result.error.errors.forEach((err) => {
        if (err.path[0]) fieldErrors[err.path[0] as string] = err.message;
      });
      setErrors(fieldErrors);
      return;
    }

    setIsSubmitting(true);
    setTimeout(() => {
      setIsSubmitting(false);
      toast({
        title: "✅ Demande envoyée !",
        description: "Estimation débloquée. Notre équipe vous contactera sous 24h.",
      });
      onOpenChange(false);
      onSuccess?.();
      setFormData({ email: "", phone: "", company: "", position: "", size: "" });
      setErrors({});
    }, 1200);
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-lg">
        <DialogHeader>
          <DialogTitle className="text-xl font-bold">Demande d'accès</DialogTitle>
          <DialogDescription>
            Nous vérifions que vous êtes bien un centre d'appels. Accès envoyé sous 24h.
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-4 pt-2">
          <div className="space-y-1.5">
            <Label className="flex items-center gap-2 text-sm">
              <Mail className="w-4 h-4 text-muted-foreground" />
              Email professionnel <span className="text-destructive">*</span>
            </Label>
            <Input
              type="email"
              placeholder="vous@centretunisie.tn"
              value={formData.email}
              onChange={(e) => handleChange("email", e.target.value)}
              className={errors.email ? "border-destructive" : ""}
            />
            {errors.email && <p className="text-xs text-destructive">{errors.email}</p>}
          </div>

          <div className="space-y-1.5">
            <Label className="flex items-center gap-2 text-sm">
              <Phone className="w-4 h-4 text-muted-foreground" />
              Téléphone <span className="text-destructive">*</span>
            </Label>
            <Input
              type="tel"
              placeholder="+216 XX XXX XXX"
              value={formData.phone}
              onChange={(e) => handleChange("phone", e.target.value)}
              className={errors.phone ? "border-destructive" : ""}
            />
            {errors.phone && <p className="text-xs text-destructive">{errors.phone}</p>}
          </div>

          <div className="space-y-1.5">
            <Label className="flex items-center gap-2 text-sm">
              <Building className="w-4 h-4 text-muted-foreground" />
              Nom du centre d'appels <span className="text-destructive">*</span>
            </Label>
            <Input
              placeholder="Ex: Call Center Tunis"
              value={formData.company}
              onChange={(e) => handleChange("company", e.target.value)}
              className={errors.company ? "border-destructive" : ""}
            />
            {errors.company && <p className="text-xs text-destructive">{errors.company}</p>}
          </div>

          <div className="space-y-1.5">
            <Label className="flex items-center gap-2 text-sm">
              <Briefcase className="w-4 h-4 text-muted-foreground" />
              Votre poste <span className="text-destructive">*</span>
            </Label>
            <Select value={formData.position} onValueChange={(v) => handleChange("position", v)}>
              <SelectTrigger className={errors.position ? "border-destructive" : ""}>
                <SelectValue placeholder="Sélectionnez" />
              </SelectTrigger>
              <SelectContent>
                {positions.map((p) => (
                  <SelectItem key={p} value={p}>{p}</SelectItem>
                ))}
              </SelectContent>
            </Select>
            {errors.position && <p className="text-xs text-destructive">{errors.position}</p>}
          </div>

          <div className="space-y-1.5">
            <Label className="flex items-center gap-2 text-sm">
              <Users className="w-4 h-4 text-muted-foreground" />
              Taille du centre <span className="text-destructive">*</span>
            </Label>
            <Select value={formData.size} onValueChange={(v) => handleChange("size", v)}>
              <SelectTrigger className={errors.size ? "border-destructive" : ""}>
                <SelectValue placeholder="Sélectionnez" />
              </SelectTrigger>
              <SelectContent>
                {sizes.map((s) => (
                  <SelectItem key={s} value={s}>{s}</SelectItem>
                ))}
              </SelectContent>
            </Select>
            {errors.size && <p className="text-xs text-destructive">{errors.size}</p>}
          </div>

          <Button
            type="submit"
            className="w-full font-semibold"
            disabled={isSubmitting}
          >
            {isSubmitting ? "Envoi en cours..." : "Envoyer ma demande"}
          </Button>

          <p className="text-xs text-muted-foreground text-center">
            Nous ne partageons jamais vos données. Un email de confirmation vous sera envoyé.
          </p>
        </form>
      </DialogContent>
    </Dialog>
  );
};

export default AccessRequestModal;
