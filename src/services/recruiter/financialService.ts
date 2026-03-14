/**
 * Financial Service — orders, invoices, contracts.
 * Currently uses mock data. Replace internals with API calls.
 */
import type { FinancialData } from '@/types/recruiter';
import { mockFinancialData } from '@/data/recruiterMockData';

export const financialService = {
  async getFinancialData(): Promise<FinancialData> {
    // TODO: Replace with API call
    return { ...mockFinancialData };
  },
};
