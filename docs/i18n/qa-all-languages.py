#!/usr/bin/env python3
"""Browser-less language QA: verify all locale strings and simulate page apply."""

import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
FULL = json.loads((ROOT / "i18n-full.json").read_text(encoding="utf-8"))
INDEX = (ROOT / "index.html").read_text(encoding="utf-8")

LANGS = [
    "de", "en", "fr", "es", "it", "pt", "nl", "pl", "tr", "ru", "uk", "cs",
    "ar", "zh", "ja", "ko", "hi", "sv", "ro", "hu", "el", "th", "vi", "id",
    "da", "no", "fi",
]

BAD = re.compile(r"Ã|Â.|â€|ðŸ|�|\ufffd|\[missing\]|\{\{")
KEYS = list(FULL["en"].keys())

# Long strings that may need mobile shortening (warn only)
MOBILE_WARN_LEN = 120


def main() -> int:
    rows = []
    errors = 0
    for lang in LANGS:
        table = FULL.get(lang, {})
        missing = [k for k in KEYS if k not in table]
        empty = [k for k in KEYS if not str(table.get(k, "")).strip()]
        bad_keys = [k for k in KEYS if BAD.search(str(table.get(k, "")))]
        long_keys = [
            k for k in KEYS
            if len(str(table.get(k, ""))) > MOBILE_WARN_LEN and k.startswith(("hero.", "nav.", "feat."))
        ]
        ok = not (missing or empty or bad_keys)
        if not ok:
            errors += 1
        rows.append({
            "lang": lang,
            "ok": ok,
            "missing": len(missing),
            "empty": len(empty),
            "bad": len(bad_keys),
            "long": len(long_keys),
        })
        if bad_keys:
            print(f"FAIL {lang}: bad encoding in {bad_keys[:5]}")
        if missing:
            print(f"FAIL {lang}: missing {missing[:5]}")
        if empty:
            print(f"FAIL {lang}: empty {empty[:5]}")

    if "charset" not in INDEX.lower():
        print("FAIL index.html missing charset")
        errors += 1

    print(f"\nChecked {len(LANGS)} languages; failures: {errors}")
    out = ROOT.parent / "WEBSITE_LANGUAGE_QA_ROWS.json"
    out.write_text(json.dumps(rows, indent=2), encoding="utf-8")
    return 1 if errors else 0


if __name__ == "__main__":
    sys.exit(main())
