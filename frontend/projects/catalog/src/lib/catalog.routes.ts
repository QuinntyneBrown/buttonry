import { Routes } from '@angular/router';

const page = () => import('./catalog/catalog-page').then((m) => m.CatalogPage);

/** Lazy routes of the catalog feature area, mounted at /: the catalog, one page per category and the listing detail. */
export const routes: Routes = [
  { path: '', pathMatch: 'full', loadComponent: page, title: 'Buttonry' },
  { path: 'jewelry', loadComponent: page, data: { pageCategory: 'Jewelry' }, title: 'Jewelry · Buttonry' },
  { path: 'clothing', loadComponent: page, data: { pageCategory: 'Clothing' }, title: 'Clothing · Buttonry' },
  { path: 'housewares', loadComponent: page, data: { pageCategory: 'Housewares' }, title: 'Housewares · Buttonry' },
  { path: 'electronics', loadComponent: page, data: { pageCategory: 'Electronics' }, title: 'Electronics · Buttonry' },
  {
    path: 'items/:reference',
    loadComponent: () => import('./listing-detail/listing-detail-page').then((m) => m.ListingDetailPage),
    title: 'Item · Buttonry',
  },
];
