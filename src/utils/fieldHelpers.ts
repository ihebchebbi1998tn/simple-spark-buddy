/**
 * Check if a field value is empty
 */
export function isFieldEmpty(value: string | undefined | null): boolean {
  return !value || value.trim() === '';
}

/**
 * Get CSS classes for form field based on its state
 */
export function getFieldClasses(value: string | undefined, isEditing: boolean): string {
  const isEmpty = isFieldEmpty(value);
  if (isEmpty && !isEditing) {
    return 'border-destructive bg-destructive/10 placeholder:text-destructive/60';
  }
  if (!isEmpty && !isEditing) {
    return 'border-primary/30 bg-primary/5';
  }
  return '';
}

/**
 * Get label classes based on field state
 */
export function getLabelClasses(isEmpty: boolean, isEditing: boolean): string {
  return isEmpty && !isEditing ? 'text-destructive' : 'text-foreground';
}

/**
 * Calculate form completion percentage
 */
export function calculateCompletionPercentage(
  formData: Record<string, any>,
  requiredFields: string[]
): { completed: number; total: number; percentage: number } {
  const completed = requiredFields.filter(field => !isFieldEmpty(formData[field])).length;
  const total = requiredFields.length;
  const percentage = total > 0 ? (completed / total) * 100 : 0;

  return { completed, total, percentage };
}

/**
 * Get auto-save handler
 */
export function createAutoSaveHandler<T>(
  data: T,
  callback?: (data: T) => void,
  logPrefix: string = 'Auto-save'
) {
  return () => {
    console.log(`${logPrefix}:`, data);
    callback?.(data);
  };
}
