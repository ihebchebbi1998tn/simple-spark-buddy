
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Badge } from '@/components/ui/badge';
import { Edit3, Save, X } from 'lucide-react';
import { isFieldEmpty, getFieldClasses } from '@/utils/fieldHelpers';
import { candidateService, type CandidateProfile } from '@/services/candidateService';
import { useToast } from '@/hooks/use-toast';
import { useState, useEffect, useMemo } from 'react';
import { filterExperienceLevelsByGlobal, EXPERIENCE_LEVELS, OPERATIONS, getOperationsForActivity } from '@/utils/constants';

interface ProfileSectionProps {
  profile: CandidateProfile | null;
  onRefresh: () => void;
}

const ProfileSection = ({ profile, onRefresh }: ProfileSectionProps) => {
  const { toast } = useToast();
  const [isSaving, setIsSaving] = useState(false);
  const [isEditingContact, setIsEditingContact] = useState(false);
  const [isEditingSearch, setIsEditingSearch] = useState(false);

  // Store saved data from backend
  const [savedData, setSavedData] = useState({
    civilite: profile?.candidate?.gender || '',
    nom: profile?.candidate?.name || '',
    prenom: profile?.candidate?.surname || '',
    telephone: profile?.candidate?.phone || '',
    email: profile?.candidate?.email || '',
    ville: profile?.candidate?.city || '',
    birthday: (profile?.candidate as any)?.birthday || '',
    codePostal: (profile?.candidate as any)?.postal_code || '',
    facebook: (profile?.candidate as any)?.facebook || '',
    instagram: (profile?.candidate as any)?.instagram || '',
    linkedin: (profile?.candidate as any)?.linkedin || '',
    experienceGlobale: profile?.profile?.call_center_experience || '',
    posteRecherche: profile?.profile?.desired_position || '',
    experiencePoste: profile?.profile?.position_experience || '',
    activitePrincipale: profile?.profile?.primary_activity || '',
    experienceActivitePrincipale: profile?.profile?.primary_activity_experience || '',
    operation1: profile?.profile?.operation_1 || '',
    experienceOperation1: profile?.profile?.operation_1_experience || '',
    operation2: profile?.profile?.operation_2 || '',
    experienceOperation2: profile?.profile?.operation_2_experience || ''
  });

  // Working data during editing
  const [profileData, setProfileData] = useState(savedData);

  const isEditing = isEditingContact || isEditingSearch;

  // Update field in working data (no auto-save)
  const updateField = (field: string, value: string) => {
    setProfileData(prev => ({ ...prev, [field]: value }));
  };

  // Sync with profile data when not editing
  useEffect(() => {
    if (profile && !isEditing) {
      const updates = {
        civilite: profile.candidate?.gender || '',
        nom: profile.candidate?.name || '',
        prenom: profile.candidate?.surname || '',
        telephone: profile.candidate?.phone || '',
        email: profile.candidate?.email || '',
        ville: profile.candidate?.city || '',
        birthday: (profile.candidate as any)?.birthday || '',
        codePostal: (profile.candidate as any)?.postal_code || '',
        facebook: (profile.candidate as any)?.facebook || '',
        instagram: (profile.candidate as any)?.instagram || '',
        linkedin: (profile.candidate as any)?.linkedin || '',
        experienceGlobale: profile.profile?.call_center_experience || '',
        posteRecherche: profile.profile?.desired_position || '',
        experiencePoste: profile.profile?.position_experience || '',
        activitePrincipale: profile.profile?.primary_activity || '',
        experienceActivitePrincipale: profile.profile?.primary_activity_experience || '',
        operation1: profile.profile?.operation_1 || '',
        experienceOperation1: profile.profile?.operation_1_experience || '',
        operation2: profile.profile?.operation_2 || '',
        experienceOperation2: profile.profile?.operation_2_experience || ''
      };
      setSavedData(updates);
      setProfileData(updates);
    }
  }, [profile, isEditing]);

  // Handle edit mode
  const handleEditContact = () => {
    setProfileData({ ...savedData });
    setIsEditingContact(true);
  };

  const handleEditSearch = () => {
    setProfileData({ ...savedData });
    setIsEditingSearch(true);
  };

  // Handle cancel
  const handleCancel = () => {
    setProfileData({ ...savedData });
    setIsEditingContact(false);
    setIsEditingSearch(false);
  };

  // Handle save
  const handleSave = async () => {
    setIsSaving(true);
    console.log('💾 Manual profile save initiated');
    
    try {
      await candidateService.updateCandidateInfo({
        name: profileData.nom,
        surname: profileData.prenom,
        phone: profileData.telephone,
        city: profileData.ville,
        gender: profileData.civilite,
        birthday: profileData.birthday || undefined,
        postal_code: profileData.codePostal || undefined,
        facebook: profileData.facebook || undefined,
        instagram: profileData.instagram || undefined,
        linkedin: profileData.linkedin || undefined
      });
      
      await candidateService.updateProfile({
        call_center_experience: profileData.experienceGlobale,
        desired_position: profileData.posteRecherche,
        position_experience: profileData.experiencePoste,
        primary_activity: profileData.activitePrincipale,
        primary_activity_experience: profileData.experienceActivitePrincipale,
        operation_1: profileData.operation1,
        operation_1_experience: profileData.experienceOperation1,
        operation_2: profileData.operation2,
        operation_2_experience: profileData.experienceOperation2
      });
      
      console.log('✅ Profile saved successfully');
      setSavedData({ ...profileData });
      setIsEditingContact(false);
      setIsEditingSearch(false);
      toast({
        title: 'Sauvegardé',
        description: 'Vos modifications ont été enregistrées.',
        className: "bg-green-50 border-green-200 text-green-800",
        duration: 3000,
      });
      onRefresh();
    } catch (error) {
      console.error('❌ Save failed:', error);
      toast({
        title: 'Erreur',
        description: 'Impossible de sauvegarder les modifications.',
        variant: 'destructive',
      });
    } finally {
      setIsSaving(false);
    }
  };

  // Check if user has minimal experience (0 = Aucune expérience, 1 = 0-6 mois)
  const hasMinimalExperience = useMemo(() => {
    const globalExp = profileData.experienceGlobale;
    return globalExp === '0' || globalExp === '1';
  }, [profileData.experienceGlobale]);

  // Map experience value to numeric (in months) for cumulative calculation
  const experienceToMonths = (value: string): number => {
    const mapping: Record<string, number> = {
      "1": 6, "2": 12, "3": 24, "4": 36, "5": 60, "6": 84, "7": 120,
    };
    return mapping[value] || 0;
  };

  const activityExperienceMonths = experienceToMonths(profileData.experienceActivitePrincipale || "0");

  const filteredPositionExperienceOptions = useMemo(() => {
    const globalExp = profileData.experienceGlobale;
    return filterExperienceLevelsByGlobal(
      globalExp || '1', 
      EXPERIENCE_LEVELS.map(exp => ({ value: exp.value, label: exp.label }))
    );
  }, [profileData.experienceGlobale]);

  const filteredActivityExperienceOptions = useMemo(() => {
    const globalExp = profileData.experienceGlobale;
    return filterExperienceLevelsByGlobal(
      globalExp || '1', 
      EXPERIENCE_LEVELS.map(exp => ({ value: exp.value, label: exp.label }))
    );
  }, [profileData.experienceGlobale]);

  const filteredOperation1ExperienceOptions = useMemo(() => {
    if (!profileData.experienceActivitePrincipale) return [];
    const usedByOp2 = profileData.experienceOperation2 ? experienceToMonths(profileData.experienceOperation2) : 0;
    const remainingBudget = activityExperienceMonths - usedByOp2;
    return EXPERIENCE_LEVELS
      .map(exp => ({ value: exp.value, label: exp.label }))
      .filter(exp => experienceToMonths(exp.value) <= remainingBudget);
  }, [profileData.experienceActivitePrincipale, profileData.experienceOperation2, activityExperienceMonths]);

  const filteredOperation2ExperienceOptions = useMemo(() => {
    if (!profileData.experienceActivitePrincipale) return [];
    const usedByOp1 = profileData.experienceOperation1 ? experienceToMonths(profileData.experienceOperation1) : 0;
    const remainingBudget = activityExperienceMonths - usedByOp1;
    return EXPERIENCE_LEVELS
      .map(exp => ({ value: exp.value, label: exp.label }))
      .filter(exp => experienceToMonths(exp.value) <= remainingBudget);
  }, [profileData.experienceActivitePrincipale, profileData.experienceOperation1, activityExperienceMonths]);

  useEffect(() => {
    if (hasMinimalExperience && isEditing) {
      const updates: Record<string, string> = {};
      if (profileData.activitePrincipale && profileData.experienceActivitePrincipale !== '1') updates.experienceActivitePrincipale = '1';
      if (profileData.operation1 && profileData.experienceOperation1 !== '1') updates.experienceOperation1 = '1';
      if (profileData.operation2 && profileData.experienceOperation2 !== '1') updates.experienceOperation2 = '1';
      if (Object.keys(updates).length > 0) setProfileData(prev => ({ ...prev, ...updates }));
    }
  }, [hasMinimalExperience, isEditing, profileData.activitePrincipale, profileData.operation1, profileData.operation2]);

  useEffect(() => {
    if (!hasMinimalExperience && isEditing && profileData.experienceActivitePrincipale) {
      const activityBudget = experienceToMonths(profileData.experienceActivitePrincipale);
      const updates: Record<string, string> = {};
      if (profileData.experienceOperation1) {
        const usedByOp2 = profileData.experienceOperation2 ? experienceToMonths(profileData.experienceOperation2) : 0;
        if (experienceToMonths(profileData.experienceOperation1) > activityBudget - usedByOp2) updates.experienceOperation1 = '';
      }
      if (profileData.experienceOperation2) {
        const usedByOp1 = profileData.experienceOperation1 ? experienceToMonths(profileData.experienceOperation1) : 0;
        if (experienceToMonths(profileData.experienceOperation2) > activityBudget - usedByOp1) updates.experienceOperation2 = '';
      }
      if (Object.keys(updates).length > 0) setProfileData(prev => ({ ...prev, ...updates }));
    }
  }, [profileData.experienceActivitePrincipale, hasMinimalExperience, isEditing]);

  const filteredOperations = useMemo(() => {
    return getOperationsForActivity(profileData.activitePrincipale, OPERATIONS);
  }, [profileData.activitePrincipale]);

  useEffect(() => {
    if (isEditing && profileData.activitePrincipale) {
      const validOperationValues = filteredOperations.map(op => op.value);
      const updates: Record<string, string> = {};
      if (profileData.operation1 && !validOperationValues.includes(profileData.operation1)) {
        updates.operation1 = ''; updates.experienceOperation1 = '';
      }
      if (profileData.operation2 && !validOperationValues.includes(profileData.operation2)) {
        updates.operation2 = ''; updates.experienceOperation2 = '';
      }
      if (Object.keys(updates).length > 0) setProfileData(prev => ({ ...prev, ...updates }));
    }
  }, [profileData.activitePrincipale, filteredOperations, isEditing]);

  useEffect(() => {
    if (isEditing && profileData.experiencePoste && profileData.experienceGlobale && profileData.posteRecherche !== '1') {
      const globalExpInt = parseInt(profileData.experienceGlobale, 10);
      const posteExpInt = parseInt(profileData.experiencePoste, 10);
      if (posteExpInt > globalExpInt) setProfileData(prev => ({ ...prev, experiencePoste: '' }));
    }
  }, [profileData.experienceGlobale, isEditing]);

  return (
    <div className="space-y-4 sm:space-y-6">
      {/* Section: Mes Coordonnées */}
      <Card className="border border-border bg-card">
        <CardContent className="pt-6">
          <div className="flex flex-col sm:flex-row sm:items-center justify-between mb-6">
            <h2 className="text-lg font-bold text-primary">Mes Coordonnées</h2>
            <div className="flex gap-2 mt-2 sm:mt-0">
              {isEditingContact ? (
                <>
                  <Button variant="outline" size="sm" onClick={handleCancel} disabled={isSaving}>
                    <X className="h-4 w-4 mr-1" /> Annuler
                  </Button>
                  <Button size="sm" onClick={handleSave} disabled={isSaving} className="bg-emerald-600 hover:bg-emerald-700">
                    <Save className="h-4 w-4 mr-1" /> {isSaving ? 'Sauvegarde...' : 'Sauvegarder'}
                  </Button>
                </>
              ) : (
                <Button variant="outline" size="sm" onClick={handleEditContact}>
                  <Edit3 className="h-4 w-4 mr-1" /> Modifier
                </Button>
              )}
            </div>
          </div>
          <div className="space-y-6">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 sm:gap-4">
              <div className="space-y-2">
                <Label htmlFor="civilite">Civilité</Label>
                <Select value={profileData.civilite} onValueChange={(value) => updateField('civilite', value)} disabled={!isEditingContact}>
                  <SelectTrigger>
                    <SelectValue placeholder="Sélectionnez">
                      {profileData.civilite === 'madame' ? 'Madame' : profileData.civilite === 'monsieur' ? 'Monsieur' : profileData.civilite || 'Sélectionnez'}
                    </SelectValue>
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="madame">Madame</SelectItem>
                    <SelectItem value="monsieur">Monsieur</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <div className="space-y-2">
                <Label htmlFor="nom" className={isFieldEmpty(profileData.nom) ? 'text-red-600' : ''}>
                  Nom {isFieldEmpty(profileData.nom) && <span className="text-red-500">*</span>}
                </Label>
                <Input id="nom" value={profileData.nom} onChange={(e) => updateField('nom', e.target.value)} disabled={!isEditingContact} placeholder="Votre nom" className={`${getFieldClasses(profileData.nom, isEditingContact)} ${isFieldEmpty(profileData.nom) && !isEditingContact ? 'border-red-400 ring-1 ring-red-300' : ''}`} />
              </div>
              <div className="space-y-2">
                <Label htmlFor="prenom" className={isFieldEmpty(profileData.prenom) ? 'text-red-600' : ''}>
                  Prénom {isFieldEmpty(profileData.prenom) && <span className="text-red-500">*</span>}
                </Label>
                <Input id="prenom" value={profileData.prenom} onChange={(e) => updateField('prenom', e.target.value)} disabled={!isEditingContact} placeholder="Votre prénom" className={`${getFieldClasses(profileData.prenom, isEditingContact)} ${isFieldEmpty(profileData.prenom) && !isEditingContact ? 'border-red-400 ring-1 ring-red-300' : ''}`} />
              </div>
              <div className="space-y-2">
                <Label htmlFor="telephone" className={isFieldEmpty(profileData.telephone) ? 'text-red-600' : ''}>
                  Téléphone {isFieldEmpty(profileData.telephone) && <span className="text-red-500">*</span>}
                </Label>
                <Input id="telephone" value={profileData.telephone} onChange={(e) => updateField('telephone', e.target.value)} disabled={!isEditingContact} placeholder="Votre téléphone" className={`${getFieldClasses(profileData.telephone, isEditingContact)} ${isFieldEmpty(profileData.telephone) && !isEditingContact ? 'border-red-400 ring-1 ring-red-300' : ''}`} />
              </div>
              <div className="space-y-2">
                <Label htmlFor="email">Email</Label>
                <Input id="email" value={profileData.email} disabled={true} placeholder="Votre email" className="bg-muted" />
                <p className="text-xs text-muted-foreground">L'email ne peut pas être modifié ici</p>
              </div>
              <div className="space-y-2">
                <Label htmlFor="birthday">Date de naissance <span className="text-xs text-muted-foreground">(facultatif)</span></Label>
                {isEditingContact ? (
                  <div className="grid grid-cols-3 gap-2">
                    <Select value={profileData.birthday ? new Date(profileData.birthday).getDate().toString() : ''} onValueChange={(day) => { const current = profileData.birthday ? new Date(profileData.birthday) : new Date(2000, 0, 1); const newDate = new Date(current.getFullYear(), current.getMonth(), parseInt(day)); updateField('birthday', newDate.toISOString().split('T')[0]); }}>
                      <SelectTrigger><SelectValue placeholder="Jour" /></SelectTrigger>
                      <SelectContent className="bg-white z-50">{Array.from({ length: 31 }, (_, i) => (<SelectItem key={i + 1} value={(i + 1).toString()}>{i + 1}</SelectItem>))}</SelectContent>
                    </Select>
                    <Select value={profileData.birthday ? new Date(profileData.birthday).getMonth().toString() : ''} onValueChange={(month) => { const current = profileData.birthday ? new Date(profileData.birthday) : new Date(2000, 0, 1); const newDate = new Date(current.getFullYear(), parseInt(month), current.getDate()); updateField('birthday', newDate.toISOString().split('T')[0]); }}>
                      <SelectTrigger><SelectValue placeholder="Mois" /></SelectTrigger>
                      <SelectContent className="bg-white z-50">{['Janvier','Février','Mars','Avril','Mai','Juin','Juillet','Août','Septembre','Octobre','Novembre','Décembre'].map((m, i) => (<SelectItem key={i} value={i.toString()}>{m}</SelectItem>))}</SelectContent>
                    </Select>
                    <Select value={profileData.birthday ? new Date(profileData.birthday).getFullYear().toString() : ''} onValueChange={(year) => { const current = profileData.birthday ? new Date(profileData.birthday) : new Date(2000, 0, 1); const newDate = new Date(parseInt(year), current.getMonth(), current.getDate()); updateField('birthday', newDate.toISOString().split('T')[0]); }}>
                      <SelectTrigger><SelectValue placeholder="Année" /></SelectTrigger>
                      <SelectContent className="bg-card z-50 max-h-60">{Array.from({ length: new Date().getFullYear() - 16 - 1950 + 1 }, (_, i) => { const year = new Date().getFullYear() - 16 - i; return <SelectItem key={year} value={year.toString()}>{year}</SelectItem>; })}</SelectContent>
                    </Select>
                  </div>
                ) : (
                  <Input id="birthday" value={profileData.birthday ? (() => { const d = new Date(profileData.birthday); const months = ['Janvier','Février','Mars','Avril','Mai','Juin','Juillet','Août','Septembre','Octobre','Novembre','Décembre']; return `${d.getDate()} ${months[d.getMonth()]} ${d.getFullYear()}`; })() : 'Non renseigné'} disabled={true} className="bg-muted" />
                )}
              </div>
              <div className="space-y-2">
                <Label htmlFor="ville" className={isFieldEmpty(profileData.ville) ? 'text-red-600' : ''}>
                  Ville de résidence {isFieldEmpty(profileData.ville) && <span className="text-red-500">*</span>}
                </Label>
                <Input id="ville" value={profileData.ville} onChange={(e) => updateField('ville', e.target.value)} disabled={!isEditingContact} placeholder="Votre ville" className={`${getFieldClasses(profileData.ville, isEditingContact)} ${isFieldEmpty(profileData.ville) && !isEditingContact ? 'border-red-400 ring-1 ring-red-300' : ''}`} />
              </div>
              <div className="space-y-2">
                <Label htmlFor="codePostal">Code postal <span className="text-xs text-muted-foreground">(facultatif)</span></Label>
                <Input id="codePostal" value={profileData.codePostal} onChange={(e) => updateField('codePostal', e.target.value.replace(/\D/g, '').slice(0, 5))} disabled={!isEditingContact} placeholder="Ex: 10000" className={isEditingContact ? '' : 'bg-muted'} maxLength={5} />
              </div>
            </div>

            {/* Social Media */}
            <div className="space-y-3 pt-2">
              <h4 className="text-sm font-semibold text-muted-foreground">Réseaux sociaux <span className="text-xs text-muted-foreground">(facultatif)</span></h4>
              <div className="grid grid-cols-1 sm:grid-cols-3 gap-3">
                <div className="space-y-1">
                  <Label htmlFor="facebook" className="text-xs">Facebook</Label>
                  <Input id="facebook" value={profileData.facebook} onChange={(e) => updateField('facebook', e.target.value)} disabled={!isEditingContact} placeholder="Lien profil Facebook" className={isEditingContact ? '' : 'bg-muted'} />
                </div>
                <div className="space-y-1">
                  <Label htmlFor="instagram" className="text-xs">Instagram</Label>
                  <Input id="instagram" value={profileData.instagram} onChange={(e) => updateField('instagram', e.target.value)} disabled={!isEditingContact} placeholder="Lien profil Instagram" className={isEditingContact ? '' : 'bg-muted'} />
                </div>
                <div className="space-y-1">
                  <Label htmlFor="linkedin" className="text-xs">LinkedIn</Label>
                  <Input id="linkedin" value={profileData.linkedin} onChange={(e) => updateField('linkedin', e.target.value)} disabled={!isEditingContact} placeholder="Lien profil LinkedIn" className={isEditingContact ? '' : 'bg-muted'} />
                </div>
              </div>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Section: Ma Recherche */}
      <Card className="border border-border bg-card">
        <CardContent className="pt-6">
          <div className="flex flex-col sm:flex-row sm:items-center justify-between mb-6">
            <h2 className="text-lg font-bold text-primary">Ma Recherche</h2>
            <div className="flex gap-2 mt-2 sm:mt-0">
              {isEditingSearch ? (
                <>
                  <Button variant="outline" size="sm" onClick={handleCancel} disabled={isSaving}>
                    <X className="h-4 w-4 mr-1" /> Annuler
                  </Button>
                  <Button size="sm" onClick={handleSave} disabled={isSaving} className="bg-emerald-600 hover:bg-emerald-700">
                    <Save className="h-4 w-4 mr-1" /> {isSaving ? 'Sauvegarde...' : 'Sauvegarder'}
                  </Button>
                </>
              ) : (
                <Button variant="outline" size="sm" onClick={handleEditSearch}>
                  <Edit3 className="h-4 w-4 mr-1" /> Modifier
                </Button>
              )}
            </div>
          </div>
          <div className="space-y-6 sm:space-y-8">
            {/* 1. Poste recherché (first per customer request) */}
            <div className="space-y-4">
              <h3 className="text-base font-semibold text-foreground border-b border-border pb-2">Poste recherché</h3>
              <div className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="posteRecherche" className={isFieldEmpty(profileData.posteRecherche) ? 'text-red-600' : ''}>
                    Poste recherché {isFieldEmpty(profileData.posteRecherche) && <span className="text-red-500">*</span>}
                  </Label>
                  <Select value={profileData.posteRecherche} onValueChange={(value) => updateField('posteRecherche', value)} disabled={!isEditingSearch}>
                    <SelectTrigger className={`${getFieldClasses(profileData.posteRecherche, isEditingSearch)} ${isFieldEmpty(profileData.posteRecherche) && !isEditingSearch ? 'border-red-400 ring-1 ring-red-300' : ''}`}>
                      <SelectValue placeholder="Sélectionnez le poste recherché">
                        {profileData.posteRecherche === '1' ? 'Agent' : profileData.posteRecherche === '2' ? 'Team Leader' : profileData.posteRecherche === '3' ? 'Responsable Activité' : profileData.posteRecherche === '4' ? 'Responsable Qualité' : profileData.posteRecherche === '5' ? 'Responsable Service client' : profileData.posteRecherche === '6' ? 'Responsable Recrutement' : profileData.posteRecherche === '7' ? 'Responsable Plateau' : profileData.posteRecherche === '8' ? 'Formateur' : profileData.posteRecherche === '9' ? 'Chargé Recrutement' : 'Sélectionnez le poste recherché'}
                      </SelectValue>
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="1">Agent</SelectItem>
                      <SelectItem value="2">Team Leader</SelectItem>
                      <SelectItem value="3">Responsable Activité</SelectItem>
                      <SelectItem value="4">Responsable Qualité</SelectItem>
                      <SelectItem value="5">Responsable Service client</SelectItem>
                      <SelectItem value="6">Responsable Recrutement</SelectItem>
                      <SelectItem value="7">Responsable Plateau</SelectItem>
                      <SelectItem value="8">Formateur</SelectItem>
                      <SelectItem value="9">Chargé Recrutement</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
                {profileData.posteRecherche !== '1' && (
                  <div className="space-y-2">
                    <Label htmlFor="experiencePoste" className={isFieldEmpty(profileData.experiencePoste) ? 'text-red-600' : ''}>
                      Expérience pour ce poste {isFieldEmpty(profileData.experiencePoste) && <span className="text-red-500">*</span>}
                    </Label>
                    <Select value={profileData.experiencePoste} onValueChange={(value) => updateField('experiencePoste', value)} disabled={!isEditingSearch}>
                      <SelectTrigger className={`${getFieldClasses(profileData.experiencePoste, isEditingSearch)} ${isFieldEmpty(profileData.experiencePoste) && !isEditingSearch ? 'border-red-400 ring-1 ring-red-300' : ''}`}>
                        <SelectValue placeholder="Sélectionnez votre expérience" />
                      </SelectTrigger>
                      <SelectContent>
                        {filteredPositionExperienceOptions.map((exp) => (
                          <SelectItem key={exp.value} value={exp.value}>{exp.label}</SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                )}
              </div>
            </div>

            {/* 2. Expérience globale en centres d'appels */}
            <div className="space-y-4">
              <h3 className="text-base font-semibold text-foreground border-b border-border pb-2">Expérience globale en centres d'appels</h3>
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 sm:gap-4">
                <div className="space-y-2">
                  <Label htmlFor="experienceGlobale" className={isFieldEmpty(profileData.experienceGlobale) ? 'text-red-600' : ''}>
                    Expérience en centre d'appels {isFieldEmpty(profileData.experienceGlobale) && <span className="text-red-500">*</span>}
                  </Label>
                  <Select value={profileData.experienceGlobale} onValueChange={(value) => updateField('experienceGlobale', value)} disabled={!isEditingSearch}>
                    <SelectTrigger className={`${getFieldClasses(profileData.experienceGlobale, isEditingSearch)} ${isFieldEmpty(profileData.experienceGlobale) && !isEditingSearch ? 'border-red-400 ring-1 ring-red-300' : ''}`}>
                      <SelectValue placeholder="Sélectionnez votre expérience">
                        {profileData.experienceGlobale === '0' ? 'Aucune expérience' : profileData.experienceGlobale === '1' ? '0 - 6 mois' : profileData.experienceGlobale === '2' ? '6 mois - 1 an' : profileData.experienceGlobale === '3' ? '1 an - 2 ans' : profileData.experienceGlobale === '4' ? '2 ans - 3 ans' : profileData.experienceGlobale === '5' ? '3 ans - 5 ans' : profileData.experienceGlobale === '6' ? '5 ans - 7 ans' : profileData.experienceGlobale === '7' ? 'Plus de 7 ans' : 'Sélectionnez votre expérience'}
                      </SelectValue>
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="0">Aucune expérience</SelectItem>
                      <SelectItem value="1">0 - 6 mois</SelectItem>
                      <SelectItem value="2">6 mois - 1 an</SelectItem>
                      <SelectItem value="3">1 an - 2 ans</SelectItem>
                      <SelectItem value="4">2 ans - 3 ans</SelectItem>
                      <SelectItem value="5">3 ans - 5 ans</SelectItem>
                      <SelectItem value="6">5 ans - 7 ans</SelectItem>
                      <SelectItem value="7">Plus de 7 ans</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>
            </div>

            {/* 3. Activités et Opérations */}
            <div className="space-y-4">
              <h3 className="text-base font-semibold text-foreground border-b border-border pb-2">Activités & Opérations</h3>
              <div className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="activitePrincipale" className={isFieldEmpty(profileData.activitePrincipale) ? 'text-red-600' : ''}>
                    Activité principale {isFieldEmpty(profileData.activitePrincipale) && <span className="text-red-500">*</span>}
                  </Label>
                  <Select value={profileData.activitePrincipale} onValueChange={(value) => updateField('activitePrincipale', value)} disabled={!isEditingSearch}>
                    <SelectTrigger className={`${getFieldClasses(profileData.activitePrincipale, isEditingSearch)} ${isFieldEmpty(profileData.activitePrincipale) && !isEditingSearch ? 'border-red-400 ring-1 ring-red-300' : ''}`}>
                      <SelectValue placeholder="Sélectionnez l'activité">
                        {profileData.activitePrincipale === '1' ? 'Télévente' : profileData.activitePrincipale === '2' ? 'Téléprospection' : profileData.activitePrincipale === '3' ? 'Prise de RDV' : profileData.activitePrincipale === '4' ? 'Service client' : 'Sélectionnez l\'activité'}
                      </SelectValue>
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="1">Télévente</SelectItem>
                      <SelectItem value="2">Téléprospection</SelectItem>
                      <SelectItem value="3">Prise de RDV</SelectItem>
                      <SelectItem value="4">Service client</SelectItem>
                    </SelectContent>
                  </Select>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="experienceActivitePrincipale" className={isFieldEmpty(profileData.experienceActivitePrincipale) ? 'text-red-600' : ''}>
                    Expérience dans cette activité {isFieldEmpty(profileData.experienceActivitePrincipale) && <span className="text-red-500">*</span>}
                  </Label>
                  {hasMinimalExperience ? (
                    <div className="flex items-center gap-2">
                      <Badge variant="secondary" className="bg-emerald-100 text-emerald-800 border-emerald-200">0 - 6 mois</Badge>
                      <span className="text-xs text-muted-foreground">Sélectionné automatiquement</span>
                    </div>
                  ) : (
                    <Select value={profileData.experienceActivitePrincipale} onValueChange={(value) => updateField('experienceActivitePrincipale', value)} disabled={!isEditingSearch}>
                      <SelectTrigger className={getFieldClasses(profileData.experienceActivitePrincipale, isEditingSearch)}>
                        <SelectValue placeholder="Sélectionnez votre expérience" />
                      </SelectTrigger>
                      <SelectContent>{filteredActivityExperienceOptions.map((exp) => (<SelectItem key={exp.value} value={exp.value}>{exp.label}</SelectItem>))}</SelectContent>
                    </Select>
                  )}
                </div>

                <div className="space-y-2">
                  <Label htmlFor="operation1" className={isFieldEmpty(profileData.operation1) ? 'text-red-600' : ''}>
                    Opération 1 {isFieldEmpty(profileData.operation1) && <span className="text-red-500">*</span>}
                  </Label>
                  <Select value={profileData.operation1} onValueChange={(value) => updateField('operation1', value)} disabled={!isEditingSearch}>
                    <SelectTrigger className={`${getFieldClasses(profileData.operation1, isEditingSearch)} ${isFieldEmpty(profileData.operation1) && !isEditingSearch ? 'border-red-400 ring-1 ring-red-300' : ''}`}>
                      <SelectValue placeholder="Sélectionnez l'opération">{getOperationLabel(profileData.operation1)}</SelectValue>
                    </SelectTrigger>
                    <SelectContent>{filteredOperations.map((op) => (<SelectItem key={op.value} value={op.value}>{op.label}</SelectItem>))}</SelectContent>
                  </Select>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="experienceOperation1" className={isFieldEmpty(profileData.experienceOperation1) ? 'text-red-600' : ''}>
                    Expérience pour cette opération {isFieldEmpty(profileData.experienceOperation1) && <span className="text-red-500">*</span>}
                  </Label>
                  {hasMinimalExperience ? (
                    <div className="flex items-center gap-2">
                      <Badge variant="secondary" className="bg-emerald-100 text-emerald-800 border-emerald-200">0 - 6 mois</Badge>
                      <span className="text-xs text-muted-foreground">Sélectionné automatiquement</span>
                    </div>
                  ) : (
                    <Select value={profileData.experienceOperation1} onValueChange={(value) => updateField('experienceOperation1', value)} disabled={!isEditingSearch}>
                      <SelectTrigger className={getFieldClasses(profileData.experienceOperation1, isEditingSearch)}>
                        <SelectValue placeholder="Sélectionnez votre expérience" />
                      </SelectTrigger>
                      <SelectContent>{filteredOperation1ExperienceOptions.map((exp) => (<SelectItem key={exp.value} value={exp.value}>{exp.label}</SelectItem>))}</SelectContent>
                    </Select>
                  )}
                </div>

                {profileData.operation2 && (
                  <>
                    <div className="space-y-2">
                      <Label htmlFor="operation2">Opération 2 <span className="text-gray-500 text-sm">(Optionnelle)</span></Label>
                      <Select value={profileData.operation2} onValueChange={(value) => updateField('operation2', value)} disabled={!isEditingSearch}>
                        <SelectTrigger><SelectValue placeholder="Sélectionnez l'opération">{getOperationLabel(profileData.operation2)}</SelectValue></SelectTrigger>
                        <SelectContent>{filteredOperations.filter(op => op.value !== profileData.operation1).map((op) => (<SelectItem key={op.value} value={op.value}>{op.label}</SelectItem>))}</SelectContent>
                      </Select>
                    </div>
                    <div className="space-y-2">
                      <Label htmlFor="experienceOperation2">Expérience pour cette opération</Label>
                      {hasMinimalExperience ? (
                        <div className="flex items-center gap-2">
                          <Badge variant="secondary" className="bg-emerald-100 text-emerald-800 border-emerald-200">0 - 6 mois</Badge>
                          <span className="text-xs text-muted-foreground">Sélectionné automatiquement</span>
                        </div>
                      ) : (
                        <Select value={profileData.experienceOperation2} onValueChange={(value) => updateField('experienceOperation2', value)} disabled={!isEditingSearch}>
                          <SelectTrigger><SelectValue placeholder="Sélectionnez votre expérience" /></SelectTrigger>
                          <SelectContent>{filteredOperation2ExperienceOptions.map((exp) => (<SelectItem key={exp.value} value={exp.value}>{exp.label}</SelectItem>))}</SelectContent>
                        </Select>
                      )}
                    </div>
                  </>
                )}
              </div>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

// Helper function to get operation label
const getOperationLabel = (value: string): string => {
  const operations: Record<string, string> = {
    '1': 'Énergie Électricité & Gaz', '2': 'Énergie Photovoltaïque - Panneaux solaires',
    '3': 'Télécommunication B2B - B2C', '4': 'Mutuelle Santé - Assurance',
    '5': 'Voyance - Astrologie', '6': 'Produits & Compléments alimentaires',
    '7': 'Sites web & App mobiles', '8': 'Télésecretariat',
    '9': 'Dons humanitaires', '10': 'Qualification de fichiers',
    '11': 'Création de trafic - Jeux de concours', '12': 'Tourisme médical',
    '13': 'Traitement de dossiers administratifs', '14': 'CPF - Formations',
    '15': 'Gestion de commandes', '16': 'Modération de sites web',
    '17': 'Gestion des réservations'
  };
  return operations[value] || 'Sélectionnez l\'opération';
};

export default ProfileSection;
