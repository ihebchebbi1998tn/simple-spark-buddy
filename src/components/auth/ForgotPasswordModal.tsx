import { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { Mail, KeyRound, Eye, EyeOff, ArrowLeft, CheckCircle, Loader2 } from "lucide-react";
import { useToast } from "@/hooks/use-toast";
import { passwordResetService } from "@/services/passwordResetService";

interface ForgotPasswordModalProps {
  isOpen: boolean;
  onClose: () => void;
}

type Step = 'email' | 'otp' | 'password' | 'success';

const ForgotPasswordModal = ({ isOpen, onClose }: ForgotPasswordModalProps) => {
  const [currentStep, setCurrentStep] = useState<Step>('email');
  const [formData, setFormData] = useState({
    email: '',
    otp: '',
    newPassword: '',
    confirmPassword: ''
  });
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState<{[key: string]: string}>({});
  const { toast } = useToast();

  const handleInputChange = (field: string, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    if (errors[field]) {
      setErrors(prev => ({ ...prev, [field]: '' }));
    }
  };

  const validateEmail = () => {
    const newErrors: {[key: string]: string} = {};
    if (!formData.email) {
      newErrors.email = "L'email est obligatoire";
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = "Veuillez entrer un email valide";
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const validateOTP = () => {
    const newErrors: {[key: string]: string} = {};
    if (!formData.otp) {
      newErrors.otp = "Le code de vérification est obligatoire";
    } else if (formData.otp.length !== 5) {
      newErrors.otp = "Le code doit contenir 5 chiffres";
    } else if (!/^\d+$/.test(formData.otp)) {
      newErrors.otp = "Le code ne peut contenir que des chiffres";
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const validatePasswords = () => {
    const newErrors: {[key: string]: string} = {};
    if (!formData.newPassword) {
      newErrors.newPassword = "Le nouveau mot de passe est obligatoire";
    } else if (formData.newPassword.length < 6) {
      newErrors.newPassword = "Le mot de passe doit contenir au moins 6 caractères";
    } else {
      const hasUpperCase = /[A-Z]/.test(formData.newPassword);
      const hasLowerCase = /[a-z]/.test(formData.newPassword);
      const hasNumber = /\d/.test(formData.newPassword);
      
      if (!hasUpperCase || !hasLowerCase || !hasNumber) {
        newErrors.newPassword = "Le mot de passe doit contenir au moins une majuscule, une minuscule et un chiffre";
      }
    }
    
    if (!formData.confirmPassword) {
      newErrors.confirmPassword = "Veuillez confirmer le mot de passe";
    } else if (formData.newPassword !== formData.confirmPassword) {
      newErrors.confirmPassword = "Les mots de passe ne correspondent pas";
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleEmailSubmit = async () => {
    if (!validateEmail()) return;
    
    setIsLoading(true);
    try {
      const result = await passwordResetService.requestReset(formData.email);
      
      if (result.success) {
        toast({
          title: "Code envoyé !",
          description: "Un code de vérification a été envoyé à votre adresse email.",
        });
        setCurrentStep('otp');
      } else {
        toast({
          title: "Erreur",
          description: result.message || "Impossible d'envoyer le code.",
          variant: "destructive"
        });
      }
    } catch (error: any) {
      toast({
        title: "Erreur",
        description: error.message || "Impossible d'envoyer le code. Réessayez plus tard.",
        variant: "destructive"
      });
    } finally {
      setIsLoading(false);
    }
  };

  const handleOTPSubmit = async () => {
    if (!validateOTP()) return;
    
    setIsLoading(true);
    try {
      const result = await passwordResetService.verifyCode(formData.email, formData.otp);
      
      if (result.success) {
        toast({
          title: "Code vérifié !",
          description: "Vous pouvez maintenant définir votre nouveau mot de passe.",
        });
        setCurrentStep('password');
      } else {
        toast({
          title: "Code incorrect",
          description: result.message || "Le code de vérification est invalide.",
          variant: "destructive"
        });
      }
    } catch (error: any) {
      toast({
        title: "Code incorrect",
        description: error.message || "Le code de vérification est invalide.",
        variant: "destructive"
      });
    } finally {
      setIsLoading(false);
    }
  };

  const handlePasswordSubmit = async () => {
    if (!validatePasswords()) return;
    
    setIsLoading(true);
    try {
      const result = await passwordResetService.resetPassword(
        formData.email, 
        formData.otp, 
        formData.newPassword
      );
      
      if (result.success) {
        setCurrentStep('success');
        toast({
          title: "Mot de passe modifié !",
          description: "Vous pouvez maintenant vous connecter.",
        });
      } else {
        toast({
          title: "Erreur",
          description: result.message || "Impossible de modifier le mot de passe.",
          variant: "destructive"
        });
      }
    } catch (error: any) {
      toast({
        title: "Erreur",
        description: error.message || "Impossible de modifier le mot de passe. Réessayez plus tard.",
        variant: "destructive"
      });
    } finally {
      setIsLoading(false);
    }
  };

  const handleClose = () => {
    setCurrentStep('email');
    setFormData({ email: '', otp: '', newPassword: '', confirmPassword: '' });
    setErrors({});
    onClose();
  };

  const getStepTitle = () => {
    switch (currentStep) {
      case 'email': return 'Mot de passe oublié';
      case 'otp': return 'Code de vérification';
      case 'password': return 'Nouveau mot de passe';
      case 'success': return 'Mot de passe modifié';
      default: return '';
    }
  };

  const getStepDescription = () => {
    switch (currentStep) {
      case 'email': return 'Entrez votre adresse email pour recevoir un code de réinitialisation';
      case 'otp': return `Entrez le code à 5 chiffres envoyé à ${formData.email}`;
      case 'password': return 'Définissez votre nouveau mot de passe';
      case 'success': return 'Votre mot de passe a été modifié avec succès !';
      default: return '';
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={handleClose}>
      <DialogContent className="sm:max-w-md">
        <DialogHeader className="text-center">
          <motion.div
            initial={{ scale: 0 }}
            animate={{ scale: 1 }}
            transition={{ type: "spring", stiffness: 200 }}
            className="mx-auto w-12 h-12 bg-gradient-to-br from-primary to-primary/80 rounded-full flex items-center justify-center mb-4"
          >
            {currentStep === 'success' ? (
              <CheckCircle className="h-6 w-6 text-white" />
            ) : (
              <KeyRound className="h-6 w-6 text-white" />
            )}
          </motion.div>
          <DialogTitle className="text-xl font-bold">{getStepTitle()}</DialogTitle>
          <p className="text-sm text-muted-foreground">{getStepDescription()}</p>
        </DialogHeader>

        <AnimatePresence mode="wait">
          {currentStep === 'email' && (
            <motion.div
              key="email"
              initial={{ opacity: 0, x: 20 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -20 }}
              className="space-y-4"
            >
              <div className="space-y-2">
                <Label htmlFor="reset-email" className="flex items-center gap-2">
                  <Mail className="h-4 w-4" />
                  Adresse email
                </Label>
                <Input
                  id="reset-email"
                  type="email"
                  placeholder="votre.email@exemple.com"
                  value={formData.email}
                  onChange={(e) => handleInputChange('email', e.target.value)}
                  className={errors.email ? 'border-destructive' : ''}
                  disabled={isLoading}
                />
                {errors.email && (
                  <p className="text-sm text-destructive">{errors.email}</p>
                )}
              </div>
              
              <div className="flex gap-2 pt-2">
                <Button
                  variant="outline"
                  onClick={handleClose}
                  className="flex-1"
                  disabled={isLoading}
                >
                  Annuler
                </Button>
                <Button
                  onClick={handleEmailSubmit}
                  className="flex-1"
                  disabled={isLoading}
                >
                  {isLoading ? (
                    <div className="flex items-center gap-2">
                      <Loader2 className="h-4 w-4 animate-spin" />
                      Envoi...
                    </div>
                  ) : (
                    'Envoyer le code'
                  )}
                </Button>
              </div>
            </motion.div>
          )}

          {currentStep === 'otp' && (
            <motion.div
              key="otp"
              initial={{ opacity: 0, x: 20 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -20 }}
              className="space-y-4"
            >
              <div className="space-y-2">
                <Label htmlFor="otp-code" className="flex items-center gap-2">
                  <KeyRound className="h-4 w-4" />
                  Code de vérification
                </Label>
                <Input
                  id="otp-code"
                  type="text"
                  placeholder="12345"
                  maxLength={5}
                  value={formData.otp}
                  onChange={(e) => handleInputChange('otp', e.target.value.replace(/\D/g, ''))}
                  className={`text-center text-lg tracking-widest ${errors.otp ? 'border-destructive' : ''}`}
                  disabled={isLoading}
                />
                {errors.otp && (
                  <p className="text-sm text-destructive">{errors.otp}</p>
                )}
              </div>
              
              <div className="flex gap-2 pt-2">
                <Button
                  variant="outline"
                  onClick={() => setCurrentStep('email')}
                  className="flex-1"
                  disabled={isLoading}
                >
                  <ArrowLeft className="h-4 w-4 mr-2" />
                  Retour
                </Button>
                <Button
                  onClick={handleOTPSubmit}
                  className="flex-1"
                  disabled={isLoading}
                >
                  {isLoading ? (
                    <div className="flex items-center gap-2">
                      <Loader2 className="h-4 w-4 animate-spin" />
                      Vérification...
                    </div>
                  ) : (
                    'Vérifier'
                  )}
                </Button>
              </div>
            </motion.div>
          )}

          {currentStep === 'password' && (
            <motion.div
              key="password"
              initial={{ opacity: 0, x: 20 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -20 }}
              className="space-y-4"
            >
              <div className="space-y-2">
                <Label htmlFor="new-password">Nouveau mot de passe</Label>
                <div className="relative">
                  <Input
                    id="new-password"
                    type={showPassword ? "text" : "password"}
                    placeholder="Min. 6 car. (majuscule, minuscule, chiffre)"
                    value={formData.newPassword}
                    onChange={(e) => handleInputChange('newPassword', e.target.value)}
                    className={`pr-12 ${errors.newPassword ? 'border-destructive' : ''}`}
                    disabled={isLoading}
                  />
                  <button
                    type="button"
                    onClick={() => setShowPassword(!showPassword)}
                    className="absolute right-3 top-1/2 transform -translate-y-1/2 text-muted-foreground hover:text-foreground"
                    disabled={isLoading}
                  >
                    {showPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                  </button>
                </div>
                {errors.newPassword && (
                  <p className="text-sm text-destructive">{errors.newPassword}</p>
                )}
              </div>

              <div className="space-y-2">
                <Label htmlFor="confirm-password">Confirmer le mot de passe</Label>
                <div className="relative">
                  <Input
                    id="confirm-password"
                    type={showConfirmPassword ? "text" : "password"}
                    placeholder="Retapez votre mot de passe"
                    value={formData.confirmPassword}
                    onChange={(e) => handleInputChange('confirmPassword', e.target.value)}
                    className={`pr-12 ${errors.confirmPassword ? 'border-destructive' : ''}`}
                    disabled={isLoading}
                  />
                  <button
                    type="button"
                    onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                    className="absolute right-3 top-1/2 transform -translate-y-1/2 text-muted-foreground hover:text-foreground"
                    disabled={isLoading}
                  >
                    {showConfirmPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                  </button>
                </div>
                {errors.confirmPassword && (
                  <p className="text-sm text-destructive">{errors.confirmPassword}</p>
                )}
              </div>
              
              <div className="flex gap-2 pt-2">
                <Button
                  variant="outline"
                  onClick={() => setCurrentStep('otp')}
                  className="flex-1"
                  disabled={isLoading}
                >
                  <ArrowLeft className="h-4 w-4 mr-2" />
                  Retour
                </Button>
                <Button
                  onClick={handlePasswordSubmit}
                  className="flex-1"
                  disabled={isLoading}
                >
                  {isLoading ? (
                    <div className="flex items-center gap-2">
                      <Loader2 className="h-4 w-4 animate-spin" />
                      Modification...
                    </div>
                  ) : (
                    'Modifier le mot de passe'
                  )}
                </Button>
              </div>
            </motion.div>
          )}

          {currentStep === 'success' && (
            <motion.div
              key="success"
              initial={{ opacity: 0, scale: 0.9 }}
              animate={{ opacity: 1, scale: 1 }}
              exit={{ opacity: 0, scale: 0.9 }}
              className="text-center space-y-4"
            >
              <div className="py-4">
                <p className="text-muted-foreground">
                  Vous pouvez maintenant vous connecter avec votre nouveau mot de passe.
                </p>
              </div>
              
              <Button
                onClick={handleClose}
                className="w-full"
              >
                <CheckCircle className="h-4 w-4 mr-2" />
                Retour à la connexion
              </Button>
            </motion.div>
          )}
        </AnimatePresence>
      </DialogContent>
    </Dialog>
  );
};

export default ForgotPasswordModal;
