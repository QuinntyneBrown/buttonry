# ADR-0017: buttonry.com shows the marketing site

**Date:** 2026-10-31
**Category:** platform
**Status:** Accepted
**Deciders:** Quinntyne Brown (product owner)

## Context

[ADR platform/0016](0016-publish-with-azure-dns.md) publishes both sites on the VM's Azure DNS
name and needs no custom domain. The owner has registered `buttonry.com` at Namecheap and wants
the domain to show the deployed marketing page, without changing how either site is built or
deployed.

## Decision

- **DNS at Namecheap.** `A @ → 20.63.41.137` (the VM's static public IP) and
  `CNAME www → buttonry.com.`, both with a 5-minute TTL. The parking page and the apex URL
  redirect are gone. The Azure DNS name stays as it is.
- **The marketing application owns the domain.** `www.buttonry.com` and `buttonry.com` both
  serve the marketing application at `/`, with no redirect and no `/welcome` path. The
  `buttonry-marketing` image carries its build twice: `/srv/marketing` (base href `/welcome/`,
  port 80, for the Azure host) and `/srv/marketing-root` (the same files with base href `/`,
  port 8080). The edge sends both domain hosts to `marketing:8080`. Caddy obtains both
  certificates from Let's Encrypt on its own.
- **The application on `app.buttonry.com`.** `CNAME app → buttonry.com.` at Namecheap and an
  `app.buttonry.com` site in the edge that proxies to `app:80`. The marketing build's
  `appOrigin` is `https://app.buttonry.com`, so its links open the application there; share tags
  and the canonical URL use `https://www.buttonry.com/`. `App__BaseUrl` in
  `/opt/buttonry/.env` should be `https://app.buttonry.com` so emails link there.
- **The Azure name keeps working.** `SITE_HOST` and both smoke tests stay on the Azure name, so a
  deploy never waits on a new certificate. If Google sign-in is turned on, the OAuth client needs
  `https://app.buttonry.com` as an origin and `/api/auth/google/callback` on it as a redirect URI.

## Options Considered

### Option 1: Keep the domain parked (ADR platform/0016 as is)
- **Pros:** No change.
- **Cons:** The domain the owner bought shows a Namecheap parking page.

### Option 2: The domain serves the marketing application at its root; the application stays on the Azure name (chosen)
- **Pros:** The marketing page is its own application at `https://www.buttonry.com/`; one image
  serves both base paths without a second Angular build; no new secret, resource or cost; the
  application's origin, cookies and Google sign-in do not move.
- **Cons:** Visitors cross from `buttonry.com` to the Azure name when they open the application.

### Option 3: Move both sites to the domain (the ADR platform/0015 hosts)
- **Pros:** One branded origin for everything.
- **Cons:** Changes `App__BaseUrl`, the Google OAuth client, `SITE_HOST`, the marketing
  application origin and the smoke tests at once; this is what ADR platform/0016 backed away from.

## Consequences

- `https://www.buttonry.com/` and `https://buttonry.com/` show the marketing application at `/`.
  The Azure URLs keep working.
- Both deploy workflows copy the edge Caddyfile and reload Caddy, so the domain survives every
  deploy.
- If the VM's public IP ever changes, the Namecheap A record must change with it.

## References

- [ADR platform/0016](0016-publish-with-azure-dns.md)
- [Caddy automatic HTTPS](https://caddyserver.com/docs/automatic-https)
- [Deployment](../../deployment.md)
