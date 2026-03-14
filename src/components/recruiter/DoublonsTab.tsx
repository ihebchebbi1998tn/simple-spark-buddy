import { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Progress } from '@/components/ui/progress';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Switch } from '@/components/ui/switch';
import {
  Upload,
  FileSpreadsheet,
  Settings,
  History,
  CheckCircle,
  AlertTriangle,
  XCircle,
  RefreshCw,
  Copy,
  Eye,
  Webhook,
  Key,
  Shield,
  Clock,
  Search,
  Download
} from 'lucide-react';
import { useToast } from '@/hooks/use-toast';

import { mockVerificationHistory } from '@/data/recruiterMockData';

export function DoublonsTab() {
  const { toast } = useToast();
  const [subTab, setSubTab] = useState('import');
  const [emailColumn, setEmailColumn] = useState('');
  const [phoneColumn, setPhoneColumn] = useState('');
  const [nameColumn, setNameColumn] = useState('');
  const [isImporting, setIsImporting] = useState(false);
  const [importProgress, setImportProgress] = useState(0);
  const [apiEnabled, setApiEnabled] = useState(false);
  const [webhookEnabled, setWebhookEnabled] = useState(false);

  const handleStartImport = () => {
    setIsImporting(true);
    setImportProgress(0);
    const interval = setInterval(() => {
      setImportProgress(prev => {
        if (prev >= 100) {
          clearInterval(interval);
          setIsImporting(false);
          toast({
            title: 'Import terminé',
            description: '1 250 lignes analysées, 87 doublons détectés.',
          });
          return 100;
        }
        return prev + 5;
      });
    }, 150);
  };

  const getImportStepLabel = () => {
    if (importProgress < 25) return 'Analyse du fichier...';
    if (importProgress < 50) return 'Détection des colonnes...';
    if (importProgress < 75) return 'Croisement avec la base de données...';
    if (importProgress < 100) return 'Finalisation de l\'import...';
    return 'Import terminé !';
  };

  return (
    <div className="space-y-4">
      {/* Stats rapides */}
      <div className="grid grid-cols-2 lg:grid-cols-4 gap-3 sm:gap-4">
        <Card className="border shadow-sm">
          <CardContent className="p-4 text-center">
            <div className="w-10 h-10 bg-primary/10 rounded-lg flex items-center justify-center mx-auto mb-2">
              <Search className="w-5 h-5 text-primary" />
            </div>
            <p className="text-xl font-bold text-primary">4</p>
            <p className="text-xs text-muted-foreground">Vérifications ce mois</p>
          </CardContent>
        </Card>
        <Card className="border shadow-sm">
          <CardContent className="p-4 text-center">
            <div className="w-10 h-10 bg-amber-500/10 rounded-lg flex items-center justify-center mx-auto mb-2">
              <Copy className="w-5 h-5 text-amber-600" />
            </div>
            <p className="text-xl font-bold text-amber-600">152</p>
            <p className="text-xs text-muted-foreground">Doublons détectés</p>
          </CardContent>
        </Card>
        <Card className="border shadow-sm">
          <CardContent className="p-4 text-center">
            <div className="w-10 h-10 bg-emerald-500/10 rounded-lg flex items-center justify-center mx-auto mb-2">
              <CheckCircle className="w-5 h-5 text-emerald-600" />
            </div>
            <p className="text-xl font-bold text-emerald-600">2 690</p>
            <p className="text-xs text-muted-foreground">Lignes analysées</p>
          </CardContent>
        </Card>
        <Card className="border shadow-sm">
          <CardContent className="p-4 text-center">
            <div className="w-10 h-10 bg-violet-500/10 rounded-lg flex items-center justify-center mx-auto mb-2">
              <Shield className="w-5 h-5 text-violet-600" />
            </div>
            <p className="text-xl font-bold text-violet-600">94%</p>
            <p className="text-xs text-muted-foreground">Taux de précision</p>
          </CardContent>
        </Card>
      </div>

      <Card className="border shadow-sm">
        <CardHeader className="px-4 sm:px-6 py-3 sm:py-4">
          <CardTitle className="text-base sm:text-lg flex items-center gap-2">
            <RefreshCw className="w-5 h-5 text-primary" />
            Gestion des Doublons
          </CardTitle>
          <p className="text-sm text-muted-foreground mt-1">
            Configurez la détection des doublons avec votre base de données existante
          </p>
        </CardHeader>
        <CardContent className="px-4 sm:px-6 pb-4 sm:pb-6">
          <Tabs value={subTab} onValueChange={setSubTab}>
            <TabsList className="grid grid-cols-3 h-auto gap-1 bg-muted/50 p-1 mb-4">
              <TabsTrigger value="import" className="data-[state=active]:bg-primary data-[state=active]:text-primary-foreground text-xs sm:text-sm py-2">
                <Upload className="w-3.5 h-3.5 sm:mr-1.5" />
                <span className="hidden sm:inline">Import Fichier</span>
                <span className="sm:hidden">Import</span>
              </TabsTrigger>
              <TabsTrigger value="api" className="data-[state=active]:bg-primary data-[state=active]:text-primary-foreground text-xs sm:text-sm py-2">
                <Settings className="w-3.5 h-3.5 sm:mr-1.5" />
                <span className="hidden sm:inline">Configuration API</span>
                <span className="sm:hidden">API</span>
              </TabsTrigger>
              <TabsTrigger value="historique" className="data-[state=active]:bg-primary data-[state=active]:text-primary-foreground text-xs sm:text-sm py-2">
                <History className="w-3.5 h-3.5 sm:mr-1.5" />
                <span className="hidden sm:inline">Historique</span>
                <span className="sm:hidden">Historique</span>
              </TabsTrigger>
            </TabsList>

            {/* Import de Fichier */}
            <TabsContent value="import" className="space-y-4">
              <Card className="border">
                <CardContent className="p-4 sm:p-6">
                  <h4 className="font-semibold mb-1 flex items-center gap-2">
                    <FileSpreadsheet className="w-4 h-4 text-primary" />
                    Import de Base de Données
                  </h4>
                  <p className="text-sm text-muted-foreground mb-4">
                    Téléchargez votre fichier Excel ou CSV pour synchroniser avec notre système
                  </p>

                  {/* Upload zone */}
                  <div className="border-2 border-dashed border-border rounded-lg p-6 sm:p-8 text-center hover:border-primary/50 transition-colors cursor-pointer mb-4">
                    <Upload className="w-10 h-10 text-muted-foreground mx-auto mb-3" />
                    <p className="text-base font-medium mb-1">Glissez-déposez votre fichier ici</p>
                    <p className="text-sm text-muted-foreground mb-1">Formats acceptés : .xlsx, .xls, .csv</p>
                    <p className="text-xs text-muted-foreground mb-4">Taille maximale : 10 Mo</p>
                    <Button variant="outline">
                      Parcourir les fichiers
                    </Button>
                  </div>

                  {/* Import progress simulation */}
                  {isImporting && (
                    <div className="space-y-3 p-4 bg-muted/30 rounded-lg mb-4">
                      <div className="flex items-center justify-between">
                        <span className="text-sm font-medium">{getImportStepLabel()}</span>
                        <span className="text-sm text-muted-foreground">{importProgress}%</span>
                      </div>
                      <Progress value={importProgress} className="h-2" />
                    </div>
                  )}

                  {/* Column mapping */}
                  <div className="grid sm:grid-cols-3 gap-4 mb-4">
                    <div className="space-y-2">
                      <Label className="text-sm">Colonne des emails</Label>
                      <Select value={emailColumn} onValueChange={setEmailColumn}>
                        <SelectTrigger>
                          <SelectValue placeholder="Sélectionnez" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="email">email</SelectItem>
                          <SelectItem value="Email">Email</SelectItem>
                          <SelectItem value="EMAIL">EMAIL</SelectItem>
                          <SelectItem value="e_mail">e_mail</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>
                    <div className="space-y-2">
                      <Label className="text-sm">Colonne des téléphones</Label>
                      <Select value={phoneColumn} onValueChange={setPhoneColumn}>
                        <SelectTrigger>
                          <SelectValue placeholder="Sélectionnez" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="telephone">telephone</SelectItem>
                          <SelectItem value="phone">phone</SelectItem>
                          <SelectItem value="tel">tel</SelectItem>
                          <SelectItem value="mobile">mobile</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>
                    <div className="space-y-2">
                      <Label className="text-sm">Colonne nom complet</Label>
                      <Select value={nameColumn} onValueChange={setNameColumn}>
                        <SelectTrigger>
                          <SelectValue placeholder="Sélectionnez" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="nom">nom</SelectItem>
                          <SelectItem value="name">name</SelectItem>
                          <SelectItem value="full_name">full_name</SelectItem>
                          <SelectItem value="nom_complet">nom_complet</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>
                  </div>

                  <div className="flex gap-2">
                    <Button onClick={handleStartImport} disabled={isImporting}>
                      <RefreshCw className={`w-4 h-4 mr-2 ${isImporting ? 'animate-spin' : ''}`} />
                      {isImporting ? 'Analyse en cours...' : 'Lancer la vérification'}
                    </Button>
                  </div>
                </CardContent>
              </Card>

              {/* Résultats si import terminé */}
              {importProgress === 100 && (
                <Card className="border border-emerald-500/30 bg-emerald-500/5">
                  <CardContent className="p-4 sm:p-6">
                    <h4 className="font-semibold mb-3 flex items-center gap-2 text-emerald-600">
                      <CheckCircle className="w-5 h-5" />
                      Résultats de l'analyse
                    </h4>
                    <div className="grid grid-cols-3 gap-4 text-center">
                      <div className="p-3 bg-background rounded-lg">
                        <p className="text-lg font-bold text-primary">1 250</p>
                        <p className="text-xs text-muted-foreground">Lignes analysées</p>
                      </div>
                      <div className="p-3 bg-background rounded-lg">
                        <p className="text-lg font-bold text-amber-600">87</p>
                        <p className="text-xs text-muted-foreground">Doublons détectés</p>
                      </div>
                      <div className="p-3 bg-background rounded-lg">
                        <p className="text-lg font-bold text-emerald-600">1 163</p>
                        <p className="text-xs text-muted-foreground">Profils uniques</p>
                      </div>
                    </div>
                    <div className="flex gap-2 mt-4">
                      <Button variant="outline" size="sm">
                        <Download className="w-4 h-4 mr-2" />
                        Télécharger le rapport
                      </Button>
                      <Button variant="outline" size="sm">
                        <Eye className="w-4 h-4 mr-2" />
                        Voir les doublons
                      </Button>
                    </div>
                  </CardContent>
                </Card>
              )}
            </TabsContent>

            {/* Configuration API */}
            <TabsContent value="api" className="space-y-4">
              <Card className="border">
                <CardContent className="p-4 sm:p-6 space-y-5">
                  <div>
                    <h4 className="font-semibold mb-1 flex items-center gap-2">
                      <Webhook className="w-4 h-4 text-primary" />
                      Configuration API & Webhooks
                    </h4>
                    <p className="text-sm text-muted-foreground">
                      Connectez votre CRM ou ATS pour une vérification automatique des doublons en temps réel
                    </p>
                  </div>

                  {/* API Key */}
                  <div className="p-4 bg-muted/30 rounded-lg space-y-3">
                    <div className="flex items-center justify-between">
                      <Label className="text-sm font-semibold flex items-center gap-2">
                        <Key className="w-4 h-4 text-primary" />
                        Clé API
                      </Label>
                      <div className="flex items-center gap-2">
                        <Label htmlFor="api-toggle" className="text-xs text-muted-foreground">Activer l'API</Label>
                        <Switch id="api-toggle" checked={apiEnabled} onCheckedChange={setApiEnabled} />
                      </div>
                    </div>
                    <div className="flex gap-2">
                      <Input
                        value="sk-ccm-prod-xxxxxxxxxxxxxxxxxxxxxxxxxxxx"
                        readOnly
                        className="font-mono text-xs bg-background"
                      />
                      <Button variant="outline" size="sm" onClick={() => {
                        navigator.clipboard.writeText('sk-ccm-prod-xxxxxxxxxxxxxxxxxxxxxxxxxxxx');
                        toast({ title: 'Copié !', description: 'Clé API copiée dans le presse-papier.' });
                      }}>
                        <Copy className="w-4 h-4" />
                      </Button>
                    </div>
                  </div>

                  {/* Webhook URL */}
                  <div className="p-4 bg-muted/30 rounded-lg space-y-3">
                    <div className="flex items-center justify-between">
                      <Label className="text-sm font-semibold flex items-center gap-2">
                        <Webhook className="w-4 h-4 text-primary" />
                        URL Webhook
                      </Label>
                      <div className="flex items-center gap-2">
                        <Label htmlFor="webhook-toggle" className="text-xs text-muted-foreground">Activer le webhook</Label>
                        <Switch id="webhook-toggle" checked={webhookEnabled} onCheckedChange={setWebhookEnabled} />
                      </div>
                    </div>
                    <Input
                      placeholder="https://votre-crm.com/api/webhooks/doublons"
                      className="bg-background"
                    />
                    <p className="text-xs text-muted-foreground">
                      Notre système enverra un POST à cette URL chaque fois qu'un doublon est détecté lors de la livraison de leads.
                    </p>
                  </div>

                  {/* Endpoint Documentation */}
                  <div className="p-4 bg-primary/5 border border-primary/20 rounded-lg">
                    <h5 className="font-medium text-sm mb-2 text-primary">📖 Documentation de l'endpoint</h5>
                    <div className="space-y-2 text-xs font-mono bg-background p-3 rounded-lg">
                      <p className="text-muted-foreground">POST /api/v1/doublons/check</p>
                      <p className="text-muted-foreground">Content-Type: application/json</p>
                      <p className="text-muted-foreground">Authorization: Bearer {'<votre_cle_api>'}</p>
                      <pre className="mt-2 text-foreground whitespace-pre-wrap">{`{
  "email": "candidat@email.com",
  "telephone": "+216 XX XXX XXX",
  "nom": "Nom Complet"
}`}</pre>
                    </div>
                  </div>
                </CardContent>
              </Card>
            </TabsContent>

            {/* Historique */}
            <TabsContent value="historique">
              <div className="overflow-x-auto">
                <table className="w-full text-sm">
                  <thead>
                    <tr className="border-b bg-muted/30">
                      <th className="text-left p-3 font-medium">ID</th>
                      <th className="text-left p-3 font-medium hidden sm:table-cell">Date</th>
                      <th className="text-left p-3 font-medium">Type</th>
                      <th className="text-left p-3 font-medium hidden md:table-cell">Fichier</th>
                      <th className="text-left p-3 font-medium">Lignes</th>
                      <th className="text-left p-3 font-medium">Doublons</th>
                      <th className="text-left p-3 font-medium">Statut</th>
                      <th className="text-left p-3 font-medium hidden sm:table-cell">Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {mockVerificationHistory.map((item) => (
                      <tr key={item.id} className="border-b hover:bg-muted/30">
                        <td className="p-3 font-medium text-primary text-xs">{item.id}</td>
                        <td className="p-3 hidden sm:table-cell text-muted-foreground">{item.date}</td>
                        <td className="p-3">
                          <Badge variant="outline" className="text-xs">
                            {item.type === 'Import fichier' ? (
                              <><FileSpreadsheet className="w-3 h-3 mr-1" />{item.type}</>
                            ) : (
                              <><Webhook className="w-3 h-3 mr-1" />{item.type}</>
                            )}
                          </Badge>
                        </td>
                        <td className="p-3 hidden md:table-cell text-muted-foreground text-xs truncate max-w-[150px]">
                          {item.fichier}
                        </td>
                        <td className="p-3 font-medium">{item.totalLignes.toLocaleString()}</td>
                        <td className="p-3">
                          <Badge className="bg-amber-500/10 text-amber-600 border-amber-500/30 text-xs">
                            {item.doublonsDetectes}
                          </Badge>
                        </td>
                        <td className="p-3">
                          {item.status === 'completed' ? (
                            <Badge className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30 text-xs gap-1">
                              <CheckCircle className="w-3 h-3" />
                              Terminé
                            </Badge>
                          ) : (
                            <Badge className="bg-destructive/10 text-destructive border-destructive/30 text-xs gap-1">
                              <XCircle className="w-3 h-3" />
                              Erreur
                            </Badge>
                          )}
                        </td>
                        <td className="p-3 hidden sm:table-cell">
                          <div className="flex gap-1.5">
                            <Button variant="outline" size="sm" className="h-8 px-2">
                              <Eye className="w-3 h-3" />
                            </Button>
                            <Button variant="outline" size="sm" className="h-8 px-2">
                              <Download className="w-3 h-3" />
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
