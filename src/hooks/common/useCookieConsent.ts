import { useState, useEffect, useCallback } from "react";

export interface CookiePreferences {
  necessary: boolean;
  analytics: boolean;
  marketing: boolean;
  preferences: boolean;
}

const COOKIE_CONSENT_KEY = "cookie-consent";
const COOKIE_PREFERENCES_KEY = "cookie-preferences";

const defaultPreferences: CookiePreferences = {
  necessary: true,
  analytics: false,
  marketing: false,
  preferences: false,
};

// Get cookie preferences from localStorage
export const getCookiePreferences = (): CookiePreferences => {
  try {
    const saved = localStorage.getItem(COOKIE_PREFERENCES_KEY);
    if (saved) {
      return JSON.parse(saved);
    }
  } catch (e) {
    console.error("Error reading cookie preferences:", e);
  }
  return defaultPreferences;
};

// Check if user has given consent
export const hasGivenConsent = (): boolean => {
  return localStorage.getItem(COOKIE_CONSENT_KEY) === "true";
};

// Check if a specific cookie type is allowed
export const isCookieAllowed = (type: keyof CookiePreferences): boolean => {
  if (!hasGivenConsent()) return type === "necessary";
  const prefs = getCookiePreferences();
  return prefs[type] ?? false;
};

// Initialize tracking services based on preferences
export const initializeTracking = (preferences: CookiePreferences) => {
  console.log("🍪 Initializing tracking with preferences:", preferences);

  // Analytics (e.g., Google Analytics, Vercel Analytics)
  if (preferences.analytics) {
    console.log("✅ Analytics cookies enabled");
    // Example: Initialize Google Analytics
    // if (typeof gtag !== 'undefined') {
    //   gtag('consent', 'update', { analytics_storage: 'granted' });
    // }
  } else {
    console.log("❌ Analytics cookies disabled");
    // Disable analytics
  }

  // Marketing (e.g., Facebook Pixel, Google Ads)
  if (preferences.marketing) {
    console.log("✅ Marketing cookies enabled");
    // Example: Initialize marketing pixels
  } else {
    console.log("❌ Marketing cookies disabled");
  }

  // Preferences (e.g., language, theme)
  if (preferences.preferences) {
    console.log("✅ Preference cookies enabled");
  } else {
    console.log("❌ Preference cookies disabled");
  }
};

// Clear all non-essential cookies
export const clearNonEssentialCookies = () => {
  // Get all cookies
  const cookies = document.cookie.split(";");
  
  // List of essential cookie names to keep
  const essentialCookies = ["cookie-consent", "cookie-preferences", "auth"];
  
  cookies.forEach((cookie) => {
    const cookieName = cookie.split("=")[0].trim();
    if (!essentialCookies.includes(cookieName)) {
      // Delete the cookie by setting expiry to past
      document.cookie = `${cookieName}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/`;
    }
  });
  
  console.log("🧹 Cleared non-essential cookies");
};

// Hook for managing cookie consent
export const useCookieConsent = () => {
  const [hasConsent, setHasConsent] = useState<boolean>(false);
  const [preferences, setPreferences] = useState<CookiePreferences>(defaultPreferences);
  const [isLoaded, setIsLoaded] = useState(false);

  // Load saved preferences on mount
  useEffect(() => {
    const consent = hasGivenConsent();
    setHasConsent(consent);
    
    if (consent) {
      const savedPrefs = getCookiePreferences();
      setPreferences(savedPrefs);
      initializeTracking(savedPrefs);
    }
    
    setIsLoaded(true);
  }, []);

  // Save consent and preferences
  const saveConsent = useCallback((newPreferences: CookiePreferences) => {
    try {
      localStorage.setItem(COOKIE_CONSENT_KEY, "true");
      localStorage.setItem(COOKIE_PREFERENCES_KEY, JSON.stringify(newPreferences));
      setHasConsent(true);
      setPreferences(newPreferences);
      
      // Initialize or disable tracking based on new preferences
      initializeTracking(newPreferences);
      
      // If analytics/marketing disabled, clear those cookies
      if (!newPreferences.analytics || !newPreferences.marketing) {
        clearNonEssentialCookies();
      }
      
      console.log("✅ Cookie consent saved:", newPreferences);
    } catch (e) {
      console.error("Error saving cookie consent:", e);
    }
  }, []);

  // Reset consent (for testing or when user wants to change preferences)
  const resetConsent = useCallback(() => {
    localStorage.removeItem(COOKIE_CONSENT_KEY);
    localStorage.removeItem(COOKIE_PREFERENCES_KEY);
    setHasConsent(false);
    setPreferences(defaultPreferences);
    clearNonEssentialCookies();
    console.log("🔄 Cookie consent reset");
  }, []);

  // Accept all cookies
  const acceptAll = useCallback(() => {
    const allAccepted: CookiePreferences = {
      necessary: true,
      analytics: true,
      marketing: true,
      preferences: true,
    };
    saveConsent(allAccepted);
  }, [saveConsent]);

  // Reject all non-essential cookies
  const rejectAll = useCallback(() => {
    const onlyNecessary: CookiePreferences = {
      necessary: true,
      analytics: false,
      marketing: false,
      preferences: false,
    };
    saveConsent(onlyNecessary);
  }, [saveConsent]);

  return {
    hasConsent,
    preferences,
    isLoaded,
    saveConsent,
    resetConsent,
    acceptAll,
    rejectAll,
    isCookieAllowed: (type: keyof CookiePreferences) => preferences[type],
  };
};

export default useCookieConsent;
