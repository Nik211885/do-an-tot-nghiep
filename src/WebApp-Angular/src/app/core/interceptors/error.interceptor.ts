import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private router: Router
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        // Handle different error status codes
        switch (error.status) {
          case 401: // Unauthorized
            // Let AuthInterceptor handle token refresh
            this.router.navigate(['/unauthorized']);
            break;

          case 403: // Forbidden
            this.router.navigate(['/forbidden']);
            break;

          case 404: // Not Found
            // Could navigate to a not-found page
            this.router.navigate(['/not-found']);
            break;

          case 500: // Server Error
            // Could show a server error notification
            this.router.navigate(['/internal-server']);
            break;
        }

        // Log error for debugging
        console.error('HTTP Error:', error);

        // Pass the error along
        return throwError(() => error);
      })
    );
  }
}
