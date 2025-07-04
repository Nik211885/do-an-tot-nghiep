import { inject } from '@angular/core';
import {
  HttpInterceptorFn,
  HttpRequest,
  HttpHandlerFn,
  HttpErrorResponse
} from '@angular/common/http';
import { AuthService } from '../auth/auth.service';
import { environment } from '../../../environments/environment';
import {
  catchError,
  filter,
  finalize,
  switchMap,
  take,
  throwError,
  BehaviorSubject,
  Observable,
  of
} from 'rxjs';

// Token refresh state
let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

// Keycloak init state
let isInitialized = false;
let init$: Observable<boolean> | null = null;

export const authInterceptorFn: HttpInterceptorFn = (
  request: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<any> => {

  const authService = inject(AuthService);

  // Skip URLs
  if (shouldSkip(request)) {
    return next(removeSkipHeader(request));
  }

  // Add base URL
  request = addBaseUrl(request);

  return ensureInitialized(authService).pipe(
    switchMap(() => attachAuthHeader(request, authService)),
    switchMap((reqWithToken) => next(reqWithToken)),
    catchError((error) => handleError(error, request, next, authService))
  );
};

// ---------------------------------------
// Initialization
// ---------------------------------------

function ensureInitialized(authService: AuthService): Observable<boolean> {
  if (isInitialized) return of(true);

  if (!init$) {
    init$ = authService.initialize().pipe(
      finalize(() => {
        isInitialized = true;
        init$ = null;
      })
    );
  }

  return init$;
}

// ---------------------------------------
// Helpers
// ---------------------------------------

function shouldSkip(request: HttpRequest<any>): boolean {
  return (
    request.headers.has('X-Skip-Auth-Interceptor') ||
    request.url.includes('cloudinary.com') ||
    request.url.includes('/auth/realms/') ||
    request.url.includes('/public-api')
  );
}

function removeSkipHeader(request: HttpRequest<any>): HttpRequest<any> {
  if (request.headers.has('X-Skip-Auth-Interceptor')) {
    return request.clone({
      headers: request.headers.delete('X-Skip-Auth-Interceptor')
    });
  }
  return request;
}

function addBaseUrl(request: HttpRequest<any>): HttpRequest<any> {
  if (!request.url.startsWith('http')) {
    const baseUrl = environment.apiUrl;
    const fullUrl = baseUrl + (request.url.startsWith('/') ? request.url : '/' + request.url);
    return request.clone({ url: fullUrl });
  }
  return request;
}

function attachAuthHeader(request: HttpRequest<any>, authService: AuthService): Observable<HttpRequest<any>> {
  return authService.getToken().pipe(
    switchMap((token) => {
      if (token) {
        return of(
          request.clone({
            setHeaders: { Authorization: `Bearer ${token}` }
          })
        );
      }
      return of(request);
    })
  );
}

// ---------------------------------------
// Error Handling
// ---------------------------------------

function handleError(
  error: any,
  request: HttpRequest<any>,
  next: HttpHandlerFn,
  authService: AuthService
): Observable<any> {
  if (error instanceof HttpErrorResponse && error.status === 401) {
    return handle401(request, next, authService);
  }

  return throwError(() => error);
}

function handle401(
  request: HttpRequest<any>,
  next: HttpHandlerFn,
  authService: AuthService
): Observable<any> {
  if (!isRefreshing) {
    isRefreshing = true;
    refreshTokenSubject.next(null);

    return authService.refreshToken().pipe(
      switchMap((success) => {
        if (!success) {
          return throwError(() => new HttpErrorResponse({
            status: 401,
            statusText: 'Session expired'
          }));
        }

        return authService.getToken().pipe(
          switchMap((newToken) => {
            if (!newToken) {
              return throwError(() => new HttpErrorResponse({
                status: 401,
                statusText: 'No token available'
              }));
            }

            refreshTokenSubject.next(newToken);
            const newRequest = request.clone({
              setHeaders: { Authorization: `Bearer ${newToken}` }
            });
            return next(newRequest);
          })
        );
      }),
      catchError((err) => throwError(() => err)),
      finalize(() => {
        isRefreshing = false;
      })
    );
  } else {
    return refreshTokenSubject.pipe(
      filter(token => token !== null),
      take(1),
      switchMap((token) => {
        const newRequest = request.clone({
          setHeaders: { Authorization: `Bearer ${token}` }
        });
        return next(newRequest);
      })
    );
  }
}
