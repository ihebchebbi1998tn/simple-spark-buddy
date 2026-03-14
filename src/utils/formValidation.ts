// Phone number formatting for display (XX XXX XXX)
export const formatPhoneNumber = (value: string): string => {
  const cleaned = value.replace(/\D/g, '');
  
  if (cleaned.length <= 2) return cleaned;
  if (cleaned.length <= 5) return `${cleaned.slice(0, 2)} ${cleaned.slice(2)}`;
  if (cleaned.length <= 8) return `${cleaned.slice(0, 2)} ${cleaned.slice(2, 5)} ${cleaned.slice(5)}`;
  return `${cleaned.slice(0, 2)} ${cleaned.slice(2, 5)} ${cleaned.slice(5, 8)}`;
};

// Convert Tunisian phone to international format for backend
export const formatPhoneForBackend = (value: string): string => {
  const cleaned = value.replace(/\D/g, '');
  // Add Tunisia country code if not present
  if (cleaned.length === 8) {
    return `+216${cleaned}`;
  }
  return cleaned.startsWith('+') ? cleaned : `+${cleaned}`;
};

// Email validation and suggestions
export const validateEmail = (email: string): { isValid: boolean; suggestion?: string } => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  const isValid = emailRegex.test(email);
  
  if (!isValid && email.includes('@')) {
    const commonDomains = ['gmail.com', 'yahoo.com', 'hotmail.com', 'outlook.com'];
    const [, domain] = email.split('@');
    
    if (domain) {
      const suggestion = commonDomains.find(d => 
        d.startsWith(domain.toLowerCase()) || 
        levenshteinDistance(domain.toLowerCase(), d) <= 2
      );
      
      if (suggestion) {
        return { isValid: false, suggestion: email.split('@')[0] + '@' + suggestion };
      }
    }
  }
  
  return { isValid, suggestion: undefined };
};

// Levenshtein distance for typo detection
const levenshteinDistance = (str1: string, str2: string): number => {
  const matrix: number[][] = [];
  
  for (let i = 0; i <= str2.length; i++) {
    matrix[i] = [i];
  }
  
  for (let j = 0; j <= str1.length; j++) {
    matrix[0][j] = j;
  }
  
  for (let i = 1; i <= str2.length; i++) {
    for (let j = 1; j <= str1.length; j++) {
      if (str2.charAt(i - 1) === str1.charAt(j - 1)) {
        matrix[i][j] = matrix[i - 1][j - 1];
      } else {
        matrix[i][j] = Math.min(
          matrix[i - 1][j - 1] + 1,
          matrix[i][j - 1] + 1,
          matrix[i - 1][j] + 1
        );
      }
    }
  }
  
  return matrix[str2.length][str1.length];
};

// Password strength checker
export interface PasswordStrength {
  score: number; // 0-4
  label: string;
  color: string;
  suggestions: string[];
  isValid: boolean; // Meets minimum backend requirements
}

// Validate password meets backend requirements: 8+ chars, uppercase, lowercase, digit, special char
export const validatePasswordRequirements = (password: string): { isValid: boolean; errors: string[] } => {
  const errors: string[] = [];
  
  if (password.length < 8) {
    errors.push('Au moins 8 caractères');
  }
  if (!/[A-Z]/.test(password)) {
    errors.push('Au moins une majuscule');
  }
  if (!/[a-z]/.test(password)) {
    errors.push('Au moins une minuscule');
  }
  if (!/\d/.test(password)) {
    errors.push('Au moins un chiffre');
  }
  if (!/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(password)) {
    errors.push('Au moins un caractère spécial (!@#$%...)');
  }
  
  return {
    isValid: errors.length === 0,
    errors
  };
};

export const checkPasswordStrength = (password: string): PasswordStrength => {
  let score = 0;
  const suggestions: string[] = [];
  
  // Check backend requirements first
  const requirements = validatePasswordRequirements(password);
  
  // Score based on backend requirements (these are mandatory)
  if (password.length >= 8) score++;
  else suggestions.push('Au moins 8 caractères');
  
  if (/[a-z]/.test(password)) score++;
  else suggestions.push('Au moins une minuscule');
  
  if (/[A-Z]/.test(password)) score++;
  else suggestions.push('Au moins une majuscule');
  
  if (/\d/.test(password)) score++;
  else suggestions.push('Au moins un chiffre');
  
  // Bonus for special characters (not required by backend but strengthens password)
  if (/[!@#$%^&*(),.?":{}|<>]/.test(password) && requirements.isValid) {
    // Only count as bonus if base requirements are met
  }
  
  const labels = ['Très faible', 'Faible', 'Moyen', 'Fort', 'Très fort'];
  const colors = [
    'hsl(var(--destructive))',
    'hsl(20 100% 50%)',
    'hsl(45 100% 50%)',
    'hsl(120 60% 50%)',
    'hsl(120 80% 40%)'
  ];
  
  return {
    score,
    label: labels[score],
    color: colors[score],
    suggestions,
    isValid: requirements.isValid
  };
};

// Field validation
export const validateField = (name: string, value: string): { isValid: boolean; error?: string } => {
  if (!value || value.trim() === '') {
    return { isValid: false, error: 'Ce champ est requis' };
  }
  
  switch (name) {
    case 'telephone':
      const cleaned = value.replace(/\D/g, '');
      // Check if it's a Tunisian phone number (8 digits)
      if (cleaned.length !== 8) {
        return { isValid: false, error: 'Format invalide. Le numéro doit contenir 8 chiffres (ex: 20123456)' };
      }
      // Validate Tunisian phone number format (starts with valid prefix)
      const validPrefixes = ['2', '3', '4', '5', '7', '9'];
      if (!validPrefixes.includes(cleaned[0])) {
        return { isValid: false, error: 'Numéro tunisien invalide. Doit commencer par 2, 3, 4, 5, 7 ou 9' };
      }
      return { isValid: true };
      
    case 'email':
      const emailCheck = validateEmail(value);
      if (!emailCheck.isValid) {
        return { isValid: false, error: 'Email invalide' };
      }
      return { isValid: true };
      
    case 'motDePasse':
      const pwdRequirements = validatePasswordRequirements(value);
      if (!pwdRequirements.isValid) {
        return { isValid: false, error: pwdRequirements.errors.join(', ') };
      }
      return { isValid: true };
      
    default:
      return { isValid: true };
  }
};
