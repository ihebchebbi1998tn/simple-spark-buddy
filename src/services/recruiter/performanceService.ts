/**
 * Performance Service — metrics, campaigns, team ranking.
 * Currently uses mock data. Replace internals with API calls.
 */
import type { PerformanceMetrics, Campaign, TeamMember, Lead, Objective, WeeklyStat } from '@/types/recruiter';
import {
  mockPerformanceMetrics,
  mockCampaigns,
  mockTeamRanking,
  mockRecentLeads,
  mockObjectives,
  mockWeeklyStats,
  mockAdminOrders,
} from '@/data/recruiterMockData';

export const performanceService = {
  async getMetrics(_period?: string): Promise<PerformanceMetrics> {
    // TODO: Replace with API call
    return { ...mockPerformanceMetrics };
  },

  async getCampaigns(): Promise<Campaign[]> {
    // TODO: Replace with API call
    return [...mockCampaigns];
  },

  async getTeamRanking(): Promise<TeamMember[]> {
    // TODO: Replace with API call
    return [...mockTeamRanking];
  },

  async getRecentLeads(): Promise<Lead[]> {
    // TODO: Replace with API call
    return [...mockRecentLeads];
  },

  async getObjectives(): Promise<Objective[]> {
    // TODO: Replace with API call
    return [...mockObjectives];
  },

  async getWeeklyStats(): Promise<WeeklyStat[]> {
    // TODO: Replace with API call
    return [...mockWeeklyStats];
  },

  async getAdminOrders() {
    // TODO: Replace with API call
    return [...mockAdminOrders];
  },
};
