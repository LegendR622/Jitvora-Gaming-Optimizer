# REDLINE Intro – Clip-Skript (wie die App sich vorstellt)

Dauer gesamt: **~4 Sekunden** (Fast Intro) oder **~12 Sekunden** (normales Intro)

---

## Was du im Clip siehst (1:1 wie in der App)

### Bildaufbau (zentriert, schwarzer Hintergrund)
- Roter **Glow** hinter dem Logo (weicher Kreis)
- **Logo** mit rotem Schatten (R-Speedometer)
- Titel: **REDLINE GAMING OPTIMIZER** (weiß, groß)
- Untertitel: **V9.1 · UPDATE OK** (rot) – bei V9.0: „V9.0 · AI EDITION“
- Statuszeile (wechselt, siehe unten)
- **Roter Fortschrittsbalken** (420px breit)
- Unten: **Made by Tobias Immisch ❤**

### Ablauf – Texte nacheinander (für Schnitt/Marker)

| Sek. (ca.) | Status-Text | Fortschritt |
|------------|-------------|-------------|
| 0.0 | Redline Core starten... | 17% |
| 0.5 | Systemmodule laden... | 33% |
| 1.0 | Gaming Engine laden... | 50% |
| 1.5 | Security Module prüfen... | 67% |
| 2.0 | UI vorbereiten... | 83% |
| 2.5 | Redline ist einsatzbereit. | 100% |

**Fast Intro** (Einstellung): gleiche Texte, ~0,04 s pro Schritt → **~0,25 s gesamt** (sehr schnell)

Danach: **Fade/Wechsel** → Hauptfenster mit Sidebar + Dashboard

---

## So nimmst du den Clip auf (Windows)

1. App starten **ohne** `--nosplash` (Doppelklick auf EXE, nicht die Test-Version mit SKIP)
2. **Win + Alt + R** → Xbox Game Bar → Aufnahme starten
3. App schließen und neu öffnen → Intro erneut abspielen
4. Clip auf **9:16** croppen für TikTok/Reels

**Intro erzwingen:** In `%AppData%\RedlineGamingOptimizer\settings.json` → `"FastIntro": false` für langsames Intro (~12 s)

---

## Werbe-Clip-Idee (15–20 Sek. für TikTok)

| Zeit | Bild | Text-Overlay (optional) |
|------|------|-------------------------|
| 0–3 s | App-Intro (Logo + Glow) | „REDLINE V9.1“ |
| 3–4 s | „Redline ist einsatzbereit“ + Balken 100% | „100% FREE“ |
| 4–8 s | Dashboard (AI Score) | „AI System Scan“ |
| 8–12 s | Gaming / Performance | „Mehr FPS“ |
| 12–16 s | Update-Seite | „Auto-Update“ |
| 16–20 s | Pro-Karte „COMING SOON“ | „Kein Abo · Pro später“ |

**Sound:** epischer Bass-Drop beim Logo, „Whoosh“ beim Übergang zum Dashboard

---

## Voiceover (optional, Deutsch)

> „REDLINE Gaming Optimizer – Version 9.1.  
> Kostenlos für Windows.  
> Scannen, optimieren, updaten – alles in einer App.  
> Pro kommt bald. Jetzt komplett free.“

---

## App jetzt mit Intro starten

```
C:\Users\Tobi\Desktop\GamingBooster_Pro\publish\win-x64\GamingBooster_Pro.exe
```

**Nicht** mit `REDLINE_SKIP_INTRO=1` – sonst siehst du kein Intro.
