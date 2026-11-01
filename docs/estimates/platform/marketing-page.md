# Estimate — Marketing page

## Basis

The hours assume an intermediate Angular/TypeScript developer who does not yet know the
Buttonry codebase. They cover implementation, automated tests and review fixes, and exclude
requirements, design, mock and photo work. They assume the app shell and `AuthState` exist.
The page, components and tests are built; the hero, founder and share photos are committed.
The pitch video (poster, player, dialog and the media build) was added on 2026-10-30
([ADR platform/0014](../../adr/platform/0014-pitch-video.md)).
[Detailed design](../../detailed-designs/platform/marketing-page/README.md)

## Full design

| Work item | Hours |
|-----------|------:|
| `marketing` library skeleton, lazy route, `marketing` shell, title and meta description | 4 |
| `bt-marketing-hero` on paper with the hero photo media slot; the terracotta and cream band tones | 6 |
| `bt-feature-band`, `bt-steps` and the page copy with CTA targets | 5 |
| `bt-quote` and the Our story band with the founder photo | 2 |
| Footer link, hero, founder and share images with `ATTRIBUTION.md` | 1 |
| `bt-video-poster`, `bt-video-player` and `PitchVideoDialog` (loading, error, captions, focus return) | 6 |
| Pitch video build (`build_video.py`: slides, Kokoro narration, Pixabay track, ffmpeg MP4 and WebM, captions, `ATTRIBUTION.md`) | 4 |
| Playwright page object and acceptance tests | 8 |
| Visual check at phone and desktop, light and dark, and fixes | 4 |
| **Total** | **40** |

## Minimal scope

The minimal version drops `bt-quote` (the Our story band is plain text with the founder
photo), the share image and the pitch video (the hero photo stays a plain photograph). Minimal
total: **26** hours.

## Partial descoping

| Descope | What is lost | Hours saved |
|---------|--------------|------------:|
| `bt-quote` component | Plain-text quote without the attribution layout | 2 |
| Share image and Open Graph tags | Link previews show no image | 1 |
| Founder photo | Our story band is text only | 1 |
| Pitch video, poster, player and dialog | The hero photo is a plain photograph; no video | 10 |
