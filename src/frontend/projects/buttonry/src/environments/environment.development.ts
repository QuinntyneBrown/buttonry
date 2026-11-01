/** Development: ng serve proxies /api to Buttonry.Api (projects/buttonry/proxy.conf.json), so cookies stay same-origin. */
export const environment = {
  apiBaseUrl: '/api',
  appOrigin: '',
  analytics: { scriptUrl: 'https://analytics.buttonry.invalid/script.js', cookieNames: ['_sya'] },
};
