import { useState } from 'react';
import { NavLink, useLocation } from 'react-router-dom';
import {
  LayoutDashboard,
  UserCircle,
  Headphones,
  MapPin,
  Briefcase,
  Globe,
  CalendarDays,
  SlidersHorizontal,
  Shield,
  LogOut,
  ChevronRight,
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
import { useNavigate } from 'react-router-dom';
import { toast } from '@/hooks/use-toast';
import { useTranslation } from 'react-i18next';

const menuItemKeys = [
  { id: 'overview', labelKey: 'sidebar.overview', icon: LayoutDashboard, path: '/dashboard', tourId: 'sidebar-overview' },
  { id: 'profile', labelKey: 'sidebar.profile', icon: UserCircle, path: '/dashboard?section=profile', tourId: 'sidebar-profile' },
  { id: 'callcenters', labelKey: 'sidebar.callcenters', icon: Headphones, path: '/dashboard?section=callcenters', tourId: 'sidebar-callcenters' },
  { id: 'proximity', labelKey: 'sidebar.proximity', icon: MapPin, path: '/dashboard?section=proximity', tourId: 'sidebar-proximity' },
  { id: 'activities', labelKey: 'sidebar.activities', icon: Briefcase, path: '/dashboard?section=activities', tourId: 'sidebar-activities' },
  { id: 'languages', labelKey: 'sidebar.languages', icon: Globe, path: '/dashboard?section=languages', tourId: 'sidebar-languages' },
  { id: 'availability', labelKey: 'sidebar.availability', icon: CalendarDays, path: '/dashboard?section=availability', tourId: 'sidebar-availability' },
  { id: 'preferences', labelKey: 'sidebar.preferences', icon: SlidersHorizontal, path: '/dashboard?section=preferences', tourId: 'sidebar-preferences' },
  { id: 'access', labelKey: 'sidebar.access', icon: Shield, path: '/dashboard?section=access', tourId: 'sidebar-access' }
];

interface AppSidebarProps {
  activeSection: string;
  onSectionChange: (section: string) => void;
  onHelpClick?: () => void;
}

export function AppSidebar({ activeSection, onSectionChange, onHelpClick }: AppSidebarProps) {
  const { state } = useSidebar();
  const navigate = useNavigate();
  const collapsed = state === 'collapsed';
  const { t } = useTranslation('dashboard');

  const handleLogout = async () => {
    const { authService } = await import('@/services/authService');
    authService.logout();

    toast({
      title: t('sidebar.logoutSuccess'),
      description: t('sidebar.logoutMessage'),
    });
    navigate('/espace-candidats');
  };

  const handleSectionClick = (sectionId: string) => {
    onSectionChange(sectionId);
  };

  return (
    <Sidebar className="border-r border-sidebar-border bg-sidebar">
      <SidebarHeader className="border-b border-sidebar-border p-3 sm:p-4">
        <div className="flex items-center justify-center">
          <NavLink to="/">
            <img
              src="/uploads/488f38af-4f42-45f8-a54f-4c1b46a2dfff.png"
              alt="CallCenterMatch Logo"
              className="h-10 w-auto sm:h-12 object-contain"
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
              {menuItemKeys.map((item) => {
                const Icon = item.icon;
                const isActive = activeSection === item.id;

                return (
                  <SidebarMenuItem key={item.id}>
                    <SidebarMenuButton
                      onClick={() => handleSectionClick(item.id)}
                      isActive={isActive}
                      data-tour={item.tourId}
                      className={`
                        w-full justify-start h-10 px-3 rounded-lg transition-all duration-150
                        ${isActive
                          ? 'bg-primary text-primary-foreground shadow-sm font-medium'
                          : 'hover:bg-sidebar-accent/60 text-sidebar-foreground/80 hover:text-sidebar-foreground'
                        }
                      `}
                    >
                      <Icon className={`h-4 w-4 mr-3 shrink-0 ${isActive ? '' : 'opacity-70'}`} />
                      <span className="text-[13px] truncate">{t(item.labelKey)}</span>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                );
              })}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>

      <SidebarFooter className="border-t border-sidebar-border p-3">
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
