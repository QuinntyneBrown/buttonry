import { Routes } from '@angular/router';

/** The privacy page, mounted at /privacy. */
export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/policy-pages').then((m) => m.PrivacyPage),
    title: 'Privacy · Buttonry',
  },
];

/** The other static pages, mounted at /: the terms, the shop policies and the server error page. */
export const staticRoutes: Routes = [
  {
    path: 'terms',
    loadComponent: () => import('./pages/policy-pages').then((m) => m.TermsPage),
    title: 'Terms of service · Buttonry',
  },
  {
    path: 'rules',
    loadComponent: () => import('./pages/policy-pages').then((m) => m.ShopPoliciesPage),
    title: 'Shop policies · Buttonry',
  },
  {
    path: 'error',
    loadComponent: () => import('./pages/policy-pages').then((m) => m.ServerErrorPage),
    title: 'Something went wrong · Buttonry',
  },
];
