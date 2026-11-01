import { Routes } from '@angular/router';
import { founderGuard } from '@buttonry/api';

/** Lazy routes of the listings feature area, mounted at /: the sell form, the edit form and the seller's listings. */
export const routes: Routes = [
  {
    path: 'shop',
    loadComponent: () => import('./shop/shop-page').then((m) => m.ShopPage),
    title: 'Shop · Buttonry',
    canActivate: [founderGuard],
    data: { area: 'account' },
  },
  {
    path: 'sell',
    loadComponent: () => import('./sell/sell-page').then((m) => m.SellPage),
    title: 'Sell an item · Buttonry',
    canActivate: [founderGuard],
  },
  {
    path: 'sell/:reference',
    loadComponent: () => import('./sell/sell-page').then((m) => m.SellPage),
    title: 'Sell an item · Buttonry',
    canActivate: [founderGuard],
  },
  {
    path: 'items/:reference/edit',
    loadComponent: () => import('./edit-listing/edit-listing-page').then((m) => m.EditListingPage),
    title: 'Edit listing · Buttonry',
    canActivate: [founderGuard],
  },
  {
    path: 'account/listings',
    loadComponent: () => import('./my-listings/my-listings-page').then((m) => m.MyListingsPage),
    title: 'Inventory · Buttonry',
    canActivate: [founderGuard],
    data: { area: 'account' },
  },
];
