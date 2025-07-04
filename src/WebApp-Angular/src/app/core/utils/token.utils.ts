/**
 * Parse a JWT token without using any libraries
 * @param token The JWT token string to parse
 * @returns The decoded token payload or null if invalid
 */
export function parseJwtToken(token: string): any {
  if (!token) {
    return null;
  }

  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    return JSON.parse(jsonPayload);
  } catch (e) {
    console.error('Error parsing JWT token:', e);
    return null;
  }
}

/**
 * Check if a token is expired
 * @param token The JWT token to check
 * @returns True if the token is expired or invalid, false otherwise
 */
export function isTokenExpired(token: string): boolean {
  if (!token) {
    return true;
  }

  try {
    const decoded = parseJwtToken(token);
    if (!decoded || !decoded.exp) {
      return true;
    }

    // Add a small buffer (10 seconds) to account for clock differences
    const currentTime = Math.floor(Date.now() / 1000) + 10;
    return decoded.exp < currentTime;
  } catch (e) {
    console.error('Error checking token expiration:', e);
    return true;
  }
}

/**
 * Get roles from JWT token
 * @param token The JWT token to extract roles from
 * @returns Array of roles found in the token
 */
export function getRolesFromToken(token: string): string[] {
  if (!token) {
    return [];
  }

  try {
    const decoded = parseJwtToken(token);
    if (!decoded) {
      return [];
    }

    const realmRoles = decoded.realm_access?.roles || [];
    let resourceRoles: string[] = [];

    if (decoded.resource_access) {
      Object.keys(decoded.resource_access).forEach(resource => {
        const roles = decoded.resource_access[resource]?.roles || [];
        if (roles.length > 0) {
          resourceRoles = resourceRoles.concat(roles);
        }
      });
    }

    return [...new Set([...realmRoles, ...resourceRoles])]; // Remove duplicates
  } catch (e) {
    console.error('Error extracting roles from token:', e);
    return [];
  }
}

/**
 * Extract user ID from JWT token
 * @param token The JWT token to extract user ID from
 * @returns The user ID or null if not found
 */
export function getUserIdFromToken(token: string): string | null {
  if (!token) {
    return null;
  }

  try {
    const decoded = parseJwtToken(token);
    return decoded?.sub || null;
  } catch (e) {
    console.error('Error extracting user ID from token:', e);
    return null;
  }
}

/**
 * Calculate time until token expiration in seconds
 * @param token The JWT token or token model to check
 * @returns Seconds until expiration, 0 if expired or invalid
 */
export function getTokenExpirationTime(token: string): number {
  if (!token) {
    return 0;
  }

  // Handle string token type
  try {
    const decoded = parseJwtToken(token);
    if (!decoded || !decoded.exp) {
      return 0;
    }

    const expiresAt = decoded.exp * 1000; // Convert to milliseconds
    const now = Date.now();
    return Math.max(0, Math.floor((expiresAt - now) / 1000));
  } catch (e) {
    console.error('Error calculating token expiration time:', e);
    return 0;
  }
}

