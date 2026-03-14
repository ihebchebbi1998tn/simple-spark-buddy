import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Progress } from '@/components/ui/progress';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { CheckCircle, XCircle, AlertCircle, ChevronRight, Check } from 'lucide-react';
import { type CandidateProfile } from '@/services/candidateService';
import { useTranslation } from 'react-i18next';

interface DashboardOverviewProps {
  onSectionChange?: (section: string) => void;
  profile: CandidateProfile | null;
  onRefresh?: () => void;
}

const DashboardOverview = ({ onSectionChange, profile, onRefresh }: DashboardOverviewProps) => {
  const { t } = useTranslation('dashboard');
  
  console.log('📊 DashboardOverview - testScores:', profile?.testScores);
  
  const hasCallCentersSelected = 
    (profile?.availability?.blacklist && profile.availability.blacklist.length > 0) ||
    (profile?.availability?.whitelist && profile.availability.whitelist.length > 0);

  const hasProximitySet = 
    profile?.availability?.nearby_cities && profile.availability.nearby_cities.length > 0;

  const hasAdditionalActivities = 
    (profile?.profile?.operation_1 && profile.profile.operation_1.trim() !== '' && 
     profile?.profile?.operation_1_experience && profile.profile.operation_1_experience.trim() !== '') ||
    (profile?.profile?.operation_2 && profile.profile.operation_2.trim() !== '' &&
     profile?.profile?.operation_2_experience && profile.profile.operation_2_experience.trim() !== '');

  const languageTests = [
    { id: 'french', label: t('overview.french'), completed: profile?.testScores?.french_test_completed || false, score: profile?.testScores?.french_overall_score },
    { id: 'english', label: t('overview.english'), completed: profile?.testScores?.english_test_completed || false, score: profile?.testScores?.english_overall_score },
    { id: 'italian', label: t('overview.italian'), completed: profile?.testScores?.italian_test_completed || profile?.testScores?.italiano_test_completed || false, score: profile?.testScores?.italian_overall_score ?? profile?.testScores?.italiano_overall_score },
    { id: 'spanish', label: t('overview.spanish'), completed: profile?.testScores?.spanish_test_completed || profile?.testScores?.espanol_test_completed || false, score: profile?.testScores?.spanish_overall_score ?? profile?.testScores?.espanol_overall_score },
    { id: 'german', label: t('overview.german'), completed: profile?.testScores?.german_test_completed || profile?.testScores?.deutsch_test_completed || false, score: profile?.testScores?.german_overall_score ?? profile?.testScores?.deutsch_overall_score },
  ];
  
  const completedTests = languageTests.filter(test => test.completed);
  const hasAnyTestCompleted = completedTests.length > 0;
  const allTestsCompleted = languageTests.every(test => test.completed);

  const getLanguageTestDescription = () => {
    if (completedTests.length > 0) {
      const testDescriptions = completedTests.map(test => `${test.label} (${test.score || 0}%)`).join(', ');
      const more = completedTests.length < 5 ? t('overview.moreTests') : t('overview.allTestsDone');
      return t('overview.testsCompleted', { tests: testDescriptions, more });
    }
    return t('overview.selfAssessmentDesc');
  };

  const profileSections = [
    { 
      id: 'language-test', 
      label: hasAnyTestCompleted 
        ? t('overview.selfAssessmentLangs', { count: completedTests.length }) 
        : t('overview.selfAssessment'), 
      completed: hasAnyTestCompleted,
      priority: 1,
      description: getLanguageTestDescription(),
      showLanguageBreakdown: hasAnyTestCompleted,
      canDoMore: hasAnyTestCompleted && !allTestsCompleted
    },
    { 
      id: 'callcenters', 
      label: t('overview.callCenters'), 
      completed: hasCallCentersSelected, 
      priority: 2,
      description: t('overview.callCentersDesc')
    },
    { 
      id: 'proximity', 
      label: t('overview.proximity'), 
      completed: hasProximitySet, 
      priority: 3,
      description: t('overview.proximityDesc')
    },
    { 
      id: 'activities', 
      label: t('overview.additionalActivities'), 
      completed: hasAdditionalActivities, 
      priority: 4,
      description: t('overview.additionalActivitiesDesc')
    }
  ];

  const handleSectionClick = (sectionId: string, completed: boolean, canDoMore?: boolean) => {
    if (!completed || canDoMore) {
      if (sectionId === 'language-test') {
        if (onSectionChange) {
          onSectionChange('language-test');
        }
      } else if (onSectionChange) {
        onSectionChange(sectionId);
      }
    }
  };

  const completedSections = profileSections.filter(section => section.completed).length;
  const completionRate = Math.round((completedSections / profileSections.length) * 100);

  const getStatusIcon = (completed: boolean) => {
    if (completed) {
      return <CheckCircle className="h-4 w-4 sm:h-5 sm:w-5 text-green-600 dark:text-green-500" />;
    }
    return <XCircle className="h-4 w-4 sm:h-5 sm:w-5 text-destructive" />;
  };

  const getStatusBadge = (completed: boolean) => {
    if (completed) {
      return (
        <Badge className="bg-green-600 text-white border-0 font-semibold text-xs px-3 py-1 shadow-sm">
          {t('overview.completed_badge')}
        </Badge>
      );
    }
    return (
      <Badge className="bg-primary text-primary-foreground border-0 font-semibold text-xs px-3 py-1 shadow-sm">
        {t('overview.recommended')}
      </Badge>
    );
  };

  return (
    <div className="space-y-5 sm:space-y-6">
      <div className="bg-card rounded-lg border border-border">
        <div className="px-4 sm:px-6 py-4 sm:py-5 border-b border-border bg-muted">
          <div className="flex items-start gap-3 sm:gap-4">
            <div className="w-10 h-10 sm:w-12 sm:h-12 rounded-lg bg-primary flex items-center justify-center shrink-0">
              <span className="text-xl sm:text-2xl">🎯</span>
            </div>
            <div className="flex-1 min-w-0">
              <div className="flex items-center gap-2 flex-wrap">
                <h2 className="text-base sm:text-lg md:text-xl font-bold text-foreground">
                  {t('overview.recommendedActions')}
                </h2>
                <Badge className="bg-green-600 text-white text-[10px] sm:text-xs font-bold px-2 py-0.5 border-0 whitespace-nowrap">
                  {t('overview.hiringBoost')}
                </Badge>
              </div>
              <p className="text-xs sm:text-sm text-muted-foreground mt-1.5 sm:mt-2 leading-relaxed">
                {t('overview.completeSteps').split('<1>').map((part, i) => 
                  i === 0 ? part : <><span key={i} className="text-primary font-bold">{part.split('</1>')[0]}</span>{part.split('</1>')[1]}</>
                )}
              </p>
              <div className="mt-2 sm:mt-3 flex items-center gap-2 text-xs bg-primary/5 px-2.5 sm:px-3 py-1.5 sm:py-2 rounded-lg border border-primary/20">
                <AlertCircle className="h-3.5 w-3.5 text-primary shrink-0" />
                <span className="text-muted-foreground"><strong className="text-foreground">{completedSections}/{profileSections.length}</strong> {t('overview.completed')} • <strong className="text-foreground">{4 - completedSections}</strong> {t('overview.remaining')}</span>
              </div>
            </div>
          </div>
        </div>
        <div className="p-3 sm:p-4 md:p-5">
          <div className="space-y-2 sm:space-y-2.5">
            {profileSections
              .sort((a, b) => a.priority - b.priority)
              .map((section) => (
                 <div 
                  key={`${section.id}-${section.priority}`}
                  onClick={() => handleSectionClick(section.id, section.completed, 'canDoMore' in section ? section.canDoMore : undefined)}
                  className={`group relative flex items-start sm:items-center gap-3 sm:gap-4 p-3 sm:p-4 rounded-lg border transition-all duration-200 overflow-hidden ${
                    !section.completed 
                      ? 'bg-card hover:bg-muted border-border hover:border-primary/30 cursor-pointer' 
                      : 'canDoMore' in section && section.canDoMore
                        ? 'bg-green-50 border-green-200 cursor-pointer hover:bg-green-100/50'
                        : 'bg-green-50 border-green-200'
                  }`}
                >
                  <div className="absolute top-2 right-2 w-5 h-5 sm:w-6 sm:h-6 rounded-full bg-primary text-primary-foreground text-[9px] sm:text-[10px] font-bold flex items-center justify-center">
                    {section.priority}
                  </div>
                  
                  <div className="flex items-start sm:items-center gap-2.5 sm:gap-3 flex-1 min-w-0 pr-6 sm:pr-8">
                    <div className="shrink-0 mt-0.5 sm:mt-0">
                      {getStatusIcon(section.completed)}
                    </div>
                    <div className="min-w-0 flex-1">
                      <div className="flex items-start sm:items-center gap-1.5 sm:gap-2 mb-0.5 sm:mb-1 flex-wrap">
                        <p className="font-bold text-foreground text-sm sm:text-base leading-tight">{section.label}</p>
                        {!section.completed && (
                          <Badge className="bg-destructive text-destructive-foreground text-[9px] sm:text-[10px] font-bold px-1.5 sm:px-2 py-0.5 border-0 whitespace-nowrap">
                            {t('overview.urgent')}
                          </Badge>
                        )}
                      </div>
                      {'description' in section && section.description ? (
                        <p className="text-[11px] sm:text-xs md:text-sm text-muted-foreground line-clamp-2 leading-relaxed">{section.description}</p>
                      ) : null}
                      
                      {section.id === 'language-test' && 'showLanguageBreakdown' in section && section.showLanguageBreakdown && (
                        <div className="mt-3 pt-3 border-t border-border/50">
                          <p className="text-xs font-semibold text-foreground mb-2">{t('overview.completedTests')}</p>
                          <div className="grid grid-cols-2 sm:grid-cols-3 gap-2">
                            {languageTests.map((test) => (
                              <div 
                                key={test.id}
                                className={`flex items-center gap-1.5 px-2 py-1.5 rounded-md text-xs ${
                                  test.completed 
                                    ? 'bg-green-500/10 text-green-700 dark:text-green-400 border border-green-500/30' 
                                    : 'bg-muted text-muted-foreground border border-border'
                                }`}
                              >
                                {test.completed ? (
                                  <Check className="h-3 w-3 text-green-600 dark:text-green-500" />
                                ) : (
                                  <div className="h-3 w-3 rounded-full border-2 border-muted-foreground/30" />
                                )}
                                <span className="font-medium">{test.label}</span>
                                {test.completed && test.score !== undefined && (
                                  <span className="ml-auto font-bold text-green-600 dark:text-green-400">{test.score}%</span>
                                )}
                              </div>
                            ))}
                          </div>
                        </div>
                      )}
                    </div>
                  </div>
                  
                  <div className="flex flex-col sm:flex-row items-end sm:items-center gap-2 shrink-0 self-start sm:self-center mt-1 sm:mt-0">
                    <div className="hidden sm:block">
                      {getStatusBadge(section.completed)}
                    </div>
                    {!section.completed && (
                      <div className="flex items-center gap-1.5 sm:gap-2 px-2.5 sm:px-3 py-1 sm:py-1.5 rounded-full bg-primary/10 border border-primary/30 transition-all">
                        <span className="text-[10px] sm:text-xs font-bold text-primary whitespace-nowrap">{t('overview.start')}</span>
                        <ChevronRight className="h-3 w-3 sm:h-3.5 sm:w-3.5 text-primary" />
                      </div>
                    )}
                  </div>
                </div>
              ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default DashboardOverview;
