# Schritt für Schritt: Trust (SHA256 + VirusTotal)

## Schritt 1 — Hash erzeugen (PC)

```powershell
cd c:\Users\Tobi\Desktop\Redline-Gaming-Optimizer
.\scripts\release\write-trust-manifest.ps1 -Version 1.6
```

Erzeugt:

- `docs/trust-latest.json` (für die Webseite)
- `dist/TRUST_v1.6.json` (für Releases)

## Schritt 2 — Webseite live

```powershell
git add docs/trust-latest.json docs/sitemap.xml
git commit -m "chore: trust SHA256 and sitemap"
git push origin main
```

Warten (~1–2 Min), dann prüfen: https://legendr622.github.io/Jitvora-Gaming-Optimizer/trust.html

## Schritt 3 — VirusTotal (manuell, einmal pro Version)

1. Installer von GitHub Releases laden (gleiche Datei wie in Schritt 1)  
2. https://www.virustotal.com/gui/home/upload — Datei hochladen  
3. Nach dem Scan: Link kopieren (oder Hash-Link von der Trust-Seite nutzen)

**Hinweis:** Der Hash-Link auf der Trust-Seite zeigt Ergebnisse erst, wenn die Datei bei VirusTotal mindestens einmal bekannt ist (Upload oder Hash-Suche).

## Schritt 4 — Code-Signing

Vollständige Anleitung: **docs/CODE-SIGNING.md**

```powershell
# Dev-Test (selbstsigniert):
.\scripts\build\new-dev-code-sign-cert.ps1
$env:REDLINE_CODE_SIGN_PFX = "$PWD\secrets\redline-codesign.pfx"
$env:REDLINE_CODE_SIGN_PASSWORD = Get-Content .\secrets\codesign-password.txt -Raw

# Oder OV/EV-PFX vom Anbieter:
# $env:REDLINE_CODE_SIGN_PFX = "C:\secure\cert.pfx"
# $env:REDLINE_CODE_SIGN_PASSWORD = "..."

.\scripts\build\build-release.ps1 -Version 1.6
.\scripts\build\verify-code-signature.ps1 -Version 1.6
git add docs/trust-latest.json
git push origin main
```
