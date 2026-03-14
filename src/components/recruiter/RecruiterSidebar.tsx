import { NavLink } from 'react-router-dom';
import {
  LayoutDashboard,
  Search,
  ShoppingCart,
  Package,
  Building2,
  Users,
  BarChart3,
  Settings,
  LogOut,
  ChevronRight,
  Home,
  Megaphone,
  UserCog,
  Upload,
} from 'lucide-react';
import {
  Sidebar,
  SidebarContent,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarHeader,
  SidebarFooter,
  useSidebar,
} from '@/components/ui/sidebar';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { useNavigate } from 'react-router-dom';
import { useToast } from '@/hooks/use-toast';
import { recruiterAuthService } from '@/services/recruiterAuthService';
import { ROLE_LABELS, getAllowedSections } from '@/types/recruiter';
import type { RecruiterRole, RecruiterUser } from '@/types/recruiter';
import { useTranslation } from 'react-i18next';

interface RecruiterSidebarProps {
  activeSection: string;
  onSectionChange: (section: string) => void;
  userRole: RecruiterRole;
  user?: RecruiterUser;
}

const allMenuItemKeys = [
  { id: 'overview', labelKey: 'sidebar.overview', icon: LayoutDashboard, tourId: 'recruiter-overview' },
  { id: 'search', labelKey: 'sidebar.search', icon: Search, tourId: 'recruiter-search' },
  { id: 'orders', labelKey: 'sidebar.orders', icon: ShoppingCart, tourId: 'recruiter-orders' },
  { id: 'offers', labelKey: 'sidebar.offers', icon: Package, tourId: 'recruiter-offers' },
  { id: 'profile', labelKey: 'sidebar.profile', icon: Building2, tourId: 'recruiter-profile' },
  { id: 'performance', labelKey: 'sidebar.performance', icon: BarChart3, tourId: 'recruiter-performance' },
  { id: 'campaigns', labelKey: 'sidebar.campaigns', icon: Megaphone, tourId: 'recruiter-campaigns' },
  { id: 'users', labelKey: 'sidebar.users', icon: Users, tourId: 'recruiter-users' },
  { id: 'lead-import', labelKey: 'sidebar.leadImport', icon: Upload, tourId: 'recruiter-import' },
];

const getMenuItems = (user?: RecruiterUser) => {
  if (!user) return allMenuItemKeys.slice(0, 4);
  const allowed = getAllowedSections(user);
  return allMenuItemKeys
    .filter(item => {
      if (item.id === 'lead-import') {
        return user.role === 'super_admin';
      }
      return allowed.includes(item.id);
    })
    .map(item => {
      if (user.role === 'charge' && item.id === 'performance') {
        return { ...item, labelKey: 'sidebar.myDashboard', icon: LayoutDashboard };
      }
      return item;
    });
};

export function RecruiterSidebar({ activeSection, onSectionChange, userRole, user }: RecruiterSidebarProps) {
  const { state } = useSidebar();
  const navigate = useNavigate();
  const { toast } = useToast();
  const collapsed = state === 'collapsed';
  const { t } = useTranslation('recruiterDashboard');

  const menuItems = getMenuItems(user);

  const handleLogout = () => {
    recruiterAuthService.logout();
    toast({
      title: t('sidebar.logoutSuccess'),
      description: t('sidebar.logoutMessage'),
    });
    navigate('/recruteurs/login');
  };

  return (
    <Sidebar className="border-r border-sidebar-border bg-sidebar shadow-none">
      <SidebarHeader className="border-b border-sidebar-border p-4 sm:p-5">
        <div className="flex items-center justify-center">
          <NavLink to="/">
            <img
              src="/uploads/488f38af-4f42-45f8-a54f-4c1b46a2dfff.png"
              alt="CallCenterMatch Logo"
              className="h-10 w-auto sm:h-11 object-contain"
            />
          </NavLink>
        </div>
      </SidebarHeader>

      <SidebarContent className="px-3 py-4">
        <SidebarGroup>
          <SidebarGroupLabel className="text-[10px] font-semibold text-sidebar-foreground/50 uppercase tracking-widest px-3 pb-3 hidden sm:block">
            {t('sidebar.navigation')}
          </SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu className="space-y-0.5">
              {menuItems.map((item) => {
                const Icon = item.icon;
                const isActive = activeSection === item.id;

                return (
                  <SidebarMenuItem key={item.id}>
                    <SidebarMenuButton
                      onClick={() => onSectionChange(item.id)}
                      data-tour={item.tourId}
                      className={`
                        w-full justify-start h-10 px-3 rounded-lg transition-all duration-150
                        ${isActive
                          ? 'bg-primary text-white shadow-sm font-medium'
                          : 'hover:bg-sidebar-accent/60 text-sidebar-foreground/80 hover:text-sidebar-foreground'
                        }
                      `}
                    >
                      <Icon className={`h-4 w-4 mr-3 shrink-0 ${isActive ? '' : 'opacity-70'}`} />
                      <span className="text-[13px] truncate">
                        {t(item.labelKey)}
                      </span>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                );
              })}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>

        {/* Role badge */}
        <div className="mt-6 px-3">
          <Badge
            variant="outline"
            className={`w-full justify-center py-1.5 text-[10px] font-semibold uppercase tracking-wider ${userRole === 'super_admin'
              ? 'bg-accent/10 text-accent border-accent/30'
              : userRole === 'admin'
                ? 'bg-primary/10 text-primary border-primary/30'
                : 'bg-emerald-500/10 text-emerald-600 border-emerald-500/30'
              }`}
          >
            {t(`roles.${userRole}`)}
          </Badge>
        </div>
      </SidebarContent>

      <SidebarFooter className="border-t border-sidebar-border p-3 space-y-0.5">
        <Button
          variant="ghost"
          size="sm"
          onClick={() => onSectionChange('my-account')}
          className={`w-full justify-start h-9 px-3 rounded-lg text-[13px] ${activeSection === 'my-account'
            ? 'bg-primary text-white font-medium'
            : 'text-sidebar-foreground/70 hover:text-sidebar-foreground hover:bg-sidebar-accent/60'
            }`}
        >
          <UserCog className="h-4 w-4 mr-3 shrink-0" />
          {t('sidebar.myAccount')}
        </Button>

        <Button
          variant="ghost"
          size="sm"
          onClick={() => navigate('/')}
          className="w-full justify-start h-9 px-3 text-sidebar-foreground/70 hover:text-sidebar-foreground hover:bg-sidebar-accent/60 rounded-lg text-[13px]"
        >
          <Home className="h-4 w-4 mr-3 shrink-0 opacity-70" />
          {t('sidebar.backToHome')}
        </Button>

        <Button
          variant="ghost"
          size="sm"
          onClick={handleLogout}
          className="w-full justify-start h-9 px-3 text-destructive/80 hover:text-destructive hover:bg-destructive/10 rounded-lg text-[13px]"
        >
          <LogOut className="h-4 w-4 mr-3 shrink-0 opacity-70" />
          {t('sidebar.logout')}
        </Button>
      </SidebarFooter>
    </Sidebar>
  );
}
