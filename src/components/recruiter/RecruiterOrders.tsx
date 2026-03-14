import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Input } from '@/components/ui/input';
import { 
  Search, 
  Download, 
  Eye, 
  Clock, 
  CheckCircle, 
  AlertCircle,
  Calendar,
  Users,
  Package,
  TrendingUp,
  Plus,
  Settings,
  Archive,
  FileText,
} from 'lucide-react';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '@/components/ui/alert-dialog';
import { LeadDistributionConfig } from './LeadDistributionConfig';
import { recruiterAuthService } from '@/services/recruiterAuthService';
import { getOrdersByRole } from '@/data/recruiterMockData';
import { useToast } from '@/hooks/use-toast';
import type { Order } from '@/types/recruiter';

interface RecruiterOrdersProps {
  onNavigateToOffers?: () => void;
}

export function RecruiterOrders({ onNavigateToOffers }: RecruiterOrdersProps) {
  const navigate = useNavigate();
  const { toast } = useToast();
  const user = recruiterAuthService.getUser();
  const role = user?.role ?? 'charge';
  const userName = user?.name ?? '';

  const allOrders = getOrdersByRole(role, userName);
  const [archivedIds, setArchivedIds] = useState<Set<string>>(new Set());
  const orders = allOrders.filter(o => !archivedIds.has(o.id));

  const [searchTerm, setSearchTerm] = useState('');
  const [statusFilter, setStatusFilter] = useState<string>('all');
  const [configOrderId, setConfigOrderId] = useState<string | null>(null);
  const [archiveOrderId, setArchiveOrderId] = useState<string | null>(null);

  const stats = {
    totalOrders: orders.length,
    activeOrders: orders.filter(o => o.status === 'active').length,
    totalCandidates: orders.reduce((sum, o) => sum + o.candidates, 0),
    totalIntegrated: orders.reduce((sum, o) => sum + o.integrated, 0),
    totalRevenue: orders.reduce((sum, o) => sum + parseInt(o.amount.replace(/\s/g, '').replace('TND', '')), 0),
    avgConversion: orders.length > 0 ? Math.round(orders.reduce((sum, o) => sum + (o.delivered > 0 ? (o.integrated / o.delivered) * 100 : 0), 0) / orders.length) : 0,
  };

  const filteredOrders = orders.filter(order => {
    const matchesSearch = order.id.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          order.type.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          order.city.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          (order.assignedTo?.toLowerCase().includes(searchTerm.toLowerCase()) ?? false);
    const matchesStatus = statusFilter === 'all' || order.status === statusFilter;
    return matchesSearch && matchesStatus;
  });

  const configOrder = orders.find(o => o.id === configOrderId);

  const getStatusBadge = (status: string) => {
    switch (status) {
      case 'active':
        return (
          <Badge className="bg-primary/10 text-primary border-primary/20 gap-1.5 text-xs">
            <Clock className="w-3 h-3" />
            En cours
          </Badge>
        );
      case 'completed':
        return (
          <Badge className="bg-emerald-500/10 text-emerald-600 border-emerald-500/20 gap-1.5 text-xs">
            <CheckCircle className="w-3 h-3" />
            Terminée
          </Badge>
        );
      case 'pending':
        return (
          <Badge className="bg-amber-500/10 text-amber-600 border-amber-500/20 gap-1.5 text-xs">
            <AlertCircle className="w-3 h-3" />
            En attente
          </Badge>
        );
      default:
        return <Badge variant="outline">{status}</Badge>;
    }
  };

  const getProgressColor = (delivered: number, total: number) => {
    const progress = (delivered / total) * 100;
    if (progress === 100) return 'bg-emerald-500';
    if (progress >= 50) return 'bg-primary';
    return 'bg-amber-500';
  };

  const handleViewOrder = (order: Order) => {
    navigate(`/recruteurs/commandes/${order.id}`);
  };

  const archiveTarget = orders.find(o => o.id === archiveOrderId);
  const isDeleteAction = archiveTarget?.status === 'active';

  const handleArchive = () => {
    if (!archiveOrderId) return;
    setArchivedIds(prev => new Set(prev).add(archiveOrderId));
    toast({
      title: isDeleteAction ? 'Commande supprimée' : 'Commande archivée',
      description: `La commande ${archiveOrderId} a été ${isDeleteAction ? 'supprimée' : 'archivée'}.`,
    });
    setArchiveOrderId(null);
  };

  return (
    <div className="space-y-5 sm:space-y-6">
      {/* Page Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-xl sm:text-2xl font-semibold text-foreground mb-1">
            Mes commandes
          </h1>
          <p className="text-sm text-muted-foreground">
            Suivez l'état de vos commandes et l'avancement des livraisons
          </p>
        </div>
        <Button onClick={onNavigateToOffers}>
          <Plus className="w-4 h-4 mr-2" />
          Nouvelle commande
        </Button>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-2 lg:grid-cols-4 xl:grid-cols-6 gap-3 sm:gap-4">
        <Card className="border shadow-sm">
          <CardContent className="p-4">
            <div className="flex items-center gap-3">
              <div className="w-9 h-9 sm:w-10 sm:h-10 bg-primary/10 rounded-lg flex items-center justify-center shrink-0">
                <Package className="w-4 h-4 sm:w-5 sm:h-5 text-primary" />
              </div>
              <div className="min-w-0">
                <p className="text-xl sm:text-2xl font-bold">{stats.totalOrders}</p>
                <p className="text-xs text-muted-foreground truncate">Commandes</p>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card className="border shadow-sm">
          <CardContent className="p-4">
            <div className="flex items-center gap-3">
              <div className="w-9 h-9 sm:w-10 sm:h-10 bg-primary/10 rounded-lg flex items-center justify-center shrink-0">
                <Clock className="w-4 h-4 sm:w-5 sm:h-5 text-primary" />
              </div>
              <div className="min-w-0">
                <p className="text-xl sm:text-2xl font-bold">{stats.activeOrders}</p>
                <p className="text-xs text-muted-foreground truncate">En cours</p>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card className="border shadow-sm">
          <CardContent className="p-4">
            <div className="flex items-center gap-3">
              <div className="w-9 h-9 sm:w-10 sm:h-10 bg-primary/10 rounded-lg flex items-center justify-center shrink-0">
                <Users className="w-4 h-4 sm:w-5 sm:h-5 text-primary" />
              </div>
              <div className="min-w-0">
                <p className="text-xl sm:text-2xl font-bold">{stats.totalCandidates}</p>
                <p className="text-xs text-muted-foreground truncate">Candidats</p>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card className="border shadow-sm">
          <CardContent className="p-4">
            <div className="flex items-center gap-3">
              <div className="w-9 h-9 sm:w-10 sm:h-10 bg-primary/10 rounded-lg flex items-center justify-center shrink-0">
                <CheckCircle className="w-4 h-4 sm:w-5 sm:h-5 text-primary" />
              </div>
              <div className="min-w-0">
                <p className="text-xl sm:text-2xl font-bold">{stats.totalIntegrated}</p>
                <p className="text-xs text-muted-foreground truncate">Intégrés</p>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card className="border shadow-sm hidden xl:block">
          <CardContent className="p-4">
            <div className="flex items-center gap-3">
              <div className="w-9 h-9 sm:w-10 sm:h-10 bg-primary/10 rounded-lg flex items-center justify-center shrink-0">
                <TrendingUp className="w-4 h-4 sm:w-5 sm:h-5 text-primary" />
              </div>
              <div className="min-w-0">
                <p className="text-xl sm:text-2xl font-bold">{stats.avgConversion}%</p>
                <p className="text-xs text-muted-foreground truncate">Conversion</p>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card className="border shadow-sm hidden xl:block">
          <CardContent className="p-4">
            <div className="flex items-center gap-3">
              <div className="w-9 h-9 sm:w-10 sm:h-10 bg-primary/10 rounded-lg flex items-center justify-center shrink-0">
                <span className="text-sm font-bold text-primary">TND</span>
              </div>
              <div className="min-w-0">
                <p className="text-lg sm:text-xl font-bold">{stats.totalRevenue.toLocaleString()}</p>
                <p className="text-xs text-muted-foreground truncate">Investissement</p>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Filters */}
      <Card className="border shadow-sm">
        <CardContent className="p-3 sm:p-4">
          <div className="flex flex-col gap-3">
            <div className="relative">
              <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-muted-foreground" />
              <Input
                placeholder="Rechercher par ID, type, ville..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="pl-9"
              />
            </div>
            <div className="flex flex-wrap gap-2">
              {[
                { key: 'all', label: 'Toutes' },
                { key: 'active', label: 'En cours' },
                { key: 'completed', label: 'Terminées' },
                { key: 'pending', label: 'En attente' },
              ].map((filter) => (
                <Button
                  key={filter.key}
                  variant={statusFilter === filter.key ? 'default' : 'outline'}
                  size="sm"
                  onClick={() => setStatusFilter(filter.key)}
                  className="flex-1 min-w-[70px] sm:flex-none text-xs sm:text-sm"
                >
                  {filter.label}
                </Button>
              ))}
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Orders Table */}
      <Card className="border shadow-sm overflow-hidden">
        <CardHeader className="bg-muted/30 border-b px-4 sm:px-6 py-3 sm:py-4">
          <div className="flex items-center justify-between">
            <CardTitle className="text-base sm:text-lg">Liste des commandes</CardTitle>
            <Button variant="outline" size="sm" className="h-8">
              <Download className="w-4 h-4 sm:mr-2" />
              <span className="hidden sm:inline">Exporter</span>
            </Button>
          </div>
        </CardHeader>
        <CardContent className="p-0">
          <div className="overflow-x-auto">
            <Table>
              <TableHeader>
                <TableRow className="bg-muted/30">
                  <TableHead className="font-medium text-xs sm:text-sm">Référence</TableHead>
                  <TableHead className="font-medium text-xs sm:text-sm hidden sm:table-cell">Date</TableHead>
                  <TableHead className="font-medium text-xs sm:text-sm">Type</TableHead>
                  <TableHead className="font-medium text-xs sm:text-sm hidden md:table-cell">Ville</TableHead>
                  <TableHead className="font-medium text-xs sm:text-sm hidden lg:table-cell">Progression</TableHead>
                  <TableHead className="font-medium text-xs sm:text-sm">Statut</TableHead>
                  <TableHead className="font-medium text-xs sm:text-sm hidden sm:table-cell">Montant</TableHead>
                  <TableHead className="font-medium text-xs sm:text-sm text-right">Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredOrders.map((order) => (
                  <TableRow key={order.id} className="hover:bg-muted/30">
                    <TableCell className="font-medium text-sm">{order.id}</TableCell>
                    <TableCell className="text-muted-foreground text-sm hidden sm:table-cell">
                      <div className="flex items-center gap-1.5">
                        <Calendar className="w-3.5 h-3.5" />
                        {order.date}
                      </div>
                    </TableCell>
                    <TableCell className="text-sm">{order.type}</TableCell>
                    <TableCell className="text-sm hidden md:table-cell">{order.city}</TableCell>
                    <TableCell className="hidden lg:table-cell">
                      <div className="space-y-1">
                        <div className="flex items-center gap-2 text-xs">
                          <span className="text-muted-foreground">
                            {order.delivered}/{order.candidates}
                          </span>
                          <span className="text-emerald-600 font-medium">
                            ({order.integrated} intégrés)
                          </span>
                        </div>
                        <div className="w-full h-1.5 bg-muted rounded-full overflow-hidden">
                          <div
                            className={`h-full rounded-full ${getProgressColor(order.delivered, order.candidates)}`}
                            style={{ width: `${(order.delivered / order.candidates) * 100}%` }}
                          />
                        </div>
                      </div>
                    </TableCell>
                    <TableCell>{getStatusBadge(order.status)}</TableCell>
                    <TableCell className="font-medium text-sm hidden sm:table-cell">{order.amount}</TableCell>
                    <TableCell className="text-right">
                      <div className="flex items-center gap-1 justify-end">
                        <Button 
                          variant="ghost" 
                          size="sm" 
                          className="h-7 px-2 text-xs"
                          onClick={() => handleViewOrder(order)}
                        >
                          <Eye className="w-3.5 h-3.5 sm:mr-1" />
                          <span className="hidden sm:inline">Détails</span>
                        </Button>
                        {order.status === 'completed' && (
                          <Button
                            variant="ghost"
                            size="sm"
                            className="h-7 px-2 text-xs"
                            onClick={() => setConfigOrderId(order.id)}
                          >
                            <Settings className="w-3.5 h-3.5 sm:mr-1" />
                            <span className="hidden sm:inline">Configurer</span>
                          </Button>
                        )}
                        {order.status === 'completed' && (
                          <Button
                            variant="ghost"
                            size="sm"
                            className="h-7 px-2 text-xs text-muted-foreground hover:text-destructive"
                            onClick={() => setArchiveOrderId(order.id)}
                          >
                            <Archive className="w-3.5 h-3.5 sm:mr-1" />
                            <span className="hidden sm:inline">Archiver</span>
                          </Button>
                        )}
                        {order.status === 'active' && (
                          <Button
                            variant="ghost"
                            size="sm"
                            className="h-7 px-2 text-xs"
                            onClick={() => handleViewOrder(order)}
                            title="Bon de commande"
                          >
                            <FileText className="w-3.5 h-3.5 sm:mr-1" />
                            <span className="hidden sm:inline">Bon de commande</span>
                          </Button>
                        )}
                        {order.status === 'active' && (
                          <Button
                            variant="ghost"
                            size="icon"
                            className="h-7 w-7 text-muted-foreground hover:text-destructive"
                            onClick={() => setArchiveOrderId(order.id)}
                            title="Supprimer"
                          >
                            <Archive className="w-3.5 h-3.5" />
                          </Button>
                        )}
                      </div>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>

          {filteredOrders.length === 0 && (
            <div className="p-8 text-center">
              <Package className="w-10 h-10 text-muted-foreground mx-auto mb-3" />
              <h3 className="text-base font-medium text-foreground mb-1">Aucune commande trouvée</h3>
              <p className="text-sm text-muted-foreground">
                Modifiez vos critères de recherche ou créez une nouvelle commande.
              </p>
            </div>
          )}
        </CardContent>
      </Card>

      {/* Lead Distribution Config Modal */}
      <Dialog open={!!configOrderId} onOpenChange={(open) => !open && setConfigOrderId(null)}>
        <DialogContent className="max-w-2xl max-h-[85vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle className="flex items-center gap-2">
              <Settings className="w-5 h-5 text-primary" />
              Configuration de la distribution des leads
            </DialogTitle>
            {configOrder && (
              <p className="text-sm text-muted-foreground">
                {configOrder.id} — {configOrder.type} · {configOrder.candidates} candidats
              </p>
            )}
          </DialogHeader>
          {configOrder && (
            <LeadDistributionConfig
              totalLeads={configOrder.candidates * 5}
              orderId={configOrder.id}
              userRole={role}
            />
          )}
        </DialogContent>
      </Dialog>

      {/* Archive Confirmation */}
      <AlertDialog open={!!archiveOrderId} onOpenChange={(open) => !open && setArchiveOrderId(null)}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>{isDeleteAction ? 'Supprimer cette commande ?' : 'Archiver cette commande ?'}</AlertDialogTitle>
            <AlertDialogDescription>
              La commande <strong>{archiveOrderId}</strong> sera {isDeleteAction ? 'définitivement supprimée' : 'déplacée dans les archives'}. 
              {isDeleteAction 
                ? ' Cette action est irréversible.'
                : ' Vous pourrez la retrouver ultérieurement dans la section archivée.'}
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Annuler</AlertDialogCancel>
            <AlertDialogAction onClick={handleArchive} className={isDeleteAction ? 'bg-destructive hover:bg-destructive/90' : ''}>
              {isDeleteAction ? 'Supprimer' : 'Archiver'}
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  );
}
