/**
 * Candidate Search Service — search simulation and criteria.
 * Currently uses mock data. Replace internals with API calls.
 */
import type { CandidateSearchCriteria, SearchResults } from '@/types/recruiter';
import { searchOptions } from '@/data/recruiterMockData';

export const candidateSearchService = {
  /** Get available filter options */
  getOptions() {
    return searchOptions;
  },

  /** Run a search simulation (will become API call) */
  async search(_criteria: CandidateSearchCriteria): Promise<SearchResults> {
    // TODO: Replace with API call
    // Simulate network delay
    await new Promise(r => setTimeout(r, 1500));
    return {
      highMatch: Math.floor(Math.random() * 200) + 100,
      acceptableMatch: Math.floor(Math.random() * 300) + 200,
      mediumMatch: Math.floor(Math.random() * 400) + 300,
      lowMatch: Math.floor(Math.random() * 500) + 400,
      outOfTarget: Math.floor(Math.random() * 1500) + 1000,
    };
  },

  /** Calculate PPI pricing */
  calculatePPIPricing(config: {
    location: string;
    urgency: string;
    model: string;
    candidatesCount: number;
  }) {
    const { ppiOptions } = require('@/data/recruiterMockData');
    const basePrice = 350;
    const locationMult = ppiOptions.locations.find((l: any) => l.value === config.location)?.multiplier ?? 1;
    const urgencyMult = ppiOptions.urgencies.find((u: any) => u.value === config.urgency)?.multiplier ?? 1;
    const modelMult = ppiOptions.models.find((m: any) => m.value === config.model)?.multiplier ?? 1;
    const costPerCandidate = Math.round(basePrice * locationMult * urgencyMult * modelMult);
    const leadsProvided = config.candidatesCount * 50;
    const totalEstimate = costPerCandidate * config.candidatesCount;
    return { costPerCandidate, leadsProvided, totalEstimate };
  },
};
