import { Button } from '@/components/ui/button';
import { LANGUAGES } from '@/utils/constants';

interface LanguageSelectorProps {
  value: string;
  onChange: (value: string) => void;
  disabled?: boolean;
  disabledLanguages?: string[];
}

export function LanguageSelector({ value, onChange, disabled, disabledLanguages = [] }: LanguageSelectorProps) {
  return (
    <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-5 gap-2">
      {LANGUAGES.map((lang) => {
        const FlagIcon = lang.flag;
        const isDisabled = disabled || disabledLanguages.includes(lang.value);
        return (
          <Button
            key={lang.value}
            type="button"
            variant={value === lang.value ? 'default' : 'outline'}
            onClick={() => !isDisabled && onChange(lang.value)}
            disabled={isDisabled}
            className={`h-auto py-3 flex flex-col items-center gap-2 transition-transform ${
              isDisabled && value !== lang.value 
                ? 'opacity-40 cursor-not-allowed' 
                : 'hover:scale-105'
            }`}
          >
            <FlagIcon className="w-8 h-6 rounded shadow-sm" />
            <span className="text-xs">{lang.label}</span>
          </Button>
        );
      })}
    </div>
  );
}
