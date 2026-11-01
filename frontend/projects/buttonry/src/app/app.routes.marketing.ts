import { Routes } from '@angular/router';

/**
 * Routes for the public marketing build (`ng build buttonry --configuration marketing`), served at
 * /welcome/ on the Azure host: the base href is /welcome/ and other local paths redirect to its root.
 * Swapped in for app.routes.ts by angular.json fileReplacements.
 */
export const routes: Routes = [
  { path: '', loadChildren: () => import('@buttonry/marketing').then((m) => m.routes) },
  { path: '**', redirectTo: '' },
];
