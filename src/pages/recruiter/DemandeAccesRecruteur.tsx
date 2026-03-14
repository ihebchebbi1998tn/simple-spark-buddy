import { useState } from "react";
import { motion } from "framer-motion";
import { useNavigate } from "react-router-dom";
import Header from "@/components/shared/Header";
import Footer from "@/components/shared/Footer";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { useToast } from "@/hooks/use-toast";
import { Building, User, Briefcase, MapPin, Phone, Mail, MessageSquare, CheckCircle2, Shield, Zap, Target, Send } from "lucide-react";
import { useTranslation } from "react-i18next";

const tunisianCities = [
  "Tunis", "Ariana", "Ben Arous", "Manouba", "Nabeul", "Zaghouan",
  "Bizerte", "Béja", "Jendouba", "Le Kef", "Siliana", "Kairouan",
  "Kasserine", "Sidi Bouzid", "Sousse", "Monastir", "Mahdia", "Sfax",
  "Gabès", "Médenine", "Tataouine", "Gafsa", "Tozeur", "Kébili",
];

const positions = [
  "Directeur Général", "Directeur RH", "Responsable Recrutement",
  "Chargé(e) de Recrutement", "Responsable Opérations", "Autre",
];

const advIcons = [Target, Zap, Shield, CheckCircle2];

const DemandeAccesRecruteur = () => {
  const navigate = useNavigate();
  const { toast } = useToast();
  const { t } = useTranslation('accessRequest');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [formData, setFormData] = useState({
    company: "", firstName: "", lastName: "", position: "",
    city: "", phone: "", email: "", companySize: "", message: "",
  });

  const advantages = t('advantages', { returnObjects: true }) as { title: string; desc: string }[];
  const sizes = t('sizes', { returnObjects: true }) as string[];

  const handleChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    setTimeout(() => {
      setIsSubmitting(false);
      toast({ title: t('successTitle'), description: t('successMsg') });
      setFormData({ company: "", firstName: "", lastName: "", position: "", city: "", phone: "", email: "", companySize: "", message: "" });
    }, 1200);
  };

  return (
    <div className="min-h-screen bg-background">
      <Header />
      <main className="pt-20 pb-16">
        <section className="bg-primary py-12 sm:py-16">
          <div className="container mx-auto px-4 text-center max-w-3xl">
            <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.6 }}>
              <Badge className="bg-primary-foreground/20 text-primary-foreground border-primary-foreground/30 mb-4">
                <Building className="w-3 h-3 mr-1" />
                {t('badge')}
              </Badge>
              <h1 className="text-2xl sm:text-3xl lg:text-4xl font-bold text-primary-foreground mb-4">{t('title')}</h1>
              <p className="text-primary-foreground/80 text-sm sm:text-base max-w-2xl mx-auto">{t('subtitle')}</p>
            </motion.div>
          </div>
        </section>

        <div className="container mx-auto px-4 max-w-5xl -mt-8">
          <div className="grid lg:grid-cols-5 gap-8">
            <motion.div className="lg:col-span-3" initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.6, delay: 0.2 }}>
              <Card className="border shadow-lg">
                <CardHeader className="border-b bg-muted/30">
                  <CardTitle className="text-lg flex items-center gap-2">
                    <Send className="w-5 h-5 text-primary" />
                    {t('formTitle')}
                  </CardTitle>
                </CardHeader>
                <CardContent className="p-6">
                  <form onSubmit={handleSubmit} className="space-y-5">
                    <div className="space-y-1.5">
                      <Label className="flex items-center gap-2 text-sm"><Building className="w-4 h-4 text-muted-foreground" />{t('company')} <span className="text-destructive">*</span></Label>
                      <Input placeholder="Ex: TechCall Center" value={formData.company} onChange={(e) => handleChange("company", e.target.value)} required />
                    </div>
                    <div className="grid sm:grid-cols-2 gap-4">
                      <div className="space-y-1.5">
                        <Label className="flex items-center gap-2 text-sm"><User className="w-4 h-4 text-muted-foreground" />{t('firstName')} <span className="text-destructive">*</span></Label>
                        <Input value={formData.firstName} onChange={(e) => handleChange("firstName", e.target.value)} required />
                      </div>
                      <div className="space-y-1.5">
                        <Label className="flex items-center gap-2 text-sm"><User className="w-4 h-4 text-muted-foreground" />{t('lastName')} <span className="text-destructive">*</span></Label>
                        <Input value={formData.lastName} onChange={(e) => handleChange("lastName", e.target.value)} required />
                      </div>
                    </div>
                    <div className="grid sm:grid-cols-2 gap-4">
                      <div className="space-y-1.5">
                        <Label className="flex items-center gap-2 text-sm"><Briefcase className="w-4 h-4 text-muted-foreground" />{t('position')} <span className="text-destructive">*</span></Label>
                        <Select value={formData.position} onValueChange={(v) => handleChange("position", v)}>
                          <SelectTrigger><SelectValue /></SelectTrigger>
                          <SelectContent>{positions.map((p) => <SelectItem key={p} value={p}>{p}</SelectItem>)}</SelectContent>
                        </Select>
                      </div>
                      <div className="space-y-1.5">
                        <Label className="flex items-center gap-2 text-sm"><MapPin className="w-4 h-4 text-muted-foreground" />{t('city')} <span className="text-destructive">*</span></Label>
                        <Select value={formData.city} onValueChange={(v) => handleChange("city", v)}>
                          <SelectTrigger><SelectValue /></SelectTrigger>
                          <SelectContent>{tunisianCities.map((c) => <SelectItem key={c} value={c}>{c}</SelectItem>)}</SelectContent>
                        </Select>
                      </div>
                    </div>
                    <div className="grid sm:grid-cols-2 gap-4">
                      <div className="space-y-1.5">
                        <Label className="flex items-center gap-2 text-sm"><Phone className="w-4 h-4 text-muted-foreground" />{t('phone')} <span className="text-destructive">*</span></Label>
                        <Input type="tel" placeholder="+216 XX XXX XXX" value={formData.phone} onChange={(e) => handleChange("phone", e.target.value)} required />
                      </div>
                      <div className="space-y-1.5">
                        <Label className="flex items-center gap-2 text-sm"><Mail className="w-4 h-4 text-muted-foreground" />{t('email')} <span className="text-destructive">*</span></Label>
                        <Input type="email" placeholder="email@societe.com" value={formData.email} onChange={(e) => handleChange("email", e.target.value)} required />
                      </div>
                    </div>
                    <div className="space-y-1.5">
                      <Label className="text-sm">{t('companySize')} <span className="text-destructive">*</span></Label>
                      <Select value={formData.companySize} onValueChange={(v) => handleChange("companySize", v)}>
                        <SelectTrigger><SelectValue placeholder={t('companySizePlaceholder')} /></SelectTrigger>
                        <SelectContent>{sizes.map((s) => <SelectItem key={s} value={s}>{s}</SelectItem>)}</SelectContent>
                      </Select>
                    </div>
                    <div className="space-y-1.5">
                      <Label className="flex items-center gap-2 text-sm"><MessageSquare className="w-4 h-4 text-muted-foreground" />{t('message')}</Label>
                      <Textarea placeholder={t('messagePlaceholder')} value={formData.message} onChange={(e) => handleChange("message", e.target.value)} className="min-h-[100px] resize-none" />
                    </div>
                    <div className="flex gap-3 pt-2">
                      <Button type="button" variant="outline" className="flex-1" onClick={() => navigate("/")}>{t('cancel')}</Button>
                      <Button type="submit" className="flex-1 font-semibold" disabled={isSubmitting}>
                        {isSubmitting ? t('submitting') : t('submit')}
                      </Button>
                    </div>
                  </form>
                </CardContent>
              </Card>
            </motion.div>

            <motion.div className="lg:col-span-2 space-y-4" initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.6, delay: 0.4 }}>
              <Card className="border shadow-lg">
                <CardHeader><CardTitle className="text-lg">{t('whyJoin')}</CardTitle></CardHeader>
                <CardContent className="space-y-5">
                  {advantages.map((adv, i) => {
                    const Icon = advIcons[i] || Target;
                    return (
                      <div key={i} className="flex gap-3">
                        <div className="w-10 h-10 rounded-lg bg-primary/10 flex items-center justify-center shrink-0">
                          <Icon className="w-5 h-5 text-primary" />
                        </div>
                        <div>
                          <h4 className="font-semibold text-foreground text-sm">{adv.title}</h4>
                          <p className="text-muted-foreground text-xs mt-0.5">{adv.desc}</p>
                        </div>
                      </div>
                    );
                  })}
                </CardContent>
              </Card>

              <Card className="border bg-primary/5">
                <CardContent className="p-5 text-center">
                  <p className="text-sm text-muted-foreground mb-3">{t('alreadyAccount')}</p>
                  <Button variant="outline" className="w-full border-primary text-primary hover:bg-primary hover:text-primary-foreground"
                    onClick={() => navigate("/recruteurs/login")}>{t('login')}</Button>
                </CardContent>
              </Card>

              <Card className="border">
                <CardContent className="p-5 text-center">
                  <p className="text-sm text-muted-foreground mb-3">{t('trySimulator')}</p>
                  <Button variant="outline" className="w-full" onClick={() => navigate("/entreprises")}>{t('accessSimulator')}</Button>
                </CardContent>
              </Card>
            </motion.div>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default DemandeAccesRecruteur;
