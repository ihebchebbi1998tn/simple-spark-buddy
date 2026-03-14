import { Tour, TourStep } from '@/components/ui/tour';

interface RecruiterTourProps {
  run: boolean;
  onFinish: () => void;
  userRole: 'super_admin' | 'admin' | 'charge';
}

const getSteps = (userRole: string): TourStep[] => {
  const baseSteps: TourStep[] = [
    {
      target: 'body',
      content: (
        <div className="text-left">
          <h3 className="font-bold text-lg mb-2">🎉 Bienvenue sur l'Espace Recruteur !</h3>
          <p className="text-sm text-muted-foreground">
            Découvrez comment utiliser notre plateforme pour trouver les meilleurs candidats pour votre centre d'appels.
          </p>
        </div>
      ),
      placement: 'center',
    },
    {
      target: '[data-tour="recruiter-overview"]',
      content: (
        <div className="text-left">
          <h3 className="font-bold mb-2">📊 Tableau de Bord</h3>
          <p className="text-sm text-muted-foreground">
            Vue d'ensemble de votre activité : candidats disponibles, commandes en cours et statistiques clés.
          </p>
        </div>
      ),
      placement: 'right',
    },
    {
      target: '[data-tour="recruiter-search"]',
      content: (
        <div className="text-left">
          <h3 className="font-bold mb-2">🔍 Recherche de Candidats</h3>
          <p className="text-sm text-muted-foreground">
            Utilisez notre simulateur avancé pour trouver des candidats selon vos critères : ville, langues, expérience, opérations.
          </p>
        </div>
      ),
      placement: 'right',
    },
    {
      target: '[data-tour="recruiter-orders"]',
      content: (
        <div className="text-left">
          <h3 className="font-bold mb-2">🛒 Mes Commandes</h3>
          <p className="text-sm text-muted-foreground">
            Suivez vos commandes de candidats, leur statut de livraison et gérez vos achats.
          </p>
        </div>
      ),
      placement: 'right',
    },
    {
      target: '[data-tour="recruiter-offers"]',
      content: (
        <div className="text-left">
          <h3 className="font-bold mb-2">📦 Nos Offres</h3>
          <p className="text-sm text-muted-foreground">
            Découvrez nos différentes formules : Starter, Business et Premium pour répondre à vos besoins de recrutement.
          </p>
        </div>
      ),
      placement: 'right',
    },
  ];

  const adminSteps: TourStep[] = [
    {
      target: '[data-tour="recruiter-profile"]',
      content: (
        <div className="text-left">
          <h3 className="font-bold mb-2">🏢 Profil Centre</h3>
          <p className="text-sm text-muted-foreground">
            Gérez les informations de votre centre d'appels : coordonnées, description et paramètres.
          </p>
        </div>
      ),
      placement: 'right',
    },
  ];

  const superAdminSteps: TourStep[] = [
    {
      target: '[data-tour="recruiter-users"]',
      content: (
        <div className="text-left">
          <h3 className="font-bold mb-2">👥 Gestion des Utilisateurs</h3>
          <p className="text-sm text-muted-foreground">
            Ajoutez, modifiez ou supprimez des utilisateurs et définissez leurs permissions d'accès.
          </p>
        </div>
      ),
      placement: 'right',
    },
    {
      target: '[data-tour="recruiter-performance"]',
      content: (
        <div className="text-left">
          <h3 className="font-bold mb-2">📈 Performance</h3>
          <p className="text-sm text-muted-foreground">
            Analysez les performances de votre équipe, suivez les KPIs et consultez le classement.
          </p>
        </div>
      ),
      placement: 'right',
    },
  ];

  const headerSteps: TourStep[] = [
    {
      target: '[data-tour="recruiter-notifications"]',
      content: (
        <div className="text-left">
          <h3 className="font-bold mb-2">🔔 Notifications</h3>
          <p className="text-sm text-muted-foreground">
            Recevez des alertes sur les nouvelles candidatures et les mises à jour de vos commandes.
          </p>
        </div>
      ),
      placement: 'bottom',
    },
    {
      target: '[data-tour="recruiter-user-menu"]',
      content: (
        <div className="text-left">
          <h3 className="font-bold mb-2">👤 Votre Compte</h3>
          <p className="text-sm text-muted-foreground">
            Accédez à vos paramètres personnels et déconnectez-vous en toute sécurité.
          </p>
        </div>
      ),
      placement: 'bottom',
    },
  ];

  // Build final list based on role
  let allSteps = [...baseSteps];
  
  if (userRole === 'admin' || userRole === 'super_admin') {
    allSteps = [...allSteps, ...adminSteps];
  }
  
  if (userRole === 'super_admin') {
    allSteps = [...allSteps, ...superAdminSteps];
  }
  
  return [...allSteps, ...headerSteps];
};

export function RecruiterTour({ run, onFinish, userRole }: RecruiterTourProps) {
  const steps = getSteps(userRole);

  return (
    <Tour
      steps={steps}
      run={run}
      onFinish={onFinish}
      locale={{
        back: 'Retour',
        close: 'Fermer',
        last: 'Terminer',
        next: 'Suivant',
        skip: 'Passer',
      }}
    />
  );
}
