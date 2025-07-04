import { KeycloakInitOptions, KeycloakLoginOptions } from 'keycloak-js';

/**
 * Create default Keycloak init options
 * @param baseUrl The base URL of the application
 * @returns KeycloakInitOptions object
 */
export function createKeycloakInitOptions(baseUrl: string): KeycloakInitOptions {
  return {
    onLoad: 'check-sso',
    silentCheckSsoRedirectUri: `${baseUrl}/assets/silent-check-sso.html`,
    checkLoginIframe: false,
    pkceMethod: 'S256'
  };
}

/**
 * Create default Keycloak login options
 * @param redirectUri The URI to redirect to after login
 * @returns KeycloakLoginOptions object
 */
export function createKeycloakLoginOptions(redirectUri: string): KeycloakLoginOptions {
  return {
    redirectUri,
    prompt: 'login'
  };
}

/**
 * Parse Keycloak errors from URL fragments
 * @returns Error message if present in URL
 */
export function parseKeycloakError(): string | null {
  const urlParams = new URLSearchParams(window.location.search);
  const error = urlParams.get('error');
  const errorDescription = urlParams.get('error_description');

  if (error) {
    return errorDescription || error;
  }

  return null;
}

/**
 * Build Keycloak account URL
 * @param keycloakUrl The base Keycloak URL
 * @param realm The Keycloak realm
 * @param section Optional section of account (e.g., 'personal-info')
 * @returns Full Keycloak account URL
 */
export function buildKeycloakAccountUrl(keycloakUrl: string, realm: string, section?: string): string {
  const baseUrl = `${keycloakUrl}/realms/${realm}/account`;

  if (section) {
    return `${baseUrl}/#/${section}`;
  }

  return baseUrl;
}
