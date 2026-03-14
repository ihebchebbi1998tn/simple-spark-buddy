// Mapping between clean URL paths and landing page section IDs
export const SECTION_ROUTES: Record<string, string> = {
  '/comment-ca-marche': 'how-it-works',
  '/inscription-candidats': 'candidats',
  '/nos-recruteurs': 'recruiters',
  '/test-de-niveau': 'test-niveau',
};

// Reverse mapping: section ID → clean path
export const SECTION_TO_PATH: Record<string, string> = Object.fromEntries(
  Object.entries(SECTION_ROUTES).map(([path, id]) => [id, path])
);

/**
 * Get the section ID for a given path, or null if not a section route
 */
export const getSectionIdFromPath = (path: string): string | null => {
  return SECTION_ROUTES[path] ?? null;
};

/**
 * Get the clean path for a given section ID, or null
 */
export const getPathFromSectionId = (sectionId: string): string | null => {
  return SECTION_TO_PATH[sectionId] ?? null;
};
