# ADR-0019: Backend and frontend root folders

**Date:** 2026-11-01
**Category:** platform
**Status:** Accepted
**Deciders:** Quinntyne Brown (product owner)

## Context

The .NET solution lived in `src/backend/`, its test projects in a root `tests/` folder, and the
Angular workspace in `src/frontend/`. Backend code was split across two root folders, and the
layout differed from the owner's other repository, Saturdaze, which keeps each stack in its own
root folder.

## Decision

- **`backend/`** holds everything .NET: `Buttonry.sln`, `Directory.Build.props`, `global.json`,
  `.editorconfig` and `.dockerignore` at its root, the projects under `backend/src/Buttonry.*`
  and the test projects under `backend/tests/Buttonry.*.Tests`.
- **`frontend/`** holds the Angular workspace unchanged (`angular.json`, `package.json`, `e2e/`,
  `projects/`).
- `src/` and `tests/` are gone. `npm run verify` runs from `frontend/`; the backend image builds
  with context `backend/`; the app and marketing images copy `frontend/`.

## Options Considered

### Option 1: Keep `src/backend` and `tests/`
- **Pros:** No change.
- **Cons:** Backend code in two root folders; differs from Saturdaze.

### Option 2: Move only `tests/` into `src/backend/tests/`
- **Pros:** Smaller change.
- **Cons:** Still differs from Saturdaze; `src/` would hold only two folders.

### Option 3: `backend/{src,tests}` and `frontend/` at the root, as in Saturdaze (chosen)
- **Pros:** Each stack has one folder; the same layout in both repositories.
- **Cons:** Touches CI, the Dockerfiles and every document that names a path.

## Consequences

- Every path in `AGENTS.md`, the README, the designs and the videos uses `backend/src/...`,
  `backend/tests/...` or `frontend/...`.
- Test projects reference their project as `..\..\src\Buttonry.<Project>`; the shared
  `Directory.Build.props` in `backend/` applies to both `src/` and `tests/`, so the old
  `tests/Directory.Build.props` and `tests/.editorconfig` were removed.

## References

- `AGENTS.md`, Folder structure
- [ADR platform/0002](0002-single-verify-command.md)
- [ADR platform/0015](0015-separate-marketing-and-application-deployments.md)
