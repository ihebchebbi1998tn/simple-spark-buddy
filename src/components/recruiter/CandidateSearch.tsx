import { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Checkbox } from '@/components/ui/checkbox';
import { Badge } from '@/components/ui/badge';
import { 
  Search, 
  Filter, 
  Users, 
  MapPin, 
  Briefcase,
  Languages,
  Clock,
  ChevronDown,
  ChevronUp,
  RotateCcw
} from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';

import { searchOptions } from '@/data/recruiterMockData';

const { cities, positions, activities, operations, languages, experienceLevels } = searchOptions;

import type { SearchResults } from '@/types/recruiter';

interface CandidateSearchProps {
  onNavigateToOffers?: () => void;
}

export function CandidateSearch({ onNavigateToOffers }: CandidateSearchProps) {
  const [showAdvanced, setShowAdvanced] = useState(false);
  const [isSearching, setIsSearching] = useState(false);
  const [results, setResults] = useState<SearchResults | null>(null);

  // Form state
  const [formData, setFormData] = useState({
    city: '',
    position: '',
    globalExperience: '',
    positionExperience: '',
    activity: '',
    activityExperience: '',
    operation: '',
    operationExperience: '',
    primaryLanguage: '',
    secondaryLanguage: '',
    workMode: 'all', // 'remote', 'onsite', 'all'
    workTime: 'all', // 'full', 'part', 'all'
  });

  const handleSearch = () => {
    setIsSearching(true);
    // Simulate API call
    setTimeout(() => {
      setResults({
        highMatch: Math.floor(Math.random() * 200) + 100,
        acceptableMatch: Math.floor(Math.random() * 300) + 200,
        mediumMatch: Math.floor(Math.random() * 400) + 300,
        lowMatch: Math.floor(Math.random() * 500) + 400,
        outOfTarget: Math.floor(Math.random() * 1500) + 1000,
      });
      setIsSearching(false);
    }, 1500);
  };

  const handleReset = () => {
    setFormData({
      city: '',
      position: '',
      globalExperience: '',
      positionExperience: '',
      activity: '',
      activityExperience: '',
      operation: '',
      operationExperience: '',
      primaryLanguage: '',
      secondaryLanguage: '',
      workMode: 'all',
      workTime: 'all',
    });
    setResults(null);
  };

  const ResultCard = ({ title, score, count, scoreClass }: { title: string; score: string; count: number; scoreClass: string }) => (
    <Card className="border shadow-sm hover:shadow-md transition-all hover:-translate-y-0.5">
      <CardContent className="p-4 sm:p-5 text-center">
        <h3 className="text-xs sm:text-sm font-medium text-muted-foreground mb-2">{title}</h3>
        <p className={`text-xl sm:text-2xl font-bold mb-1 ${scoreClass}`}>{score}</p>
        <p className="text-lg sm:text-xl font-bold text-primary">{count.toLocaleString()} leads</p>
        <p className="text-[10px] sm:text-xs text-muted-foreground mt-1.5 line-clamp-2">
          {title === 'Correspondance élevée' && 'Correspondent parfaitement'}
          {title === 'Correspondance acceptable' && 'Correspondent bien'}
          {title === 'Correspondance moyenne' && 'Correspondent moyennement'}
          {title === 'Correspondance faible' && 'Correspondent partiellement'}
          {title === 'Hors cible' && 'Ne correspondent pas'}
        </p>
      </CardContent>
    </Card>
  );

  return (
    <div className="space-y-5 sm:space-y-6">
      {/* Page Header */}
      <div>
        <h1 className="text-xl sm:text-2xl font-bold text-foreground mb-1">
          Rechercher des candidats
        </h1>
        <p className="text-sm text-muted-foreground">
          Définissez vos critères pour trouver les profils qui correspondent à vos besoins
        </p>
      </div>

      {/* Search Form OR Results */}
      <AnimatePresence mode="wait">
        {!results ? (
          <motion.div
            key="search-form"
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            exit={{ opacity: 0, y: -10 }}
            transition={{ duration: 0.25 }}
          >
            <Card className="border shadow-sm">
              <CardHeader className="border-b bg-muted/30 px-4 sm:px-6 py-3 sm:py-4">
                <div className="flex items-center gap-3">
                  <div className="w-9 h-9 sm:w-10 sm:h-10 bg-primary rounded-lg flex items-center justify-center shrink-0">
                    <Filter className="w-4 h-4 sm:w-5 sm:h-5 text-primary-foreground" />
                  </div>
                  <div className="min-w-0">
                    <CardTitle className="text-base sm:text-lg">Critères de recherche</CardTitle>
                    <p className="text-xs sm:text-sm text-muted-foreground">Précisez le profil idéal de candidat</p>
                  </div>
                </div>
              </CardHeader>
              <CardContent className="p-4 sm:p-6">
                <div className="space-y-5">
                  {/* Basic Criteria */}
                  <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-3 sm:gap-4">
                    {/* City */}
                    <div className="space-y-1.5">
                      <Label className="flex items-center gap-2 text-sm">
                        <MapPin className="w-4 h-4 text-muted-foreground" />
                        Ville
                      </Label>
                      <Select value={formData.city} onValueChange={(v) => setFormData({...formData, city: v})}>
                        <SelectTrigger>
                          <SelectValue placeholder="Sélectionnez une ville" />
                        </SelectTrigger>
                        <SelectContent>
                          {cities.map((city) => (
                            <SelectItem key={city} value={city}>{city}</SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </div>

                    {/* Position */}
                    <div className="space-y-1.5">
                      <Label className="flex items-center gap-2 text-sm">
                        <Briefcase className="w-4 h-4 text-muted-foreground" />
                        Poste recherché
                      </Label>
                      <Select value={formData.position} onValueChange={(v) => setFormData({...formData, position: v})}>
                        <SelectTrigger>
                          <SelectValue placeholder="Sélectionnez un poste" />
                        </SelectTrigger>
                        <SelectContent>
                          {positions.map((pos) => (
                            <SelectItem key={pos} value={pos}>{pos}</SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </div>

                    {/* Global Experience */}
                    <div className="space-y-1.5">
                      <Label className="flex items-center gap-2 text-sm">
                        <Clock className="w-4 h-4 text-muted-foreground" />
                        Expérience globale
                      </Label>
                      <Select value={formData.globalExperience} onValueChange={(v) => setFormData({...formData, globalExperience: v})}>
                        <SelectTrigger>
                          <SelectValue placeholder="Sélectionnez" />
                        </SelectTrigger>
                        <SelectContent>
                          {experienceLevels.map((exp) => (
                            <SelectItem key={exp} value={exp}>{exp}</SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </div>

                    {/* Activity */}
                    <div className="space-y-1.5">
                      <Label className="text-sm">Activité</Label>
                      <Select value={formData.activity} onValueChange={(v) => setFormData({...formData, activity: v})}>
                        <SelectTrigger>
                          <SelectValue placeholder="Sélectionnez une activité" />
                        </SelectTrigger>
                        <SelectContent>
                          {activities.map((act) => (
                            <SelectItem key={act} value={act}>{act}</SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </div>

                    {/* Primary Language */}
                    <div className="space-y-1.5">
                      <Label className="flex items-center gap-2 text-sm">
                        <Languages className="w-4 h-4 text-muted-foreground" />
                        Langue principale
                      </Label>
                      <Select value={formData.primaryLanguage} onValueChange={(v) => setFormData({...formData, primaryLanguage: v})}>
                        <SelectTrigger>
                          <SelectValue placeholder="Sélectionnez une langue" />
                        </SelectTrigger>
                        <SelectContent>
                          {languages.map((lang) => (
                            <SelectItem key={lang} value={lang}>{lang}</SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </div>

                    {/* Operation */}
                    <div className="space-y-1.5">
                      <Label className="text-sm">Opération / Secteur</Label>
                      <Select value={formData.operation} onValueChange={(v) => setFormData({...formData, operation: v})}>
                        <SelectTrigger>
                          <SelectValue placeholder="Sélectionnez" />
                        </SelectTrigger>
                        <SelectContent>
                          {operations.map((op) => (
                            <SelectItem key={op} value={op}>{op}</SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </div>
                  </div>

                  {/* Advanced Filters Toggle */}
                  <Button
                    variant="ghost"
                    onClick={() => setShowAdvanced(!showAdvanced)}
                    className="text-primary hover:text-primary/80 h-9 px-3"
                  >
                    {showAdvanced ? <ChevronUp className="w-4 h-4 mr-2" /> : <ChevronDown className="w-4 h-4 mr-2" />}
                    {showAdvanced ? 'Masquer les filtres avancés' : 'Afficher plus de filtres'}
                  </Button>

                  {/* Advanced Filters */}
                  <AnimatePresence>
                    {showAdvanced && (
                      <motion.div
                        initial={{ opacity: 0, height: 0 }}
                        animate={{ opacity: 1, height: 'auto' }}
                        exit={{ opacity: 0, height: 0 }}
                        className="space-y-5 overflow-hidden"
                      >
                        <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-3 sm:gap-4 pt-4 border-t">
                          <div className="space-y-1.5">
                            <Label className="text-sm">Expérience au poste</Label>
                            <Select value={formData.positionExperience} onValueChange={(v) => setFormData({...formData, positionExperience: v})}>
                              <SelectTrigger>
                                <SelectValue placeholder="Sélectionnez" />
                              </SelectTrigger>
                              <SelectContent>
                                {experienceLevels.map((exp) => (
                                  <SelectItem key={exp} value={exp}>{exp}</SelectItem>
                                ))}
                              </SelectContent>
                            </Select>
                          </div>

                          <div className="space-y-1.5">
                            <Label className="text-sm">Expérience activité</Label>
                            <Select value={formData.activityExperience} onValueChange={(v) => setFormData({...formData, activityExperience: v})}>
                              <SelectTrigger>
                                <SelectValue placeholder="Sélectionnez" />
                              </SelectTrigger>
                              <SelectContent>
                                {experienceLevels.map((exp) => (
                                  <SelectItem key={exp} value={exp}>{exp}</SelectItem>
                                ))}
                              </SelectContent>
                            </Select>
                          </div>

                          <div className="space-y-1.5">
                            <Label className="text-sm">Langue secondaire (bilingue)</Label>
                            <Select value={formData.secondaryLanguage} onValueChange={(v) => setFormData({...formData, secondaryLanguage: v})}>
                              <SelectTrigger>
                                <SelectValue placeholder="Sélectionnez" />
                              </SelectTrigger>
                              <SelectContent>
                                {languages.map((lang) => (
                                  <SelectItem key={lang} value={lang}>{lang}</SelectItem>
                                ))}
                              </SelectContent>
                            </Select>
                          </div>
                        </div>

                        <div className="grid sm:grid-cols-2 gap-4 sm:gap-6 pt-4 border-t">
                          <div className="space-y-2.5">
                            <Label className="font-medium text-sm">Mode de travail</Label>
                            <div className="flex flex-wrap gap-3 sm:gap-4">
                              {[
                                { value: 'all', label: 'Tous' },
                                { value: 'onsite', label: 'Présentiel' },
                                { value: 'remote', label: 'Télétravail' },
                              ].map((mode) => (
                                <label key={mode.value} className="flex items-center gap-2 cursor-pointer">
                                  <input
                                    type="radio"
                                    name="workMode"
                                    value={mode.value}
                                    checked={formData.workMode === mode.value}
                                    onChange={(e) => setFormData({...formData, workMode: e.target.value})}
                                    className="w-4 h-4 text-primary accent-primary"
                                  />
                                  <span className="text-sm">{mode.label}</span>
                                </label>
                              ))}
                            </div>
                          </div>

                          <div className="space-y-2.5">
                            <Label className="font-medium text-sm">Temps de travail</Label>
                            <div className="flex flex-wrap gap-3 sm:gap-4">
                              {[
                                { value: 'all', label: 'Tous' },
                                { value: 'full', label: 'Plein temps' },
                                { value: 'part', label: 'Temps partiel' },
                              ].map((time) => (
                                <label key={time.value} className="flex items-center gap-2 cursor-pointer">
                                  <input
                                    type="radio"
                                    name="workTime"
                                    value={time.value}
                                    checked={formData.workTime === time.value}
                                    onChange={(e) => setFormData({...formData, workTime: e.target.value})}
                                    className="w-4 h-4 text-primary accent-primary"
                                  />
                                  <span className="text-sm">{time.label}</span>
                                </label>
                              ))}
                            </div>
                          </div>
                        </div>
                      </motion.div>
                    )}
                  </AnimatePresence>

                  {/* Actions */}
                  <div className="flex flex-col sm:flex-row gap-2.5 pt-4 border-t">
                    <Button 
                      variant="outline" 
                      onClick={handleReset}
                      className="order-2 sm:order-1 h-10"
                    >
                      <RotateCcw className="w-4 h-4 mr-2" />
                      Réinitialiser
                    </Button>
                    <Button 
                      onClick={handleSearch}
                      disabled={isSearching}
                      className="order-1 sm:order-2 sm:ml-auto h-10"
                    >
                      {isSearching ? (
                        <>
                          <div className="w-4 h-4 border-2 border-primary-foreground border-t-transparent rounded-full animate-spin mr-2" />
                          Recherche en cours...
                        </>
                      ) : (
                        <>
                          <Search className="w-4 h-4 mr-2" />
                          Lancer la simulation
                        </>
                      )}
                    </Button>
                  </div>
                </div>
              </CardContent>
            </Card>
          </motion.div>
        ) : (
          <motion.div
            key="results"
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            exit={{ opacity: 0, y: -10 }}
            transition={{ duration: 0.25 }}
            className="space-y-5"
          >
            {/* Results Header with criteria summary */}
            <Card className="border shadow-sm">
              <CardHeader className="border-b bg-muted/30 px-4 sm:px-6 py-3 sm:py-4">
                <div className="flex items-center justify-between">
                  <div className="flex items-center gap-3">
                    <div className="w-9 h-9 sm:w-10 sm:h-10 bg-primary rounded-lg flex items-center justify-center shrink-0">
                      <Users className="w-4 h-4 sm:w-5 sm:h-5 text-primary-foreground" />
                    </div>
                    <div className="min-w-0">
                      <CardTitle className="text-base sm:text-lg">Résultats de la simulation</CardTitle>
                      <p className="text-xs sm:text-sm text-muted-foreground">Potentiel de notre base selon vos critères</p>
                    </div>
                  </div>
                  <div className="flex gap-2">
                    <Button variant="outline" size="sm" onClick={() => setResults(null)}>
                      <Filter className="w-4 h-4 mr-2" />
                      Modifier les critères
                    </Button>
                    <Button variant="outline" size="sm" onClick={handleReset}>
                      <RotateCcw className="w-4 h-4 mr-2" />
                      Nouvelle recherche
                    </Button>
                  </div>
                </div>
              </CardHeader>
              <CardContent className="p-4 sm:p-6">
                {/* Active criteria badges */}
                <div className="flex flex-wrap gap-2 mb-5">
                  {formData.city && <Badge variant="secondary">{formData.city}</Badge>}
                  {formData.position && <Badge variant="secondary">{formData.position}</Badge>}
                  {formData.globalExperience && <Badge variant="secondary">{formData.globalExperience}</Badge>}
                  {formData.activity && <Badge variant="secondary">{formData.activity}</Badge>}
                  {formData.primaryLanguage && <Badge variant="secondary">{formData.primaryLanguage}</Badge>}
                  {formData.operation && <Badge variant="secondary">{formData.operation}</Badge>}
                  {formData.workMode !== 'all' && <Badge variant="secondary">{formData.workMode === 'remote' ? 'Télétravail' : 'Présentiel'}</Badge>}
                  {formData.workTime !== 'all' && <Badge variant="secondary">{formData.workTime === 'full' ? 'Plein temps' : 'Temps partiel'}</Badge>}
                </div>

                {/* Result Cards */}
                <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-5 gap-3 sm:gap-4">
                  <ResultCard 
                    title="Correspondance élevée" 
                    score="> 80%" 
                    count={results.highMatch} 
                    scoreClass="text-emerald-600"
                  />
                  <ResultCard 
                    title="Correspondance acceptable" 
                    score="70-80%" 
                    count={results.acceptableMatch} 
                    scoreClass="text-emerald-500"
                  />
                  <ResultCard 
                    title="Correspondance moyenne" 
                    score="50-70%" 
                    count={results.mediumMatch} 
                    scoreClass="text-amber-600"
                  />
                  <ResultCard 
                    title="Correspondance faible" 
                    score="40-50%" 
                    count={results.lowMatch} 
                    scoreClass="text-amber-500"
                  />
                  <ResultCard 
                    title="Hors cible" 
                    score="< 40%" 
                    count={results.outOfTarget} 
                    scoreClass="text-destructive"
                  />
                </div>
              </CardContent>
            </Card>

            {/* CTA */}
            <Card className="border-0 bg-primary text-primary-foreground shadow-md">
              <CardContent className="p-5 sm:p-6 lg:p-8 text-center">
                <h3 className="text-lg sm:text-xl lg:text-2xl font-bold mb-2">Prêt à accéder à ces profils ?</h3>
                <p className="text-primary-foreground/80 text-sm sm:text-base mb-4 sm:mb-5 max-w-2xl mx-auto">
                  Notre base de données contient des milliers de candidats qualifiés prêts à rejoindre votre centre d'appels.
                </p>
                <Button 
                  variant="secondary" 
                  className="bg-background text-foreground hover:bg-background/90"
                  onClick={onNavigateToOffers}
                >
                  Découvrir nos offres
                </Button>
              </CardContent>
            </Card>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
}
