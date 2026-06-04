# Redline bei Google findbar machen

## Erledigt (automatisch / bereits aktiv)

- [x] **SEO-Landingpage** in `docs/index.html` (Titel, Meta, Schema.org, FAQ, V1.3-Download)
- [x] **`robots.txt`** und **`sitemap.xml`**
- [x] **`README.md`** mit Suchbegriffen und FAQ
- [x] **GitHub About:** Beschreibung, Homepage, Topics (`setup-github-seo.ps1`)
- [x] **GitHub Pages** aktiviert: **https://legendr622.github.io/Redline-Gaming-Optimizer/**
- [x] **Release V1.3** + **`version.json`** auf `main`

Skripte (lokal):

```powershell
.\scripts\release\complete-seo-setup.ps1   # alles prüfen + Pages
.\scripts\release\verify-seo-online.ps1    # nur prüfen
```

---

## Einmalig von dir: Google Search Console (ca. 5 Min.)

Google-Indexierung läuft **nur über dein Google-Konto** — das kann die App nicht automatisch.

1. Öffne https://search.google.com/search-console  
2. **Property hinzufügen** → **URL-Präfix**  
   `https://legendr622.github.io/Redline-Gaming-Optimizer/`
3. **Verifizierung** (eine Methode wählen):
   - **HTML-Tag:** Meta-Tag aus Search Console kopieren → in `docs/index.html` unter dem Kommentar `Google Search Console` einfügen → `push-public-main.ps1` oder Commit auf `main` → Pages neu bauen (1–2 Min.)
   - **HTML-Datei:** Datei in `docs/` legen, pushen
4. **Sitemap einreichen:**  
   `https://legendr622.github.io/Redline-Gaming-Optimizer/sitemap.xml`
5. Optional zweite Property:  
   `https://github.com/LegendR622/Redline-Gaming-Optimizer`

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
