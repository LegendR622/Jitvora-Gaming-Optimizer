#!/usr/bin/env python3
"""Restore broken CJK/script locale tables from last known-good git commit."""

import json
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
FULL = ROOT / "i18n-full.json"
GOOD_REF = "debbd84:docs/i18n-full.json"
MOCK_REF = "fe23cf5:docs/i18n-full.json"
SCRIPT_LOCALES = ("ja", "zh", "ko", "th", "hi", "ar", "ru", "uk", "el")


def load_git_json(ref: str) -> dict:
    raw = subprocess.check_output(["git", "show", ref], cwd=ROOT.parent)
    return json.loads(raw.decode("utf-8"))


def bump_version(s: str) -> str:
    return (
        s.replace("V1.9.4", "v1.9.6")
        .replace("V1.9.5", "v1.9.6")
        .replace("v1.9.4", "v1.9.6")
        .replace("v1.9.5", "v1.9.6")
    )


def main() -> None:
    good = load_git_json(GOOD_REF)
    mock = load_git_json(MOCK_REF)
    cur = json.loads(FULL.read_text(encoding="utf-8-sig"))
    keys = list(cur["en"].keys())

    for lang in SCRIPT_LOCALES:
        base = dict(good.get(lang, {}))
        # hero mock keys added later
        for k, v in mock.get(lang, {}).items():
            if k.startswith("hero.mock.") and v:
                base[k] = v
        # fill any newer keys from current
        for k in keys:
            if k not in base and cur.get(lang, {}).get(k):
                base[k] = cur[lang][k]
        cur[lang] = {k: bump_version(base[k]) for k in keys if k in base}

    FULL.write_text(json.dumps(cur, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print("Restored script locales:", ", ".join(SCRIPT_LOCALES))


if __name__ == "__main__":
    main()
