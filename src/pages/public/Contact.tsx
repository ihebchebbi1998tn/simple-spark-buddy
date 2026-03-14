import { useState } from 'react';
import { motion } from 'framer-motion';
import { useNavigate } from 'react-router-dom';
import { Mail, Phone, MapPin, Send, CheckCircle, Building, User, Briefcase, MessageSquare, ArrowLeft } from 'lucide-react';
import Header from "@/components/shared/Header";
import Footer from "@/components/shared/Footer";
import ParticleBackground from "@/components/shared/ParticleBackground";

import { useScrollAnimation } from '@/hooks/useScrollAnimation';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { useToast } from '@/hooks/use-toast';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { useTranslation } from 'react-i18next';

const Contact = () => {
  const navigate = useNavigate();
  const { ref, isVisible } = useScrollAnimation();
  const { toast } = useToast();
  const { t } = useTranslation(['contact', 'common']);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [formData, setFormData] = useState({
    company: '',
    firstName: '',
    lastName: '',
    position: '',
    city: '',
    phone: '',
    email: '',
    subject: '',
    message: ''
  });

  const tunisianCities = [
    { value: "tunis", label: "Tunis" },
    { value: "ariana", label: "Ariana" },
    { value: "ben_arous", label: "Ben Arous" },
    { value: "manouba", label: "Manouba" },
    { value: "nabeul", label: "Nabeul" },
    { value: "zaghouan", label: "Zaghouan" },
    { value: "bizerte", label: "Bizerte" },
    { value: "beja", label: "Béja" },
    { value: "jendouba", label: "Jendouba" },
    { value: "kef", label: "Le Kef" },
    { value: "siliana", label: "Siliana" },
    { value: "kairouan", label: "Kairouan" },
    { value: "kasserine", label: "Kasserine" },
    { value: "sidi_bouzid", label: "Sidi Bouzid" },
    { value: "sousse", label: "Sousse" },
    { value: "monastir", label: "Monastir" },
    { value: "mahdia", label: "Mahdia" },
    { value: "sfax", label: "Sfax" },
    { value: "gabes", label: "Gabès" },
    { value: "medenine", label: "Médenine" },
    { value: "tataouine", label: "Tataouine" },
    { value: "gafsa", label: "Gafsa" },
    { value: "tozeur", label: "Tozeur" },
    { value: "kebili", label: "Kébili" },
  ];

  const handleInputChange = (
    eOrName: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement> | string,
    value?: string
  ) => {
    if (typeof eOrName === 'string' && value !== undefined) {
      setFormData(prev => ({ ...prev, [eOrName]: value }));
    } else if (typeof eOrName === 'object' && 'target' in eOrName) {
      const { name, value } = eOrName.target;
      setFormData(prev => ({ ...prev, [name]: value }));
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);

    setTimeout(() => {
      setIsSubmitting(false);
      toast({
        title: t('contact:successTitle'),
        description: t('contact:successMsg'),
      });
      setFormData({ company: '', firstName: '', lastName: '', position: '', city: '', phone: '', email: '', subject: '', message: '' });
    }, 2000);
  };

  return (
    <div className="min-h-screen relative bg-background">
      <ParticleBackground />
      <div className="relative z-10">
        <Header />

        <div className="container mx-auto px-4 lg:px-8 max-w-6xl pt-6">
          <Button
            variant="ghost"
            onClick={() => navigate('/')}
            className="flex items-center gap-2 text-muted-foreground hover:text-foreground transition-colors"
          >
            <ArrowLeft className="h-4 w-4" />
            {t('common:backToHome')}
          </Button>
        </div>

        <main>
          <motion.section
            ref={ref}
            className="py-12 bg-gradient-subtle"
            initial={{ opacity: 0 }}
            animate={isVisible ? { opacity: 1 } : { opacity: 0 }}
            transition={{ duration: 0.8 }}
          >
            <div className="container mx-auto px-4 lg:px-8 max-w-6xl">
              <div className="grid grid-cols-1 lg:grid-cols-2 gap-12 items-start">
                {/* Left Column */}
                <motion.div
                  initial={{ opacity: 0, x: -30 }}
                  animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: -30 }}
                  transition={{ duration: 0.6, delay: 0.2 }}
                >
                  <h2 className="text-3xl lg:text-4xl font-bold text-foreground mb-6">
                    {t('contact:title')}
                  </h2>
                  <p className="text-lg text-muted-foreground mb-8">
                    {t('contact:subtitle')}
                  </p>

                  <div className="space-y-4">
                    <div className="flex items-start gap-3">
                      <CheckCircle className="h-5 w-5 text-primary mt-1 flex-shrink-0" />
                      <div>
                        <h4 className="font-semibold text-foreground">{t('contact:guaranteedResponse')}</h4>
                        <p className="text-sm text-muted-foreground">{t('contact:guaranteedResponseDesc')}</p>
                      </div>
                    </div>
                    <div className="flex items-start gap-3">
                      <CheckCircle className="h-5 w-5 text-primary mt-1 flex-shrink-0" />
                      <div>
                        <h4 className="font-semibold text-foreground">{t('contact:personalizedAdvice')}</h4>
                        <p className="text-sm text-muted-foreground">{t('contact:personalizedAdviceDesc')}</p>
                      </div>
                    </div>
                    <div className="flex items-start gap-3">
                      <CheckCircle className="h-5 w-5 text-primary mt-1 flex-shrink-0" />
                      <div>
                        <h4 className="font-semibold text-foreground">{t('contact:fullSupport')}</h4>
                        <p className="text-sm text-muted-foreground">{t('contact:fullSupportDesc')}</p>
                      </div>
                    </div>
                  </div>
                </motion.div>

                {/* Right Column - Form */}
                <motion.div
                  className="bg-card border border-border rounded-xl p-8 shadow-lg"
                  initial={{ opacity: 0, x: 30 }}
                  animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: 30 }}
                  transition={{ duration: 0.6, delay: 0.4 }}
                >
                  <h3 className="text-xl font-semibold text-foreground mb-6">
                    {t('contact:formTitle')}
                  </h3>

                  <form onSubmit={handleSubmit} className="space-y-5">
                    <div>
                      <label htmlFor="company" className="flex items-center gap-2 text-sm font-medium text-foreground mb-2">
                        <Building size={16} />
                        {t('contact:company')} <span className="text-destructive">*</span>
                      </label>
                      <Input id="company" name="company" type="text" required value={formData.company} onChange={handleInputChange} className="w-full h-12" />
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                      <div>
                        <label htmlFor="firstName" className="flex items-center gap-2 text-sm font-medium text-foreground mb-2">
                          <User size={16} />
                          {t('contact:firstName')} <span className="text-destructive">*</span>
                        </label>
                        <Input id="firstName" name="firstName" type="text" required value={formData.firstName} onChange={handleInputChange} className="w-full h-12" />
                      </div>
                      <div>
                        <label htmlFor="lastName" className="flex items-center gap-2 text-sm font-medium text-foreground mb-2">
                          <User size={16} />
                          {t('contact:lastName')} <span className="text-destructive">*</span>
                        </label>
                        <Input id="lastName" name="lastName" type="text" required value={formData.lastName} onChange={handleInputChange} className="w-full h-12" />
                      </div>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                      <div>
                        <label htmlFor="position" className="flex items-center gap-2 text-sm font-medium text-foreground mb-2">
                          <Briefcase size={16} />
                          {t('contact:position')} <span className="text-destructive">*</span>
                        </label>
                        <Select value={formData.position} onValueChange={value => setFormData(prev => ({ ...prev, position: value }))} required>
                          <SelectTrigger className="h-12 w-full"><SelectValue /></SelectTrigger>
                          <SelectContent className="bg-popover">
                            <SelectItem value="directeur-general">Directeur Général</SelectItem>
                            <SelectItem value="directeur-rh">Directeur RH</SelectItem>
                            <SelectItem value="responsable-recrutement">Responsable Recrutement</SelectItem>
                            <SelectItem value="charge-recrutement">Chargé(e) de Recrutement</SelectItem>
                            <SelectItem value="responsable-operations">Responsable Opérations</SelectItem>
                            <SelectItem value="autre">Autre</SelectItem>
                          </SelectContent>
                        </Select>
                      </div>
                      <div>
                        <label htmlFor="city" className="flex items-center gap-2 text-sm font-medium text-foreground mb-2">
                          <MapPin size={16} />
                          {t('contact:city')} <span className="text-destructive">*</span>
                        </label>
                        <Select value={formData.city} onValueChange={value => setFormData(prev => ({ ...prev, city: value }))}>
                          <SelectTrigger className={`h-10 sm:h-12 border-2 transition-all duration-300 text-xs sm:text-sm ${formData.city ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md" : "hover:border-primary/40 hover:bg-primary/5"}`}>
                            <SelectValue />
                          </SelectTrigger>
                          <SelectContent sideOffset={8} className="bg-popover border-2 shadow-2xl max-h-[400px]" align="start">
                            {tunisianCities.map((city) => (
                              <SelectItem key={city.value} value={city.value}>{city.label}</SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                      </div>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                      <div>
                        <label htmlFor="phone" className="flex items-center gap-2 text-sm font-medium text-foreground mb-2">
                          <Phone size={16} />
                          {t('contact:phone')} <span className="text-destructive">*</span>
                        </label>
                        <Input id="phone" name="phone" type="tel" required value={formData.phone} onChange={handleInputChange} placeholder="+216 12345678" className="w-full h-12" />
                      </div>
                      <div>
                        <label htmlFor="email" className="flex items-center gap-2 text-sm font-medium text-foreground mb-2">
                          <Mail size={16} />
                          {t('contact:email')} <span className="text-destructive">*</span>
                        </label>
                        <Input id="email" name="email" type="email" required value={formData.email} onChange={handleInputChange} className="w-full h-12" />
                      </div>
                    </div>

                    <div>
                      <label htmlFor="subject" className="flex items-center gap-2 text-sm font-medium text-foreground mb-2">
                        <MessageSquare size={16} />
                        {t('contact:subject')} <span className="text-destructive">*</span>
                      </label>
                      <Select value={formData.subject} onValueChange={(value) => handleInputChange("subject", value)} required>
                        <SelectTrigger className="h-12 border-border focus:border-primary focus:ring-primary"><SelectValue /></SelectTrigger>
                        <SelectContent className="bg-popover">
                          <SelectItem value="informations">Demande d'informations</SelectItem>
                          <SelectItem value="demo">Demande de demo</SelectItem>
                          <SelectItem value="documentation">Demande de documentation</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>

                    <div>
                      <label htmlFor="message" className="flex items-center gap-2 text-sm font-medium text-foreground mb-2">
                        <MessageSquare size={16} />
                        {t('contact:message')} <span className="text-destructive">*</span>
                      </label>
                      <Textarea id="message" name="message" required value={formData.message} onChange={handleInputChange} className="w-full min-h-[120px] resize-none" />
                    </div>

                    <Button
                      type="submit"
                      disabled={isSubmitting}
                      className="w-full bg-primary text-primary-foreground h-12 font-semibold hover:bg-primary/90 transition-colors duration-200 shadow-md hover:shadow-lg disabled:opacity-50"
                    >
                      {isSubmitting ? (
                        <div className="flex items-center gap-2">
                          <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white"></div>
                          {t('contact:sending')}
                        </div>
                      ) : (
                        <div className="flex items-center gap-2">
                          <Send className="h-4 w-4" />
                          {t('contact:sendBtn')}
                        </div>
                      )}
                    </Button>
                  </form>
                </motion.div>
              </div>
            </div>
          </motion.section>
        </main>

        <Footer />
      </div>
    </div>
  );
};

export default Contact;
