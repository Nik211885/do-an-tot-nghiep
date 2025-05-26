import { inject } from '@angular/core';
import {
  HttpInterceptorFn,
  HttpRequest,
  HttpHandlerFn,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const errorInterceptorFn: HttpInterceptorFn = (request: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const router = inject(Router);

  return next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      switch (error.status) {
        case 401:
          // Let AuthInterceptor handle token refresh
          router.navigate(['error/unauthorized']);
          break;
        case 403:
          router.navigate(['error/forbidden']);
          break;
        case 404:
          router.navigate(['error/not-found']);
          break;
        case 500:
          router.navigate(['error/internal-server']);
          break;
        default:
          router.navigate([`error/${error.status}`]);
          break;
      }

      console.error('HTTP Error:', error);

      return throwError(() => error);
    })
  );
};
