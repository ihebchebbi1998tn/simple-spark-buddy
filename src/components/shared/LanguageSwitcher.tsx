import { useTranslation } from 'react-i18next';
import { supportedLanguages } from '@/i18n/config';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { useEffect } from 'react';
import FR from 'country-flag-icons/react/3x2/FR';
import GB from 'country-flag-icons/react/3x2/GB';
import TN from 'country-flag-icons/react/3x2/TN';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const flagComponents: Record<string, any> = {
  fr: FR,
  en: GB,
  ar: TN,
};

const LanguageSwitcher = () => {
  const { i18n } = useTranslation();

  const currentLang = supportedLanguages.find(l => l.code === i18n.language) || supportedLanguages[0];
  const CurrentFlag = flagComponents[currentLang.code] || FR;

  useEffect(() => {
    const lang = supportedLanguages.find(l => l.code === i18n.language);
    const dir = lang?.dir || 'ltr';
    document.documentElement.dir = dir;
    document.documentElement.lang = i18n.language;
  }, [i18n.language]);

  const handleLanguageChange = (code: string) => {
    i18n.changeLanguage(code);
  };

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <button
          className="flex items-center px-1.5 py-1.5 rounded-lg hover:bg-primary/5 transition-all duration-200 border border-transparent hover:border-primary/15"
          aria-label="Change language"
        >
          <CurrentFlag className="w-6 h-4 rounded-[2px] object-cover" />
        </button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end" className="min-w-[60px] p-1">
        {supportedLanguages.map((lang) => {
          const FlagIcon = flagComponents[lang.code];
          return (
            <DropdownMenuItem
              key={lang.code}
              onClick={() => handleLanguageChange(lang.code)}
              className={`flex items-center justify-center cursor-pointer px-2 py-1.5 ${
                i18n.language === lang.code ? 'bg-primary/8 ring-1 ring-primary/30 rounded-md' : ''
              }`}
            >
              {FlagIcon && <FlagIcon className="w-6 h-4 rounded-[2px]" />}
            </DropdownMenuItem>
          );
        })}
      </DropdownMenuContent>
    </DropdownMenu>
  );
};

export default LanguageSwitcher;
