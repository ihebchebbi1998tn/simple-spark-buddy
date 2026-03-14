/**
 * User Management Service — CRUD operations for team members.
 * Currently uses mock data. Replace internals with API calls.
 */
import type { ManagedUser, RecruiterRole, UserPermissionsExtended } from '@/types/recruiter';
import { mockUsers, getDefaultExtendedPermissions } from '@/data/recruiterMockData';

export type CreateUserPayload = {
  prenom: string;
  nom: string;
  email: string;
  telephone: string;
  role: RecruiterRole;
  langues: string[];
  password: string;
  permissions: UserPermissionsExtended;
};

export const userService = {
  async getUsers(): Promise<ManagedUser[]> {
    // TODO: Replace with API call
    return [...mockUsers];
  },

  async createUser(data: CreateUserPayload): Promise<ManagedUser> {
    // TODO: Replace with API call
    return {
      id: Date.now().toString(),
      prenom: data.prenom,
      nom: data.nom,
      email: data.email,
      telephone: data.telephone,
      role: data.role,
      langues: data.langues,
      actif: true,
      permissions: data.permissions,
      createdAt: new Date().toLocaleDateString('fr-FR'),
    };
  },

  async updateUser(id: string, data: Partial<CreateUserPayload>): Promise<void> {
    // TODO: Replace with API call
    console.log('updateUser', id, data);
  },

  async toggleStatus(id: string): Promise<void> {
    // TODO: Replace with API call
    console.log('toggleStatus', id);
  },

  async deleteUser(id: string): Promise<void> {
    // TODO: Replace with API call
    console.log('deleteUser', id);
  },

  getDefaultPermissions: getDefaultExtendedPermissions,
};
