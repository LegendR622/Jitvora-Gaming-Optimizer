# Agent workflow — Redline Gaming Optimizer

## Project path (source of truth)

`C:\Users\Tobi\Desktop\Redline-Gaming-Optimizer`

Workspace `ba` is not the app — open/use the Desktop project above.

## Pause (2026-06-13)

**Active fix session** — VM QA bugs from v1.9.6 being addressed. See `docs/QA-FIX-PLAN.md` for plan and implementation log. Builds/selftest per fix; no release/deploy until Tobias approves.

## Builds

| Build | Script | Output |
|-------|--------|--------|
| **Public** (GitHub) | `.\scripts\build\build-release.ps1 -Version X.Y` | `build\` (App + Setup) · `dist\` nur intern |
| **Family** (local only) | `.\scripts\build\build-family-release.ps1 -Version X.Y` | `build\` · Family-Setup intern |

- Sign: `secrets\redline-codesign.pfx` + `secrets\codesign-password.txt` (or env `REDLINE_CODE_SIGN_*`).
- **Family must not** update `docs/trust-latest.json` or `version.json` (public customers).
- Desktop: only **`Redline_Gaming_Optimizer_Setup_Family.exe`** for Tobias (not two EXEs).

## Public vs Family

- **Public**: `REDLINE_PUBLIC_RELEASE`, Pro Center preview, no master keys.
- **Family**: `REDLINE_FAMILY_RELEASE`, dev PC (TOBI-PC) auto developer Pro, master key hashes in EXE.
- Footer: `vX.Y · Family` vs `vX.Y` — check before debugging “Coming soon”.

## GitHub repo

- Remote `main` is mostly **docs + releases**; `GamingBooster_Pro/` and `scripts/` are gitignored on purpose.
- Upload **only** public Setup EXE to Releases, never Family.

## Code layout (for edits)

- `GamingBooster_Pro/MainWindow.*.cs` — partial MainWindow (pages, actions, UI).
- `GamingBooster_Pro/Features/Pro/` — Pro Center catalog, executors, tests.
- `GamingBooster_Pro/Features/Pro/ProHub/` — Pro Center UI (`ProHubPageView`, palette).
- `RedlineFeatureGate.cs` — preview vs run, licensing flags.

## Release hardening

- Obfuscar: rename private API, hide strings, **no control-flow** (Obfuscar limit).
- After build: `.\scripts\build\verify-release-hardening.ps1 -Version X.Y`
- Installer: `.\scripts\build\test-installer-smoke.ps1 -Version X.Y` (or `-SkipInstallerSmoke` on build)
- Cleanup: `.\scripts\maintenance\cleanup-project.ps1` (bin/obj, temp logs, archiv old releases)
- Website i18n: `python docs/i18n/generate-i18n.py` then `python docs/i18n/audit-i18n.py`

## Tests

```powershell
.\build\Redline Gaming Optimizer.exe --selftest
.\build\Redline Gaming Optimizer.exe --pro-selftest
```

## USB backup

Mirror work project to `Desktop\Redline\USB-Backup` via `AKTUALISIERE-BACKUP.bat` — not the other way around.
