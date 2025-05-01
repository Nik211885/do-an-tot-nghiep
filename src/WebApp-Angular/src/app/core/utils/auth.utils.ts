import { HttpHeaders } from '@angular/common/http';
import { UserModel } from '../models/user.model';

/**
 * Create HTTP headers with auth token
 * @param token The token to add to the headers
 * @returns HttpHeaders object with Authorization header
 */
export function createAuthHeaders(token: string): HttpHeaders {
  if (!token) {
    return new HttpHeaders({
      'Content-Type': 'application/json'
    });
  }

  return new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });
}

/**
 * Format the user's full name
 * @param firstName The user's first name
 * @param lastName The user's last name
 * @returns Formatted full name string
 */
export function formatUserFullName(firstName?: string, lastName?: string): string {
  if (!firstName && !lastName) {
    return '';
  }

  return [firstName, lastName].filter(Boolean).join(' ');
}

/**
 * Get user's initials from name
 * @param firstName The user's first name
 * @param lastName The user's last name
 * @returns String with user's initials
 */
export function getUserInitials(firstName?: string, lastName?: string): string {
  if (!firstName && !lastName) {
    return '';
  }

  const firstInitial = firstName ? firstName.charAt(0).toUpperCase() : '';
  const lastInitial = lastName ? lastName.charAt(0).toUpperCase() : '';

  return (firstInitial + lastInitial) || '';
}

/**
 * Determine if user has any of the specified roles
 * @param userRoles Array of user's roles
 * @param requiredRoles Array of roles to check for
 * @returns True if user has any of the required roles
 */
export function hasAnyRole(userRoles: string[], requiredRoles: string[]): boolean {
  if (!requiredRoles || requiredRoles.length === 0) {
    return true;
  }

  if (!userRoles || userRoles.length === 0) {
    return false;
  }

  return requiredRoles.some(role => userRoles.includes(role));
}

/**
 * Determine if user has all of the specified roles
 * @param userRoles Array of user's roles
 * @param requiredRoles Array of roles to check for
 * @returns True if user has all of the required roles
 */
export function hasAllRoles(userRoles: string[], requiredRoles: string[]): boolean {
  if (!requiredRoles || requiredRoles.length === 0) {
    return true;
  }

  if (!userRoles || userRoles.length === 0) {
    return false;
  }

  return requiredRoles.every(role => userRoles.includes(role));
}

/**
 * Extract user display name from user model
 * @param user The user model
 * @returns User's display name
 */
export function getUserDisplayName(user: UserModel | null): string {
  if (!user) {
    return '';
  }

  const fullName = formatUserFullName(user.firstName, user.lastName);
  if (fullName) {
    return fullName;
  }

  return user.username || user.email || '';
}

/**
 * Check if a URL belongs to an allowed list
 * Useful for redirect validation
 * @param url The URL to check
 * @param allowedUrls Array of allowed URL patterns
 * @returns True if URL is allowed
 */
export function isAllowedRedirectUrl(url: string, allowedUrls: string[]): boolean {
  if (!url) {
    return false;
  }

  // Convert to absolute URL if it's a relative path
  const absoluteUrl = url.startsWith('/')
    ? `${window.location.origin}${url}`
    : url;

  try {
    const urlObj = new URL(absoluteUrl);

    // Check if URL matches any of the allowed patterns
    return allowedUrls.some(allowedUrl => {
      // Handle wildcard patterns
      if (allowedUrl.includes('*')) {
        const pattern = allowedUrl.replace(/\*/g, '.*');
        const regex = new RegExp(`^${pattern}$`);
        return regex.test(absoluteUrl);
      }

      // Handle exact matches
      return absoluteUrl === allowedUrl;
    });
  } catch (e) {
    console.error('Error validating redirect URL:', e);
    return false;
  }
}
