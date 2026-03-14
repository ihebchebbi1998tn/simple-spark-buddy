import { useState } from 'react';
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import {
  Check,
  X,
  Target,
  Zap,
  Crown,
  Users,
  TrendingUp,
  Shield,
  Rocket,
  BarChart3,
  UserCheck,
  Package,
  ArrowLeft,
} from 'lucide-react';
import { PPIConfigurator } from './PPIConfigurator';
import type { PPIConfig } from '@/types/recruiter';
import { OrderConfirmationModal } from './OrderConfirmationModal';
import { SubscriptionOrderModal } from './SubscriptionOrderModal';
import { LeadOrderModal } from './LeadOrderModal';

type TabType = 'pay-per-result' | 'subscription' | 'pay-per-lead';
type View = 'offers' | 'ppi-config';

const tabs: { id: TabType; label: string }[] = [
  { id: 'pay-per-result', label: 'Paiement aux Résultats' },
  { id: 'subscription', label: 'Abonnements Mensuels' },
  { id: 'pay-per-lead', label: 'Paiement par Lead' },
];

const payPerResultOffers = [
  {
    id: 'ppi',
    name: 'Pay Per Integration',
    icon: Target,
    featured: true,
    price: 'Sur devis',
    priceNote: 'Variable selon profil',
    description:
      'Payez uniquement pour les candidats qui intègrent effectivement votre entreprise',
    features: [
      { included: true, text: 'Prix variable selon complexité du profil' },
      { included: true, text: "Garantie d'intégration sous 90 jours" },
      { included: true, text: 'Lots de profils selon vos besoins' },
      { included: true, text: 'Profils qualifiés selon vos critères' },
      { included: true, text: 'Support dédié et suivi rapproché' },
      { included: true, text: 'Paiement après intégration effective' },
    ],
    limits: {
      title: 'Économisez grâce à votre efficacité',
      description: 'Remises progressives selon votre taux de conversion',
    },
  },
];

const subscriptionOffers = [
  {
    id: 'starter',
    name: 'Starter',
    icon: Rocket,
    price: '299',
    priceNote: 'TND / mois',
    description: 'Parfait pour les besoins ponctuels en recrutement',
    limits: { title: '50 leads/mois', description: 'Renouvellement automatique — Sans engagement' },
    features: [
      { included: true, text: 'Accès à 50 leads qualifiés/mois' },
      { included: true, text: 'Tous types de profils inclus' },
      { included: true, text: 'Recherche multicritères' },
      { included: true, text: 'Support email 6j/7' },
      { included: true, text: 'Mise à jour hebdomadaire' },
      { included: false, text: 'Leads non-consommés perdus' },
    ],
  },
  {
    id: 'growth',
    name: 'Growth',
    icon: BarChart3,
    featured: true,
    price: '599',
    priceNote: 'TND / mois',
    description: 'Idéal pour les centres avec recrutement régulier',
    limits: { title: '150 leads/mois', description: 'Reports limités — Engagement 3 mois' },
    features: [
      { included: true, text: 'Accès à 150 leads qualifiés/mois' },
      { included: true, text: 'Tous types de profils inclus' },
      { included: true, text: 'Recherche avancée multicritères' },
      { included: true, text: 'Matching intelligent IA' },
      { included: true, text: 'Support téléphonique 6j/7' },
      { included: true, text: 'Report de 20% des leads' },
    ],
  },
  {
    id: 'enterprise',
    name: 'Enterprise',
    icon: Crown,
    premium: true,
    price: '999',
    priceNote: 'TND / mois',
    description: 'Solution complète pour les grands centres',
    limits: { title: '300 leads/mois', description: 'Reports illimités — Engagement 6 mois' },
    features: [
      { included: true, text: 'Accès à 300 leads qualifiés/mois' },
      { included: true, text: 'Tous types de profils inclus' },
      { included: true, text: 'Tous les filtres avancés' },
      { included: true, text: 'Matching IA prédictif' },
      { included: true, text: 'Support dédié 7j/7' },
      { included: true, text: 'Reports illimités' },
    ],
  },
];

const payPerLeadOffers = [
  {
    id: 'lead-agent',
    name: 'Lead Agent',
    icon: Users,
    price: '15',
    priceNote: 'TND / lead',
    description: 'Accédez à des leads agents qualifiés à l\'unité',
    limits: { title: 'Minimum 10 leads', description: 'Pas d\'engagement — Prix dégressif' },
    features: [
      { included: true, text: 'Leads agents qualifiés' },
      { included: true, text: 'Profils 50-80% de matching' },
      { included: true, text: 'Critères standards inclus' },
      { included: true, text: 'Livraison sous 48h' },
      { included: true, text: 'Pas d\'engagement' },
      { included: false, text: 'Sans garantie d\'intégration' },
    ],
  },
  {
    id: 'lead-responsable',
    name: 'Lead Responsable',
    icon: UserCheck,
    price: '45',
    priceNote: 'TND / lead',
    description: 'Des leads responsables et managers qualifiés',
    limits: { title: 'Minimum 5 leads', description: 'Profil détaillé inclus' },
    features: [
      { included: true, text: 'Leads responsables qualifiés' },
      { included: true, text: 'Profils 70-90% de matching' },
      { included: true, text: 'CV et profil détaillé' },
      { included: true, text: 'Évaluation des compétences' },
      { included: true, text: 'Livraison sous 72h' },
      { included: false, text: 'Sans garantie d\'intégration' },
    ],
  },
  {
    id: 'lot-mixte',
    name: 'Lot Mixte',
    icon: Package,
    featured: true,
    price: '25',
    priceNote: 'TND / lead',
    description: 'Combinaison d\'agents et responsables',
    limits: { title: 'Minimum 20 leads', description: 'Ratio 70% agents — 30% responsables' },
    features: [
      { included: true, text: 'Mix agents et responsables' },
      { included: true, text: 'Prix avantageux en volume' },
      { included: true, text: 'Profils 60-85% de matching' },
      { included: true, text: 'Livraison échelonnée' },
      { included: true, text: 'Flexibilité totale' },
      { included: false, text: 'Sans garantie d\'intégration' },
    ],
  },
];

interface RecruiterOffersProps {
  onGoBack?: () => void;
}

export function RecruiterOffers({ onGoBack }: RecruiterOffersProps) {
  const [activeTab, setActiveTab] = useState<TabType>('pay-per-result');
  const [view, setView] = useState<View>('offers');
  const [ppiConfig, setPpiConfig] = useState<PPIConfig | null>(null);
  const [showConfirmation, setShowConfirmation] = useState(false);

  // Subscription modal state
  const [selectedSubscription, setSelectedSubscription] = useState<typeof subscriptionOffers[0] | null>(null);
  const [showSubscriptionModal, setShowSubscriptionModal] = useState(false);

  // Lead order modal state
  const [selectedLeadOffer, setSelectedLeadOffer] = useState<typeof payPerLeadOffers[0] | null>(null);
  const [showLeadModal, setShowLeadModal] = useState(false);

  const getOffers = () => {
    switch (activeTab) {
      case 'pay-per-result':
        return payPerResultOffers;
      case 'subscription':
        return subscriptionOffers;
      case 'pay-per-lead':
        return payPerLeadOffers;
      default:
        return [];
    }
  };

  const handleOfferClick = (offerId: string) => {
    if (activeTab === 'pay-per-result' && offerId === 'ppi') {
      setView('ppi-config');
    } else if (activeTab === 'subscription') {
      const offer = subscriptionOffers.find(o => o.id === offerId);
      if (offer) {
        setSelectedSubscription(offer);
        setShowSubscriptionModal(true);
      }
    } else if (activeTab === 'pay-per-lead') {
      const offer = payPerLeadOffers.find(o => o.id === offerId);
      if (offer) {
        setSelectedLeadOffer(offer);
        setShowLeadModal(true);
      }
    }
  };

  const handlePPIValidate = (config: PPIConfig) => {
    setPpiConfig(config);
    setShowConfirmation(true);
  };

  const handleModifyOrder = () => {
    setShowConfirmation(false);
    setView('ppi-config');
  };

  // PPI Configurator view
  if (view === 'ppi-config') {
    return (
      <>
        <PPIConfigurator
          onBack={() => setView('offers')}
          onValidate={handlePPIValidate}
        />
        <OrderConfirmationModal
          config={ppiConfig}
          isOpen={showConfirmation}
          onClose={() => setShowConfirmation(false)}
          onModify={handleModifyOrder}
        />
      </>
    );
  }

  const offers = getOffers();

  return (
    <div className="space-y-5 sm:space-y-6">
      {/* Modals */}
      <SubscriptionOrderModal
        offer={selectedSubscription}
        isOpen={showSubscriptionModal}
        onClose={() => setShowSubscriptionModal(false)}
      />
      <LeadOrderModal
        offer={selectedLeadOffer}
        isOpen={showLeadModal}
        onClose={() => setShowLeadModal(false)}
      />

      {/* Page Header */}
      <div className="flex items-start gap-3">
        {onGoBack && (
          <Button variant="outline" size="icon" className="h-9 w-9 shrink-0 mt-0.5" onClick={onGoBack}>
            <ArrowLeft className="w-4 h-4" />
          </Button>
        )}
        <div>
          <h1 className="text-xl sm:text-2xl font-bold text-foreground mb-1">
            Nos Solutions de Recrutement
          </h1>
          <p className="text-sm text-muted-foreground">
            Choisissez le modèle qui correspond le mieux à votre stratégie et budget
          </p>
        </div>
      </div>

      {/* Tabs */}
      <Card className="border shadow-sm overflow-hidden">
        <CardContent className="p-1.5">
          <div className="flex flex-col sm:flex-row gap-1.5">
            {tabs.map((tab) => (
              <Button
                key={tab.id}
                variant={activeTab === tab.id ? 'default' : 'ghost'}
                onClick={() => setActiveTab(tab.id)}
                className="flex-1 text-xs sm:text-sm py-2.5 h-auto"
              >
                {tab.label}
              </Button>
            ))}
          </div>
        </CardContent>
      </Card>

      {/* Offers Grid */}
      <div
        className={`grid gap-4 sm:gap-5 ${
          offers.length === 1
            ? 'max-w-xl mx-auto'
            : offers.length === 2
              ? 'sm:grid-cols-2 max-w-3xl mx-auto'
              : 'sm:grid-cols-2 lg:grid-cols-3'
        }`}
      >
        {offers.map((offer) => {
          const Icon = offer.icon;
          const isFeatured = 'featured' in offer && offer.featured;
          const isPremium = 'premium' in offer && offer.premium;

          return (
            <Card
              key={offer.id}
              className={`border-2 relative flex flex-col transition-all hover:shadow-md ${
                isFeatured
                  ? 'border-primary shadow-md'
                  : isPremium
                    ? 'border-amber-500 shadow-sm'
                    : 'border-border shadow-sm'
              }`}
            >
              {/* Badge */}
              {(isFeatured || isPremium) && (
                <div
                  className={`absolute -top-2.5 left-1/2 -translate-x-1/2 px-3 py-0.5 rounded-full text-xs font-medium text-primary-foreground ${
                    isPremium ? 'bg-amber-500' : 'bg-primary'
                  }`}
                >
                  {isPremium ? 'Premium' : activeTab === 'pay-per-lead' ? 'Économique' : activeTab === 'subscription' ? 'Le plus populaire' : 'Solution Phare'}
                </div>
              )}

              <CardContent className="p-4 sm:p-5 flex flex-col flex-1">
                {/* Icon & Title */}
                <div className="text-center mb-3">
                  <div
                    className={`w-12 h-12 sm:w-14 sm:h-14 mx-auto mb-3 rounded-xl flex items-center justify-center ${
                      isPremium ? 'bg-amber-500/10' : 'bg-primary/10'
                    }`}
                  >
                    <Icon
                      className={`w-6 h-6 sm:w-7 sm:h-7 ${
                        isPremium ? 'text-amber-600' : 'text-primary'
                      }`}
                    />
                  </div>
                  <h3
                    className={`text-lg sm:text-xl font-bold ${
                      isPremium ? 'text-amber-600' : 'text-primary'
                    }`}
                  >
                    {offer.name}
                  </h3>
                </div>

                {/* Price */}
                <div className="text-center mb-3">
                  <div className="flex items-baseline justify-center gap-1">
                    <span
                      className={`text-2xl sm:text-3xl font-bold ${
                        isPremium ? 'text-amber-600' : 'text-foreground'
                      }`}
                    >
                      {offer.price}
                    </span>
                    <span className="text-xs sm:text-sm text-muted-foreground">
                      {offer.priceNote}
                    </span>
                  </div>
                </div>

                {/* Description */}
                <p className="text-xs sm:text-sm text-muted-foreground text-center mb-4">
                  {offer.description}
                </p>

                {/* Limits Box */}
                {'limits' in offer && offer.limits && (
                  <div className="bg-primary/5 border border-primary/20 rounded-lg p-3 mb-4 text-center">
                    <p className="font-medium text-primary text-sm">{offer.limits.title}</p>
                    <p className="text-xs text-muted-foreground mt-0.5">
                      {offer.limits.description}
                    </p>
                  </div>
                )}

                {/* Features */}
                <ul className="space-y-2 sm:space-y-2.5 flex-1 mb-4">
                  {offer.features.map((feature, index) => (
                    <li key={index} className="flex items-start gap-2">
                      {feature.included ? (
                        <Check className="w-4 h-4 text-emerald-600 shrink-0 mt-0.5" />
                      ) : (
                        <X className="w-4 h-4 text-muted-foreground/40 shrink-0 mt-0.5" />
                      )}
                      <span
                        className={`text-xs sm:text-sm ${
                          feature.included ? 'text-foreground' : 'text-muted-foreground/50'
                        }`}
                      >
                        {feature.text}
                      </span>
                    </li>
                  ))}
                </ul>

                {/* CTA Button */}
                <Button
                  className={`w-full h-10 ${
                    isPremium ? 'bg-amber-500 hover:bg-amber-600 text-white' : ''
                  }`}
                  variant={isFeatured || isPremium ? 'default' : 'outline'}
                  onClick={() => handleOfferClick(offer.id)}
                >
                  {activeTab === 'pay-per-result'
                    ? 'Configurer ma commande'
                    : activeTab === 'subscription'
                      ? 'Souscrire'
                      : 'Commander des leads'}
                </Button>
              </CardContent>
            </Card>
          );
        })}
      </div>
    </div>
  );
}
