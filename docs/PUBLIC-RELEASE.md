# Public GitHub release (V1.9+) — Pro Center preview

Use this checklist when you publish the next **public** installer. The app stays **fully free**; Pro Center is **visible but not runnable** for normal users until you flip the release flag after QA.

## What users see (default public build)

| Area | Behavior |
|------|----------|
| **Cleaner, Treiber, Netzwerk, Gaming, …** | Fully usable, free |
| **Pro Center** | 28 tools, category filter & search, **◷ Demnächst** / **Coming soon** |
| **Preview button** | Short description + “starts in a later update” |
| **Settings** | “Deine Edition / Vollversion” — no license key field |
| **Not on GitHub** | `dist/family/`, master keys, Family-only EXE |

## Before you upload

1. **Build public Release** (no Family keys):
   ```powershell
   .\scripts\build\build-release.ps1 -Version 1.9
   ```
2. **Verify package**:
   ```powershell
   .\scripts\release\verify-public-package.ps1 -Version 1.9
   ```
3. **Self-tests**:
   ```powershell
   dotnet .\GamingBooster_Pro\bin\Release\net10.0-windows\GamingBooster_Pro.dll --selftest
   dotnet .\GamingBooster_Pro\bin\Release\net10.0-windows\GamingBooster_Pro.dll --pro-selftest
   ```
4. **Confirm gating** in `RedlineFeatureGate.cs`:
   - `ProHubReleasedForCustomers = false` ← public preview
   - `ProGatingEnabled = false` ← rest of app stays free
5. **Update `version.json`** (in-app update + download URL):
   ```json
   "version": "1.9",
   "downloadUrl": "https://github.com/LegendR622/Redline-Gaming-Optimizer/releases/download/v1.9/Redline_Gaming_Optimizer_Setup_v1.9.exe"
   ```
6. **Website** — `docs/index.html`, `docs/changelog.html` (download links + “Pro Center preview” text)
7. **Publish**:
   ```powershell
   .\scripts\release\publish-github-clean.ps1 -Version 1.9
   .\scripts\release\push-public-main.ps1
   ```

## When Pro Center is ready for everyone

1. Finish QA on Family/dev build (`--pro-live-test` → 28 OK).
2. Set `ProHubReleasedForCustomers = true` in `RedlineFeatureGate.cs`.
3. New release notes: “Pro Center — features can now be started”.
4. Rebuild and publish **V1.10** (or next version).

## Release notes snippet (copy for GitHub)

**English**

> **Pro Center (preview)** — Browse **28** Pro tools (DNS benchmark, registry read, GPU scheduling, FPS boost, and more). They show **Coming soon** in this release; running them will arrive in a later update. The rest of Redline stays **fully free and usable**.

**Deutsch**

> **Pro Center (Vorschau)** — **28** Pro-Werkzeuge ansehen (DNS-Benchmark, Registry-Lesen, GPU-Planung, FPS-Boost, …). In diesem Release **Demnächst** — Starten folgt in einem späteren Update. Der Rest von Redline bleibt **voll nutzbar und kostenlos**.
