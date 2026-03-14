import { Button } from '@/components/ui/button';
import { Star } from 'lucide-react';
import { LANGUAGE_LEVELS } from '@/utils/constants';

interface LevelSelectorProps {
  value: string;
  onChange: (value: string) => void;
  disabled?: boolean;
}

export function LevelSelector({ value, onChange, disabled }: LevelSelectorProps) {
  const renderStars = (count: number) => {
    return Array.from({ length: 5 }, (_, i) => (
      <Star 
        key={i} 
        className={`h-3 w-3 ${i < count ? 'fill-yellow-400 text-yellow-400' : 'text-muted/30'}`} 
      />
    ));
  };

  return (
    <div className="grid grid-cols-2 sm:grid-cols-3 gap-2">
      {LANGUAGE_LEVELS.map((niveau) => (
        <Button
          key={niveau.value}
          type="button"
          variant={value === niveau.value ? 'default' : 'outline'}
          onClick={() => onChange(niveau.value)}
          disabled={disabled}
          className="h-auto py-3 flex flex-col items-center gap-1"
        >
          <div className="flex">{renderStars(niveau.stars)}</div>
          <span className="text-xs">{niveau.label}</span>
        </Button>
      ))}
    </div>
  );
}
