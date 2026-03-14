import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { Menu, Home, HelpCircle } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useCandidateProfile } from '@/hooks/useCandidateProfile';
import { activityLogService, ACTION_TYPES } from '@/services/activityLogService';
import { 
  SidebarProvider, 
  SidebarTrigger,
  SidebarInset 
} from '@/components/ui/sidebar';
import { Button } from '@/components/ui/button';
import { AppSidebar } from '@/components/dashboard/AppSidebar';
import { HelpModal } from '@/components/dashboard/HelpModal';
import { AlertsModal } from '@/components/dashboard/AlertsModal';
import { DashboardTour } from '@/components/dashboard/DashboardTour';
import DashboardOverview from '@/components/dashboard/DashboardOverview';
import ProfileSection from '@/components/dashboard/ProfileSection';
import ActivitiesSection from '@/components/dashboard/ActivitiesSection';
import LanguagesSection from '@/components/dashboard/LanguagesSection';
import CallCentersSection from '@/components/dashboard/CallCentersSection';
import ContractPreferencesSection from '@/components/dashboard/ContractPreferencesSection';
import AccessManagementSection from '@/components/dashboard/AccessManagementSection';
import DashboardLanguageTest from '@/components/dashboard/DashboardLanguageTest';
import ProximitySection from '@/components/dashboard/ProximitySection';
import AvailabilitySection from '@/components/dashboard/AvailabilitySection';

const Dashboard = () => {
  const navigate = useNavigate();
  const { t } = useTranslation('dashboard');
  const [activeSection, setActiveSection] = useState('overview');
  const [helpModalOpen, setHelpModalOpen] = useState(false);
  const [alertsModalOpen, setAlertsModalOpen] = useState(false);
  const [profileProgress, setProfileProgress] = useState(0);
  const [runTour, setRunTour] = useState(false);
  
  // Enable API integration to fetch real candidate data
  const { profile: fetchedProfile, loading, error, refreshProfile } = useCandidateProfile();
  
  // Local profile state that can be updated without refetching
  const [localProfile, setLocalProfile] = useState(fetchedProfile);
  
  // Sync local profile when fetched profile changes
  useEffect(() => {
    if (fetchedProfile) {
      setLocalProfile(fetchedProfile);
    }
  }, [fetchedProfile]);
  
  // Use local profile for rendering
  const profile = localProfile;
  
  // Update local profile when sections save data (no API call needed)
  const updateLocalProfile = (updates: Partial<typeof profile>) => {
    setLocalProfile(prev => {
      if (!prev) return prev;
      return {
        ...prev,
        ...updates,
        availability: {
          ...prev.availability,
          ...(updates.availability || {})
        }
      };
    });
  };

  // Log section changes as page views
  const handleSectionChange = (section: string) => {
    setActiveSection(section);
    const sectionActionMap: Record<string, number> = {
      'profile': ACTION_TYPES.PROFILE_VIEWED,
      'settings': ACTION_TYPES.SETTINGS_VIEWED,
    };
    activityLogService.log(sectionActionMap[section] || ACTION_TYPES.PAGE_VIEW, { section });
  };

  // Get section from URL params on mount
  useEffect(() => {
    const urlParams = new URLSearchParams(window.location.search);
    const section = urlParams.get('section');
    if (section) {
      setActiveSection(section);
    }
  }, []);

  // Scroll to top on mount, log dashboard view, and check for tour
  useEffect(() => {
    window.scrollTo(0, 0);
    activityLogService.log(ACTION_TYPES.DASHBOARD_VIEWED);
    
    // Check if this is the first visit
    const tourCompleted = localStorage.getItem('dashboard-tour-completed');
    if (!tourCompleted) {
      // Delay tour start to let the page render
      setTimeout(() => setRunTour(true), 1000);
    }
  }, []);

  // Calculate actual profile progress based on completed tasks (same logic as DashboardOverview)
  useEffect(() => {
    if (!profile) return;
    
    // Check if call centers have been selected (blacklist or whitelist has items)
    const hasCallCentersSelected = 
      (profile?.availability?.blacklist && profile.availability.blacklist.length > 0) ||
      (profile?.availability?.whitelist && profile.availability.whitelist.length > 0);

    // Check if proximity has been set (nearby cities selected)
    const hasProximitySet = 
      profile?.availability?.nearby_cities && profile.availability.nearby_cities.length > 0;

    // Check if additional activities have been added (requires operation AND experience for at least one)
    const hasAdditionalActivities = 
      (profile?.profile?.operation_1 && profile.profile.operation_1.trim() !== '' && 
       profile?.profile?.operation_1_experience && profile.profile.operation_1_experience.trim() !== '') ||
      (profile?.profile?.operation_2 && profile.profile.operation_2.trim() !== '' &&
       profile?.profile?.operation_2_experience && profile.profile.operation_2_experience.trim() !== '');

    // Check if at least one language test has been completed - check both standard and legacy field names
    const hasAnyTestCompleted = 
      profile?.testScores?.french_test_completed ||
      profile?.testScores?.english_test_completed ||
      (profile?.testScores?.italian_test_completed || profile?.testScores?.italiano_test_completed) ||
      (profile?.testScores?.spanish_test_completed || profile?.testScores?.espanol_test_completed) ||
      (profile?.testScores?.german_test_completed || profile?.testScores?.deutsch_test_completed);

    // Count completed tasks (4 total)
    const completedCount = [
      hasAnyTestCompleted,
      hasCallCentersSelected,
      hasProximitySet,
      hasAdditionalActivities
    ].filter(Boolean).length;

    // Calculate progress percentage (4 tasks = 100%)
    const progress = Math.round((completedCount / 4) * 100);
    setProfileProgress(progress);
    
    console.log('📊 Dashboard progress calculation:', {
      hasAnyTestCompleted,
      hasCallCentersSelected,
      hasProximitySet,
      hasAdditionalActivities,
      completedCount,
      progress
    });
  }, [profile]);

  const getProgressStatus = () => {
    if (profileProgress >= 100) return { color: 'green', status: 'complete', text: t('header.profileComplete') };
    if (profileProgress >= 70) return { color: 'orange', status: 'action', text: t('header.actionRequired') };
    return { color: 'red', status: 'urgent', text: t('header.profileIncomplete') };
  };

  const renderActiveSection = () => {
    // Show loading state while fetching profile data
    if (loading) {
      return (
        <div className="flex items-center justify-center min-h-[400px]">
          <div className="text-center space-y-4">
            <div className="w-12 h-12 border-4 border-primary border-t-transparent rounded-full animate-spin mx-auto"></div>
            <p className="text-muted-foreground">{t('loading.title')}</p>
          </div>
        </div>
      );
    }

    // Show error state if profile fetch failed
    if (error) {
      return (
        <div className="flex items-center justify-center min-h-[400px]">
          <div className="text-center space-y-4 max-w-md">
            <div className="w-12 h-12 bg-destructive/10 rounded-full flex items-center justify-center mx-auto">
              <svg className="w-6 h-6 text-destructive" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
              </svg>
            </div>
            <h3 className="text-lg font-semibold text-foreground">{t('loading.error')}</h3>
            <p className="text-sm text-muted-foreground">
              {error.includes('serveur') 
                ? t('loading.serverDown')
                : error}
            </p>
            <div className="flex flex-col gap-2">
              <button 
                onClick={refreshProfile}
                className="px-4 py-2 bg-primary text-primary-foreground rounded-lg hover:bg-primary/90 transition-colors"
              >
                {t('loading.retry')}
              </button>
              {error.includes('serveur') && (
                <p className="text-xs text-muted-foreground">
                  {t('loading.command')} <code className="bg-muted px-2 py-1 rounded">cd CCM_Backend_Dev-NEW && npm run dev</code>
                </p>
              )}
            </div>
          </div>
        </div>
      );
    }

    // Render sections with real profile data
    switch (activeSection) {
      case 'overview': 
        return <DashboardOverview onSectionChange={handleSectionChange} profile={profile} onRefresh={refreshProfile} />;
      case 'profile': 
        return <ProfileSection profile={profile} onRefresh={refreshProfile} />;
      case 'activities': 
        return <ActivitiesSection profile={profile} onRefresh={refreshProfile} />;
      case 'languages': 
        return <LanguagesSection profile={profile} onRefresh={refreshProfile} />;
      case 'callcenters': 
        return <CallCentersSection profile={profile} onRefresh={refreshProfile} onProfileUpdate={updateLocalProfile} />;
      case 'language-test': 
        return <DashboardLanguageTest profile={profile} onRefresh={refreshProfile} />;
      case 'proximity': 
        return <ProximitySection profile={profile} onRefresh={refreshProfile} onProfileUpdate={updateLocalProfile} />;
      case 'availability': 
        return <AvailabilitySection profile={profile} onRefresh={refreshProfile} />;
      case 'preferences': 
        return <ContractPreferencesSection profile={profile} onRefresh={refreshProfile} />;
      case 'access': 
        return <AccessManagementSection profile={profile} onSectionChange={handleSectionChange} />;
      default: 
        return <DashboardOverview onSectionChange={handleSectionChange} profile={profile} />;
    }
  };

  const handleTourFinish = () => {
    setRunTour(false);
    localStorage.setItem('dashboard-tour-completed', 'true');
  };

  return (
    <SidebarProvider defaultOpen={true}>
      <DashboardTour run={runTour} onFinish={handleTourFinish} />
      <div className="flex min-h-screen w-full bg-gradient-to-br from-slate-50 via-blue-50/30 to-indigo-50/20 overflow-hidden">
        <AppSidebar 
          activeSection={activeSection} 
          onSectionChange={handleSectionChange}
          onHelpClick={() => setHelpModalOpen(true)}
        />

        <SidebarInset className="flex-1">
          {/* Professional Header with Progress - Mobile Optimized */}
          <header className="sticky top-0 z-50 bg-white/95 backdrop-blur-sm border-b border-border/50">
            <div className="px-3 sm:px-4 md:px-6 lg:px-8">
              {/* Main Header Row - Mobile Responsive */}
              <div className="flex h-14 sm:h-20 md:h-24 items-center justify-between gap-2 sm:gap-4">
                <div className="flex items-center gap-2 sm:gap-4 flex-1 min-w-0">
                  <SidebarTrigger className="md:hidden p-2 hover:bg-secondary/80 rounded-lg transition-colors shrink-0">
                    <Menu className="h-4 w-4 sm:h-5 sm:w-5" />
                  </SidebarTrigger>
                  
                  <div className="flex items-center gap-2 sm:gap-3 min-w-0">
                    <div className="w-8 h-8 sm:w-10 sm:h-10 bg-gradient-to-br from-primary to-primary/80 rounded-lg sm:rounded-xl flex items-center justify-center shadow-lg shrink-0">
                      <svg className="w-4 h-4 sm:w-5 sm:h-5 text-primary-foreground" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                      </svg>
                    </div>
                      <div className="min-w-0">
                      <h1 className="text-sm sm:text-lg md:text-xl lg:text-2xl font-bold tracking-tight text-foreground truncate">
                        {t('header.candidateSpace')}
                      </h1>
                      <div className="flex items-center gap-2">
                        <p className="text-[10px] sm:text-xs text-muted-foreground truncate">
                          {profile?.candidate ? `${profile.candidate.name} ${profile.candidate.surname}` : t('header.loading')}
                        </p>
                        {/* Progress indicator on same line as user name */}
                        <div className="flex items-center gap-1 px-2 py-1 bg-secondary/50 rounded-full">
                          <div className={`w-1.5 h-1.5 rounded-full transition-all duration-500 ${
                            profileProgress >= 100 ? 'bg-green-500 animate-pulse' :
                            profileProgress >= 70 ? 'bg-orange-500 animate-pulse' :
                            'bg-red-500 animate-pulse'
                          }`}></div>
                          <span className="text-[10px] font-medium text-muted-foreground">{profileProgress}%</span>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                {/* Right side with action buttons */}
                <div className="flex items-center gap-0.5 sm:gap-1.5 shrink-0">
                  {/* Home button - visible on mobile */}
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => navigate('/')}
                    className="flex flex-col items-center gap-0.5 px-1.5 sm:px-3 py-2 hover:bg-secondary/80 rounded-lg transition-all duration-200 group md:hidden h-auto"
                    title={t('header.home')}
                  >
                    <Home className="h-5 w-5 sm:h-6 sm:w-6 text-muted-foreground group-hover:text-foreground transition-colors" />
                    <span className="text-[10px] sm:text-xs font-medium text-muted-foreground group-hover:text-foreground transition-colors hidden sm:inline">{t('header.home')}</span>
                  </Button>

                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => setHelpModalOpen(true)}
                    data-tour="header-support"
                    className="flex flex-col items-center gap-0.5 px-1.5 sm:px-3 py-2 hover:bg-secondary/80 rounded-lg transition-all duration-200 group h-auto"
                    title={t('header.support')}
                  >
                    <svg className="h-5 w-5 sm:h-6 sm:w-6 text-muted-foreground group-hover:text-foreground transition-colors" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M18.364 5.636l-3.536 3.536m0 5.656l3.536 3.536M9.172 9.172L5.636 5.636m3.536 9.192l-3.536 3.536M21 12a9 9 0 11-18 0 9 9 0 0118 0zm-5 0a4 4 0 11-8 0 4 4 0 018 0z" />
                    </svg>
                    <span className="text-[10px] sm:text-xs font-medium text-muted-foreground group-hover:text-foreground transition-colors hidden sm:inline">{t('header.support')}</span>
                  </Button>
                  
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => handleSectionChange('language-test')}
                    data-tour="header-test"
                    className="flex flex-col items-center gap-0.5 px-1.5 sm:px-3 py-2 hover:bg-secondary/80 rounded-lg transition-all duration-200 group h-auto"
                    title={t('header.myTest')}
                  >
                    <svg className="h-5 w-5 sm:h-6 sm:w-6 text-muted-foreground group-hover:text-foreground transition-colors" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4" />
                    </svg>
                    <span className="text-[10px] sm:text-xs font-medium text-muted-foreground group-hover:text-foreground transition-colors hidden sm:inline">{t('header.myTest')}</span>
                  </Button>
                  
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => setAlertsModalOpen(true)}
                    data-tour="header-alerts"
                    className="flex flex-col items-center gap-0.5 px-1.5 sm:px-3 py-2 hover:bg-secondary/80 rounded-lg transition-all duration-200 group relative h-auto"
                    title={t('header.alerts')}
                  >
                    <svg className="h-5 w-5 sm:h-6 sm:w-6 text-muted-foreground group-hover:text-foreground transition-colors" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" />
                    </svg>
                    <span className="text-[10px] sm:text-xs font-medium text-muted-foreground group-hover:text-foreground transition-colors hidden sm:inline">{t('header.alerts')}</span>
                    <div className="absolute top-0.5 right-0.5 w-2 h-2 bg-destructive rounded-full animate-pulse"></div>
                  </Button>

                  {/* Replay Tour Button - hidden on very small screens */}
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => setRunTour(true)}
                    className="hidden sm:flex flex-col items-center gap-0.5 px-1.5 sm:px-3 py-2 hover:bg-secondary/80 rounded-lg transition-all duration-200 group h-auto"
                    title={t('header.guide')}
                  >
                    <HelpCircle className="h-5 w-5 sm:h-6 sm:w-6 text-muted-foreground group-hover:text-foreground transition-colors" />
                    <span className="text-[10px] sm:text-xs font-medium text-muted-foreground group-hover:text-foreground transition-colors hidden sm:inline">{t('header.guide')}</span>
                  </Button>
                </div>
              </div>

              {/* Progress Bar - Mobile Optimized */}
              <div className="pb-2 sm:pb-4 px-2 sm:px-0" data-tour="header-progress">
                <div className="flex flex-col sm:flex-row sm:items-center gap-1 sm:gap-4 mb-1.5 sm:mb-2">
                  <span className="text-xs sm:text-sm font-medium text-foreground">{t('header.completeProfile')}</span>
                  <span className="text-[10px] sm:text-xs text-muted-foreground">
                    <span className={`font-semibold ${profileProgress >= 100 ? 'text-green-500' : profileProgress >= 50 ? 'text-orange-500' : 'text-red-500'}`}>{Math.round(profileProgress / 25)}</span>/4 {t('header.stepsCompleted')}
                  </span>
                </div>
                <div className="relative pr-2 sm:pr-0">
                  <div className="w-full bg-secondary/60 rounded-full h-1.5 sm:h-2 overflow-hidden">
                    <motion.div 
                      className={`h-full rounded-full shadow-sm transition-all duration-500 ${
                        profileProgress >= 100 ? 'bg-gradient-to-r from-green-500 to-green-400' :
                        profileProgress >= 70 ? 'bg-gradient-to-r from-orange-500 to-orange-400' :
                        'bg-gradient-to-r from-red-500 to-red-400'
                      }`}
                      initial={{ width: 0 }}
                      animate={{ width: `${profileProgress}%` }}
                      transition={{ duration: 1.2, ease: "easeOut" }}
                    />
                  </div>
                  <div className="absolute top-0 right-0 -translate-y-0.5 sm:-translate-y-1 -translate-x-1 sm:translate-x-1">
                    <div className={`w-3 h-3 sm:w-4 sm:h-4 rounded-full shadow-lg transition-all duration-500 ${
                      profileProgress >= 100 ? 'bg-green-500 animate-pulse shadow-green-500/50' :
                      profileProgress >= 70 ? 'bg-orange-500 animate-pulse shadow-orange-500/50' :
                      'bg-red-500 animate-pulse shadow-red-500/50'
                    }`}></div>
                  </div>
                </div>
                <div className="flex items-center gap-2 mt-1.5 sm:mt-2">
                  {profileProgress >= 100 ? (
                    <div className="flex items-center gap-1 text-[10px] sm:text-xs font-medium text-green-600">
                      <div className="w-1.5 h-1.5 sm:w-2 sm:h-2 bg-green-500 rounded-full animate-pulse"></div>
                      {t('header.profileComplete')}
                    </div>
                  ) : profileProgress >= 70 ? (
                    <div className="flex items-center gap-1 text-[10px] sm:text-xs font-medium text-orange-600">
                      <div className="w-1.5 h-1.5 sm:w-2 sm:h-2 bg-orange-500 rounded-full animate-pulse"></div>
                      {t('header.actionRequired')}
                    </div>
                  ) : (
                    <div className="flex items-center gap-1 text-[10px] sm:text-xs font-medium text-red-600">
                      <div className="w-1.5 h-1.5 sm:w-2 sm:h-2 bg-red-500 rounded-full animate-pulse"></div>
                      {t('header.urgentProfileIncomplete')}
                    </div>
                  )}
                </div>
              </div>
            </div>
          </header>

          {/* Main Content - Mobile Responsive */}
          <main className="flex-1 p-3 sm:p-4 md:p-6 lg:p-8 bg-gradient-to-b from-transparent to-blue-50/20">
            <motion.div
              key={activeSection}
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.3, ease: "easeOut" }}
              className="max-w-7xl mx-auto w-full bg-white/40 backdrop-blur-sm rounded-2xl shadow-lg p-4 sm:p-6"
            >
              <div className="space-y-4 sm:space-y-6">
                {renderActiveSection()}
              </div>
            </motion.div>
          </main>
        </SidebarInset>

        <HelpModal 
          open={helpModalOpen} 
          onOpenChange={setHelpModalOpen} 
        />
        
        <AlertsModal 
          open={alertsModalOpen} 
          onOpenChange={setAlertsModalOpen} 
        />
      </div>
    </SidebarProvider>
  );
};

export default Dashboard;
