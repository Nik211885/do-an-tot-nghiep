import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import {provideAnimationsAsync} from "@angular/platform-browser/animations/async";

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimationsAsync(),
    provideRouter(routes),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideHttpClient(withFetch()),
    provideClientHydration(withEventReplay())],
};

