# Changelog

All notable changes to **Redline Gaming Optimizer** are listed here (newest first).

**Writing rules (for releases & GitHub notes):**
- Use **English** for all new entries (`### Fixed`, `### Improved`, `### Added`, `### Changed`).
- Put **bug fixes** under `### Fixed` (e.g. “CPU temperature on AMD Ryzen”).
- Use `### Improved` for UX/performance, `### Added` for new features, `### Changed` for behavior changes.
- Version format: **1.0**, **1.1** (no more 9.x).

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
https://github.com/LegendR622/Redline-Gaming-Optimizer/releases

© 2026 Tobias Immisch
