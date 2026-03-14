import { useNavigate } from 'react-router-dom';
import { ArrowLeft } from 'lucide-react';
import Header from "@/components/shared/Header";
import Footer from "@/components/shared/Footer";
import { FAQSection } from "@/components/landing";
import { Button } from '@/components/ui/button';
import { useTranslation } from 'react-i18next';

const FAQ = () => {
  const navigate = useNavigate();
  const { t } = useTranslation(['faq', 'common']);

  return (
    <div className="min-h-screen">
      <Header />
      <main className="pt-8">
        <div className="container mx-auto px-4 lg:px-8 max-w-7xl py-8">
          <Button
            variant="ghost"
            onClick={() => navigate('/')}
            className="flex items-center gap-2 text-muted-foreground hover:text-foreground transition-colors mb-8"
          >
            <ArrowLeft className="h-4 w-4" />
            {t('common:backToHome')}
          </Button>

          <div className="text-center mb-12">
            <h1 className="text-4xl lg:text-5xl font-bold text-brand-dark mb-4">
              {t('faq:pageTitle')} <span className="text-primary">{t('faq:pageTitleHighlight')}</span>
            </h1>
            <p className="text-lg text-muted-foreground max-w-2xl mx-auto">
              {t('faq:pageSubtitle')}
            </p>
          </div>
        </div>
        <FAQSection />
      </main>
      <Footer />
    </div>
  );
};

export default FAQ;
