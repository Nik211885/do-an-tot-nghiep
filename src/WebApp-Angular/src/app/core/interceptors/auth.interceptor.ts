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
  BehaviorSubject
} from 'rxjs';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<any>(null);

export const authInterceptorFn: HttpInterceptorFn = (request, next: HttpHandlerFn) => {
  const authService = inject(AuthService);
  const baseUrl = environment.apiUrl;

  // Nếu request tới Cloudinary, bỏ qua interceptor, xóa header X-Skip-Auth-Interceptor nếu có
  if (request.url.includes('cloudinary.com')) {
    if (request.headers.has('X-Skip-Auth-Interceptor')) {
      const headers = request.headers.delete('X-Skip-Auth-Interceptor');
      const cleanRequest = request.clone({ headers });
      return next(cleanRequest);
    }
    return next(request);
  }

  // Nếu có header X-Skip-Auth-Interceptor thì xóa header và gửi tiếp
  if (request.headers.has('X-Skip-Auth-Interceptor')) {
    const headers = request.headers.delete('X-Skip-Auth-Interceptor');
    const cleanRequest = request.clone({ headers });
    return next(cleanRequest);
  }

  // Bỏ qua các request không cần token
  if (
    request.url.includes('/auth/realms/') ||
    request.url.endsWith('/public-api')
  ) {
    return next(request);
  }

  // Gắn baseUrl nếu url không phải là url đầy đủ
  if (!request.url.startsWith('http')) {
    request = request.clone({
      url: baseUrl + request.url
    });
  }

  // Gắn token nếu có
  const token = authService.getToken();
  if (token) {
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(request).pipe(
    catchError((error) => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        return handle401Error(request, next, authService);
      }
      return throwError(() => error);
    })
  );
};

function handle401Error(request: HttpRequest<any>, next: HttpHandlerFn, authService: AuthService) {
  if (!isRefreshing) {
    isRefreshing = true;
    refreshTokenSubject.next(null);

    return authService.refreshToken().pipe(
      switchMap((success) => {
        refreshTokenSubject.next(success);
        if (success) {
          const newToken = authService.getToken();
          const newReq = request.clone({
            setHeaders: {
              Authorization: `Bearer ${newToken}`
            }
          });
          return next(newReq);
        }
        authService.logout();
        return throwError(() => new Error('Session expired'));
      }),
      catchError((err) => {
        authService.logout();
        return throwError(() => err);
      }),
      finalize(() => {
        isRefreshing = false;
      })
    );
  } else {
    return refreshTokenSubject.pipe(
      filter(result => result !== null),
      take(1),
      switchMap(() => {
        const newToken = authService.getToken();
        const newReq = request.clone({
          setHeaders: {
            Authorization: `Bearer ${newToken}`
          }
        });
        return next(newReq);
      })
    );
  }
}
