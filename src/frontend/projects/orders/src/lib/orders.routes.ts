import { Routes } from '@angular/router';
import { founderGuard } from '@buttonry/api';

/** Lazy routes of the orders feature area, mounted at /: checkout, the order confirmation, purchases and sales, the buyer's order, the founder's sales, support queue and problem page. */
export const routes: Routes = [
  {
    path: 'checkout/:reference',
    loadComponent: () => import('./checkout/checkout-page').then((m) => m.CheckoutPage),
    title: 'Checkout · Buttonry',
  },
  {
    path: 'orders/:id/confirmed',
    loadComponent: () => import('./order-confirmation/order-confirmation-page').then((m) => m.OrderConfirmationPage),
    title: 'Order confirmed · Buttonry',
  },
  {
    path: 'orders/:id',
    loadComponent: () => import('./order-detail/order-detail-page').then((m) => m.OrderDetailPage),
    title: 'Order · Buttonry',
  },
  {
    path: 'shop/sales/:id',
    canActivate: [founderGuard],
    loadComponent: () => import('./sale-detail/sale-detail-page').then((m) => m.SaleDetailPage),
    title: 'Sale · Buttonry',
  },
  {
    path: 'account/purchases',
    loadComponent: () => import('./order-history/order-history-page').then((m) => m.OrderHistoryPage),
    title: 'Purchases · Buttonry',
    data: { list: 'purchases', area: 'account' },
  },
  {
    path: 'shop/sales',
    canActivate: [founderGuard],
    loadComponent: () => import('./order-history/order-history-page').then((m) => m.OrderHistoryPage),
    title: 'Sales · Buttonry',
    data: { list: 'sales', area: 'account' },
  },
  {
    path: 'shop/support',
    canActivate: [founderGuard],
    loadComponent: () => import('./support/support-page').then((m) => m.SupportPage),
    title: 'Support · Buttonry',
    data: { area: 'account' },
  },
  {
    path: 'shop/problems/:reference',
    canActivate: [founderGuard],
    loadComponent: () => import('./problem-detail/problem-detail-page').then((m) => m.ProblemDetailPage),
    title: 'Order problem · Buttonry',
    data: { area: 'account' },
  },
];
