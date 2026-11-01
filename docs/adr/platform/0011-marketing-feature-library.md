# ADR-0011: A Marketing Feature Library and a Marketing Shell

**Date:** 2026-10-26 (trimmed 2026-10-30: the pitch video was dropped by [ADR general/0009](../general/0009-cloned-from-tutumuch.md) and returned the same day by [ADR platform/0014](0014-pitch-video.md))
**Category:** platform
**Status:** Accepted
**Deciders:** Quinntyne Brown (product owner)

## Context

The public marketing page (`/welcome`, `L1-020`) sells Buttonry to shoppers and sellers with
short copy, a hero photograph and the founder's story. It belongs to no existing feature area,
and the app project may not own feature screens (`AGENTS.md`, Frontend workspace layout). Its
full-bleed bands cannot sit in the app shell's `main.container.page`. Its images must be served
by the static host.

## Decision

- Add the `marketing` library: `src/frontend/projects/marketing/`, alias `@buttonry/marketing`,
  registered in `angular.json`, `tsconfig.json` and `build:libs`. It exports its lazy routes and
  holds `WelcomePage`. The app mounts it at `welcome`.
- Reusable pieces go to `@buttonry/components`: `bt-marketing-hero`, `bt-feature-band`,
  `bt-steps` and `bt-quote`.
- Add a `marketing` shell value: routes with `data.shell = 'marketing'` get the same top bar
  and footer but `<main id="main" class="marketing">` without the container.
- Page media live in `src/frontend/projects/buttonry/public/media/` (served at `/media/`):
  the hero photo `buttonry-hero.jpg` (also the video poster), the founder photo
  `buttonry-founder.jpg`, the 1200×630 share image `buttonry-share.jpg` and the pitch video
  `buttonry-pitch.mp4`, `buttonry-pitch.webm` and `buttonry-pitch.vtt`
  ([ADR platform/0014](0014-pitch-video.md)), with their sources and licences in
  `public/media/ATTRIBUTION.md`.

## Options Considered

### Option 1: New `marketing` library and shell value (chosen)
- **Pros:** Follows the one-library-per-area rule; the app stays shell-only; bands are full width.
- **Cons:** One more library to build.

### Option 2: Put the page in `catalog`
- **Pros:** No new library.
- **Cons:** Mixes a static sales page into the catalog area.

## Consequences

### Positive
- The page is lazy-loaded and calls no Buttonry API endpoint of its own.
- Three photographs and copy, plus one 29-second video that plays only on request; its build
  step lives in `docs/mocks/_build/video/`, not in the frontend build (ADR platform/0014).

### Negative
- `public/media/` adds committed images and the video; keep the images optimised (JPEG, under
  300 KB each) and the video short (under 30 s, a few MB).

### Risks
- Photographs change with the brand. Mitigation: `ATTRIBUTION.md` records source and licence so
  a swap is a file replacement.

## References

- [marketing-page design](../../detailed-designs/platform/marketing-page/README.md)
- [ADR general/0005](../general/0005-marketing-wording.md)
