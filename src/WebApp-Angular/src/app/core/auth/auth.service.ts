import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, of, switchMap, tap } from 'rxjs';
import { UserModel } from '../models/user.model';
import { KeyCloakService } from './key-cloak.service';
import { AuthConfig } from './auth.config';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authConfig: AuthConfig = new AuthConfig()
  private currentUserSubject: BehaviorSubject<UserModel | null>;
  public currentUser: Observable<UserModel | null>;
  private initializing = false;
  private initialized = false;
  constructor(private keyCloakService: KeyCloakService,
  ) {
    this.currentUserSubject = new BehaviorSubject<UserModel | null>(null);
    this.currentUser = this.currentUserSubject.asObservable();
  }
  public initialize(): Observable<boolean> {
    if (this.initialized) {
      return of(true);
    }

    if (this.initializing) {
      return of(false);
    }

    this.initializing = true;

    return this.keyCloakService.init().pipe(
      switchMap(authenticated => {
        if (authenticated) {
          return this.loadUserProfile().pipe(
            tap(user => this.currentUserSubject.next(user)),
            map(() => {
              this.initialized = true;
              this.initializing = false;
              return true;
            })
          );
        } else {
          this.initializing = false;
          return of(false);
        }
      }),
      catchError(err => {
        this.initialized = false;
        this.initializing = false;
        console.error('Auth initialization error:', err);
        return of(false);
      })
    );
  }
  public loadUserProfile(): Observable<UserModel> {
    return this.keyCloakService.loadProfile().pipe(
      map(profile => {
        const user: UserModel = new UserModel({
          ... profile,
          roles: this.keyCloakService.getRoles()
        });
        this.currentUserSubject.next(user);
        return user;
      }),
      catchError(error => {
        console.error('Error loading user profile:', error);
        this.currentUserSubject.next(null);
        return of({} as UserModel);
      })
    );
  }
  public getCurrentUser(): Observable<UserModel | null> {
    return this.currentUserSubject.asObservable();
  }
  public getToken(): string | undefined{
    return this.keyCloakService.getToken();
  }
  public hasRole(role: string): boolean {
    return this.keyCloakService.hasRealmRole(role) || this.keyCloakService.hasResourceRole(role);
  }
  public refreshToken(): Observable<boolean> {
    return this.keyCloakService.updateToken();
  }
  public redirectToProfile(): void {
    window.location.href = `${this.authConfig.url}/realms/${this.authConfig.realm}/account`;
  }
  public login(redirectUrl?: string): Observable<boolean> {
    const options = {
      redirectUri: redirectUrl !== null
      ?  window.location.origin  + redirectUrl
      : window.location.origin + this.authConfig.defaultLoginRedirectUri
    };

    return this.keyCloakService.login(options);
  }
  public register(redirectUrl?: string): Observable<boolean> {
    const options = {
      redirectUri: redirectUrl !== null
      ?  window.location.origin  + redirectUrl
      : window.location.origin + this.authConfig.defaultLoginRedirectUri
    };

    return this.keyCloakService.register(options);
  }
  public logout(): Observable<void> {
    // Clear current user
    this.currentUserSubject.next(null);

    return this.keyCloakService.logout();
  }
  public isAuthenticated(): boolean {
    return this.keyCloakService.isAuthenticated();
  }
}
