#!/usr/bin/env python3
"""Validate the Jitvora website localization.

Checks the dictionary (docs/i18n-full.json), the generated bundle
(docs/site-i18n.js) and the HTML that consumes them.

Run:  python docs/i18n/validate-i18n.py
Exit: 0 = clean, 1 = errors found (warnings alone do not fail the run).
"""

from __future__ import annotations

import json
import re
import sys
from pathlib import Path

DOCS = Path(__file__).resolve().parent.parent
FULL = DOCS / "i18n-full.json"
BUNDLE = DOCS / "site-i18n.js"
BOOT = DOCS / "site-lang-boot.js"
GENERATOR = DOCS / "i18n" / "generate-i18n.py"

BASE = "de"          # canonical key order lives here
FALLBACK = "en"      # what a visitor sees when a string is missing

# Placeholders the runtime resolves ({version} -> v3.0.7, {file} -> setup name).
PLACEHOLDER = re.compile(r"\{[a-zA-Z_][a-zA-Z0-9_]*\}")

# Patterns that must never survive into rendered markup.
LEAKED = [
    (re.compile(r"\$\{[^}]*\}"), "JS template literal"),
    (re.compile(r"\{\{[^}]*\}\}"), "mustache placeholder"),
    (re.compile(r"\bundefined\b"), "literal 'undefined'"),
    (re.compile(r"\bNaN\b"), "literal 'NaN'"),
    (re.compile(r"REDLINE_I18N|REDLINE_LANG"), "internal object name"),
]

# Only keys ending in .html are allowed to carry markup.
TAG = re.compile(r"<[a-zA-Z/][^>]*>")

# Product names and technical terms that are deliberately the same everywhere.
# Flagging these as "untranslated" would bury the warnings that matter.
SAME_ON_PURPOSE = {
    "netwatch.label", "preview.cap2", "feat.pro.title", "nav.github",
    "netwatch.row2", "netwatch.node.router", "netwatch.rowLive",
    "hero.win.online", "nav.faq", "faq.label", "preview.cap1", "preview.label",
    "nav.menu", "nav.trust", "hero.trust", "perf.label", "dl.label",
    "dl.navCta", "hero.pill4", "safe.3.title", "ui.preview", "ui.optional",
    "feat.pro.badge", "nav.changelog", "hero.download", "dl.compat",
}

errors: list[str] = []
warnings: list[str] = []


def err(msg: str) -> None:
    errors.append(msg)


def warn(msg: str) -> None:
    warnings.append(msg)


# ── load ────────────────────────────────────────────────────────────────────
try:
    raw = FULL.read_text(encoding="utf-8")
    data = json.loads(raw)
except json.JSONDecodeError as exc:
    print(f"FATAL  {FULL.name} is not valid JSON: {exc}")
    sys.exit(1)

if BASE not in data:
    print(f"FATAL  base locale '{BASE}' missing from {FULL.name}")
    sys.exit(1)

base_keys = list(data[BASE].keys())
base_set = set(base_keys)

# ── duplicate keys survive json.loads silently, so re-scan the raw text ─────
for locale_block in re.finditer(r'"([a-z]{2})":\s*\{(.*?)\n  \}', raw, re.S):
    code, body = locale_block.group(1), locale_block.group(2)
    seen: dict[str, int] = {}
    for k in re.findall(r'^\s{4}"([^"]+)":', body, re.M):
        seen[k] = seen.get(k, 0) + 1
    dupes = sorted(k for k, n in seen.items() if n > 1)
    if dupes:
        err(f"[{code}] duplicate keys in the JSON source: {', '.join(dupes)}")

# ── locale identifiers ──────────────────────────────────────────────────────
gen_src = GENERATOR.read_text(encoding="utf-8") if GENERATOR.exists() else ""
declared = set(re.findall(r'^\s{4}"([a-z]{2})":\s*\{"label"', gen_src, re.M))
for code in data:
    if not re.fullmatch(r"[a-z]{2}", code):
        err(f"unsupported locale identifier '{code}' (expected ISO 639-1)")
    if declared and code not in declared:
        err(f"locale '{code}' has translations but no entry in LANG_META")
for code in declared - set(data):
    err(f"locale '{code}' is offered in the switcher but has no translations")

# ── per-locale key + value checks ───────────────────────────────────────────
for code, table in sorted(data.items()):
    keys = set(table)

    for k in sorted(base_set - keys):
        err(f"[{code}] missing key: {k}")
    for k in sorted(keys - base_set):
        err(f"[{code}] extra key not in base '{BASE}': {k}")

    for k in sorted(keys & base_set):
        value = table[k]

        if not isinstance(value, str):
            err(f"[{code}] {k}: value is {type(value).__name__}, expected string")
            continue
        if not value.strip():
            err(f"[{code}] {k}: empty translation")
            continue

        # placeholders must match the base string exactly
        want = sorted(PLACEHOLDER.findall(data[BASE][k]))
        got = sorted(PLACEHOLDER.findall(value))
        if want != got:
            err(f"[{code}] {k}: placeholder mismatch — base has {want or '[]'}, "
                f"translation has {got or '[]'}")

        for pattern, label in LEAKED:
            if pattern.search(value):
                err(f"[{code}] {k}: contains {label}: {value[:60]!r}")

        # markup only where the key opts in
        if TAG.search(value) and not k.endswith(".html"):
            err(f"[{code}] {k}: contains HTML but the key does not end in '.html' "
                f"(site-lang.js will escape it): {value[:60]!r}")

        # external links inside translations need rel=noopener
        for anchor in re.findall(r"<a\s[^>]*href=\"https?://[^\"]*\"[^>]*>", value):
            if "rel=" not in anchor:
                err(f"[{code}] {k}: external link without rel attribute")

        # an untranslated locale is a warning, not an error — but flag exact
        # copies of the English string in locales that are neither en nor de
        if (code not in (BASE, FALLBACK) and k not in SAME_ON_PURPOSE
                and value == data[FALLBACK].get(k)):
            warn(f"[{code}] {k}: identical to {FALLBACK} (untranslated fallback)")

# ── HTML consumers ──────────────────────────────────────────────────────────
html_files = sorted(DOCS.glob("*.html"))
used_keys: set[str] = set()
ATTRS = ("data-i18n", "data-i18n-aria", "data-i18n-alt", "data-i18n-ph", "data-i18n-title")

for page in html_files:
    src = page.read_text(encoding="utf-8")
    body = src[src.index("<body"):] if "<body" in src else src

    for attr in ATTRS:
        for k in re.findall(rf'{attr}="([^"]+)"', src):
            used_keys.add(k)
            if k not in base_set:
                err(f"{page.name}: uses translation key '{k}' which does not exist")

    # placeholders that no runtime path resolves
    for m in PLACEHOLDER.finditer(body):
        token = m.group(0)
        if token in ("{version}", "{file}"):
            # resolved by landing.js — only valid on pages that load it
            if "landing.js" not in src:
                err(f"{page.name}: {token} used but landing.js is not loaded to resolve it")
        else:
            err(f"{page.name}: unresolved placeholder {token}")

    for pattern, label in LEAKED:
        for m in pattern.finditer(body):
            err(f"{page.name}: rendered markup contains {label}: {m.group(0)[:40]!r}")

    # a translation key accidentally left as visible text
    for m in re.finditer(r">\s*((?:[a-z]+\.){1,3}[a-zA-Z]+)\s*<", body):
        token = m.group(1)
        if token in base_set:
            err(f"{page.name}: translation key '{token}' rendered as visible text")

obsolete = sorted(base_set - used_keys)
if obsolete:
    warn(f"{len(obsolete)} key(s) defined but never used in HTML: {', '.join(obsolete)}")

# ── generated bundle is in sync ─────────────────────────────────────────────
if BUNDLE.exists():
    bundle = BUNDLE.read_text(encoding="utf-8")
    for code in data:
        if not re.search(rf"^\s+{code}: ", bundle, re.M):
            err(f"site-i18n.js is missing locale '{code}' — re-run generate-i18n.py")
    for pattern, label in LEAKED[:2]:      # template syntax only
        if pattern.search(bundle):
            err(f"site-i18n.js contains {label}")
    sample = data[BASE][base_keys[0]]
    if sample not in bundle:
        err("site-i18n.js is stale — re-run generate-i18n.py after editing i18n-full.json")
else:
    err("site-i18n.js not found — run generate-i18n.py")

# ── boot script must know every locale (it runs before the bundle) ──────────
if BOOT.exists():
    boot = BOOT.read_text(encoding="utf-8")
    boot_codes = set(re.findall(r'"([a-z]{2})"', boot.split("var rtl")[0]))
    for code in set(data) - boot_codes:
        err(f"site-lang-boot.js does not list '{code}' — the first paint would "
            f"fall back to {FALLBACK} for those visitors")

# ── report ──────────────────────────────────────────────────────────────────
locales = len(data)
print(f"locales: {locales}   keys/locale: {len(base_keys)}   "
      f"html pages: {len(html_files)}   keys used in HTML: {len(used_keys)}")

if warnings:
    print(f"\n{len(warnings)} warning(s):")
    for w in warnings[:40]:
        print(f"  WARN  {w}")
    if len(warnings) > 40:
        print(f"  … {len(warnings) - 40} more")

if errors:
    print(f"\n{len(errors)} error(s):")
    for e in errors[:80]:
        print(f"  ERR   {e}")
    if len(errors) > 80:
        print(f"  … {len(errors) - 80} more")
    sys.exit(1)

print("\nOK — no errors.")
sys.exit(0)
