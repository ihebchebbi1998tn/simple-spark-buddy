import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogDescription } from "@/components/ui/dialog";
import { useNavigate } from "react-router-dom";
import { Users, Briefcase } from "lucide-react";
import { useTranslation } from "react-i18next";

interface RoleChoiceModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  mode: "register" | "login";
}

const RoleChoiceModal = ({ open, onOpenChange, mode }: RoleChoiceModalProps) => {
  const navigate = useNavigate();
  const { t } = useTranslation('roleModal');

  const isRegister = mode === "register";

  const handleChoice = (role: "candidate" | "recruiter") => {
    onOpenChange(false);
    if (isRegister) {
      navigate(role === "candidate" ? "/candidats" : "/recruteurs-info");
    } else {
      navigate(role === "candidate" ? "/espace-candidats" : "/recruteurs/login");
    }
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-md p-0 overflow-hidden rounded-2xl">
        <DialogHeader className="px-6 pt-6 pb-2">
          <DialogTitle className="text-xl font-bold text-center">
            {isRegister ? t('registerTitle') : t('loginTitle')}
          </DialogTitle>
          <DialogDescription className="text-center text-muted-foreground">
            {isRegister ? t('registerDesc') : t('loginDesc')}
          </DialogDescription>
        </DialogHeader>

        <div className="grid grid-cols-2 gap-4 p-6">
          <button
            onClick={() => handleChoice("candidate")}
            className="group flex flex-col items-center gap-3 p-6 rounded-xl border-2 border-border bg-background hover:border-primary hover:bg-primary/5 transition-all duration-200 cursor-pointer"
          >
            <div className="w-14 h-14 rounded-full bg-primary/10 flex items-center justify-center group-hover:bg-primary/20 transition-colors">
              <Users className="w-7 h-7 text-primary" />
            </div>
            <span className="font-semibold text-foreground">{t('iAmCandidate')}</span>
            <span className="text-xs text-muted-foreground text-center leading-tight">
              {isRegister ? t('registerCandidate') : t('loginCandidate')}
            </span>
          </button>

          <button
            onClick={() => handleChoice("recruiter")}
            className="group flex flex-col items-center gap-3 p-6 rounded-xl border-2 border-border bg-background hover:border-primary hover:bg-primary/5 transition-all duration-200 cursor-pointer"
          >
            <div className="w-14 h-14 rounded-full bg-primary/10 flex items-center justify-center group-hover:bg-primary/20 transition-colors">
              <Briefcase className="w-7 h-7 text-primary" />
            </div>
            <span className="font-semibold text-foreground">{t('iAmRecruiter')}</span>
            <span className="text-xs text-muted-foreground text-center leading-tight">
              {isRegister ? t('registerRecruiter') : t('loginRecruiter')}
            </span>
          </button>
        </div>
      </DialogContent>
    </Dialog>
  );
};

export default RoleChoiceModal;
