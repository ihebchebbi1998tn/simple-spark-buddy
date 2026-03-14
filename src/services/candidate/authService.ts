import { apiClient } from '../common/api';
import CryptoJS from 'crypto-js';
import { jwtDecode } from 'jwt-decode';

const SECRET_KEY = import.meta.env.VITE_STORAGE_SECRET_KEY || 'default_secret_key';
const TOKEN_KEY = 'auth_token';
const USER_KEY = 'user_data';

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface LoginResponse {
  success: boolean;
  message: string;
  data?: {
    token: string;
    user: {
      id: string;
      email: string;
      name: string;
      surname: string;
      role: number;
    };
  };
}

export interface DecodedToken {
  exp: number;
  [key: string]: any;
}

export const authService = {
  // Encrypt and store token in localStorage
  storeAuthToken(token: string) {
    try {
      // Allow mock tokens for testing
      if (!token.startsWith('mock-')) {
        jwtDecode(token); // Validate the token format
      }
      const encryptedToken = CryptoJS.AES.encrypt(token, SECRET_KEY).toString();
      localStorage.setItem(TOKEN_KEY, encryptedToken);
      console.log('✅ Token stored securely (encrypted)');
    } catch (error) {
      console.error('❌ Invalid token. Token not stored:', error);
    }
  },

  // Retrieve and decrypt token from localStorage
  getAuthToken(): string | null {
    const encryptedToken = localStorage.getItem(TOKEN_KEY);
    if (!encryptedToken) return null;

    try {
      const bytes = CryptoJS.AES.decrypt(encryptedToken, SECRET_KEY);
      const decryptedToken = bytes.toString(CryptoJS.enc.Utf8);

      // Allow mock tokens for testing
      if (!decryptedToken.startsWith('mock-')) {
        jwtDecode(decryptedToken); // Will throw an error if invalid
      }
      return decryptedToken;
    } catch (error) {
      console.error('❌ Invalid or corrupted token. Clearing from storage:', error);
      localStorage.removeItem(TOKEN_KEY); // Clear the invalid token
      return null;
    }
  },

  // Check if the token is expired
  isTokenExpired(token?: string): boolean {
    const tokenToCheck = token || this.getAuthToken();
    if (!tokenToCheck) return true;

    // Mock tokens never expire
    if (tokenToCheck.startsWith('mock-')) {
      return false;
    }

    try {
      const decoded = jwtDecode<DecodedToken>(tokenToCheck);
      return Date.now() >= decoded.exp * 1000; // Convert exp to milliseconds
    } catch (error) {
      console.error('❌ Failed to decode token:', error);
      return true; // Treat invalid tokens as expired
    }
  },

  // Handle token validation and auto-logout
  handleTokenValidation(navigate?: (path: string) => void): boolean {
    const token = this.getAuthToken();
    if (!token || this.isTokenExpired(token)) {
      console.error('🔒 Token is invalid or expired. Logging out.');
      this.logout();
      if (navigate) {
        navigate('/espace-candidats');
      }
      return false;
    }
    return true;
  },

  storeUserData(user: any) {
    try {
      const encryptedUser = CryptoJS.AES.encrypt(
        JSON.stringify(user),
        SECRET_KEY
      ).toString();
      localStorage.setItem(USER_KEY, encryptedUser);
    } catch (error) {
      console.error('❌ Failed to store user data:', error);
    }
  },

  getUserData() {
    const encryptedUser = localStorage.getItem(USER_KEY);
    if (!encryptedUser) return null;

    try {
      const bytes = CryptoJS.AES.decrypt(encryptedUser, SECRET_KEY);
      const decryptedUser = bytes.toString(CryptoJS.enc.Utf8);
      return JSON.parse(decryptedUser);
    } catch (error) {
      console.error('❌ Failed to decrypt user data:', error);
      localStorage.removeItem(USER_KEY);
      return null;
    }
  },

  logout() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    console.log('🔓 User logged out successfully');
  },

  isAuthenticated(): boolean {
    const token = this.getAuthToken();
    return !!token && !this.isTokenExpired(token);
  },

  async login(credentials: LoginCredentials): Promise<LoginResponse> {
    const response = await apiClient.post('/api/v2/leads/login', {
      email: credentials.email,
      password: credentials.password,
    });
    
    // Store token and user data if login successful
    if (response.success && response.data?.token) {
      this.storeAuthToken(response.data.token);
      // Map V2 response to stored user format
      this.storeUserData({
        id: response.data.lead_id,
        lead_id: response.data.lead_id,
        email: response.data.email,
        name: response.data.last_name,
        surname: response.data.first_name,
        public_id: response.data.public_id,
      });
    }
    
    // Return in compatible format
    return {
      success: response.success,
      message: response.message,
      data: response.data ? {
        token: response.data.token,
        user: {
          id: response.data.lead_id,
          email: response.data.email,
          name: response.data.last_name,
          surname: response.data.first_name,
          role: 0,
        },
      } : undefined,
    };
  },

  async getCurrentUser() {
    const userData = this.getUserData();
    if (!userData?.lead_id && !userData?.id) return null;
    const leadId = userData.lead_id || userData.id;
    return apiClient.get(`/api/v2/leads/${leadId}`, true);
  },

  async getCandidateProfile() {
    const userData = this.getUserData();
    if (!userData?.lead_id && !userData?.id) return null;
    const leadId = userData.lead_id || userData.id;
    return apiClient.get(`/api/v2/leads/${leadId}?includeProfile=true&includeScoring=true`, true);
  },
};
