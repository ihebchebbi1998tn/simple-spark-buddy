import { useState } from 'react';
import { 
  X, 
  Package, 
  MapPin, 
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
  UserX,
  PhoneCall,
  CalendarCheck,
  History,
  Send,
  ChevronRight,
  Building2,
  Briefcase
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Progress } from '@/components/ui/progress';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { ScrollArea } from '@/components/ui/scroll-area';
import { Textarea } from '@/components/ui/textarea';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { useToast } from '@/hooks/use-toast';

interface Order {
  id: string;
  date: string;
  type: string;
  candidates: number;
  delivered: number;
  integrated: number;
  status: string;
  amount: string;
  city: string;
  activity: string;
}

interface OrderDetailModalProps {
  order: Order | null;
  isOpen: boolean;
  onClose: () => void;
}

// Extended order details
const getOrderDetails = (order: Order) => ({
  ...order,
  createdAt: order.date + ' 09:30',
  updatedAt: '05/02/2024 14:45',
  paymentStatus: 'paid',
  paymentDate: '16/01/2024',
  paymentMethod: 'Virement bancaire',
  invoiceRef: 'FAC-2024-' + order.id.split('-')[2],
  contact: {
    name: 'Wael Ben Ali',
    email: 'wael.benali@techcall.tn',
    phone: '+216 22 123 456',
  },
  criteria: {
    languages: ['Français', 'Arabe'],
    experience: '1-3 ans',
    activities: [order.activity],
    availability: 'Immédiate',
  },
  leads: [
    { id: 'L-001', name: 'Mohamed Benali', status: 'integre', phone: '+216 22 ***', score: 92 },
    { id: 'L-002', name: 'Fatima Trabelsi', status: 'retenu', phone: '+216 23 ***', score: 88 },
    { id: 'L-003', name: 'Ahmed Sahli', status: 'entretien', phone: '+216 24 ***', score: 85 },
    { id: 'L-004', name: 'Sara Ben Amor', status: 'appele', phone: '+216 25 ***', score: 82 },
    { id: 'L-005', name: 'Karim Jebali', status: 'non_joignable', phone: '+216 26 ***', score: 78 },
    { id: 'L-006', name: 'Leila Mansouri', status: 'a_traiter', phone: '+216 27 ***', score: 75 },
  ],
  timeline: [
    { date: order.date + ' 09:30', action: 'Commande créée', user: 'Wael Ben Ali', type: 'create' },
    { date: order.date + ' 09:35', action: 'Paiement validé', user: 'Système', type: 'payment' },
    { date: order.date + ' 10:00', action: 'Traitement en cours', user: 'Équipe CallMatch', type: 'process' },
    { date: '16/01/2024 14:00', action: '5 leads livrés', user: 'Système', type: 'delivery' },
    { date: '18/01/2024 11:30', action: '5 leads livrés', user: 'Système', type: 'delivery' },
    { date: '20/01/2024 16:00', action: '5 leads livrés', user: 'Système', type: 'delivery' },
  ],
});

export function OrderDetailModal({ order, isOpen, onClose }: OrderDetailModalProps) {
  const { toast } = useToast();
  const [activeTab, setActiveTab] = useState('overview');
  const [note, setNote] = useState('');

  if (!order) return null;

  const details = getOrderDetails(order);
  const progressPercent = Math.round((order.delivered / order.candidates) * 100);
  const conversionRate = order.delivered > 0 ? Math.round((order.integrated / order.delivered) * 100) : 0;

  const getStatusBadge = (status: string) => {
    switch (status) {
      case 'active':
        return <Badge className="bg-primary/10 text-primary border-primary/30">En cours</Badge>;
      case 'completed':
        return <Badge className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30">Terminée</Badge>;
      case 'pending':
        return <Badge className="bg-amber-500/10 text-amber-600 border-amber-500/30">En attente</Badge>;
      default:
        return <Badge variant="outline">{status}</Badge>;
    }
  };

  const getLeadStatusBadge = (status: string) => {
    switch (status) {
      case 'integre':
        return <Badge className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30 text-xs">Intégré</Badge>;
      case 'retenu':
        return <Badge className="bg-violet-500/10 text-violet-600 border-violet-500/30 text-xs">Retenu</Badge>;
      case 'entretien':
        return <Badge className="bg-primary/10 text-primary border-primary/30 text-xs">Entretien</Badge>;
      case 'appele':
        return <Badge className="bg-sky-500/10 text-sky-600 border-sky-500/30 text-xs">Appelé</Badge>;
      case 'non_joignable':
        return <Badge className="bg-muted text-muted-foreground text-xs">Non joignable</Badge>;
      case 'a_traiter':
        return <Badge className="bg-amber-500/10 text-amber-600 border-amber-500/30 text-xs">À traiter</Badge>;
      default:
        return <Badge variant="outline" className="text-xs">{status}</Badge>;
    }
  };

  const getTimelineIcon = (type: string) => {
    switch (type) {
      case 'create':
        return <Package className="w-4 h-4 text-primary" />;
      case 'payment':
        return <CheckCircle className="w-4 h-4 text-emerald-600" />;
      case 'process':
        return <Clock className="w-4 h-4 text-amber-600" />;
      case 'delivery':
        return <Users className="w-4 h-4 text-violet-600" />;
      default:
        return <History className="w-4 h-4 text-muted-foreground" />;
    }
  };

  const handleAddNote = () => {
    if (!note.trim()) return;
    toast({
      title: 'Note ajoutée',
      description: 'Votre commentaire a été enregistré.',
    });
    setNote('');
  };

  const handleExport = () => {
    toast({
      title: 'Export en cours',
      description: 'Le fichier sera téléchargé dans quelques instants.',
    });
  };

  const handlePrint = () => {
    window.print();
    toast({
      title: 'Impression',
      description: 'La fenêtre d\'impression a été ouverte.',
    });
  };

  return (
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent className="max-w-4xl max-h-[90vh] p-0 gap-0 overflow-hidden w-[95vw] sm:w-full">
        {/* Header */}
        <div className="bg-gradient-to-r from-primary to-primary/80 p-4 sm:p-5 text-primary-foreground">
          <DialogHeader>
            <div className="flex flex-col sm:flex-row sm:items-start sm:justify-between gap-3">
              <div className="flex items-center gap-3 sm:gap-4">
                <div className="w-12 h-12 sm:w-14 sm:h-14 rounded-xl bg-primary-foreground/20 flex items-center justify-center shrink-0">
                  <Package className="w-6 h-6 sm:w-7 sm:h-7" />
                </div>
                <div className="min-w-0">
                  <DialogTitle className="text-lg sm:text-xl font-bold text-primary-foreground">
                    {order.id}
                  </DialogTitle>
                  <p className="text-primary-foreground/80 text-xs sm:text-sm mt-0.5">
                    {order.type} • {order.city}
                  </p>
                  <div className="flex flex-wrap items-center gap-2 mt-2">
                    {getStatusBadge(order.status)}
                    <Badge className="bg-primary-foreground/20 text-primary-foreground border-0 text-xs">
                      {order.amount}
                    </Badge>
                  </div>
                </div>
              </div>
              <div className="flex gap-2 shrink-0">
                <Button variant="secondary" size="sm" onClick={handleExport} className="h-8">
                  <Download className="w-4 h-4 sm:mr-1" />
                  <span className="hidden sm:inline">Export</span>
                </Button>
                <Button variant="secondary" size="sm" onClick={handlePrint} className="h-8">
                  <Printer className="w-4 h-4 sm:mr-1" />
                  <span className="hidden sm:inline">Imprimer</span>
                </Button>
              </div>
            </div>
          </DialogHeader>
        </div>

        {/* Progress Summary */}
        <div className="px-4 sm:px-5 py-3 bg-muted/30 border-b flex flex-wrap gap-4 sm:gap-6">
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center">
              <Users className="w-4 h-4 text-primary" />
            </div>
            <div>
              <p className="text-lg font-bold text-foreground">{order.delivered}/{order.candidates}</p>
              <p className="text-[10px] text-muted-foreground">Livrés</p>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-lg bg-emerald-500/10 flex items-center justify-center">
              <UserCheck className="w-4 h-4 text-emerald-600" />
            </div>
            <div>
              <p className="text-lg font-bold text-emerald-600">{order.integrated}</p>
              <p className="text-[10px] text-muted-foreground">Intégrés</p>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-lg bg-violet-500/10 flex items-center justify-center">
              <TrendingUp className="w-4 h-4 text-violet-600" />
            </div>
            <div>
              <p className="text-lg font-bold text-violet-600">{conversionRate}%</p>
              <p className="text-[10px] text-muted-foreground">Conversion</p>
            </div>
          </div>
          <div className="flex-1 min-w-[150px]">
            <div className="flex items-center justify-between mb-1">
              <span className="text-xs text-muted-foreground">Progression</span>
              <span className="text-xs font-medium">{progressPercent}%</span>
            </div>
            <Progress value={progressPercent} className="h-2" />
          </div>
        </div>

        {/* Tabs */}
        <Tabs value={activeTab} onValueChange={setActiveTab} className="flex-1 flex flex-col">
          <div className="border-b px-3 sm:px-5 overflow-x-auto">
            <TabsList className="h-11 sm:h-12 bg-transparent p-0 gap-1 sm:gap-3 w-full justify-start">
              <TabsTrigger 
                value="overview" 
                className="data-[state=active]:bg-transparent data-[state=active]:shadow-none data-[state=active]:border-b-2 data-[state=active]:border-primary rounded-none px-2 sm:px-3 pb-3 text-xs sm:text-sm shrink-0"
              >
                <FileText className="w-4 h-4 mr-1.5" />
                Détails
              </TabsTrigger>
              <TabsTrigger 
                value="leads" 
                className="data-[state=active]:bg-transparent data-[state=active]:shadow-none data-[state=active]:border-b-2 data-[state=active]:border-primary rounded-none px-2 sm:px-3 pb-3 text-xs sm:text-sm shrink-0"
              >
                <Users className="w-4 h-4 mr-1.5" />
                Leads ({details.leads.length})
              </TabsTrigger>
              <TabsTrigger 
                value="timeline" 
                className="data-[state=active]:bg-transparent data-[state=active]:shadow-none data-[state=active]:border-b-2 data-[state=active]:border-primary rounded-none px-2 sm:px-3 pb-3 text-xs sm:text-sm shrink-0"
              >
                <History className="w-4 h-4 mr-1.5" />
                Historique
              </TabsTrigger>
              <TabsTrigger 
                value="notes" 
                className="data-[state=active]:bg-transparent data-[state=active]:shadow-none data-[state=active]:border-b-2 data-[state=active]:border-primary rounded-none px-2 sm:px-3 pb-3 text-xs sm:text-sm shrink-0"
              >
                <MessageSquare className="w-4 h-4 mr-1.5" />
                Notes
              </TabsTrigger>
            </TabsList>
          </div>

          <ScrollArea className="flex-1 h-[350px] sm:h-[400px]">
            {/* Overview Tab */}
            <TabsContent value="overview" className="p-4 sm:p-5 m-0">
              <div className="grid md:grid-cols-2 gap-4 sm:gap-6">
                {/* Order Info */}
                <div className="space-y-4">
                  <h3 className="font-semibold text-foreground flex items-center gap-2 text-sm sm:text-base">
                    <Package className="w-4 h-4 text-primary" />
                    Informations commande
                  </h3>
                  <div className="space-y-3 bg-muted/30 rounded-xl p-3 sm:p-4">
                    <div className="flex items-center justify-between">
                      <span className="text-sm text-muted-foreground">Référence</span>
                      <span className="text-sm font-medium">{order.id}</span>
                    </div>
                    <div className="flex items-center justify-between">
                      <span className="text-sm text-muted-foreground">Type</span>
                      <span className="text-sm font-medium">{order.type}</span>
                    </div>
                    <div className="flex items-center justify-between">
                      <span className="text-sm text-muted-foreground">Date création</span>
                      <span className="text-sm font-medium">{details.createdAt}</span>
                    </div>
                    <div className="flex items-center justify-between">
                      <span className="text-sm text-muted-foreground">Dernière MAJ</span>
                      <span className="text-sm font-medium">{details.updatedAt}</span>
                    </div>
                    <div className="flex items-center justify-between">
                      <span className="text-sm text-muted-foreground">Montant</span>
                      <span className="text-sm font-bold text-primary">{order.amount}</span>
                    </div>
                  </div>
                </div>

                {/* Criteria */}
                <div className="space-y-4">
                  <h3 className="font-semibold text-foreground flex items-center gap-2 text-sm sm:text-base">
                    <Briefcase className="w-4 h-4 text-primary" />
                    Critères de recherche
                  </h3>
                  <div className="space-y-3 bg-muted/30 rounded-xl p-3 sm:p-4">
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
                    <div className="flex items-start justify-between">
                      <span className="text-sm text-muted-foreground">Langues</span>
                      <div className="flex flex-wrap gap-1 justify-end">
                        {details.criteria.languages.map(lang => (
                          <Badge key={lang} variant="outline" className="text-xs">{lang}</Badge>
                        ))}
                      </div>
                    </div>
                    <div className="flex items-center justify-between">
                      <span className="text-sm text-muted-foreground">Disponibilité</span>
                      <span className="text-sm font-medium">{details.criteria.availability}</span>
                    </div>
                  </div>
                </div>

                {/* Payment Info */}
                <div className="space-y-4">
                  <h3 className="font-semibold text-foreground flex items-center gap-2 text-sm sm:text-base">
                    <CheckCircle className="w-4 h-4 text-emerald-600" />
                    Paiement
                  </h3>
                  <div className="space-y-3 bg-emerald-500/5 border border-emerald-500/20 rounded-xl p-3 sm:p-4">
                    <div className="flex items-center justify-between">
                      <span className="text-sm text-muted-foreground">Statut</span>
                      <Badge className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30 text-xs">Payé</Badge>
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
                </div>

                {/* Contact */}
                <div className="space-y-4">
                  <h3 className="font-semibold text-foreground flex items-center gap-2 text-sm sm:text-base">
                    <Building2 className="w-4 h-4 text-primary" />
                    Contact référent
                  </h3>
                  <div className="space-y-3 bg-muted/30 rounded-xl p-3 sm:p-4">
                    <div className="flex items-center gap-3">
                      <div className="w-10 h-10 rounded-full bg-primary/10 flex items-center justify-center">
                        <span className="text-sm font-bold text-primary">
                          {details.contact.name.split(' ').map(n => n[0]).join('')}
                        </span>
                      </div>
                      <div>
                        <p className="text-sm font-medium">{details.contact.name}</p>
                        <p className="text-xs text-muted-foreground">Contact principal</p>
                      </div>
                    </div>
                    <div className="flex items-center gap-2 text-sm">
                      <Mail className="w-4 h-4 text-muted-foreground" />
                      <span>{details.contact.email}</span>
                    </div>
                    <div className="flex items-center gap-2 text-sm">
                      <Phone className="w-4 h-4 text-muted-foreground" />
                      <span>{details.contact.phone}</span>
                    </div>
                  </div>
                </div>
              </div>
            </TabsContent>

            {/* Leads Tab */}
            <TabsContent value="leads" className="p-0 m-0">
              <div className="overflow-x-auto">
                <Table>
                  <TableHeader>
                    <TableRow className="bg-muted/30">
                      <TableHead className="text-xs font-medium">ID</TableHead>
                      <TableHead className="text-xs font-medium">Nom</TableHead>
                      <TableHead className="text-xs font-medium hidden sm:table-cell">Téléphone</TableHead>
                      <TableHead className="text-xs font-medium hidden md:table-cell">Score</TableHead>
                      <TableHead className="text-xs font-medium">Statut</TableHead>
                      <TableHead className="text-xs font-medium text-right">Actions</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {details.leads.map((lead) => (
                      <TableRow key={lead.id} className="hover:bg-muted/30">
                        <TableCell className="text-sm font-medium">{lead.id}</TableCell>
                        <TableCell className="text-sm">{lead.name}</TableCell>
                        <TableCell className="text-sm text-muted-foreground hidden sm:table-cell">{lead.phone}</TableCell>
                        <TableCell className="hidden md:table-cell">
                          <div className="flex items-center gap-2">
                            <div className="w-12 h-1.5 bg-muted rounded-full overflow-hidden">
                              <div 
                                className={`h-full rounded-full ${lead.score >= 85 ? 'bg-emerald-500' : lead.score >= 70 ? 'bg-amber-500' : 'bg-red-500'}`}
                                style={{ width: `${lead.score}%` }}
                              />
                            </div>
                            <span className="text-xs font-medium">{lead.score}%</span>
                          </div>
                        </TableCell>
                        <TableCell>{getLeadStatusBadge(lead.status)}</TableCell>
                        <TableCell className="text-right">
                          <Button variant="ghost" size="sm" className="h-7 px-2">
                            <ChevronRight className="w-4 h-4" />
                          </Button>
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </div>
            </TabsContent>

            {/* Timeline Tab */}
            <TabsContent value="timeline" className="p-4 sm:p-5 m-0">
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
            </TabsContent>

            {/* Notes Tab */}
            <TabsContent value="notes" className="p-4 sm:p-5 m-0">
              <div className="space-y-4">
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
                    <div className="bg-muted/30 rounded-lg p-3">
                      <div className="flex items-center justify-between mb-1">
                        <span className="text-xs font-medium">Wael Ben Ali</span>
                        <span className="text-xs text-muted-foreground">20/01/2024 15:30</span>
                      </div>
                      <p className="text-sm">Livraison en cours, 15 leads déjà envoyés. Bonne qualité selon le client.</p>
                    </div>
                    <div className="bg-muted/30 rounded-lg p-3">
                      <div className="flex items-center justify-between mb-1">
                        <span className="text-xs font-medium">Support CallMatch</span>
                        <span className="text-xs text-muted-foreground">16/01/2024 10:00</span>
                      </div>
                      <p className="text-sm">Paiement confirmé, démarrage du traitement de la commande.</p>
                    </div>
                  </div>
                </div>
              </div>
            </TabsContent>
          </ScrollArea>
        </Tabs>

        {/* Footer */}
        <div className="border-t p-3 sm:p-4 bg-muted/30 flex flex-wrap gap-2 justify-between">
          <Button variant="outline" onClick={onClose} className="h-9 text-sm">
            Fermer
          </Button>
          <div className="flex gap-2">
            <Button variant="outline" className="h-9 text-sm">
              <MessageSquare className="w-4 h-4 mr-1.5" />
              Contacter support
            </Button>
            {order.status === 'active' && (
              <Button className="h-9 text-sm">
                <Package className="w-4 h-4 mr-1.5" />
                Demander + de leads
              </Button>
            )}
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}
