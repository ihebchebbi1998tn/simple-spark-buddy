// ─── Roles & Permissions ─────────────────────────────────────
export type RecruiterRole = 'super_admin' | 'admin' | 'charge';

export interface RecruiterPermissions {
  overview: boolean;
  search: boolean;
  orders: boolean;
  offers: boolean;
  profile: boolean;
  users: boolean;
  performance: boolean;
  campaigns: boolean;
}

export interface RecruiterUser {
  id?: string;
  name: string;
  surname: string;
  email: string;
  company: string;
  role: RecruiterRole;
  permissions?: RecruiterPermissions;
}

export interface RecruiterSession {
  user: RecruiterUser;
  token?: string;
  expiresAt?: string;
}

// ─── Orders ──────────────────────────────────────────────────
export interface Order {
  id: string;
  date: string;
  type: string;
  candidates: number;
  delivered: number;
  integrated: number;
  status: 'active' | 'completed' | 'pending';
  amount: string;
  city: string;
  activity: string;
  assignedTo?: string;
}

export interface OrderStats {
  totalOrders: number;
  activeOrders: number;
  totalCandidates: number;
  totalIntegrated: number;
  totalRevenue: number;
  avgConversion: number;
}

export interface OrderDetail extends Order {
  createdAt: string;
  updatedAt: string;
  paymentStatus: string;
  paymentDate: string;
  paymentMethod: string;
  invoiceRef: string;
  contact: {
    name: string;
    email: string;
    phone: string;
  };
  criteria: {
    languages: string[];
    experience: string;
    activities: string[];
    availability: string;
  };
  leads: OrderLead[];
  timeline: OrderTimelineEvent[];
}

export interface OrderLead {
  id: string;
  name: string;
  status: string;
  phone: string;
  score: number;
}

export interface OrderTimelineEvent {
  date: string;
  action: string;
  user: string;
  type: string;
}

// ─── Users (managed team members) ────────────────────────────
export interface UserPermissionsExtended {
  dashboard: RecruiterPermissions;
  participerDistribution: boolean;
  limiterLangues: boolean;
  accesReportings: 'oui' | 'non' | 'lecture';
}

export interface ManagedUser {
  id: string;
  prenom: string;
  nom: string;
  email: string;
  telephone: string;
  role: RecruiterRole;
  langues: string[];
  actif: boolean;
  permissions: UserPermissionsExtended;
  createdAt: string;
}

// ─── Profile / Company ───────────────────────────────────────
export interface CompanyProfile {
  name: string;
  legalName: string;
  registrationNumber: string;
  taxId: string;
  address: string;
  city: string;
  postalCode: string;
  country: string;
  phone: string;
  email: string;
  website: string;
  description: string;
  employeeCount: number;
  maxCapacity: number;
  occupationRate: number;
  annualRecruitmentNeed: number;
  turnoverRate: number;
  operatingHours: string;
  languages: string[];
  centerType: string;
  sectors: string[];
  technologies: string[];
  foundedYear: string;
  activities: string[];
  locationService: boolean;
  searchingOperations: boolean;
}

export interface BillingContact {
  id: string;
  civilite: string;
  nom: string;
  prenom: string;
  fonction: string;
  email: string;
  telephone: string;
  type: 'principal' | 'secondaire' | 'urgence';
}

// ─── Performance ─────────────────────────────────────────────
export interface PerformanceMetrics {
  leadsTraites: { value: number; target: number; percentage: number };
  entretiensRealises: { value: number; target: number; percentage: number };
  candidatsRetenus: { value: number; target: number; percentage: number };
  candidatsIntegres: { value: number; target: number; percentage: number };
  tauxConversion: number;
  tauxReponse: number;
  tempsTraitementMoyen: string;
  scoreQualite: number;
}

export interface Campaign {
  id: string;
  name: string;
  leadsLivres: number;
  leadsTraites: number;
  retenus: number;
  integres: number;
  tauxTraitement: number;
  tauxConversionRetenus: number;
  tauxConversionIntegres: number;
}

export interface TeamMember {
  rank: number;
  name: string;
  avatar: string;
  leadsTraites: number;
  tauxConversion: number;
  score: number;
}

export interface Lead {
  id: string;
  nom: string;
  statut: string;
  date: string;
  score: number;
}

export interface Objective {
  label: string;
  current: number;
  target: number;
  color: string;
}

export interface WeeklyStat {
  jour: string;
  leads: number;
  entretiens: number;
  integrations: number;
}

// ─── Candidate Search ────────────────────────────────────────
export interface CandidateSearchCriteria {
  city: string;
  position: string;
  globalExperience: string;
  positionExperience: string;
  activity: string;
  activityExperience: string;
  operation: string;
  operationExperience: string;
  primaryLanguage: string;
  secondaryLanguage: string;
  workMode: string;
  workTime: string;
}

export interface SearchResults {
  highMatch: number;
  acceptableMatch: number;
  mediumMatch: number;
  lowMatch: number;
  outOfTarget: number;
}

// ─── PPI Config ──────────────────────────────────────────────
export interface PPIConfig {
  poste: string;
  expGlobale: string;
  expPoste: string;
  activite: string;
  expActivite: string;
  operation: string;
  expOperation: string;
  genre: string;
  languePrincipale: string;
  bilingue: boolean;
  langueSecondaire: string;
  candidatesCount: number;
  location: string;
  urgency: string;
  model: string;
  costPerCandidate: number;
  leadsProvided: number;
  totalEstimate: number;
}

// ─── Financial ───────────────────────────────────────────────
export interface FinancialOrder {
  ref: string;
  date: string;
  client: string;
  amount: string;
  status: 'validated' | 'pending' | 'rejected';
}

export interface Invoice {
  ref: string;
  date: string;
  client: string;
  amount: string;
  status: 'paid' | 'pending' | 'overdue';
}

export interface Contract {
  ref: string;
  dateDebut: string;
  dateFin: string;
  client: string;
  value: string;
  status: 'active' | 'expiring' | 'expired';
}

export interface FinancialData {
  totalOrders: number;
  totalRevenue: string;
  activeOrders: number;
  pendingInvoices: number;
  orders: FinancialOrder[];
  invoices: Invoice[];
  contracts: Contract[];
}

// ─── Dashboard Stats ─────────────────────────────────────────
export interface DashboardStats {
  candidatesViewed: number;
  activeOrders: number;
  pendingIntegrations: number;
  conversionRate: number;
  totalRevenue?: number;
  totalUsers?: number;
  activeCenters?: number;
}

export interface RecentActivity {
  id: string;
  label: string;
  detail: string;
  time: string;
  type: 'order' | 'search' | 'integration' | 'user';
}

// ─── Constants ───────────────────────────────────────────────
export const ROLE_LABELS: Record<RecruiterRole, string> = {
  super_admin: 'Super Admin',
  admin: 'Administrateur',
  charge: 'Chargé de recrutement',
};

export const DEFAULT_PERMISSIONS: Record<RecruiterRole, RecruiterPermissions> = {
  super_admin: {
    overview: true,
    search: true,
    orders: true,
    offers: true,
    profile: true,
    users: true,
    performance: true,
    campaigns: true,
  },
  admin: {
    overview: true,
    search: true,
    orders: true,
    offers: true,
    profile: true,
    users: false,
    performance: false,
    campaigns: true,
  },
  charge: {
    overview: false,
    search: false,
    orders: false,
    offers: false,
    profile: false,
    users: false,
    performance: true,
    campaigns: true,
  },
};

export const PERMISSION_LABELS: Record<keyof RecruiterPermissions, string> = {
  overview: 'Tableau de bord',
  search: 'Recherche candidats',
  orders: 'Gestion commandes',
  offers: 'Nos offres',
  profile: 'Profil centre',
  users: 'Gestion utilisateurs',
  performance: 'Performance & Analytics',
  campaigns: 'Campagnes en cours',
};

// ─── Helpers ─────────────────────────────────────────────────
export const getAllowedSections = (user: RecruiterUser): string[] => {
  const perms = user.permissions ?? DEFAULT_PERMISSIONS[user.role];
  const sections = (Object.keys(perms) as (keyof RecruiterPermissions)[]).filter(k => perms[k]);
  // my-account is always accessible
  sections.push('my-account' as any);
  return sections;
};

export const ROLE_PERMISSIONS: Record<RecruiterRole, string[]> = {
  super_admin: ['overview', 'search', 'orders', 'offers', 'profile', 'users', 'performance', 'campaigns'],
  admin: ['overview', 'search', 'orders', 'offers', 'profile', 'campaigns'],
  charge: ['performance', 'campaigns'],
};
