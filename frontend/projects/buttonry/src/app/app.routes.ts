import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: 'welcome', loadChildren: () => import('@buttonry/marketing').then((m) => m.routes) },
  { path: '', loadChildren: () => import('@buttonry/catalog').then((m) => m.routes) },
  { path: '', loadChildren: () => import('@buttonry/listings').then((m) => m.routes) },
  { path: '', loadChildren: () => import('@buttonry/orders').then((m) => m.routes) },
  { path: 'members', loadChildren: () => import('@buttonry/reviews').then((m) => m.routes) },
  { path: 'privacy', loadChildren: () => import('@buttonry/privacy').then((m) => m.routes) },
  { path: '', loadChildren: () => import('@buttonry/privacy').then((m) => m.staticRoutes) },
  { path: '', loadChildren: () => import('@buttonry/accounts').then((m) => m.routes) },
  { path: '**', loadChildren: () => import('@buttonry/accounts').then((m) => m.notFoundRoutes) },
];
