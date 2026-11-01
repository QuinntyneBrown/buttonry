import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideApi } from '@buttonry/api';
import { provideAppOrigin } from '@buttonry/components';
import { provideAnalytics } from '@buttonry/privacy';

import { environment } from '../environments/environment';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withComponentInputBinding()),
    provideApi(environment.apiBaseUrl),
    provideAppOrigin(environment.appOrigin),
    provideAnalytics(environment.analytics),
  ],
};
