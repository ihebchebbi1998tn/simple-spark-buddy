import Header from "@/components/shared/Header";
import Footer from "@/components/shared/Footer";

const TermsOfService = () => {
  return (
    <div className="min-h-screen">
      <Header />
      <main className="pt-20">
        <div className="container mx-auto px-4 lg:px-8 max-w-4xl py-16">
          <div className="text-center mb-12">
            <h1 className="text-4xl lg:text-5xl font-bold text-brand-dark mb-4">
              Conditions Générales d'Utilisation <span className="text-primary">de callcentermatch.ai</span>
            </h1>
            <p className="text-lg text-muted-foreground">
              Dernière mise à jour : {new Date().toLocaleDateString('fr-FR')}
            </p>
          </div>
          <div className="prose prose-lg mx-auto text-gray-700">
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">A/ INTRODUCTION</h2>
              <p className="mb-4">
                La plateforme callcentermatch.ai (ci-après dénommée "la Plateforme") est un service en ligne, propriété exclusive de la société BBS Datatech Sarl, permettant aux Utilisateurs (centres d'appels et professionnels) de se mettre en relation pour faciliter le recrutement, la collaboration et la gestion des ressources humaines dans le domaine des centres d'appels. La Plateforme n'est pas un cabinet de recrutement et n'exerce pas d'activité de consultation en matière de recrutement. Elle ne garantit en aucun cas la conformité, l'exactitude ou la qualité des offres de ses partenaires recruteurs, des profils des Utilisateurs, ou des informations échangées.
              </p>
              <p className="mb-4">
                En utilisant la Plateforme, l'Utilisateur reconnaît avoir lu, compris et accepté sans réserve l'ensemble des conditions, obligations et modalités d'utilisation décrites ci-dessous.
              </p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">B/ TERMINOLOGIE</h2>
              <ul className="list-disc pl-6 mb-4">
                <li><b>Plateforme</b> : Désigne le site internet callcentermatch.ai et l'ensemble des services associés.</li>
                <li><b>Utilisateur</b> : Toute personne physique ou morale utilisant les services de la Plateforme, qu'il s'agisse d'un centre d'appels (Utilisateur Recruteur) ou d'un professionnel (Utilisateur Candidat).</li>
                <li><b>Service Gratuit</b> : Services de base offerts gratuitement, tels que la création de compte, la publication d'offres d'emploi ou de profils professionnels.</li>
                <li><b>Service Payant</b> : Services supplémentaires proposés aux Utilisateurs Recruteurs, tels que l’achat des packs basiques ou des abonnements pour accéder à des fonctionnalités avancées (par exemple, accès à une base de données élargie de candidats).</li>
                <li><b>Données Personnelles</b> : Toutes les informations permettant d'identifier directement ou indirectement un Utilisateur (nom, adresse e-mail, numéro de téléphone, etc.).</li>
              </ul>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">C/ OBJET</h2>
              <p className="mb-4">
                Les présentes Conditions Générales d'Utilisation (CGU) régissent l'accès et l'utilisation de la Plateforme callcentermatch.ai par les Utilisateurs. Elles définissent également les modalités d'utilisation des services gratuits et payants proposés par la Plateforme.
              </p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">D/ UTILISATION DES SERVICES</h2>
              <h3 className="text-xl font-semibold text-brand-dark mb-2">1. Service Gratuit</h3>
              <ul className="list-disc pl-6 mb-4">
                <li>Créer un compte personnel (pour les candidats) et professionnel (pour les centres d’appels).</li>
                <li>Accéder à un simulateur de la base de données de candidats (pour les centres d'appels).</li>
                <li>Recevoir des propositions d’embauche par téléphone et/ou par e-mail (pour les candidats).</li>
                <li>Accéder à des outils de gestion des candidatures.</li>
              </ul>
              <p className="mb-4">
                Les Utilisateurs s'engagent à utiliser les services de manière honnête, légale et conforme aux présentes CGU. La Plateforme se réserve le droit de supprimer tout contenu jugé non conforme à ses valeurs.
              </p>
              <h3 className="text-xl font-semibold text-brand-dark mb-2">2. Service Payant</h3>
              <ul className="list-disc pl-6 mb-4">
                <li>L'accès à une base de données élargie de candidats.</li>
                <li>Des outils avancés de gestion des candidatures.</li>
                <li>Des services de mise en relation prioritaire.</li>
                <li>Etc...</li>
              </ul>
              <p className="mb-4">
                Les détails des packs et abonnements payants, y compris leurs tarifs et durées, sont disponibles sur demande auprès de notre service commercial à l'adresse <a href="mailto:commercial@callcentermatch.ai" className="text-primary underline">commercial@callcentermatch.ai</a>
              </p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">E/ ENGAGEMENTS DES UTILISATEURS</h2>
              <ul className="list-disc pl-6 mb-4">
                <li>Fournir des informations exactes et à jour lors de leur inscription.</li>
                <li>Respecter les lois en vigueur et les règles de déontologie professionnelle.</li>
                <li>Ne pas publier de contenu illégal, diffamatoire, discriminatoire ou contraire aux bonnes mœurs.</li>
                <li>Ne pas utiliser la Plateforme à des fins commerciales ou publicitaires sans autorisation préalable.</li>
              </ul>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">F/ RESPONSABILITÉS</h2>
              <h3 className="text-xl font-semibold text-brand-dark mb-2">1. Responsabilité de la Plateforme</h3>
              <ul className="list-disc pl-6 mb-4">
                <li>Met en œuvre tous les moyens raisonnables pour assurer la qualité et la sécurité des services.</li>
                <li>Se réserve le droit de modifier, suspendre ou interrompre les services à tout moment, sans préavis.</li>
                <li>Ne garantit pas la disponibilité, l'exactitude ou la fiabilité des offres d'emploi, des profils ou des informations publiées par les Utilisateurs.</li>
              </ul>
              <h3 className="text-xl font-semibold text-brand-dark mb-2">2. Responsabilité des Utilisateurs</h3>
              <ul className="list-disc pl-6 mb-4">
                <li>De l'utilisation qu'ils font de la Plateforme.</li>
                <li>Des contenus qu'ils publient (offres d'emploi, CV, messages, etc.).</li>
                <li>Des relations qu'ils établissent avec d'autres Utilisateurs via la Plateforme.</li>
              </ul>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">G/ DONNÉES PERSONNELLES</h2>
              <p className="mb-4">
                La Plateforme collecte et traite les données personnelles des Utilisateurs dans le respect de la réglementation applicable (loi tunisienne sur la protection des données, etc.). Les données collectées sont utilisées pour :
              </p>
              <ul className="list-disc pl-6 mb-4">
                <li>Faciliter la mise en relation entre centres d'appels et professionnels.</li>
                <li>Améliorer les services proposés.</li>
                <li>Envoyer des communications relatives aux services (alertes, newsletters, etc.).</li>
              </ul>
              <p className="mb-4">
                Les Utilisateurs disposent d'un droit d'accès, de rectification et de suppression de leurs données personnelles. Plus d’informations à ce sujet sont disponibles dans la section Politique de confidentialité.
              </p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">H/ PROPRIÉTÉ INTELLECTUELLE</h2>
              <p className="mb-4">
                Tous les contenus (textes, images, logos, etc.) présents sur la Plateforme sont la propriété exclusive de callcentermatch.ai ou de ses partenaires. Les Utilisateurs s'engagent à ne pas reproduire, modifier ou exploiter ces contenus sans autorisation préalable.
              </p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">I/ MODIFICATION DES CGU</h2>
              <p className="mb-4">
                La Plateforme se réserve le droit de modifier les présentes CGU à tout moment. Les nouvelles conditions seront applicables dès leur publication en ligne. Les Utilisateurs sont invités à consulter régulièrement les CGU pour prendre connaissance des éventuelles modifications.
              </p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">J/ LOI APPLICABLE ET LITIGES</h2>
              <p className="mb-4">
                Les présentes CGU sont régies par la loi tunisienne. En cas de litige, les tribunaux compétents sont ceux de Tunis. Les Utilisateurs s'engagent à privilégier une résolution amiable avant toute action en justice.
              </p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">K/ CONTACT</h2>
              <p className="mb-4">
                Pour toute question ou réclamation concernant les CGU ou les services de la Plateforme, les Utilisateurs peuvent contacter callcentermatch.ai à l'adresse suivante : <a href="mailto:commercial@callcentermatch.ai" className="text-primary underline">commercial@callcentermatch.ai</a>
              </p>
            </section>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default TermsOfService;
