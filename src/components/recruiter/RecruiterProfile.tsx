import { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { Badge } from '@/components/ui/badge';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from '@/components/ui/dialog';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import {
  Building2,
  MapPin,
  Phone,
  Mail,
  Globe,
  Edit3,
  Save,
  X,
  Users,
  FileText,
  CreditCard,
  Download,
  Upload,
  Settings,
  Eye,
  TrendingUp,
  RefreshCw,
  Plus,
  Trash2,
  UserPlus,
} from 'lucide-react';
import { FinancialTab } from './FinancialTab';
import { DoublonsTab } from './DoublonsTab';
import { useToast } from '@/hooks/use-toast';
import type { RecruiterUser, BillingContact } from '@/types/recruiter';
import { mockCompanyProfile, mockBillingContacts } from '@/data/recruiterMockData';

interface RecruiterProfileProps {
  user: RecruiterUser;
}

const emptyContact: Omit<BillingContact, 'id'> = {
  civilite: 'M',
  nom: '',
  prenom: '',
  fonction: '',
  email: '',
  telephone: '',
  type: 'secondaire',
};

export function RecruiterProfile({ user }: RecruiterProfileProps) {
  const { toast } = useToast();
  const [isEditing, setIsEditing] = useState(false);
  const [activeTab, setActiveTab] = useState('general');
  const [companyData, setCompanyData] = useState(mockCompanyProfile);

  // Billing contacts state
  const [billingContacts, setBillingContacts] = useState<BillingContact[]>(mockBillingContacts);
  const [contactModalOpen, setContactModalOpen] = useState(false);
  const [editingContact, setEditingContact] = useState<BillingContact | null>(null);
  const [contactForm, setContactForm] = useState<Omit<BillingContact, 'id'>>(emptyContact);
  const [deleteConfirmOpen, setDeleteConfirmOpen] = useState(false);
  const [contactToDelete, setContactToDelete] = useState<string | null>(null);

  const handleSave = () => {
    toast({ title: 'Profil mis à jour', description: 'Les informations ont été sauvegardées.' });
    setIsEditing(false);
  };

  const handleCancel = () => {
    setCompanyData(mockCompanyProfile);
    setIsEditing(false);
  };

  // Contact CRUD
  const openAddContact = () => {
    setEditingContact(null);
    setContactForm({ ...emptyContact });
    setContactModalOpen(true);
  };

  const openEditContact = (contact: BillingContact) => {
    setEditingContact(contact);
    setContactForm({
      civilite: contact.civilite,
      nom: contact.nom,
      prenom: contact.prenom,
      fonction: contact.fonction,
      email: contact.email,
      telephone: contact.telephone,
      type: contact.type,
    });
    setContactModalOpen(true);
  };

  const handleSaveContact = () => {
    if (!contactForm.nom || !contactForm.prenom || !contactForm.email) {
      toast({ title: 'Erreur', description: 'Veuillez remplir les champs obligatoires.', variant: 'destructive' });
      return;
    }

    if (editingContact) {
      setBillingContacts((prev) =>
        prev.map((c) => (c.id === editingContact.id ? { ...c, ...contactForm } : c))
      );
      toast({ title: 'Contact modifié', description: `${contactForm.prenom} ${contactForm.nom} a été mis à jour.` });
    } else {
      const newContact: BillingContact = {
        ...contactForm,
        id: Date.now().toString(),
      };
      setBillingContacts((prev) => [...prev, newContact]);
      toast({ title: 'Contact ajouté', description: `${contactForm.prenom} ${contactForm.nom} a été ajouté.` });
    }
    setContactModalOpen(false);
  };

  const confirmDeleteContact = (id: string) => {
    setContactToDelete(id);
    setDeleteConfirmOpen(true);
  };

  const handleDeleteContact = () => {
    if (contactToDelete) {
      setBillingContacts((prev) => prev.filter((c) => c.id !== contactToDelete));
      toast({ title: 'Contact supprimé', description: 'Le contact a été supprimé.' });
    }
    setDeleteConfirmOpen(false);
    setContactToDelete(null);
  };

  const getContactTypeBadge = (type: string) => {
    switch (type) {
      case 'principal':
        return <Badge className="bg-emerald-500/10 text-emerald-600 border-emerald-500/30 text-xs">Principal</Badge>;
      case 'secondaire':
        return <Badge className="bg-primary/10 text-primary border-primary/30 text-xs">Secondaire</Badge>;
      case 'urgence':
        return <Badge className="bg-amber-500/10 text-amber-600 border-amber-500/30 text-xs">Urgence</Badge>;
      default:
        return <Badge variant="outline" className="text-xs">{type}</Badge>;
    }
  };

  return (
    <div className="space-y-5 sm:space-y-6">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-xl sm:text-2xl font-semibold text-foreground mb-1">Profil du Centre</h1>
          <p className="text-sm text-muted-foreground">
            Gérez les informations complètes de votre centre d'appels
          </p>
        </div>
        {activeTab === 'general' || activeTab === 'complementary' ? (
          isEditing ? (
            <div className="flex gap-2">
              <Button variant="outline" onClick={handleCancel}>
                <X className="w-4 h-4 mr-2" />
                Annuler
              </Button>
              <Button onClick={handleSave}>
                <Save className="w-4 h-4 mr-2" />
                Sauvegarder
              </Button>
            </div>
          ) : (
            <Button onClick={() => setIsEditing(true)}>
              <Edit3 className="w-4 h-4 mr-2" />
              Modifier
            </Button>
          )
        ) : null}
      </div>

      {/* Company Header Card */}
      <Card className="border shadow-sm overflow-hidden">
        <div className="bg-primary p-5 sm:p-6 text-primary-foreground">
          <div className="flex items-center gap-4">
            <div className="w-14 h-14 sm:w-16 sm:h-16 bg-primary-foreground/20 rounded-xl flex items-center justify-center shrink-0">
              <Building2 className="w-7 h-7 sm:w-8 sm:h-8" />
            </div>
            <div className="min-w-0">
              <h2 className="text-xl sm:text-2xl font-semibold truncate">{companyData.name}</h2>
              <p className="text-primary-foreground/80 text-sm truncate">{companyData.legalName}</p>
              <div className="flex flex-wrap gap-2 mt-2">
                <Badge className="bg-primary-foreground/20 text-primary-foreground border-primary-foreground/30 text-xs">
                  <Users className="w-3 h-3 mr-1" />
                  {companyData.employeeCount} positions
                </Badge>
                <Badge className="bg-primary-foreground/20 text-primary-foreground border-primary-foreground/30 text-xs">
                  <TrendingUp className="w-3 h-3 mr-1" />
                  {companyData.occupationRate}% occupation
                </Badge>
              </div>
            </div>
          </div>
        </div>
      </Card>

      {/* Tabs */}
      <Tabs value={activeTab} onValueChange={setActiveTab} className="space-y-4">
        <TabsList className="grid grid-cols-3 sm:grid-cols-6 h-auto gap-1 bg-muted/50 p-1">
          <TabsTrigger value="general" className="data-[state=active]:bg-primary data-[state=active]:text-primary-foreground text-xs sm:text-sm py-2 px-2">
            <FileText className="w-4 h-4 sm:mr-1.5" />
            <span className="hidden sm:inline">Général</span>
          </TabsTrigger>
          <TabsTrigger value="complementary" className="data-[state=active]:bg-primary data-[state=active]:text-primary-foreground text-xs sm:text-sm py-2 px-2">
            <Settings className="w-4 h-4 sm:mr-1.5" />
            <span className="hidden sm:inline">Complémentaire</span>
          </TabsTrigger>
          <TabsTrigger value="billing" className="data-[state=active]:bg-primary data-[state=active]:text-primary-foreground text-xs sm:text-sm py-2 px-2">
            <Users className="w-4 h-4 sm:mr-1.5" />
            <span className="hidden sm:inline">Facturation</span>
          </TabsTrigger>
          <TabsTrigger value="financial" className="data-[state=active]:bg-primary data-[state=active]:text-primary-foreground text-xs sm:text-sm py-2 px-2">
            <CreditCard className="w-4 h-4 sm:mr-1.5" />
            <span className="hidden sm:inline">Financier</span>
          </TabsTrigger>
          <TabsTrigger value="doublons" className="data-[state=active]:bg-primary data-[state=active]:text-primary-foreground text-xs sm:text-sm py-2 px-2">
            <RefreshCw className="w-4 h-4 sm:mr-1.5" />
            <span className="hidden sm:inline">Doublons</span>
          </TabsTrigger>
          <TabsTrigger value="documents" className="data-[state=active]:bg-primary data-[state=active]:text-primary-foreground text-xs sm:text-sm py-2 px-2">
            <Download className="w-4 h-4 sm:mr-1.5" />
            <span className="hidden sm:inline">Documents</span>
          </TabsTrigger>
        </TabsList>

        {/* General Tab */}
        <TabsContent value="general" className="space-y-4">
          <Card className="border shadow-sm">
            <CardHeader className="px-4 sm:px-6 py-3 sm:py-4">
              <CardTitle className="text-base sm:text-lg flex items-center gap-2">
                <FileText className="w-5 h-5 text-primary" />
                Informations générales
              </CardTitle>
            </CardHeader>
            <CardContent className="px-4 sm:px-6 pb-4 sm:pb-6 space-y-4">
              <div className="grid sm:grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label className="text-sm">Nom commercial</Label>
                  <Input value={companyData.name} onChange={(e) => setCompanyData({ ...companyData, name: e.target.value })} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Raison sociale</Label>
                  <Input value={companyData.legalName} onChange={(e) => setCompanyData({ ...companyData, legalName: e.target.value })} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Numéro RC</Label>
                  <Input value={companyData.registrationNumber} onChange={(e) => setCompanyData({ ...companyData, registrationNumber: e.target.value })} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Identifiant Fiscal</Label>
                  <Input value={companyData.taxId} onChange={(e) => setCompanyData({ ...companyData, taxId: e.target.value })} disabled={!isEditing} />
                </div>
              </div>
              <div className="space-y-2">
                <Label className="text-sm">Description</Label>
                <Textarea value={companyData.description} onChange={(e) => setCompanyData({ ...companyData, description: e.target.value })} disabled={!isEditing} rows={3} />
              </div>
            </CardContent>
          </Card>

          <Card className="border shadow-sm">
            <CardHeader className="px-4 sm:px-6 py-3 sm:py-4">
              <CardTitle className="text-base sm:text-lg flex items-center gap-2">
                <MapPin className="w-5 h-5 text-primary" />
                Coordonnées
              </CardTitle>
            </CardHeader>
            <CardContent className="px-4 sm:px-6 pb-4 sm:pb-6 space-y-4">
              <div className="space-y-2">
                <Label className="text-sm">Adresse</Label>
                <Input value={companyData.address} onChange={(e) => setCompanyData({ ...companyData, address: e.target.value })} disabled={!isEditing} />
              </div>
              <div className="grid sm:grid-cols-3 gap-4">
                <div className="space-y-2">
                  <Label className="text-sm">Ville</Label>
                  <Input value={companyData.city} onChange={(e) => setCompanyData({ ...companyData, city: e.target.value })} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Code Postal</Label>
                  <Input value={companyData.postalCode} onChange={(e) => setCompanyData({ ...companyData, postalCode: e.target.value })} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Pays</Label>
                  <Input value={companyData.country} onChange={(e) => setCompanyData({ ...companyData, country: e.target.value })} disabled={!isEditing} />
                </div>
              </div>
              <div className="grid sm:grid-cols-3 gap-4">
                <div className="space-y-2">
                  <Label className="flex items-center gap-2 text-sm"><Phone className="w-4 h-4 text-muted-foreground" />Téléphone</Label>
                  <Input value={companyData.phone} onChange={(e) => setCompanyData({ ...companyData, phone: e.target.value })} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="flex items-center gap-2 text-sm"><Mail className="w-4 h-4 text-muted-foreground" />Email</Label>
                  <Input value={companyData.email} onChange={(e) => setCompanyData({ ...companyData, email: e.target.value })} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="flex items-center gap-2 text-sm"><Globe className="w-4 h-4 text-muted-foreground" />Site Web</Label>
                  <Input value={companyData.website} onChange={(e) => setCompanyData({ ...companyData, website: e.target.value })} disabled={!isEditing} />
                </div>
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Complementary Tab */}
        <TabsContent value="complementary" className="space-y-4">
          <Card className="border shadow-sm">
            <CardHeader className="px-4 sm:px-6 py-3 sm:py-4">
              <CardTitle className="text-base sm:text-lg flex items-center gap-2">
                <Settings className="w-5 h-5 text-primary" />
                Informations complémentaires
              </CardTitle>
            </CardHeader>
            <CardContent className="px-4 sm:px-6 pb-4 sm:pb-6">
              <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-4">
                <div className="space-y-2">
                  <Label className="text-sm">Nombre de positions actuelles</Label>
                  <Input type="number" value={companyData.employeeCount} onChange={(e) => setCompanyData({ ...companyData, employeeCount: Number(e.target.value) })} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Capacité maximale</Label>
                  <Input type="number" value={companyData.maxCapacity} onChange={(e) => setCompanyData({ ...companyData, maxCapacity: Number(e.target.value) })} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Taux d'occupation</Label>
                  <Input value={`${companyData.occupationRate}%`} disabled className="bg-muted" />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Besoin annuel en recrutement</Label>
                  <Input type="number" value={companyData.annualRecruitmentNeed} onChange={(e) => setCompanyData({ ...companyData, annualRecruitmentNeed: Number(e.target.value) })} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Turnover annuel</Label>
                  <Input value={`${companyData.turnoverRate}%`} disabled className="bg-muted" />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Heures d'ouverture</Label>
                  <Input value={companyData.operatingHours} onChange={(e) => setCompanyData({ ...companyData, operatingHours: e.target.value })} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Type de centre</Label>
                  <Input value={companyData.centerType} onChange={(e) => setCompanyData({ ...companyData, centerType: e.target.value })} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Langues supportées</Label>
                  <Input value={companyData.languages.join(', ')} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Secteurs d'activité</Label>
                  <Input value={companyData.sectors.join(', ')} disabled={!isEditing} />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Location de positions</Label>
                  <Input value={companyData.locationService ? 'Disponible' : 'Non disponible'} disabled className="bg-muted" />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Recherche de nouvelles opérations</Label>
                  <Input value={companyData.searchingOperations ? 'Actif' : 'Inactif'} disabled className="bg-muted" />
                </div>
                <div className="space-y-2">
                  <Label className="text-sm">Technologies utilisées</Label>
                  <Input value={companyData.technologies.join(', ')} disabled={!isEditing} />
                </div>
              </div>

              <div className="mt-5">
                <Label className="mb-2 block text-sm">Activités proposées</Label>
                <div className="flex flex-wrap gap-2">
                  {companyData.activities.map((activity) => (
                    <Badge key={activity} variant="secondary" className="px-3 py-1">{activity}</Badge>
                  ))}
                </div>
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        {/* Billing Contacts Tab */}
        <TabsContent value="billing" className="space-y-4">
          <Card className="border shadow-sm">
            <CardHeader className="flex flex-row items-center justify-between px-4 sm:px-6 py-3 sm:py-4">
              <CardTitle className="text-base sm:text-lg flex items-center gap-2">
                <Users className="w-5 h-5 text-primary" />
                Contacts de Facturation
              </CardTitle>
              <Button size="sm" onClick={openAddContact}>
                <UserPlus className="w-4 h-4 mr-1.5" />
                <span className="hidden sm:inline">Ajouter</span>
              </Button>
            </CardHeader>
            <CardContent className="px-4 sm:px-6 pb-4 sm:pb-6">
              {billingContacts.length === 0 ? (
                <div className="text-center py-8">
                  <Users className="w-10 h-10 text-muted-foreground mx-auto mb-3" />
                  <p className="text-sm text-muted-foreground mb-3">Aucun contact de facturation</p>
                  <Button variant="outline" onClick={openAddContact}>
                    <Plus className="w-4 h-4 mr-2" />
                    Ajouter un contact
                  </Button>
                </div>
              ) : (
                <div className="space-y-3">
                  {billingContacts.map((contact) => (
                    <div key={contact.id} className="flex items-center justify-between p-3 sm:p-4 bg-muted/30 rounded-lg hover:bg-muted/50 transition-colors gap-3">
                      <div className="flex items-center gap-3 min-w-0 flex-1">
                        <div className="w-9 h-9 rounded-full bg-primary/10 flex items-center justify-center shrink-0">
                          <span className="text-xs font-bold text-primary">
                            {contact.prenom[0]}{contact.nom[0]}
                          </span>
                        </div>
                        <div className="min-w-0">
                          <div className="flex items-center gap-2 flex-wrap">
                            <p className="font-medium text-foreground text-sm">
                              {contact.civilite} {contact.prenom} {contact.nom}
                            </p>
                            {getContactTypeBadge(contact.type)}
                          </div>
                          <p className="text-xs text-muted-foreground truncate">
                            {contact.fonction && `${contact.fonction} • `}{contact.email}
                          </p>
                          <p className="text-xs text-muted-foreground sm:hidden">{contact.telephone}</p>
                        </div>
                      </div>
                      <div className="flex items-center gap-2 shrink-0">
                        <span className="text-xs text-muted-foreground hidden sm:inline">{contact.telephone}</span>
                        <Button variant="outline" size="sm" className="h-8 px-2" onClick={() => openEditContact(contact)}>
                          <Edit3 className="w-3 h-3" />
                        </Button>
                        <Button variant="outline" size="sm" className="h-8 px-2 text-destructive hover:text-destructive hover:bg-destructive/10" onClick={() => confirmDeleteContact(contact.id)}>
                          <Trash2 className="w-3 h-3" />
                        </Button>
                      </div>
                    </div>
                  ))}
                </div>
              )}
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="financial"><FinancialTab /></TabsContent>
        <TabsContent value="doublons"><DoublonsTab /></TabsContent>

        {/* Documents Tab */}
        <TabsContent value="documents" className="space-y-4">
          <Card className="border shadow-sm">
            <CardHeader className="px-4 sm:px-6 py-3 sm:py-4">
              <CardTitle className="text-base sm:text-lg flex items-center gap-2">
                <Upload className="w-5 h-5 text-primary" />
                Documents du Centre
              </CardTitle>
            </CardHeader>
            <CardContent className="px-4 sm:px-6 pb-4 sm:pb-6">
              <div className="border-2 border-dashed border-border rounded-lg p-6 sm:p-8 text-center hover:border-primary/50 transition-colors cursor-pointer">
                <Upload className="w-10 h-10 text-muted-foreground mx-auto mb-3" />
                <p className="text-base font-medium mb-1">Glissez-déposez vos fichiers ici</p>
                <p className="text-sm text-muted-foreground mb-4">ou cliquez pour sélectionner</p>
                <Button variant="outline">Parcourir les fichiers</Button>
              </div>
              <div className="mt-5">
                <h4 className="font-medium mb-3 text-sm">Documents existants</h4>
                <div className="space-y-2">
                  {[
                    { name: 'Registre de commerce.pdf', date: '15/01/2024', size: '2.3 MB' },
                    { name: 'Attestation fiscale.pdf', date: '10/01/2024', size: '1.1 MB' },
                    { name: 'Contrat cadre.pdf', date: '05/01/2024', size: '4.5 MB' },
                  ].map((doc, index) => (
                    <div key={index} className="flex items-center justify-between p-3 bg-muted/30 rounded-lg gap-3">
                      <div className="flex items-center gap-3 min-w-0">
                        <FileText className="w-7 h-7 text-destructive shrink-0" />
                        <div className="min-w-0">
                          <p className="font-medium text-sm truncate">{doc.name}</p>
                          <p className="text-xs text-muted-foreground">{doc.date} • {doc.size}</p>
                        </div>
                      </div>
                      <div className="flex gap-1.5 shrink-0">
                        <Button variant="outline" size="sm" className="h-8 px-2"><Eye className="w-3 h-3" /></Button>
                        <Button variant="outline" size="sm" className="h-8 px-2"><Download className="w-3 h-3" /></Button>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>

      {/* Add/Edit Contact Modal */}
      <Dialog open={contactModalOpen} onOpenChange={setContactModalOpen}>
        <DialogContent className="max-w-md w-[95vw] sm:w-full">
          <DialogHeader>
            <DialogTitle className="text-lg font-semibold">
              {editingContact ? 'Modifier le contact' : 'Nouveau contact de facturation'}
            </DialogTitle>
          </DialogHeader>
          <div className="space-y-4 py-2">
            <div className="grid grid-cols-2 gap-3">
              <div className="space-y-1.5">
                <Label className="text-sm">Civilité</Label>
                <Select value={contactForm.civilite} onValueChange={(v) => setContactForm({ ...contactForm, civilite: v })}>
                  <SelectTrigger><SelectValue /></SelectTrigger>
                  <SelectContent>
                    <SelectItem value="M">Monsieur</SelectItem>
                    <SelectItem value="Mme">Madame</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <div className="space-y-1.5">
                <Label className="text-sm">Type de contact</Label>
                <Select value={contactForm.type} onValueChange={(v) => setContactForm({ ...contactForm, type: v as BillingContact['type'] })}>
                  <SelectTrigger><SelectValue /></SelectTrigger>
                  <SelectContent>
                    <SelectItem value="principal">Principal</SelectItem>
                    <SelectItem value="secondaire">Secondaire</SelectItem>
                    <SelectItem value="urgence">Urgence</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>
            <div className="grid grid-cols-2 gap-3">
              <div className="space-y-1.5">
                <Label className="text-sm">Prénom *</Label>
                <Input placeholder="Ex: Marie" value={contactForm.prenom} onChange={(e) => setContactForm({ ...contactForm, prenom: e.target.value })} />
              </div>
              <div className="space-y-1.5">
                <Label className="text-sm">Nom *</Label>
                <Input placeholder="Ex: Dubois" value={contactForm.nom} onChange={(e) => setContactForm({ ...contactForm, nom: e.target.value })} />
              </div>
            </div>
            <div className="space-y-1.5">
              <Label className="text-sm">Fonction</Label>
              <Input placeholder="Ex: Responsable Financier" value={contactForm.fonction} onChange={(e) => setContactForm({ ...contactForm, fonction: e.target.value })} />
            </div>
            <div className="space-y-1.5">
              <Label className="text-sm">Email *</Label>
              <Input type="email" placeholder="Ex: marie.dubois@entreprise.com" value={contactForm.email} onChange={(e) => setContactForm({ ...contactForm, email: e.target.value })} />
            </div>
            <div className="space-y-1.5">
              <Label className="text-sm">Téléphone</Label>
              <Input type="tel" placeholder="Ex: +216 22 123 456" value={contactForm.telephone} onChange={(e) => setContactForm({ ...contactForm, telephone: e.target.value })} />
            </div>
          </div>
          <DialogFooter className="flex-col-reverse sm:flex-row gap-2">
            <Button variant="outline" onClick={() => setContactModalOpen(false)}>Annuler</Button>
            <Button onClick={handleSaveContact}>
              <Save className="w-4 h-4 mr-2" />
              {editingContact ? 'Enregistrer' : 'Ajouter'}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Delete Confirmation Modal */}
      <Dialog open={deleteConfirmOpen} onOpenChange={setDeleteConfirmOpen}>
        <DialogContent className="max-w-sm w-[95vw]">
          <DialogHeader>
            <DialogTitle className="text-lg font-semibold">Confirmer la suppression</DialogTitle>
          </DialogHeader>
          <p className="text-sm text-muted-foreground py-2">
            Êtes-vous sûr de vouloir supprimer ce contact de facturation ? Cette action est irréversible.
          </p>
          <DialogFooter className="flex-col-reverse sm:flex-row gap-2">
            <Button variant="outline" onClick={() => setDeleteConfirmOpen(false)}>Annuler</Button>
            <Button variant="destructive" onClick={handleDeleteContact}>
              <Trash2 className="w-4 h-4 mr-2" />
              Supprimer
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}
