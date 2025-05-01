import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
  UrlTree
} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { AuthConfig } from '../auth/auth.config';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard {
  constructor(
    private authService: AuthService,
    private router: Router,
    private authConfig: AuthConfig
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const requiredRoles = route.data['roles'] as string[];

    if (!requiredRoles || requiredRoles.length === 0) {
      return true;
    }

    if (!this.authService.isAuthenticated()) {
      this.router.navigate([this.authConfig.loginRoute], {
        queryParams: { returnUrl: state.url }
      });
      return false;
    }

    const hasRole = requiredRoles.some(role => this.authService.hasRole(role));

    if (!hasRole) {
      this.router.navigate([this.authConfig.unauthorizedRoute]);
      return false;
    }

    return true;
  }
}
