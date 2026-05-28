# Mit ChatGPT (Plus) bessere Promo – Copy & Paste

Ich (Cursor) kann **nicht** in deinen Browser oder ChatGPT einloggen.  
So arbeitet ihr **zusammen**: Du nutzt ChatGPT für Text/Musik-Ideen, Cursor baut App + Video.

---

## Prompt 1 – Besseres Video-Konzept (in ChatGPT einfügen)

```
Du bist Marketing-Experte für PC/Gaming-Software.

Produkt: REDLINE Gaming Optimizer V9.1 (Windows, WPF, dunkles UI, rot/schwarz).
FREE Edition, Pro = Coming Soon.
Features: Dashboard AI-Scan, Gaming-Profile, Performance, Cleaner, Autostart,
Security, Network, Driver, Repair, Auto-Update von GitHub.

Ich habe einen ~78 Sekunden Screen-Recording-Clip (1920x1080), der automatisch
durch alle Seiten klickt.

Aufgabe:
1. Schreibe eine Shot-Liste (Sekunde → was im Bild → Text-Overlay DE)
2. Empfehle 3 lizenzfreie Musik-Stile (kein Summen) von Pixabay/Mixkit – Suchbegriffe
3. TikTok Caption DE + EN (max 150 Zeichen Hook)
4. Instagram Reels: 3 Hook-Varianten für die ersten 2 Sekunden
5. Was soll im Video UNBEDINGT sichtbar sein pro Seite (1 Satz pro Seite)

Zielgruppe: Gamer 16–30, Windows 10/11, Deutsch primär.
```

---

## Prompt 2 – Text-Overlays für CapCut/Premiere

```
Hier ist mein REDLINE App Promo Video. Erstelle 8 kurze Text-Overlays (max 4 Wörter),
die ich oben im Video einblenden kann – Timing vorschlagen:

0-5s: Intro
5-12s: Dashboard
12-20s: Gaming + Performance
20-35s: Cleaner, Autostart, Security
35-50s: Network, Driver, Repair
50-60s: Update (Auto-Update GitHub)
60-78s: Settings + FREE / Coming Soon

Stil: Gaming, kraftvoll, Deutsch, kein Clickbait-Lügen.
```

---

## Prompt 3 – Eigene Musik finden

```
Ich brauche lizenzfreie Hintergrundmusik für ein 78s Gaming-Tool-Promo-Video.
Vibe: modern, energetic, electronic – NICHT nur Drone/Summen.
Quellen: Pixabay, Mixkit, YouTube Audio Library.

Gib mir:
- 5 konkrete Suchbegriffe
- 3 Track-Typen (BPM-Bereich)
- Wie ich den MP3 in mein Video lege (ffmpeg Befehl, Musik leiser als Sprache)
```

Datei danach speichern als: `GamingBooster_Pro\video\bg_music.mp3`

Dann neu rendern:
```powershell
powershell -File scripts\create-intro-mp4.ps1 -MusicPath "C:\Pfad\track.mp3"
```

---

## Prompt 4 – Thumbnail (DALL·E in ChatGPT Plus)

```
Erstelle ein YouTube/TikTok Thumbnail-Konzept (Beschreibung für Bild-KI):

- Schwarzer Hintergrund, roter Neon-Glow
- Logo: stylisches R mit Tachonadel
- Text: REDLINE V9.1 | 100% FREE
- Stil: AAA Gaming Launcher, kein Clip-Art
- Format: 1280x720 und 1080x1920 Variante beschreiben
```

---

## Was Cursor automatisch macht

```powershell
# Voller Rundgang + Scroll + Musik (~78s)
powershell -File C:\Users\Tobi\Desktop\GamingBooster_Pro\scripts\create-intro-mp4.ps1

# TikTok Hochformat zusätzlich
powershell -File ...\create-intro-mp4.ps1 -Vertical

# Eigene Musik von ChatGPT-Empfehlung
powershell -File ...\create-intro-mp4.ps1 -MusicPath "C:\Downloads\track.mp3"
```

Output: `video\REDLINE_Promo.mp4` + Kopie auf Desktop

---

## Ehrliche Grenzen

| ChatGPT Plus (du) | Cursor (ich) |
|-------------------|--------------|
| Texte, Hooks, Musik-Tipps, Thumbnail-Ideen | App-Code, Demo-Tour, Video rendern |
| CapCut-Anleitung, Storyboard | Auto-Update, GitHub |
| DALL·E Bilder | Screen-Recording der echten App |

**Bestes Ergebnis:** ChatGPT liefert Story + Musik → Cursor/Script nimmt echte App auf → du schneidest optional in CapCut 2–3 Texte drüber.
