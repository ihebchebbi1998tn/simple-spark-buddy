import { useState, useEffect } from 'react';
import { candidateService, type CandidateProfile } from '@/services/candidate/candidateService';
import { authService } from '@/services/candidate/authService';
import { useToast } from '@/hooks/use-toast';

export function useCandidateProfile() {
  const [profile, setProfile] = useState<CandidateProfile | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const { toast } = useToast();

  const loadProfile = async () => {
    try {
      setLoading(true);
      setError(null);

      // Check if user is authenticated
      if (!authService.isAuthenticated()) {
        throw new Error('Not authenticated');
      }

      console.log('🔍 Fetching profile data for authenticated user (JWT in request header)');
      const response = await candidateService.getProfile();

      if (response.success && response.data) {
      console.log('✅ Profile data loaded successfully from backend:', {
        candidateId: response.data.candidate?._id,
        email: response.data.candidate?.email,
        hasProfile: !!response.data.profile,
        hasAvailability: !!response.data.availability,
        hasTestScores: !!response.data.testScores,
        // Log notification settings explicitly
        notificationSettings: response.data.testScores ? {
          email_notifications: response.data.testScores.email_notifications,
          sms_notifications: response.data.testScores.sms_notifications,
          profile_visibility: response.data.testScores.profile_visibility,
          secure_data_sharing: response.data.testScores.secure_data_sharing,
          updated_at: response.data.testScores.updated_at
        } : null
      });
      setProfile(response.data);
      } else {
        throw new Error('Failed to load profile');
      }
    } catch (err: any) {
      console.error('❌ Error loading candidate profile:', err);
      setError(err.message || 'Failed to load profile');
      
      // Show error toast
      toast({
        title: 'Erreur',
        description: 'Impossible de charger votre profil. Veuillez réessayer.',
        variant: 'destructive',
      });
    } finally {
      setLoading(false);
    }
  };

  const refreshProfile = () => {
    loadProfile();
  };

  useEffect(() => {
    loadProfile();
  }, []);

  return {
    profile,
    loading,
    error,
    refreshProfile,
  };
}
