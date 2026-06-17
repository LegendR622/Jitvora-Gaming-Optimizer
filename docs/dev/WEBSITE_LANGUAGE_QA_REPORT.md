# Website Language & Encoding QA Report

**Date:** 2026-06-12  
**Scope:** GitHub Pages website only (`docs/`)  
**Live URL:** https://legendr622.github.io/Redline-Gaming-Optimizer/  
**Cache-bust check:** https://legendr622.github.io/Redline-Gaming-Optimizer/?encoding-check=latest

## Summary

Fixed widespread UTF-8 mojibake across all 27 website locales, repaired `index.html` fallback strings and broken emoji icons (replaced with inline SVG), and improved mobile layout CSS below 768px. No app code, installer, release pipeline, or app version changes were made.

## 1. Files changed

| File | Change |
|------|--------|
| `docs/i18n-full.json` | Repaired encoding for all 27 locales; restored complete `en` locale; v1.9.6 strings preserved |
| `docs/site-i18n.js` | Regenerated from fixed `i18n-full.json` |
| `docs/index.html` | Fixed meta/title/fallback mojibake; SVG feature icons instead of broken emoji |
| `docs/landing.css` | Mobile polish (768px/520px): nav, hero, cards, FAQ, footer; SVG icon styling |
| `docs/i18n/rebuild-from-good.py` | Rebuild all 26 locales from known-good UTF-8 git baseline (`debbd84`) |
| `docs/i18n/restore-script-locales.py` | Restore CJK/script locales from history |
| `docs/i18n/fix-index-html.py` | HTML fallback encoding + icon repair |
| `docs/i18n/scan-website-i18n.py` | Automated encoding/completeness scan |
| `docs/i18n/qa-all-languages.py` | Per-locale key/encoding QA |
| `docs/i18n/*.py` | Moved i18n tooling from `docs/` root (no logic change to app) |
| `docs/dev/*.md` | Moved internal dev docs out of Pages root (no user-facing URL change) |

## 2. Encoding fixes made

- **Root cause:** Double-encoded UTF-8 (UTF-8 bytes interpreted as Latin-1/Windows-1252 and saved again as UTF-8).
- **Patterns fixed:** `FÃ¼r`→`Für`, `prÃ¼fen`→`prüfen`, `â€"`→`—`, `Â·`→`·`, `Â©`→`©`, Cyrillic/CJK mojibake, Romanian/Turkish/Vietnamese diacritics, broken emoji `ðŸ…`→ SVG icons.
- **Method:** Segment-aware Latin-1→UTF-8 roundtrip, UTF-8 pair repair, literal sequence map, targeted Vietnamese overrides, UTF-8 JSON rewrite without BOM.
- **Scan result:** `scan-website-i18n.py` — 27/27 locales, 87 keys each, zero bad markers in website files.

## 3. Mobile layout fixes made

Below **768px**:

- Flexible nav height; logo ellipsis; nav links wrap
- Hero: more padding, readable subtitle/badge, full-width CTA buttons (520px)
- Trust pills wrap cleanly
- Product mockup stacks sidebar horizontally on small screens
- Feature cards / FAQ / footer: larger tap targets and readable font sizes
- No horizontal scroll (`overflow-x: clip` preserved)

Test widths: **390px**, **430px**, **768px**, desktop.

## 4. Desktop regression check

- Premium dark/red branding preserved
- Language switcher (DE/EN + dropdown) intact
- Hero, features, how-it-works, safety, FAQ sections unchanged structurally
- Download still targets **v1.9.6** setup EXE on GitHub Releases
- No desktop layout regression observed in local verification

## 5. Links checked

| Link | Status |
|------|--------|
| Download (v1.9.6 setup) | OK — unchanged target |
| GitHub repo | OK |
| GitHub Releases | OK |
| Trust & SHA256 (`trust.html`) | OK |
| Changelog | OK |
| Sitemap (`sitemap.xml`) | OK |
| Privacy / Imprint / Terms / Support / Licenses | OK |
| FAQ anchors | OK |
| Language switch localStorage (`redline-site-lang`) | OK |

## 6. Live GitHub Pages status

- **Before deploy:** Live site showed broken encoding (pre-fix `site-i18n.js`).
- **After deploy:** Verified at `?encoding-check=latest` (commit `de36d32`) — all 27 locales render correct UTF-8; language dropdown labels correct (e.g. 日本語, Русский, Français); Download v1.9.6 on all locales.
- **Cache bust:** Asset query `?v=20260612c` on `site-i18n.js`, `site-lang.js`, `site-lang-boot.js`, `landing.css`, `site-lang.css` so browsers fetch repaired translations.

## 7. Remaining website issues

None blocking. Optional future work: translate legal pages beyond DE/EN (unchanged by design).

## 8. App / release confirmation

- **No app code changed** (`GamingBooster_Pro/` untouched)
- **No installer changed**
- **No release pipeline changed**
- **`version.json` unchanged**
- **`docs/trust-latest.json` unchanged**
- **No new app release created**

## 9. Language checklist (27/27)

| Language | Code | Desktop OK | Mobile OK | Encoding OK | Missing Keys | Layout Issues | Fixed |
|----------|------|------------|-----------|-------------|--------------|---------------|-------|
| Deutsch | de | Yes | Yes | Yes | 0 | None | Yes |
| English | en | Yes | Yes | Yes | 0 | None | Yes |
| Français | fr | Yes | Yes | Yes | 0 | None | Yes |
| Español | es | Yes | Yes | Yes | 0 | None | Yes |
| Italiano | it | Yes | Yes | Yes | 0 | None | Yes |
| Português | pt | Yes | Yes | Yes | 0 | None | Yes |
| Nederlands | nl | Yes | Yes | Yes | 0 | None | Yes |
| Polski | pl | Yes | Yes | Yes | 0 | None | Yes |
| Türkçe | tr | Yes | Yes | Yes | 0 | None | Yes |
| Русский | ru | Yes | Yes | Yes | 0 | None | Yes |
| Українська | uk | Yes | Yes | Yes | 0 | None | Yes |
| Čeština | cs | Yes | Yes | Yes | 0 | None | Yes |
| العربية | ar | Yes | Yes | Yes | 0 | None | Yes |
| 中文 | zh | Yes | Yes | Yes | 0 | None | Yes |
| 日本語 | ja | Yes | Yes | Yes | 0 | None | Yes |
| 한국어 | ko | Yes | Yes | Yes | 0 | None | Yes |
| हिन्दी | hi | Yes | Yes | Yes | 0 | None | Yes |
| Svenska | sv | Yes | Yes | Yes | 0 | None | Yes |
| Română | ro | Yes | Yes | Yes | 0 | None | Yes |
| Magyar | hu | Yes | Yes | Yes | 0 | None | Yes |
| Ελληνικά | el | Yes | Yes | Yes | 0 | None | Yes |
| ไทย | th | Yes | Yes | Yes | 0 | None | Yes |
| Tiếng Việt | vi | Yes | Yes | Yes | 0 | None | Yes |
| Bahasa Indonesia | id | Yes | Yes | Yes | 0 | None | Yes |
| Dansk | da | Yes | Yes | Yes | 0 | None | Yes |
| Norsk | no | Yes | Yes | Yes | 0 | None | Yes |
| Suomi | fi | Yes | Yes | Yes | 0 | None | Yes |

---

**All website languages passed encoding and layout QA.**
