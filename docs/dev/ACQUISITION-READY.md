# Acquisition readiness — Jitvora Gaming Optimizer

Stand: 2026-06-04. Checkliste für Due Diligence / Aufkauf.

## Status-Legende

| Symbol | Bedeutung |
|--------|-----------|
| ✅ | Vorhanden / umgesetzt |
| 🟡 | Teilweise — dokumentiert oder Stub |
| ❌ | Noch offen (extern/abhängig) |

## Technik & Code

| Punkt | Status | Nachweis |
|-------|--------|----------|
| Saubere Code-Trennung | ✅ | `CODEBASE.md`, `Features/`, MainWindow-Partials |
| Automatisierte Tests | 🟡 | `TEST.bat`, `--selftest`; keine Unit-Test-Suite |
| Update nur offizielles GitHub | ✅ | `RedlineOnlineUpdate.cs` |
| Admin nur bei Bedarf | ✅ | `permissions.html`, `docs/dev/SECURITY-REVIEW.md` |

## Trust & Legal (Website)

| Punkt | Status | Nachweis |
|-------|--------|----------|
| Landing + Download | ✅ | `docs/index.html` |
| Impressum | ✅ | `docs/impressum.html` |
| Datenschutz | ✅ | `docs/datenschutz.html` |
| Haftung / Nutzung | ✅ | `docs/haftung.html`, `docs/EULA.txt` |
| Support-Seite | ✅ | `docs/support.html` |
| Changelog (Web) | ✅ | `docs/changelog.html` + GitHub CHANGELOG |
| Trust / SHA256 / VT | 🟡 | `docs/trust.html` + `write-trust-manifest.ps1` |
| Code-Signing | ❌ | `sign-installer.ps1` — Zertifikat vom Inhaber |

## Produkt

| Punkt | Status | Nachweis |
|-------|--------|----------|
| EULA beim ersten Start | ✅ | `RedlineLegalAcceptance.cs` |
| Auto-Wiederherstellungspunkt | ✅ | Einstellung + `TryAutoRestorePointAsync` |
| Undo / Rollback | 🟡 | Protokoll + Rollback Power Plan / Game Mode |
| Lizenz Gamer / Firma | 🟡 | `licenses.html`, `RedlineAppData` vorbereitet |
| Keine Cloud-Telemetrie | ✅ | `datenschutz.html`, `RedlineTelemetry` lokal |

## Vor Go-Live (Inhaber)

1. ~~Postanschrift & Support-E-Mail~~ ✅  
2. EV-Zertifikat kaufen → `sign-installer.ps1` ausführen  
3. Release bauen → `write-trust-manifest.ps1` → VirusTotal → `trust-latest.json` committen  
4. GitHub Pages deploy (`docs/`)

## Build-Befehle

```powershell
.\scripts\build\build-release.ps1 -Version 1.6
.\scripts\release\write-trust-manifest.ps1 -Version 1.6
# Optional:
.\scripts\build\sign-installer.ps1 -Version 1.6
```
