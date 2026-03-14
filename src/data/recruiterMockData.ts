import type {
  RecruiterRole,
  DashboardStats,
  Order,
  RecentActivity,
  CompanyProfile,
  BillingContact,
  ManagedUser,
  UserPermissionsExtended,
  PerformanceMetrics,
  Campaign,
  TeamMember,
  Lead,
  Objective,
  WeeklyStat,
  FinancialData,
} from '@/types/recruiter';
import { DEFAULT_PERMISSIONS } from '@/types/recruiter';

// ─── Dashboard Stats ─────────────────────────────────────────
const statsByRole: Record<RecruiterRole, DashboardStats> = {
  super_admin: {
    candidatesViewed: 1_248,
    activeOrders: 18,
    pendingIntegrations: 47,
    conversionRate: 72,
    totalRevenue: 145_000,
    totalUsers: 34,
    activeCenters: 6,
  },
  admin: {
    candidatesViewed: 356,
    activeOrders: 7,
    pendingIntegrations: 19,
    conversionRate: 65,
  },
  charge: {
    candidatesViewed: 42,
    activeOrders: 2,
    pendingIntegrations: 5,
    conversionRate: 58,
  },
};

export const getStatsByRole = (role: RecruiterRole): DashboardStats => statsByRole[role];

// ─── Orders ──────────────────────────────────────────────────
const allOrders: Order[] = [
  { id: 'CMD-2024-010', date: '18/01/2024', type: 'Pay Per Integration', candidates: 15, delivered: 8, integrated: 4, status: 'active', amount: '1 200 TND', city: 'Tunis', activity: 'Télévente', assignedTo: 'Ahmed' },
  { id: 'CMD-2024-011', date: '12/01/2024', type: 'Pack 25 Leads', candidates: 25, delivered: 25, integrated: 18, status: 'completed', amount: '750 TND', city: 'Sousse', activity: 'Service Client', assignedTo: 'Ahmed' },
  { id: 'CMD-2024-001', date: '15/01/2024', type: 'Pay Per Integration', candidates: 25, delivered: 15, integrated: 8, status: 'active', amount: '2 500 TND', city: 'Tunis', activity: 'Télévente', assignedTo: 'Fatima' },
  { id: 'CMD-2024-002', date: '10/01/2024', type: 'Pack 50 Leads', candidates: 50, delivered: 50, integrated: 32, status: 'completed', amount: '1 500 TND', city: 'Sfax', activity: 'Service Client', assignedTo: 'Fatima' },
  { id: 'CMD-2024-003', date: '05/01/2024', type: 'Abonnement Pro', candidates: 100, delivered: 25, integrated: 0, status: 'pending', amount: '5 000 TND', city: 'Sousse', activity: 'Prise de RDV', assignedTo: 'Fatima' },
  { id: 'CMD-2024-004', date: '02/01/2024', type: 'Pack 25 Leads', candidates: 25, delivered: 25, integrated: 20, status: 'completed', amount: '750 TND', city: 'Monastir', activity: 'Fidélisation', assignedTo: 'Fatima' },
  { id: 'CMD-2024-005', date: '20/01/2024', type: 'Abonnement Pro', candidates: 200, delivered: 60, integrated: 22, status: 'active', amount: '10 000 TND', city: 'Kairouan', activity: 'SAV', assignedTo: 'Youssef' },
  { id: 'CMD-2024-006', date: '19/01/2024', type: 'Pack 100 Leads', candidates: 100, delivered: 100, integrated: 58, status: 'completed', amount: '2 400 TND', city: 'Bizerte', activity: 'Recouvrement', assignedTo: 'Sara' },
  { id: 'CMD-2024-007', date: '17/01/2024', type: 'Pay Per Integration', candidates: 30, delivered: 30, integrated: 22, status: 'completed', amount: '3 300 TND', city: 'Gabès', activity: 'Télévente', assignedTo: 'Karim' },
  { id: 'CMD-2024-008', date: '14/01/2024', type: 'Pack 50 Leads', candidates: 50, delivered: 10, integrated: 0, status: 'active', amount: '1 500 TND', city: 'Ariana', activity: 'E-commerce', assignedTo: 'Nadia' },
  { id: 'CMD-2024-009', date: '11/01/2024', type: 'Abonnement Pro', candidates: 150, delivered: 0, integrated: 0, status: 'pending', amount: '7 500 TND', city: 'Sfax', activity: 'Banque', assignedTo: 'Omar' },
];

export const getOrdersByRole = (role: RecruiterRole, userName: string): Order[] => {
  switch (role) {
    case 'super_admin':
      return allOrders;
    case 'admin':
      return allOrders.filter(o => o.assignedTo === userName || o.assignedTo === 'Ahmed');
    case 'charge':
      return allOrders.filter(o => o.assignedTo === userName);
  }
};

// ─── Recent Activity ─────────────────────────────────────────
const allActivities: (RecentActivity & { visibleTo: RecruiterRole[] })[] = [
  { id: '1', label: 'Nouvelle commande créée', detail: 'CMD-2024-005 — 200 candidats', time: 'Il y a 2h', type: 'order', visibleTo: ['super_admin'] },
  { id: '2', label: 'Utilisateur ajouté', detail: 'Sara — Chargée de recrutement', time: 'Il y a 3h', type: 'user', visibleTo: ['super_admin'] },
  { id: '3', label: 'Commande terminée', detail: 'CMD-2024-002 — 50 leads livrés', time: 'Il y a 5h', type: 'order', visibleTo: ['super_admin', 'admin'] },
  { id: '4', label: 'Recherche effectuée', detail: 'Tunis • Télévente • Français', time: 'Il y a 6h', type: 'search', visibleTo: ['super_admin', 'admin', 'charge'] },
  { id: '5', label: '4 candidats intégrés', detail: 'CMD-2024-010', time: 'Il y a 8h', type: 'integration', visibleTo: ['super_admin', 'admin', 'charge'] },
  { id: '6', label: 'Commande en attente', detail: 'CMD-2024-003 — en cours de validation', time: 'Hier', type: 'order', visibleTo: ['super_admin', 'admin'] },
];

export const getActivitiesByRole = (role: RecruiterRole): RecentActivity[] =>
  allActivities.filter(a => a.visibleTo.includes(role)).map(({ visibleTo, ...rest }) => rest);

// ─── Company Profile ─────────────────────────────────────────
export const mockCompanyProfile: CompanyProfile = {
  name: 'TechCall Solutions',
  legalName: 'TechCall Solutions SARL',
  registrationNumber: 'RC-2020-12345',
  taxId: 'MF-987654321',
  address: '123 Avenue Habib Bourguiba',
  city: 'Tunis',
  postalCode: '1000',
  country: 'Tunisie',
  phone: '+216 71 123 456',
  email: 'contact@techcall.tn',
  website: 'www.techcall.tn',
  description: "Centre d'appels spécialisé dans le service client et la télévente pour les entreprises européennes.",
  employeeCount: 45,
  maxCapacity: 60,
  occupationRate: 75,
  annualRecruitmentNeed: 120,
  turnoverRate: 18,
  operatingHours: '24h/24, 7j/7',
  languages: ['Français', 'Anglais', 'Arabe'],
  centerType: 'Inbound & Outbound',
  sectors: ['Télécom', 'Banque', 'Assurance'],
  technologies: ['CRM Salesforce', 'Dialer predictive'],
  foundedYear: '2020',
  activities: ['Télévente', 'Service Client', 'SAV', 'Prise de RDV'],
  locationService: false,
  searchingOperations: true,
};

export const mockBillingContacts: BillingContact[] = [
  {
    id: '1',
    civilite: 'M',
    nom: 'Ben Ali',
    prenom: 'Wael',
    fonction: 'Directeur Financier',
    email: 'wael.benali@techcall.tn',
    telephone: '+216 22 123 456',
    type: 'principal',
  },
  {
    id: '2',
    civilite: 'Mme',
    nom: 'Trabelsi',
    prenom: 'Sami',
    fonction: 'Comptable',
    email: 'sami.trabelsi@techcall.tn',
    telephone: '+216 23 234 567',
    type: 'secondaire',
  },
];

// ─── Users ───────────────────────────────────────────────────
const getDefaultExtendedPermissions = (role: RecruiterRole): UserPermissionsExtended => ({
  dashboard: { ...DEFAULT_PERMISSIONS[role] },
  participerDistribution: role !== 'super_admin',
  limiterLangues: true,
  accesReportings: role === 'charge' ? 'lecture' : 'oui',
});

export { getDefaultExtendedPermissions };

export const mockUsers: ManagedUser[] = [
  {
    id: '1',
    prenom: 'Mohammed',
    nom: 'Alami',
    email: 'mohammed.alami@techcall.ma',
    telephone: '+212 661 123 456',
    role: 'super_admin',
    langues: ['Français', 'Arabe', 'Anglais'],
    actif: true,
    permissions: getDefaultExtendedPermissions('super_admin'),
    createdAt: '15/01/2024',
  },
  {
    id: '2',
    prenom: 'Fatima',
    nom: 'Benali',
    email: 'fatima.benali@techcall.ma',
    telephone: '+212 662 234 567',
    role: 'admin',
    langues: ['Français', 'Espagnol'],
    actif: true,
    permissions: getDefaultExtendedPermissions('admin'),
    createdAt: '20/01/2024',
  },
  {
    id: '3',
    prenom: 'Ahmed',
    nom: 'Saidi',
    email: 'ahmed.saidi@techcall.ma',
    telephone: '+212 663 345 678',
    role: 'charge',
    langues: ['Français', 'Anglais'],
    actif: true,
    permissions: getDefaultExtendedPermissions('charge'),
    createdAt: '25/01/2024',
  },
  {
    id: '4',
    prenom: 'Salma',
    nom: 'Idrissi',
    email: 'salma.idrissi@techcall.ma',
    telephone: '+212 664 456 789',
    role: 'charge',
    langues: ['Français', 'Italien'],
    actif: false,
    permissions: getDefaultExtendedPermissions('charge'),
    createdAt: '01/02/2024',
  },
];

// ─── Performance ─────────────────────────────────────────────
export const mockPerformanceMetrics: PerformanceMetrics = {
  leadsTraites: { value: 285, target: 320, percentage: 89 },
  entretiensRealises: { value: 142, target: 200, percentage: 71 },
  candidatsRetenus: { value: 89, target: 120, percentage: 74 },
  candidatsIntegres: { value: 56, target: 80, percentage: 70 },
  tauxConversion: 31,
  tauxReponse: 68,
  tempsTraitementMoyen: '3.2j',
  scoreQualite: 92,
};

export const mockCampaigns: Campaign[] = [
  { id: 'CAMP-2024-001', name: 'Campagne Premium', leadsLivres: 50, leadsTraites: 45, retenus: 22, integres: 15, tauxTraitement: 90, tauxConversionRetenus: 49, tauxConversionIntegres: 68 },
  { id: 'CAMP-2024-002', name: 'Télécom France', leadsLivres: 80, leadsTraites: 72, retenus: 35, integres: 20, tauxTraitement: 90, tauxConversionRetenus: 49, tauxConversionIntegres: 57 },
  { id: 'CAMP-2024-003', name: 'Assurance Pro', leadsLivres: 120, leadsTraites: 98, retenus: 45, integres: 21, tauxTraitement: 82, tauxConversionRetenus: 46, tauxConversionIntegres: 47 },
];

export const mockTeamRanking: TeamMember[] = [
  { rank: 1, name: 'Ahmed Saidi', avatar: 'AS', leadsTraites: 320, tauxConversion: 35, score: 98 },
  { rank: 2, name: 'Wael Wael', avatar: 'ML', leadsTraites: 285, tauxConversion: 31, score: 92 },
  { rank: 3, name: 'Fatima Benali', avatar: 'FB', leadsTraites: 260, tauxConversion: 28, score: 88 },
  { rank: 4, name: 'Karim Tazi', avatar: 'KT', leadsTraites: 245, tauxConversion: 26, score: 82 },
  { rank: 5, name: 'Sara Idrissi', avatar: 'SI', leadsTraites: 220, tauxConversion: 24, score: 78 },
];

export const mockRecentLeads: Lead[] = [
  { id: 'L-001', nom: 'Mohamed Benali', statut: 'entretien_programme', date: '15/01/2024', score: 85 },
  { id: 'L-002', nom: 'Fatima Alaoui', statut: 'a_traiter', date: '15/01/2024', score: 78 },
  { id: 'L-003', nom: 'Ahmed Saidi', statut: 'retenu', date: '14/01/2024', score: 92 },
  { id: 'L-004', nom: 'Sara Benjelloun', statut: 'integre', date: '14/01/2024', score: 88 },
  { id: 'L-005', nom: 'Karim Tazi', statut: 'non_joignable', date: '13/01/2024', score: 72 },
];

export const mockObjectives: Objective[] = [
  { label: 'Leads traités', current: 285, target: 320, color: 'bg-primary' },
  { label: 'Candidats retenus', current: 89, target: 120, color: 'bg-violet-500' },
  { label: 'Intégrations', current: 56, target: 80, color: 'bg-emerald-500' },
];

export const mockWeeklyStats: WeeklyStat[] = [
  { jour: 'Lun', leads: 32, entretiens: 5, integrations: 2 },
  { jour: 'Mar', leads: 28, entretiens: 6, integrations: 1 },
  { jour: 'Mer', leads: 35, entretiens: 4, integrations: 2 },
  { jour: 'Jeu', leads: 30, entretiens: 7, integrations: 1 },
  { jour: 'Ven', leads: 20, entretiens: 6, integrations: 2 },
];

export const mockAdminOrders = [
  { id: 'CMD-2024-001', date: '15/01/2024', status: 'en_cours', leadsLivres: 60, leadsTotal: 200, tauxConversion: 75 },
  { id: 'CMD-2024-002', date: '10/01/2024', status: 'terminee', leadsLivres: 150, leadsTotal: 150, tauxConversion: 82 },
  { id: 'CMD-2024-003', date: '12/01/2024', status: 'en_cours', leadsLivres: 45, leadsTotal: 100, tauxConversion: 68 },
];

// ─── Financial ───────────────────────────────────────────────
export const mockFinancialData: FinancialData = {
  totalOrders: 127,
  totalRevenue: '56 912 TND',
  activeOrders: 42,
  pendingInvoices: 18,
  orders: [
    { ref: 'BC-2024-001', date: '15/01/2024', client: 'Ooredoo Tunisie', amount: '2 500 TND', status: 'validated' },
    { ref: 'BC-2024-002', date: '22/01/2024', client: 'Tunisie Telecom', amount: '1 750 TND', status: 'pending' },
    { ref: 'BC-2024-003', date: '05/02/2024', client: 'BIAT Bank', amount: '3 040 TND', status: 'rejected' },
    { ref: 'BC-2024-004', date: '10/02/2024', client: 'Attijari Bank', amount: '4 200 TND', status: 'validated' },
    { ref: 'BC-2024-005', date: '18/02/2024', client: 'Orange Tunisie', amount: '6 100 TND', status: 'pending' },
  ],
  invoices: [
    { ref: 'FAC-2024-045', date: '05/01/2024', client: 'Ooredoo Tunisie', amount: '2 500 TND', status: 'paid' },
    { ref: 'FAC-2024-046', date: '12/01/2024', client: 'Tunisie Telecom', amount: '1 750 TND', status: 'pending' },
    { ref: 'FAC-2024-047', date: '20/01/2024', client: 'BIAT Bank', amount: '3 040 TND', status: 'overdue' },
    { ref: 'FAC-2024-048', date: '28/01/2024', client: 'Attijari Bank', amount: '4 200 TND', status: 'paid' },
  ],
  contracts: [
    { ref: 'CT-2024-015', dateDebut: '01/01/2024', dateFin: '31/12/2024', client: 'Ooredoo Tunisie', value: '150 000 TND', status: 'active' },
    { ref: 'CT-2024-016', dateDebut: '15/02/2024', dateFin: '14/02/2025', client: 'BIAT Bank', value: '85 000 TND', status: 'active' },
    { ref: 'CT-2023-012', dateDebut: '01/06/2023', dateFin: '31/05/2024', client: 'Tunisie Telecom', value: '120 000 TND', status: 'expiring' },
  ],
};

// ─── Doublons Verification History ───────────────────────────
export const mockVerificationHistory = [
  { id: 'VER-2024-045', date: '15/01/2024', type: 'Import fichier', fichier: 'base_candidats_jan.xlsx', totalLignes: 1250, doublonsDetectes: 87, status: 'completed' },
  { id: 'VER-2024-044', date: '10/01/2024', type: 'API automatique', fichier: '-', totalLignes: 340, doublonsDetectes: 12, status: 'completed' },
  { id: 'VER-2024-043', date: '05/01/2024', type: 'Import fichier', fichier: 'export_crm_dec.csv', totalLignes: 890, doublonsDetectes: 45, status: 'completed' },
  { id: 'VER-2024-042', date: '28/12/2023', type: 'API automatique', fichier: '-', totalLignes: 210, doublonsDetectes: 8, status: 'error' },
];

// ─── Search Options ──────────────────────────────────────────
export const searchOptions = {
  cities: ['Tunis', 'Sfax', 'Sousse', 'Kairouan', 'Bizerte', 'Gabès', 'Ariana', 'Monastir'],
  positions: ['Agent', 'Team Leader', 'Superviseur', 'Manager', 'Responsable Qualité'],
  activities: ['Télévente', 'Prise de RDV', 'Service Client', 'SAV', 'Fidélisation', 'Recouvrement'],
  operations: ['Énergie', 'Télécom', 'Voyance', 'Banque', 'Assurance', 'E-commerce', 'Santé'],
  languages: ['Français', 'Anglais', 'Espagnol', 'Italien', 'Allemand', 'Arabe', 'Néerlandais'],
  experienceLevels: ['0-6 mois', '6-12 mois', '1-2 ans', '2-3 ans', '3-5 ans', '5+ ans'],
  allLanguages: ['Français', 'Anglais', 'Espagnol', 'Allemand', 'Italien', 'Portugais', 'Arabe', 'Néerlandais'],
};

// ─── PPI Configurator Options ────────────────────────────────
export const ppiOptions = {
  postes: [
    { value: 'agent', label: 'Agent' },
    { value: 'team-leader', label: 'Team Leader' },
    { value: 'superviseur', label: 'Superviseur' },
    { value: 'manager', label: 'Manager' },
    { value: 'quality', label: 'Responsable Qualité' },
  ],
  experiences: [
    { value: '0-1', label: '0-1 an' },
    { value: '1-2', label: '1-2 ans' },
    { value: '2-5', label: '2-5 ans' },
    { value: '5+', label: 'Plus de 5 ans' },
  ],
  activites: [
    { value: 'televente', label: 'Télévente' },
    { value: 'prise-rdv', label: 'Prise de RDV' },
    { value: 'service-client', label: 'Service Client' },
    { value: 'sav', label: 'SAV' },
    { value: 'fidelisation', label: 'Fidélisation' },
    { value: 'recouvrement', label: 'Recouvrement' },
  ],
  operations: [
    { value: 'energie', label: 'Énergie' },
    { value: 'telecom', label: 'Télécom' },
    { value: 'voyance', label: 'Voyance' },
    { value: 'banque', label: 'Banque' },
    { value: 'assurance', label: 'Assurance' },
    { value: 'ecommerce', label: 'E-commerce' },
    { value: 'sante', label: 'Santé' },
  ],
  langues: [
    { value: 'french', label: 'Français' },
    { value: 'english', label: 'Anglais' },
    { value: 'italian', label: 'Italien' },
    { value: 'spanish', label: 'Espagnol' },
    { value: 'german', label: 'Allemand' },
    { value: 'arabic', label: 'Arabe' },
    { value: 'dutch', label: 'Néerlandais' },
    { value: 'turkish', label: 'Turc' },
    { value: 'portuguese', label: 'Portugais' },
    { value: 'chinese', label: 'Chinois' },
  ],
  locations: [
    { value: 'tunis', label: 'Grand Tunis', multiplier: 1.0 },
    { value: 'coast', label: 'Côte (Sousse, Monastir)', multiplier: 1.2 },
    { value: 'south', label: 'Sud Tunisien', multiplier: 1.4 },
    { value: 'remote', label: 'Télétravail', multiplier: 0.9 },
  ],
  urgencies: [
    { value: 'normal', label: 'Normal (4-6 semaines)', multiplier: 1.0 },
    { value: 'urgent', label: 'Urgent (2-3 semaines)', multiplier: 1.3 },
    { value: 'critical', label: 'Critique (1-2 semaines)', multiplier: 1.6 },
  ],
  models: [
    { value: 'interview', label: 'Candidat retenu à l\'entretien physique', multiplier: 0.7 },
    { value: 'integration', label: 'Candidat intégré sur site', multiplier: 1.0 },
  ],
  discounts: [
    { percent: '5%', condition: 'Si vous utilisez moins de 90% des leads', featured: false },
    { percent: '10%', condition: 'Si vous utilisez moins de 80% des leads', featured: false },
    { percent: '15%', condition: 'Si vous utilisez moins de 70% des leads', featured: true },
    { percent: '+10 leads', condition: 'Bonus sur votre prochaine commande', featured: false },
  ],
};

// ─── Campaign Leads (STEP5) ──────────────────────────────────
export interface CampaignLead {
  id: string;
  civilite: string;
  nom: string;
  prenom: string;
  telephone: string;
  joignabilite: string;
  dateAppel: string;
  status: 'pending' | 'contacted' | 'qualified' | 'rejected' | 'integrated';
  lastAction: string;
  charge: string;
  niveauLinguistique: string;
  experience: string;
  entretien: string;
  dateEntretien: string;
  presenceEntretien: string;
  retenu: string;
  dateFormation: string;
  presenceFormation: string;
  integration: string;
  statutTraitement: string;
  commentaire: string;
}

export const mockCampaignLeads: Record<string, CampaignLead[]> = {
  'CAMP-2024-001': [
    { id: 'L-001', civilite: 'Mme', nom: 'Martin', prenom: 'Sophie', telephone: '+216 12 *** ***', joignabilite: 'Joignable', dateAppel: '20/01/2024', status: 'pending', lastAction: '-', charge: 'Wael Wael', niveauLinguistique: '', experience: '', entretien: '', dateEntretien: '', presenceEntretien: '', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: '' },
    { id: 'L-002', civilite: 'M', nom: 'Bernard', prenom: 'Thomas', telephone: '+216 23 *** ***', joignabilite: 'Joignable', dateAppel: '15/01/2024', status: 'contacted', lastAction: '15/01/2024 10:30', charge: 'Wael Wael', niveauLinguistique: 'Bon', experience: 'Moyen', entretien: 'Non', dateEntretien: '', presenceEntretien: '', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: 'À rappeler la semaine prochaine' },
    { id: 'L-003', civilite: 'Mme', nom: 'Dubois', prenom: 'Laura', telephone: '+216 34 *** ***', joignabilite: 'Joignable', dateAppel: '18/01/2024', status: 'pending', lastAction: '-', charge: 'Wael Wael', niveauLinguistique: '', experience: '', entretien: '', dateEntretien: '', presenceEntretien: '', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: '' },
    { id: 'L-004', civilite: 'M', nom: 'Robert', prenom: 'Michel', telephone: '+216 45 *** ***', joignabilite: 'Joignable', dateAppel: '14/01/2024', status: 'integrated', lastAction: '14/01/2024 16:45', charge: 'Wael Wael', niveauLinguistique: 'Excellent', experience: 'Excellent', entretien: 'Oui', dateEntretien: '2024-01-20', presenceEntretien: 'Oui', retenu: 'Oui', dateFormation: '2024-01-25', presenceFormation: 'Oui', integration: 'Oui', statutTraitement: 'Intégré', commentaire: 'Candidat idéal, très motivé' },
    { id: 'L-005', civilite: 'Mme', nom: 'Petit', prenom: 'Catherine', telephone: '+216 56 *** ***', joignabilite: 'Non joignable', dateAppel: '19/01/2024', status: 'pending', lastAction: '-', charge: 'Wael Wael', niveauLinguistique: '', experience: '', entretien: '', dateEntretien: '', presenceEntretien: '', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: '' },
    { id: 'L-006', civilite: 'M', nom: 'Leroy', prenom: 'David', telephone: '+216 67 *** ***', joignabilite: 'Joignable', dateAppel: '16/01/2024', status: 'contacted', lastAction: '16/01/2024 14:20', charge: 'Wael Wael', niveauLinguistique: 'Intermédiaire', experience: 'Bon', entretien: 'Non', dateEntretien: '', presenceEntretien: '', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: 'Intéressé mais pas disponible immédiatement' },
    { id: 'L-007', civilite: 'Mme', nom: 'Moreau', prenom: 'Nathalie', telephone: '+216 78 *** ***', joignabilite: 'Joignable', dateAppel: '21/01/2024', status: 'pending', lastAction: '-', charge: 'Wael Wael', niveauLinguistique: '', experience: '', entretien: '', dateEntretien: '', presenceEntretien: '', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: '' },
    { id: 'L-008', civilite: 'M', nom: 'Blanc', prenom: 'Philippe', telephone: '+216 89 *** ***', joignabilite: 'Joignable', dateAppel: '17/01/2024', status: 'rejected', lastAction: '17/01/2024 11:45', charge: 'Wael Wael', niveauLinguistique: 'Débutant', experience: 'KO', entretien: 'Non', dateEntretien: '', presenceEntretien: '', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'Clôturé', commentaire: 'Niveau linguistique insuffisant' },
    { id: 'L-009', civilite: 'Mme', nom: 'Roux', prenom: 'Isabelle', telephone: '+216 90 *** ***', joignabilite: 'Joignable', dateAppel: '13/01/2024', status: 'integrated', lastAction: '13/01/2024 09:15', charge: 'Wael Wael', niveauLinguistique: 'Excellent', experience: 'Excellent', entretien: 'Oui', dateEntretien: '2024-01-22', presenceEntretien: 'Oui', retenu: 'Oui', dateFormation: '2024-01-28', presenceFormation: 'Oui', integration: 'Oui', statutTraitement: 'Intégré', commentaire: 'Très bon profil, expérience confirmée' },
    { id: 'L-010', civilite: 'M', nom: 'Lefebvre', prenom: 'Marc', telephone: '+216 01 *** ***', joignabilite: 'Joignable', dateAppel: '22/01/2024', status: 'pending', lastAction: '-', charge: 'Wael Wael', niveauLinguistique: '', experience: '', entretien: '', dateEntretien: '', presenceEntretien: '', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: '' },
  ],
  'CAMP-2024-002': [
    { id: 'L-011', civilite: 'Mme', nom: 'Gauthier', prenom: 'Christine', telephone: '+216 11 *** ***', joignabilite: 'Joignable', dateAppel: '10/01/2024', status: 'contacted', lastAction: '10/01/2024 11:00', charge: 'Wael Wael', niveauLinguistique: 'Bon', experience: 'Bon', entretien: 'Oui', dateEntretien: '2024-01-18', presenceEntretien: 'Oui', retenu: 'Oui', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: 'En attente de formation' },
    { id: 'L-012', civilite: 'M', nom: 'Dupont', prenom: 'Jean', telephone: '+216 22 *** ***', joignabilite: 'Non joignable', dateAppel: '12/01/2024', status: 'pending', lastAction: '-', charge: 'Wael Wael', niveauLinguistique: '', experience: '', entretien: '', dateEntretien: '', presenceEntretien: '', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: '' },
    { id: 'L-013', civilite: 'Mme', nom: 'Fontaine', prenom: 'Marie', telephone: '+216 33 *** ***', joignabilite: 'Joignable', dateAppel: '08/01/2024', status: 'qualified', lastAction: '08/01/2024 15:30', charge: 'Wael Wael', niveauLinguistique: 'Excellent', experience: 'Bon', entretien: 'Oui', dateEntretien: '2024-01-15', presenceEntretien: 'Oui', retenu: 'Oui', dateFormation: '2024-01-22', presenceFormation: 'Oui', integration: 'Oui', statutTraitement: 'Intégré', commentaire: 'Profil parfait pour le poste' },
    { id: 'L-014', civilite: 'M', nom: 'Simon', prenom: 'Pierre', telephone: '+216 44 *** ***', joignabilite: 'Joignable', dateAppel: '11/01/2024', status: 'contacted', lastAction: '11/01/2024 09:45', charge: 'Wael Wael', niveauLinguistique: 'Intermédiaire', experience: 'Moyen', entretien: 'Non', dateEntretien: '', presenceEntretien: '', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: 'Disponible dans 2 semaines' },
    { id: 'L-015', civilite: 'Mme', nom: 'Laurent', prenom: 'Anne', telephone: '+216 55 *** ***', joignabilite: 'Joignable', dateAppel: '14/01/2024', status: 'pending', lastAction: '-', charge: 'Wael Wael', niveauLinguistique: '', experience: '', entretien: '', dateEntretien: '', presenceEntretien: '', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: '' },
  ],
  'CAMP-2024-003': [
    { id: 'L-016', civilite: 'M', nom: 'Morel', prenom: 'François', telephone: '+216 66 *** ***', joignabilite: 'Joignable', dateAppel: '05/01/2024', status: 'contacted', lastAction: '05/01/2024 10:00', charge: 'Wael Wael', niveauLinguistique: 'Bon', experience: 'Bon', entretien: 'Oui', dateEntretien: '2024-01-12', presenceEntretien: 'Non', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: 'Absent à l\'entretien, à recontacter' },
    { id: 'L-017', civilite: 'Mme', nom: 'Girard', prenom: 'Émilie', telephone: '+216 77 *** ***', joignabilite: 'Joignable', dateAppel: '07/01/2024', status: 'pending', lastAction: '-', charge: 'Wael Wael', niveauLinguistique: '', experience: '', entretien: '', dateEntretien: '', presenceEntretien: '', retenu: '', dateFormation: '', presenceFormation: '', integration: '', statutTraitement: 'En cours', commentaire: '' },
  ],
};

// ─── Label Maps ──────────────────────────────────────────────
export const ppiLabelMap: Record<string, string> = {
  agent: 'Agent',
  'team-leader': 'Team Leader',
  superviseur: 'Superviseur',
  manager: 'Manager',
  quality: 'Responsable Qualité',
  televente: 'Télévente',
  'prise-rdv': 'Prise de RDV',
  'service-client': 'Service Client',
  sav: 'SAV',
  fidelisation: 'Fidélisation',
  recouvrement: 'Recouvrement',
  tunis: 'Grand Tunis',
  coast: 'Côte (Sousse, Monastir)',
  south: 'Sud Tunisien',
  remote: 'Télétravail',
  normal: 'Normal (4-6 semaines)',
  urgent: 'Urgent (2-3 semaines)',
  critical: 'Critique (1-2 semaines)',
  interview: 'Candidat retenu à l\'entretien',
  integration: 'Candidat intégré sur site',
  french: 'Français',
  english: 'Anglais',
  italian: 'Italien',
  spanish: 'Espagnol',
  german: 'Allemand',
  arabic: 'Arabe',
  dutch: 'Néerlandais',
  turkish: 'Turc',
  portuguese: 'Portugais',
  chinese: 'Chinois',
  male: 'Hommes',
  female: 'Femmes',
  both: 'Les deux',
  '0-1': '0-1 an',
  '1-2': '1-2 ans',
  '2-5': '2-5 ans',
  '5+': 'Plus de 5 ans',
};
