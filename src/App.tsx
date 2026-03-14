import { useState } from "react"
import { Toaster } from "@/components/ui/toaster"
import { Toaster as Sonner } from "@/components/ui/sonner"
import { TooltipProvider } from "@/components/ui/tooltip"
import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { BrowserRouter, Routes, Route } from "react-router-dom"
import { ProtectedRoute } from "@/components/auth/ProtectedRoute"
import { RecruiterProtectedRoute } from "@/components/recruiter/RecruiterProtectedRoute"
import { ResumeRegistrationModal, useResumeRegistration } from "@/components/auth/ResumeRegistrationModal"
import CookieConsent from "@/components/shared/CookieConsent"
import ScrollToTop from "@/components/shared/ScrollToTop"
import { SECTION_ROUTES } from "@/utils/sectionRoutes"

// Public pages
import Index from "./pages/Index"
import APropos from "./pages/public/APropos"
import Candidats from "./pages/public/Candidats"
import Contact from "./pages/public/Contact"
import FAQ from "./pages/public/FAQ"
import PrivacyPolicy from "./pages/public/PrivacyPolicy"
import TermsOfService from "./pages/public/TermsOfService"
import RecruteursInfo from "./pages/public/RecruteursInfo"
import ModelesPerformance from "./pages/public/ModelesPerformance"
import NotFound from "./pages/public/NotFound"

// Candidate pages
import CandidateLogin from "./pages/candidate/CandidateLogin"
import Dashboard from "./pages/candidate/Dashboard"
import DashboardContact from "./pages/candidate/DashboardContact"
import DetailedInscription from "./pages/candidate/DetailedInscription"
import LanguageTest from "./pages/candidate/LanguageTest"
import PreInscription from "./pages/candidate/PreInscription"
import ProcessingInscription from "./pages/candidate/ProcessingInscription"

// Recruiter pages
import RecruiterDashboard from "./pages/recruiter/RecruiterDashboard"
import RecruiterLogin from "./pages/recruiter/RecruiterLogin"
import RecruiterSimulator from "./pages/recruiter/RecruiterSimulator"
import DemandeAccesRecruteur from "./pages/recruiter/DemandeAccesRecruteur"

import ChatBot from "./components/ChatBot"
import { Lock } from "lucide-react"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"

const queryClient = new QueryClient()

const LOCK_PASSWORD = import.meta.env.VITE_LOCK_PASSWORD || "ved";

const AppContent = () => {
  const {
    showModal,
    setShowModal,
    completionPercentage,
    handleContinue,
    hasStep1,
    hasStep2,
    hasStep3,
    hasStep4,
  } = useResumeRegistration();

  return (
    <>
      <Routes>
        <Route path="/" element={<Index />} />
        {/* Section routes that render Index and scroll to section */}
        {Object.keys(SECTION_ROUTES).map((path) => (
          <Route key={path} path={path} element={<Index />} />
        ))}
        <Route path="/processing" element={<ProcessingInscription />} />
        <Route path="/pre-inscription" element={<PreInscription />} />
        <Route path="/a-propos" element={<APropos />} />
        <Route path="/inscription-detaillee" element={<DetailedInscription />} />
        <Route path="/candidats" element={<Candidats />} />
        <Route path="/demande-acces-recruteur" element={<DemandeAccesRecruteur />} />
        <Route path="/recruteurs-info" element={<RecruteursInfo />} />
        <Route path="/modeles-performance" element={<ModelesPerformance />} />
        <Route path="/espace-candidats" element={<CandidateLogin />} />
        <Route 
          path="/dashboard" 
          element={
            <ProtectedRoute>
              <Dashboard />
            </ProtectedRoute>
          } 
        />
        <Route 
          path="/dashboard/contact" 
          element={
            <ProtectedRoute>
              <DashboardContact />
            </ProtectedRoute>
          } 
        />
        <Route path="/contact" element={<Contact />} />
        <Route path="/faq" element={<FAQ />} />
        <Route path="/privacy" element={<PrivacyPolicy />} />
        <Route path="/terms" element={<TermsOfService />} />
        <Route path="/test-langue" element={<LanguageTest />} />
        <Route path="/entreprises" element={<RecruiterSimulator />} />
        <Route path="/recruteurs" element={
          <RecruiterProtectedRoute><RecruiterDashboard /></RecruiterProtectedRoute>
        } />
        <Route path="/recruteurs/login" element={<RecruiterLogin />} />
        <Route path="/recruteurs/dashboard" element={
          <RecruiterProtectedRoute><RecruiterDashboard /></RecruiterProtectedRoute>
        } />
        <Route path="/recruteurs/commandes/:orderId" element={
          <RecruiterProtectedRoute><RecruiterDashboard overridePage="order-detail" /></RecruiterProtectedRoute>
        } />
        
        {/* ADD ALL CUSTOM ROUTES ABOVE THE CATCH-ALL "*" ROUTE */}
        <Route path="*" element={<NotFound />} />
      </Routes>

      <ResumeRegistrationModal
        isOpen={showModal}
        onClose={() => setShowModal(false)}
        onContinue={handleContinue}
        completionPercentage={completionPercentage}
        hasStep1={hasStep1}
        hasStep2={hasStep2}
        hasStep3={hasStep3}
        hasStep4={hasStep4}
      />
    </>
  );
};

const App = () => {
  const [isUnlocked, setIsUnlocked] = useState(() => sessionStorage.getItem("app_unlocked") === "true");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(false);

  const handleUnlock = (e: React.FormEvent) => {
    e.preventDefault();
    if (password === LOCK_PASSWORD) {
      sessionStorage.setItem("app_unlocked", "true");
      setIsUnlocked(true);
      setError(false);
    } else {
      setError(true);
    }
  };

  if (!isUnlocked) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-background">
        <form onSubmit={handleUnlock} className="flex flex-col items-center gap-4 p-8 rounded-xl border border-border bg-card shadow-lg max-w-sm w-full">
          <Lock className="h-10 w-10 text-muted-foreground" />
          <h2 className="text-lg font-semibold text-foreground">Accès protégé</h2>
          <Input
            type="password"
            placeholder="Mot de passe"
            value={password}
            onChange={(e) => { setPassword(e.target.value); setError(false); }}
            className={error ? "border-destructive" : ""}
          />
          {error && <p className="text-sm text-destructive">Mot de passe incorrect</p>}
          <Button type="submit" className="w-full">Accéder</Button>
        </form>
      </div>
    );
  }

  return (
    <QueryClientProvider client={queryClient}>
      <TooltipProvider>
        <Toaster />
        <Sonner />
        <BrowserRouter>
          <ScrollToTop />
          <AppContent />
          <ChatBot />
          <CookieConsent />
        </BrowserRouter>
      </TooltipProvider>
    </QueryClientProvider>
  );
}

export default App
