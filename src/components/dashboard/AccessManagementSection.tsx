import { useState, useEffect } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Switch } from '@/components/ui/switch';
import { AlertDialog, AlertDialogAction, AlertDialogCancel, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogHeader, AlertDialogTitle, AlertDialogTrigger } from '@/components/ui/alert-dialog';
import { Settings, Eye, EyeOff, Key, Trash2, Shield, Mail, Bell, MessageSquare, Gift, PowerOff } from 'lucide-react';
import { toast } from '@/hooks/use-toast';
import { useNavigate } from 'react-router-dom';
import { candidateService, type CandidateProfile } from '@/services/candidateService';
import { authService } from '@/services/authService';

interface AccessManagementSectionProps {
  profile?: CandidateProfile | null;
  onSectionChange?: (section: string) => void;
}

const AccessManagementSection = ({ profile, onSectionChange }: AccessManagementSectionProps) => {
  const navigate = useNavigate();
  const [showCurrentPassword, setShowCurrentPassword] = useState(false);
  const [showNewPassword, setShowNewPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [passwordData, setPasswordData] = useState({
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  });
  const [accessSettings, setAccessSettings] = useState({
    emailNotifications: true,
    smsNotifications: true,
    partnerOffers: true,
    accountDeactivated: false
  });
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    if (profile?.testScores) {
      const ts = profile.testScores as any;
      setAccessSettings({
        emailNotifications: ts.email_notifications !== false,
        smsNotifications: ts.sms_notifications !== false,
        partnerOffers: ts.partner_offers !== false,
        accountDeactivated: ts.account_deactivated === true
      });
    }
  }, [profile?.testScores]);

  const handlePasswordChange = async () => {
    if (passwordData.newPassword !== passwordData.confirmPassword) {
      toast({ title: "Erreur", description: "Les mots de passe ne correspondent pas.", variant: "destructive" });
      return;
    }
    if (passwordData.newPassword.length < 8) {
      toast({ title: "Erreur", description: "Le mot de passe doit contenir au moins 8 caractères.", variant: "destructive" });
      return;
    }
    try {
      await candidateService.changePassword({ currentPassword: passwordData.currentPassword, newPassword: passwordData.newPassword });
      toast({ title: "Mot de passe modifié", description: "Votre mot de passe a été mis à jour avec succès.", className: "bg-green-50 border-green-200 text-green-800" });
      setPasswordData({ currentPassword: '', newPassword: '', confirmPassword: '' });
    } catch (error: any) {
      toast({ title: "Erreur", description: error.message || "Impossible de modifier le mot de passe.", variant: "destructive" });
    }
  };

  const handleDeleteAccount = async () => {
    const password = prompt("Veuillez entrer votre mot de passe pour confirmer la suppression:");
    if (!password) return;
    try {
      await candidateService.deleteAccount({ password, confirmation: 'SUPPRIMER' });
      toast({ title: "Compte supprimé", description: "Votre compte a été supprimé. Vous allez être redirigé...", variant: "destructive" });
      authService.logout();
      setTimeout(() => navigate('/'), 2000);
    } catch (error: any) {
      toast({ title: "Erreur", description: error.message || "Impossible de supprimer le compte.", variant: "destructive" });
    }
  };

  const updateAccessSettings = async (field: string, value: boolean) => {
    const newSettings = { ...accessSettings, [field]: value };
    setAccessSettings(newSettings);
    
    const backendData: any = {};
    if (field === 'emailNotifications') backendData.email_notifications = value;
    if (field === 'smsNotifications') backendData.sms_notifications = value;
    if (field === 'partnerOffers') backendData.partner_offers = value;
    if (field === 'accountDeactivated') backendData.account_deactivated = value;
    
    try {
      setIsSaving(true);
      await candidateService.updateNotificationSettings(backendData);
      toast({ title: "Paramètres mis à jour", description: "Vos préférences ont été sauvegardées.", className: "bg-green-50 border-green-200 text-green-800", duration: 3000 });
    } catch (error) {
      setAccessSettings(accessSettings);
      toast({ title: "Erreur", description: "Impossible de sauvegarder les paramètres.", variant: "destructive" });
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="space-y-4 sm:space-y-6">
      {/* Gestion des accès */}
      <div className="bg-card rounded-lg border border-border">
        <div className="p-4 sm:p-6 border-b border-border bg-muted">
          <h2 className="text-base sm:text-lg font-bold text-foreground flex items-center gap-2">
            <Shield className="h-5 w-5 text-muted-foreground" />
            Gestion de vos Accès et Confidentialité
          </h2>
        </div>
        <div className="p-4 sm:p-6 space-y-4 sm:space-y-6">
          <div className="bg-muted border border-border rounded-lg p-3 sm:p-4 mb-4">
            <h4 className="font-bold text-foreground mb-2">
              Configuration recommandée
            </h4>
            <p className="text-sm text-muted-foreground leading-relaxed">
              <strong>Les options a, b et c sont activées par défaut.</strong> L'option d (Désactivation du compte) est désactivée par défaut. Vous pouvez ajuster ces paramètres à tout moment.
            </p>
          </div>
          <div className="space-y-4">
            <h3 className="text-sm sm:text-base font-bold text-foreground border-b border-border pb-2 flex items-center gap-2">
              <Settings className="h-5 w-5 text-muted-foreground" />
              Paramètres de notifications et confidentialité
            </h3>
            
            <div className="space-y-3 sm:space-y-4">
              {/* a - Email notifications */}
              <div className="flex flex-col sm:flex-row sm:items-center justify-between p-4 sm:p-5 border border-border rounded-lg gap-3 sm:gap-0 hover:shadow-sm transition-all bg-card">
                <div className="space-y-1 flex-1">
                  <div className="flex items-center space-x-2">
                    <Mail className="h-5 w-5 text-muted-foreground shrink-0" />
                    <span className="font-bold text-sm sm:text-base text-foreground">a. Notifications par E-mail</span>
                  </div>
                  <p className="text-xs sm:text-sm text-muted-foreground pr-2">
                    La désactivation de cette option vous empêchera de recevoir les nouveautés qui concernent votre recherche d'emploi.
                  </p>
                </div>
                <Switch checked={accessSettings.emailNotifications} onCheckedChange={(checked) => updateAccessSettings('emailNotifications', checked)} className="data-[state=checked]:bg-primary" />
              </div>

              {/* b - SMS notifications */}
              <div className="flex flex-col sm:flex-row sm:items-center justify-between p-4 sm:p-5 border border-border rounded-lg gap-3 sm:gap-0 hover:shadow-sm transition-all bg-card">
                <div className="space-y-1 flex-1">
                  <div className="flex items-center space-x-2">
                    <MessageSquare className="h-5 w-5 text-muted-foreground shrink-0" />
                    <span className="font-bold text-sm sm:text-base text-foreground">b. Notifications par SMS</span>
                  </div>
                  <p className="text-xs sm:text-sm text-muted-foreground pr-2">
                    La désactivation de cette option vous empêchera de recevoir les nouveautés qui concernent votre recherche d'emploi.
                  </p>
                </div>
                <Switch checked={accessSettings.smsNotifications} onCheckedChange={(checked) => updateAccessSettings('smsNotifications', checked)} className="data-[state=checked]:bg-primary" />
              </div>

              {/* c - Partner offers */}
              <div className="flex flex-col sm:flex-row sm:items-center justify-between p-4 sm:p-5 border border-border rounded-lg gap-3 sm:gap-0 hover:shadow-sm transition-all bg-card">
                <div className="space-y-1 flex-1">
                  <div className="flex items-center space-x-2">
                    <Gift className="h-5 w-5 text-muted-foreground shrink-0" />
                    <span className="font-bold text-sm sm:text-base text-foreground">c. Offres partenaires</span>
                  </div>
                  <p className="text-xs sm:text-sm text-muted-foreground pr-2">
                    La désactivation de cette option vous empêchera de recevoir des offres de nos partenaires telles que les formations métier, linguistiques, coaching etc.
                  </p>
                </div>
                <Switch checked={accessSettings.partnerOffers} onCheckedChange={(checked) => updateAccessSettings('partnerOffers', checked)} className="data-[state=checked]:bg-primary" />
              </div>

              {/* d - Account deactivation */}
              <div className="flex flex-col sm:flex-row sm:items-center justify-between p-4 sm:p-5 border border-amber-200 rounded-lg gap-3 sm:gap-0 hover:shadow-sm transition-all bg-amber-50">
                <div className="space-y-1 flex-1">
                  <div className="flex items-center space-x-2">
                    <PowerOff className="h-5 w-5 text-amber-700 shrink-0" />
                    <span className="font-bold text-sm sm:text-base text-amber-900">d. Désactivation du compte</span>
                  </div>
                  <p className="text-xs sm:text-sm text-amber-800 pr-2">
                    L'activation de cette option écartera votre profil de toutes les recherches de nos centres partenaires. Votre compte ne sera pas supprimé. Vous pouvez le réactiver à tout moment.
                  </p>
                </div>
                <Switch checked={accessSettings.accountDeactivated} onCheckedChange={(checked) => updateAccessSettings('accountDeactivated', checked)} className="data-[state=checked]:bg-amber-600" />
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Changer mot de passe */}
      <div className="bg-card rounded-lg border border-border">
        <div className="p-4 sm:p-6 border-b border-border bg-muted">
          <h2 className="text-base sm:text-lg font-bold text-foreground flex items-center gap-2">
            <Key className="h-5 w-5 text-muted-foreground" />
            Modifier mon Mot de Passe
          </h2>
        </div>
        <div className="p-4 sm:p-6 space-y-3 sm:space-y-4">
          <div className="space-y-3 sm:space-y-4">
            <div className="space-y-2">
              <Label htmlFor="current-password">Mot de passe actuel</Label>
              <div className="relative">
                <Input id="current-password" type={showCurrentPassword ? "text" : "password"} value={passwordData.currentPassword} onChange={(e) => setPasswordData({...passwordData, currentPassword: e.target.value})} placeholder="Saisissez votre mot de passe actuel" />
                <Button type="button" variant="ghost" size="sm" className="absolute right-2 top-1/2 -translate-y-1/2 h-8 w-8 p-0" onClick={() => setShowCurrentPassword(!showCurrentPassword)}>
                  {showCurrentPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                </Button>
              </div>
            </div>
            <div className="space-y-2">
              <Label htmlFor="new-password">Nouveau mot de passe</Label>
              <div className="relative">
                <Input id="new-password" type={showNewPassword ? "text" : "password"} value={passwordData.newPassword} onChange={(e) => setPasswordData({...passwordData, newPassword: e.target.value})} placeholder="Saisissez votre nouveau mot de passe" />
                <Button type="button" variant="ghost" size="sm" className="absolute right-2 top-1/2 -translate-y-1/2 h-8 w-8 p-0" onClick={() => setShowNewPassword(!showNewPassword)}>
                  {showNewPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                </Button>
              </div>
            </div>
            <div className="space-y-2">
              <Label htmlFor="confirm-password">Confirmer le nouveau mot de passe</Label>
              <div className="relative">
                <Input id="confirm-password" type={showConfirmPassword ? "text" : "password"} value={passwordData.confirmPassword} onChange={(e) => setPasswordData({...passwordData, confirmPassword: e.target.value})} placeholder="Confirmez votre nouveau mot de passe" />
                <Button type="button" variant="ghost" size="sm" className="absolute right-2 top-1/2 -translate-y-1/2 h-8 w-8 p-0" onClick={() => setShowConfirmPassword(!showConfirmPassword)}>
                  {showConfirmPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                </Button>
              </div>
            </div>
            <Button onClick={handlePasswordChange} className="w-full" disabled={!passwordData.currentPassword || !passwordData.newPassword || !passwordData.confirmPassword}>
              Modifier le mot de passe
            </Button>
          </div>
        </div>
      </div>

      {/* Supprimer compte */}
      <div className="bg-card rounded-lg border border-destructive/30">
        <div className="p-4 sm:p-6 border-b border-destructive/30">
          <h2 className="text-base sm:text-lg font-semibold text-destructive">Supprimer mon Compte</h2>
        </div>
        <div className="p-4 sm:p-6 space-y-3 sm:space-y-4">
          <div className="bg-red-50 border border-red-200 rounded-md p-3 sm:p-4">
            <h4 className="font-medium text-red-800 mb-2 text-sm sm:text-base">Attention !</h4>
            <p className="text-xs sm:text-sm text-red-700">
              Cette action est irréversible. Une fois votre compte supprimé, toutes vos données seront définitivement perdues et vous ne pourrez plus accéder à votre espace candidat.
            </p>
          </div>
          <AlertDialog>
            <AlertDialogTrigger asChild>
              <Button variant="destructive" className="w-full">
                <Trash2 className="h-4 w-4 mr-2" /> Supprimer définitivement mon compte
              </Button>
            </AlertDialogTrigger>
            <AlertDialogContent>
              <AlertDialogHeader>
                <AlertDialogTitle>Êtes-vous absolument sûr ?</AlertDialogTitle>
                <AlertDialogDescription>
                  Cette action ne peut pas être annulée. Cela supprimera définitivement votre compte et toutes vos données de nos serveurs.
                </AlertDialogDescription>
              </AlertDialogHeader>
              <AlertDialogFooter>
                <AlertDialogCancel>Annuler</AlertDialogCancel>
                <AlertDialogAction onClick={handleDeleteAccount} className="bg-red-600 hover:bg-red-700">
                  Oui, supprimer mon compte
                </AlertDialogAction>
              </AlertDialogFooter>
            </AlertDialogContent>
          </AlertDialog>
        </div>
      </div>
    </div>
  );
};

export default AccessManagementSection;
