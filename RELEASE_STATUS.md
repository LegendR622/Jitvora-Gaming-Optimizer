# Redline Gaming Optimizer — QA Report & Release Status (V1.9.3)

**Date:** 2026-06-12  
**QA pass:** Full deep QA (automated + code review + safe host checks)  
**Published online:** Yes — asset refresh (same version, new build bytes)

---

## 1. Test environment

| Item | Result |
|------|--------|
| **Host** | TOBI-PC (Windows 10.0.26200, x64) |
| **VM / Hyper-V** | **Not available** (`Get-VM` unavailable; no test VM configured) |
| **Risky system actions** | **Not executed** on host (Cleaner delete, SFC, DISM, Winsock, driver install, registry writes, power plan, autostart disable) |
| **Safe checks used** | `--selftest`, `--pro-selftest` (dry-run), code review of handlers/confirmations, installer silent install (no destructive actions) |

---

## 2. Build & installer

| Check | Result |
|-------|--------|
| Release build | **OK** (Obfuscar, 0 compile errors) |
| Installer | **OK** — `Redline_Gaming_Optimizer_Setup_v1.9.3.exe` |
| Public EXE name | **OK** — only `Redline Gaming Optimizer.exe` |
| No User/Family/Pro/Developer public EXE | **OK** |
| Version shown | **OK** — 1.9.3 |
| Code signing | **Skipped** (PFX password incorrect — unsigned, known) |
| ZIP public package | **OK** — no source/bin/obj |
| `--selftest` (publish EXE, offline) | **271 OK, 1 FAIL** (Authenticode only) |
| Installer silent install | **OK** — single EXE in temp dir |
| Post-install `--selftest` in smoke script | **Hung on dev PC** (known; install itself OK) |

**SHA256 (current public Setup):**
```
290c1024ba5ac258598635026841e34f91aa4bf70d849dc6556d6a91b1db64b1
```

**Desktop folder:** `C:\Users\Tobi\Desktop\Redline_Release_Latest`

---

## 3. Pages tested (code + navigation review)

All 13 sidebar pages have builders and route handlers — **no crash/null-ref patterns found**:

Dashboard · AI Advisor · Gaming · Pro Center · Performance · Cleaner · Autostart · Security · Network · Drivers · Repair · Update · Settings

**Layout/resolution:** Not manually clicked at 1366/1400/1920/125% in this pass (no VM/GUI automation). Code uses scroll viewers and responsive patterns; **needs manual UI pass** for pixel-perfect layout.

---

## 4. Buttons / actions (automated + review)

| Area | Result |
|------|--------|
| Handlers present | **OK** — no empty primary action handlers found |
| Risky actions with confirmation | **OK** — Cleaner, SFC, DISM, Winsock, DNS preset, drivers, repair-all, autostart disable, update install |
| **DNS flush standalone** | **Fixed** — Network + Repair tiles now use `FlushDNSWithConfirm()` |
| Direct perf tile (Game Mode / power plan) | **No extra confirm** (low risk registry/plan; wizard path still confirms) — **needs VM manual test** |
| Pro Center (public) | **Preview only** — Run locked; filters/search wired |

---

## 5. Language QA (DE / EN)

| Check | Result |
|-------|--------|
| Core messages (`RedlineUserMessages`) | **OK** |
| Website i18n (27 locales) | **OK** — audit 87/87 keys |
| Minor hardcoded EN in DE UI | **Known** — e.g. `GAME MODE` tile label, `Trust`/`Rollback` settings buttons (cosmetic) |
| Developer text for normal users | **OK** — gated to authorized dev PC only |
| Family/User EXE wording | **Not in public UI** |

---

## 6. Live Log QA

| Check | Result |
|-------|--------|
| User-facing filter | **OK** |
| Exception sanitization | **OK** |
| **Copy/export filter** | **Fixed** — `GetPlainText()` now respects same filter as UI |
| **Duplicate lines** | **Fixed** — consecutive identical stamped lines suppressed |
| Popups vs log-only success | **OK** by design (review) |

---

## 7. Hardware detection

| Check | Result |
|-------|--------|
| CPU / GPU / RAM / Windows / board / disk / adapter / DNS / ping | **OK** in `RedlineSystemInfoService` |
| No serial / MAC / UUID in UI | **OK** |
| Refresh | **Code OK** — **manual UI test recommended** |

---

## 8. Safety & rollback

| Action | Confirmation | Notes |
|--------|--------------|-------|
| Cleaner delete | Yes + restore point option | Not executed on host |
| Repair (SFC/DISM/Winsock/Store) | Yes | Not executed on host |
| DNS flush (standalone) | **Yes (new)** | Not executed on host |
| Driver install | Yes + UAC | Not executed on host |
| Update install | Manual confirm | Not executed on host |

---

## 9. Update / release pipeline

| Check | Result |
|-------|--------|
| Single public EXE in installer | **OK** |
| `version.json` | **OK** — v1.9.3 setup URL |
| GitHub release asset | **Updated** (same tag, new bytes) |
| Website download | **OK** — https://legendr622.github.io/Redline-Gaming-Optimizer/ |
| `trust-latest.json` | **Updated** with new SHA256 |
| Auto-install without user | **No** |

---

## 10. Website QA (live)

| Page / check | Result |
|--------------|--------|
| Landing | **OK** (200) |
| DE/EN + 25 languages | **OK** |
| Download / GitHub / Trust / Changelog / legal pages | **OK** |
| Pro Center on website | Preview copy only (28 tools) |
| Mobile / overflow | **Not fully automated** — spot-check recommended |

---

## 11. Secrets / protection scan

| Check | Result |
|-------|--------|
| Public repo docs/version/trust | **No API keys, tokens, passwords** |
| Selftest secret audit | **OK** |
| `secrets/` folder | Local only (gitignored) |
| Dev license hashes in public EXE | **OK** — disabled |

---

## 12. Bugs found & fixed (this QA pass)

| Bug | Fix |
|-----|-----|
| Performance score refresh used wrong nav key `"Optimization"` | → `"Optimierung"` |
| AI Advisor could stay locked if exception during run | `try/finally` on `_advisorBusy` |
| DNS flush from Network/Repair had no confirmation | `FlushDNSWithConfirm()` |
| Live Log copy/export leaked technical filtered lines | Filter `GetPlainText()` |
| Consecutive duplicate log lines | Suppress in `Append()` |
| `write-github-release-notes.ps1` parse error (Unicode dash) | ASCII-safe string |
| `verify-public-package.ps1` false fail exit code | explicit `exit 0` |

**Not changed:** Pro Center UI polish (local only, not in public release scope).

---

## 13. Tests skipped (safety)

- Cleaner real delete · Repair SFC/DISM/Winsock · Driver install/update · Registry/power-plan live toggles · Autostart disable · Network stack changes · Pro feature execution (customer build locked)
- **Reason:** No VM; destructive/system-changing actions blocked on main PC per safety rule.

---

## 14. Manual testing still recommended

- [ ] Full GUI click-through all pages at 1366×768, 1920×1080, 125% scaling
- [ ] Cleaner scan + delete on **VM**
- [ ] SFC/DISM on **VM** (long-running)
- [ ] Download from live site → install on clean PC
- [ ] SmartScreen unsigned flow
- [ ] Friend re-test install

---

## 15. Online status

| Item | Value |
|------|--------|
| **Live** | **Yes** |
| **Version** | 1.9.3 (asset refresh) |
| **Release** | https://github.com/LegendR622/Redline-Gaming-Optimizer/releases/tag/v1.9.3 |
| **Website** | https://legendr622.github.io/Redline-Gaming-Optimizer/ |

Existing users on 1.9.3 will not see an in-app update prompt (same version number; bytes replaced intentionally).
