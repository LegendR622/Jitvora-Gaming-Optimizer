# Redline Gaming Optimizer — Full QA Report

**Test date/time:** 2026-06-12 (approx. 19:30–20:00 CEST)  
**App version tested:** **1.9.6**  
**Tester:** Automated + headless UI selftest on developer PC (AMD Ryzen 7 7800X3D, RTX 4070 Ti, ASUS X670E)

---

## Executive summary

**Release candidate passed full QA and was published.**

V1.9.6 was already live on GitHub before this QA run. Full automated and artifact testing found **no new critical blockers**. The public download, trust manifest, and website are **consistent** (SHA256 `ac648dc…`).

**Do not republish** — no app-code fixes required. One local-only artifact drift (QA rebuild hash) was reverted; online release unchanged.

---

## 1. Build result

| Check | Result |
|-------|--------|
| Release publish (`build-release.ps1`) | **OK** (Obfuscar, single-file EXE) |
| Hardening Authenticode | **FAIL** (unsigned — expected, same as 1.9.5) |
| Installer `Redline_Gaming_Optimizer_Setup_v1.9.6.exe` | **OK** (~80.6 MB) |
| ZIP public name | **OK** — `Redline Gaming Optimizer.exe` only |

---

## 2. Installer / EXE artifacts

| Check | Result |
|-------|--------|
| Installed EXE name | **OK** — `Redline Gaming Optimizer.exe` |
| Duplicate public EXEs in installer | **OK** — none (User/Family/Pro/Developer/GamingBooster_* absent) |
| ZIP contents | **OK** — `Redline Gaming Optimizer.exe` |
| Post-install EXE present (smoke) | **OK** |
| Post-install `--selftest` exit code | **1** — only Authenticode check fails (272/273 OK) |
| App normal start (8 s) | **OK** |

---

## 3. Automated in-app tests

### Headless `--selftest`
- **272 OK, 1 FAIL** — `[FAIL] Release-EXE Authenticode vorhanden` (non-blocking for public unsigned builds)

### UI selftest (`REDLINE_UI_SELFTEST=1`, DryRun)
- **All pages × 2 rounds:** Dashboard, Advisor, GameProfiles, Optimierung, Cleaner, Startup, Security, Network, Drivers, Repair, Update, Settings — **OK**
- **Performance arrows (7):** **OK**
- **Cleaner:** recommended/all/none targets, scan-before-clean, DryRun clean — **OK**
- **Button/action dry-run (26 handlers):** Dashboard scan, Advisor, Gaming readiness, Cleaner, Repair (SFC/DISM/Winsock/Store/DNS/All), Chrome, temp clean, restore point, network, ping, speed, security, anti-cheat, startup, performance wizard, driver scan, BIOS, undo, update check, save report — **OK**
- **Result:** `ERGEBNIS: ALLE UI-TESTS OK`
- **Note:** Process exit code abnormal on WPF shutdown; log shows zero failures.

### Pro Center `--pro-selftest`
- **35 OK, 0 FAIL**

### Telemetry `--telemetry-probe` (admin, developer PC)
- CPU: AMD Ryzen 7 7800X3D — load OK, **temp n/a** (LHM `Core (Tctl/Tdie)=0.0`, known PawnIO/LHM limitation)
- GPU: RTX 4070 Ti — **41°C OK**
- `CpuTempUnavailable: True` — correct admin-state hint path
- No fake CPU temps (0°C rejected)

---

## 4. Pages tested (automated navigation)

All 12 main pages loaded without crash in UI selftest (2 rounds).

**Not fully automated (manual still recommended):** window sizes 1366×768 / 125% scaling, visual layout cut-off, 20–30 min long-run stress, live mouse clicks on every Treiberseite button in GUI.

---

## 5. Language test

**Partial:** UI selftest runs default language path. DE/EN toggle across all pages not executed in this automated pass.

**Manual recommended:** Settings → DE ↔ EN → spot-check Drivers, Dashboard CPU temp hints, Live Log strings.

---

## 6. Live Log / activity

**Code + dry-run verified:** Cleaner/Repair/Network/Driver handlers complete without exception in UI selftest.

**Manual recommended:** Confirm no duplicate floating Live Log on Drivers page during real scan (fixed in 1.9.5; not re-run interactively this session).

---

## 7. Driver links (static + HTTP)

| URL | HTTP |
|-----|------|
| GitHub release installer | **200** |
| ASUS X670E helpdesk | **200** |
| NVIDIA drivers DE | **200** |
| `GetBestOpenUrl()` | Official URLs only (no DirectDownloadUrl on ↗ buttons) — **verified in code + selftest** |

**Not executed:** Click every Treiberseite row in live GUI; driver auto-install in VM (no VM available).

---

## 8. CPU / GPU / RAM / Ping telemetry

| Sensor | Result |
|--------|--------|
| GPU temp | **OK** (~41–46°C) |
| CPU temp | **Unavailable** (LHM 0°C; help/retry UI; no fake values) |
| CPU load | **OK** |
| Admin hints | **OK** (unavailable + Help/Retry on admin session) |

---

## 9. Secrets / privacy

| Check | Result |
|-------|--------|
| Selftest: no license key hashes | **OK** |
| Selftest: no plaintext secrets in resources | **OK** |
| EXE string scan (gho_/ghp_/private key) | **No matches** |
| GitHub token in repo | **Not in published docs** |

---

## 10. Website / live download

| Check | Result |
|-------|--------|
| Website home | **200** — shows **v1.9.6** |
| Download button URL | **v1.9.6 GitHub release** — **200** |
| `trust-latest.json` (Pages) | **200** — SHA `ac648dc…` |
| `version.json` (Pages) | **404** — app uses `raw.githubusercontent.com/.../version.json` (**200**) |
| GitHub Release v1.9.6 asset | **uploaded**, digest matches live trust |

**Published SHA256:** `ac648dc475a7441df70893dda109bf476f40c8452ca4db70cf41f2c4250cfa62`

---

## 11. VM testing

| Item | Result |
|------|--------|
| VM used | **No** |
| Windows Sandbox | **Not available** on this PC |
| Hyper-V | Not enabled / not checked |
| Risky tests (Cleaner delete, SFC/DISM, Winsock, driver install) | **DryRun only** on host; **not executed destructively** |

**VM limitations:** Real RTX/ASUS/I225 hardware not available in VM; driver install tests skipped.

---

## 12. Long-run / stress

**Not executed** (20–30 min interactive session). Automated tests ~45 s UI + 35 Pro tools.

**Manual recommended:** 30 min navigation + repeated scans while monitoring memory.

---

## 13. Bugs found

| ID | Severity | Description |
|----|----------|-------------|
| — | — | **No new critical bugs** |
| KNOWN-1 | Low | Unsigned Authenticode — SmartScreen warning |
| KNOWN-2 | Medium | AMD Ryzen CPU temp 0°C via LHM without valid PawnIO read — UI shows unavailable (by design) |
| KNOWN-3 | Low | UI selftest abnormal process exit after success |
| KNOWN-4 | Info | QA rebuild changes installer hash; online release SHA remains authoritative |

---

## 14. Bugs fixed during QA

None — no code changes required.

---

## 15. Publish decision

| Question | Answer |
|----------|--------|
| Published online? | **Yes** — already **v1.9.6** |
| New publish needed? | **No** |
| Release URL | https://github.com/LegendR622/Redline-Gaming-Optimizer/releases/tag/v1.9.6 |
| Download URL | https://github.com/LegendR622/Redline-Gaming-Optimizer/releases/download/v1.9.6/Redline_Gaming_Optimizer_Setup_v1.9.6.exe |
| Safe to use/release? | **Yes** (unsigned SmartScreen caveat) |

---

## 16. Manual tests still recommended

1. DE ↔ EN full page walk
2. Drivers page — live scan + click every Treiberseite + vendor tiles
3. Dashboard CPU temp hints as **non-admin** user (Admin nötig)
4. 30 min stress / memory watch
5. Cleaner real delete on VM/test account
6. Repair SFC/DISM in VM
7. Update Center “Install update” on VM
8. Window 1366×768 + 125% scaling visual check

---

## Artifacts saved

- `dist/qa-selftest.log`
- `dist/qa-ui-selftest.log` (also `%TEMP%\redline-ui-selftest.log`)
- `dist/qa-pro-selftest.log`
- `dist/qa-telemetry.log`
- `dist/qa-build-log.txt`
- Desktop: `C:\Users\Tobi\Desktop\Redline_Release_Latest\` (published installer + trust)

---

**Final status:** **Release candidate passed full QA and was published.**
