#!/usr/bin/env python3
"""Scan website i18n/HTML files for encoding and completeness issues."""

import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
FULL = ROOT / "i18n-full.json"
SCAN_FILES = [
    ROOT / "index.html",
    ROOT / "site-i18n.js",
    ROOT / "site-lang.js",
    ROOT / "landing.css",
]

BAD_RE = re.compile(r"Ã.|Â.|â€.|ðŸ|�|\ufffd|Å.|Ä.")
LANG_META = [
    "de", "en", "fr", "es", "it", "pt", "nl", "pl", "tr", "ru", "uk", "cs",
    "ar", "zh", "ja", "ko", "hi", "sv", "ro", "hu", "el", "th", "vi", "id",
    "da", "no", "fi",
]


def main() -> int:
    data = json.loads(FULL.read_text(encoding="utf-8"))
    keys = list(data["en"].keys())
    errors = 0

    print(f"Locales: {len(data)} (expected {len(LANG_META)})")
    for code in LANG_META:
        if code not in data:
            print(f"MISSING locale: {code}")
            errors += 1

    for lang, table in sorted(data.items()):
        missing = [k for k in keys if k not in table]
        empty = [k for k in keys if not str(table.get(k, "")).strip()]
        bad = [k for k in keys if BAD_RE.search(table.get(k, ""))]
        if missing or empty or bad:
            errors += 1
            print(f"{lang}: missing={len(missing)} empty={len(empty)} bad={len(bad)}")
            if bad:
                print("  bad keys:", ", ".join(bad[:8]))

    for fp in SCAN_FILES:
        if not fp.exists():
            continue
        text = fp.read_text(encoding="utf-8-sig")
        hits = [i + 1 for i, line in enumerate(text.splitlines()) if BAD_RE.search(line)]
        if hits:
            errors += 1
            print(f"{fp.name}: {len(hits)} bad lines")

    dup = [c for c in data if c not in LANG_META]
    if dup:
        print("Extra locales:", dup)

    if errors:
        print(f"SCAN FAILED ({errors} issue groups)")
        return 1
    print("SCAN OK — all locales complete, no encoding markers in website files")
    return 0


if __name__ == "__main__":
    sys.exit(main())
