# Backup — Redline Gaming Optimizer

Wenn Windows, die Festplatte oder das Projekt kaputt geht, brauchst du eine **zweite Kopie** des **kompletten** Projekts — nicht nur die öffentliche GitHub-Seite.

## Wichtig: Was ist wo?

| Speicher | Inhalt |
|----------|--------|
| **Öffentliches GitHub** (`LegendR622/Redline-Gaming-Optimizer`) | Nur **~26 Dateien**: README, CHANGELOG, `docs/`, `version.json` — **kein App-Quellcode** |
| **Lokales Backup (Festplatte)** | **Alles**: `GamingBooster_Pro\`, `scripts\`, `secrets\`, `dist\`, Family-Builds, … |
| **Privates GitHub** (optional) | Vollständiger Quellcode in einem **Private** Repository |

Ohne lokales oder privates Backup ist der Quellcode **nur auf deinem PC** unter  
`Desktop\Redline-Gaming-Optimizer`.

---

## 1. USB-Sicherheitskopie (Haupt-Backup)

**Arbeiten:** nur in `Desktop\Redline-Gaming-Optimizer\`  
**Backup:** `Desktop\Redline\USB-Backup\` — wird bei jedem Sync **überschrieben/aktualisiert** (Spiegel), nicht dort entwickeln.

Doppelklick: **`BACKUP-FUER-USB.bat`** im Arbeitsordner, oder `Desktop\Redline\AKTUALISIERE-BACKUP.bat`

Stick: ganzen Ordner **`USB-Backup`** kopieren.

## 2. Archiv-Backup mit Datum (optional)

Doppelklick: **`BACKUP-JETZT.bat`** im Projektordner.

### Oder PowerShell

```powershell
cd C:\Users\Tobi\Desktop\Redline-Gaming-Optimizer
.\scripts\backup\Backup-RedlineFull.ps1
```

Anderes Laufwerk (z. B. externe Festplatte **D:**):

```powershell
.\scripts\backup\Backup-RedlineFull.ps1 -BackupRoot "D:\Backups\Redline"
```

Mit ZIP (eine Datei zum Kopieren):

```powershell
.\scripts\backup\Backup-RedlineFull.ps1 -BackupRoot "D:\Backups\Redline" -CreateZip
```

### Was wird gesichert?

- `GamingBooster_Pro\` (kompletter C#-Quellcode)
- `scripts\`, `tools\`, `docs\`, `family-release\`
- `secrets\` (Keys/Signing — **vertraulich**)
- `dist\` (Installer, falls vorhanden)
- Nicht mitkopiert: `bin\`, `obj\`, `.vs\` (werden beim Build neu erzeugt)

Jeder Lauf legt einen Ordner an, z. B.  
`D:\Backups\Redline\Redline-Gaming-Optimizer_2026-06-04_1530`

---

## 2. Windows-Zusatz (optional)

- **Dateiversionsverlauf** für den Projektordner aktivieren (Eigenschaften → Vorgängerversionen)
- Wichtige Backups zusätzlich auf **USB / zweite interne Platte** kopieren
- `BACKUP-JETZT.bat` z. B. wöchentlich per **Aufgabenplanung** ausführen

---

## 3. Privates GitHub-Backup (optional)

Nur wenn du den Code **verschlüsselt online** haben willst — Repository muss **Private** sein.

1. Auf GitHub: **New repository** → Name z. B. `Redline-Gaming-Optimizer-Private` → **Private**
2. Im Terminal (einmalig `gh auth login`):

```powershell
.\scripts\backup\Push-RedlinePrivateGit.ps1 -PrivateRepoUrl "https://github.com/DEINUSER/Redline-Gaming-Optimizer-Private.git"
```

**Niemals** `secrets\`, PFX oder Master-Keys ins **öffentliche** Repo legen. Im privaten Repo: nur wenn du es wirklich brauchst und das Repo **private** bleibt.

---

## 4. Wiederherstellen

1. Backup-Ordner nach `C:\Users\Tobi\Desktop\Redline-Gaming-Optimizer` kopieren (oder neuen Ordner wählen)
2. `dotnet build GamingBooster_Pro\GamingBooster_Pro.csproj -c Release`
3. Bei Bedarf: `.\scripts\build\build-release.ps1 -Version 1.9`

---

## Checkliste (einmalig)

- [ ] `BACKUP-JETZT.bat` ausgeführt → Ordner auf **D:** oder `Documents\Backups\Redline` vorhanden
- [ ] Backup-Ordner **nicht** in öffentliches GitHub committed
- [ ] Optional: privates Repo angelegt und `Push-RedlinePrivateGit.ps1` ausgeführt
- [ ] USB oder zweite Platte: ZIP-Kopie abgelegt
