// API Configuration
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5050';

console.log('🔧 API Configuration:', {
  baseURL: API_BASE_URL,
  environment: import.meta.env.MODE
});

// Get token without importing authService (to avoid circular dependency)
async function getAuthTokenForRequest(): Promise<string | null> {
  try {
    // Import authService dynamically to decrypt token
    const { authService } = await import('../candidate/authService');
    return authService.getAuthToken();
  } catch (error) {
    console.error('Error getting token:', error);
    return null;
  }
}

export const apiClient = {
  async post(endpoint: string, data: any, requiresAuth: boolean = false) {
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
    };

    // Add JWT token to authenticated requests
    if (requiresAuth) {
      const token = await getAuthTokenForRequest();
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
        console.log('🔐 JWT token added to POST request');
      } else if (requiresAuth) {
        console.warn('⚠️ Auth required but no token found');
      }
    }

    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'POST',
      headers,
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      const error = await response.json().catch(() => ({ message: 'Request failed' }));
      throw new Error(error.message || `HTTP error! status: ${response.status}`);
    }

    return response.json();
  },

  async get(endpoint: string, requiresAuth: boolean = false) {
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
    };

    // Add JWT token to authenticated requests
    if (requiresAuth) {
      const token = await getAuthTokenForRequest();
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
        console.log('🔐 JWT token added to GET request');
      } else if (requiresAuth) {
        console.warn('⚠️ Auth required but no token found');
      }
    }

    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'GET',
      headers,
    });

    if (!response.ok) {
      const error = await response.json().catch(() => ({ message: 'Request failed' }));
      throw new Error(error.message || `HTTP error! status: ${response.status}`);
    }

    return response.json();
  },

  async put(endpoint: string, data: any, requiresAuth: boolean = false) {
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
    };

    // Add JWT token to authenticated requests
    if (requiresAuth) {
      const token = await getAuthTokenForRequest();
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
        console.log('🔐 JWT token added to PUT request');
      } else if (requiresAuth) {
        console.warn('⚠️ Auth required but no token found');
      }
    }

    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'PUT',
      headers,
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      const error = await response.json().catch(() => ({ message: 'Request failed' }));
      throw new Error(error.message || `HTTP error! status: ${response.status}`);
    }

    return response.json();
  },

  async delete(endpoint: string, data: any, requiresAuth: boolean = false) {
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
    };

    // Add JWT token to authenticated requests
    if (requiresAuth) {
      const token = await getAuthTokenForRequest();
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
        console.log('🔐 JWT token added to DELETE request');
      } else if (requiresAuth) {
        console.warn('⚠️ Auth required but no token found');
      }
    }

    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'DELETE',
      headers,
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      const error = await response.json().catch(() => ({ message: 'Request failed' }));
      throw new Error(error.message || `HTTP error! status: ${response.status}`);
    }

    return response.json();
  },
};
