/**
 * Profile Service — company profile and billing contacts CRUD.
 * Currently uses mock data. Replace internals with API calls.
 */
import type { CompanyProfile, BillingContact } from '@/types/recruiter';
import { mockCompanyProfile, mockBillingContacts } from '@/data/recruiterMockData';

export const profileService = {
  async getCompanyProfile(): Promise<CompanyProfile> {
    // TODO: Replace with API call
    return { ...mockCompanyProfile };
  },

  async updateCompanyProfile(data: Partial<CompanyProfile>): Promise<void> {
    // TODO: Replace with API call
    console.log('updateCompanyProfile', data);
  },

  async getBillingContacts(): Promise<BillingContact[]> {
    // TODO: Replace with API call
    return [...mockBillingContacts];
  },

  async addBillingContact(contact: Omit<BillingContact, 'id'>): Promise<BillingContact> {
    // TODO: Replace with API call
    return { ...contact, id: Date.now().toString() };
  },

  async updateBillingContact(id: string, data: Partial<BillingContact>): Promise<void> {
    // TODO: Replace with API call
    console.log('updateBillingContact', id, data);
  },

  async deleteBillingContact(id: string): Promise<void> {
    // TODO: Replace with API call
    console.log('deleteBillingContact', id);
  },
};
