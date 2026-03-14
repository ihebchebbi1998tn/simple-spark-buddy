/**
 * Password Reset Service
 * Handles forgot password flow API calls
 */

import { apiClient } from '../common/api';

export interface PasswordResetResponse {
  success: boolean;
  message: string;
  data?: any;
}

export const passwordResetService = {
  /**
   * Request password reset - sends code to email
   */
  async requestReset(email: string): Promise<PasswordResetResponse> {
    try {
      console.log('📧 Requesting password reset for:', email);
      const response = await apiClient.post('/api/auth/forgot-password', { email });
      console.log('📧 Password reset response:', response);
      return {
        success: response.success === true,
        message: response.message || 'Code de réinitialisation envoyé',
        data: response.data
      };
    } catch (error: any) {
      console.error('Password reset request error:', error);
      return {
        success: false,
        message: error.message || 'Erreur lors de l\'envoi du code'
      };
    }
  },

  /**
   * Verify the reset code
   */
  async verifyCode(email: string, code: string): Promise<PasswordResetResponse> {
    try {
      const response = await apiClient.post('/api/auth/verify-reset-code', { email, code });
      return {
        success: true,
        message: response.message || 'Code vérifié',
        data: response.data
      };
    } catch (error: any) {
      console.error('Code verification error:', error);
      return {
        success: false,
        message: error.message || 'Code invalide ou expiré'
      };
    }
  },

  /**
   * Reset password with verified code
   */
  async resetPassword(email: string, code: string, newPassword: string): Promise<PasswordResetResponse> {
    try {
      const response = await apiClient.post('/api/auth/reset-password', { 
        email, 
        code, 
        newPassword 
      });
      return {
        success: true,
        message: response.message || 'Mot de passe modifié avec succès',
        data: response.data
      };
    } catch (error: any) {
      console.error('Password reset error:', error);
      return {
        success: false,
        message: error.message || 'Erreur lors de la modification du mot de passe'
      };
    }
  }
};
