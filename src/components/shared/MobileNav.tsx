import { Button } from "@/components/ui/button";
import {
  Sheet,
  SheetContent,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet";
import { Menu, Home, HelpCircle, Users, Briefcase, Info, Mail, LogIn, LogOut, Facebook, Twitter, Linkedin, LayoutDashboard, User } from "lucide-react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { useState } from "react";
import { authService } from "@/services/authService";
import { useToast } from "@/hooks/use-toast";
import { getSectionIdFromPath } from "@/utils/sectionRoutes";
import RoleChoiceModal from "@/components/auth/RoleChoiceModal";
import { useTranslation } from "react-i18next";

const MobileNav = () => {
  const [open, setOpen] = useState(false);
  const [roleModal, setRoleModal] = useState<{ open: boolean; mode: "register" | "login" }>({ open: false, mode: "register" });
  const location = useLocation();
  const navigate = useNavigate();
  const { toast } = useToast();
  const { t } = useTranslation(['header', 'common']);
  
  const isLoggedIn = !!authService.getAuthToken() && !authService.isTokenExpired();
  const userData = authService.getUserData();
  
  const handleLogout = () => {
    setOpen(false);
    authService.logout();
    toast({
      title: t('common:logoutSuccess'),
      description: t('common:logoutMessage'),
    });
    navigate('/');
    window.location.reload();
  };
  
  const getUserDisplayName = () => {
    if (userData?.name && userData?.surname) {
      return `${userData.name} ${userData.surname}`;
    }
    if (userData?.email) {
      return userData.email;
    }
    return t('common:mySpace');
  };

  const menuItems = isLoggedIn 
    ? [
        { label: t('header:home'), href: "/", icon: Home },
        { label: t('common:mySpace'), href: "/dashboard", icon: LayoutDashboard },
        { label: t('header:howItWorks'), href: "/comment-ca-marche", icon: Info },
        { label: t('header:levelTest'), href: "/test-de-niveau", icon: HelpCircle },
        { label: t('header:recruiters'), href: "/nos-recruteurs", icon: Briefcase },
        { label: t('header:about'), href: "/a-propos", icon: Info },
        { label: t('header:faq'), href: "/faq", icon: HelpCircle },
        { label: t('header:contact'), href: "/contact", icon: Mail },
      ]
    : [
        { label: t('header:home'), href: "/", icon: Home },
        { label: t('header:about'), href: "/a-propos", icon: Info },
        { label: t('header:candidates'), href: "/candidats", icon: Users },
        { label: t('header:selfAssessment'), href: "/test-langue", icon: HelpCircle },
        { label: t('header:recruiters'), href: "/entreprises", icon: Briefcase },
        { label: t('header:performanceModels'), href: "/modeles-performance", icon: Briefcase },
        { label: t('header:faq'), href: "/faq", icon: HelpCircle },
        { label: t('header:contact'), href: "/contact", icon: Mail },
      ];

  const isActive = (href: string) => {
    return (href === "/" && location.pathname === "/") || 
           (href !== "/" && location.pathname === href);
  };

  const handleSectionClick = (e: React.MouseEvent<HTMLAnchorElement>, href: string) => {
    e.preventDefault();
    setOpen(false);
    
    const sectionId = getSectionIdFromPath(href);
    if (!sectionId) return;

    if (location.pathname === '/' || getSectionIdFromPath(location.pathname)) {
      setTimeout(() => {
        const element = document.getElementById(sectionId);
        if (element) {
          const headerOffset = 80;
          const elementPosition = element.getBoundingClientRect().top;
          const offsetPosition = elementPosition + window.pageYOffset - headerOffset;
          window.scrollTo({ top: offsetPosition, behavior: 'smooth' });
          window.history.pushState(null, '', href);
        }
      }, 350);
    } else {
      navigate(href);
    }
  };

  const isSectionPath = (href: string) => !!getSectionIdFromPath(href);

  const socialLinks = [
    { icon: Facebook, href: "https://facebook.com", label: "Facebook" },
    { icon: Twitter, href: "https://twitter.com", label: "Twitter" },
    { icon: Linkedin, href: "https://linkedin.com", label: "LinkedIn" },
  ];

  return (
    <>
      <Sheet open={open} onOpenChange={setOpen}>
        <SheetTrigger asChild>
          <button
            className="lg:hidden p-2 rounded-lg text-foreground hover:bg-primary/10 transition-all duration-200"
            aria-label="Toggle menu"
          >
            <Menu size={24} />
          </button>
        </SheetTrigger>
        <SheetContent side="left" className="w-[300px] sm:w-[340px] p-0 flex flex-col">
          <SheetHeader className="px-6 py-6 border-b border-border/40 bg-gradient-to-r from-primary/5 to-transparent">
            <SheetTitle className="text-left text-xl font-bold text-foreground flex items-center gap-2">
              <div className="w-1 h-6 bg-primary rounded-full" />
              {t('common:menu')}
            </SheetTitle>
          </SheetHeader>
          
          <nav className="flex flex-col py-4 flex-1 overflow-y-auto">
            {menuItems.map((item) => {
              const active = isActive(item.href);
              const Icon = item.icon;
              
              if (isSectionPath(item.href)) {
                return (
                  <a
                    key={item.label}
                    href={item.href}
                    onClick={(e) => handleSectionClick(e, item.href)}
                    className={`flex items-center gap-3 px-6 py-3.5 text-sm font-medium transition-all duration-200 border-l-4 ${
                      active 
                        ? 'text-primary bg-primary/10 border-primary shadow-sm' 
                        : 'text-muted-foreground hover:text-foreground hover:bg-secondary/50 border-transparent hover:border-primary/20'
                    }`}
                  >
                    <Icon size={20} className={active ? "text-primary" : "text-muted-foreground"} />
                    <span>{item.label}</span>
                  </a>
                );
              }
              
              return (
                <Link
                  key={item.label}
                  to={item.href}
                  onClick={() => setOpen(false)}
                  className={`flex items-center gap-3 px-6 py-3.5 text-sm font-medium transition-all duration-200 border-l-4 ${
                    active 
                      ? 'text-primary bg-primary/10 border-primary shadow-sm' 
                      : 'text-muted-foreground hover:text-foreground hover:bg-secondary/50 border-transparent hover:border-primary/20'
                  }`}
                >
                  <Icon size={20} className={active ? "text-primary" : "text-muted-foreground"} />
                  <span>{item.label}</span>
                </Link>
              );
            })}
          </nav>

          <div className="mt-auto border-t border-border/40 bg-gradient-to-b from-transparent to-primary/5">
            {isLoggedIn ? (
              <>
                <div className="px-6 pt-4 pb-2">
                  <div className="flex items-center gap-3 p-3 bg-primary/5 rounded-lg border border-primary/20">
                    <div className="w-10 h-10 rounded-full bg-primary/20 flex items-center justify-center">
                      <User size={20} className="text-primary" />
                    </div>
                    <div className="flex-1 min-w-0">
                      <p className="text-sm font-medium text-foreground truncate">
                        {getUserDisplayName()}
                      </p>
                      <p className="text-xs text-muted-foreground">{t('common:connected')}</p>
                    </div>
                  </div>
                </div>
                
                <div className="px-6 pb-2">
                  <Link to="/dashboard" onClick={() => setOpen(false)}>
                    <Button 
                      size="lg" 
                      className="w-full bg-primary hover:bg-primary/90 text-white font-semibold shadow-lg hover:shadow-xl transition-all duration-300"
                    >
                      <LayoutDashboard size={18} className="mr-2" />
                      {t('common:mySpace')}
                    </Button>
                  </Link>
                </div>
                
                <div className="px-6 pb-4">
                  <Button 
                    variant="outline"
                    size="lg" 
                    onClick={handleLogout}
                    className="w-full border-destructive/30 text-destructive hover:bg-destructive/10 hover:text-destructive"
                  >
                    <LogOut size={18} className="mr-2" />
                    {t('common:logout')}
                  </Button>
                </div>
              </>
            ) : (
              <>
                <div className="px-6 pt-4 pb-2">
                  <Button 
                    size="lg" 
                    className="w-full bg-primary hover:bg-primary/90 text-white font-semibold shadow-lg hover:shadow-xl transition-all duration-300"
                    onClick={() => { setOpen(false); setRoleModal({ open: true, mode: "login" }); }}
                  >
                    <LogIn size={18} className="mr-2" />
                    {t('common:login')}
                  </Button>
                </div>
                <div className="px-6 pb-4">
                  <Button 
                    variant="outline"
                    size="lg" 
                    className="w-full border-primary text-primary hover:bg-primary/5 font-semibold"
                    onClick={() => { setOpen(false); setRoleModal({ open: true, mode: "register" }); }}
                  >
                    {t('common:register')}
                  </Button>
                </div>
              </>
            )}

            <div className="px-6 pb-6">
              <p className="text-xs text-muted-foreground mb-3 font-medium">{t('common:followUs')}</p>
              <div className="flex items-center gap-3 justify-center">
                {socialLinks.map((social) => (
                  <a
                    key={social.label}
                    href={social.href}
                    target="_blank"
                    rel="noopener noreferrer"
                    aria-label={social.label}
                    className="w-10 h-10 rounded-full bg-secondary/50 hover:bg-primary hover:text-white flex items-center justify-center transition-all duration-300 hover:scale-110 hover:shadow-md"
                  >
                    <social.icon size={18} />
                  </a>
                ))}
              </div>
            </div>
          </div>
        </SheetContent>
      </Sheet>

      <RoleChoiceModal
        open={roleModal.open}
        onOpenChange={(o) => setRoleModal((prev) => ({ ...prev, open: o }))}
        mode={roleModal.mode}
      />
    </>
  );
};

export default MobileNav;
