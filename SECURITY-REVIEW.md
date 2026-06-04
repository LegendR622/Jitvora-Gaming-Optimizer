# Redline Gaming Optimizer — Security Review (V1.2)

Stand: 2026-06-03. Kurzprüfung vor Online-Veröffentlichung.

## Veröffentlichung (öffentliche Pakete)

| Kanal | Inhalt | Status |
|-------|--------|--------|
| GitHub Release | Nur `dist/Redline_Gaming_Optimizer_Setup_v*.exe` | OK — `publish-github-clean.ps1` |
| GitHub `main` (öffentlich) | README, CHANGELOG, `version.json` — **kein Quellcode** | `push-public-main.ps1` entfernt `GamingBooster_Pro/`, `scripts/`, `tools/` |
| Optional ZIP | Nur `publish/win-x64/*` nach `dotnet publish` | Kein `bin/`, `obj/`, `.git` |

Vor Release: `scripts/release/verify-public-package.ps1` ausführen.

## Admin-Funktionen

- Reparatur (SFC, DISM, Winsock), DNS-Flush, Autostart-Deaktivierung, Defender-Offline-Scan: erfordern Bestätigungsdialog; kritische Aktionen prüfen `IsAdmin()` / UAC.
- `RedlineAdminHelper`: startet Elevation nur auf Nutzeraktion (z. B. CPU-Temp-Hinweis).
- **Empfehlung online:** Hinweis in README — „Als Administrator für Reparatur/DNS“.

## Cleaner

- Nur ausgewählte Cache/Temp-Pfade und Browser-Daten (nach Bestätigung); keine Systemordner außerhalb der Whitelist.
- Gesperrte Dateien werden übersprungen; Papierkorb/DNS nur wenn Kategorie aktiv.
- **Risiko:** Browser-Verlauf/Cookies — bewusst destruktiv, nur nach `PreActionCheck`.

## DNS / Netzwerk

- DNS-Presets und `ipconfig`/netsh nur nach Bestätigung; Anti-Cheat-Safe-Mode blockiert riskante Netzwerk-Resets.
- Keine stillen Remote-DNS-Umleitungen.

## Treiber-Install / Update

- `RedlineDriverInstallEngine`: Downloads nur von konfigurierten Hersteller-/GitHub-URLs; Hash-Prüfung wo implementiert.
- In-App-Update: nur offizielle GitHub-`version.json` / Releases; Installation immer mit Nutzerbestätigung (`RedlineUpdateInstaller`).
- **Kein** automatisches Silent-Install ohne Dialog.

## Telemetry

- `RedlineTelemetry`: **lokal** LibreHardwareMonitor + WMI — **keine** Cloud-Telemetrie, keine Datenübertragung.
- Overlay/Dashboard lesen nur lokalen Sensor-Status.

## Datenschutz (App)

- Einstellungen/Daten unter `%AppData%\RedlineGamingOptimizer\` (lokal).
- Kein Kontakt zu fremden Analytics-Servern in Standard-Build (`RedlineDeveloperBuild=false`).
- Öffentliche Texte: `docs/datenschutz.html`, `docs/haftung.html`, `docs/EULA.txt` (Installer).
- Erststart: `RedlineLegalAcceptance` (Version `1.6`).

## Trust / Aufkauf

- Checkliste: `docs/ACQUISITION-READY.md`
- Trust Center: `docs/trust.html` + `scripts/release/write-trust-manifest.ps1`
- Code-Signing (optional): `scripts/build/sign-installer.ps1` + `REDLINE_CODE_SIGN_PFX`

## Bekannte Restrisiken (akzeptiert / dokumentiert)

1. Admin-Tools können System beschädigen, wenn Nutzer alle Reparaturen bestätigt — durch Dialoge abgefedert.
2. Treiber-Downloads: Vertrauen in HTTPS-Endpunkte der Hersteller.
3. Öffentliches Git-History kann alte Commits mit Source enthalten — bei striktem Bedarf: neues Repo oder History-Rewrite.

## Go-Live Checkliste

- [ ] `TEST.bat` grün
- [ ] `run-full-pc-test.ps1 -Slow` grün
- [ ] `verify-public-package.ps1` grün
- [ ] Release-EXE auf Test-PC installiert (frischer Start, Footer zeigt eine Version)
- [ ] Defender/Firewall im Dashboard geprüft (nicht Sidebar)
