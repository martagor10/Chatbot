import {ApplicationConfig, provideZoneChangeDetection} from '@angular/core';
import {provideClientHydration, withEventReplay} from '@angular/platform-browser';
import {CommonModule} from '@angular/common';
import {provideAnimations} from '@angular/platform-browser/animations';
import {provideHttpClient, withFetch, withInterceptorsFromDi} from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({eventCoalescing: true}),
    provideClientHydration(withEventReplay()),
    CommonModule,
    provideAnimations(),
    provideHttpClient(withInterceptorsFromDi(), withFetch())
  ]
};
