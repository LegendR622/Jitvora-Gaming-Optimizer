# Redline bei Google findbar machen

## Erledigt (automatisch / bereits aktiv)

- [x] **SEO-Landingpage** in `docs/index.html` (Titel, Meta, Schema.org, FAQ, V1.3-Download)
- [x] **`robots.txt`** und **`sitemap.xml`**
- [x] **`README.md`** mit Suchbegriffen und FAQ
- [x] **GitHub About:** Beschreibung, Homepage, Topics (`setup-github-seo.ps1`)
- [x] **GitHub Pages** aktiviert: **https://legendr622.github.io/Redline-Gaming-Optimizer/**
- [x] **Release V1.3** + **`version.json`** auf `main`
- [x] **Search Console Verifizierung** — `google2155792da4e8c226.html` in `docs/`

Skripte (lokal):

```powershell
.\scripts\release\complete-seo-setup.ps1   # alles prüfen + Pages
.\scripts\release\verify-seo-online.ps1    # nur prüfen
```

---

## Domain oder URL? (Search Console)

| Option | Für Redline? | Warum |
|--------|--------------|--------|
| **URL-Präfix** | **Ja — das nimmst du** | Passt zu deiner GitHub-Pages-Adresse mit Pfad |
| **Domain** | **Nein** | Nur sinnvoll bei **eigener** Domain (z. B. `redline-tool.de`), Verifizierung per **DNS** |

**Eintragen (exakt so, mit `https` und Schrägstrich am Ende):**

```text
https://legendr622.github.io/Redline-Gaming-Optimizer/
```

Optional **zweite** Property (auch URL-Präfix):

```text
https://github.com/LegendR622/Redline-Gaming-Optimizer
```

**Nicht** `legendr622.github.io` als Domain-Property — die ganze Domain `github.io` gehört GitHub, nicht dir.

**GitHub Pages → „Custom domain“:** leer lassen, bis du eine **eigene** Domain gekauft hast. Ohne eigene Domain reicht die `github.io`-URL oben.

---

## Einmalig von dir: Google Search Console (ca. 5 Min.)

Google-Indexierung läuft **nur über dein Google-Konto** — das kann die App nicht automatisch.

1. Öffne https://search.google.com/search-console (anmelden)  
2. **Property hinzufügen** → **URL-Präfix** (nicht Domain)  
3. URL einfügen: `https://legendr622.github.io/Redline-Gaming-Optimizer/`  
4. **Verifizierung:** HTML-Datei (bereits erledigt) → **BESTÄTIGEN**

### Nach der Bestätigung (in deinem offenen Search Console-Tab)

**1. Sitemaps (wichtig)**  
- Linkes Menü: **Sitemaps**  
- **Volle URL** eintragen (kurz nur `sitemap.xml` schlägt bei GitHub Pages oft fehl):

```text
https://legendr622.github.io/Redline-Gaming-Optimizer/sitemap.xml
```

- Alte Zeile mit Status **„Konnte nicht abgerufen werden“** (`/sitemap.xml`) → drei Punkte → **Sitemap entfernen**  
- **Senden** — Status nach einiger Zeit **„Erfolgreich“**

**2. URL-Prüfung (empfohlen, einmal)**  
- Menü: **URL-Prüfung**  
- URL: `https://legendr622.github.io/Redline-Gaming-Optimizer/`  
- **Indexierung beantragen** (wenn angeboten)

**3. Optional**  
- Menü: **Seiten** — später prüfen, ob URLs indexiert werden  
- Keine weiteren Pflicht-Einträge für GitHub Pages

**Indexierung:** oft **3–14 Tage** bis „Redline Gaming Optimizer“ in Google erscheint.

---

## Öffentliches Repo aktualisieren (nach Änderung an `docs/`)

```powershell
.\scripts\release\push-public-main.ps1 -Message "docs: SEO update"
```

---

## Suchbegriffe (abgedeckt)

| Suche | Wo |
|--------|-----|
| Redline Gaming Optimizer | Landing H1, Schema, README |
| Gaming Optimizer Windows | README, Landing |
| PC optimieren Gaming / FPS Booster | FAQ |
| Treiber Update Windows | Features |
| Windows 11 Gaming Optimizer | Tags, FAQ |

---

## Optional später

- Eigene Domain → CNAME auf GitHub Pages  
- YouTube „Install V1.3“ mit Link zur Releases-URL  
- Bing Webmaster Tools (gleiche Sitemap URL)
