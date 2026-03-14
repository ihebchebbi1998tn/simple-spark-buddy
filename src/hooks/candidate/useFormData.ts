import { useState, useEffect, useCallback } from 'react';

interface UseFormDataOptions<T> {
  initialData: T;
  onAutoSave?: (data: T) => void;
  autoSaveDelay?: number;
}

export function useFormData<T extends Record<string, any>>({
  initialData,
  onAutoSave,
  autoSaveDelay = 1000
}: UseFormDataOptions<T>) {
  const [formData, setFormData] = useState<T>(initialData);
  const [isDirty, setIsDirty] = useState(false);

  // Auto-save on changes
  useEffect(() => {
    if (!isDirty || !onAutoSave) return;

    const timer = setTimeout(() => {
      onAutoSave(formData);
      setIsDirty(false);
    }, autoSaveDelay);

    return () => clearTimeout(timer);
  }, [formData, isDirty, onAutoSave, autoSaveDelay]);

  // Auto-save on page unload
  useEffect(() => {
    if (!onAutoSave) return;

    const handleBeforeUnload = () => {
      onAutoSave(formData);
    };

    window.addEventListener('beforeunload', handleBeforeUnload);
    return () => window.removeEventListener('beforeunload', handleBeforeUnload);
  }, [formData, onAutoSave]);

  const updateField = useCallback((field: keyof T, value: any) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    setIsDirty(true);
  }, []);

  const updateFields = useCallback((updates: Partial<T>) => {
    setFormData(prev => ({ ...prev, ...updates }));
    setIsDirty(true);
  }, []);

  // Update fields without triggering auto-save (used when loading from backend)
  const updateFieldsSilently = useCallback((updates: Partial<T>) => {
    setFormData(prev => ({ ...prev, ...updates }));
    // Don't set isDirty to prevent auto-save trigger
  }, []);

  const resetForm = useCallback(() => {
    setFormData(initialData);
    setIsDirty(false);
  }, [initialData]);

  return {
    formData,
    updateField,
    updateFields,
    updateFieldsSilently,
    resetForm,
    isDirty,
    setFormData
  };
}
