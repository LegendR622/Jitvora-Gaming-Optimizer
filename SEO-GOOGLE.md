# Redline bei Google findbar machen

Diese Schritte ergänzen die Dateien `docs/` (Landingpage) und das optimierte `README.md`.

## 1. GitHub Pages aktivieren (wichtig)

1. Öffne: https://github.com/LegendR622/Redline-Gaming-Optimizer/settings/pages  
2. **Source:** Deploy from a branch  
3. **Branch:** `main` · Ordner **`/docs`**  
4. Speichern — nach 1–5 Minuten erreichbar unter:  
   **https://legendr622.github.io/Redline-Gaming-Optimizer/**

Diese Seite hat Titel, Beschreibung, Keywords und Schema.org — Google indexiert sie besser als nur ein README.

## 2. Öffentliches Repo pushen (mit `docs/`)

```powershell
.\scripts\release\push-public-main.ps1 -Message "docs: SEO landing page for Google"
```

(Das Skript fügt `docs/` mit ein, wenn du die aktualisierte Version nutzt.)

## 3. GitHub-Repo für Suche optimieren

Einmal ausführen (GitHub CLI `gh` eingeloggt):

```powershell
.\scripts\release\setup-github-seo.ps1
```

Oder manuell auf GitHub unter **About** (Zahnrad):

- **Description:** `Free Windows gaming optimizer — FPS, drivers, PC cleanup, safe tweaks. Redline Gaming Optimizer by Tobias Immisch.`
- **Website:** `https://legendr622.github.io/Redline-Gaming-Optimizer/`
- **Topics:** `gaming`, `optimizer`, `windows`, `fps`, `driver-updater`, `pc-cleaner`, `game-mode`, `windows-11`, `tweaks`, `utility`

## 4. Google Search Console

1. https://search.google.com/search-console  
2. Property hinzufügen: **URL-Präfix**  
   `https://legendr622.github.io/Redline-Gaming-Optimizer/`  
3. Verifizierung (HTML-Datei oder DNS — GitHub Pages: oft HTML-Tag in `docs/index.html` möglich)  
4. **Sitemap einreichen:**  
   `https://legendr622.github.io/Redline-Gaming-Optimizer/sitemap.xml`  
5. Optional zweite Property: `https://github.com/LegendR622/Redline-Gaming-Optimizer`

Indexierung dauert oft **Tage bis Wochen** — Geduld.

## 5. Was Nutzer suchen (in README & Seite abgedeckt)

| Suchbegriff (DE/EN) | Wo |
|---------------------|-----|
| Redline Gaming Optimizer | Titel, H1, Schema |
| Gaming Optimizer Windows | README, Landing |
| PC optimieren Gaming / FPS Booster | FAQ, Features |
| Treiber Update Tool Windows | Features |
| Windows 11 Gaming Optimizer | Tags, FAQ |

## 6. Realistische Erwartung

- **Markenname „Redline Gaming Optimizer“** — nach Indexierung gut auffindbar.  
- **Generisch „gaming optimizer“** — stark umkämpft; braucht Zeit, Backlinks (YouTube, Forum, Reddit), regelmäßige Releases.  
- Jede **neue Version** mit Release-Notes + CHANGELOG hilft Google (frische Inhalte).

## 7. Optional später

- Eigenes Domain (z. B. `redline-optimizer.de`) → CNAME auf GitHub Pages  
- Kurzes YouTube-Video „Download & Install“ mit Link in der Beschreibung  
- Eintrag in Software-Verzeichnissen (nur seriöse Seiten)

---

**Checkliste vor „wir sind bei Google“**

- [ ] GitHub Pages live (URL öffnet Landing)  
- [ ] `setup-github-seo.ps1` oder Topics/Description gesetzt  
- [ ] Search Console + Sitemap  
- [ ] Neuestes Release auf GitHub mit klarem Titel „Redline Gaming Optimizer Vx.x“
