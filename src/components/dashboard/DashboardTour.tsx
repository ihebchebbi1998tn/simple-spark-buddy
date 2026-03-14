import { Tour, TourStep } from '@/components/ui/tour';

interface DashboardTourProps {
  run: boolean;
  onFinish: () => void;
}

const steps: TourStep[] = [
  {
    target: 'body',
    content: (
      <div className="text-left">
        <h3 className="font-bold text-lg mb-2">🎉 Bienvenue dans votre espace candidat !</h3>
        <p className="text-sm text-muted-foreground leading-relaxed">
          Ce guide vous aidera à naviguer et à compléter votre profil pour maximiser vos chances d'être recruté(e).
        </p>
      </div>
    ),
    placement: 'center',
  },
  {
    target: '[data-tour="sidebar-overview"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">📊 Vue d'ensemble</h3>
        <p className="text-sm text-muted-foreground">
          Accédez à un résumé de votre profil, votre progression et les actions à compléter.
        </p>
      </div>
    ),
    placement: 'right',
  },
  {
    target: '[data-tour="sidebar-profile"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">👤 Mon Profil</h3>
        <p className="text-sm text-muted-foreground">
          Mettez à jour vos informations : Coordonnées et détails de votre recherche.
        </p>
      </div>
    ),
    placement: 'right',
  },
  {
    target: '[data-tour="sidebar-activities"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">💼 Mes Activités</h3>
        <p className="text-sm text-muted-foreground">
          Ajoutez vos expériences professionnelles et les opérations que vous maîtrisez (télévente, SAV, etc.).
        </p>
      </div>
    ),
    placement: 'right',
  },
  {
    target: '[data-tour="sidebar-languages"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">🌍 Mes Langues</h3>
        <p className="text-sm text-muted-foreground">
          Indiquez les langues que vous parlez et passez des tests pour certifier votre niveau.
        </p>
      </div>
    ),
    placement: 'right',
  },
  {
    target: '[data-tour="sidebar-callcenters"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">🏢 Mes Centres d'Appels</h3>
        <p className="text-sm text-muted-foreground">
          Sélectionnez les centres que vous souhaitez éviter et ceux que vous aimeriez bien intégrer.
        </p>
      </div>
    ),
    placement: 'right',
  },
  {
    target: '[data-tour="sidebar-proximity"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">📍 Proximité Géographique</h3>
        <p className="text-sm text-muted-foreground">
          Définissez les villes à proximité où vous êtes prêt à travailler.
        </p>
      </div>
    ),
    placement: 'right',
  },
  {
    target: '[data-tour="sidebar-availability"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">📅 Ma Disponibilité</h3>
        <p className="text-sm text-muted-foreground">
          Définissez vos horaires et votre disponibilité pour les recruteurs.
        </p>
      </div>
    ),
    placement: 'right',
  },
  {
    target: '[data-tour="sidebar-preferences"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">📋 Préférences Contractuelles</h3>
        <p className="text-sm text-muted-foreground">
          Indiquez vos préférences de contrat : CDI, CDD, temps plein, temps partiel, etc.
        </p>
      </div>
    ),
    placement: 'right',
  },
  {
    target: '[data-tour="sidebar-access"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">🔐 Gérer mes Accès</h3>
        <p className="text-sm text-muted-foreground">
          Modifiez votre mot de passe et gérez vos paramètres de sécurité.
        </p>
      </div>
    ),
    placement: 'right',
  },
  {
    target: '[data-tour="header-progress"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">📈 Barre de Progression</h3>
        <p className="text-sm text-muted-foreground">
          Suivez l'avancement de votre profil. Un profil complet augmente vos chances d'être contacté !
        </p>
      </div>
    ),
    placement: 'bottom',
  },
  {
    target: '[data-tour="header-test"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">📝 Tests de Langue</h3>
        <p className="text-sm text-muted-foreground">
          Passez des tests pour certifier vos compétences linguistiques et vous démarquer.
        </p>
      </div>
    ),
    placement: 'bottom',
  },
  {
    target: '[data-tour="header-alerts"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">🔔 Notifications</h3>
        <p className="text-sm text-muted-foreground">
          Recevez des alertes sur les nouvelles opportunités et les mises à jour importantes.
        </p>
      </div>
    ),
    placement: 'bottom',
  },
  {
    target: '[data-tour="header-support"]',
    content: (
      <div className="text-left">
        <h3 className="font-bold mb-2">🆘 Support</h3>
        <p className="text-sm text-muted-foreground">
          Besoin d'aide ? Contactez notre équipe support à tout moment.
        </p>
      </div>
    ),
    placement: 'bottom',
  },
];

export function DashboardTour({ run, onFinish }: DashboardTourProps) {
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
