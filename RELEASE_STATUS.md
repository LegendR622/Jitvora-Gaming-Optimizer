# Redline Gaming Optimizer — Release Status (V1.9.3)

**Date:** 2026-06-12  
**Status:** Published online (asset replaced, same version)

---

## Summary

| Item | Value |
|------|--------|
| **Published online?** | **Yes** |
| **Final version** | **1.9.3** (unchanged — QA/polish only) |
| **Installer filename** | `Redline_Gaming_Optimizer_Setup_v1.9.3.exe` |
| **Installed EXE name** | `Redline Gaming Optimizer.exe` |
| **GitHub release** | https://github.com/LegendR622/Redline-Gaming-Optimizer/releases/tag/v1.9.3 |
| **Website** | https://legendr622.github.io/Redline-Gaming-Optimizer/ |
| **Desktop folder** | `C:\Users\Tobi\Desktop\Redline_Release_Latest` |

---

## What changed in this publish

**Same version (1.9.3)** — replaced the public GitHub release asset with a rebuilt installer containing:

- User-facing message QA (Live Log, dialogs, status colors, hardware card)
- Consistent running/success/warning/error wording (EN/DE)
- No duplicate popup + Live Log for normal success actions
- Sanitized error messages (no raw exceptions to users)

**Not changed:** release pipeline scripts, installer layout, website design, version number.

---

## Pre-publish verification

| Check | Result |
|-------|--------|
| Release build | OK (Obfuscar, 0 compile errors) |
| Public package verify | OK |
| Installer silent install | OK — only `Redline Gaming Optimizer.exe` |
| Single public EXE rule | OK |
| Family/Pro EXE on GitHub | None (family builds stay in `dist/family/` locally) |
| `version.json` | Points to v1.9.3 setup URL |
| Code signing | Skipped (PFX password incorrect — unsigned, same as prior publish) |
| `--selftest` in smoke script | Hung on dev machine (known); publish EXE selftest exit 0/1 in hardening |
| Secrets in public repo | None in docs/trust/version.json |
| Website screenshots | CSS mockup kept (no outdated dev screenshots) |

---

## SHA256 (V1.9.3 Setup — current asset)

```
6ecca0643d0dd97da28d3dad7b8e5a27d8d68f7284141ff1d9f0962445b7cbfc
```

Previous asset SHA (replaced): `073269374be86e192a918cd924e3c0626ef2a2a1306d10137169fb221d17b288`

---

## GitHub / website

- Release asset **replaced** via `gh release upload v1.9.3 --clobber` (no new version tag)
- `docs/trust-latest.json` updated with new SHA256
- Public `main` pushed (docs + trust manifest + this file)

---

## Desktop output (`Redline_Release_Latest`)

| File | Purpose |
|------|---------|
| `Redline_Gaming_Optimizer_Setup_v1.9.3.exe` | Public installer |
| `Redline Gaming Optimizer.exe` | Portable copy (from publish output) |
| `TRUST_v1.9.3.json` | SHA256 trust manifest |

---

## Manual testing still recommended

- [ ] Download from live website → install on a clean PC
- [ ] Confirm SmartScreen “Run anyway” flow (unsigned)
- [ ] System scan + dashboard optimize → Live Log only, no success popup
- [ ] Settings → Hardware refresh → one clear log line
- [ ] Friend re-test install (same v1.9.3 URL, new build bytes)

---

## Notes

- In-app update check compares version **number** (still 1.9.3) — existing users on 1.9.3 will not get an auto-update prompt unless they re-download. That is intentional for a same-version asset refresh.
- When OV/EV code signing is configured, rebuild and replace the asset again (or ship 1.9.4 if update trigger is needed).
