import { useState, useEffect, useCallback, createContext, useContext } from 'react';
import { createPortal } from 'react-dom';
import { Button } from './button';
import { X } from 'lucide-react';
import { cn } from '@/lib/utils';

export interface TourStep {
  target: string;
  content: React.ReactNode;
  placement?: 'top' | 'bottom' | 'left' | 'right' | 'center';
  disableBeacon?: boolean;
}

interface TourContextType {
  currentStep: number;
  totalSteps: number;
  nextStep: () => void;
  prevStep: () => void;
  skipTour: () => void;
  endTour: () => void;
}

const TourContext = createContext<TourContextType | null>(null);

interface TourProps {
  steps: TourStep[];
  run: boolean;
  onFinish: () => void;
  locale?: {
    back?: string;
    close?: string;
    last?: string;
    next?: string;
    skip?: string;
  };
}

interface TooltipPosition {
  top: number;
  left: number;
  placement: 'top' | 'bottom' | 'left' | 'right' | 'center';
}

function getTooltipPosition(
  target: Element | null,
  placement: TourStep['placement'] = 'bottom'
): TooltipPosition {
  if (!target || placement === 'center') {
    return {
      top: window.innerHeight / 2,
      left: window.innerWidth / 2,
      placement: 'center',
    };
  }

  const rect = target.getBoundingClientRect();
  const tooltipWidth = 320;
  const tooltipHeight = 200;
  const margin = 12;

  let top: number;
  let left: number;
  let finalPlacement = placement;

  switch (placement) {
    case 'top':
      top = rect.top - tooltipHeight - margin;
      left = rect.left + rect.width / 2;
      if (top < margin) {
        top = rect.bottom + margin;
        finalPlacement = 'bottom';
      }
      break;
    case 'bottom':
      top = rect.bottom + margin;
      left = rect.left + rect.width / 2;
      if (top + tooltipHeight > window.innerHeight - margin) {
        top = rect.top - tooltipHeight - margin;
        finalPlacement = 'top';
      }
      break;
    case 'left':
      top = rect.top + rect.height / 2;
      left = rect.left - tooltipWidth - margin;
      if (left < margin) {
        left = rect.right + margin;
        finalPlacement = 'right';
      }
      break;
    case 'right':
      top = rect.top + rect.height / 2;
      left = rect.right + margin;
      if (left + tooltipWidth > window.innerWidth - margin) {
        left = rect.left - tooltipWidth - margin;
        finalPlacement = 'left';
      }
      break;
    default:
      top = rect.bottom + margin;
      left = rect.left + rect.width / 2;
  }

  // Clamp values
  top = Math.max(margin, Math.min(top, window.innerHeight - tooltipHeight - margin));
  left = Math.max(margin, Math.min(left, window.innerWidth - tooltipWidth - margin));

  return { top, left, placement: finalPlacement };
}

function Spotlight({ target }: { target: Element | null }) {
  if (!target) return null;

  const rect = target.getBoundingClientRect();
  const padding = 8;

  return (
    <div
      className="fixed pointer-events-none z-[9998] rounded-xl"
      style={{
        top: rect.top - padding,
        left: rect.left - padding,
        width: rect.width + padding * 2,
        height: rect.height + padding * 2,
        boxShadow: '0 0 0 9999px rgba(0, 0, 0, 0.5)',
      }}
    />
  );
}

function TourTooltip({
  step,
  position,
  currentStep,
  totalSteps,
  onNext,
  onPrev,
  onSkip,
  onClose,
  locale,
}: {
  step: TourStep;
  position: TooltipPosition;
  currentStep: number;
  totalSteps: number;
  onNext: () => void;
  onPrev: () => void;
  onSkip: () => void;
  onClose: () => void;
  locale: TourProps['locale'];
}) {
  const isFirst = currentStep === 0;
  const isLast = currentStep === totalSteps - 1;
  const isCenter = position.placement === 'center';

  return (
    <div
      className={cn(
        "fixed z-[9999] w-80 bg-background rounded-xl shadow-2xl border p-4",
        isCenter && "-translate-x-1/2 -translate-y-1/2"
      )}
      style={{
        top: position.top,
        left: position.left,
        transform: isCenter ? 'translate(-50%, -50%)' : undefined,
      }}
    >
      <button
        onClick={onClose}
        className="absolute top-3 right-3 text-muted-foreground hover:text-foreground transition-colors"
      >
        <X className="h-4 w-4" />
      </button>

      <div className="pr-6 mb-4">{step.content}</div>

      <div className="flex items-center justify-between">
        <button
          onClick={onSkip}
          className="text-sm text-muted-foreground hover:text-foreground transition-colors"
        >
          {locale?.skip || 'Passer'}
        </button>

        <div className="flex items-center gap-2">
          <span className="text-xs text-muted-foreground">
            {currentStep + 1}/{totalSteps}
          </span>

          {!isFirst && (
            <Button variant="ghost" size="sm" onClick={onPrev}>
              {locale?.back || 'Retour'}
            </Button>
          )}

          <Button size="sm" onClick={onNext}>
            {isLast ? (locale?.last || 'Terminer') : (locale?.next || 'Suivant')}
          </Button>
        </div>
      </div>
    </div>
  );
}

export function Tour({ steps, run, onFinish, locale }: TourProps) {
  const [currentStep, setCurrentStep] = useState(0);
  const [targetElement, setTargetElement] = useState<Element | null>(null);
  const [position, setPosition] = useState<TooltipPosition>({
    top: 0,
    left: 0,
    placement: 'center',
  });

  const step = steps[currentStep];

  const updatePosition = useCallback(() => {
    if (!step) return;

    let target: Element | null = null;
    if (step.target !== 'body') {
      target = document.querySelector(step.target);
    }

    setTargetElement(target);
    setPosition(getTooltipPosition(target, step.placement));
  }, [step]);

  useEffect(() => {
    if (!run) {
      setCurrentStep(0);
      return;
    }

    updatePosition();

    const handleResize = () => updatePosition();
    window.addEventListener('resize', handleResize);

    return () => window.removeEventListener('resize', handleResize);
  }, [run, currentStep, updatePosition]);

  const nextStep = useCallback(() => {
    if (currentStep < steps.length - 1) {
      setCurrentStep(currentStep + 1);
    } else {
      onFinish();
    }
  }, [currentStep, steps.length, onFinish]);

  const prevStep = useCallback(() => {
    if (currentStep > 0) {
      setCurrentStep(currentStep - 1);
    }
  }, [currentStep]);

  const skipTour = useCallback(() => {
    onFinish();
  }, [onFinish]);

  if (!run || !step) return null;

  return createPortal(
    <>
      {/* Overlay */}
      <div className="fixed inset-0 z-[9997]" onClick={skipTour} />

      {/* Spotlight */}
      <Spotlight target={targetElement} />

      {/* Tooltip */}
      <TourTooltip
        step={step}
        position={position}
        currentStep={currentStep}
        totalSteps={steps.length}
        onNext={nextStep}
        onPrev={prevStep}
        onSkip={skipTour}
        onClose={skipTour}
        locale={locale}
      />
    </>,
    document.body
  );
}
