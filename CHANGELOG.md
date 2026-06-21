# Changelog

All notable changes to **Jitvora Gaming Optimizer** are listed here (newest first).

**Writing rules (for releases & GitHub notes):**
- Use **English** for all new entries (`### Fixed`, `### Improved`, `### Added`, `### Changed`).
- Put **bug fixes** under `### Fixed` (e.g. “CPU temperature on AMD Ryzen”).
- Use `### Improved` for UX/performance, `### Added` for new features, `### Changed` for behavior changes.
- Version format: **2.0**, **2.1** (semantic major/minor).

---

## [3.0.5] — 2026-06-21 (local build — source gitignored on GitHub)

### Added
- **Safe Visual Selftest** — read-only UI walkthrough with screenshots; Apply/Reconnect blocked on main PC (`--safe-visual-selftest`, Settings button)

### Improved
- **Network Adapter Gaming Profile** — “Apply Recommended Settings” stays available when scan reports 0 changes but force-applicable registry values exist (Intel I225-V / unsupported rows); confirmation dialog explains force mode
- **Network Adapter Gaming Profile** — manual adapter switch via “Wählen/Select” without requiring a restore script first; backup/restore script on Apply or “Create Restore Script” when changes are planned
- **Network Adapter Gaming Profile** — admin restart passes `--elevated-network-adapter {InterfaceIndex}` and resumes scan on the Network page

### Fixed
- **Network Adapter Gaming Profile** — elevated admin restart no longer loses the selected adapter index

---

## [3.0.4] — 2026-06-21

### Fixed
- **Network Adapter Gaming Profile** — manual “Wählen/Select” now switches adapters reliably (no auto-fallback override); apply uses interface index instead of adapter name
- **Website i18n** — fixed mojibake in translations (`·`, `—`, umlauts); regenerated `site-i18n.js`

---

## [3.0.3] — 2026-06-21

### Fixed
- **Installer** — EULA no longer shows outdated “Version 1.8”; welcome and legacy-cleanup messages use Jitvora branding instead of Redline
- **Network Adapter Gaming Profile** — Apply button works after scan (no longer requires a separate preview/restore step before apply)

---

## [3.0.2] — 2026-06-21

### Fixed
- **Network Adapter Gaming Profile** — robust multi-stage adapter detection (default route, gateway fallback, physical Ethernet fallback); fixes false “no active adapter” when PowerShell script was broken by one-line mangling

### Improved
- **Network Adapter Gaming Profile** — “Already optimal” state with green status, full preview table when 0 changes, honest export/restore behavior, disconnected adapter labels, Apply disabled with tooltip when nothing to change

---

## [3.0.1] — 2026-06-21

### Added
- **Network Adapter Gaming Profile** — scan active adapter via default route, preview safe gaming advanced properties, restore script, and apply with confirmation (no DNS/router/firewall changes)

### Improved
- **Network page** — Intel I225-V oriented gaming profile with honest before/after reports and IPv6 left enabled by default

---

## [3.0] — 2026-06-21 (Stable)

### Fixed
- **Dashboard** — no fake ping, live logs, scan time, or score before a real scan; honest placeholders (N/A, Not measured, Not scanned yet)
- **Update page** — honest offline/unreachable state; progress bar hidden when online check fails
- **GitHub update check** — app reads `version.json` from Jitvora-Gaming-Optimizer repo (legacy Redline URLs still accepted)

### Improved
- **Jitvora UI** — sidebar redesign (no chevrons), custom title bar, dashboard polish, cyan/green theme, RAM/live-system icons
- **Drivers page** — honest vendor labels (“Open official page”); header no longer claims automatic install
- **Last scan / perf hints** — real dates (Today/Yesterday/actual date), not always “Today”
- **Website & GitHub** — release metadata, trust manifest, and download links aligned to v3.0 Stable

### Changed
- **Public version** — v3.0 Stable replaces v2.0.0 as the recommended public download

---

## [2.0.0] — 2026-06-17

### Changed
- **Public branding** — product, website, GitHub repo, and release assets now use **Jitvora Gaming Optimizer**
- **GitHub Pages** — new site URL under `Jitvora-Gaming-Optimizer`
- **Trust manifest** — download metadata and SHA256 for v2.0.0 installer

### Improved
- **Website** — premium homepage, multilingual public i18n, sitemap and robots for new URL
- **Network Watch** — clearer presentation and wording (no game file modifications)

---

## [1.9.6] — 2026-06-12

### Fixed
- **Dashboard** — AMD Ryzen CPU temperature no longer shows invalid readings (0°C / 1°C); only valid sensor values (10–125°C) are displayed

### Improved
- **Dashboard** — clearer CPU temp hints for AMD Ryzen (admin required / unavailable), short UI text, Help and Retry buttons, official LibreHardwareMonitor info link only (no auto-install)

---

## [1.9.5] — 2026-06-12

### Fixed
- **Drivers** — vendor ↗ button opens official support pages instead of direct ZIP/EXE downloads
- **Drivers** — AMD High Definition Audio on ASUS boards opens the board helpdesk download page
- **Drivers** — no duplicate floating Live Log on the Drivers page; activity no longer stuck on “Working…”
- **Drivers** — winget verification no longer shown as “Updated”; verified devices use a clear “Verified” status
- **Drivers** — auto-install only runs on verified hardware matches; unsafe cases skip install and open the vendor page

### Improved
- **Drivers UI** — clearer status pills, “Driver page” buttons, vendor cards (“Driver page →”), and device-type icons (EN/DE)

---

## [1.9.4] — 2026-06-12

### Fixed
- **Performance page** — gaming score refresh opens the correct Optimierung page again
- **AI Advisor** — advisor no longer stays locked after an error during live measurement
- **DNS flush** — Network and Repair ask for confirmation before flushing the DNS cache
- **Live Log** — copy/export respects the same user-facing filter as the UI; duplicate consecutive lines suppressed

### Improved
- **Website** — multilingual landing page (DE/EN switcher + 25 additional read languages, 87 translated strings each)
- **QA & release** — trust manifest and update check aligned to V1.9.4

---

## [1.9.3] — 2026-06-12

### Fixed
- **Public release executable** — fixed silent startup failure on unsigned public builds (IntegrityGuard)
- **Release pipeline** — one public app EXE (`VantaCore Gaming Optimizer.exe`) in installer; no duplicate public builds

### Improved
- **Startup window** — default 1400×900, minimum 1200×760 for clearer UI on first launch
- **Website** — mobile hero button layout polish on landing page
- **User messages** — Live Log, status colors, and hardware card wording polished (EN/DE)
- **Pro Center** — grouped layout, duplicate tools hidden; preview for users, full access on developer PC only (no license keys)

---

## [1.9.2] — 2026-06-12

### Fixed
- **Stability** — bug fixes and general maintenance update

### Improved
- **Hardware detection** — dashboard and system info polish
- **Website & SEO** — landing page, version.json, and sitemap updated for V1.9.2

---

## [1.9.1] — 2026-06-04

### Added
- **Release hardening** — Obfuscar (private API + hidden strings), integrity check on startup, signed public builds

### Improved
- **Pro Center UI** — grouped 2-column layout (hero, search, categories); preview mode unchanged (28 tools, Coming soon)

### Fixed
- **Release pipeline** — public setup build and trust manifest aligned to V1.9.1

---

## [1.9] — 2026-06-04 (public release — Pro Center preview)

### Added
- **Pro Center** — 28 Pro tools visible in the app (Gaming, Network, System, Security, Maintenance, Bundle)
- **Pro Center preview mode** — cards show **Coming soon** / **Demnächst**; **Preview** button explains the roadmap (features are not runnable for public users yet)
- **Read-before-apply** — GPU scheduling, fullscreen optimizations, core parking, registry gaming read, and more ask or show current values before changes (when Pro Center is unlocked)

### Improved
- **Settings (public build)** — “Your edition / Full version” without license-key field (less confusion; app stays fully free)
- **Pro Center info banner** — clear note that Cleaner, drivers, network, and the rest of the app remain fully usable

### In development (not runnable in this public build)
- **Pro Center — run features** — unlock for customers after internal QA (`ProHubReleasedForCustomers`)
- **Smart Network Pro** — one-click DNS + latency tuning (developer preview)

---

## [1.8] — 2026-06-04

### Added
- **Settings — license key** — compact password field and activate button; hint that all features are currently free

### Improved
- **License security** — keys are never stored in plain text (hash only in settings); master keys embedded as obfuscated hashes in family builds only

---

## [1.7] — 2026-06-04

### Fixed
- **Autostart page** — scan no longer stuck on “Please wait…”; result panel shows entry count and status
- **Autostart scan** — runs in background so the scan button stays responsive

### Improved
- **Code signing** — build scripts for Authenticode (optional PFX); trust manifest reflects signature status

---

## [1.6] — 2026-06-04

### Improved
- **Network** — smarter DNS apply (skips when already optimal), safer IPv4/IPv6 rules for fast connections
- **Code** — `MainWindow` UI helpers split into smaller files for easier maintenance

### In development (not in this build)
- **Smart Network Pro** — one-click DNS + browser/latency tuning for Pro (dev preview on author PC only)

---

## [1.5] — 2026-06-03

### Fixed
- **Network** — active adapter detection uses Windows default route (more reliable than speed-only guess)
- **Network check** — clearer results when IPv4 gateway is missing

### Improved
- **Stability** — minor UI and logging polish on the network page

---

## [1.4] — 2026-06-04

### Fixed
- **Network page** — clearer status text and more reliable DNS actions
- **Update check** — version manifest handling improved
- **UI** — minor layout and navigation stability fixes

### Improved
- **Network check** — adapter list refreshes more reliably after tests

---

## [1.3] — 2026-06-03

### Added
- **Display settings** — font size Small / Normal / Large / XL; **monitor auto-fit** keeps window and controls readable on smaller screens
- **Compact settings page** — two-column layout (language, theme, font, scan depth, notifications) plus slim system row
- **SEO landing page** (`docs/`) for GitHub Pages — better findability on Google (see `docs/dev/SEO-GOOGLE.md`)

### Improved
- **Driver download (↗)** — opens the correct vendor download page (motherboard, GPU, LAN, Wi‑Fi); on failed auto-install, browser opens automatically
- **Vendor links action** — opens only hardware-matched URLs (not generic NVIDIA/AMD spam tabs)
- **Status & results** — “Please wait…” while working; clean user-facing result lines (no PowerShell/step dumps)
- **UI shell** — compact footer; sidebar without duplicate version / SYSTEMSTATUS boxes
- **Code structure** — `MainWindow` split into partial files (pages, actions, navigation, log)
- **README & GitHub** — search-friendly description, topics, and FAQ for “VantaCore Gaming Optimizer”

### Fixed
- **Settings layout** — controls no longer overlap on narrow widths
- **Update check** — `version.json` stays in sync with the published Setup EXE (V1.3)

---

## [1.2] — 2026-06-04

### Fixed
- **Driver vendor links (↗)** — ASUS/MediaTek/Wi-Fi arrows open your detected motherboard support page, not a hardcoded ROG X670E URL
- **AMD chipset links** — AM4 boards (e.g. B550) route to the correct AMD chipset page instead of AM5-only

### Improved
- **Settings layout** — language, scan depth, theme, and action buttons align in one column (380px)
- **Driver page** — vendor tiles (ASUS mainboard, AMD chipset) use detected hardware URLs
- **Testing** — `TEST-VOLL.bat` runs a slow sidebar audit (4s pause per menu) plus full UI selftest

---

## [1.1] — 2026-06-04

### Added
- **Setup installer cleanup** — official Setup EXE scans for old Redline/GamingBooster EXEs on desktop and legacy folders, lists them, and deletes before install
- **Update install prompt** — in-app update shows which old files are removed before installing a new version

### Improved
- **Release hardening** — Obfuscar, no debug symbols, no dev license backdoors in public builds
- **Version display** — sidebar and title bar show running EXE version (V1.1)

### Fixed
- **Version metadata** — `FileVersion` and `AssemblyVersion` stay in sync with displayed version

---

## [1.0] — 2026-06-03

### Added
- **Full release 1.0** — public versioning starts at V1.0 (no more 9.x); version shown in title bar and sidebar logo

### Improved
- **Build pipeline** — `FileVersion`, `AssemblyVersion`, and UI version stay in sync on every release build

### Fixed
- **Update check** — users on legacy 9.x builds are offered V1.0 as a newer release

---

## [9.54] — 2026-06-03

### Fixed
- **AMD CPU temperature** — detects Ryzen `Core (Tctl/Tdie)` sensor; shows **CPU temp: run as admin** when value is blocked without UAC (GPU temp still works)

---

## [9.53] — 2026-06-03

### Fixed
- **CPU temperature** — broader sensor scan (all `/cpu/` paths, max core fallback, double hardware refresh)
- **Live tiles** — removed decorative wave (looked like "~"); temp on its own line, sharper text (Ideal/ClearType)
- **Language** — autostart labels refresh when switching DE/EN (cache cleared)

### Improved
- **GPU/RAM temp line** — larger bold °C, detail line below (sharper than single muted line)

---

## [9.52] — 2026-06-03

### Fixed
- **CPU temperature on dashboard** — reads all CPU device sensors plus motherboard; WMI/CIM fallback; no longer overwritten without telemetry
- **RAM temperature** — shown when the board exposes DIMM/memory sensors (optional)

---

## [9.51] — 2026-06-03

### Fixed
- **CPU temperature** — dashboard shows °C under CPU (like GPU); improved sensor detection (Package, Tctl, cores, motherboard)

### Improved
- **GitHub releases** — clearer notes with highlights, bug fixes / changes sections, and install table

---

## [9.50] — 2026-06-03

### Changed
- **Settings** — removed duplicate language/scan panel; Pro/license block removed (status lives in sidebar)
- **Sidebar** — shows “Full version · all features active” instead of “Coming Soon”

---

## [9.49] — 2026-06-03

### Changed
- **No auto-update on startup** — removed from Settings and Update page; users check/install manually
- **Title bar icon** — transparent background (no dark box border on light Windows title bar)

---

## [9.48] — 2026-06-03

### Fixed
- **App icon** — sharp multi-size ICO (16–256) for title bar and taskbar; embedded in the app
- **Version label** — shows hint when a newer build is installed but an old desktop EXE is still running

---

## [9.47] — 2026-06-03

### Fixed
- **In-app update** — deletes old Desktop/legacy EXEs before install; only the new version is copied and started (fixes “same old version” after update)

---

## [9.46] — 2026-06-03

### Improved
- **GitHub release notes** — full English changelog on every release (marketing-friendly)
- **App icon** — new Redline hex + lightning logo in taskbar, title bar, and installer
- **Copyright UI** — cleaner badge in sidebar and footer (no tiny gray `(c)` text)
- **README** on GitHub — detailed feature list for promotion

---

## [9.45] — 2026-06-03

### Fixed
- **In-app update** — setup file no longer locked (“used by another process”)
- Unique download filename, safe copy with retry, installer waits and restarts app
- Desktop EXE sync when you run from Desktop

---

## [9.44] — 2026-06-03

### Added
- **All features free** — no license key required
- License key field visible but **disabled (gray)** — Pro purchase coming later
- English “What Redline can do” list in Settings
- Default language: English

### Fixed
- Auto-update: installer completion, app restart, version from EXE file

---

## [9.43] — 2026-06-03

### Fixed
- Auto-update pipeline: wait for setup, restart installed app, refresh Desktop copy
- Version detection from EXE (not hardcoded only)

---

## [9.42] — 2026-06-03

### Changed
- **Pro gating off** — Windows FPS Boost, deep scan, game tips, drivers: all free until paid Pro launches

---

## Download

Always use the latest **Setup EXE** from:  
https://github.com/LegendR622/Jitvora-Gaming-Optimizer/releases

© 2026 Tobias Immisch
