import { CheckCircle2, Phone, Target, TrendingUp, X, Building, User, MapPin, Briefcase, Mail, MessageSquare, FileText } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { useState } from "react";
import { motion } from "framer-motion";
import { useScrollAnimation } from "@/hooks/useScrollAnimation";
import { useTranslation } from "react-i18next";

const RecruiterSection = () => {
  const { ref, isVisible } = useScrollAnimation();
  const { t } = useTranslation('hero');
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState({
    company: "", firstName: "", lastName: "", position: "",
    city: "", phone: "", email: "", subject: "", message: ""
  });

  const tunisianCities = [
    { value: "tunis", label: "Tunis" }, { value: "ariana", label: "Ariana" },
    { value: "ben_arous", label: "Ben Arous" }, { value: "manouba", label: "Manouba" },
    { value: "nabeul", label: "Nabeul" }, { value: "zaghouan", label: "Zaghouan" },
    { value: "bizerte", label: "Bizerte" }, { value: "beja", label: "Béja" },
    { value: "jendouba", label: "Jendouba" }, { value: "kef", label: "Le Kef" },
    { value: "siliana", label: "Siliana" }, { value: "kairouan", label: "Kairouan" },
    { value: "kasserine", label: "Kasserine" }, { value: "sidi_bouzid", label: "Sidi Bouzid" },
    { value: "sousse", label: "Sousse" }, { value: "monastir", label: "Monastir" },
    { value: "mahdia", label: "Mahdia" }, { value: "sfax", label: "Sfax" },
    { value: "gabes", label: "Gabès" }, { value: "medenine", label: "Médenine" },
    { value: "tataouine", label: "Tataouine" }, { value: "gafsa", label: "Gafsa" },
    { value: "tozeur", label: "Tozeur" }, { value: "kebili", label: "Kébili" },
  ];

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log("Form submitted:", formData);
    setShowForm(false);
  };

  const handleInputChange = (field: string, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  return (
    <motion.section
      ref={ref}
      id="recruiter-section"
      className="relative py-20 lg:py-32 overflow-hidden"
      initial={{ opacity: 0 }}
      animate={isVisible ? { opacity: 1 } : { opacity: 0 }}
      transition={{ duration: 0.8 }}
    >
      <div className="container mx-auto px-4 lg:px-8 max-w-6xl relative z-10">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-12 lg:gap-16 items-center">
          <motion.div
            className="space-y-8 order-2 lg:order-1"
            initial={{ opacity: 0, x: -50 }}
            animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: -50 }}
            transition={{ duration: 0.8, delay: 0.2 }}
          >
            <div>
              <h2 className="text-3xl sm:text-4xl lg:text-5xl font-bold text-foreground mb-6 leading-tight">
                {t('recruiter.title').split('<1>').map((part, i) => 
                  i === 0 ? part : <span key={i}><span className="text-primary">{part.split('</1>')[0]}</span>{part.split('</1>')[1]}</span>
                )}
              </h2>
              <p className="text-lg text-muted-foreground leading-relaxed">
                {t('recruiter.description')}{" "}
                <button onClick={() => setShowForm(true)} className="text-primary hover:underline font-semibold cursor-pointer">
                  {t('recruiter.contactUs')}
                </button>{" "}
                {t('recruiter.descriptionEnd')}
              </p>
            </div>

            <div className="space-y-6">
              <div className="flex items-start space-x-4">
                <div className="flex-shrink-0 w-6 h-6 bg-primary rounded-full flex items-center justify-center mt-1">
                  <CheckCircle2 className="w-4 h-4 text-primary-foreground" />
                </div>
                <div>
                  <h3 className="text-xl font-semibold text-foreground mb-2">{t('recruiter.smartSourcing')}</h3>
                  <p className="text-muted-foreground">{t('recruiter.smartSourcingDesc')}</p>
                </div>
              </div>
            </div>

            <Button
              size="lg"
              className="bg-primary hover:bg-primary/90 text-primary-foreground px-8 py-3 shadow-lg hover:shadow-xl transition-all duration-300 transform hover:scale-[1.02] relative overflow-hidden mx-auto block sm:mx-0 sm:inline-flex"
              onClick={() => setShowForm(true)}
            >
              <span className="absolute inset-0 bg-gradient-to-r from-transparent via-white/20 to-transparent transform -skew-x-12 animate-slide"></span>
              <span className="relative flex items-center justify-center gap-2">
                <FileText className="w-5 h-5" />
                {t('recruiter.iRecruit')}
              </span>
            </Button>
          </motion.div>

          <motion.div
            className="relative order-1 lg:order-2"
            initial={{ opacity: 0, x: 50 }}
            animate={isVisible ? { opacity: 1, x: 0 } : { opacity: 0, x: 50 }}
            transition={{ duration: 0.8, delay: 0.3 }}
          >
            <div className="relative">
              <div className="absolute -top-6 left-12 w-3 h-3 bg-blue-400 rounded-full animate-[float_4s_ease-in-out_infinite] opacity-80"></div>
              <div className="absolute top-16 -left-4 w-2 h-2 bg-blue-500 rounded-full animate-[float_3s_ease-in-out_infinite_1s] opacity-60"></div>
              <div className="absolute top-1/4 right-12 w-4 h-4 bg-blue-300 rounded-full animate-[float_5s_ease-in-out_infinite_2s] opacity-70"></div>
              <div className="absolute bottom-24 left-6 w-2.5 h-2.5 bg-blue-600 rounded-full animate-[float_3.5s_ease-in-out_infinite_0.5s] opacity-80"></div>
              <div className="absolute -bottom-2 right-20 w-3 h-3 bg-blue-400 rounded-full animate-[float_4.5s_ease-in-out_infinite_1.5s] opacity-60"></div>
              <div className="absolute top-2/3 -right-2 w-2 h-2 bg-blue-500 rounded-full animate-[float_3s_ease-in-out_infinite_2.5s] opacity-70"></div>

              <div className="w-full h-80 lg:h-[500px] rounded-2xl overflow-hidden">
                <img src="/recruiter-new.png" alt="Recruiter" className="w-full h-full object-contain" />
              </div>

              <div className="absolute -top-2 -right-2 lg:-top-4 lg:-right-4 bg-card border border-border rounded-xl p-2 lg:p-4 shadow-lg animate-pulse">
                <div className="flex items-center space-x-2">
                  <TrendingUp className="w-4 h-4 lg:w-5 lg:h-5 text-primary" />
                  <div>
                    <p className="text-xs lg:text-sm font-semibold text-foreground">+250%</p>
                    <p className="text-xs text-muted-foreground">{t('recruiter.efficiency')}</p>
                  </div>
                </div>
              </div>

              <div className="absolute top-1/4 -left-4 lg:-left-6 bg-primary rounded-full p-2 lg:p-3 shadow-lg animate-bounce">
                <Phone className="w-5 h-5 lg:w-6 lg:h-6 text-primary-foreground" />
              </div>

              <div className="absolute bottom-6 -left-4 lg:bottom-8 lg:-left-8 bg-secondary rounded-full p-2 lg:p-4 shadow-lg">
                <Target className="w-6 h-6 lg:w-8 lg:h-8 text-secondary-foreground" />
              </div>

              <div className="absolute -bottom-4 -right-3 lg:-bottom-8 lg:-right-6 bg-card border border-border rounded-lg p-2 lg:p-3 shadow-lg">
                <div className="space-y-2">
                  <div className="w-16 lg:w-20 h-2 bg-primary rounded"></div>
                  <div className="w-12 lg:w-16 h-2 bg-muted rounded"></div>
                  <div className="w-8 lg:w-12 h-2 bg-muted rounded"></div>
                </div>
              </div>

              <div className="absolute top-16 right-16 w-3 h-3 bg-primary rounded-full animate-ping"></div>
              <div className="absolute bottom-20 left-8 w-2 h-2 bg-secondary rounded-full animate-pulse"></div>
            </div>
          </motion.div>
        </div>
      </div>

      {/* Contact Form Modal */}
      {showForm && (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-sm z-[100] flex items-center justify-center p-4 animate-fade-in">
          <div className="bg-card rounded-2xl shadow-2xl max-w-2xl w-full max-h-[90vh] overflow-y-auto animate-scale-in">
            <Card className="border-0">
              <CardHeader className="relative">
                <button onClick={() => setShowForm(false)} className="absolute right-4 top-4 p-2 hover:bg-muted rounded-full transition-colors">
                  <X className="w-5 h-5" />
                </button>
                <CardTitle className="text-2xl font-bold text-center text-foreground pr-12">{t('recruiter.formTitle')}</CardTitle>
                <p className="text-center text-muted-foreground">{t('recruiter.formSubtitle')}</p>
              </CardHeader>
              <CardContent className="space-y-6">
                <form onSubmit={handleSubmit} className="space-y-6">
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div className="space-y-2 md:col-span-2">
                      <Label htmlFor="company" className="flex items-center gap-2 text-foreground">
                        <Building size={16} /> {t('recruiter.companyName')} <span className="text-destructive">*</span>
                      </Label>
                      <Input id="company" type="text" placeholder={t('recruiter.companyPlaceholder')} value={formData.company} onChange={(e) => handleInputChange("company", e.target.value)} className="h-12 border-border focus:border-primary focus:ring-primary" required />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="firstName" className="flex items-center gap-2 text-foreground">
                        <User size={16} /> {t('recruiter.firstName')} <span className="text-destructive">*</span>
                      </Label>
                      <Input id="firstName" type="text" placeholder={t('recruiter.firstNamePlaceholder')} value={formData.firstName} onChange={(e) => handleInputChange("firstName", e.target.value)} className="h-12 border-border focus:border-primary focus:ring-primary" required />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="lastName" className="flex items-center gap-2 text-foreground">
                        <User size={16} /> {t('recruiter.lastName')} <span className="text-destructive">*</span>
                      </Label>
                      <Input id="lastName" type="text" placeholder={t('recruiter.lastNamePlaceholder')} value={formData.lastName} onChange={(e) => handleInputChange("lastName", e.target.value)} className="h-12 border-border focus:border-primary focus:ring-primary" required />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="position" className="flex items-center gap-2 text-foreground">
                        <Briefcase size={16} /> {t('recruiter.position')} <span className="text-destructive">*</span>
                      </Label>
                      <Select value={formData.position} onValueChange={(value) => handleInputChange("position", value)} required>
                        <SelectTrigger className="h-12 border-border focus:border-primary focus:ring-primary">
                          <SelectValue placeholder={t('recruiter.positionPlaceholder')} />
                        </SelectTrigger>
                        <SelectContent className="bg-popover">
                          <SelectItem value="directeur-general">{t('recruiter.positionDG')}</SelectItem>
                          <SelectItem value="directeur-rh">{t('recruiter.positionDRH')}</SelectItem>
                          <SelectItem value="responsable-recrutement">{t('recruiter.positionRR')}</SelectItem>
                          <SelectItem value="charge-recrutement">{t('recruiter.positionCR')}</SelectItem>
                          <SelectItem value="responsable-operations">{t('recruiter.positionRO')}</SelectItem>
                          <SelectItem value="autre">{t('recruiter.positionOther')}</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="city" className="flex items-center gap-2 text-foreground">
                        <MapPin size={16} /> {t('recruiter.city')} <span className="text-destructive">*</span>
                      </Label>
                      <Select value={formData.city} onValueChange={value => setFormData(prev => ({ ...prev, city: value }))}>
                        <SelectTrigger className={`h-10 sm:h-12 border-2 transition-all duration-300 input-glow text-xs sm:text-sm ${formData.city ? "border-primary/50 bg-gradient-to-br from-primary/10 to-primary/5 shadow-md" : "hover:border-primary/40 hover:bg-primary/5"}`}>
                          <SelectValue placeholder={t('recruiter.cityPlaceholder')} />
                        </SelectTrigger>
                        <SelectContent sideOffset={8} className="bg-white dark:bg-gray-800 border-2 shadow-2xl max-h-[400px]" align="start">
                          {tunisianCities.map((city) => (
                            <SelectItem key={city.value} value={city.value}>{city.label}</SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="phone" className="flex items-center gap-2 text-foreground">
                        <Phone size={16} /> {t('recruiter.phone')} <span className="text-destructive">*</span>
                      </Label>
                      <Input id="phone" type="tel" placeholder={t('recruiter.phonePlaceholder')} value={formData.phone} onChange={(e) => handleInputChange("phone", e.target.value)} className="h-12 border-border focus:border-primary focus:ring-primary" required />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="email" className="flex items-center gap-2 text-foreground">
                        <Mail size={16} /> {t('recruiter.email')} <span className="text-destructive">*</span>
                      </Label>
                      <Input id="email" type="email" placeholder={t('recruiter.emailPlaceholder')} value={formData.email} onChange={(e) => handleInputChange("email", e.target.value)} className="h-12 border-border focus:border-primary focus:ring-primary" required />
                    </div>

                    <div className="space-y-2 md:col-span-2">
                      <Label htmlFor="subject" className="flex items-center gap-2 text-foreground">
                        <MessageSquare size={16} /> {t('recruiter.subject')} <span className="text-destructive">*</span>
                      </Label>
                      <Select value={formData.subject} onValueChange={(value) => handleInputChange("subject", value)} required>
                        <SelectTrigger className="h-12 border-border focus:border-primary focus:ring-primary">
                          <SelectValue placeholder={t('recruiter.subjectPlaceholder')} />
                        </SelectTrigger>
                        <SelectContent className="bg-popover">
                          <SelectItem value="informations">{t('recruiter.subjectInfo')}</SelectItem>
                          <SelectItem value="demo">{t('recruiter.subjectDemo')}</SelectItem>
                          <SelectItem value="documentation">{t('recruiter.subjectDoc')}</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>

                    <div className="space-y-2 md:col-span-2">
                      <Label htmlFor="message" className="flex items-center gap-2 text-foreground">
                        <MessageSquare size={16} /> {t('recruiter.message')} <span className="text-destructive">*</span>
                      </Label>
                      <Textarea id="message" placeholder={t('recruiter.messagePlaceholder')} value={formData.message} onChange={(e) => handleInputChange("message", e.target.value)} className="min-h-[120px] border-border focus:border-primary focus:ring-primary resize-none" required />
                    </div>
                  </div>

                  <div className="flex gap-4 pt-4">
                    <Button type="button" variant="outline" className="flex-1" onClick={() => setShowForm(false)}>
                      {t('recruiter.cancel')}
                    </Button>
                    <Button type="submit" className="flex-1 bg-primary hover:bg-primary/90 text-primary-foreground shadow-lg hover:shadow-xl transition-all duration-300 transform hover:scale-[1.02] relative overflow-hidden">
                      <span className="absolute inset-0 bg-gradient-to-r from-transparent via-white/20 to-transparent transform -skew-x-12 animate-slide"></span>
                      <span className="relative">
                        <span className="sm:hidden">{t('recruiter.send')}</span>
                        <span className="hidden sm:inline">{t('recruiter.sendFull')}</span>
                      </span>
                    </Button>
                  </div>
                </form>
              </CardContent>
            </Card>
          </div>
        </div>
      )}
    </motion.section>
  );
};

export default RecruiterSection;
