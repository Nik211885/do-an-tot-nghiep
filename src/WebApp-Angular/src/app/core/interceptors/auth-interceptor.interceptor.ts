import { HttpContextToken, HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import {inject } from '@angular/core';
import { AuthKeyCloakService } from '../auth/auth-key-cloak.service';
import { environment } from '../../../environments/environment';
import { catchError, from, switchMap, throwError } from 'rxjs';

export const SKIP_AUTH = new HttpContextToken(() => false);

export const authInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  if(req.context.get(SKIP_AUTH)){
    return next(req)
  }
  const authKeyCloakService = inject(AuthKeyCloakService)
  const token = authKeyCloakService.getToken()
  const apiRequest = req.url.startsWith("http")
  ? req : req.clone({url:`${environment.apiUrl}/${req.url}`})
  const authReq = token 
  ? apiRequest.clone({setHeaders: {Authorization : `Bearer ${token}`}}) : apiRequest
  return next(authReq).pipe(
    catchError((error: HttpErrorResponse)=>{
      if(error.status === 401){
        return from(authKeyCloakService.updateToken()).pipe(
          switchMap((newToken)=>{
            const retryReq = req.clone({setHeaders:{Authorization: `Bearer ${newToken}`}})
            return next(retryReq);
          }),
          catchError((refreshErr)=>{
            authKeyCloakService.logout();
            return throwError(() => refreshErr);
          })
        )
      }
      return throwError(() => error);
    })
  )
};
