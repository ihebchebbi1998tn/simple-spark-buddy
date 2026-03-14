"use client"

import { useState, useEffect } from "react"
import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Label } from "@/components/ui/label"
import { Badge } from "@/components/ui/badge"
import { Edit3, Save, Plus, Trash2, Star, CheckCircle2 } from "lucide-react"
import { toast } from "@/hooks/use-toast"
import { useFormData } from "@/hooks/useFormData"
import { LANGUAGES, LANGUAGE_LEVELS } from "@/utils/constants"
import { LanguageSelector } from "@/components/forms/LanguageSelector"
import { LevelSelector } from "@/components/forms/LevelSelector"
import { createAutoSaveHandler } from "@/utils/fieldHelpers"
import { candidateService, type CandidateProfile } from '@/services/candidateService';

interface LanguagesSectionProps {
  profile: CandidateProfile | null;
  onRefresh: () => void;
}

const LanguagesSection = ({ profile, onRefresh }: LanguagesSectionProps) => {
  // Initialize languages from backend profile
  const initializeLanguages = () => {
    const langs = [
      { 
        id: 1, 
        langue: profile?.profile?.native_language || "", 
        niveau: "", 
        type: "maternelle" 
      },
      { 
        id: 2, 
        langue: profile?.profile?.foreign_language_1 || "", 
        niveau: profile?.profile?.foreign_language_1_level || "", 
        type: "principale" 
      },
    ];
    
    // Add secondary language if it exists
    if (profile?.profile?.foreign_language_2) {
      langs.push({
        id: 3,
        langue: profile.profile.foreign_language_2,
        niveau: profile.profile.foreign_language_2_level || "",
        type: "secondaire"
      });
    }
    
    return langs;
  };

  const [languages, setLanguages] = useState(initializeLanguages());
  const [hasSecondForeignLanguage, setHasSecondForeignLanguage] = useState<boolean | null>(
    profile?.profile?.foreign_language_2 ? true : null
  );
  const [isSaving, setIsSaving] = useState(false);

  // Update languages when profile changes
  useEffect(() => {
    if (profile) {
      const initializedLanguages = initializeLanguages();
      setLanguages(initializedLanguages);
      setHasSecondForeignLanguage(profile.profile?.foreign_language_2 ? true : null);
      
      // Only auto-enable editing if required fields are empty
      const hasEmptyRequired = !initializedLanguages[0]?.langue || 
                                !initializedLanguages[1]?.langue || 
                                !initializedLanguages[1]?.niveau;
      if (hasEmptyRequired) {
        setIsEditing(true);
      }
    }
  }, [profile]);

  const hasEmptyLanguages = !languages[0].langue || !languages[1].langue || !languages[1].niveau
  // Default to view mode, only edit if user clicks edit OR if required fields are empty
  const [isEditing, setIsEditing] = useState(false)

  const { formData: bilingualData, updateField } = useFormData({
    initialData: { },
    onAutoSave: createAutoSaveHandler({ languages, hasSecondForeignLanguage }, undefined, 'Auto-save langues')
  })

  const handleSave = async () => {
    if (!languages[0].langue || !languages[1].langue || !languages[1].niveau) {
      toast({
        title: "Champs manquants",
        description: "Veuillez remplir au minimum la langue maternelle et la langue étrangère principale.",
        className: "bg-red-50 border-red-200 text-red-800",
      })
      return
    }

    if (hasSecondForeignLanguage === null && languages[1].niveau) {
      toast({
        title: "Réponse manquante",
        description: "Veuillez indiquer si vous maîtrisez une autre langue étrangère.",
        className: "bg-red-50 border-red-200 text-red-800",
      })
      return
    }

    if (hasSecondForeignLanguage && (!languages[2]?.langue || !languages[2]?.niveau)) {
      toast({
        title: "Champs manquants",
        description: "Veuillez compléter les informations de la langue étrangère secondaire.",
        className: "bg-red-50 border-red-200 text-red-800",
      })
      return
    }
    
    setIsSaving(true);
    console.log('💾 Languages save initiated for authenticated user');
    console.log('📋 Data to save:', {
      native: languages[0].langue,
      foreign1: { langue: languages[1].langue, niveau: languages[1].niveau },
      foreign2: languages[2] ? { langue: languages[2].langue, niveau: languages[2].niveau } : null
    });
    
    try {
      // Backend extracts candidate ID from JWT token automatically
      await candidateService.updateProfile({
        native_language: languages[0].langue,
        foreign_language_1: languages[1].langue,
        foreign_language_1_level: languages[1].niveau,
        foreign_language_2: languages[2]?.langue || null,
        foreign_language_2_level: languages[2]?.niveau || null,
        is_bilingual: hasSecondForeignLanguage || false
      });
      
      console.log('✅ Languages updated via /api/candidates/profile for authenticated user');
      
      const completedCount = languages.filter((lang) => lang.langue).length
      toast({
        title: "Langues mises à jour",
        description: `${completedCount} langue(s) sauvegardée(s) avec succès.`,
        className: "bg-green-50 border-green-200 text-green-800",
        duration: 3000,
      })
      setIsEditing(false)
      // Refresh profile to ensure data is synced
      onRefresh();
    } catch (error) {
      console.error('❌ Languages save failed:', error);
      toast({
        title: "Erreur de sauvegarde",
        description: "Impossible de sauvegarder les langues.",
        variant: "destructive",
      })
    } finally {
      setIsSaving(false);
    }
  }

  const updateLanguage = (id: number, field: string, value: string) => {
    setLanguages((prev) => prev.map((lang) => (lang.id === id ? { ...lang, [field]: value } : lang)))
  }

  const handleSecondLanguageChoice = (choice: boolean) => {
    setHasSecondForeignLanguage(choice)
    if (choice && languages.length === 2) {
      setLanguages((prev) => [...prev, { id: 3, langue: "", niveau: "", type: "secondaire" }])
    } else if (!choice && languages.length === 3) {
      setLanguages((prev) => prev.filter(lang => lang.type !== "secondaire"))
    }
  }

  const getSelectedLanguages = () => {
    return languages.filter(lang => lang.langue).map(lang => lang.langue)
  }

  const getNiveauBadge = (niveau: string) => {
    const niveauInfo = LANGUAGE_LEVELS.find((n) => n.value === niveau)
    return niveauInfo || { label: "Non défini", color: "bg-gray-50 text-gray-600 border-gray-200", stars: 0 }
  }

  const renderStars = (count: number) => {
    return Array.from({ length: 5 }, (_, i) => (
      <Star key={i} className={`h-3 w-3 ${i < count ? "fill-yellow-400 text-yellow-400" : "text-gray-300"}`} />
    ))
  }

  const completedLanguages = languages.filter((lang) => lang.langue).length

  return (
    <div className="space-y-6">
      <Card className="border border-slate-200 shadow-sm bg-white">
        <CardContent className="pt-6">
          <div className="flex items-center justify-between">
            <h3 className="text-lg font-semibold text-slate-800">Compétences Linguistiques</h3>
            <Button
              variant={isEditing ? "default" : "outline"}
              size="sm"
              onClick={isEditing ? handleSave : () => setIsEditing(true)}
            >
              {isEditing ? <Save className="h-4 w-4 mr-2" /> : <Edit3 className="h-4 w-4 mr-2" />}
              {isEditing ? "Sauvegarder" : "Modifier"}
            </Button>
          </div>
        </CardContent>
      </Card>

      <div className="grid gap-4">
        {languages.map((language, index) => {
          const isMotherTongue = language.type === "maternelle"
          const isPrincipal = language.type === "principale"
          const isSecondary = language.type === "secondaire"
          const isComplete = isMotherTongue ? language.langue : (language.langue && language.niveau)
          const niveauInfo = getNiveauBadge(language.niveau)
          const selectedLanguage = LANGUAGES.find((l) => l.value === language.langue)
          const selectedLanguages = getSelectedLanguages()

          const getTitle = () => {
            if (isMotherTongue) return "Langue maternelle"
            if (isPrincipal) return "Langue étrangère principale"
            return "Langue étrangère secondaire"
          }

          const isMandatory = isMotherTongue || isPrincipal

          return (
            <Card
              key={language.id}
              className={`transition-all duration-200 ${
                isMandatory
                  ? "border-blue-200 bg-blue-50/30"
                  : isComplete
                    ? "border-green-200 bg-green-50/30"
                    : isEditing
                      ? "border-blue-200 bg-blue-50/30"
                      : "border-gray-200"
              }`}
            >
              <CardContent className="p-6">
                <div className="flex items-start justify-between mb-4">
                  <div className="flex items-center gap-2">
                    <span className="text-sm font-medium text-gray-600">
                      {getTitle()} {isMandatory && <span className="text-blue-600">(Obligatoire)</span>}
                    </span>
                    {isComplete && <CheckCircle2 className="h-4 w-4 text-green-600" />}
                  </div>
                </div>

                <div className="space-y-4">
                  <div className="space-y-2">
                    <Label className="text-sm font-medium">Langue</Label>
                    {isEditing ? (
                      <LanguageSelector
                        value={language.langue}
                        onChange={(value) => updateLanguage(language.id, "langue", value)}
                        disabled={false}
                        disabledLanguages={selectedLanguages.filter(lang => lang !== language.langue)}
                      />
                    ) : (
                      <div className="p-3 border rounded-md bg-white flex items-center gap-2">
                        {selectedLanguage && (
                          <>
                            <selectedLanguage.flag className="w-6 h-4 rounded shadow-sm" />
                            <span className="text-gray-900">{language.langue}</span>
                          </>
                        )}
                        {!language.langue && <span className="text-gray-400">Non définie</span>}
                      </div>
                    )}
                  </div>

                  {!isMotherTongue && (
                    <div className="space-y-2">
                      <Label className="text-sm font-medium">Niveau</Label>
                      {isEditing ? (
                        <LevelSelector
                          value={language.niveau}
                          onChange={(value) => updateLanguage(language.id, "niveau", value)}
                          disabled={false}
                        />
                      ) : (
                        <div className="p-3 border rounded-md bg-white">
                          {language.niveau ? (
                            <div className="flex items-center gap-2">
                              <Badge className={niveauInfo.color}>{niveauInfo.label}</Badge>
                              <div className="flex">{renderStars(niveauInfo.stars)}</div>
                            </div>
                          ) : (
                            <span className="text-gray-400">Non défini</span>
                          )}
                        </div>
                      )}
                    </div>
                  )}
                </div>
              </CardContent>
            </Card>
          )
        })}
      </div>

      {isEditing && languages[1]?.niveau && hasSecondForeignLanguage === null && (
        <Card className="border-2 border-blue-200 bg-blue-50/50">
          <CardContent className="pt-6">
            <div className="space-y-4">
              <Label className="text-sm font-medium text-blue-900">Maitrisez-vous une autre langue étrangère ?</Label>
              <div className="flex gap-2">
                <Button
                  type="button"
                  variant="outline"
                  size="sm"
                  onClick={() => handleSecondLanguageChoice(true)}
                  className="flex-1"
                >
                  Oui
                </Button>
                <Button
                  type="button"
                  variant="outline"
                  size="sm"
                  onClick={() => handleSecondLanguageChoice(false)}
                  className="flex-1"
                >
                  Non
                </Button>
              </div>
            </div>
          </CardContent>
        </Card>
      )}


      {!isEditing && completedLanguages > 0 && (
        <Card className="bg-gradient-to-r from-green-50 to-emerald-50 border-green-200">
          <CardContent className="pt-6">
            <h3 className="font-semibold text-green-900 mb-4 flex items-center gap-2">
              <CheckCircle2 className="h-5 w-5" />
              Résumé de vos compétences linguistiques
            </h3>
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-3">
              {languages
                .filter((lang) => lang.langue)
                .map((language) => {
                  const niveauInfo = getNiveauBadge(language.niveau)
                  const selectedLanguage = LANGUAGES.find((l) => l.value === language.langue)
                  const isMotherTongue = language.type === "maternelle"
                  return (
                    <div
                      key={language.id}
                      className="flex items-center justify-between p-3 bg-white border border-green-200 rounded-lg"
                    >
                      <div className="flex flex-col">
                        <span className="font-medium text-gray-900">{language.langue}</span>
                        {isMotherTongue ? (
                          <span className="text-xs text-gray-500">Langue maternelle</span>
                        ) : (
                          <div className="flex items-center gap-1 mt-1">{renderStars(niveauInfo.stars)}</div>
                        )}
                      </div>
                      {selectedLanguage && (
                        <div className="flex items-center gap-2">
                          <selectedLanguage.flag className="w-6 h-4 rounded shadow-sm" />
                          {!isMotherTongue && <Badge className={niveauInfo.color}>{niveauInfo.label}</Badge>}
                        </div>
                      )}
                    </div>
                  )
                })}
            </div>
          </CardContent>
        </Card>
      )}
    </div>
  )
}

export default LanguagesSection
