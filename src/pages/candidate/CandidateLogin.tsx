import { useState, useEffect } from "react";
import { Link, useNavigate, useLocation } from "react-router-dom";
import { motion } from "framer-motion";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { Separator } from "@/components/ui/separator";
import { Eye, EyeOff, Mail, Lock, User, ArrowLeft, CheckCircle, Shield, Zap } from "lucide-react";
import { useToast } from "@/hooks/use-toast";
import ParticleBackground from "@/components/shared/ParticleBackground";
import ForgotPasswordModal from "@/components/auth/ForgotPasswordModal";
import { useTranslation } from "react-i18next";

const CandidateLogin = () => {
  const location = useLocation()
  const navigate = useNavigate()
  const { toast } = useToast()
  const { t } = useTranslation('login')

  const getTestDataFromLocalStorage = () => {
    try {
      const stored = localStorage.getItem('languageTestResult');
      if (stored) {
        const testResult = JSON.parse(stored);
        return {
          fromLanguageTest: true,
          testLanguage: testResult.language,
          testScore: testResult.totalScore && testResult.totalQuestions
            ? Math.round((testResult.totalScore / testResult.totalQuestions) * 100)
            : null
        };
      }
    } catch (e) {
      console.error('Error reading languageTestResult from localStorage:', e);
    }
    return null;
  };

  const localStorageTestData = getTestDataFromLocalStorage();

  const fromLanguageTest = location.state?.fromLanguageTest || localStorageTestData?.fromLanguageTest
  const testScore = location.state?.testScore || localStorageTestData?.testScore
  const testLanguage = location.state?.testLanguage || localStorageTestData?.testLanguage

  const [formData, setFormData] = useState({
    email: "",
    password: ""
  });
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState<{ [key: string]: string }>({});
  const [showForgotPasswordModal, setShowForgotPasswordModal] = useState(false);

  useEffect(() => {
    const checkAuth = async () => {
      const { authService } = await import('@/services/authService');
      if (authService.isAuthenticated()) {
        navigate('/dashboard', { replace: true });
      }
    };
    checkAuth();
  }, [navigate]);

  const handleInputChange = (field: string, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    if (errors[field]) {
      setErrors(prev => ({ ...prev, [field]: "" }));
    }
  };

  const validateForm = () => {
    const newErrors: { [key: string]: string } = {};
    if (!formData.email) {
      newErrors.email = t('emailRequired');
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = t('emailInvalid');
    }
    if (!formData.password) {
      newErrors.password = t('passwordRequired');
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validateForm()) return;
    setIsLoading(true);

    try {
      const { authService } = await import('@/services/authService');

      if (formData.email === "admin@test.com" && formData.password === "admin") {
        const mockToken = "mock-admin-token-" + Date.now();
        const mockUser = { id: "admin-123", email: "admin@test.com", name: "Admin", surname: "Test", role: "admin" };
        authService.storeAuthToken(mockToken);
        authService.storeUserData(mockUser);
        toast({ title: t('loginSuccess'), description: `${t('welcomeBack')} Admin !` });
        navigate('/dashboard');
        return;
      }

      const response = await authService.login({ email: formData.email, password: formData.password });

      if (response.success && response.data?.token) {
        const user = response.data.user;

        if (fromLanguageTest && testScore && testLanguage) {
          const storedTest = localStorage.getItem('languageTestResult');
          localStorage.removeItem('languageTestResult');

          const languageMap: Record<string, string> = {
            'francais': 'french', 'english': 'english', 'deutsch': 'german',
            'italiano': 'italian', 'espanol': 'spanish'
          };
          const backendLanguage = languageMap[testLanguage] || 'french';

          try {
            const { candidateService } = await import('@/services/candidateService');
            const profileResponse = await candidateService.getProfile();
            const testCompletedKey = `${backendLanguage}_test_completed` as keyof typeof profileResponse.data.testScores;
            const alreadyCompleted = profileResponse.data?.testScores?.[testCompletedKey] === true;

            if (!alreadyCompleted) {
              let detailedScores = { linguistic: 0, softSkills: 0, jobSkills: 0, overall: typeof testScore === 'number' ? testScore : 0 };
              if (storedTest) {
                try {
                  const testResult = JSON.parse(storedTest);
                  const scores = testResult.scores;
                  detailedScores = {
                    linguistic: scores?.language?.percentage || 0,
                    softSkills: scores?.softskills?.percentage || 0,
                    jobSkills: scores?.competences?.percentage || 0,
                    overall: testResult.totalScore && testResult.totalQuestions
                      ? Math.round((testResult.totalScore / testResult.totalQuestions) * 100)
                      : (typeof testScore === 'number' ? testScore : 0)
                  };
                } catch (e) { console.error('Error parsing test data:', e); }
              }
              await candidateService.addTestScore(backendLanguage, detailedScores);
            }
          } catch (error) { console.error('Failed to check/save test scores:', error); }
        } else {
          localStorage.removeItem('languageTestResult');
        }

        toast({ title: t('loginSuccess'), description: `${t('welcomeBack')} ${user?.name || ''} !` });
        navigate('/dashboard');
      } else {
        toast({ title: t('loginError'), description: response.message || t('errorGeneric'), variant: "destructive" });
      }
    } catch (error: any) {
      let errorMessage = t('errorGeneric');
      if (error.message?.includes('not active')) errorMessage = t('errorNotActive');
      else if (error.message?.includes('not found')) errorMessage = t('errorNotFound');
      else if (error.message?.includes('password')) errorMessage = t('errorPassword');
      else if (error.message?.includes('fetch') || error.message?.includes('network')) errorMessage = t('errorNetwork');
      else if (error.message?.includes('timeout')) errorMessage = t('errorTimeout');

      toast({ title: t('loginError'), description: errorMessage, variant: "destructive" });
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen relative overflow-hidden bg-gradient-to-br from-background via-background/95 to-primary/5">
      <ParticleBackground />

      <div className="relative z-10 bg-white/80 backdrop-blur-sm border-b border-border/50">
        <div className="container mx-auto px-4 lg:px-8 max-w-7xl">
          <div className="flex items-center justify-between h-16">
            <Link to="/" className="flex items-center gap-2 text-muted-foreground hover:text-foreground transition-colors group">
              <ArrowLeft className="h-4 w-4 group-hover:-translate-x-1 transition-transform" />
              <span className="font-medium">{t('backToHome')}</span>
            </Link>
            <div className="flex items-center">
              <img src="/uploads/488f38af-4f42-45f8-a54f-4c1b46a2dfff.png" alt="Call Center Match" className="h-16 w-auto" />
            </div>
          </div>
        </div>
      </div>

      <div className="relative z-10 flex items-center justify-center min-h-[calc(100vh-4rem)] py-12 px-4">
        <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.6 }} className="w-full max-w-md">
          <Card className="bg-card/80 backdrop-blur-sm border-border/50 shadow-xl">
            <CardHeader className="space-y-4 text-center pb-6">
              <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }} transition={{ delay: 0.2, type: "spring", stiffness: 200 }}
                className="mx-auto w-16 h-16 bg-gradient-to-br from-primary to-primary/80 rounded-full flex items-center justify-center">
                <User className="h-8 w-8 text-white" />
              </motion.div>
              <div>
                <CardTitle className="text-2xl font-bold text-foreground mb-2">
                  {fromLanguageTest ? t('titleFromTest') : t('titleDefault')}
                </CardTitle>
                <p className="text-muted-foreground">
                  {fromLanguageTest ? t('subtitleFromTest') : t('subtitleDefault')}
                </p>
              </div>
            </CardHeader>

            <CardContent className="space-y-6">
              <form onSubmit={handleSubmit} className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="email" className="flex items-center gap-2 text-foreground font-medium">
                    <Mail className="h-4 w-4" />
                    {t('email')}
                  </Label>
                  <Input id="email" type="email" placeholder="votre.email@exemple.com" value={formData.email}
                    onChange={(e) => handleInputChange("email", e.target.value)}
                    className={`h-12 transition-all ${errors.email ? 'border-destructive' : 'border-border focus:border-primary'}`}
                    disabled={isLoading} />
                  {errors.email && <p className="text-sm text-destructive">{errors.email}</p>}
                </div>

                <div className="space-y-2">
                  <Label htmlFor="password" className="flex items-center gap-2 text-foreground font-medium">
                    <Lock className="h-4 w-4" />
                    {t('password')}
                  </Label>
                  <div className="relative">
                    <Input id="password" type={showPassword ? "text" : "password"} placeholder="••••••••"
                      value={formData.password} onChange={(e) => handleInputChange("password", e.target.value)}
                      className={`h-12 pr-12 transition-all ${errors.password ? 'border-destructive' : 'border-border focus:border-primary'}`}
                      disabled={isLoading} />
                    <button type="button" onClick={() => setShowPassword(!showPassword)}
                      className="absolute right-3 top-1/2 transform -translate-y-1/2 text-muted-foreground hover:text-foreground transition-colors" disabled={isLoading}>
                      {showPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                    </button>
                  </div>
                  {errors.password && <p className="text-sm text-destructive">{errors.password}</p>}
                </div>

                <div className="flex items-center justify-between text-sm">
                  <label className="flex items-center space-x-2 cursor-pointer">
                    <input type="checkbox" className="rounded border-border" />
                    <span className="text-muted-foreground text-[11px] sm:text-sm">{t('rememberMe')}</span>
                  </label>
                  <button type="button" onClick={() => setShowForgotPasswordModal(true)}
                    className="text-primary hover:text-primary/80 font-medium transition-colors text-[11px] sm:text-sm">
                    {t('forgotPassword')}
                  </button>
                </div>

                <Button type="submit"
                  className="w-full h-12 bg-gradient-to-r from-primary to-primary/90 hover:from-primary/90 hover:to-primary text-white font-semibold shadow-lg hover:shadow-xl transition-all duration-300 transform hover:scale-[1.02]"
                  disabled={isLoading}>
                  {isLoading ? (
                    <div className="flex items-center gap-2">
                      <div className="animate-spin rounded-full h-4 w-4 border-2 border-white border-t-transparent"></div>
                      {t('loggingIn')}
                    </div>
                  ) : (
                    <div className="flex items-center gap-2">
                      <CheckCircle className="h-4 w-4" />
                      {t('loginBtn')}
                    </div>
                  )}
                </Button>
              </form>

              <div className="relative">
                <Separator />
                <div className="absolute inset-0 flex items-center justify-center">
                  <span className="bg-card px-4 text-sm text-muted-foreground">{t('or')}</span>
                </div>
              </div>

              <Alert className="border-primary/20 bg-primary/5">
                <AlertDescription className="text-center">
                  <span className="text-muted-foreground">{t('noAccount')} </span>
                  <button onClick={() => navigate("/inscription-detaillee")}
                    className="text-primary hover:text-primary/80 font-semibold underline underline-offset-2 transition-colors">
                    {t('createAccount')}
                  </button>
                </AlertDescription>
              </Alert>
            </CardContent>
          </Card>
        </motion.div>
      </div>

      <ForgotPasswordModal isOpen={showForgotPasswordModal} onClose={() => setShowForgotPasswordModal(false)} />
    </div>
  );
};

export default CandidateLogin;
