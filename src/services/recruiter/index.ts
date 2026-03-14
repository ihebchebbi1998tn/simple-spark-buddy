/**
 * Recruiter Services — barrel export.
 * Each service encapsulates a domain entity and is ready for backend swap.
 */
export { orderService } from './orderService';
export { userService } from './userService';
export { profileService } from './profileService';
export { performanceService } from './performanceService';
export { candidateSearchService } from './searchService';
export { financialService } from './financialService';

// Re-export auth service from its existing location
export { recruiterAuthService } from '@/services/recruiterAuthService';
