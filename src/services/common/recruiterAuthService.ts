import type { RecruiterUser, RecruiterSession } from '@/types/recruiter';

const SESSION_KEY = 'recruiter_session';

// Demo accounts — will be replaced by real API calls
const DEMO_ACCOUNTS: (RecruiterUser & { password: string })[] = [
  {
    id: '1',
    email: 'admin@callcenter.ma',
    password: 'admin123',
    role: 'super_admin',
    name: 'Mohammed',
    surname: 'Alami',
    company: 'TechCall Solutions',
  },
  {
    id: '2',
    email: 'manager@callcenter.ma',
    password: 'manager123',
    role: 'admin',
    name: 'Fatima',
    surname: 'Benali',
    company: 'TechCall Solutions',
  },
  {
    id: '3',
    email: 'recruteur@callcenter.ma',
    password: 'recruteur123',
    role: 'charge',
    name: 'Ahmed',
    surname: 'Saidi',
    company: 'TechCall Solutions',
  },
];

export const recruiterAuthService = {
  /**
   * Authenticate a recruiter (mock — swap for real API later)
   */
  async login(email: string, password: string): Promise<{ success: boolean; user?: RecruiterUser; error?: string }> {
    // Simulate network delay
    await new Promise((r) => setTimeout(r, 800));

    const account = DEMO_ACCOUNTS.find((a) => a.email === email && a.password === password);

    if (!account) {
      return { success: false, error: 'Email ou mot de passe incorrect' };
    }

    const { password: _, ...user } = account;

    const session: RecruiterSession = { user };
    sessionStorage.setItem(SESSION_KEY, JSON.stringify(session));

    return { success: true, user };
  },

  /**
   * Get current session (returns null if not logged in)
   */
  getSession(): RecruiterSession | null {
    try {
      const raw = sessionStorage.getItem(SESSION_KEY);
      if (!raw) return null;
      return JSON.parse(raw) as RecruiterSession;
    } catch {
      return null;
    }
  },

  /**
   * Get current user from session
   */
  getUser(): RecruiterUser | null {
    return this.getSession()?.user ?? null;
  },

  /**
   * Check if a recruiter is logged in
   */
  isAuthenticated(): boolean {
    return this.getUser() !== null;
  },

  /**
   * Log out
   */
  logout(): void {
    sessionStorage.removeItem(SESSION_KEY);
  },
};
