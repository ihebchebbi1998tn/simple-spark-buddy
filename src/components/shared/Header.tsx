import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Link, useLocation, useNavigate } from "react-router-dom"
import MobileNav from "@/components/shared/MobileNav"
import { authService } from "@/services/authService"
import { LogOut, LayoutDashboard } from "lucide-react"
import { useToast } from "@/hooks/use-toast"
import RoleChoiceModal from "@/components/auth/RoleChoiceModal"
import LanguageSwitcher from "@/components/shared/LanguageSwitcher"
import { useTranslation } from "react-i18next"

const Header = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { toast } = useToast();
  const { t } = useTranslation(['header', 'common']);
  const [roleModal, setRoleModal] = useState<{ open: boolean; mode: "register" | "login" }>({ open: false, mode: "register" });
  
  const isLoggedIn = !!authService.getAuthToken() && !authService.isTokenExpired();
  
  const handleLogout = () => {
    authService.logout();
    toast({
      title: t('common:logoutSuccess'),
      description: t('common:logoutMessage'),
    });
    navigate('/');
    window.location.reload();
  };

  const navLinks = [
    { label: t('header:about'), href: "/a-propos" },
    { label: t('header:candidates'), href: "/candidats" },
    { label: t('header:selfAssessment'), href: "/test-langue" },
    { label: t('header:recruiters'), href: "/entreprises" },
    { label: t('header:performanceModels'), href: "/modeles-performance" },
  ];

  const isActive = (href: string) => location.pathname === href;

  return (
    <>
      <header className="bg-background/95 backdrop-blur-md border-b border-border/60 sticky top-0 z-50">
        <div className="container mx-auto px-4 lg:px-6 max-w-[1280px]">
          <div className="flex items-center justify-between h-[68px]">
            {/* Logo */}
            <Link to="/" className="flex-shrink-0">
              <img
                src="/uploads/488f38af-4f42-45f8-a54f-4c1b46a2dfff.png"
                alt="CallCenterMatch.ai"
                className="h-11 w-auto cursor-pointer hover:opacity-90 transition-opacity duration-200"
              />
            </Link>

            {/* Desktop Navigation */}
            <nav className="hidden lg:flex items-center gap-0.5">
              {navLinks.map((item) => (
                <Link
                  key={item.href}
                  to={item.href}
                  className={`px-3.5 py-2 rounded-lg text-[13px] font-medium transition-all duration-200 whitespace-nowrap ${
                    isActive(item.href)
                      ? "bg-primary/8 text-primary font-semibold"
                      : "text-foreground/70 hover:text-primary hover:bg-primary/5"
                  }`}
                >
                  {item.label}
                </Link>
              ))}
            </nav>

            {/* Auth Buttons + Language Switcher */}
            <div className="hidden lg:flex items-center gap-1.5">
              {isLoggedIn ? (
                <>
                  <Link to="/dashboard">
                    <Button variant="outline" size="sm" className="rounded-lg font-medium px-5 h-9 border-border text-foreground/80 hover:text-primary hover:border-primary/30 transition-all">
                      <LayoutDashboard size={15} className="mr-1.5" />
                      {t('common:mySpace')}
                    </Button>
                  </Link>
                  <Button
                    variant="ghost"
                    size="icon"
                    onClick={handleLogout}
                    className="text-destructive/70 hover:text-destructive hover:bg-destructive/8 h-9 w-9 rounded-lg"
                  >
                    <LogOut size={16} />
                  </Button>
                </>
              ) : (
                <>
                  <Button
                    variant="ghost"
                    size="sm"
                    className="rounded-lg font-medium px-5 h-9 text-[13px] text-foreground/70 hover:text-primary hover:bg-primary/5 whitespace-nowrap transition-all"
                    onClick={() => setRoleModal({ open: true, mode: "login" })}
                  >
                    {t('common:login')}
                  </Button>
                  <Button
                    size="sm"
                    className="rounded-lg font-semibold px-5 h-9 text-[13px] shadow-sm hover:shadow-md whitespace-nowrap transition-all"
                    onClick={() => setRoleModal({ open: true, mode: "register" })}
                  >
                    {t('common:register')}
                  </Button>
                </>
              )}
              <LanguageSwitcher />
            </div>

            {/* Mobile Menu */}
            <div className="flex lg:hidden items-center gap-1.5">
              <LanguageSwitcher />
              <MobileNav />
            </div>
          </div>
        </div>
      </header>

      <RoleChoiceModal
        open={roleModal.open}
        onOpenChange={(open) => setRoleModal((prev) => ({ ...prev, open }))}
        mode={roleModal.mode}
      />
    </>
  )
}

export default Header
