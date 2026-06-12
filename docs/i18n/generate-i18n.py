#!/usr/bin/env python3
"""Regenerate docs/site-i18n.js from docs/i18n-full.json + lang metadata."""

import json
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
FULL = json.loads((ROOT / "i18n-full.json").read_text(encoding="utf-8"))

LANG_META = {
    "de": {"label": "Deutsch", "primary": True},
    "en": {"label": "English", "primary": True},
    "fr": {"label": "Français"},
    "es": {"label": "Español"},
    "it": {"label": "Italiano"},
    "pt": {"label": "Português"},
    "nl": {"label": "Nederlands"},
    "pl": {"label": "Polski"},
    "tr": {"label": "Türkçe"},
    "ru": {"label": "Русский"},
    "uk": {"label": "Українська"},
    "cs": {"label": "Čeština"},
    "ar": {"label": "العربية", "rtl": True},
    "zh": {"label": "中文"},
    "ja": {"label": "日本語"},
    "ko": {"label": "한국어"},
    "hi": {"label": "हिन्दी"},
    "sv": {"label": "Svenska"},
    "ro": {"label": "Română"},
    "hu": {"label": "Magyar"},
    "el": {"label": "Ελληνικά"},
    "th": {"label": "ไทย"},
    "vi": {"label": "Tiếng Việt"},
    "id": {"label": "Bahasa Indonesia"},
    "da": {"label": "Dansk"},
    "no": {"label": "Norsk"},
    "fi": {"label": "Suomi"},
}

# English base = canonical key order from de (all locales share keys)
KEYS = list(FULL["de"].keys())
EN = {k: FULL["de"][k] for k in KEYS}  # placeholder wrong

# Build EN from dedicated export or first complete set - use fr's structure with English text
# Read existing site-i18n.js base is hard; store EN in i18n-full as optional
if "en" in FULL:
    EN = FULL["en"]
else:
    # English strings (source of truth for en locale)
    EN = {
        "nav.features": "Features",
        "nav.faq": "FAQ",
        "nav.trust": "Trust",
        "nav.github": "GitHub",
        "nav.home": "Home",
        "nav.privacy": "Privacy",
        "nav.imprint": "Imprint",
        "nav.terms": "Terms",
        "nav.support": "Support",
        "nav.licenses": "Licenses",
        "nav.changelog": "Changelog",
        "nav.sitemap": "Sitemap",
        "nav.releases": "Releases",
        "lang.label": "Language",
        "lang.note": "App: German & English only · Website in many languages",
        "lang.more": "More languages",
        "hero.badge": "Windows 10/11 · 64-bit · Free",
        "hero.sub": "Tune FPS, clean your PC, check drivers and fix network issues — safely from one desktop app.",
        "hero.download": "Download V1.9.4",
        "hero.trust": "Trust & SHA256",
        "hero.trustAria": "Trust highlights",
        "hero.pill1": "Official GitHub release",
        "hero.pill2": "Manual updates only",
        "hero.pill3": "No game file changes",
        "hero.pill4": "SHA256 available",
        "hero.mock.cpu": "Ready",
        "hero.mock.gpu": "Active",
        "hero.mock.fps.title": "Gaming FPS Boost",
        "hero.mock.fps.text": "Game Mode · power plan · safe Windows tweaks",
        "hero.mock.rec.title": "Recommendations",
        "hero.mock.rec.text": "Review before apply — no automatic changes without confirmation.",
        "feat.label": "Features",
        "feat.title": "Built for your gaming PC",
        "feat.lead": "Six core tools. One app. Plus Pro Center with 28 advanced tools (preview).",
        "feat.fps.title": "FPS tuning",
        "feat.fps.text": "Game Mode, power plan and safe Windows tweaks for gaming.",
        "feat.clean.title": "Cleaner",
        "feat.clean.text": "Remove temp files and cache with size preview first.",
        "feat.driver.title": "Driver check",
        "feat.driver.text": "Scan hardware and open official vendor download pages.",
        "feat.net.title": "Network tools",
        "feat.net.text": "Adapter info, DNS flush and ping checks in one place.",
        "feat.sec.title": "Security check",
        "feat.sec.text": "Quick security overview and system status at a glance.",
        "feat.repair.title": "Repair tools",
        "feat.repair.text": "Shortcuts to SFC, DISM and common Windows repair actions.",
        "feat.pro.title": "Pro Center",
        "feat.pro.text": "28 pro tools in Gaming, Network, System & more — browse in the app (preview in V1.9.4).",
        "how.label": "How it works",
        "how.title": "Three simple steps",
        "how.lead": "Scan, review, then apply — you stay in control.",
        "how.s1.title": "Scan",
        "how.s1.text": "Check your system, hardware and gaming-related settings.",
        "how.s2.title": "Review",
        "how.s2.text": "See recommendations before anything changes.",
        "how.s3.title": "Apply",
        "how.s3.text": "Apply safe fixes only when you confirm.",
        "safe.label": "Safety",
        "safe.title": "Trust built in",
        "safe.lead": "Honest, local and transparent — no overpromising.",
        "safe.1.title": "Official GitHub releases only",
        "safe.1.text": "Download from GitHub — not third-party sites.",
        "safe.2.title": "Manual update confirmation",
        "safe.2.text": "Updates require your approval in the app.",
        "safe.3.title": "SHA256 hash available",
        "safe.3.html": 'Verify via <a href="trust.html">Trust page</a> or <a href="trust-latest.json">trust-latest.json</a>.',
        "safe.4.title": "No game file modifications",
        "safe.4.text": "Windows settings only — anti-cheat friendly approach.",
        "safe.5.title": "Safe Windows settings only",
        "safe.5.text": "Recommendations before changes where possible.",
        "safe.6.title": "No cloud account required",
        "safe.6.text": "Runs locally on your PC. No login needed.",
        "faq.label": "FAQ",
        "faq.title": "Quick answers",
        "faq.q1": "Is Redline free on Windows 10 and 11?",
        "faq.a1": "Yes — the full version is free on 64-bit Windows 10 and 11. No license key for V1.9.4.",
        "faq.q2": "Where do I download safely?",
        "faq.a2.html": 'Only from <a href="https://github.com/LegendR622/Redline-Gaming-Optimizer/releases">GitHub Releases</a>. Avoid third-party download sites.',
        "faq.q3": "Safe for online games?",
        "faq.a3": "No game file changes — Redline adjusts Windows settings only.",
        "faq.q4": "How do updates work?",
        "faq.a4": "Manual only — check the in-app Update page or GitHub. You confirm before installing.",
        "faq.q5": "German and English?",
        "faq.a5": "The app supports German and English in settings. This website is available in many languages.",
        "footer.aria": "Legal and resources",
        "meta.title": "Redline Gaming Optimizer — Windows Gaming Optimizer, FPS & PC Cleaner",
        "meta.description": "Redline Gaming Optimizer — premium Windows gaming optimizer for FPS tuning, PC cleaner, driver check and network tools. Safe Windows tweaks. Free download V1.9.4 for Windows 10/11.",
    }

assert set(KEYS) == set(EN.keys()), (set(KEYS) ^ set(EN.keys()))
for code, table in FULL.items():
    assert set(table.keys()) == set(KEYS), f"{code} key mismatch"


def js_str(s: str) -> str:
    return json.dumps(s, ensure_ascii=False)


def js_obj(obj: dict) -> str:
    lines = ["{"]
    for i, k in enumerate(KEYS):
        comma = "," if i < len(KEYS) - 1 else ""
        lines.append(f'    "{k}": {js_str(obj[k])}{comma}')
    lines.append("  }")
    return "\n".join(lines)


def js_meta(meta: dict) -> str:
    parts = []
    for k, v in meta.items():
        if isinstance(v, bool):
            parts.append(f"{k}: {str(v).lower()}")
        else:
            parts.append(f'{k}: {js_str(v)}')
    return "{ " + ", ".join(parts) + " }"


meta_lines = []
for code, m in LANG_META.items():
    meta_lines.append(f"    {code}: {js_meta(m)},")

i18n_lines = ['    en: EN,']
for code in LANG_META:
    if code == "en":
        continue
    i18n_lines.append(f"    {code}: {js_obj(FULL[code])},")

out = f"""/* Auto-generated by generate-i18n.py — edit i18n-full.json then re-run */
(function (global) {{
  "use strict";

  var EN = {js_obj(EN)};

  global.REDLINE_LANG_META = {{
{chr(10).join(meta_lines)}
  }};

  global.REDLINE_LANG_CODES = Object.keys(global.REDLINE_LANG_META);
  global.REDLINE_I18N_BASE = EN;
  global.REDLINE_I18N = {{
{chr(10).join(i18n_lines)}
  }};
}})(typeof window !== "undefined" ? window : globalThis);
"""

(ROOT / "site-i18n.js").write_text(out, encoding="utf-8", newline="\n")
print("OK: site-i18n.js regenerated")
