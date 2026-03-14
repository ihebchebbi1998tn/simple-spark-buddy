import { useState, useEffect, useCallback, useRef, useMemo } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Checkbox } from '@/components/ui/checkbox';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Edit3, Save, Plus, Trash2, AlertCircle } from 'lucide-react';
import { toast } from '@/hooks/use-toast';
import { candidateService, type CandidateProfile } from '@/services/candidateService';
import { ACTIVITIES, OPERATIONS, EXPERIENCE_LEVELS, getOperationsForActivity } from '@/utils/constants';

interface ActivitiesSectionProps {
  profile: CandidateProfile | null;
  onRefresh: () => void;
}

const ActivitiesSection = ({ profile, onRefresh }: ActivitiesSectionProps) => {
  // Helper functions to convert between values and labels
  const getActivityLabel = (valueOrLabel: string): string => {
    if (!valueOrLabel) return '';
    // First try to find by value (numeric string like "2")
    const byValue = ACTIVITIES.find(a => a.value === valueOrLabel);
    if (byValue) return byValue.label;
    // Then try to find by label (string like "Téléprospection")
    const byLabel = ACTIVITIES.find(a => a.label === valueOrLabel);
    if (byLabel) return byLabel.label;
    // Return as-is for legacy values
    return valueOrLabel;
  };

  const getActivityValue = (valueOrLabel: string): string => {
    if (!valueOrLabel) return '';
    // First try to find by value (numeric string like "2")
    const byValue = ACTIVITIES.find(a => a.value === valueOrLabel);
    if (byValue) return byValue.value;
    // Then try to find by label (string like "Téléprospection")
    const byLabel = ACTIVITIES.find(a => a.label === valueOrLabel);
    if (byLabel) return byLabel.value;
    // Return as-is for legacy values
    return valueOrLabel;
  };

  const getOperationLabel = (valueOrLabel: string): string => {
    if (!valueOrLabel) return '';
    // First try to find by value (numeric string like "5")
    const byValue = OPERATIONS.find(o => o.value === valueOrLabel);
    if (byValue) return byValue.label;
    // Then try to find by label
    const byLabel = OPERATIONS.find(o => o.label === valueOrLabel);
    if (byLabel) return byLabel.label;
    return valueOrLabel;
  };

  const getOperationValue = (valueOrLabel: string): string => {
    if (!valueOrLabel) return '';
    const byValue = OPERATIONS.find(o => o.value === valueOrLabel);
    if (byValue) return byValue.value;
    const byLabel = OPERATIONS.find(o => o.label === valueOrLabel);
    if (byLabel) return byLabel.value;
    return valueOrLabel;
  };

  const getExperienceLabel = (valueOrLabel: string): string => {
    if (!valueOrLabel) return '';
    const byValue = EXPERIENCE_LEVELS.find(e => e.value === valueOrLabel);
    if (byValue) return byValue.label;
    const byLabel = EXPERIENCE_LEVELS.find(e => e.label === valueOrLabel);
    if (byLabel) return byLabel.label;
    return valueOrLabel;
  };

  const getExperienceValue = (valueOrLabel: string): string => {
    if (!valueOrLabel) return '';
    const byValue = EXPERIENCE_LEVELS.find(e => e.value === valueOrLabel);
    if (byValue) return byValue.value;
    const byLabel = EXPERIENCE_LEVELS.find(e => e.label === valueOrLabel);
    if (byLabel) return byLabel.value;
    return valueOrLabel;
  };

  // Dashboard-specific experience options aligned with constants
  const experienceOptions = [
    { value: '1', label: '0 - 6 mois' },
    { value: '2', label: '6 mois - 12 mois' },
    { value: '3', label: '1 an - 2 ans' },
    { value: '4', label: '2 ans - 3 ans' },
    { value: '5', label: '3 ans - 5 ans' },
    { value: '6', label: '5 ans - 7 ans' },
    { value: '7', label: 'Plus de 7 ans' }
  ];

  // Map experience value to numeric (in months) for cumulative calculation
  const experienceToMonths = (value: string): number => {
    const mapping: Record<string, number> = {
      "1": 6,    // 0-6 mois
      "2": 12,   // 6-12 mois
      "3": 24,   // 1-2 ans
      "4": 36,   // 2-3 ans
      "5": 60,   // 3-5 ans
      "6": 84,   // 5-7 ans
      "7": 120,  // Plus de 7 ans
    };
    return mapping[value] || 0;
  };

  // Get filtered experience options for an operation based on activity experience budget
  const getFilteredOperationExperienceOptions = (
    activityExperience: string,
    operationsExperience: string[],
    currentOperationIndex: number
  ): typeof experienceOptions => {
    if (!activityExperience) return [];
    
    const activityBudget = experienceToMonths(activityExperience);
    
    // Calculate used experience by OTHER operations (not current one)
    let usedByOthers = 0;
    operationsExperience.forEach((exp, index) => {
      if (index !== currentOperationIndex && exp) {
        usedByOthers += experienceToMonths(exp);
      }
    });
    
    const remainingBudget = activityBudget - usedByOthers;
    
    // Filter options that fit within remaining budget
    return experienceOptions.filter(exp => experienceToMonths(exp.value) <= remainingBudget);
  };

  // Check if any operation experience exceeds activity experience for an activity
  const checkOperationsExperienceExceeded = (activity: typeof activities[0]): boolean => {
    if (!activity.experience) return false;
    const activityBudget = experienceToMonths(activity.experience);
    let totalUsed = 0;
    activity.operationsExperience.forEach((opExp) => {
      if (opExp) {
        totalUsed += experienceToMonths(opExp);
      }
    });
    return totalUsed > activityBudget;
  };

  // Activities options - always return standard ACTIVITIES
  const activitiesOptions = ACTIVITIES.map(a => ({ value: a.value, label: a.label }));
  
  // Get filtered operations based on activity
  const getFilteredOperationsForActivity = (activityValue: string) => {
    return getOperationsForActivity(activityValue, OPERATIONS);
  };

  const getDefaultActivity = () => ({
    id: 1,
    activite: '',
    experience: '',
    operations: ['', ''],
    operationsExperience: ['', ''],
    trainingNeeds: [] as string[]
  });

  const [activities, setActivities] = useState([getDefaultActivity()]);
  const [editingFields, setEditingFields] = useState<Record<string, boolean>>({});
  const lastActiveField = useRef<string | null>(null);

  // Helper to check if a value is valid (exists in our options)
  const isValidActivityValue = (value: string): boolean => {
    if (!value) return true; // Empty is valid
    return ACTIVITIES.some(a => a.value === value);
  };

  const isValidOperationValue = (value: string): boolean => {
    if (!value) return true; // Empty is valid
    return OPERATIONS.some(o => o.value === value);
  };

  // Sync with profile data when not editing
  useEffect(() => {
    const hasEditingActivity = Object.values(editingFields).some(v => v);
    
    if (profile && !hasEditingActivity) {
      console.log('📊 Syncing activities data from profile');
      console.log('📊 Profile data:', profile.profile);
      
      // Try to load from localStorage first (as backend doesn't store activities array)
      const savedActivities = localStorage.getItem('dashboard_activities');
      if (savedActivities) {
        try {
          const loadedActivities = JSON.parse(savedActivities);
          if (loadedActivities && loadedActivities.length > 0) {
            // Check if localStorage has valid data (not legacy labels)
            const firstAct = loadedActivities[0];
            const hasValidData = isValidActivityValue(firstAct.activite) && 
                                 isValidOperationValue(firstAct.operations?.[0] || '');
            
            if (hasValidData && firstAct.activite) {
              console.log('✅ Activities loaded from localStorage (valid data):', loadedActivities);
              setActivities(loadedActivities);
              return;
            } else {
              // Legacy data detected - clear it and use profile
              console.log('⚠️ Legacy localStorage data detected, clearing and using profile data');
              localStorage.removeItem('dashboard_activities');
            }
          }
        } catch (error) {
          console.error('❌ Error loading activities from localStorage:', error);
          localStorage.removeItem('dashboard_activities');
        }
      }
      
      // Use profile data to pre-fill first activity
      // Profile stores values like "2", "5" which correspond to ACTIVITIES[1], OPERATIONS[4]
      const firstActivity = {
        id: 1,
        activite: profile.profile?.primary_activity || '',
        experience: profile.profile?.primary_activity_experience || profile.profile?.call_center_experience || '',
        operations: [
          profile.profile?.operation_1 || '', 
          profile.profile?.operation_2 || '', 
          ''
        ],
        operationsExperience: [
          profile.profile?.operation_1_experience || '', 
          profile.profile?.operation_2_experience || '', 
          ''
        ],
        trainingNeeds: [] as string[]
      };
      setActivities([firstActivity]);
      console.log('✅ Activities initialized from profile data:', firstActivity);
    }
  }, [profile, editingFields]);

  const autoSave = async () => {
    console.log("Auto-save données activités:", activities);
    
    try {
      // Save activities data to localStorage as backup
      localStorage.setItem('dashboard_activities', JSON.stringify(activities));
      console.log('✅ Activities data saved to localStorage');
      
      // Note: Backend API endpoint for additional activities not yet implemented
      // When backend endpoint is ready, uncomment and use:
      // await candidateService.updateActivities({ activities });
      
    } catch (error) {
      console.error('❌ Error saving activities:', error);
    }
  };

  // Auto-save when data changes (debounced)
  useEffect(() => {
    const timer = setTimeout(() => {
      autoSave();
    }, 1000);
    return () => clearTimeout(timer);
  }, [activities]);

  // Auto-save when user leaves the page
  useEffect(() => {
    const handleBeforeUnload = (e: BeforeUnloadEvent) => {
      autoSave();
    };
    window.addEventListener('beforeunload', handleBeforeUnload);
    return () => window.removeEventListener('beforeunload', handleBeforeUnload);
  }, [activities]);

  // Track field focus to save previous field when switching
  const handleFieldFocus = useCallback((fieldName: string) => {
    // If there was a previous active field and it's different from current
    if (lastActiveField.current && lastActiveField.current !== fieldName) {
      const prevField = lastActiveField.current;
      
      // Check if previous field has content based on field type
      let hasValue = false;
      
      if (prevField.startsWith('activite-')) {
        const activityId = parseInt(prevField.split('-')[1]);
        const activity = activities.find(a => a.id === activityId);
        hasValue = !!(activity && activity.activite);
      } else if (prevField.startsWith('experience-')) {
        const activityId = parseInt(prevField.split('-')[1]);
        const activity = activities.find(a => a.id === activityId);
        hasValue = !!(activity && activity.experience);
      } else if (prevField.startsWith('operation-')) {
        const parts = prevField.split('-');
        const activityId = parseInt(parts[1]);
        const operationIndex = parseInt(parts[2]);
        const activity = activities.find(a => a.id === activityId);
        hasValue = !!(activity && activity.operations[operationIndex]);
      } else if (prevField.startsWith('operation-exp-')) {
        const parts = prevField.split('-');
        const activityId = parseInt(parts[2]);
        const operationIndex = parseInt(parts[3]);
        const activity = activities.find(a => a.id === activityId);
        hasValue = !!(activity && activity.operationsExperience[operationIndex]);
      }
      
      // Save previous field if it has content
      if (hasValue) {
        const activityFieldName = prevField.startsWith('activite-') || prevField.startsWith('experience-') || prevField.startsWith('operation-') 
          ? `activity-${prevField.split('-')[1]}` 
          : prevField;
        setEditingFields(prev => ({
          ...prev,
          [activityFieldName]: false
        }));
      }
    }
    
    // Set current field as active
    lastActiveField.current = fieldName;
    
    // Enable editing for current field if not already
    const activityFieldName = fieldName.startsWith('activite-') || fieldName.startsWith('experience-') || fieldName.startsWith('operation-') 
      ? `activity-${fieldName.split('-')[1]}` 
      : fieldName;
    setEditingFields(prev => ({
      ...prev,
      [activityFieldName]: true
    }));
  }, [activities]);

  const toggleFieldEdit = (fieldName: string) => {
    // If we're turning off editing and activity is complete, auto-save
    if (editingFields[fieldName] && fieldName.startsWith('activity-')) {
      const activityId = parseInt(fieldName.split('-')[1]);
      const activity = activities.find(a => a.id === activityId);
      
      if (activity && activity.activite && activity.experience) {
        handleFieldSave(fieldName);
        return;
      }
    }
    
    setEditingFields(prev => ({
      ...prev,
      [fieldName]: !prev[fieldName]
    }));
  };

  const handleFieldSave = (fieldName: string) => {
    setEditingFields(prev => ({
      ...prev,
      [fieldName]: false
    }));
    // Toast removed as requested
  };

  const isFieldEmpty = (value: string | undefined) => !value || value.trim() === '';

  const getFieldClasses = (value: string | undefined, isEditing: boolean) => {
    const isEmpty = isFieldEmpty(value);
    return `${isEmpty && !isEditing ? 'border-red-300 bg-red-50' : ''} ${isEmpty && !isEditing ? 'placeholder:text-red-400' : ''}`;
  };

  const updateActivity = (activityId: number, field: string, value: string) => {
    setActivities(prev => prev.map(activity => {
      if (activity.id !== activityId) return activity;
      
      const updated = { ...activity, [field]: value };
      
      // If activity type changed, reset operations that are no longer valid
      if (field === 'activite' && value) {
        const validOperations = getFilteredOperationsForActivity(value);
        const validOperationValues = validOperations.map(op => op.value);
        
        // Reset any operations that aren't valid for the new activity
        updated.operations = updated.operations.map(op => 
          op && !validOperationValues.includes(op) ? '' : op
        );
        updated.operationsExperience = updated.operationsExperience.map((exp, index) => 
          updated.operations[index] ? exp : ''
        );
      }
      
      // If activity experience changed, reset operations experience that exceed budget
      if (field === 'experience' && value) {
        const newBudget = experienceToMonths(value);
        let usedBudget = 0;
        
        // Check each operation's experience and reset if it exceeds remaining budget
        const newOperationsExperience = updated.operationsExperience.map((opExp, index) => {
          if (!opExp || !updated.operations[index]) return opExp;
          
          const opExpMonths = experienceToMonths(opExp);
          if (usedBudget + opExpMonths > newBudget) {
            return ''; // Reset this operation's experience
          }
          usedBudget += opExpMonths;
          return opExp;
        });
        
        updated.operationsExperience = newOperationsExperience;
      }
      
      return updated;
    }));
  };

  const updateOperation = (activityId: number, operationIndex: number, field: 'operations' | 'operationsExperience', value: string) => {
    setActivities(prev => prev.map(activity => 
      activity.id === activityId 
        ? { 
            ...activity, 
            [field]: activity[field].map((item, index) => 
              index === operationIndex ? value : item
            )
          }
        : activity
    ));
  };

  return (
    <div className="space-y-6">
      <Card className="border border-border bg-card">
        <CardContent className="pt-6">
          <div className="flex items-center justify-between mb-6">
            <h2 className="text-lg font-semibold text-foreground">Mes Activités & Opérations</h2>
          </div>
          <div className="space-y-8">
          {activities.map((activity, activityIndex) => (
            <div key={activity.id} className={`space-y-6 p-6 border rounded-lg ${activityIndex === 0 ? 'bg-primary/5 border-primary/20' : (isFieldEmpty(activity.activite) || isFieldEmpty(activity.experience)) ? 'border-destructive/30 bg-destructive/5' : 'bg-muted'}`}>
              <div className="flex items-center justify-between">
                <h3 className={`text-base font-semibold ${activityIndex === 0 ? 'text-primary' : (isFieldEmpty(activity.activite) || isFieldEmpty(activity.experience)) ? 'text-destructive' : 'text-foreground'}`}>
                  Activité {activityIndex + 1} {activityIndex === 0 && <span className="text-primary">(Obligatoire - pré-rempli)</span>} {activityIndex > 0 && (isFieldEmpty(activity.activite) || isFieldEmpty(activity.experience)) && <span className="text-destructive">*</span>}
                </h3>
                <div className="flex space-x-2">
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={() => editingFields[`activity-${activity.id}`] ? handleFieldSave(`activity-${activity.id}`) : toggleFieldEdit(`activity-${activity.id}`)}
                  >
                    {editingFields[`activity-${activity.id}`] ? <Save className="h-4 w-4" /> : <Edit3 className="h-4 w-4" />}
                  </Button>
                  {activityIndex > 0 && (
                    <Button
                      variant="outline"
                      size="sm"
                      onClick={() => setActivities(prev => prev.filter(a => a.id !== activity.id))}
                    >
                      <Trash2 className="h-4 w-4" />
                    </Button>
                  )}
                </div>
              </div>

              {/* Activité */}
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor={`activite-${activity.id}`} className={isFieldEmpty(activity.activite) && !editingFields[`activity-${activity.id}`] ? 'text-red-600' : ''}>
                    Activité {isFieldEmpty(activity.activite) && <span className="text-red-500">*</span>}
                  </Label>
                  <Select
                    value={activity.activite}
                    onValueChange={(value) => updateActivity(activity.id, 'activite', value)}
                    disabled={!editingFields[`activity-${activity.id}`]}
                  >
                    <SelectTrigger className={getFieldClasses(activity.activite, editingFields[`activity-${activity.id}`])}>
                      <SelectValue placeholder="Sélectionnez une activité" />
                    </SelectTrigger>
                    <SelectContent>
                      {activitiesOptions.map((option) => (
                        <SelectItem key={option.value} value={option.value}>
                          {option.label}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-2">
                  <Label htmlFor={`experience-${activity.id}`} className={isFieldEmpty(activity.experience) && !editingFields[`activity-${activity.id}`] ? 'text-red-600' : ''}>
                    Expérience activité {isFieldEmpty(activity.experience) && <span className="text-red-500">*</span>}
                  </Label>
                  <Select
                    value={activity.experience}
                    onValueChange={(value) => updateActivity(activity.id, 'experience', value)}
                    disabled={!editingFields[`activity-${activity.id}`]}
                  >
                    <SelectTrigger className={getFieldClasses(activity.experience, editingFields[`activity-${activity.id}`])}>
                      <SelectValue placeholder="Sélectionnez votre expérience" />
                    </SelectTrigger>
                    <SelectContent>
                      {experienceOptions.map((option) => (
                        <SelectItem key={option.value} value={option.value}>
                          {option.label}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
              </div>

              {/* Opérations */}
              <div className="space-y-4">
                <h4 className="font-medium text-foreground">Opérations recherchées</h4>
                <div className="grid grid-cols-1 gap-4">
                  {activity.operations.map((operation, operationIndex) => (
                    <div key={operationIndex} className="grid grid-cols-1 md:grid-cols-2 gap-4 p-4 border border-border rounded-md bg-card">
                      <div className="space-y-2">
                        <div className="flex items-center justify-between">
                          <Label htmlFor={`operation-${activity.id}-${operationIndex}`}>
                            Opération {operationIndex + 1}
                          </Label>
                          {operationIndex >= 2 && editingFields[`activity-${activity.id}`] && (
                            <Button
                              variant="ghost"
                              size="sm"
                              className="h-6 w-6 p-0 text-red-500 hover:text-red-700"
                              onClick={() => {
                                setActivities(prev => prev.map(a => {
                                  if (a.id !== activity.id) return a;
                                  const newOps = [...a.operations];
                                  const newOpsExp = [...a.operationsExperience];
                                  newOps.splice(operationIndex, 1);
                                  newOpsExp.splice(operationIndex, 1);
                                  return { ...a, operations: newOps, operationsExperience: newOpsExp };
                                }));
                              }}
                            >
                              <Trash2 className="h-3 w-3" />
                            </Button>
                          )}
                        </div>
                        <Select
                          value={operation}
                          onValueChange={(value) => updateOperation(activity.id, operationIndex, 'operations', value)}
                          disabled={!editingFields[`activity-${activity.id}`]}
                        >
                          <SelectTrigger className={getFieldClasses(operation, editingFields[`activity-${activity.id}`])}>
                            <SelectValue placeholder="Sélectionnez une opération" />
                          </SelectTrigger>
                          <SelectContent>
                            {getFilteredOperationsForActivity(activity.activite)
                              .filter(op => !activity.operations.includes(op.value) || op.value === operation)
                              .map((option) => (
                              <SelectItem key={option.value} value={option.value}>
                                {option.label}
                              </SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                      </div>
                      <div className="space-y-2">
                        <Label htmlFor={`operation-exp-${activity.id}-${operationIndex}`}>
                          Expérience opération {operationIndex + 1}
                        </Label>
                        <Select
                          value={activity.operationsExperience[operationIndex]}
                          onValueChange={(value) => updateOperation(activity.id, operationIndex, 'operationsExperience', value)}
                          disabled={!editingFields[`activity-${activity.id}`]}
                        >
                          <SelectTrigger className={getFieldClasses(activity.operationsExperience[operationIndex], editingFields[`activity-${activity.id}`])}>
                            <SelectValue placeholder="Sélectionnez votre expérience" />
                          </SelectTrigger>
                          <SelectContent>
                            {getFilteredOperationExperienceOptions(
                              activity.experience,
                              activity.operationsExperience,
                              operationIndex
                            ).map((option) => (
                              <SelectItem key={option.value} value={option.value}>
                                {option.label}
                              </SelectItem>
                            ))}
                          </SelectContent>
                        </Select>
                      </div>
                    </div>
                  ))}
                </div>

                {/* Add operation button (max 3 per activity) */}
                {activity.operations.length < 3 && editingFields[`activity-${activity.id}`] && (
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() => {
                      setActivities(prev => prev.map(a => {
                        if (a.id !== activity.id) return a;
                        return {
                          ...a,
                          operations: [...a.operations, ''],
                          operationsExperience: [...a.operationsExperience, '']
                        };
                      }));
                    }}
                    className="flex items-center gap-2"
                  >
                    <Plus className="h-4 w-4" />
                    <span>Ajouter une opération</span>
                  </Button>
                )}

                {/* Error message when operations experience exceeds activity experience */}
                {checkOperationsExperienceExceeded(activity) && (
                  <div className="p-4 rounded-lg bg-red-50 border border-red-200">
                    <div className="flex items-start gap-3">
                      <AlertCircle className="w-5 h-5 text-red-500 flex-shrink-0 mt-0.5" />
                      <div className="space-y-1">
                        <p className="text-sm font-medium text-red-700">
                          Expérience incohérente : Veuillez vérifier vos informations.
                        </p>
                      </div>
                    </div>
                  </div>
                )}
              </div>

              {/* Training Needs Question */}
              {(activity.activite || activity.operations.some(op => op)) && (
                <div className="space-y-3 mt-4 p-4 bg-muted border border-border rounded-lg">
                  <Label className="text-sm font-semibold text-foreground">
                    Auriez-vous besoin de formation pour cette activité ?
                  </Label>
                  <div className="flex flex-col md:flex-row md:gap-4">
                    {['Formation métier', 'Formation linguistique', 'Formation soft skills', 'Non, pas besoin'].map((training) => (
                      <div key={training} className="flex items-center space-x-2">
                        <Checkbox
                          id={`training-${activity.id}-${training}`}
                          checked={activity.trainingNeeds.includes(training)}
                          disabled={!editingFields[`activity-${activity.id}`]}
                          onCheckedChange={(checked) => {
                            const newTrainingNeeds = checked
                              ? training === 'Non, pas besoin'
                                ? [training] // If "Non, pas besoin" is selected, clear all others
                                : activity.trainingNeeds.filter(t => t !== 'Non, pas besoin').concat(training)
                              : activity.trainingNeeds.filter(t => t !== training);
                            
                            setActivities(prev => prev.map(a => 
                              a.id === activity.id 
                                ? { ...a, trainingNeeds: newTrainingNeeds }
                                : a
                            ));
                          }}
                        />
                        <Label htmlFor={`training-${activity.id}-${training}`} className="text-sm cursor-pointer">
                          {training}
                        </Label>
                      </div>
                    ))}
                  </div>
                </div>
              )}
            </div>
          ))}

          {activities.length < 2 && (
            <>
              {activities.length === 1 && (
                <div className="bg-muted border border-border rounded-lg p-4 text-center">
                  <p className="text-foreground font-medium mb-2">
                    Conseil : Ajoutez une 2ème activité pour augmenter vos chances !
                  </p>
                  <p className="text-muted-foreground text-sm">
                    Plus vous renseignez d'activités et d'opérations, plus vous augmentez vos opportunités de recevoir des offres adaptées à votre profil.
                  </p>
                </div>
              )}

              <div className="flex justify-center">
                <Button
                  variant="outline"
                  onClick={() => {
                    const newId = Math.max(...activities.map(a => a.id)) + 1;
                    setActivities(prev => [...prev, {
                      id: newId,
                      activite: '',
                      experience: '',
                      operations: ['', ''],
                      operationsExperience: ['', ''],
                      trainingNeeds: []
                    }]);
                    setEditingFields(prev => ({
                      ...prev,
                      [`activity-${newId}`]: true
                    }));
                  }}
                  className="flex items-center space-x-2"
                >
                  <Plus className="h-4 w-4" />
                  <span>Ajouter une activité</span>
                </Button>
              </div>
            </>
          )}
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default ActivitiesSection;
