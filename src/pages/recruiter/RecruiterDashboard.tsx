import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { Menu, Bell, HelpCircle, LogOut } from 'lucide-react';
import { useTranslation } from 'react-i18next';
import { useNavigate, useSearchParams } from 'react-router-dom';
import {
  SidebarProvider,
  SidebarTrigger,
  SidebarInset
} from '@/components/ui/sidebar';
import { Button } from '@/components/ui/button';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import { RecruiterSidebar } from '@/components/recruiter/RecruiterSidebar';
import { RecruiterTour } from '@/components/recruiter/RecruiterTour';
import { RecruiterOverview } from '@/components/recruiter/RecruiterOverview';
import { CandidateSearch } from '@/components/recruiter/CandidateSearch';
import { RecruiterOrders } from '@/components/recruiter/RecruiterOrders';
import { RecruiterOffers } from '@/components/recruiter/RecruiterOffers';
import { RecruiterProfile } from '@/components/recruiter/RecruiterProfile';
import { UserManagement } from '@/components/recruiter/UserManagement';
import { PerformanceDashboard } from '@/components/recruiter/PerformanceDashboard';
import { ChargeCampaigns } from '@/components/recruiter/ChargeCampaigns';
import { MyAccount } from '@/components/recruiter/MyAccount';
import { NotificationsPanel } from '@/components/recruiter/NotificationsPanel';
import { LeadImportPage } from '@/components/recruiter/LeadImportPage';
import OrderDetailPage from '@/pages/recruiter/OrderDetailPage';
import { useToast } from '@/hooks/use-toast';
import { recruiterAuthService } from '@/services/recruiterAuthService';
import { ROLE_LABELS, getAllowedSections } from '@/types/recruiter';
import type { RecruiterUser } from '@/types/recruiter';

interface RecruiterDashboardProps {
  overridePage?: string;
}

const RecruiterDashboard = ({ overridePage }: RecruiterDashboardProps) => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const { toast } = useToast();
  const { t } = useTranslation('recruiterDashboard');
  const [activeSection, setActiveSection] = useState(() => {
    const session = recruiterAuthService.getUser();
    return session?.role === 'charge' ? 'performance' : 'overview';
  });
  const [previousSection, setPreviousSection] = useState<string | null>(null);
  const [runTour, setRunTour] = useState(false);
  const [isNotificationsOpen, setIsNotificationsOpen] = useState(false);

  // Read user from session (RecruiterProtectedRoute guarantees it exists)
  const sessionUser = recruiterAuthService.getUser();
  const user: RecruiterUser = sessionUser ?? {
    name: 'Utilisateur',
    surname: '',
    email: '',
    company: '',
    role: 'charge',
  };

  const allowedSections = getAllowedSections(user);

  // Get section from URL params on mount
  useEffect(() => {
    const section = searchParams.get('section');
    if (section) {
      setActiveSection(section);
    }
  }, [searchParams]);

  // Scroll to top on mount and check for tour
  useEffect(() => {
    window.scrollTo(0, 0);

    // Check if this is the first visit
    const tourCompleted = localStorage.getItem('recruiter-tour-completed');
    if (!tourCompleted) {
      setTimeout(() => setRunTour(true), 1000);
    }
  }, []);

  const handleSectionChange = (section: string, trackPrevious = false) => {
    if (trackPrevious) {
      setPreviousSection(activeSection);
    } else {
      setPreviousSection(null);
    }
    // Allow lead-import for super_admin only
    const isLeadImport = section === 'lead-import' && user.role === 'super_admin';
    if (allowedSections.includes(section) || isLeadImport) {
      // If we're on an order detail route, navigate back to dashboard
      if (overridePage) {
        navigate('/recruteurs/dashboard?section=' + section);
        return;
      }
      setActiveSection(section);
    }
  };

  const handleLogout = () => {
    recruiterAuthService.logout();
    toast({
      title: t('sidebar.logoutSuccess'),
      description: t('sidebar.logoutMessage'),
    });
    navigate('/recruteurs/login');
  };

  const renderActiveSection = () => {
    if (overridePage === 'order-detail') {
      return <OrderDetailPage />;
    }
    switch (activeSection) {
      case 'overview':
        return <RecruiterOverview onSectionChange={handleSectionChange} user={user} />;
      case 'search':
        return <CandidateSearch onNavigateToOffers={() => handleSectionChange('offers', true)} />;
      case 'orders':
        return <RecruiterOrders onNavigateToOffers={() => handleSectionChange('offers', true)} />;
      case 'offers':
        return <RecruiterOffers onGoBack={previousSection ? () => handleSectionChange(previousSection) : undefined} />;
      case 'profile':
        return <RecruiterProfile user={user} />;
      case 'users':
        return <UserManagement />;
      case 'performance':
        return <PerformanceDashboard userRole={user.role} userName={user.name} />;
      case 'campaigns':
        return <ChargeCampaigns />;
      case 'lead-import':
        // Security check: only admin and super_admin can access
        if (user.role === 'admin' || user.role === 'super_admin') {
          return <LeadImportPage onBack={() => handleSectionChange('overview')} />;
        }
        return <RecruiterOverview onSectionChange={handleSectionChange} user={user} />;
      case 'my-account':
        return <MyAccount user={user} />;
      default:
        // For charge role, default to performance (Mon Dashboard)
        if (user.role === 'charge') {
          return <PerformanceDashboard userRole={user.role} userName={user.name} />;
        }
        return <RecruiterOverview onSectionChange={handleSectionChange} user={user} />;
    }
  };

  const handleTourFinish = () => {
    setRunTour(false);
    localStorage.setItem('recruiter-tour-completed', 'true');
  };

  return (
    <SidebarProvider defaultOpen={true}>
      <RecruiterTour run={runTour} onFinish={handleTourFinish} userRole={user.role} />
      <NotificationsPanel isOpen={isNotificationsOpen} onClose={() => setIsNotificationsOpen(false)} />
      <div className="flex min-h-screen w-full bg-background overflow-hidden">
        <RecruiterSidebar
          activeSection={activeSection}
          onSectionChange={handleSectionChange}
          userRole={user.role}
          user={user}
        />

        <SidebarInset className="flex-1 flex flex-col min-w-0">
          {/* Header */}
          <header className="sticky top-0 z-50 bg-background/80 backdrop-blur-md border-b border-border/50">
            <div className="px-4 sm:px-6 lg:px-8">
              <div className="flex h-14 items-center justify-between gap-3">
                {/* Left side */}
                <div className="flex items-center gap-3 flex-1 min-w-0">
                  <SidebarTrigger className="md:hidden p-2 hover:bg-muted rounded-lg transition-colors shrink-0">
                    <Menu className="h-5 w-5 text-muted-foreground" />
                  </SidebarTrigger>

                  <div className="min-w-0">
                    <h1 className="text-sm font-semibold text-foreground truncate">
                      {t('header.recruiterSpace')}
                    </h1>
                    <p className="text-[11px] text-muted-foreground truncate">
                      {user.company}
                    </p>
                  </div>
                </div>

                {/* Right side */}
                <div className="flex items-center gap-1 sm:gap-2 shrink-0">
                  <Button
                    variant="ghost"
                    size="icon"
                    data-tour="recruiter-notifications"
                    className="relative h-8 w-8 rounded-lg"
                    onClick={() => setIsNotificationsOpen(true)}
                  >
                    <Bell className="h-4 w-4 text-muted-foreground" />
                    <span className="absolute top-1 right-1 w-2 h-2 bg-destructive rounded-full ring-2 ring-background" />
                  </Button>

                  <Button
                    variant="ghost"
                    size="icon"
                    onClick={() => setRunTour(true)}
                    className="h-8 w-8 rounded-lg hidden sm:flex"
                    title="Guide"
                  >
                    <HelpCircle className="h-4 w-4 text-muted-foreground" />
                  </Button>

                  <div
                    className="flex items-center gap-2.5 ml-1 pl-3 border-l border-border/50 cursor-pointer hover:opacity-80 transition-opacity"
                    data-tour="recruiter-user-menu"
                    onClick={() => handleSectionChange('my-account')}
                    title={t('header.myAccount')}
                  >
                    <Avatar className="h-8 w-8">
                      <AvatarFallback className="bg-primary text-primary-foreground text-xs font-semibold">
                        {user.name.charAt(0)}{user.surname.charAt(0)}
                      </AvatarFallback>
                    </Avatar>
                    <div className="hidden lg:block min-w-0">
                      <p className="text-sm font-medium text-foreground truncate leading-tight">
                        {user.name} {user.surname}
                      </p>
                      <p className="text-[11px] text-muted-foreground truncate">
                        {t(`roles.${user.role}`)}
                      </p>
                    </div>
                  </div>

                  <Button
                    variant="ghost"
                    size="icon"
                    onClick={handleLogout}
                    className="h-8 w-8 rounded-lg text-muted-foreground hover:text-destructive hover:bg-destructive/10"
                  >
                    <LogOut className="h-4 w-4" />
                  </Button>
                </div>
              </div>
            </div>
          </header>

          {/* Main Content */}
          <main className="flex-1 p-3 sm:p-6 lg:p-8 overflow-auto">
            <motion.div
              key={activeSection}
              initial={{ opacity: 0, y: 8 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.2 }}
            >
              {renderActiveSection()}
            </motion.div>
          </main>
        </SidebarInset>
      </div>
    </SidebarProvider>
  );
};

export default RecruiterDashboard;
