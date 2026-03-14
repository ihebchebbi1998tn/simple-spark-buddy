import { FR, TN, GB, ES, DE, IT, PT, NL, RU, CN, TR } from 'country-flag-icons/react/3x2';

export const POSITIONS = [
  { value: '1', label: 'Agent' },
  { value: '2', label: 'Team Leader' },
  { value: '3', label: 'Responsable Activité' },
  { value: '4', label: 'Responsable Qualité' },
  { value: '5', label: 'Responsable Service client' },
  { value: '6', label: 'Responsable Recrutement' },
  { value: '7', label: 'Responsable Plateau' },
  { value: '8', label: 'Formateur' },
  { value: '9', label: 'Chargé Recrutement' },
] as const;

export const EXPERIENCE_LEVELS = [
  { value: '1', label: '0 - 6 mois' },
  { value: '2', label: '6 mois - 12 mois' },
  { value: '3', label: '1 an - 2 ans' },
  { value: '4', label: '2 ans - 3 ans' },
  { value: '5', label: '3 ans - 5 ans' },
  { value: '6', label: '5 ans - 7 ans' },
  { value: '7', label: 'Plus de 7 ans' },
] as const;

export const TUNISIAN_CITIES = [
  { value: 'tunis', label: 'Tunis' },
  { value: 'ariana', label: 'Ariana' },
  { value: 'ben_arous', label: 'Ben Arous' },
  { value: 'manouba', label: 'Manouba' },
  { value: 'nabeul', label: 'Nabeul' },
  { value: 'zaghouan', label: 'Zaghouan' },
  { value: 'bizerte', label: 'Bizerte' },
  { value: 'beja', label: 'Béja' },
  { value: 'jendouba', label: 'Jendouba' },
  { value: 'kef', label: 'Le Kef' },
  { value: 'siliana', label: 'Siliana' },
  { value: 'kairouan', label: 'Kairouan' },
  { value: 'kasserine', label: 'Kasserine' },
  { value: 'sidi_bouzid', label: 'Sidi Bouzid' },
  { value: 'sousse', label: 'Sousse' },
  { value: 'monastir', label: 'Monastir' },
  { value: 'mahdia', label: 'Mahdia' },
  { value: 'sfax', label: 'Sfax' },
  { value: 'gabes', label: 'Gabès' },
  { value: 'medenine', label: 'Médenine' },
  { value: 'tataouine', label: 'Tataouine' },
  { value: 'gafsa', label: 'Gafsa' },
  { value: 'tozeur', label: 'Tozeur' },
  { value: 'kebili', label: 'Kébili' },
] as const;

export const ACTIVITIES = [
  { value: '1', label: 'Télévente' },
  { value: '2', label: 'Téléprospection' },
  { value: '3', label: 'Prise de RDV' },
  { value: '4', label: 'Service client' },
] as const;

export const OPERATIONS = [
  { value: '1', label: 'Énergie Électricité & Gaz' },
  { value: '2', label: 'Énergie Photovoltaïque - Panneaux solaires' },
  { value: '3', label: 'Télécommunication B2B - B2C' },
  { value: '4', label: 'Mutuelle Santé - Assurance' },
  { value: '5', label: 'Voyance - Astrologie' },
  { value: '6', label: 'Produits & Compléments alimentaires' },
  { value: '7', label: 'Sites web & App mobiles' },
  { value: '8', label: 'Télésecretariat' },
  { value: '9', label: 'Dons humanitaires' },
  { value: '10', label: 'Qualification de fichiers' },
  { value: '11', label: 'Création de trafic - Jeux de concours' },
  { value: '12', label: 'Tourisme médical' },
  { value: '13', label: 'Traitement de dossiers administratifs' },
  { value: '14', label: 'CPF - Formations' },
  { value: '15', label: 'Gestion de commandes' },
  { value: '16', label: 'Modération de sites web' },
  { value: '17', label: 'Gestion des réservations' },
] as const;

/**
 * Mapping of operations to their allowed activities
 * Key: operation value, Value: array of activity values that this operation belongs to
 * 
 * Activities: 1=Télévente, 2=Téléprospection, 3=Prise de RDV, 4=Service client
 */
export const OPERATION_ACTIVITY_MAPPING: Record<string, string[]> = {
  '1': ['1', '3', '2'],    // Énergie Électricité & Gaz → Télévente, Prise de RDV, Téléprospection
  '2': ['1', '3', '2'],    // Énergie Photovoltaïque → Télévente, Prise de RDV, Téléprospection
  '3': ['1', '3', '4'],    // Télécommunication B2B-B2C → Télévente, Prise de RDV, Service client
  '4': ['1', '2'],         // Mutuelle Santé - Assurance → Télévente, Téléprospection
  '5': ['1', '2', '4'],    // Voyance - Astrologie → Télévente, Téléprospection, Service client
  '6': ['1', '3', '2'],    // Produits & Compléments alimentaires → Télévente, Prise de RDV, Téléprospection
  '7': ['1', '4', '2'],    // Sites web & App mobiles → Télévente, Service client, Téléprospection
  '8': ['4', '3'],         // Télésecretariat → Service client, Prise de RDV
  '9': ['2'],              // Dons humanitaires → Téléprospection only
  '10': ['1', '2'],        // Qualification de fichiers → Télévente, Téléprospection
  '11': ['1', '2'],        // Création de trafic - Jeux de concours → Télévente, Téléprospection
  '12': ['1', '3', '2'],   // Tourisme médical → Télévente, Prise de RDV, Téléprospection
  '13': ['4'],             // Traitement de dossiers administratifs → Service client only
  '14': ['1', '3', '2'],   // CPF - Formations → Télévente, Prise de RDV, Téléprospection
  '15': ['4'],             // Gestion de commandes → Service client only
  '16': ['4'],             // Modération de sites web → Service client only
  '17': ['4'],             // Gestion des réservations → Service client only
};

/**
 * Get operations filtered by the selected activity
 * Returns only operations that belong to the specified activity
 */
export function getOperationsForActivity(
  activityValue: string,
  allOperations: ReadonlyArray<{ value: string; label: string }>
): Array<{ value: string; label: string }> {
  if (!activityValue) {
    return [...allOperations];
  }

  return allOperations.filter(operation => {
    const allowedActivities = OPERATION_ACTIVITY_MAPPING[operation.value];
    return allowedActivities?.includes(activityValue);
  });
}

export const LANGUAGES = [
  { value: 'Français', label: 'Français', flag: FR, country: 'France' },
  { value: 'Arabe', label: 'Arabe', flag: TN, country: 'Tunisie' },
  { value: 'Anglais', label: 'Anglais', flag: GB, country: 'Royaume-Uni' },
  { value: 'Espagnol', label: 'Espagnol', flag: ES, country: 'Espagne' },
  { value: 'Allemand', label: 'Allemand', flag: DE, country: 'Allemagne' },
  { value: 'Italien', label: 'Italien', flag: IT, country: 'Italie' },
  { value: 'Portugais', label: 'Portugais', flag: PT, country: 'Portugal' },
  { value: 'Néerlandais', label: 'Néerlandais', flag: NL, country: 'Pays-Bas' },
  { value: 'Turc', label: 'Turc', flag: TR, country: 'Turquie' },
  { value: 'Russe', label: 'Russe', flag: RU, country: 'Russie' },
  { value: 'Chinois', label: 'Chinois', flag: CN, country: 'Chine' },
] as const;

export const LANGUAGE_LEVELS = [
  { value: 'debutant', label: 'Débutant', stars: 1, color: 'bg-red-50 text-red-700 border-red-200' },
  { value: 'elementaire', label: 'Élémentaire', stars: 2, color: 'bg-orange-50 text-orange-700 border-orange-200' },
  { value: 'intermediaire', label: 'Intermédiaire', stars: 3, color: 'bg-yellow-50 text-yellow-700 border-yellow-200' },
  { value: 'avance', label: 'Avancé', stars: 4, color: 'bg-green-50 text-green-700 border-green-200' },
  { value: 'courant', label: 'Courant', stars: 5, color: 'bg-blue-50 text-blue-700 border-blue-200' },
  { value: 'natif', label: 'Natif', stars: 5, color: 'bg-purple-50 text-purple-700 border-purple-200' },
] as const;

export const AVAILABILITY_OPTIONS = [
  { value: 'immédiate', label: 'Disponible immédiatement', icon: '⚡', color: 'from-green-50 to-emerald-50 border-green-300' },
  { value: 'en_poste', label: 'En poste mais ouvert(e) à des nouvelles opportunités', icon: '💼', color: 'from-blue-50 to-indigo-50 border-blue-300' },
  { value: '1_semaine', label: 'Disponible sous 1 semaine', icon: '📅', color: 'from-teal-50 to-cyan-50 border-teal-300' },
  { value: '2_semaines', label: 'Disponible sous 2 semaines', icon: '📆', color: 'from-sky-50 to-blue-50 border-sky-300' },
  { value: '1_mois', label: 'Disponible sous 1 mois', icon: '🗓️', color: 'from-indigo-50 to-purple-50 border-indigo-300' },
  { value: 'negociable', label: 'À négocier', icon: '💬', color: 'from-purple-50 to-pink-50 border-purple-300' },
] as const;

/**
 * Filter experience levels based on global experience
 * Users cannot claim more experience in a specific activity than their total global experience
 * 
 * Global experience values (from step 1):
 * "0" = Aucune expérience → can only select "0-6 mois" (value "1")
 * "1" = 0-6 mois → can only select "0-6 mois" (value "1")
 * "2" = 6-12 mois → can select up to "6-12 mois" (values "1", "2")
 * "3" = 1-2 ans → can select up to "1-2 ans" (values "1", "2", "3")
 * etc.
 */
export function filterExperienceLevelsByGlobal(
  globalExperience: string,
  allLevels: Array<{ value: string; label: string }>
): Array<{ value: string; label: string }> {
  // If no global experience set, return all (shouldn't happen in normal flow)
  if (!globalExperience) {
    return allLevels;
  }

  // Map global experience value to maximum allowed specific experience value
  // Global: "0" (no exp) or "1" (0-6 months) → max specific: "1" (0-6 months)
  // Global: "2" (6-12 months) → max specific: "2" (6-12 months)
  // Global: "3" (1-2 years) → max specific: "3" (1-2 years)
  // etc.
  
  const globalExpInt = parseInt(globalExperience, 10);
  
  // For "0" (no experience) or "1" (0-6 months), max allowed is "1" (0-6 months)
  const maxAllowedValue = globalExpInt === 0 ? 1 : globalExpInt;

  // Filter to only show options up to the max allowed value
  return allLevels.filter((level) => {
    const levelValue = parseInt(level.value, 10);
    return levelValue <= maxAllowedValue;
  });
}
