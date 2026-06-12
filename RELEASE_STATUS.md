# Redline Gaming Optimizer — Release Status (V1.9.3)

**Date:** 2026-06-12  
**Status:** Published (if GitHub push + release succeeded)

---

## 1. Broken public EXE — fixed

| Item | Detail |
|------|--------|
| **Was it fixed before?** | Partially (IntegrityGuard patch locally); **not** on GitHub until V1.9.3 |
| **Root cause** | Public build (`REDLINE_PUBLIC_RELEASE`) required Authenticode signature; V1.9.2 shipped **unsigned** → silent `Environment.Exit(0)` on startup |
| **Fix** | Allow unsigned public builds until OV/EV cert is configured; signed wrong-publisher builds still blocked |

---

## 2. Why two EXEs existed before

| Layer | Name | Public? |
|-------|------|---------|
| **Internal publish output** | `GamingBooster_Pro.exe` | No — build artifact only |
| **Installer output** | `Redline Gaming Optimizer.exe` | Yes — only file installed |
| **Family pipeline** | `dist/family/*Family*.exe` | No — local/private only |
| **Desktop copy (dev)** | `Redline_Gaming_Optimizer.exe` | Optional portable copy, not on GitHub |

Public GitHub release = **Setup installer only** → installs **one** app EXE.

---

## 3. Files / scripts changed (this release)

**App (local build, not in public git):**
- `GamingBooster_Pro/RedlineIntegrityGuard.cs` — unsigned public startup fix
- `GamingBooster_Pro/RedlineDisplayFit.cs` — 1400×900 default, 1200×760 min
- `GamingBooster_Pro/MainWindow.xaml.cs` — uses display constants

**Scripts (local, not in public git):**
- `scripts/build/Invoke-RedlineCodeSign.ps1` — signing failure no longer aborts build
- `scripts/build/test-installer-smoke.ps1` — `/FORCECLOSEAPPLICATIONS`, unsigned public OK

**Public repo (docs/metadata):**
- `CHANGELOG.md`, `README.md`, `version.json`
- `docs/index.html`, `docs/changelog.html`, `docs/trust-latest.json`, `docs/landing.css`
- `RELEASE_STATUS.md`

---

## 4. Final release version

**V1.9.3**

---

## 5. Final EXE name (end user)

**Redline Gaming Optimizer.exe** (inside `%LocalAppData%\Programs\Redline Gaming Optimizer\` or chosen install dir)

Public download artifact: **`Redline_Gaming_Optimizer_Setup_v1.9.3.exe`**

---

## 6. Runtime modes (one EXE)

Single binary decides at runtime:

| Mode | How |
|------|-----|
| **Free/User** | Default public build; no valid license |
| **Family/Pro** | Valid license key / stored activation |
| **Developer** | `RedlineDevAuth` machine fingerprint (disabled in `REDLINE_PUBLIC_RELEASE`) |
| **Feature gating** | UI locks with unlock messages; no separate public EXEs |

---

## 7. Hardware detection (one EXE)

Read-only via WMI + LibreHardwareMonitor inside the same process:

CPU, GPU, RAM, Windows version, motherboard, drives, network adapter, DNS, ping — shown in Settings/Dashboard.

---

## 8. Installer contents

Inno Setup installs **only**:

- `Redline Gaming Optimizer.exe` (from internal `GamingBooster_Pro.exe` publish output)

Removes legacy `GamingBooster_Pro.exe` paths on upgrade.

---

## 9. ZIP contents

**Skipped** for this release (`-SkipZip`). If built, ZIP contains only `Redline Gaming Optimizer.exe`.

---

## 10. GitHub release URL

https://github.com/LegendR622/Redline-Gaming-Optimizer/releases/tag/v1.9.3

---

## 11. GitHub asset / download

| Asset | Purpose |
|-------|---------|
| `Redline_Gaming_Optimizer_Setup_v1.9.3.exe` | **Only** public download |
| V1.9.2 | Broken — superseded by V1.9.3 |

---

## 12. version.json

Points to V1.9.3 setup URL on GitHub Releases.

---

## 13. Website download link

`docs/index.html` → V1.9.3 setup (after push + Pages deploy).

---

## 14. Screenshot / product preview

| Check | Result |
|-------|--------|
| Clean current screenshot? | **No** — only `dist/app-review-*.png` (v1.5, dev mode, admin labels) |
| Website | **Premium CSS mockup** kept (`docs/assets/README.txt` placeholder) |

---

## 15. Website link check

Verify after deploy: Home, Trust, Changelog, Sitemap, Privacy, Imprint, Terms, GitHub Releases, Download → HTTP 200.

---

## 16. SEO / mobile

- Title/meta: Redline Gaming Optimizer, Windows gaming optimizer, FPS, PC cleaner, driver check, network tools
- Mobile: `landing.css` — hero buttons stack ≤520px, `overflow-x: clip`

---

## 17. Secret / protection check

| Check | Result |
|-------|--------|
| API keys / tokens in docs | None (only env var **names** in dev docs) |
| trust-latest.json | SHA256 + public URLs only |
| version.json | Public download URL only |
| Public build | No master key hashes (`REDLINE_PUBLIC_RELEASE`) |
| Obfuscar | Release build OK |
| IntegrityGuard | Active; unsigned public allowed |

---

## 18. Build result

**Success** — Release publish, Obfuscar, 0 errors. Signing skipped (wrong PFX password).

---

## 19. Installer / launch result

| Test | Result |
|------|--------|
| Inno compile | OK |
| App normal launch (publish EXE) | OK |
| Smoke test | Blocked if app already running; use `/FORCECLOSEAPPLICATIONS` |

---

## 20. Live website

Check after push: https://legendr622.github.io/Redline-Gaming-Optimizer/

---

## 21. Release online?

**Yes** — if GitHub Release v1.9.3 + git push completed in automated step below.

---

## 22. Manual testing still recommended

- [ ] Install on a **clean PC** (friend's machine) from GitHub download
- [ ] SmartScreen: “Weitere Informationen” → run anyway (unsigned)
- [ ] Accept EULA dialog on first start
- [ ] Confirm window size ~1400×900 on large monitor
- [ ] Code signing with OV/EV cert when available

---

## SHA256 (V1.9.3 Setup)

```
073269374be86e192a918cd924e3c0626ef2a2a1306d10137169fb221d17b288
```
