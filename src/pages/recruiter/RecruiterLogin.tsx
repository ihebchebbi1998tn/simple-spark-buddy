import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { motion } from "framer-motion";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { Separator } from "@/components/ui/separator";
import { Eye, EyeOff, Mail, Lock, Building2, ArrowLeft, CheckCircle, Users, Target, Briefcase } from "lucide-react";
import { useToast } from "@/hooks/use-toast";
import ParticleBackground from "@/components/shared/ParticleBackground";
import { recruiterAuthService } from "@/services/recruiterAuthService";

const RecruiterLogin = () => {
  const navigate = useNavigate();
  const { toast } = useToast();

  const [formData, setFormData] = useState({
    email: "",
    password: ""
  });
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState<{ [key: string]: string }>({});

  const handleInputChange = (field: string, value: string) => {
    setFormData(prev => ({
      ...prev,
      [field]: value
    }));
    // Clear errors when user starts typing
    if (errors[field]) {
      setErrors(prev => ({
        ...prev,
        [field]: ""
      }));
    }
  };

  const validateForm = () => {
    const newErrors: { [key: string]: string } = {};

    if (!formData.email) {
      newErrors.email = "L'email est obligatoire";
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = "Veuillez entrer un email valide";
    }

    if (!formData.password) {
      newErrors.password = "Le mot de passe est obligatoire";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateForm()) return;

    setIsLoading(true);

    try {
      const result = await recruiterAuthService.login(formData.email, formData.password);

      if (result.success && result.user) {
        toast({
          title: "Connexion réussie ! 🎉",
          description: `Bienvenue ${result.user.name} !`,
        });
        navigate('/recruteurs');
      } else {
        toast({
          title: "Erreur de connexion",
          description: result.error || "Email ou mot de passe incorrect",
          variant: "destructive",
        });
      }
    } catch (error: any) {
      console.error("Login error:", error);
      toast({
        title: "Erreur de connexion",
        description: "Une erreur est survenue lors de la connexion.",
        variant: "destructive"
      });
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen relative overflow-hidden bg-gradient-to-br from-background via-background/95 to-primary/5">
      <ParticleBackground />

      {/* Header with back button */}
      <div className="relative z-10 bg-white/80 backdrop-blur-sm border-b border-border/50">
        <div className="container mx-auto px-4 lg:px-8 max-w-7xl">
          <div className="flex items-center justify-between h-16">
            <Link
              to="/"
              className="flex items-center gap-2 text-muted-foreground hover:text-foreground transition-colors group"
            >
              <ArrowLeft className="h-4 w-4 group-hover:-translate-x-1 transition-transform" />
              <span className="font-medium">Retour à l'accueil</span>
            </Link>

            <div className="flex items-center">
              <img
                src="/uploads/488f38af-4f42-45f8-a54f-4c1b46a2dfff.png"
                alt="Call Center Match"
                className="h-16 w-auto"
              />
            </div>
          </div>
        </div>
      </div>

      <div className="relative z-10 flex items-center justify-center min-h-[calc(100vh-4rem)] py-12 px-4">
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6 }}
          className="w-full max-w-md"
        >
          <Card className="bg-card/80 backdrop-blur-sm border-border/50 shadow-xl">
            <CardHeader className="space-y-4 text-center pb-6">
              <motion.div
                initial={{ scale: 0 }}
                animate={{ scale: 1 }}
                transition={{ delay: 0.2, type: "spring", stiffness: 200 }}
                className="mx-auto w-16 h-16 bg-gradient-to-br from-primary to-primary/80 rounded-full flex items-center justify-center"
              >
                <Building2 className="h-8 w-8 text-white" />
              </motion.div>

              <div>
                <CardTitle className="text-2xl font-bold text-foreground mb-2">
                  Espace Recruteurs
                </CardTitle>
                <p className="text-muted-foreground">
                  Connectez-vous pour accéder à votre espace de recrutement
                </p>
              </div>
            </CardHeader>

            <CardContent className="space-y-6">
              <form onSubmit={handleSubmit} className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="email" className="flex items-center gap-2 text-foreground font-medium">
                    <Mail className="h-4 w-4" />
                    Adresse email professionnelle
                  </Label>
                  <Input
                    id="email"
                    type="email"
                    placeholder="votre.email@entreprise.com"
                    value={formData.email}
                    onChange={(e) => handleInputChange("email", e.target.value)}
                    className={`h-12 transition-all ${errors.email ? 'border-destructive' : 'border-border focus:border-primary'}`}
                    disabled={isLoading}
                  />
                  {errors.email && (
                    <p className="text-sm text-destructive">{errors.email}</p>
                  )}
                </div>

                <div className="space-y-2">
                  <Label htmlFor="password" className="flex items-center gap-2 text-foreground font-medium">
                    <Lock className="h-4 w-4" />
                    Mot de passe
                  </Label>
                  <div className="relative">
                    <Input
                      id="password"
                      type={showPassword ? "text" : "password"}
                      placeholder="Votre mot de passe"
                      value={formData.password}
                      onChange={(e) => handleInputChange("password", e.target.value)}
                      className={`h-12 pr-12 transition-all ${errors.password ? 'border-destructive' : 'border-border focus:border-primary'}`}
                      disabled={isLoading}
                    />
                    <button
                      type="button"
                      onClick={() => setShowPassword(!showPassword)}
                      className="absolute right-3 top-1/2 transform -translate-y-1/2 text-muted-foreground hover:text-foreground transition-colors"
                      disabled={isLoading}
                    >
                      {showPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                    </button>
                  </div>
                  {errors.password && (
                    <p className="text-sm text-destructive">{errors.password}</p>
                  )}
                </div>

                <div className="flex items-center justify-between text-sm">
                  <label className="flex items-center space-x-2 cursor-pointer">
                    <input type="checkbox" className="rounded border-border" />
                    <span className="text-muted-foreground text-[11px] sm:text-sm">Se souvenir de moi</span>
                  </label>
                  <button
                    type="button"
                    className="text-primary hover:text-primary/80 font-medium transition-colors text-[11px] sm:text-sm"
                  >
                    Mot de passe oublié ?
                  </button>
                </div>

                <Button
                  type="submit"
                  className="w-full h-12 bg-gradient-to-r from-primary to-primary/90 hover:from-primary/90 hover:to-primary text-white font-semibold shadow-lg hover:shadow-xl transition-all duration-300 transform hover:scale-[1.02]"
                  disabled={isLoading}
                >
                  {isLoading ? (
                    <div className="flex items-center gap-2">
                      <div className="animate-spin rounded-full h-4 w-4 border-2 border-white border-t-transparent"></div>
                      Connexion en cours...
                    </div>
                  ) : (
                    <div className="flex items-center gap-2">
                      <CheckCircle className="h-4 w-4" />
                      Se connecter
                    </div>
                  )}
                </Button>
              </form>

              <div className="relative">
                <Separator />
                <div className="absolute inset-0 flex items-center justify-center">
                  <span className="bg-card px-4 text-sm text-muted-foreground">ou</span>
                </div>
              </div>

              <Alert className="border-primary/20 bg-primary/5">
                <AlertDescription className="text-center">
                  <span className="text-muted-foreground">Nouveau recruteur ? </span>
                  <Link
                    to="/contact"
                    className="text-primary hover:text-primary/80 font-semibold underline underline-offset-2 transition-colors"
                  >
                    Demander un accès
                  </Link>
                </AlertDescription>
              </Alert>

              {/* Demo accounts info */}
              <div className="mt-4 p-4 bg-muted/30 rounded-lg border border-border/50">
                <p className="text-xs font-medium text-muted-foreground mb-2 text-center">Comptes de démonstration :</p>
                <div className="space-y-1.5 text-xs text-muted-foreground">
                  <div className="flex justify-between items-center px-2 py-1 bg-purple-50 rounded">
                    <span className="font-medium text-purple-700">Super Admin</span>
                    <code className="text-[10px]">admin@callcenter.ma / admin123</code>
                  </div>
                  <div className="flex justify-between items-center px-2 py-1 bg-blue-50 rounded">
                    <span className="font-medium text-blue-700">Admin</span>
                    <code className="text-[10px]">manager@callcenter.ma / manager123</code>
                  </div>
                  <div className="flex justify-between items-center px-2 py-1 bg-green-50 rounded">
                    <span className="font-medium text-green-700">Chargé</span>
                    <code className="text-[10px]">recruteur@callcenter.ma / recruteur123</code>
                  </div>
                </div>
              </div>

            </CardContent>
          </Card>

          {/* Features below card */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: 0.4, duration: 0.6 }}
            className="mt-8 grid grid-cols-3 gap-4"
          >
            <div className="text-center">
              <div className="w-10 h-10 mx-auto mb-2 bg-primary/10 rounded-full flex items-center justify-center">
                <Users className="h-5 w-5 text-primary" />
              </div>
              <p className="text-xs text-muted-foreground">+10 000 candidats</p>
            </div>
            <div className="text-center">
              <div className="w-10 h-10 mx-auto mb-2 bg-primary/10 rounded-full flex items-center justify-center">
                <Target className="h-5 w-5 text-primary" />
              </div>
              <p className="text-xs text-muted-foreground">Matching précis</p>
            </div>
            <div className="text-center">
              <div className="w-10 h-10 mx-auto mb-2 bg-primary/10 rounded-full flex items-center justify-center">
                <Briefcase className="h-5 w-5 text-primary" />
              </div>
              <p className="text-xs text-muted-foreground">Recrutement efficace</p>
            </div>
          </motion.div>
        </motion.div>
      </div>
    </div>
  );
};

export default RecruiterLogin;
