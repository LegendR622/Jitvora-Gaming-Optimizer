#!/usr/bin/env python3
"""Fix mojibake in docs/i18n-full.json and scan website files for encoding issues."""

import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
FULL_PATH = ROOT / "i18n-full.json"

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
    "hero.download": "Download v3.0.4",
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
    "feat.pro.text": "28 pro tools in Gaming, Network, System & more — browse in the app (preview in v3.0.4).",
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
    "faq.q1": "Is Jitvora free on Windows 10 and 11?",
    "faq.a1": "Yes — the full version is free on 64-bit Windows 10 and 11. No license key for v3.0.4.",
    "faq.q2": "Where do I download safely?",
    "faq.a2.html": 'Only from <a href="https://github.com/LegendR622/Jitvora-Gaming-Optimizer/releases">GitHub Releases</a>. Avoid third-party download sites.',
    "faq.q3": "Safe for online games?",
    "faq.a3": "No game file changes — Jitvora adjusts Windows settings only.",
    "faq.q4": "How do updates work?",
    "faq.a4": "Manual only — check the in-app Update page or GitHub. You confirm before installing.",
    "faq.q5": "German and English?",
    "faq.a5": "The app supports German and English in settings. This website is available in many languages.",
    "footer.aria": "Legal and resources",
    "meta.title": "Jitvora Gaming Optimizer — Windows Gaming Optimizer, FPS & PC Cleaner",
    "meta.description": "Jitvora Gaming Optimizer — Windows gaming optimizer with Network Watch, game latency monitoring, gaming network diagnostics, FPS tuning, PC cleanup, driver checks and safe Windows tweaks. Free download v3.0.4 for Windows 10/11.",
}

# Literal mojibake sequences still seen after byte-roundtrip fixes
LITERAL_FIXES = [
    ("â€\u009d", "\u2014"),
    ("â€\u009c", "\u2014"),
    ("â€\u201d", "\u2014"),
    ("â€\u201c", "\u2014"),
    ("â€™", "\u2019"),
    ("â€œ", "\u201c"),
    ("â€\u0099", "\u2019"),
    ("â€\u0098", "\u2018"),
    ("â€\u0093", "\u2013"),
    ("â€\u0094", "\u2014"),
    ("â€¦", "\u2026"),
    ("â€", "\u201d"),
    ("Ã\u0082\u00b7", "·"),
    ("Ã\u0082\u00a9", "©"),
    ("Â·", "·"),
    ("Â©", "©"),
    ("Ã\u00a0", "\u00a0"),
    ("Ã©", "é"),
    ("Ã¨", "è"),
    ("Ã ", "à"),
    ("Ã¢", "â"),
    ("Ã®", "î"),
    ("Ã´", "ô"),
    ("Ã»", "û"),
    ("Ã§", "ç"),
    ("Ã±", "ñ"),
    ("Ã³", "ó"),
    ("Ã¡", "á"),
    ("Ã­", "í"),
    ("Ãº", "ú"),
    ("Ã¤", "ä"),
    ("Ã¶", "ö"),
    ("Ã¼", "ü"),
    ("ÃŸ", "ß"),
    ("Ã„", "Ä"),
    ("Ã–", "Ö"),
    ("Ãœ", "Ü"),
    ("Ã‰", "É"),
    ("Ã…", "Å"),
    ("Ã¸", "ø"),
    ("Ã¦", "æ"),
    ("Ã†", "Æ"),
    ("Ã—", "×"),
    ("Ã¯", "ï"),
    ("Ã¸", "ø"),
    ("Ã½", "ý"),
    ("Ãµ", "õ"),
    ("Ã£", "ã"),
    ("Ãª", "ê"),
    ("Ã¥", "å"),
    ("Ã¬", "ì"),
    ("ÃŽ", "Î"),
    ("Äƒ", "ă"),
    ("Ä\u0083", "ă"),
    ("È™", "ș"),
    ("È\u009b", "ț"),
    ("È›", "ț"),
    ("Å\u0082", "ł"),
    ("Å\u0093", "“"),
    ("Å\u009e", "ž"),
    ("Å¡", "š"),
    ("Å¾", "ž"),
    ("Ä\u008d", "č"),
    ("Ä\u009b", "ě"),
    ("Ä\u0099", "ě"),
    ("Ã\u0081", "Á"),
]


def fix_utf8_pairs(s: str) -> str:
    out = []
    i = 0
    while i < len(s):
        if i + 1 < len(s):
            o1, o2 = ord(s[i]), ord(s[i + 1])
            if 0xC2 <= o1 <= 0xF4 and 0x80 <= o2 <= 0xBF:
                pair = s[i : i + 2]
                try:
                    out.append(pair.encode("latin-1").decode("utf-8"))
                    i += 2
                    continue
                except UnicodeDecodeError:
                    pass
        out.append(s[i])
        i += 1
    return "".join(out)

BAD_MARKERS = ("Ã", "Â", "â€", "ðŸ", "�")
BAD_RE = re.compile(r"Ã.|Â.|â€.|ðŸ|�|\ufffd")


def decode_latin1_chunk(chunk: str) -> str:
    for _ in range(4):
        try:
            raw = bytes(ord(c) for c in chunk if ord(c) < 256)
            nxt = raw.decode("utf-8")
        except UnicodeDecodeError:
            break
        if nxt == chunk:
            break
        chunk = nxt
    for old, new in LITERAL_FIXES:
        chunk = chunk.replace(old, new)
    return chunk


def fix_mojibake_mixed(s: str) -> str:
    if not isinstance(s, str) or not any(m in s for m in BAD_MARKERS):
        for old, new in LITERAL_FIXES:
            if old in s:
                s = s.replace(old, new)
        return s
    out = []
    i = 0
    while i < len(s):
        if ord(s[i]) < 256:
            j = i
            while j < len(s) and ord(s[j]) < 256:
                j += 1
            out.append(decode_latin1_chunk(s[i:j]))
            i = j
        else:
            out.append(s[i])
            i += 1
    result = "".join(out)
    result = fix_utf8_pairs(result)
    for old, new in LITERAL_FIXES:
        result = result.replace(old, new)
    return result


MANUAL_OVERRIDES = {
    "vi": {
        "how.s3.text": "Áp dụng sửa lỗi an toàn chỉ khi bạn xác nhận.",
        "hero.sub": "Tối ưu FPS, dọn PC, kiểm tra driver và mạng — an toàn trong một ứng dụng desktop.",
        "hero.mock.rec.text": "Xem lại trước khi áp dụng — không thay đổi tự động khi chưa xác nhận.",
        "how.lead": "Quét, xem xét, áp dụng — bạn kiểm soát.",
        "feat.clean.text": "Xóa file tạm và cache — xem trước dung lượng.",
        "faq.a1": "Có — phiên bản đầy đủ miễn phí trên Windows 10 và 11 64-bit. Không cần khóa bản quyền cho v1.9.6.",
        "safe.1.text": "Tải từ GitHub — không phải trang bên thứ ba.",
    },
}


def fix_table(table: dict) -> dict:
    out = {}
    for k, v in table.items():
        if not isinstance(v, str):
            out[k] = v
            continue
        v = fix_mojibake_mixed(v)
        v = fix_utf8_pairs(v)
        for old, new in LITERAL_FIXES:
            v = v.replace(old, new)
        out[k] = v
    return out


def has_bad_encoding(s: str) -> bool:
    if not s:
        return False
    return bool(BAD_RE.search(s) or "Ã" in s or "â€" in s or "ðŸ" in s or "�" in s)


def main() -> int:
    data = json.loads(FULL_PATH.read_text(encoding="utf-8-sig"))
    fixed = {lang: fix_table(table) for lang, table in data.items()}
    fixed["en"] = EN.copy()
    for lang, overrides in MANUAL_OVERRIDES.items():
        if lang in fixed:
            fixed[lang].update(overrides)

    keys = list(EN.keys())
    errors = []
    for lang, table in sorted(fixed.items()):
        missing = [k for k in keys if k not in table]
        empty = [k for k in keys if not str(table.get(k, "")).strip()]
        bad = [k for k in keys if has_bad_encoding(table.get(k, ""))]
        if missing:
            errors.append(f"{lang}: missing keys {missing}")
        if empty:
            errors.append(f"{lang}: empty keys {empty}")
        if bad:
            errors.append(f"{lang}: encoding issues in {bad}")

    FULL_PATH.write_text(
        json.dumps(fixed, ensure_ascii=False, indent=2) + "\n",
        encoding="utf-8",
        newline="\n",
    )
    print(f"Fixed i18n-full.json ({len(fixed)} locales, {len(keys)} keys each)")

    if errors:
        print("LOCALE ERRORS:")
        for e in errors:
            print(" ", e)
        return 1
    print("ALL LOCALES CLEAN")
    return 0


if __name__ == "__main__":
    sys.exit(main())
