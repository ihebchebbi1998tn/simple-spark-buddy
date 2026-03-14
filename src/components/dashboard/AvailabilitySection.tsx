import { useState, useEffect } from 'react';
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group';
import { Edit3, Save, Calendar } from 'lucide-react';
import { toast } from '@/hooks/use-toast';
import { useFormData } from '@/hooks/useFormData';
import { AVAILABILITY_OPTIONS } from '@/utils/constants';
import { createAutoSaveHandler } from '@/utils/fieldHelpers';
import { candidateService, type CandidateProfile } from '@/services/candidateService';

interface AvailabilitySectionProps {
  profile: CandidateProfile | null;
  onRefresh: () => void;
}

const AvailabilitySection = ({ profile, onRefresh }: AvailabilitySectionProps) => {
  const [isEditing, setIsEditing] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  const [isInitializing, setIsInitializing] = useState(true);

  const { formData: availabilityData, updateField, updateFieldsSilently } = useFormData({
    initialData: { 
      // Use only 'availability' field - work_mode is for a different purpose (présentiel/télétravail/hybride)
      disponibilite: profile?.availability?.availability || 'immédiate',
      commentaires: ''
    },
    onAutoSave: async (data) => {
      // Don't auto-save during initial data load
      if (isInitializing) {
        console.log("⏭️ Skipping availability auto-save during initialization");
        return;
      }
      
      setIsSaving(true);
      console.log('💾 Availability auto-save initiated for authenticated user');
      console.log('📋 Data to save:', { availability: data.disponibilite });
      
      try {
        // Backend extracts candidate ID from JWT token automatically
        // Only send availability field - work_mode/work_time/work_park are managed by ContractPreferencesSection
        await candidateService.updateAvailability({
          availability: data.disponibilite
        });
        console.log('✅ Availability updated via /api/candidates/availability for authenticated user');
        // Don't refresh here to avoid infinite loop - data is already up to date
      } catch (error) {
        console.error('❌ Availability auto-save failed:', error);
        toast({
          title: 'Erreur de sauvegarde',
          description: 'Impossible de sauvegarder la disponibilité.',
          variant: 'destructive',
        });
      } finally {
        setIsSaving(false);
      }
    }
  });

  // Update form data when profile changes (silently to avoid triggering auto-save)
  useEffect(() => {
    if (profile?.availability) {
      // Debug: log the raw availability data from backend
      console.log("📊 Raw availability data from profile:", {
        availability: profile.availability.availability,
        work_mode: profile.availability.work_mode,
        fullAvailability: profile.availability
      });
      
      // Use ONLY the 'availability' field - don't fallback to work_mode (which is for a different purpose)
      const availabilityValue = profile.availability.availability || 'immédiate';
      console.log("📊 Resolved availability value:", availabilityValue);
      
      setIsInitializing(true); // Prevent auto-save during initial load
      updateFieldsSilently({ disponibilite: availabilityValue });
      // Allow auto-save after initial load completes
      setTimeout(() => setIsInitializing(false), 500);
    }
  }, [profile, updateFieldsSilently]);

  const handleSave = async () => {
    // Trigger immediate save instead of waiting for auto-save debounce
    setIsSaving(true);
    console.log('💾 Manual save - sending availability:', availabilityData.disponibilite);
    try {
      // Only send the availability field - work_mode is managed by ContractPreferencesSection
      const response = await candidateService.updateAvailability({
        availability: availabilityData.disponibilite
      });
      console.log('✅ Save response:', response);
      toast({
        title: "Disponibilité mise à jour",
        description: `Votre disponibilité a été changée.`,
        className: "bg-green-50 border-green-200 text-green-800",
        duration: 3000,
      });
      setIsEditing(false);
      // Refresh the profile to ensure we have the latest data from backend
      onRefresh();
    } catch (error) {
      console.error('❌ Save failed:', error);
      toast({
        title: 'Erreur',
        description: 'Impossible de sauvegarder.',
        variant: 'destructive',
      });
    } finally {
      setIsSaving(false);
    }
  };

  const getAvailabilityInfo = (value: string) => {
    const option = AVAILABILITY_OPTIONS.find(opt => opt.value === value);
    return {
      label: option?.label || 'Non définie',
      icon: option?.icon || '❓',
      color: option?.color || 'from-gray-50 to-slate-50 border-gray-300'
    };
  };

  return (
    <div className="space-y-6">
      <Card className="border border-border bg-card">
        <CardContent className="pt-6">
          <div className="flex items-center justify-between mb-6">
            <h2 className="text-lg font-semibold text-foreground">Ma Disponibilité</h2>
            <Button
              variant={isEditing ? "default" : "outline"}
              size="sm"
              onClick={isEditing ? handleSave : () => setIsEditing(true)}
            >
              {isEditing ? <Save className="h-4 w-4 mr-2" /> : <Edit3 className="h-4 w-4 mr-2" />}
              {isEditing ? 'Sauvegarder' : 'Modifier'}
            </Button>
          </div>

          <div className="space-y-6">
          {/* Disponibilité */}
          <div className="space-y-4">
            <div className="flex items-center gap-2">
              <Calendar className="h-5 w-5 text-muted-foreground" />
              <Label className="font-bold text-foreground text-base">
                Quelle est votre disponibilité pour un nouveau poste ?
              </Label>
            </div>
            <p className="text-sm text-muted-foreground mb-4">
              Indiquez votre disponibilité pour maximiser vos chances d'être contacté(e) rapidement par les recruteurs.
            </p>

            {isEditing ? (
              <RadioGroup
                value={availabilityData.disponibilite}
                onValueChange={(value) => updateField('disponibilite', value)}
                className="space-y-3"
              >
                {AVAILABILITY_OPTIONS.map((option) => (
                  <div
                    key={option.value}
                    className={`flex items-center space-x-3 p-4 border-2 rounded-lg cursor-pointer transition-all ${
                      availabilityData.disponibilite === option.value
                        ? `bg-gradient-to-r ${option.color}`
                        : 'border-border hover:border-muted-foreground/30 bg-card'
                    }`}
                  >
                    <RadioGroupItem value={option.value} id={option.value} />
                    <Label htmlFor={option.value} className="flex items-center gap-3 cursor-pointer flex-1">
                      <span className="text-2xl">{option.icon}</span>
                      <span className="font-semibold text-sm">{option.label}</span>
                    </Label>
                  </div>
                ))}
              </RadioGroup>
            ) : (
              <div className={`p-5 border-2 rounded-lg bg-gradient-to-r ${getAvailabilityInfo(availabilityData.disponibilite).color}`}>
                <div className="flex items-center gap-3">
                  <span className="text-3xl">{getAvailabilityInfo(availabilityData.disponibilite).icon}</span>
                  <div>
                    <span className="font-bold text-base block text-foreground">
                      {getAvailabilityInfo(availabilityData.disponibilite).label}
                    </span>
                    <span className="text-sm text-muted-foreground">Votre disponibilité actuelle</span>
                  </div>
                </div>
              </div>
            )}
          </div>

          {/* Info Box */}
          <div className="bg-muted border border-border rounded-lg p-4">
            <h4 className="font-bold text-foreground mb-3">
              Conseils pour maximiser vos opportunités
            </h4>
            <ul className="space-y-2 text-sm text-muted-foreground">
              <li className="flex items-start gap-2">
                <span className="font-bold mt-0.5 text-primary">•</span>
                <span><strong className="text-foreground">Disponibilité immédiate :</strong> Augmente vos chances de +80% d'être contacté rapidement</span>
              </li>
              <li className="flex items-start gap-2">
                <span className="font-bold mt-0.5 text-primary">•</span>
                <span><strong className="text-foreground">En poste mais ouvert(e) :</strong> Idéal si vous explorez de nouvelles opportunités sans urgence</span>
              </li>
              <li className="flex items-start gap-2">
                <span className="font-bold mt-0.5 text-primary">•</span>
                <span><strong className="text-foreground">Délais courts (1-2 semaines) :</strong> Excellent compromis entre préparation et réactivité</span>
              </li>
            </ul>
          </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default AvailabilitySection;
