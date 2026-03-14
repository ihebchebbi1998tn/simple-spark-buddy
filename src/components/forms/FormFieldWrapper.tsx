import { ReactNode } from 'react';
import { Label } from '@/components/ui/label';
import { Button } from '@/components/ui/button';
import { Edit3, Save, CheckCircle2 } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';

interface FormFieldWrapperProps {
  label: string;
  fieldName: string;
  isRequired?: boolean;
  isEditing?: boolean;
  isCompleted?: boolean;
  isEmpty?: boolean;
  showEditButton?: boolean;
  icon?: ReactNode;
  onToggleEdit?: () => void;
  children: ReactNode;
  helperText?: string;
  className?: string;
}

export function FormFieldWrapper({
  label,
  fieldName,
  isRequired = false,
  isEditing = true,
  isCompleted = false,
  isEmpty = false,
  showEditButton = false,
  icon,
  onToggleEdit,
  children,
  helperText,
  className = ''
}: FormFieldWrapperProps) {
  return (
    <motion.div
      initial={{ opacity: 0, y: 10 }}
      animate={{ opacity: 1, y: 0 }}
      className={`space-y-2 ${className}`}
    >
      <div className="flex items-center justify-between">
        <Label 
          htmlFor={fieldName}
          className={`flex items-center gap-2 ${isEmpty && !isEditing ? 'text-destructive' : 'text-foreground'}`}
        >
          {icon}
          <span>{label}</span>
          {isRequired && <span className="text-destructive">*</span>}
          <AnimatePresence>
            {isCompleted && !isEditing && (
              <motion.span
                initial={{ scale: 0, opacity: 0 }}
                animate={{ scale: 1, opacity: 1 }}
                exit={{ scale: 0, opacity: 0 }}
                transition={{ type: 'spring', stiffness: 500, damping: 30 }}
                className="ml-auto"
              >
                <CheckCircle2 className="w-4 h-4 text-primary" />
              </motion.span>
            )}
          </AnimatePresence>
        </Label>
        
        {showEditButton && onToggleEdit && (
          <Button
            variant="ghost"
            size="sm"
            onClick={onToggleEdit}
            type="button"
          >
            {isEditing ? (
              <Save className="h-4 w-4" />
            ) : (
              <Edit3 className="h-4 w-4" />
            )}
          </Button>
        )}
      </div>
      
      {children}
      
      {helperText && (
        <p className={`text-xs sm:text-sm ${isEmpty ? 'text-destructive' : 'text-muted-foreground'}`}>
          {helperText}
        </p>
      )}
    </motion.div>
  );
}
