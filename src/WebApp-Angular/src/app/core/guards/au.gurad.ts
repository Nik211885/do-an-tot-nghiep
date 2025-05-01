import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
  UrlTree
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class AuthGuard {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    // First check if auth has been initialized
    if (!this.authService.isAuthenticated()) {
      return this.authService.initialize().pipe(
        map(initialized => {
          if (initialized && this.authService.isAuthenticated()) {
            return true;
          }

          // Store the attempted URL for redirecting
          const url = state.url;

          // Navigate to the login page with extras
          this.router.navigate(['/login'], {
            queryParams: { returnUrl: url }
          });

          return false;
        }),
        catchError(() => {
          this.router.navigate(['/login']);
          return of(false);
        })
      );
    }

    return true;
  }
}
