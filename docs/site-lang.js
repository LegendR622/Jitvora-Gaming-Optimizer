(function () {
  "use strict";

  var STORAGE_KEY = "redline-site-lang";
  var switcherSeq = 0;
  var PRIMARY = ["de", "en"];

  function meta() {
    return window.REDLINE_LANG_META || { de: { primary: true }, en: { primary: true } };
  }

  function codes() {
    return window.REDLINE_LANG_CODES || PRIMARY.slice();
  }

  function strings() {
    return window.REDLINE_I18N || {};
  }

  function isPrimary(code) {
    return PRIMARY.indexOf(code) >= 0;
  }

  function legalLang(code) {
    return code === "de" ? "de" : "en";
  }

  function detectLang() {
    var stored = null;
    try { stored = localStorage.getItem(STORAGE_KEY); } catch (e) { /* private mode */ }
    if (stored && codes().indexOf(stored) >= 0) return stored;
    var nav = (navigator.language || navigator.userLanguage || "en").toLowerCase();
    var all = codes();
    var i;
    for (i = 0; i < all.length; i++) {
      if (nav === all[i] || nav.indexOf(all[i] + "-") === 0) return all[i];
    }
    return nav.indexOf("de") === 0 ? "de" : "en";
  }

  function getLang() {
    return document.documentElement.getAttribute("data-site-lang") || detectLang();
  }

  function applyDocumentLang(code) {
    var m = meta()[code] || {};
    document.documentElement.setAttribute("data-site-lang", code);
    document.documentElement.setAttribute("data-legal-lang", legalLang(code));
    document.documentElement.lang = code;
    document.documentElement.dir = m.rtl ? "rtl" : "ltr";
  }

  function setLang(code) {
    if (codes().indexOf(code) < 0) return;
    try { localStorage.setItem(STORAGE_KEY, code); } catch (e) { /* private mode */ }
    applyDocumentLang(code);
    applyStrings(code);
    syncUi(code);
    legalFallbackNote(code);
  }

  function applyStrings(code) {
    var all = strings();
    var table = all[code] || all.en;
    if (!table) return;

    document.querySelectorAll("[data-i18n-aria]").forEach(function (el) {
      var key = el.getAttribute("data-i18n-aria");
      if (table[key]) el.setAttribute("aria-label", table[key]);
    });

    document.querySelectorAll("[data-i18n-alt]").forEach(function (el) {
      var key = el.getAttribute("data-i18n-alt");
      if (table[key]) el.setAttribute("alt", table[key]);
    });

    document.querySelectorAll("[data-i18n]").forEach(function (el) {
      var key = el.getAttribute("data-i18n");
      var htmlKey = key + ".html";
      if (key.slice(-5) === ".html" && table[key]) el.innerHTML = table[key];
      else if (table[htmlKey]) el.innerHTML = table[htmlKey];
      else if (table[key]) el.textContent = table[key];
    });

    var titleEl = document.querySelector("title[data-i18n-title]");
    if (titleEl && table["meta.title"]) titleEl.textContent = table["meta.title"];

    var desc = document.querySelector('meta[name="description"][data-i18n-desc]');
    if (desc && table["meta.description"]) desc.setAttribute("content", table["meta.description"]);
  }

  function syncUi(code) {
    document.querySelectorAll(".lang-btn[data-set-lang]").forEach(function (btn) {
      var active = btn.getAttribute("data-set-lang") === code;
      btn.classList.toggle("is-active", active);
      btn.setAttribute("aria-pressed", active ? "true" : "false");
      if (active) btn.setAttribute("aria-current", "true");
      else btn.removeAttribute("aria-current");
    });

    document.querySelectorAll(".lang-select-more").forEach(function (sel) {
      sel.value = code;
      // the shortcut already covers de/en, so only highlight the select when it
      // is the control that carries the current choice
      sel.classList.toggle("is-active", !isPrimary(code));
    });
  }

  function nativeName(code) {
    var m = meta()[code] || {};
    return m.label || code.toUpperCase();
  }

  /* The long list stays a native <select>: it is keyboard-operable, closes on
     Escape and on outside click, never renders outside the viewport and needs
     no role="menu" keyboard model. Only the DE/EN shortcut is custom. */
  function buildSwitcher() {
    var wrap = document.createElement("div");
    wrap.className = "lang-switch";

    var row = document.createElement("div");
    row.className = "lang-switch-row";

    var globe = document.createElementNS("http://www.w3.org/2000/svg", "svg");
    globe.setAttribute("class", "lang-globe");
    globe.setAttribute("viewBox", "0 0 24 24");
    globe.setAttribute("width", "15");
    globe.setAttribute("height", "15");
    globe.setAttribute("fill", "none");
    globe.setAttribute("stroke", "currentColor");
    globe.setAttribute("stroke-width", "1.8");
    globe.setAttribute("aria-hidden", "true");
    globe.innerHTML = '<circle cx="12" cy="12" r="9"/><path d="M3 12h18"/>'
                    + '<path d="M12 3a15 15 0 0 1 0 18a15 15 0 0 1 0-18z"/>';
    row.appendChild(globe);

    var btns = document.createElement("div");
    btns.className = "lang-switch-btns";
    PRIMARY.forEach(function (code) {
      var btn = document.createElement("button");
      btn.type = "button";
      btn.className = "lang-btn";
      btn.setAttribute("data-set-lang", code);
      btn.textContent = code.toUpperCase();
      btn.title = nativeName(code);
      btn.addEventListener("click", function () { setLang(code); });
      btns.appendChild(btn);
    });
    row.appendChild(btns);

    // Labelled for assistive tech; the visible globe carries the meaning.
    var label = document.createElement("label");
    label.className = "sr-only lang-switch-label";
    label.setAttribute("data-i18n", "lang.label");
    label.setAttribute("for", "lang-select-" + (++switcherSeq));
    label.textContent = "Language";
    wrap.appendChild(label);

    var select = document.createElement("select");
    select.className = "lang-select-more";
    select.id = "lang-select-" + switcherSeq;
    // Every locale is listed, so this one control is enough on small screens
    // where the DE/EN shortcut is hidden.
    codes().forEach(function (code) {
      var opt = document.createElement("option");
      opt.value = code;
      opt.textContent = nativeName(code);
      select.appendChild(opt);
    });
    select.addEventListener("change", function () {
      if (select.value) setLang(select.value);
    });
    row.appendChild(select);

    wrap.appendChild(row);
    return wrap;
  }

  function injectSwitcher() {
    document.querySelectorAll("[data-lang-switch]").forEach(function (target) {
      if (target.querySelector(".lang-switch")) return;
      target.appendChild(buildSwitcher());

      var note = document.createElement("span");
      note.className = "lang-switch-note";
      note.setAttribute("data-i18n", "lang.note");
      note.textContent = "App: German & English only · Website in many languages";
      if (target.classList.contains("lang-switch-wrap")) target.appendChild(note);
    });
  }

  /* The legal/support pages only exist in German and English. When the visitor
     picked another language the nav is translated but the body is not, so say
     so once instead of leaving them guessing. */
  function legalFallbackNote(code) {
    var host = document.querySelector("main .wrap, main, .wrap");
    if (!host || document.querySelector("[data-i18n='nav.features']")) return;
    if (!document.querySelector('[lang="de"], [lang="en"]')) return;
    var note = document.querySelector("[data-legal-note]");
    if (isPrimary(code)) { if (note) note.remove(); return; }
    if (!note) {
      note = document.createElement("p");
      note.setAttribute("data-legal-note", "");
      note.setAttribute("data-i18n", "legal.langNote");
      note.className = "legal-lang-note";
      var h1 = host.querySelector("h1");
      if (h1 && h1.nextSibling) h1.parentNode.insertBefore(note, h1.nextSibling);
      else host.insertBefore(note, host.firstChild);
    }
    var table = strings()[code] || strings().en;
    if (table && table["legal.langNote"]) note.textContent = table["legal.langNote"];
  }

  function init() {
    var lang = getLang();
    if (codes().indexOf(lang) < 0) lang = "en";
    applyDocumentLang(lang);
    injectSwitcher();
    applyStrings(lang);
    syncUi(lang);
    legalFallbackNote(lang);
  }

  window.RedlineSiteLang = { setLang: setLang, getLang: getLang, init: init };

  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", init);
  } else {
    init();
  }
})();
