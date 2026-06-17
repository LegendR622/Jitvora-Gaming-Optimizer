# Code-Signing (Authenticode)

Signiert **App-EXE** und **Setup-EXE** mit SHA256 + Zeitstempel. Hilft bei SmartScreen und reduziert AV-Heuristik (z. B. Kaspersky HEUR).

**SmartScreen komplett „ruhig“:** nur mit **OV/EV-Zertifikat** von einer echten CA + Reputation — siehe **[SMARTSCREEN.md](SMARTSCREEN.md)**.

## Schnelltest (selbstsigniert, nur Pipeline)

**Nicht** für öffentliche Releases (Windows zeigt „Unbekannter Herausgeber“).

```powershell
cd c:\Users\Tobi\Desktop\Redline-Gaming-Optimizer
.\scripts\build\new-dev-code-sign-cert.ps1

$env:REDLINE_CODE_SIGN_PFX = "$PWD\secrets\redline-codesign.pfx"
$env:REDLINE_CODE_SIGN_PASSWORD = Get-Content .\secrets\codesign-password.txt -Raw

.\scripts\build\build-release.ps1 -Version 1.6
.\scripts\build\verify-code-signature.ps1 -Version 1.6
```

Rechtsklick Setup → Eigenschaften → **Digitale Signaturen** muss „Tobias Immisch / Redline …“ zeigen.

## Produktion (empfohlen für GitHub-Releases)

| Typ | SmartScreen | Kaspersky/VT | Kosten (ca.) |
|-----|-------------|--------------|--------------|
| Selbstsigniert | schwach | kaum | 0 € |
| **OV** (Organization Validation) | gut nach Reputation | besser | ~150–300 €/Jahr |
| **EV** | bestes Vertrauen | bestes | ~300–500+ €/Jahr |

Anbieter: [Certum Open Source](https://www.certum.eu/en/cert_offer_code_signing/), Sectigo, DigiCert, SSL.com.

**Antrag:** Firma/Publisher = **Tobias Immisch**, Produkt **Jitvora Gaming Optimizer**, Website + Impressum verlinken.

### Zertifikat als PFX nutzen

Nach Export aus dem Anbieter-Portal:

```powershell
$env:REDLINE_CODE_SIGN_PFX = "C:\secure\redline-ov.pfx"
$env:REDLINE_CODE_SIGN_PASSWORD = "dein-passwort"
.\scripts\build\build-release.ps1 -Version 1.6
```

Oder PFX nach `secrets\redline-codesign.pfx` legen (Ordner ist in `.gitignore`).

### Zertifikat im Windows-Zertifikatspeicher (USB/EV)

```powershell
$env:REDLINE_CODE_SIGN_THUMBPRINT = "ABCD1234..."   # certmgr.msc → Eigenes Zertifikat → Code Signing
.\scripts\build\sign-installer.ps1 -Version 1.6
```

### Nur signieren (ohne kompletten Build)

```powershell
.\scripts\build\sign-installer.ps1 -Version 1.6
```

Nach Signierung: neu auf **VirusTotal** + **Kaspersky OpenTIP** hochladen (neuer Hash wenn Datei geändert).

## Umgebungsvariablen

| Variable | Bedeutung |
|----------|-----------|
| `REDLINE_CODE_SIGN_PFX` | Pfad zur `.pfx` |
| `REDLINE_CODE_SIGN_PASSWORD` | PFX-Passwort |
| `REDLINE_CODE_SIGN_THUMBPRINT` | SHA1-Thumbprint (Zertifikatspeicher) |
| `REDLINE_TIMESTAMP_URL` | Optional, Standard: `http://timestamp.digicert.com` |

## Voraussetzungen

- **Windows SDK** (Signing Tools) → `signtool.exe`
- **Inno Setup 6** (für Setup-Build)

## Dateien

- `scripts/build/new-dev-code-sign-cert.ps1` – Dev-Zertifikat
- `scripts/build/sign-installer.ps1` – App + Setup signieren
- `scripts/build/verify-code-signature.ps1` – Prüfung
- `build-release.ps1` – signiert App vor Inno, Setup danach

**Niemals** `.pfx` oder `secrets\` committen.
