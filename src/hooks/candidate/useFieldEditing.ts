import { useState, useCallback, useRef } from 'react';

interface UseFieldEditingOptions {
  initialEditingState?: Record<string, boolean>;
  onFieldSave?: (fieldName: string, value: any) => void;
}

export function useFieldEditing({ 
  initialEditingState = {}, 
  onFieldSave 
}: UseFieldEditingOptions = {}) {
  const [editingFields, setEditingFields] = useState<Record<string, boolean>>(initialEditingState);
  const lastActiveField = useRef<string | null>(null);

  const toggleFieldEdit = useCallback((fieldName: string, value?: any) => {
    setEditingFields(prev => {
      const newState = { ...prev, [fieldName]: !prev[fieldName] };
      
      // If turning off editing and has callback, save
      if (prev[fieldName] && onFieldSave && value !== undefined && value !== '') {
        onFieldSave(fieldName, value);
      }
      
      return newState;
    });
  }, [onFieldSave]);

  const enableFieldEdit = useCallback((fieldName: string) => {
    setEditingFields(prev => ({ ...prev, [fieldName]: true }));
  }, []);

  const disableFieldEdit = useCallback((fieldName: string) => {
    setEditingFields(prev => ({ ...prev, [fieldName]: false }));
  }, []);

  const handleFieldFocus = useCallback((fieldName: string, currentData: Record<string, any>) => {
    // Save previous field if different
    if (lastActiveField.current && lastActiveField.current !== fieldName) {
      const prevValue = currentData[lastActiveField.current];
      if (prevValue && prevValue !== '') {
        disableFieldEdit(lastActiveField.current);
      }
    }
    
    lastActiveField.current = fieldName;
    enableFieldEdit(fieldName);
  }, [disableFieldEdit, enableFieldEdit]);

  const isFieldEditing = useCallback((fieldName: string) => {
    return editingFields[fieldName] ?? false;
  }, [editingFields]);

  return {
    editingFields,
    toggleFieldEdit,
    enableFieldEdit,
    disableFieldEdit,
    handleFieldFocus,
    isFieldEditing,
    lastActiveField
  };
}
