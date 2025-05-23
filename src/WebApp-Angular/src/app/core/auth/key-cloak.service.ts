import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import Keycloak, { KeycloakInitOptions, KeycloakProfile } from 'keycloak-js';
import { AuthConfig } from './auth.config';
import { catchError, from, map, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class KeyCloakService {
  private keyCloak: Keycloak
  private authConfig: AuthConfig = new AuthConfig()
  constructor(@Inject(PLATFORM_ID) private readonly platformId: object) {
    this.keyCloak = new Keycloak({
      clientId: this.authConfig.clientId,
      realm: this.authConfig.realm,
      url: this.authConfig.url
    });
  }
  init(options?: KeycloakInitOptions): Observable<boolean> {
    return from(this.keyCloak.init(options || {
      onLoad: 'check-sso',
      // silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html',
      checkLoginIframe: false,
      pkceMethod: 'S256'
    }))
  }
  login(options?: any) : Observable<boolean>{
    return from(this.keyCloak.login(options || {
      redirectUri: window.location.origin
    })).pipe(
      map(() => true),
      catchError(error => {
        console.error('Login error:', error);
        return throwError(() => new Error('Login failed'));
      })
    );
  }
  logout() : Observable<void> {
    if(isPlatformBrowser(this.platformId)){
      return from(this.keyCloak?.logout({
        redirectUri: window.location.origin
      }));
    }
    else{
      return new Observable();
    }
  }
  isAuthenticated(): boolean{
    return this.keyCloak?.authenticated ?? false;
  }
  loadProfile() : Observable<KeycloakProfile>{
    return from(this.keyCloak.loadUserProfile()).pipe(
      catchError(error => {
        console.error('Error loading user profile:', error);
        return throwError(() => new Error('Failed to load user profile'));
      })
    );
  }
  updateToken(minValidity: number = 5): Observable<boolean> {
    return from(this.keyCloak.updateToken(minValidity));
  }
  register(options?: any): Observable<boolean>{
    return from(this.keyCloak.register(options || {
      redirectUri: window.location.origin
    })).pipe(
      map(() => true),
      catchError(error => {
        console.error('Registration error:', error);
        return throwError(() => new Error('Registration failed'));
      })
    );
  }
  getToken() : string | undefined{
    return this.keyCloak.token;
  }
  hasRealmRole(role:string): boolean{
    return this.keyCloak.hasRealmRole(role);
  }
  getTokenParesd() : any{
    return this.keyCloak.tokenParsed;
  }
  hasResourceRole(role: string, resource?: string) : boolean{
    return this.keyCloak.hasResourceRole(role, resource);
  }
  getRoles() : string[]{
    const tokenParsed = this.keyCloak.tokenParsed;
    if (!tokenParsed) return [];

    const realmRoles = tokenParsed.realm_access?.roles ?? [];
    const resourceRoles: string[] = [];

    if (tokenParsed.resource_access) {
      for (const [_, access] of Object.entries(tokenParsed.resource_access)) {
        if (Array.isArray(access.roles)) {
          resourceRoles.push(...access.roles);
        }
      }
    }

    return [...realmRoles, ...resourceRoles];
  }
  hasRole(role: string): boolean {
    return this.getRoles().includes(role);
  }
}
