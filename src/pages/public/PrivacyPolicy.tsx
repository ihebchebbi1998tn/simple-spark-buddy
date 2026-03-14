import Header from "@/components/shared/Header";
import Footer from "@/components/shared/Footer";

const PrivacyPolicy = () => {
  return (
    <div className="min-h-screen">
      <Header />
      <main className="pt-20">
        <div className="container mx-auto px-4 lg:px-8 max-w-4xl py-16">
          <div className="text-center mb-12">
            <h1 className="text-4xl lg:text-5xl font-bold text-brand-dark mb-4">
              Politique de <span className="text-primary">Confidentialité</span>
            </h1>
            <p className="text-lg text-muted-foreground">
              Dernière mise à jour : {new Date().toLocaleDateString('fr-FR')}
            </p>
          </div>
          <div className="prose prose-lg mx-auto text-gray-700">
            <section className="mb-8">
              <p className="mb-4">
                Bienvenue sur CallCenterMatch.ai, une plateforme en ligne propriété exclusive de la société BBS Datatech Sarl, immatriculée sous le numéro 000/M/A/1905938H et demeurant à [Appartement N°2, 2ème étage Bloc B du complexe immobilier Espace Tunis Montplaisir, rue 8002, Tunis 1002], dédiée principalement à la mise en relation entre candidats et recruteurs du secteur des centres d’appels. La plateforme permet également de connecter d’autres acteurs de ce secteur au-delà du recrutement.
              </p>
              <p className="mb-4">
                CallCenterMatch.ai facilite ces interactions, mais ne doit en aucun cas être considérée comme une société d’intérim ou un recruteur direct. Toute activité de recrutement interne pour renforcer les équipes de CallCenterMatch.ai sera exclusivement gérée par la societé BBS Datatech Sarl.
              </p>
              <p className="mb-4">
                La protection de vos données personnelles est une priorité pour nous. Cette politique de confidentialité explique comment nous collectons, utilisons, stockons et protégeons vos informations personnelles conformément à la loi tunisienne n° 2004-63 du 27 juillet 2004 relative à la protection des données à caractère personnel.
              </p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">1. Collecte des données personnelles</h2>
              <p className="mb-4">Nous collectons les informations suivantes lorsque vous utilisez notre plateforme et/ou lorsque vous vous inscrivez via nos campagnes marketing :</p>
              <h3 className="text-lg font-semibold mt-4 mb-2">a) Pour les candidats</h3>
              <ul className="list-disc pl-6 mb-4">
                <li>Nom, prénom</li>
                <li>Adresse e-mail, numéro de téléphone, ville et âge</li>
                <li>Expérience professionnelle et compétences</li>
                <li>Niveau linguistique</li>
                <li>Préférences de travail (temps plein, temps partiel, télétravail, etc.)</li>
                <li>Prétentions salariales et preferences en terme d’aventages en nature</li>
                <li>Résultats des tests (psychotechniques, linguistiques, entretien IA)</li>
                <li>Informations techniques (adresse IP, données de navigation, cookies)</li>
              </ul>
              <h3 className="text-lg font-semibold mt-4 mb-2">b) Pour les recruteurs</h3>
              <ul className="list-disc pl-6 mb-4">
                <li>Nom de l’entreprise</li>
                <li>Informations de contact (nom, prénom du responsable/contact principal, e-mail, téléphone, ville, etc.)</li>
                <li>Besoins en recrutement (critères recherchés, objectifs escomptés)</li>
                <li>Informations légales et comptables sur l’entreprise</li>
                <li>Informations sur les utilisateurs de la plateforme s’ils diffèrent du contact principal</li>
              </ul>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">2. Utilisation des données</h2>
              <ul className="list-disc pl-6 mb-4">
                <li>Faciliter la mise en relation entre candidats et employeurs.</li>
                <li>Améliorer nos services et adapter les offres aux besoins des utilisateurs.</li>
                <li>Vérifier l’authenticité des profils et prévenir la fraude.</li>
                <li>Envoyer des notifications et informations relatives aux mises en relation.</li>
                <li>Analyser et optimiser notre plateforme via des statistiques anonymes.</li>
                <li>Assurer le respect de nos conditions générales d’utilisation.</li>
                <li>Effectuer des campagnes marketing et publicitaires ciblées (avec consentement préalable).</li>
              </ul>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">3. Base légale du traitement des données</h2>
              <ul className="list-disc pl-6 mb-4">
                <li>Votre consentement, lorsque vous créez un compte directement sur CallCenterMatch.ai ou vous vous inscrivez via nos campagnes marketing.</li>
                <li>L’exécution d’un contrat, lors de la mise en relation entre candidats et employeurs.</li>
                <li>Nos obligations légales, pour respecter la réglementation en vigueur.</li>
                <li>Notre intérêt légitime, pour améliorer nos services et garantir leur sécurité.</li>
              </ul>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">4. Stockage et durée de conservation des données</h2>
              <ul className="list-disc pl-6 mb-4">
                <li>Tant que votre compte est actif sur notre plateforme.</li>
                <li>Jusqu’à 2 ans après votre dernière activité en cas d’inactivité.</li>
                <li>Pendant 5 ans en cas d’obligation légale ou de litige.</li>
              </ul>
              <p className="mb-4">Vos données sont stockées sur des serveurs sécurisés en Tunisie ou dans des pays offrant un niveau de protection adéquat.</p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">5. Partage des données</h2>
              <ul className="list-disc pl-6 mb-4">
                <li>Les employeurs recherchant des candidats correspondant à leurs critères.</li>
                <li>Nos prestataires techniques et partenaires, pour l’hébergement, la sécurité et l’analyse des données.</li>
                <li>Les autorités judiciaires ou administratives, en cas d’obligation légale.</li>
                <li>Les annonceurs et partenaires commerciaux, uniquement après obtention de votre consentement.</li>
              </ul>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">6. Sécurité des données</h2>
              <ul className="list-disc pl-6 mb-4">
                <li>Cryptage des données sensibles.</li>
                <li>Accès restreint aux informations personnelles.</li>
                <li>Systèmes de détection des fraudes et intrusions.</li>
                <li>Hébergement sécurisé et conformité aux normes en vigueur.</li>
              </ul>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">7. Vos droits</h2>
              <ul className="list-disc pl-6 mb-4">
                <li>Accès : obtenir une copie de vos données personnelles.</li>
                <li>Rectification : modifier ou mettre à jour vos informations.</li>
                <li>Suppression : demander l’effacement de vos données sous certaines conditions.</li>
                <li>Opposition : refuser le traitement de vos données à des fins marketing.</li>
                <li>Portabilité : récupérer vos informations sous un format structuré.</li>
              </ul>
              <p className="mb-4">Vous pouvez exercer ces droits directement via votre compte sur CallCenterMatch.ia ou en nous contactant à <a href="mailto:contact@callcentermatch.ai" className="text-primary underline">contact@callcentermatch.ai</a>.</p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">8. Cookies et technologies similaires</h2>
              <ul className="list-disc pl-6 mb-4">
                <li>Améliorer votre expérience utilisateur.</li>
                <li>Personnaliser le contenu affiché.</li>
                <li>Analyser le trafic sur notre site.</li>
              </ul>
              <p className="mb-4">Vous pouvez configurer votre navigateur pour refuser les cookies, mais certaines fonctionnalités du site pourraient être limitées.</p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">9. Protection contre les violations de données</h2>
              <p className="mb-4">Nous avons mis en place un plan d’action en cas de violation de données personnelles. Si nous détectons une atteinte à la sécurité de vos données, nous prendrons immédiatement des mesures pour limiter les impacts et informerons l’INPDP sous 72 heures si nécessaire.</p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">10. Modifications de la politique de confidentialité</h2>
              <p className="mb-4">Nous nous réservons le droit de modifier cette politique à tout moment. Toute modification sera publiée sur notre site et, si nécessaire, nous vous en informerons par e-mail ou via nos réseaux sociaux.</p>
            </section>
            <section className="mb-8">
              <h2 className="text-2xl font-semibold text-brand-dark mb-4">11. Contact</h2>
              <p className="mb-4">Pour toute question relative à cette politique de confidentialité, vous pouvez nous contacter à <a href="mailto:contact@callcentermatch.ai" className="text-primary underline">contact@callcentermatch.ai</a>.</p>
            </section>
            <section className="mb-8">
              <p className="mb-4">Nous vous remercions de votre confiance et nous engageons à protéger vos données personnelles avec le plus grand soin.</p>
            </section>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default PrivacyPolicy;
