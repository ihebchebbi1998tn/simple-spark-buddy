import { useState, useEffect, useRef } from 'react';
import { candidateService } from '@/services/candidate/candidateService';
import { formatPhoneForBackend } from '@/utils/formValidation';

interface AvailabilityState {
  emailExists: boolean | null;
  phoneExists: boolean | null;
  isCheckingEmail: boolean;
  isCheckingPhone: boolean;
}

export const useAvailabilityCheck = (email: string, phone: string, debounceMs: number = 500) => {
  const [state, setState] = useState<AvailabilityState>({
    emailExists: null,
    phoneExists: null,
    isCheckingEmail: false,
    isCheckingPhone: false,
  });

  const emailTimeoutRef = useRef<NodeJS.Timeout | null>(null);
  const phoneTimeoutRef = useRef<NodeJS.Timeout | null>(null);

  // Check email availability
  useEffect(() => {
    // Clear previous timeout
    if (emailTimeoutRef.current) {
      clearTimeout(emailTimeoutRef.current);
    }

    // Reset if email is empty or invalid format
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!email || !emailRegex.test(email)) {
      setState(prev => ({ ...prev, emailExists: null, isCheckingEmail: false }));
      return;
    }

    // Set checking state
    setState(prev => ({ ...prev, isCheckingEmail: true }));

    // Debounce the API call
    emailTimeoutRef.current = setTimeout(async () => {
      try {
        const result = await candidateService.checkAvailability({ email });
        setState(prev => ({
          ...prev,
          emailExists: result.data.emailExists,
          isCheckingEmail: false,
        }));
      } catch (error) {
        console.error('Error checking email availability:', error);
        setState(prev => ({ ...prev, emailExists: null, isCheckingEmail: false }));
      }
    }, debounceMs);

    return () => {
      if (emailTimeoutRef.current) {
        clearTimeout(emailTimeoutRef.current);
      }
    };
  }, [email, debounceMs]);

  // Check phone availability
  useEffect(() => {
    // Clear previous timeout
    if (phoneTimeoutRef.current) {
      clearTimeout(phoneTimeoutRef.current);
    }

    // Clean phone number and validate
    const cleanedPhone = phone.replace(/\D/g, '');
    if (!cleanedPhone || cleanedPhone.length !== 8) {
      setState(prev => ({ ...prev, phoneExists: null, isCheckingPhone: false }));
      return;
    }

    // Set checking state
    setState(prev => ({ ...prev, isCheckingPhone: true }));

    // Debounce the API call
    phoneTimeoutRef.current = setTimeout(async () => {
      try {
        const formattedPhone = formatPhoneForBackend(cleanedPhone);
        const result = await candidateService.checkAvailability({ phone: formattedPhone });
        setState(prev => ({
          ...prev,
          phoneExists: result.data.phoneExists,
          isCheckingPhone: false,
        }));
      } catch (error) {
        console.error('Error checking phone availability:', error);
        setState(prev => ({ ...prev, phoneExists: null, isCheckingPhone: false }));
      }
    }, debounceMs);

    return () => {
      if (phoneTimeoutRef.current) {
        clearTimeout(phoneTimeoutRef.current);
      }
    };
  }, [phone, debounceMs]);

  return state;
};
