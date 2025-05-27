import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, provideHttpClient, withFetch, withInterceptors} from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import {provideAnimationsAsync} from "@angular/platform-browser/animations/async";

import { inject } from '@angular/core';
import {authInterceptorFn} from './core/interceptors/auth.interceptor';
import {errorInterceptorFn} from './core/interceptors/error.interceptor'; // thêm dòng này nếu chưa có

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimationsAsync(),
    provideRouter(routes),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideHttpClient(
      withFetch(),
      withInterceptors([authInterceptorFn/*,errorInterceptorFn*/])
    ),
    provideClientHydration(withEventReplay())
  ]
};


