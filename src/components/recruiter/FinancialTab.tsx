import { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import {
  CreditCard,
  Eye,
  Download,
  FileText,
  ShoppingCart,
  Receipt,
  FileSignature,
  TrendingUp,
  Clock,
  AlertCircle,
  CheckCircle
} from 'lucide-react';

import { mockFinancialData } from '@/data/recruiterMockData';

const getOrderStatusBadge = (status: string) => {
  switch (status) {
    case 'validated':
      return <Badge className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30 text-xs gap-1"><CheckCircle className="w-3 h-3" />Validé</Badge>;
    case 'pending':
      return <Badge className="bg-amber-500/10 text-amber-600 border-amber-500/30 text-xs gap-1"><Clock className="w-3 h-3" />En attente</Badge>;
    case 'rejected':
      return <Badge className="bg-destructive/10 text-destructive border-destructive/30 text-xs gap-1"><AlertCircle className="w-3 h-3" />Rejeté</Badge>;
    default:
      return <Badge variant="outline" className="text-xs">{status}</Badge>;
  }
};

const getInvoiceStatusBadge = (status: string) => {
  switch (status) {
    case 'paid':
      return <Badge className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30 text-xs gap-1"><CheckCircle className="w-3 h-3" />Payée</Badge>;
    case 'pending':
      return <Badge className="bg-amber-500/10 text-amber-600 border-amber-500/30 text-xs gap-1"><Clock className="w-3 h-3" />En attente</Badge>;
    case 'overdue':
      return <Badge className="bg-destructive/10 text-destructive border-destructive/30 text-xs gap-1"><AlertCircle className="w-3 h-3" />En retard</Badge>;
    default:
      return <Badge variant="outline" className="text-xs">{status}</Badge>;
  }
};

const getContractStatusBadge = (status: string) => {
  switch (status) {
    case 'active':
      return <Badge className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30 text-xs gap-1"><CheckCircle className="w-3 h-3" />Actif</Badge>;
    case 'expiring':
      return <Badge className="bg-amber-500/10 text-amber-600 border-amber-500/30 text-xs gap-1"><Clock className="w-3 h-3" />Expire bientôt</Badge>;
    case 'expired':
      return <Badge className="bg-muted text-muted-foreground text-xs">Expiré</Badge>;
    default:
      return <Badge variant="outline" className="text-xs">{status}</Badge>;
  }
};

export function FinancialTab() {
  const [subTab, setSubTab] = useState('commandes');

  return (
    <div className="space-y-4">
      {/* Stats */}
      <div className="grid grid-cols-2 lg:grid-cols-4 gap-3 sm:gap-4">
        <Card className="border shadow-sm">
          <CardContent className="p-4 text-center">
            <div className="w-10 h-10 bg-primary/10 rounded-lg flex items-center justify-center mx-auto mb-2">
              <ShoppingCart className="w-5 h-5 text-primary" />
            </div>
            <p className="text-xl sm:text-2xl font-bold text-primary">{mockFinancialData.totalOrders}</p>
            <p className="text-xs text-muted-foreground">Commandes finalisées</p>
          </CardContent>
        </Card>
        <Card className="border shadow-sm">
          <CardContent className="p-4 text-center">
            <div className="w-10 h-10 bg-emerald-500/10 rounded-lg flex items-center justify-center mx-auto mb-2">
              <TrendingUp className="w-5 h-5 text-emerald-600" />
            </div>
            <p className="text-xl sm:text-2xl font-bold text-emerald-600">{mockFinancialData.totalRevenue}</p>
            <p className="text-xs text-muted-foreground">Chiffre d'affaires</p>
          </CardContent>
        </Card>
        <Card className="border shadow-sm">
          <CardContent className="p-4 text-center">
            <div className="w-10 h-10 bg-primary/10 rounded-lg flex items-center justify-center mx-auto mb-2">
              <FileText className="w-5 h-5 text-primary" />
            </div>
            <p className="text-xl sm:text-2xl font-bold text-primary">{mockFinancialData.activeOrders}</p>
            <p className="text-xs text-muted-foreground">Bons de commande actifs</p>
          </CardContent>
        </Card>
        <Card className="border shadow-sm">
          <CardContent className="p-4 text-center">
            <div className="w-10 h-10 bg-amber-500/10 rounded-lg flex items-center justify-center mx-auto mb-2">
              <Receipt className="w-5 h-5 text-amber-600" />
            </div>
            <p className="text-xl sm:text-2xl font-bold text-amber-600">{mockFinancialData.pendingInvoices}</p>
            <p className="text-xs text-muted-foreground">Factures en attente</p>
          </CardContent>
        </Card>
      </div>

      {/* Sub-tabs: Bons de Commande / Factures / Contrats */}
      <Card className="border shadow-sm">
        <CardHeader className="px-4 sm:px-6 py-3 sm:py-4">
          <CardTitle className="text-base sm:text-lg flex items-center gap-2">
            <CreditCard className="w-5 h-5 text-primary" />
            Documents Financiers
          </CardTitle>
        </CardHeader>
        <CardContent className="px-4 sm:px-6 pb-4 sm:pb-6">
          <Tabs value={subTab} onValueChange={setSubTab}>
            <TabsList className="grid grid-cols-3 h-auto gap-1 bg-muted/50 p-1 mb-4">
              <TabsTrigger value="commandes" className="data-[state=active]:bg-primary data-[state=active]:text-primary-foreground text-xs sm:text-sm py-2">
                <ShoppingCart className="w-3.5 h-3.5 sm:mr-1.5" />
                <span className="hidden sm:inline">Bons de Commande</span>
                <span className="sm:hidden">Commandes</span>
              </TabsTrigger>
              <TabsTrigger value="factures" className="data-[state=active]:bg-primary data-[state=active]:text-primary-foreground text-xs sm:text-sm py-2">
                <Receipt className="w-3.5 h-3.5 sm:mr-1.5" />
                <span className="hidden sm:inline">Factures</span>
                <span className="sm:hidden">Factures</span>
              </TabsTrigger>
              <TabsTrigger value="contrats" className="data-[state=active]:bg-primary data-[state=active]:text-primary-foreground text-xs sm:text-sm py-2">
                <FileSignature className="w-3.5 h-3.5 sm:mr-1.5" />
                <span className="hidden sm:inline">Contrats</span>
                <span className="sm:hidden">Contrats</span>
              </TabsTrigger>
            </TabsList>

            {/* Bons de Commande */}
            <TabsContent value="commandes">
              <div className="overflow-x-auto">
                <table className="w-full text-sm">
                  <thead>
                    <tr className="border-b bg-muted/30">
                      <th className="text-left p-3 font-medium">Référence</th>
                      <th className="text-left p-3 font-medium hidden sm:table-cell">Date</th>
                      <th className="text-left p-3 font-medium">Client</th>
                      <th className="text-left p-3 font-medium">Montant</th>
                      <th className="text-left p-3 font-medium">Statut</th>
                      <th className="text-left p-3 font-medium hidden sm:table-cell">Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {mockFinancialData.orders.map((order, index) => (
                      <tr key={index} className="border-b hover:bg-muted/30">
                        <td className="p-3 font-medium text-primary">{order.ref}</td>
                        <td className="p-3 hidden sm:table-cell text-muted-foreground">{order.date}</td>
                        <td className="p-3 truncate max-w-[100px] sm:max-w-none">{order.client}</td>
                        <td className="p-3 font-medium">{order.amount}</td>
                        <td className="p-3">{getOrderStatusBadge(order.status)}</td>
                        <td className="p-3 hidden sm:table-cell">
                          <div className="flex gap-1.5">
                            <Button variant="outline" size="sm" className="h-8">
                              <Eye className="w-3 h-3 mr-1" />
                              Voir
                            </Button>
                            <Button variant="outline" size="sm" className="h-8">
                              <Download className="w-3 h-3 mr-1" />
                              PDF
                            </Button>
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </TabsContent>

            {/* Factures */}
            <TabsContent value="factures">
              <div className="overflow-x-auto">
                <table className="w-full text-sm">
                  <thead>
                    <tr className="border-b bg-muted/30">
                      <th className="text-left p-3 font-medium">Numéro</th>
                      <th className="text-left p-3 font-medium hidden sm:table-cell">Date</th>
                      <th className="text-left p-3 font-medium">Client</th>
                      <th className="text-left p-3 font-medium">Montant</th>
                      <th className="text-left p-3 font-medium">Statut</th>
                      <th className="text-left p-3 font-medium hidden sm:table-cell">Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {mockFinancialData.invoices.map((invoice, index) => (
                      <tr key={index} className="border-b hover:bg-muted/30">
                        <td className="p-3 font-medium text-primary">{invoice.ref}</td>
                        <td className="p-3 hidden sm:table-cell text-muted-foreground">{invoice.date}</td>
                        <td className="p-3 truncate max-w-[100px] sm:max-w-none">{invoice.client}</td>
                        <td className="p-3 font-medium">{invoice.amount}</td>
                        <td className="p-3">{getInvoiceStatusBadge(invoice.status)}</td>
                        <td className="p-3 hidden sm:table-cell">
                          <div className="flex gap-1.5">
                            <Button variant="outline" size="sm" className="h-8">
                              <Eye className="w-3 h-3 mr-1" />
                              Voir
                            </Button>
                            <Button variant="outline" size="sm" className="h-8">
                              <Download className="w-3 h-3 mr-1" />
                              PDF
                            </Button>
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </TabsContent>

            {/* Contrats */}
            <TabsContent value="contrats">
              <div className="overflow-x-auto">
                <table className="w-full text-sm">
                  <thead>
                    <tr className="border-b bg-muted/30">
                      <th className="text-left p-3 font-medium">Référence</th>
                      <th className="text-left p-3 font-medium hidden sm:table-cell">Date début</th>
                      <th className="text-left p-3 font-medium hidden sm:table-cell">Date fin</th>
                      <th className="text-left p-3 font-medium">Client</th>
                      <th className="text-left p-3 font-medium">Valeur</th>
                      <th className="text-left p-3 font-medium">Statut</th>
                      <th className="text-left p-3 font-medium hidden sm:table-cell">Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {mockFinancialData.contracts.map((contract, index) => (
                      <tr key={index} className="border-b hover:bg-muted/30">
                        <td className="p-3 font-medium text-primary">{contract.ref}</td>
                        <td className="p-3 hidden sm:table-cell text-muted-foreground">{contract.dateDebut}</td>
                        <td className="p-3 hidden sm:table-cell text-muted-foreground">{contract.dateFin}</td>
                        <td className="p-3 truncate max-w-[100px] sm:max-w-none">{contract.client}</td>
                        <td className="p-3 font-medium">{contract.value}</td>
                        <td className="p-3">{getContractStatusBadge(contract.status)}</td>
                        <td className="p-3 hidden sm:table-cell">
                          <div className="flex gap-1.5">
                            <Button variant="outline" size="sm" className="h-8">
                              <Eye className="w-3 h-3 mr-1" />
                              Voir
                            </Button>
                            <Button variant="outline" size="sm" className="h-8">
                              <Download className="w-3 h-3 mr-1" />
                              PDF
                            </Button>
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </TabsContent>
          </Tabs>
        </CardContent>
      </Card>
    </div>
  );
}
