# Schritt für Schritt: Sitemap bei Google

Die Sitemap liegt bereits auf der Website:

**https://legendr622.github.io/Redline-Gaming-Optimizer/sitemap.xml**

`robots.txt` verweist darauf automatisch.

## 1. Prüfen (lokal)

```powershell
.\scripts\release\verify-seo-online.ps1
```

Alle `sitemap URL` Zeilen müssen `[OK]` sein.

## 2. Google Search Console (einmalig, manuell)

1. Öffne https://search.google.com/search-console  
2. Property: `https://legendr622.github.io/Redline-Gaming-Optimizer/`  
3. Links: **Sitemaps** → neue Sitemap hinzufügen  
4. Nur den Pfad eintragen: `sitemap.xml`  
5. **Senden** — Status wird „Erfolgreich“ (kann einige Stunden dauern)

## 3. Nach neuen Seiten

- `docs/sitemap.xml` anpassen (`lastmod` aktualisieren)  
- `git push origin main` (nur `docs/`)  
- In Search Console: Sitemap erneut einreichen oder auf „Zuletzt gelesen“ warten

Keine separate Sitemap nötig — eine Datei reicht für alle HTML-Seiten unter `docs/`.
