# Windows SmartScreen — weniger Warnungen

SmartScreen lässt sich **nicht per Code in der App abschalten**. Nutzer sehen „Unbekannter Herausgeber“ oder „Windows hat den PC geschützt“, wenn:

1. Die **Setup-EXE von Internet** kommt (Mark of the Web / Zone.Identifier)
2. Der **Publisher keine Reputation** bei Microsoft hat
3. Nur **selbstsigniert** oder **gar nicht** signiert wurde

## Was Redline bereits macht

| Maßnahme | Wirkung |
|----------|---------|
| **Authenticode** (SHA256 + Zeitstempel) auf App + Setup | Pflicht für Vertrauen |
| **Gleicher Publisher** „Tobias Immisch“ / Redline | Reputation sammelt sich |
| **In-App-Update** entfernt Zone.Identifier nach Download | Etwas weniger streng bei Update-Installer |
| **Trust-Seite** + SHA256 auf GitHub | Nutzer können Datei prüfen |

## Was du für „kaum noch fragen“ brauchst (Produktion)

### 1. Echtes Code-Signing-Zertifikat (wichtigste Stufe)

| Zertifikat | SmartScreen | Ca. Kosten |
|----------|-------------|------------|
| Selbstsigniert (Dev) | **fragt fast immer** | 0 € |
| **OV** (Organization Validation) | gut, nach ein paar Downloads | ~150–300 €/Jahr |
| **EV** (Extended Validation) | **beste Chance**, oft schneller weniger Warnungen | ~300–500+ €/Jahr |

Anbieter: Certum, Sectigo, SSL.com, DigiCert.

```powershell
$env:REDLINE_CODE_SIGN_PFX = "C:\secure\redline-ov.pfx"
$env:REDLINE_CODE_SIGN_PASSWORD = "..."
.\scripts\build\build-release.ps1 -Version 1.9
.\scripts\build\verify-code-signature.ps1 -Version 1.9
```

Ausgabe muss **`Valid (trusted CA)`** sein, nicht nur „Signed (UnknownError)“.

### 2. Reputation bei Microsoft (nach OV/EV)

- Datei einreichen: https://www.microsoft.com/en-us/wdsi/filesubmission  
  → **Software developer**, Setup-EXE hochladen, Publisher = wie im Zertifikat  
- VirusTotal: gleiche signierte EXE hochladen (saubere Ergebnisse helfen indirekt)
- **Immer dieselbe signierte EXE-Identität** — nicht zwischen selbstsigniert und OV wechseln

### 3. Nutzer-Hinweis (erster Download von GitHub)

Einmalig nach Download:

- Rechtsklick Setup → **Eigenschaften** → unten **„Zulassen“** / **Unblock** (falls sichtbar), oder  
- PowerShell: `Unblock-File .\Jitvora_Gaming_Optimizer_Setup_v1.9.exe`

Das ist normal bei **jedem** kleinen Publisher, bis Reputation da ist.

## Prüfen vor Release

```powershell
.\scripts\release\Test-SmartScreenReadiness.ps1 -Version 1.9
```

## Kurz für Nutzer (Website / Support)

> Redline ist signiert von Tobias Immisch. Beim **ersten** Install von GitHub kann Windows SmartScreen kurz warnen — „Weitere Informationen“ → Herausgeber prüfen → bei vertrauenswürdigem Download von GitHub Releases fortfahren. Updates aus der App werden ohne erneuten Browser-Download installiert.
