# Release-Härtung (Obfuscation, Anti-Tamper, Signing)

## Ziele vs. Umsetzung

| Ziel | Status in Redline |
|------|-------------------|
| Klassen/Methoden/Felder umbenennen | **Obfuscar** — private API + Felder; `KeepPublicApi=true` (öffentliche Member bleiben) |
| Strings verschlüsseln/verstecken | **Obfuscar `HideStrings`** (nicht für hochsensible Secrets — trotzdem keine Keys im Client) |
| Control Flow obfuskieren | **Nicht mit Obfuscar** (Tool unterstützt das nicht). `OptimizeMethods` = IL-Optimierung, kein CFG-Flattening. Für echtes Control-Flow: kommerzieller Obfuscator (z. B. Dotfuscator, VMProtect) |
| Anti-Tamper | **`RedlineIntegrityGuard`** — Anti-Debug/Reverse-Tools, EXE-Name, Assembly-Name, Authenticode |
| WPF/XAML stabil | Nur **`App.xaml`** mit `x:Class` → `App` ausgeschlossen. UI ist code-built (`MainWindow` partial) — private Methoden werden obfusciert |
| Public API / JSON / XAML | Ausgeschlossen: `App`, `RedlineAppData`, `LicenseTier`, `RedlineTheme`, Pro-Test-Klassen + `[Obfuscation(Exclude)]` |
| Keine PDB / Debug | Release: `DebugType=none`, `DebugSymbols=false`, PDB-Löschung nach Publish |
| Installer testen | **`test-installer-smoke.ps1`** (silent install + `--selftest` + uninstall) |

## Build-Pipeline

```powershell
# Public
.\scripts\build\build-release.ps1 -Version 1.9.1

# Family (lokal)
.\scripts\build\build-family-release.ps1 -Version 1.9.1

# Nur Härtung prüfen (nach Build)
.\scripts\build\verify-release-hardening.ps1 -Version 1.9.1
.\scripts\build\test-installer-smoke.ps1 -Version 1.9.1

# Schneller Build ohne Installer-Test
.\scripts\build\build-release.ps1 -Version 1.9.1 -SkipInstallerSmoke
```

Ablauf in `build-release.ps1`:

1. `Write-ObfuscarConfig.ps1` → aktuelle .NET-Runtime-Pfade in `obfuscar.xml`
2. `dotnet publish` (Release, single-file)
3. Obfuscar MSBuild-Target (bricht bei Fehler ab)
4. Inno Setup + Signierung
5. `verify-release-hardening.ps1` + optional Installer-Smoke

## Obfuscar-Ausnahmen

| Ausgeschlossen | Grund |
|----------------|--------|
| `App` | WPF `x:Class` in `App.xaml` |
| `RedlineAppData` | `settings.json` Property-Namen |
| `LicenseTier`, `RedlineTheme` | public API / UI-Theming |
| Pro-*Test* | Headless `--pro-selftest` / `--pro-live-test` |
| **Nicht** `MainWindow` | Nur public Member bleiben lesbar; private Logik wird obfusciert |

## Integrität (`RedlineIntegrityGuard`)

- Anti-Debug / dnSpy, ILSpy, x64dbg, …
- Prozess-EXE muss Redline/GamingBooster heißen
- Assembly-Name muss `GamingBooster_Pro` sein
- Authenticode Pflicht (Release, nicht bei `--selftest`)
- Public: Publisher Tobias Immisch / Redline

## Secrets im Client

| Erlaubt | Verboten |
|---------|----------|
| SHA-256-Hashes (Lizenz) | API-Keys, Passwörter, Bearer-Tokens |
| XOR-Blob nur Family-Build | `secrets\` im Repo |

Selftest: `AuditClientForPlaintextSecrets()`.

## Stärkster Schutz für Nutzer-Vertrauen

OV/EV Code-Signing + offizielle GitHub-Releases (siehe `docs/SMARTSCREEN.md`).
