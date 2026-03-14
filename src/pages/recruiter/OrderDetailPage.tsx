import { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { motion } from 'framer-motion';
import {
  ArrowLeft,
  Package,
  Calendar,
  Clock,
  Users,
  CheckCircle,
  AlertCircle,
  Phone,
  Mail,
  FileText,
  Download,
  Printer,
  MessageSquare,
  TrendingUp,
  UserCheck,
  History,
  Send,
  Building2,
  Briefcase,
  Shield,
  PhoneCall,
  Eye,
  PhoneOff,
  CircleDot,
  Sliders,
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Card, CardContent } from '@/components/ui/card';
import { Progress } from '@/components/ui/progress';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { Textarea } from '@/components/ui/textarea';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { useToast } from '@/hooks/use-toast';
import { getOrdersByRole } from '@/data/recruiterMockData';
import { recruiterAuthService } from '@/services/recruiterAuthService';
import { LeadDetailPopup } from '@/components/recruiter/LeadDetailPopup';
import { LeadDistributionConfig } from '@/components/recruiter/LeadDistributionConfig';

import type { Order } from '@/types/recruiter';

// Extended order details (mock — ready for backend)
const getOrderDetails = (order: Order) => {
  const totalAmount = parseInt(order.amount.replace(/\s/g, '').replace('TND', ''));
  const paidAmount = order.status === 'completed' ? totalAmount : Math.round(totalAmount * 0.5);
  const remainingAmount = totalAmount - paidAmount;
  // Mock derived stats matching STEP2
  const leadsTraites = Math.round(order.delivered * 0.8);
  const entretiens = Math.round(leadsTraites * 0.2);
  const retenus = Math.round(entretiens * 0.65);
  const objectifAtteint = order.integrated;
  const resteAAtteindre = Math.max(0, order.candidates - order.delivered);

  return {
    ...order,
    createdAt: order.date + ' 09:30',
    updatedAt: '05/02/2024 14:45',
    paymentStatus: order.status === 'completed' ? 'paid' : 'partial',
    paymentDate: '16/01/2024',
    paymentMethod: 'Virement bancaire',
    invoiceRef: 'FAC-2024-' + order.id.split('-')[2],
    bonCommandeRef: 'BC-2024-' + order.id.split('-')[2],
    guaranteeDays: 90,
    totalAmount,
    paidAmount,
    remainingAmount,
    remainingPaymentDate: '30/03/2024',
    // Progress summary (STEP2)
    progressSummary: {
      leadsLivres: order.delivered,
      leadsTraites,
      leadsTraitesVsLivres: order.delivered > 0 ? Math.round((leadsTraites / order.delivered) * 100) : 0,
      entretiens,
      retenus,
      integres: order.integrated,
      objectifAtteint,
      resteAAtteindre,
      tauxConversion: order.delivered > 0 ? Math.round((order.integrated / order.delivered) * 100) : 0,
      pctLeadsALivrerVsTraites: leadsTraites > 0 ? Math.round((resteAAtteindre / leadsTraites) * 100) : 0,
    },
    contact: {
      name: 'Wael Ben Ali',
      email: 'wael.benali@techcall.tn',
      phone: '+216 22 123 456',
      role: 'Directeur Financier',
    },
    criteria: {
      languages: ['Français', 'Arabe'],
      experience: '1-3 ans',
      activities: [order.activity],
      availability: 'Immédiate',
      workMode: 'Présentiel',
      workTime: 'Plein temps',
    },
    leads: [
      { id: 'L-001', name: 'Mohamed Benali', status: 'integre', phone: '+216 22 *** ***', score: 92, date: '16/01/2024', lastAction: '16/01/2024 14:00', joignabilite: 'Joignable' },
      { id: 'L-002', name: 'Fatima Trabelsi', status: 'retenu', phone: '+216 23 *** ***', score: 88, date: '17/01/2024', lastAction: '17/01/2024 10:30', joignabilite: 'Joignable' },
      { id: 'L-003', name: 'Ahmed Sahli', status: 'entretien', phone: '+216 24 *** ***', score: 85, date: '18/01/2024', lastAction: '18/01/2024 16:45', joignabilite: 'Joignable' },
      { id: 'L-004', name: 'Sara Ben Amor', status: 'appele', phone: '+216 25 *** ***', score: 82, date: '18/01/2024', lastAction: '18/01/2024 11:00', joignabilite: 'Joignable' },
      { id: 'L-005', name: 'Karim Jebali', status: 'non_joignable', phone: '+216 26 *** ***', score: 78, date: '19/01/2024', lastAction: '19/01/2024 09:15', joignabilite: 'Non joignable' },
      { id: 'L-006', name: 'Leila Mansouri', status: 'a_traiter', phone: '+216 27 *** ***', score: 75, date: '20/01/2024', lastAction: '-', joignabilite: 'Joignable' },
    ],
    timeline: [
      { date: order.date + ' 09:30', action: 'Commande créée', user: 'Wael Ben Ali', type: 'create' },
      { date: order.date + ' 09:35', action: 'Paiement validé', user: 'Système', type: 'payment' },
      { date: order.date + ' 10:00', action: 'Traitement en cours', user: 'Équipe CallMatch', type: 'process' },
      { date: '16/01/2024 14:00', action: '5 leads livrés', user: 'Système', type: 'delivery' },
      { date: '18/01/2024 11:30', action: '5 leads supplémentaires livrés', user: 'Système', type: 'delivery' },
      { date: '20/01/2024 16:00', action: '5 leads supplémentaires livrés', user: 'Système', type: 'delivery' },
    ],
    notes: [
      { author: 'Wael Ben Ali', date: '20/01/2024 15:30', text: 'Livraison en cours, 15 leads déjà envoyés. Bonne qualité selon le client.' },
      { author: 'Support CallMatch', date: '16/01/2024 10:00', text: 'Paiement confirmé, démarrage du traitement de la commande.' },
    ],
  };
};

const getStatusBadge = (status: string) => {
  switch (status) {
    case 'active':
      return <Badge className="bg-primary/10 text-primary border-primary/30 gap-1.5"><Clock className="w-3 h-3" />En cours</Badge>;
    case 'completed':
      return <Badge className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30 gap-1.5"><CheckCircle className="w-3 h-3" />Terminée</Badge>;
    case 'pending':
      return <Badge className="bg-amber-500/10 text-amber-600 border-amber-500/30 gap-1.5"><AlertCircle className="w-3 h-3" />En attente</Badge>;
    default:
      return <Badge variant="outline">{status}</Badge>;
  }
};

const LEAD_STATUSES: Record<string, { label: string; className: string }> = {
  a_traiter: { label: 'En attente', className: 'bg-amber-500/10 text-amber-600 border-amber-500/30' },
  appele: { label: 'Contacté', className: 'bg-sky-500/10 text-sky-600 border-sky-500/30' },
  non_joignable: { label: 'Non joignable', className: 'bg-muted text-muted-foreground' },
  entretien: { label: 'Entretien', className: 'bg-primary/10 text-primary border-primary/30' },
  retenu: { label: 'Retenu', className: 'bg-violet-500/10 text-violet-600 border-violet-500/30' },
  integre: { label: 'Intégré', className: 'bg-emerald-500/10 text-emerald-600 border-emerald-500/30' },
  refuse: { label: 'Refusé', className: 'bg-destructive/10 text-destructive border-destructive/30' },
};

const getLeadStatusBadge = (status: string) => {
  const s = LEAD_STATUSES[status];
  if (!s) return <Badge variant="outline" className="text-xs">{status}</Badge>;
  return <Badge className={`text-xs ${s.className}`}>{s.label}</Badge>;
};

const getTimelineIcon = (type: string) => {
  switch (type) {
    case 'create': return <Package className="w-4 h-4 text-primary" />;
    case 'payment': return <CheckCircle className="w-4 h-4 text-emerald-600" />;
    case 'process': return <Clock className="w-4 h-4 text-amber-600" />;
    case 'delivery': return <Users className="w-4 h-4 text-violet-600" />;
    default: return <History className="w-4 h-4 text-muted-foreground" />;
  }
};

export default function OrderDetailPage() {
  const { orderId } = useParams<{ orderId: string }>();
  const navigate = useNavigate();
  const { toast } = useToast();
  const [note, setNote] = useState('');
  const [leadStatuses, setLeadStatuses] = useState<Record<string, string>>({});

  // Detail popup state
  const [selectedLead, setSelectedLead] = useState<any>(null);
  const [isDetailOpen, setIsDetailOpen] = useState(false);
  const [autoCallOnOpen, setAutoCallOnOpen] = useState(false);

  const user = recruiterAuthService.getUser();
  const role = user?.role ?? 'charge';
  const userName = user?.name ?? '';
  const orders = getOrdersByRole(role, userName);
  const order = orders.find(o => o.id === orderId);

  if (!order) {
    return (
      <div className="flex items-center justify-center min-h-[60vh]">
        <div className="text-center space-y-3">
          <Package className="w-12 h-12 text-muted-foreground mx-auto" />
          <h2 className="text-lg font-semibold">Commande introuvable</h2>
          <p className="text-sm text-muted-foreground">La commande "{orderId}" n'existe pas ou vous n'y avez pas accès.</p>
          <Button onClick={() => navigate('/recruteurs/dashboard?section=orders')}>
            <ArrowLeft className="w-4 h-4 mr-2" />
            Retour aux commandes
          </Button>
        </div>
      </div>
    );
  }

  const details = getOrderDetails(order);
  const progressPercent = Math.round((order.delivered / order.candidates) * 100);
  const conversionRate = order.delivered > 0 ? Math.round((order.integrated / order.delivered) * 100) : 0;

  const handleAddNote = () => {
    if (!note.trim()) return;
    toast({ title: 'Note ajoutée', description: 'Votre commentaire a été enregistré.' });
    setNote('');
  };

  const handleExport = () => {
    toast({ title: 'Export en cours', description: 'Le fichier sera téléchargé dans quelques instants.' });
  };

  const handleFinancialExportCSV = () => {
    const rows = [
      ['Champ', 'Valeur'],
      ['Référence commande', order.id],
      ['Statut paiement', details.paymentStatus === 'paid' ? 'Payé intégralement' : 'Paiement partiel'],
      ['Montant global', order.amount],
      ['Montant payé', `${details.paidAmount.toLocaleString('fr-FR')} TND`],
      ['Reste à payer', `${details.remainingAmount.toLocaleString('fr-FR')} TND`],
      ['Échéance reste à payer', details.remainingAmount > 0 ? details.remainingPaymentDate : 'N/A'],
      ['Bon de commande', details.bonCommandeRef],
      ['Date paiement', details.paymentDate],
      ['Méthode de paiement', details.paymentMethod],
      ['Référence facture', details.invoiceRef],
      ['Contact', details.contact.name],
      ['Email contact', details.contact.email],
      ['Téléphone contact', details.contact.phone],
    ];
    const csvContent = '\uFEFF' + rows.map(r => r.map(c => `"${c}"`).join(';')).join('\n');
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `financier_${order.id}.csv`;
    a.click();
    URL.revokeObjectURL(url);
    toast({ title: 'Export CSV téléchargé', description: `Fichier financier_${order.id}.csv généré.` });
  };

  const handleLeadStatusChange = (leadId: string, newStatus: string) => {
    setLeadStatuses(prev => ({ ...prev, [leadId]: newStatus }));
    const statusLabel = LEAD_STATUSES[newStatus]?.label ?? newStatus;
    toast({
      title: 'Statut mis à jour',
      description: `Le lead ${leadId} est maintenant "${statusLabel}".`,
    });
  };

  const getLeadStatus = (lead: { id: string; status: string }) =>
    leadStatuses[lead.id] ?? lead.status;

  const handleCall = (lead: typeof details.leads[0]) => {
    // Open the detail popup with auto-call enabled
    setSelectedLead({
      id: lead.id,
      nom: lead.name.split(' ').slice(1).join(' ') || lead.name,
      prenom: lead.name.split(' ')[0],
      email: `${lead.name.split(' ')[0].toLowerCase()}.${(lead.name.split(' ')[1] || '').toLowerCase()}@email.com`,
      telephone: lead.phone,
      ville: 'Tunis',
      statut: getLeadStatus(lead),
      score: lead.score,
      dateCreation: lead.date,
      langues: ['Français', 'Arabe'],
      experience: '2 ans',
      activites: [order.activity],
      disponibilite: 'Immédiate',
      source: 'Commande',
      campagne: order.id,
    });
    setAutoCallOnOpen(true);
    setIsDetailOpen(true);
  };

  const handleViewDetails = (lead: typeof details.leads[0]) => {
    setSelectedLead({
      id: lead.id,
      nom: lead.name.split(' ').slice(1).join(' ') || lead.name,
      prenom: lead.name.split(' ')[0],
      email: `${lead.name.split(' ')[0].toLowerCase()}.${(lead.name.split(' ')[1] || '').toLowerCase()}@email.com`,
      telephone: lead.phone,
      ville: 'Tunis',
      statut: getLeadStatus(lead),
      score: lead.score,
      dateCreation: lead.date,
      langues: ['Français', 'Arabe'],
      experience: '2 ans',
      activites: [order.activity],
      disponibilite: 'Immédiate',
      source: 'Commande',
      campagne: order.id,
    });
    setAutoCallOnOpen(false);
    setIsDetailOpen(true);
  };

  return (
    <motion.div
      initial={{ opacity: 0, y: 8 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.2 }}
      className="space-y-5 sm:space-y-6"
    >
      {/* Back + Title */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div className="flex items-center gap-3">
          <Button
            variant="ghost"
            size="icon"
            onClick={() => navigate('/recruteurs/dashboard?section=orders')}
            className="shrink-0 h-9 w-9"
          >
            <ArrowLeft className="w-4 h-4" />
          </Button>
          <div>
            <div className="flex items-center gap-2 flex-wrap">
              <h1 className="text-lg sm:text-xl font-bold text-foreground">{order.id}</h1>
              {getStatusBadge(order.status)}
            </div>
            <p className="text-sm text-muted-foreground">
              {order.type} • {order.city} • Créée le {details.createdAt}
            </p>
          </div>
        </div>
        <div className="flex gap-2">
          <Button variant="outline" size="sm" onClick={handleExport}>
            <Download className="w-4 h-4 sm:mr-1.5" />
            <span className="hidden sm:inline">Exporter</span>
          </Button>
          <Button variant="outline" size="sm" onClick={() => window.print()}>
            <Printer className="w-4 h-4 sm:mr-1.5" />
            <span className="hidden sm:inline">Imprimer</span>
          </Button>
          {order.status === 'active' && (
            <Button size="sm">
              <Package className="w-4 h-4 mr-1.5" />
              Demander + de leads
            </Button>
          )}
        </div>
      </div>

      {/* KPI Cards */}
      <div className="grid grid-cols-2 sm:grid-cols-4 gap-3 sm:gap-4">
        <Card className="border shadow-sm">
          <CardContent className="p-4">
            <div className="flex items-center gap-3">
              <div className="w-10 h-10 rounded-lg bg-primary/10 flex items-center justify-center shrink-0">
                <Users className="w-5 h-5 text-primary" />
              </div>
              <div>
                <p className="text-xl font-bold tabular-nums">{order.delivered}/{order.candidates}</p>
                <p className="text-xs text-muted-foreground">Leads livrés</p>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card className="border shadow-sm">
          <CardContent className="p-4">
            <div className="flex items-center gap-3">
              <div className="w-10 h-10 rounded-lg bg-primary/10 flex items-center justify-center shrink-0">
                <UserCheck className="w-5 h-5 text-primary" />
              </div>
              <div>
                <p className="text-xl font-bold tabular-nums">{order.integrated}</p>
                <p className="text-xs text-muted-foreground">Intégrés</p>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card className="border shadow-sm">
          <CardContent className="p-4">
            <div className="flex items-center gap-3">
              <div className="w-10 h-10 rounded-lg bg-primary/10 flex items-center justify-center shrink-0">
                <TrendingUp className="w-5 h-5 text-primary" />
              </div>
              <div>
                <p className="text-xl font-bold tabular-nums">{conversionRate}%</p>
                <p className="text-xs text-muted-foreground">Conversion</p>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card className="border shadow-sm">
          <CardContent className="p-4">
            <div className="flex items-center gap-3">
              <div className="w-10 h-10 rounded-lg bg-primary/10 flex items-center justify-center shrink-0">
                <Shield className="w-5 h-5 text-primary" />
              </div>
              <div>
                <p className="text-xl font-bold tabular-nums">{details.guaranteeDays}j</p>
                <p className="text-xs text-muted-foreground">Garantie</p>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Progress bar */}
      <Card className="border shadow-sm">
        <CardContent className="p-4">
          <div className="flex items-center justify-between mb-2">
            <span className="text-sm font-medium">Progression de la livraison</span>
            <span className="text-sm font-semibold text-primary tabular-nums">{progressPercent}%</span>
          </div>
          <Progress value={progressPercent} className="h-2.5" />
          <div className="flex justify-between mt-2 text-xs text-muted-foreground">
            <span>{order.delivered} livrés sur {order.candidates}</span>
            <span className="font-medium text-foreground">{order.amount}</span>
          </div>
        </CardContent>
      </Card>

      {/* Tabs content */}
      <Tabs defaultValue="overview" className="space-y-4">
        <TabsList className="bg-muted/50 h-10 sm:h-11 w-full justify-start overflow-x-auto">
          <TabsTrigger value="overview" className="text-xs sm:text-sm gap-1.5">
            <FileText className="w-3.5 h-3.5" />
            Détails
          </TabsTrigger>
          <TabsTrigger value="leads" className="text-xs sm:text-sm gap-1.5">
            <Users className="w-3.5 h-3.5" />
            Leads ({details.leads.length})
          </TabsTrigger>
          <TabsTrigger value="timeline" className="text-xs sm:text-sm gap-1.5">
            <History className="w-3.5 h-3.5" />
            Historique
          </TabsTrigger>
          <TabsTrigger value="notes" className="text-xs sm:text-sm gap-1.5">
            <MessageSquare className="w-3.5 h-3.5" />
            Notes
          </TabsTrigger>
          {(role === 'admin' || role === 'super_admin') && (
            <TabsTrigger value="distribution" className="text-xs sm:text-sm gap-1.5">
              <Sliders className="w-3.5 h-3.5" />
              Distribution
            </TabsTrigger>
          )}
        </TabsList>

        {/* Overview */}
        <TabsContent value="overview" className="space-y-4 mt-0">
          <div className="grid md:grid-cols-2 gap-4">
            {/* Order info */}
            <Card className="border shadow-sm">
              <CardContent className="p-4 sm:p-5 space-y-3">
                <h3 className="font-semibold text-foreground flex items-center gap-2 text-sm">
                  <Package className="w-4 h-4 text-primary" />
                  Informations commande
                </h3>
                <div className="space-y-2.5">
                  {[
                    ['Référence', order.id],
                    ['Type', order.type],
                    ['Date création', details.createdAt],
                    ['Dernière MAJ', details.updatedAt],
                    ['Montant', order.amount],
                  ].map(([label, value]) => (
                    <div key={label} className="flex items-center justify-between">
                      <span className="text-sm text-muted-foreground">{label}</span>
                      <span className={`text-sm font-medium ${label === 'Montant' ? 'text-primary font-semibold' : ''}`}>{value}</span>
                    </div>
                  ))}
                </div>
              </CardContent>
            </Card>

            {/* Criteria */}
            <Card className="border shadow-sm">
              <CardContent className="p-4 sm:p-5 space-y-3">
                <h3 className="font-semibold text-foreground flex items-center gap-2 text-sm">
                  <Briefcase className="w-4 h-4 text-primary" />
                  Critères de recherche
                </h3>
                <div className="space-y-2.5">
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Ville</span>
                    <Badge variant="secondary" className="text-xs">{order.city}</Badge>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Activité</span>
                    <Badge variant="secondary" className="text-xs">{order.activity}</Badge>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Expérience</span>
                    <span className="text-sm font-medium">{details.criteria.experience}</span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Langues</span>
                    <div className="flex gap-1">
                      {details.criteria.languages.map(lang => (
                        <Badge key={lang} variant="outline" className="text-xs">{lang}</Badge>
                      ))}
                    </div>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Mode de travail</span>
                    <span className="text-sm font-medium">{details.criteria.workMode}</span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Temps</span>
                    <span className="text-sm font-medium">{details.criteria.workTime}</span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Disponibilité</span>
                    <span className="text-sm font-medium">{details.criteria.availability}</span>
                  </div>
                </div>
              </CardContent>
            </Card>

            {/* Payment / Financial */}
            <Card className="border shadow-sm">
              <CardContent className="p-4 sm:p-5 space-y-3">
                <div className="flex items-center justify-between">
                  <h3 className="font-semibold text-foreground flex items-center gap-2 text-sm">
                    <CheckCircle className="w-4 h-4 text-emerald-600" />
                    Informations Financières
                  </h3>
                  <Button variant="outline" size="sm" className="h-7 text-xs gap-1" onClick={handleFinancialExportCSV}>
                    <Download className="w-3 h-3" />
                    CSV
                  </Button>
                </div>
                <div className="space-y-2.5">
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Statut</span>
                    <Badge className={details.paymentStatus === 'paid'
                      ? 'bg-emerald-500/10 text-emerald-600 border-emerald-500/30 text-xs'
                      : 'bg-amber-500/10 text-amber-600 border-amber-500/30 text-xs'
                    }>
                      {details.paymentStatus === 'paid' ? 'Payé intégralement' : 'Paiement partiel'}
                    </Badge>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Montant global</span>
                    <span className="text-sm font-bold text-primary">{order.amount}</span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Montant payé</span>
                    <span className="text-sm font-semibold text-emerald-600">{details.paidAmount.toLocaleString('fr-FR')} TND</span>
                  </div>
                  {details.remainingAmount > 0 && (
                    <div className="flex items-center justify-between">
                      <span className="text-sm text-muted-foreground">Reste à payer</span>
                      <span className="text-sm font-semibold text-amber-600">{details.remainingAmount.toLocaleString('fr-FR')} TND</span>
                    </div>
                  )}
                  {details.remainingAmount > 0 && (
                    <div className="flex items-center justify-between">
                      <span className="text-sm text-muted-foreground">Échéance reste</span>
                      <span className="text-sm font-medium">{details.remainingPaymentDate}</span>
                    </div>
                  )}
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Bon de commande</span>
                    <span className="text-sm font-medium font-mono">{details.bonCommandeRef}</span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Date paiement</span>
                    <span className="text-sm font-medium">{details.paymentDate}</span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Méthode</span>
                    <span className="text-sm font-medium">{details.paymentMethod}</span>
                  </div>
                  <div className="flex items-center justify-between">
                    <span className="text-sm text-muted-foreground">Facture</span>
                    <Button variant="link" className="h-auto p-0 text-primary text-sm">
                      {details.invoiceRef}
                      <Download className="w-3 h-3 ml-1" />
                    </Button>
                  </div>
                </div>
              </CardContent>
            </Card>

            {/* Contact */}
            <Card className="border shadow-sm">
              <CardContent className="p-4 sm:p-5 space-y-3">
                <h3 className="font-semibold text-foreground flex items-center gap-2 text-sm">
                  <Building2 className="w-4 h-4 text-primary" />
                  Contact référent
                </h3>
                <div className="flex items-center gap-3 mb-2">
                  <Avatar className="h-10 w-10">
                    <AvatarFallback className="bg-primary/10 text-primary text-sm font-medium">
                      {details.contact.name.split(' ').map(n => n[0]).join('')}
                    </AvatarFallback>
                  </Avatar>
                  <div>
                    <p className="text-sm font-medium">{details.contact.name}</p>
                    <p className="text-xs text-muted-foreground">{details.contact.role}</p>
                  </div>
                </div>
                <div className="space-y-2">
                  <div className="flex items-center gap-2 text-sm">
                    <Mail className="w-4 h-4 text-muted-foreground shrink-0" />
                    <span className="truncate">{details.contact.email}</span>
                  </div>
                  <div className="flex items-center gap-2 text-sm">
                    <Phone className="w-4 h-4 text-muted-foreground shrink-0" />
                    <span>{details.contact.phone}</span>
                  </div>
                </div>
                <Button variant="outline" size="sm" className="w-full mt-2">
                  <MessageSquare className="w-4 h-4 mr-1.5" />
                  Contacter
                </Button>
              </CardContent>
            </Card>
          </div>

          {/* Progress Summary — matching STEP2 "Résumé de l'avancement" */}
          <Card className="border shadow-sm">
            <CardContent className="p-4 sm:p-5 space-y-4">
              <h3 className="font-semibold text-foreground flex items-center gap-2 text-sm">
                <TrendingUp className="w-4 h-4 text-primary" />
                Résumé de l'avancement de la commande
              </h3>
              <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-5 gap-3">
                {[
                  { label: 'Leads livrés', value: details.progressSummary.leadsLivres, color: 'text-primary' },
                  { label: 'Leads traités', value: `${details.progressSummary.leadsTraites} / ${details.progressSummary.leadsLivres}`, color: 'text-foreground' },
                  { label: 'Entretiens programmés', value: details.progressSummary.entretiens, color: 'text-violet-600' },
                  { label: 'Candidats retenus', value: details.progressSummary.retenus, color: 'text-amber-600' },
                  { label: 'Candidats intégrés', value: details.progressSummary.integres, color: 'text-emerald-600' },
                  { label: 'Objectif atteint', value: details.progressSummary.objectifAtteint, color: 'text-emerald-600' },
                  { label: 'Reste à atteindre', value: details.progressSummary.resteAAtteindre, color: 'text-amber-600' },
                  { label: '% Leads traités vs livrés', value: `${details.progressSummary.leadsTraitesVsLivres}%`, color: 'text-primary' },
                  { label: '% Leads à livrer vs traités', value: `${details.progressSummary.pctLeadsALivrerVsTraites}%`, color: 'text-foreground' },
                  { label: 'Taux de conversion', value: `${details.progressSummary.tauxConversion}%`, color: 'text-violet-600' },
                ].map((item) => (
                  <div key={item.label} className="text-center p-3 bg-muted/30 rounded-lg">
                    <p className={`text-xl font-bold tabular-nums ${item.color}`}>{item.value}</p>
                    <p className="text-[10px] sm:text-xs text-muted-foreground mt-1 leading-tight">{item.label}</p>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Leads — matching STEP5 table structure */}
        <TabsContent value="leads" className="mt-0">
          <Card className="border shadow-sm overflow-hidden">
            <CardContent className="p-0">
              {/* Summary bar */}
              <div className="px-4 py-3 bg-muted/30 border-b flex flex-wrap gap-4 text-xs sm:text-sm">
                {(() => {
                  const statusCounts = details.leads.reduce<Record<string, number>>((acc, lead) => {
                    const s = getLeadStatus(lead);
                    acc[s] = (acc[s] || 0) + 1;
                    return acc;
                  }, {});
                  return Object.entries(LEAD_STATUSES)
                    .filter(([key]) => statusCounts[key])
                    .map(([key, cfg]) => (
                      <div key={key} className="flex items-center gap-1.5">
                        <span className="text-muted-foreground">{cfg.label}:</span>
                        <span className="font-semibold tabular-nums">{statusCounts[key]}</span>
                      </div>
                    ));
                })()}
              </div>

              <div className="overflow-x-auto">
                <Table>
                  <TableHeader>
                    <TableRow className="bg-muted/20">
                      <TableHead className="text-xs font-medium">ID Lead</TableHead>
                      <TableHead className="text-xs font-medium">Nom et Prénom</TableHead>
                      <TableHead className="text-xs font-medium">Téléphone</TableHead>
                      <TableHead className="text-xs font-medium">Statut</TableHead>
                      <TableHead className="text-xs font-medium hidden sm:table-cell">Dernière action</TableHead>
                      <TableHead className="text-xs font-medium">Actions</TableHead>
                      <TableHead className="text-xs font-medium text-right">Détails</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {details.leads.map((lead) => {
                      const currentStatus = getLeadStatus(lead);
                      const statusCfg = LEAD_STATUSES[currentStatus] || LEAD_STATUSES.a_traiter;
                      return (
                        <TableRow key={lead.id} className="group hover:bg-muted/30">
                          <TableCell className="text-sm font-medium text-primary font-mono">{lead.id}</TableCell>
                          <TableCell>
                            <div className="flex items-center gap-2.5">
                              <div className="relative shrink-0">
                                <div className="w-8 h-8 rounded-full bg-primary/10 flex items-center justify-center text-xs font-semibold text-primary">
                                  {lead.name.split(' ').map(n => n[0]).join('')}
                                </div>
                                <div className={`absolute -top-0.5 -right-0.5 w-2.5 h-2.5 rounded-full border-2 border-background ${
                                  currentStatus === 'a_traiter' ? 'bg-amber-400' :
                                  currentStatus === 'non_joignable' ? 'bg-muted-foreground' :
                                  'bg-emerald-500'
                                }`} title={currentStatus === 'a_traiter' ? 'Jamais contacté' : currentStatus === 'non_joignable' ? 'Non joignable' : 'Déjà contacté'} />
                              </div>
                              <span className="text-sm font-medium">{lead.name}</span>
                            </div>
                          </TableCell>
                          <TableCell>
                            <span className="font-mono text-sm text-muted-foreground">{lead.phone}</span>
                          </TableCell>
                          <TableCell>
                            <Badge className={`text-xs ${statusCfg.className}`}>{statusCfg.label}</Badge>
                          </TableCell>
                          <TableCell className="text-sm text-muted-foreground hidden sm:table-cell tabular-nums">
                            {lead.lastAction}
                          </TableCell>
                          <TableCell>
                            <Button
                              size="sm"
                              className="h-7 px-2.5 text-xs"
                              onClick={() => handleCall(lead)}
                            >
                              <PhoneCall className="w-3 h-3 mr-1" />
                              {currentStatus === 'a_traiter' ? 'Appeler' : 'Rappeler'}
                            </Button>
                          </TableCell>
                          <TableCell className="text-right">
                            <Button
                              variant="outline"
                              size="sm"
                              className="h-7 px-2.5 text-xs"
                              onClick={() => handleViewDetails(lead)}
                            >
                              <Eye className="w-3 h-3 mr-1" /> Voir les détails
                            </Button>
                          </TableCell>
                        </TableRow>
                      );
                    })}
                  </TableBody>
                </Table>
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Timeline */}
        <TabsContent value="timeline" className="mt-0">
          <Card className="border shadow-sm">
            <CardContent className="p-4 sm:p-5">
              <div className="space-y-4">
                {details.timeline.map((item, index) => (
                  <div key={index} className="relative pl-8">
                    {index < details.timeline.length - 1 && (
                      <div className="absolute left-[11px] top-8 bottom-0 w-0.5 bg-border" />
                    )}
                    <div className="absolute left-0 top-1 w-6 h-6 rounded-full bg-muted flex items-center justify-center">
                      {getTimelineIcon(item.type)}
                    </div>
                    <div className="bg-muted/30 rounded-lg p-3">
                      <div className="flex items-center justify-between mb-1">
                        <span className="font-medium text-sm">{item.action}</span>
                        <span className="text-xs text-muted-foreground">{item.date}</span>
                      </div>
                      <p className="text-xs text-muted-foreground">Par: {item.user}</p>
                    </div>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Notes */}
        <TabsContent value="notes" className="mt-0">
          <Card className="border shadow-sm">
            <CardContent className="p-4 sm:p-5 space-y-4">
              <div>
                <label className="text-sm font-medium mb-2 block">Ajouter une note</label>
                <Textarea
                  value={note}
                  onChange={(e) => setNote(e.target.value)}
                  placeholder="Saisissez votre commentaire concernant cette commande..."
                  rows={3}
                />
                <Button className="mt-3" onClick={handleAddNote} disabled={!note.trim()} size="sm">
                  <Send className="w-4 h-4 mr-2" />
                  Enregistrer
                </Button>
              </div>

              <div className="pt-4 border-t">
                <h4 className="text-sm font-medium mb-3">Notes précédentes</h4>
                <div className="space-y-3">
                  {details.notes.map((n, i) => (
                    <div key={i} className="bg-muted/30 rounded-lg p-3">
                      <div className="flex items-center justify-between mb-1">
                        <span className="text-xs font-medium">{n.author}</span>
                        <span className="text-xs text-muted-foreground">{n.date}</span>
                      </div>
                      <p className="text-sm">{n.text}</p>
                    </div>
                  ))}
                </div>
              </div>
            </CardContent>
          </Card>
        </TabsContent>
        {/* Distribution */}
        {(role === 'admin' || role === 'super_admin') && (
          <TabsContent value="distribution" className="mt-0">
            <LeadDistributionConfig totalLeads={order.candidates} orderId={order.id} userRole={role} />
          </TabsContent>
        )}
      </Tabs>

      {/* Lead Detail Popup */}
      <LeadDetailPopup
        lead={selectedLead}
        isOpen={isDetailOpen}
        onClose={() => { setIsDetailOpen(false); setAutoCallOnOpen(false); }}
        onStatusChange={handleLeadStatusChange}
        autoCall={autoCallOnOpen}
      />
    </motion.div>
  );
}
