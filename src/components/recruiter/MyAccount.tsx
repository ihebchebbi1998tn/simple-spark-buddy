import { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Badge } from '@/components/ui/badge';
import { Separator } from '@/components/ui/separator';
import {
  User, Mail, Phone, Building2, Lock, Eye, EyeOff,
  Save, Shield, Crown, Briefcase, KeyRound, CheckCircle,
} from 'lucide-react';
import { useToast } from '@/hooks/use-toast';
import type { RecruiterUser } from '@/types/recruiter';
import { ROLE_LABELS } from '@/types/recruiter';

interface MyAccountProps {
  user: RecruiterUser;
}

export function MyAccount({ user }: MyAccountProps) {
  const { toast } = useToast();

  // Personal info form
  const [personalInfo, setPersonalInfo] = useState({
    name: user.name,
    surname: user.surname,
    email: user.email,
    phone: '+212 661 000 000',
  });
  const [isEditingInfo, setIsEditingInfo] = useState(false);

  // Password form
  const [passwords, setPasswords] = useState({
    current: '',
    newPassword: '',
    confirm: '',
  });
  const [showPasswords, setShowPasswords] = useState({
    current: false,
    newPassword: false,
    confirm: false,
  });

  const handleSaveInfo = () => {
    if (!personalInfo.name.trim() || !personalInfo.surname.trim()) {
      toast({ title: 'Erreur', description: 'Le nom et prénom sont obligatoires.', variant: 'destructive' });
      return;
    }
    toast({ title: 'Profil mis à jour', description: 'Vos informations ont été enregistrées.' });
    setIsEditingInfo(false);
  };

  const handleChangePassword = () => {
    if (!passwords.current) {
      toast({ title: 'Erreur', description: 'Veuillez saisir votre mot de passe actuel.', variant: 'destructive' });
      return;
    }
    if (passwords.newPassword.length < 8) {
      toast({ title: 'Erreur', description: 'Le nouveau mot de passe doit contenir au moins 8 caractères.', variant: 'destructive' });
      return;
    }
    if (passwords.newPassword !== passwords.confirm) {
      toast({ title: 'Erreur', description: 'Les mots de passe ne correspondent pas.', variant: 'destructive' });
      return;
    }
    toast({ title: 'Mot de passe modifié', description: 'Votre mot de passe a été mis à jour avec succès.' });
    setPasswords({ current: '', newPassword: '', confirm: '' });
  };

  const getRoleIcon = () => {
    switch (user.role) {
      case 'super_admin': return <Crown className="w-4 h-4" />;
      case 'admin': return <Shield className="w-4 h-4" />;
      case 'charge': return <Briefcase className="w-4 h-4" />;
    }
  };

  const PasswordField = ({ 
    label, value, field 
  }: { 
    label: string; value: string; field: 'current' | 'newPassword' | 'confirm' 
  }) => (
    <div className="space-y-2">
      <Label className="text-sm">{label}</Label>
      <div className="relative">
        <Input
          type={showPasswords[field] ? 'text' : 'password'}
          value={value}
          onChange={(e) => setPasswords(prev => ({ ...prev, [field]: e.target.value }))}
          placeholder="••••••••"
          className="pr-10"
        />
        <button
          type="button"
          onClick={() => setShowPasswords(prev => ({ ...prev, [field]: !prev[field] }))}
          className="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground transition-colors"
        >
          {showPasswords[field] ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
        </button>
      </div>
    </div>
  );

  return (
    <div className="space-y-6 max-w-3xl">
      {/* Header */}
      <div>
        <h1 className="text-xl sm:text-2xl font-bold text-foreground flex items-center gap-2.5">
          <div className="w-9 h-9 rounded-lg bg-primary/10 flex items-center justify-center">
            <User className="w-5 h-5 text-primary" />
          </div>
          Mon Compte
        </h1>
        <p className="text-sm text-muted-foreground mt-1 ml-[46px]">
          Gérez vos informations personnelles et votre sécurité
        </p>
      </div>

      {/* Profile summary card */}
      <Card className="border shadow-sm">
        <CardContent className="p-5 sm:p-6">
          <div className="flex items-center gap-4">
            <div className="w-16 h-16 rounded-full bg-primary flex items-center justify-center text-primary-foreground text-xl font-bold shrink-0">
              {user.name.charAt(0)}{user.surname.charAt(0)}
            </div>
            <div className="flex-1 min-w-0">
              <h2 className="text-lg font-semibold text-foreground">{user.name} {user.surname}</h2>
              <p className="text-sm text-muted-foreground">{user.email}</p>
              <div className="flex items-center gap-2 mt-1.5">
                <Badge variant="outline" className="text-xs gap-1">
                  {getRoleIcon()}
                  {ROLE_LABELS[user.role]}
                </Badge>
                {user.company && (
                  <Badge variant="outline" className="text-xs gap-1">
                    <Building2 className="w-3 h-3" />
                    {user.company}
                  </Badge>
                )}
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Personal Information */}
      <Card className="border shadow-sm">
        <CardHeader className="px-5 sm:px-6 py-4 border-b">
          <div className="flex items-center justify-between">
            <CardTitle className="text-base font-semibold flex items-center gap-2">
              <Mail className="w-4 h-4 text-primary" />
              Informations Personnelles
            </CardTitle>
            {!isEditingInfo ? (
              <Button variant="outline" size="sm" onClick={() => setIsEditingInfo(true)} className="text-xs">
                Modifier
              </Button>
            ) : (
              <div className="flex gap-2">
                <Button variant="ghost" size="sm" onClick={() => setIsEditingInfo(false)} className="text-xs">
                  Annuler
                </Button>
                <Button size="sm" onClick={handleSaveInfo} className="text-xs">
                  <Save className="w-3 h-3 mr-1" /> Enregistrer
                </Button>
              </div>
            )}
          </div>
        </CardHeader>
        <CardContent className="p-5 sm:p-6 space-y-4">
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label className="text-sm text-muted-foreground">Prénom</Label>
              {isEditingInfo ? (
                <Input
                  value={personalInfo.name}
                  onChange={(e) => setPersonalInfo(prev => ({ ...prev, name: e.target.value }))}
                />
              ) : (
                <p className="text-sm font-medium py-2">{personalInfo.name}</p>
              )}
            </div>
            <div className="space-y-2">
              <Label className="text-sm text-muted-foreground">Nom</Label>
              {isEditingInfo ? (
                <Input
                  value={personalInfo.surname}
                  onChange={(e) => setPersonalInfo(prev => ({ ...prev, surname: e.target.value }))}
                />
              ) : (
                <p className="text-sm font-medium py-2">{personalInfo.surname}</p>
              )}
            </div>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label className="text-sm text-muted-foreground">Email</Label>
              <div className="flex items-center gap-2">
                {isEditingInfo ? (
                  <Input
                    type="email"
                    value={personalInfo.email}
                    onChange={(e) => setPersonalInfo(prev => ({ ...prev, email: e.target.value }))}
                  />
                ) : (
                  <p className="text-sm font-medium py-2">{personalInfo.email}</p>
                )}
              </div>
            </div>
            <div className="space-y-2">
              <Label className="text-sm text-muted-foreground">Téléphone</Label>
              {isEditingInfo ? (
                <Input
                  value={personalInfo.phone}
                  onChange={(e) => setPersonalInfo(prev => ({ ...prev, phone: e.target.value }))}
                />
              ) : (
                <p className="text-sm font-medium py-2">{personalInfo.phone}</p>
              )}
            </div>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label className="text-sm text-muted-foreground">Rôle</Label>
              <p className="text-sm font-medium py-2 flex items-center gap-1.5">
                {getRoleIcon()}
                {ROLE_LABELS[user.role]}
                <Lock className="w-3 h-3 text-muted-foreground ml-1" />
              </p>
            </div>
            <div className="space-y-2">
              <Label className="text-sm text-muted-foreground">Entreprise</Label>
              <p className="text-sm font-medium py-2 flex items-center gap-1.5">
                <Building2 className="w-3.5 h-3.5 text-muted-foreground" />
                {user.company || '—'}
                <Lock className="w-3 h-3 text-muted-foreground ml-1" />
              </p>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Change Password */}
      <Card className="border shadow-sm">
        <CardHeader className="px-5 sm:px-6 py-4 border-b">
          <CardTitle className="text-base font-semibold flex items-center gap-2">
            <KeyRound className="w-4 h-4 text-primary" />
            Changer le Mot de Passe
          </CardTitle>
        </CardHeader>
        <CardContent className="p-5 sm:p-6 space-y-4">
          <PasswordField label="Mot de passe actuel" value={passwords.current} field="current" />
          <Separator />
          <PasswordField label="Nouveau mot de passe" value={passwords.newPassword} field="newPassword" />
          <PasswordField label="Confirmer le nouveau mot de passe" value={passwords.confirm} field="confirm" />

          {passwords.newPassword.length > 0 && (
            <div className="space-y-1.5 p-3 bg-muted/30 rounded-lg border text-xs">
              <p className="font-medium text-muted-foreground mb-1">Exigences :</p>
              <p className={`flex items-center gap-1.5 ${passwords.newPassword.length >= 8 ? 'text-primary' : 'text-muted-foreground'}`}>
                <CheckCircle className="w-3 h-3" /> Au moins 8 caractères
              </p>
              <p className={`flex items-center gap-1.5 ${/[A-Z]/.test(passwords.newPassword) ? 'text-primary' : 'text-muted-foreground'}`}>
                <CheckCircle className="w-3 h-3" /> Une lettre majuscule
              </p>
              <p className={`flex items-center gap-1.5 ${/[0-9]/.test(passwords.newPassword) ? 'text-primary' : 'text-muted-foreground'}`}>
                <CheckCircle className="w-3 h-3" /> Un chiffre
              </p>
              <p className={`flex items-center gap-1.5 ${passwords.newPassword === passwords.confirm && passwords.confirm.length > 0 ? 'text-primary' : 'text-muted-foreground'}`}>
                <CheckCircle className="w-3 h-3" /> Les mots de passe correspondent
              </p>
            </div>
          )}

          <Button
            onClick={handleChangePassword}
            disabled={!passwords.current || !passwords.newPassword || !passwords.confirm}
            className="w-full sm:w-auto"
          >
            <Lock className="w-4 h-4 mr-2" />
            Mettre à jour le mot de passe
          </Button>
        </CardContent>
      </Card>
    </div>
  );
}
