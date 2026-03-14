import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Search, MessageCircle, BookOpen, Settings, User, Phone, ChevronRight } from 'lucide-react';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Separator } from '@/components/ui/separator';

interface HelpModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

const commonQuestions = [
  {
    category: "Vue d'ensemble",
    icon: BookOpen,
    questions: [
      {
        q: "Comment compléter mon profil à 100% ?",
        a: "Pour compléter votre profil, assurez-vous de remplir toutes les sections : coordonnées, expérience, activités, langues, centres d'appels et préférences contractuelles. Les sections incomplètes sont marquées en rouge dans la vue d'ensemble."
      },
      {
        q: "Que signifient les codes couleurs ?",
        a: "Rouge = section non complétée, Vert = section complétée, Orange = section partiellement complétée. Plus votre profil est vert, plus vous êtes visible par les recruteurs."
      }
    ]
  },
  {
    category: "Mon Profil",
    icon: User,
    questions: [
      {
        q: "Comment évaluer mon niveau d'expérience ?",
        a: "Débutant (0-2 ans), Confirmé (2-5 ans), Senior (5+ ans). Cette auto-évaluation nous aide à vous proposer des postes adaptés à votre niveau."
      },
      {
        q: "Puis-je modifier mes coordonnées ?",
        a: "Oui, vous pouvez modifier vos coordonnées à tout moment dans la section 'Mon Profil'. Les changements sont sauvegardés automatiquement."
      }
    ]
  },
  {
    category: "Activités & Opérations",
    icon: Settings,
    questions: [
      {
        q: "Combien d'activités puis-je sélectionner ?",
        a: "Vous pouvez sélectionner jusqu'à 2 activités principales et jusqu'à 3 opérations par activité pour maximiser vos opportunités."
      },
      {
        q: "Comment définir mon expérience par opération ?",
        a: "Pour chaque opération, indiquez votre niveau : Débutant, Confirmé ou Senior. Cela permet un matching plus précis avec les offres."
      }
    ]
  },
  {
    category: "Centres d'Appels",
    icon: Phone,
    questions: [
      {
        q: "Quelle est la différence entre white liste et black liste ?",
        a: "White liste = centres avec lesquels vous souhaitez travailler. Black liste = centres à éviter. Un centre ne peut pas être dans les deux listes simultanément."
      },
      {
        q: "Les centres voient-ils mes préférences ?",
        a: "Les centres de votre black liste ne vous verront jamais. Ceux de votre white liste seront informés de votre intérêt, ce qui peut être un avantage."
      }
    ]
  }
];

export const HelpModal = ({ open, onOpenChange }: HelpModalProps) => {
  const navigate = useNavigate();
  const [searchQuery, setSearchQuery] = useState('');
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const [selectedQuestion, setSelectedQuestion] = useState<{ q: string; a: string } | null>(null);

  const filteredQuestions = commonQuestions.map(category => ({
    ...category,
    questions: category.questions.filter(
      q => searchQuery === '' || 
      q.q.toLowerCase().includes(searchQuery.toLowerCase()) ||
      q.a.toLowerCase().includes(searchQuery.toLowerCase())
    )
  })).filter(category => category.questions.length > 0);

  const handleQuestionClick = (question: { q: string; a: string }) => {
    setSelectedQuestion(question);
  };

  const handleBack = () => {
    setSelectedQuestion(null);
    setSelectedCategory(null);
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-2xl max-h-[85vh] sm:max-h-[80vh] overflow-hidden flex flex-col mx-2 sm:mx-0 w-[calc(100vw-1rem)] sm:w-auto">
        <DialogHeader className="pb-4 border-b px-4 sm:px-6">
          <DialogTitle className="flex items-center gap-2 sm:gap-3 text-lg sm:text-xl">
            <MessageCircle className="h-5 w-5 sm:h-6 sm:w-6 text-blue-600 shrink-0" />
            <span className="truncate">Centre d'Aide</span>
            <span className="hidden sm:inline">- CallCenterMatch</span>
          </DialogTitle>
          <p className="text-xs sm:text-sm text-gray-600 mt-1">
            Trouvez rapidement des réponses à vos questions
          </p>
        </DialogHeader>

        <div className="flex-1 overflow-hidden flex flex-col">
          {/* Search Bar */}
          <div className="p-3 sm:p-4 border-b bg-gray-50/50">
            <div className="relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
              <Input
                placeholder="Rechercher..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="pl-10 bg-white text-sm sm:text-base h-10 sm:h-11"
              />
            </div>
          </div>

          {/* Content Area */}
          <div className="flex-1 overflow-y-auto p-3 sm:p-4">
            {selectedQuestion ? (
              // Question Detail View
              <div className="space-y-4">
                <Button
                  variant="ghost"
                  size="sm"
                  onClick={handleBack}
                  className="text-blue-600 hover:text-blue-700 hover:bg-blue-50 p-0"
                >
                  ← Retour aux questions
                </Button>
                
                <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
                  <h3 className="font-semibold text-blue-900 mb-2">
                    {selectedQuestion.q}
                  </h3>
                  <p className="text-blue-800 leading-relaxed">
                    {selectedQuestion.a}
                  </p>
                </div>

                <div className="bg-gray-50 border rounded-lg p-4">
                  <p className="text-sm text-gray-600">
                    <strong>Cette réponse vous a-t-elle aidé ?</strong> Si vous avez d'autres questions, 
                    vous pouvez contacter notre équipe support directement.
                  </p>
                </div>
              </div>
            ) : (
              // Categories and Questions List
              <div className="space-y-6">
                {filteredQuestions.map((category) => (
                  <div key={category.category} className="space-y-3">
                    <div className="flex items-center gap-2">
                      <category.icon className="h-5 w-5 text-gray-600" />
                      <h3 className="font-semibold text-gray-900">{category.category}</h3>
                      <Badge variant="secondary" className="text-xs">
                        {category.questions.length} question{category.questions.length > 1 ? 's' : ''}
                      </Badge>
                    </div>
                    
                    <div className="space-y-2">
                      {category.questions.map((question, idx) => (
                        <button
                          key={idx}
                          onClick={() => handleQuestionClick(question)}
                          className="w-full text-left p-3 sm:p-4 bg-white border border-gray-200 rounded-lg hover:border-blue-300 hover:bg-blue-50/50 transition-all duration-200 group active:scale-[0.98] touch-manipulation"
                        >
                          <div className="flex items-start sm:items-center justify-between gap-2">
                            <span className="text-sm sm:text-base font-medium text-gray-700 group-hover:text-blue-700 leading-relaxed">
                              {question.q}
                            </span>
                            <ChevronRight className="h-4 w-4 sm:h-5 sm:w-5 text-gray-400 group-hover:text-blue-500 shrink-0 mt-0.5 sm:mt-0" />
                          </div>
                        </button>
                      ))}
                    </div>
                  </div>
                ))}

                {filteredQuestions.length === 0 && searchQuery && (
                  <div className="text-center py-8 text-gray-500">
                    <Search className="h-12 w-12 mx-auto mb-3 text-gray-300" />
                    <p>Aucune question trouvée pour "{searchQuery}"</p>
                    <p className="text-sm mt-1">Essayez avec d'autres mots-clés</p>
                  </div>
                )}
              </div>
            )}
          </div>

          {/* Footer */}
          <div className="border-t p-3 sm:p-4 bg-gray-50/50">
            <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-2 sm:gap-0 text-xs sm:text-sm text-gray-600">
              <span className="text-center sm:text-left w-full sm:w-auto">Besoin d'aide supplémentaire ?</span>
              <Button 
                variant="outline" 
                size="sm"
                onClick={() => {
                  onOpenChange(false);
                  navigate('/dashboard/contact');
                }}
                className="w-full sm:w-auto text-xs sm:text-sm h-8 sm:h-9"
              >
                Contacter le support
              </Button>
            </div>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
};
