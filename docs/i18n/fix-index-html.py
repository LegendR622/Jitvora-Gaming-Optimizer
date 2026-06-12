#!/usr/bin/env python3
"""Fix encoding in docs/index.html fallback strings and replace broken emoji icons."""

import re
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
INDEX = ROOT / "index.html"

REPLACEMENTS = [
    ("â€\u009d", "\u2014"),
    ("â€\u009c", "\u2014"),
    ("â€\u201d", "\u2014"),
    ("â€\u201c", "\u2014"),
    ("â€\u0099", "\u2019"),
    ("â€\u0093", "\u2013"),
    ("â€\u0094", "\u2014"),
    ("â€™", "\u2019"),
    ("â€œ", "\u201c"),
    ("â€", "\u2014"),
    ("Â·", "\u00b7"),
    ("Â©", "\u00a9"),
    ("fÃ¼r", "f\u00fcr"),
    ("Ã¼", "\u00fc"),
    ("Ã¤", "\u00e4"),
    ("Ã¶", "\u00f6"),
    ("ÃŸ", "\u00df"),
    ("Ã„", "\u00c4"),
]

ICONS = [
    '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M13 2L3 14h9l-1 8 10-12h-9l1-8z"/></svg>',
    '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M3 6h18M8 6V4h8v2M6 6l1 14h10l1-14"/></svg>',
    '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="2" y="3" width="20" height="14" rx="2"/><path d="M8 21h8M12 17v4"/></svg>',
    '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><path d="M2 12h20M12 2a15 15 0 0 1 0 20M12 2a15 15 0 0 0 0 20"/></svg>',
    '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>',
    '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M14.7 6.3a1 1 0 0 0 0 1.4l1.6 1.6a1 1 0 0 0 1.4 0l3.77-3.77a6 6 0 0 1-7.94 7.94l-6.91 6.91a2.12 2.12 0 0 1-3-3l6.91-6.91a6 6 0 0 1 7.94-7.94l-3.76 3.76z"/></svg>',
    '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M6 3h12l4 6-10 13L2 9z"/></svg>',
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


def replace_feature_icons(text: str) -> str:
    icons = iter(ICONS)

    def repl(_match: re.Match) -> str:
        return f'<div class="feature-icon" aria-hidden="true">{next(icons)}</div>'

    return re.sub(
        r'<div class="feature-icon" aria-hidden="true">[^<]*</div>',
        repl,
        text,
        count=len(ICONS),
    )


def main() -> None:
    text = INDEX.read_text(encoding="utf-8-sig")
    text = fix_utf8_pairs(text)
    for old, new in REPLACEMENTS:
        text = text.replace(old, new)
    text = replace_feature_icons(text)
    INDEX.write_text(text, encoding="utf-8", newline="\n")
    markers = ("Ã", "â€", "ðŸ", "Â·")
    print("index.html fixed; bad markers:", {m: text.count(m) for m in markers})


if __name__ == "__main__":
    main()
