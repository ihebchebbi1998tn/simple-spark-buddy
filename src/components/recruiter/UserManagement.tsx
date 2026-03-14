import { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Badge } from '@/components/ui/badge';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Switch } from '@/components/ui/switch';
import { Checkbox } from '@/components/ui/checkbox';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import {
  Table, TableBody, TableCell, TableHead, TableHeader, TableRow
} from '@/components/ui/table';
import {
  Users,
  UserPlus,
  Edit3,
  Trash2,
  Shield,
  Crown,
  Briefcase,
  Mail,
  Phone,
  Languages,
  Eye,
  EyeOff,
  Save,
  X,
  LayoutDashboard,
  Search,
  ShoppingCart,
  Package,
  Building2,
  BarChart3,
  Lock,
  Settings2,
  KeyRound,
  MoreHorizontal,
  UserCheck,
  UserX,
  Filter,
} from 'lucide-react';
import { useToast } from '@/hooks/use-toast';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import type { RecruiterPermissions, RecruiterRole, ManagedUser, UserPermissionsExtended } from '@/types/recruiter';
import { DEFAULT_PERMISSIONS, PERMISSION_LABELS } from '@/types/recruiter';
import { mockUsers, getDefaultExtendedPermissions, searchOptions } from '@/data/recruiterMockData';

const PERMISSION_ICONS: Record<keyof RecruiterPermissions, React.ElementType> = {
  overview: LayoutDashboard,
  search: Search,
  orders: ShoppingCart,
  offers: Package,
  profile: Building2,
  users: Users,
  performance: BarChart3,
  campaigns: BarChart3,
};

const allLanguages = searchOptions.allLanguages;

export function UserManagement() {
  const { toast } = useToast();
  const [users, setUsers] = useState<ManagedUser[]>(mockUsers);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingUser, setEditingUser] = useState<ManagedUser | null>(null);
  const [showPassword, setShowPassword] = useState(false);
  const [filterRole, setFilterRole] = useState<'all' | RecruiterRole>('all');
  const [searchQuery, setSearchQuery] = useState('');

  const [formData, setFormData] = useState({
    prenom: '',
    nom: '',
    email: '',
    telephone: '',
    role: 'charge' as RecruiterRole,
    langues: [] as string[],
    password: '',
    permissions: getDefaultExtendedPermissions('charge'),
  });

  const resetForm = () => {
    setFormData({
      prenom: '', nom: '', email: '', telephone: '',
      role: 'charge', langues: [], password: '',
      permissions: getDefaultExtendedPermissions('charge'),
    });
    setEditingUser(null);
  };

  const openCreateModal = () => { resetForm(); setIsModalOpen(true); };

  const openEditModal = (user: ManagedUser) => {
    setEditingUser(user);
    setFormData({
      prenom: user.prenom, nom: user.nom, email: user.email,
      telephone: user.telephone, role: user.role, langues: user.langues,
      password: '',
      permissions: { ...user.permissions, dashboard: { ...user.permissions.dashboard } },
    });
    setIsModalOpen(true);
  };

  const handleRoleChange = (role: RecruiterRole) => {
    setFormData(prev => ({ ...prev, role, permissions: getDefaultExtendedPermissions(role) }));
  };

  const toggleDashboardPermission = (key: keyof RecruiterPermissions) => {
    if (formData.role === 'super_admin') return;
    setFormData(prev => ({
      ...prev,
      permissions: { ...prev.permissions, dashboard: { ...prev.permissions.dashboard, [key]: !prev.permissions.dashboard[key] } },
    }));
  };

  const handleSubmit = () => {
    if (!formData.prenom || !formData.nom || !formData.email) {
      toast({ title: 'Erreur', description: 'Veuillez remplir tous les champs obligatoires.', variant: 'destructive' });
      return;
    }
    if (editingUser) {
      setUsers(users.map(u => u.id === editingUser.id ? { ...u, ...formData, actif: u.actif } : u));
      toast({ title: 'Utilisateur modifié', description: `${formData.prenom} ${formData.nom} a été mis à jour.` });
    } else {
      const newUser: ManagedUser = {
        id: Date.now().toString(), ...formData, actif: true,
        createdAt: new Date().toLocaleDateString('fr-FR'),
      };
      setUsers([...users, newUser]);
      toast({ title: 'Utilisateur créé', description: `${formData.prenom} ${formData.nom} a été ajouté à l'équipe.` });
    }
    setIsModalOpen(false);
    resetForm();
  };

  const toggleUserStatus = (userId: string) => {
    setUsers(users.map(u => u.id === userId ? { ...u, actif: !u.actif } : u));
    const user = users.find(u => u.id === userId);
    toast({
      title: user?.actif ? 'Utilisateur désactivé' : 'Utilisateur activé',
      description: `${user?.prenom} ${user?.nom} a été ${user?.actif ? 'désactivé' : 'activé'}.`,
    });
  };

  const deleteUser = (userId: string) => {
    const user = users.find(u => u.id === userId);
    if (user?.role === 'super_admin') {
      toast({ title: 'Action impossible', description: 'Impossible de supprimer un Super Admin.', variant: 'destructive' });
      return;
    }
    setUsers(users.filter(u => u.id !== userId));
    toast({ title: 'Utilisateur supprimé', description: `${user?.prenom} ${user?.nom} a été supprimé.` });
  };

  const handleLanguageToggle = (language: string) => {
    setFormData(prev => ({
      ...prev,
      langues: prev.langues.includes(language) ? prev.langues.filter(l => l !== language) : [...prev.langues, language],
    }));
  };

  const getRoleBadge = (role: RecruiterRole) => {
    switch (role) {
      case 'super_admin':
        return <Badge className="bg-accent/10 text-accent border-accent/30 text-[10px] font-semibold"><Crown className="w-3 h-3 mr-1" />Super Admin</Badge>;
      case 'admin':
        return <Badge className="bg-primary/10 text-primary border-primary/30 text-[10px] font-semibold"><Shield className="w-3 h-3 mr-1" />Admin</Badge>;
      case 'charge':
        return <Badge className="bg-muted text-muted-foreground border-border text-[10px] font-semibold"><Briefcase className="w-3 h-3 mr-1" />Chargé</Badge>;
    }
  };

  const stats = {
    total: users.length,
    superAdmins: users.filter(u => u.role === 'super_admin').length,
    admins: users.filter(u => u.role === 'admin').length,
    charges: users.filter(u => u.role === 'charge').length,
    actifs: users.filter(u => u.actif).length,
  };

  const filteredUsers = users
    .filter(u => filterRole === 'all' || u.role === filterRole)
    .filter(u => {
      if (!searchQuery) return true;
      const q = searchQuery.toLowerCase();
      return `${u.prenom} ${u.nom}`.toLowerCase().includes(q)
        || u.email.toLowerCase().includes(q)
        || u.telephone.includes(q);
    });

  const enabledCount = (perms: RecruiterPermissions) =>
    (Object.values(perms) as boolean[]).filter(Boolean).length;

  return (
    <div className="space-y-5 sm:space-y-6">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-xl sm:text-2xl font-bold text-foreground flex items-center gap-2.5">
            <div className="w-9 h-9 rounded-lg bg-primary/10 flex items-center justify-center">
              <Users className="w-5 h-5 text-primary" />
            </div>
            Gestion des Utilisateurs
          </h1>
          <p className="text-sm text-muted-foreground mt-1 ml-[46px]">
            Gérez votre équipe et attribuez les droits d'accès
          </p>
        </div>
        <Button onClick={openCreateModal} className="shrink-0 shadow-sm">
          <UserPlus className="w-4 h-4 mr-2" />
          Nouvel Utilisateur
        </Button>
      </div>

      {/* Stats row */}
      <div className="grid grid-cols-2 sm:grid-cols-5 gap-3">
        {[
          { label: 'Total', value: stats.total, active: filterRole === 'all', onClick: () => setFilterRole('all'), icon: Users },
          { label: 'Super Admins', value: stats.superAdmins, active: filterRole === 'super_admin', onClick: () => setFilterRole('super_admin'), icon: Crown },
          { label: 'Admins', value: stats.admins, active: filterRole === 'admin', onClick: () => setFilterRole('admin'), icon: Shield },
          { label: 'Chargés', value: stats.charges, active: filterRole === 'charge', onClick: () => setFilterRole('charge'), icon: Briefcase },
          { label: 'Actifs', value: stats.actifs, active: false, onClick: () => {}, icon: UserCheck },
        ].map((s) => (
          <Card
            key={s.label}
            className={`border shadow-sm cursor-pointer transition-all hover:shadow-md ${s.active ? 'ring-2 ring-primary border-primary/30' : ''}`}
            onClick={s.onClick}
          >
            <CardContent className="p-3 sm:p-4">
              <div className="flex items-center gap-3">
                <div className={`w-9 h-9 rounded-lg flex items-center justify-center shrink-0 ${s.active ? 'bg-primary text-primary-foreground' : 'bg-primary/10 text-primary'}`}>
                  <s.icon className="w-4 h-4" />
                </div>
                <div>
                  <p className="text-lg sm:text-xl font-bold tabular-nums">{s.value}</p>
                  <p className="text-[10px] sm:text-xs text-muted-foreground leading-tight">{s.label}</p>
                </div>
              </div>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* Table */}
      <Card className="border shadow-sm">
        <CardHeader className="px-4 sm:px-6 py-3 sm:py-4 border-b">
          <div className="flex flex-col sm:flex-row sm:items-center gap-3 sm:justify-between">
            <CardTitle className="text-base font-semibold flex items-center gap-2">
              Équipe {filterRole !== 'all' ? `— ${filterRole === 'super_admin' ? 'Super Admins' : filterRole === 'admin' ? 'Administrateurs' : 'Chargés'}` : ''}
              <Badge variant="secondary" className="text-xs">{filteredUsers.length}</Badge>
            </CardTitle>
            <div className="flex items-center gap-2">
              <div className="relative">
                <Search className="absolute left-2.5 top-1/2 -translate-y-1/2 w-3.5 h-3.5 text-muted-foreground" />
                <Input
                  placeholder="Rechercher un membre..."
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  className="pl-8 h-9 w-full sm:w-56 text-sm"
                />
              </div>
              {filterRole !== 'all' && (
                <Button variant="ghost" size="sm" onClick={() => setFilterRole('all')} className="text-xs shrink-0">
                  <X className="w-3 h-3 mr-1" /> Réinitialiser
                </Button>
              )}
            </div>
          </div>
        </CardHeader>
        <CardContent className="p-0">
          {/* Desktop table */}
          <div className="hidden md:block">
            <Table>
              <TableHeader>
                <TableRow className="bg-muted/30 hover:bg-muted/30">
                  <TableHead className="text-xs font-semibold w-[250px]">Utilisateur</TableHead>
                  <TableHead className="text-xs font-semibold">Rôle</TableHead>
                  <TableHead className="text-xs font-semibold">Contact</TableHead>
                  <TableHead className="text-xs font-semibold text-center">Droits</TableHead>
                  <TableHead className="text-xs font-semibold text-center">Statut</TableHead>
                  <TableHead className="text-xs font-semibold text-right w-[80px]">Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {filteredUsers.map((user) => {
                  const isSuperAdmin = user.role === 'super_admin';
                  return (
                    <TableRow key={user.id} className={`group ${!user.actif ? 'opacity-50' : ''}`}>
                      <TableCell>
                        <div className="flex items-center gap-3">
                          <div className={`w-9 h-9 rounded-full flex items-center justify-center text-xs font-bold text-primary-foreground shrink-0 ${
                            user.role === 'super_admin' ? 'bg-accent' : user.role === 'admin' ? 'bg-primary' : 'bg-primary/60'
                          }`}>
                            {user.prenom.charAt(0)}{user.nom.charAt(0)}
                          </div>
                          <div>
                            <p className="text-sm font-semibold text-foreground">{user.prenom} {user.nom}</p>
                            <p className="text-xs text-muted-foreground">Ajouté le {user.createdAt}</p>
                          </div>
                        </div>
                      </TableCell>
                      <TableCell>{getRoleBadge(user.role)}</TableCell>
                      <TableCell>
                        <div className="space-y-0.5">
                          <p className="text-xs text-muted-foreground flex items-center gap-1.5"><Mail className="w-3 h-3" />{user.email}</p>
                          <p className="text-xs text-muted-foreground flex items-center gap-1.5"><Phone className="w-3 h-3" />{user.telephone}</p>
                        </div>
                      </TableCell>
                      <TableCell className="text-center">
                        <Badge variant="outline" className="text-[10px] px-1.5 py-0 gap-1">
                          <KeyRound className="w-2.5 h-2.5" />
                          {isSuperAdmin ? 'Tous' : `${enabledCount(user.permissions.dashboard)}/7`}
                        </Badge>
                      </TableCell>
                      <TableCell className="text-center">
                        <Badge className={`text-[10px] px-2 py-0.5 border ${
                          user.actif
                            ? 'bg-primary/10 text-primary border-primary/30'
                            : 'bg-muted text-muted-foreground border-border'
                        }`}>
                          {user.actif ? 'Actif' : 'Inactif'}
                        </Badge>
                      </TableCell>
                      <TableCell className="text-right">
                        <DropdownMenu>
                          <DropdownMenuTrigger asChild>
                            <Button variant="ghost" size="sm" className="h-8 w-8 p-0 opacity-60 group-hover:opacity-100 transition-opacity">
                              <MoreHorizontal className="w-4 h-4" />
                            </Button>
                          </DropdownMenuTrigger>
                          <DropdownMenuContent align="end" className="w-44">
                            <DropdownMenuItem onClick={() => openEditModal(user)}>
                              <Edit3 className="w-3.5 h-3.5 mr-2" /> Modifier
                            </DropdownMenuItem>
                            {!isSuperAdmin && (
                              <>
                                <DropdownMenuItem onClick={() => toggleUserStatus(user.id)}>
                                  {user.actif ? <UserX className="w-3.5 h-3.5 mr-2" /> : <UserCheck className="w-3.5 h-3.5 mr-2" />}
                                  {user.actif ? 'Désactiver' : 'Activer'}
                                </DropdownMenuItem>
                                <DropdownMenuSeparator />
                                <DropdownMenuItem onClick={() => deleteUser(user.id)} className="text-destructive focus:text-destructive">
                                  <Trash2 className="w-3.5 h-3.5 mr-2" /> Supprimer
                                </DropdownMenuItem>
                              </>
                            )}
                          </DropdownMenuContent>
                        </DropdownMenu>
                      </TableCell>
                    </TableRow>
                  );
                })}
                {filteredUsers.length === 0 && (
                  <TableRow>
                    <TableCell colSpan={6} className="text-center py-10 text-muted-foreground text-sm">
                      Aucun utilisateur trouvé
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </div>

          {/* Mobile cards */}
          <div className="md:hidden divide-y">
            {filteredUsers.map((user) => {
              const isSuperAdmin = user.role === 'super_admin';
              return (
                <div key={user.id} className={`p-4 ${!user.actif ? 'opacity-50' : ''}`}>
                  <div className="flex items-start justify-between gap-3">
                    <div className="flex items-center gap-3">
                      <div className={`w-10 h-10 rounded-full flex items-center justify-center text-xs font-bold text-primary-foreground shrink-0 ${
                        user.role === 'super_admin' ? 'bg-accent' : user.role === 'admin' ? 'bg-primary' : 'bg-primary/60'
                      }`}>
                        {user.prenom.charAt(0)}{user.nom.charAt(0)}
                      </div>
                      <div>
                        <p className="text-sm font-semibold">{user.prenom} {user.nom}</p>
                        <div className="flex items-center gap-2 mt-0.5">
                          {getRoleBadge(user.role)}
                          <Badge className={`text-[10px] px-1.5 py-0 border ${
                            user.actif ? 'bg-primary/10 text-primary border-primary/30' : 'bg-muted text-muted-foreground border-border'
                          }`}>
                            {user.actif ? 'Actif' : 'Inactif'}
                          </Badge>
                        </div>
                      </div>
                    </div>
                    <DropdownMenu>
                      <DropdownMenuTrigger asChild>
                        <Button variant="ghost" size="sm" className="h-8 w-8 p-0">
                          <MoreHorizontal className="w-4 h-4" />
                        </Button>
                      </DropdownMenuTrigger>
                      <DropdownMenuContent align="end" className="w-44">
                        <DropdownMenuItem onClick={() => openEditModal(user)}>
                          <Edit3 className="w-3.5 h-3.5 mr-2" /> Modifier
                        </DropdownMenuItem>
                        {!isSuperAdmin && (
                          <>
                            <DropdownMenuItem onClick={() => toggleUserStatus(user.id)}>
                              {user.actif ? <UserX className="w-3.5 h-3.5 mr-2" /> : <UserCheck className="w-3.5 h-3.5 mr-2" />}
                              {user.actif ? 'Désactiver' : 'Activer'}
                            </DropdownMenuItem>
                            <DropdownMenuSeparator />
                            <DropdownMenuItem onClick={() => deleteUser(user.id)} className="text-destructive focus:text-destructive">
                              <Trash2 className="w-3.5 h-3.5 mr-2" /> Supprimer
                            </DropdownMenuItem>
                          </>
                        )}
                      </DropdownMenuContent>
                    </DropdownMenu>
                  </div>
                  <div className="mt-2.5 ml-[52px] space-y-1 text-xs text-muted-foreground">
                    <p className="flex items-center gap-1.5"><Mail className="w-3 h-3" />{user.email}</p>
                    <p className="flex items-center gap-1.5"><Phone className="w-3 h-3" />{user.telephone}</p>
                  </div>
                </div>
              );
            })}
            {filteredUsers.length === 0 && (
              <div className="p-8 text-center text-muted-foreground text-sm">Aucun utilisateur trouvé</div>
            )}
          </div>
        </CardContent>
      </Card>

      {/* Create/Edit Modal */}
      <Dialog open={isModalOpen} onOpenChange={setIsModalOpen}>
        <DialogContent className="sm:max-w-2xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle className="text-lg font-bold flex items-center gap-2">
              <div className="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center">
                {editingUser ? <Edit3 className="w-4 h-4 text-primary" /> : <UserPlus className="w-4 h-4 text-primary" />}
              </div>
              {editingUser ? 'Modifier l\'utilisateur' : 'Créer un nouvel utilisateur'}
            </DialogTitle>
          </DialogHeader>

          <Tabs defaultValue="info" className="w-full">
            <TabsList className="grid w-full grid-cols-3">
              <TabsTrigger value="info">Informations</TabsTrigger>
              <TabsTrigger value="permissions">Droits d'accès</TabsTrigger>
              <TabsTrigger value="settings">Paramètres</TabsTrigger>
            </TabsList>

            <TabsContent value="info" className="space-y-4 mt-4">
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label>Prénom *</Label>
                  <Input value={formData.prenom} onChange={(e) => setFormData({ ...formData, prenom: e.target.value })} placeholder="Ex: Wael" />
                </div>
                <div className="space-y-2">
                  <Label>Nom *</Label>
                  <Input value={formData.nom} onChange={(e) => setFormData({ ...formData, nom: e.target.value })} placeholder="Ex: Dupont" />
                </div>
              </div>
              <div className="space-y-2">
                <Label>Email *</Label>
                <Input type="email" value={formData.email} onChange={(e) => setFormData({ ...formData, email: e.target.value })} placeholder="Ex: wael.dupont@entreprise.com" />
              </div>
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label>Téléphone</Label>
                  <Input value={formData.telephone} onChange={(e) => setFormData({ ...formData, telephone: e.target.value })} placeholder="Ex: +212 661 123 456" />
                </div>
                <div className="space-y-2">
                  <Label>Rôle *</Label>
                  <Select value={formData.role} onValueChange={(v) => handleRoleChange(v as RecruiterRole)}>
                    <SelectTrigger><SelectValue /></SelectTrigger>
                    <SelectContent>
                      <SelectItem value="charge">Chargé de Recrutement</SelectItem>
                      <SelectItem value="admin">Administrateur</SelectItem>
                      <SelectItem value="super_admin">Super Administrateur</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>
              {!editingUser && (
                <div className="space-y-2">
                  <Label>Mot de passe *</Label>
                  <div className="relative">
                    <Input type={showPassword ? 'text' : 'password'} value={formData.password} onChange={(e) => setFormData({ ...formData, password: e.target.value })} placeholder="Mot de passe" className="pr-10" />
                    <button type="button" onClick={() => setShowPassword(!showPassword)} className="absolute right-3 top-1/2 -translate-y-1/2 text-muted-foreground">
                      {showPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                    </button>
                  </div>
                </div>
              )}
              <div className="space-y-2">
                <Label>Langues maîtrisées</Label>
                <div className="grid grid-cols-2 gap-2 p-3 border rounded-lg bg-muted/20 max-h-32 overflow-y-auto">
                  {allLanguages.map((lang) => (
                    <label key={lang} className="flex items-center gap-2 cursor-pointer hover:bg-muted/50 p-1 rounded">
                      <Checkbox checked={formData.langues.includes(lang)} onCheckedChange={() => handleLanguageToggle(lang)} />
                      <span className="text-sm">{lang}</span>
                    </label>
                  ))}
                </div>
              </div>
            </TabsContent>

            <TabsContent value="permissions" className="space-y-4 mt-4">
              {formData.role === 'super_admin' && (
                <div className="flex items-center gap-2 p-3 bg-accent/10 border border-accent/30 rounded-lg text-sm">
                  <Lock className="w-4 h-4 text-accent shrink-0" />
                  <span className="text-accent font-medium">Le Super Admin a automatiquement tous les droits.</span>
                </div>
              )}
              <div className="space-y-3">
                <Label className="font-semibold flex items-center gap-2">
                  <KeyRound className="w-4 h-4 text-primary" /> Sections du tableau de bord
                </Label>
                <div className="space-y-2">
                  {(Object.keys(PERMISSION_LABELS) as (keyof RecruiterPermissions)[]).map((key) => {
                    const Icon = PERMISSION_ICONS[key];
                    const isLocked = formData.role === 'super_admin';
                    const enabled = formData.permissions.dashboard[key];
                    return (
                      <div key={key} className={`flex items-center justify-between p-3 rounded-lg border transition-colors ${enabled ? 'bg-primary/5 border-primary/20' : 'bg-muted/20 border-border'}`}>
                        <div className="flex items-center gap-3">
                          <div className={`w-8 h-8 rounded-lg flex items-center justify-center ${enabled ? 'bg-primary/10 text-primary' : 'bg-muted text-muted-foreground'}`}>
                            <Icon className="w-4 h-4" />
                          </div>
                          <div>
                            <p className={`text-sm font-medium ${enabled ? 'text-foreground' : 'text-muted-foreground'}`}>{PERMISSION_LABELS[key]}</p>
                            <p className="text-[10px] sm:text-xs text-muted-foreground">
                              {key === 'overview' && 'Accès au tableau de bord principal'}
                              {key === 'search' && 'Rechercher et consulter les candidats'}
                              {key === 'orders' && 'Créer et gérer les commandes'}
                              {key === 'offers' && 'Voir les offres et packs disponibles'}
                              {key === 'profile' && 'Modifier le profil du centre'}
                              {key === 'users' && 'Gérer les utilisateurs et leurs droits'}
                              {key === 'performance' && 'Consulter les tableaux de performance'}
                            </p>
                          </div>
                        </div>
                        {isLocked ? <Lock className="w-4 h-4 text-accent shrink-0" /> : (
                          <Switch checked={enabled} onCheckedChange={() => toggleDashboardPermission(key)} />
                        )}
                      </div>
                    );
                  })}
                </div>
              </div>
              <div className="space-y-3 p-4 bg-muted/30 rounded-lg border">
                <Label className="font-semibold">Accès aux reportings</Label>
                <Select
                  value={formData.permissions.accesReportings}
                  onValueChange={(v: 'oui' | 'non' | 'lecture') => setFormData({ ...formData, permissions: { ...formData.permissions, accesReportings: v } })}
                  disabled={formData.role === 'super_admin'}
                >
                  <SelectTrigger className="w-full"><SelectValue /></SelectTrigger>
                  <SelectContent>
                    <SelectItem value="oui">Accès complet</SelectItem>
                    <SelectItem value="lecture">Lecture seule</SelectItem>
                    <SelectItem value="non">Aucun accès</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </TabsContent>

            <TabsContent value="settings" className="space-y-4 mt-4">
              <div className="space-y-3 p-4 bg-muted/30 rounded-lg border">
                <Label className="font-semibold">Paramètres de distribution</Label>
                <div className="flex items-center justify-between">
                  <span className="text-sm">Participer à la distribution des leads</span>
                  <Switch checked={formData.permissions.participerDistribution} onCheckedChange={(v) => setFormData({ ...formData, permissions: { ...formData.permissions, participerDistribution: v } })} />
                </div>
                <div className="flex items-center justify-between">
                  <span className="text-sm">Limiter aux langues maîtrisées</span>
                  <Switch checked={formData.permissions.limiterLangues} onCheckedChange={(v) => setFormData({ ...formData, permissions: { ...formData.permissions, limiterLangues: v } })} />
                </div>
              </div>
            </TabsContent>
          </Tabs>

          <DialogFooter className="gap-2 mt-4">
            <Button variant="outline" onClick={() => setIsModalOpen(false)}>
              <X className="w-4 h-4 mr-1" /> Annuler
            </Button>
            <Button onClick={handleSubmit}>
              <Save className="w-4 h-4 mr-1" />
              {editingUser ? 'Mettre à jour' : 'Créer l\'utilisateur'}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}
