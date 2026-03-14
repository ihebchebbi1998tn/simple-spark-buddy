import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

// Import translations
import commonFr from './locales/fr/common.json';
import commonEn from './locales/en/common.json';
import commonAr from './locales/ar/common.json';

import headerFr from './locales/fr/header.json';
import headerEn from './locales/en/header.json';
import headerAr from './locales/ar/header.json';

import heroFr from './locales/fr/hero.json';
import heroEn from './locales/en/hero.json';
import heroAr from './locales/ar/hero.json';

import comparisonFr from './locales/fr/comparison.json';
import comparisonEn from './locales/en/comparison.json';
import comparisonAr from './locales/ar/comparison.json';

import howItWorksFr from './locales/fr/howItWorks.json';
import howItWorksEn from './locales/en/howItWorks.json';
import howItWorksAr from './locales/ar/howItWorks.json';

import testimonialsFr from './locales/fr/testimonials.json';
import testimonialsEn from './locales/en/testimonials.json';
import testimonialsAr from './locales/ar/testimonials.json';

import faqFr from './locales/fr/faq.json';
import faqEn from './locales/en/faq.json';
import faqAr from './locales/ar/faq.json';

import ctaFr from './locales/fr/cta.json';
import ctaEn from './locales/en/cta.json';
import ctaAr from './locales/ar/cta.json';

import footerFr from './locales/fr/footer.json';
import footerEn from './locales/en/footer.json';
import footerAr from './locales/ar/footer.json';

import aboutFr from './locales/fr/about.json';
import aboutEn from './locales/en/about.json';
import aboutAr from './locales/ar/about.json';

import candidatesFr from './locales/fr/candidates.json';
import candidatesEn from './locales/en/candidates.json';
import candidatesAr from './locales/ar/candidates.json';

import contactFr from './locales/fr/contact.json';
import contactEn from './locales/en/contact.json';
import contactAr from './locales/ar/contact.json';

import notFoundFr from './locales/fr/notFound.json';
import notFoundEn from './locales/en/notFound.json';
import notFoundAr from './locales/ar/notFound.json';

import performanceFr from './locales/fr/performance.json';
import performanceEn from './locales/en/performance.json';
import performanceAr from './locales/ar/performance.json';

import recruitersFr from './locales/fr/recruiters.json';
import recruitersEn from './locales/en/recruiters.json';
import recruitersAr from './locales/ar/recruiters.json';

import roleModalFr from './locales/fr/roleModal.json';
import roleModalEn from './locales/en/roleModal.json';
import roleModalAr from './locales/ar/roleModal.json';

import loginFr from './locales/fr/login.json';
import loginEn from './locales/en/login.json';
import loginAr from './locales/ar/login.json';

import candidateFormFr from './locales/fr/candidateForm.json';
import candidateFormEn from './locales/en/candidateForm.json';
import candidateFormAr from './locales/ar/candidateForm.json';

import accessRequestFr from './locales/fr/accessRequest.json';
import accessRequestEn from './locales/en/accessRequest.json';
import accessRequestAr from './locales/ar/accessRequest.json';

import dashboardFr from './locales/fr/dashboard.json';
import dashboardEn from './locales/en/dashboard.json';
import dashboardAr from './locales/ar/dashboard.json';

import recruiterDashboardFr from './locales/fr/recruiterDashboard.json';
import recruiterDashboardEn from './locales/en/recruiterDashboard.json';
import recruiterDashboardAr from './locales/ar/recruiterDashboard.json';

import chatbotFr from './locales/fr/chatbot.json';
import chatbotEn from './locales/en/chatbot.json';
import chatbotAr from './locales/ar/chatbot.json';

export const supportedLanguages = [
  { code: 'fr', label: 'Français', flag: '🇫🇷', dir: 'ltr' as const },
  { code: 'en', label: 'English', flag: '🇬🇧', dir: 'ltr' as const },
  { code: 'ar', label: 'العربية', flag: '🇹🇳', dir: 'rtl' as const },
] as const;

i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      fr: {
        common: commonFr,
        header: headerFr,
        hero: heroFr,
        comparison: comparisonFr,
        howItWorks: howItWorksFr,
        testimonials: testimonialsFr,
        faq: faqFr,
        cta: ctaFr,
        footer: footerFr,
        about: aboutFr,
        candidates: candidatesFr,
        contact: contactFr,
        notFound: notFoundFr,
        performance: performanceFr,
        recruiters: recruitersFr,
        roleModal: roleModalFr,
        login: loginFr,
        candidateForm: candidateFormFr,
        accessRequest: accessRequestFr,
        dashboard: dashboardFr,
        recruiterDashboard: recruiterDashboardFr,
        chatbot: chatbotFr,
      },
      en: {
        common: commonEn,
        header: headerEn,
        hero: heroEn,
        comparison: comparisonEn,
        howItWorks: howItWorksEn,
        testimonials: testimonialsEn,
        faq: faqEn,
        cta: ctaEn,
        footer: footerEn,
        about: aboutEn,
        candidates: candidatesEn,
        contact: contactEn,
        notFound: notFoundEn,
        performance: performanceEn,
        recruiters: recruitersEn,
        roleModal: roleModalEn,
        login: loginEn,
        candidateForm: candidateFormEn,
        accessRequest: accessRequestEn,
        dashboard: dashboardEn,
        recruiterDashboard: recruiterDashboardEn,
        chatbot: chatbotEn,
      },
      ar: {
        common: commonAr,
        header: headerAr,
        hero: heroAr,
        comparison: comparisonAr,
        howItWorks: howItWorksAr,
        testimonials: testimonialsAr,
        faq: faqAr,
        cta: ctaAr,
        footer: footerAr,
        about: aboutAr,
        candidates: candidatesAr,
        contact: contactAr,
        notFound: notFoundAr,
        performance: performanceAr,
        recruiters: recruitersAr,
        roleModal: roleModalAr,
        login: loginAr,
        candidateForm: candidateFormAr,
        accessRequest: accessRequestAr,
        dashboard: dashboardAr,
        recruiterDashboard: recruiterDashboardAr,
        chatbot: chatbotAr,
      },
    },
    fallbackLng: 'fr',
    defaultNS: 'common',
    ns: ['common', 'header', 'hero', 'comparison', 'howItWorks', 'testimonials', 'faq', 'cta', 'footer', 'about', 'candidates', 'contact', 'notFound', 'performance', 'recruiters', 'roleModal', 'login', 'candidateForm', 'accessRequest', 'dashboard', 'recruiterDashboard', 'chatbot'],
    interpolation: {
      escapeValue: false,
    },
    detection: {
      order: ['localStorage', 'navigator'],
      caches: ['localStorage'],
      lookupLocalStorage: 'i18nextLng',
    },
  });

export default i18n;
