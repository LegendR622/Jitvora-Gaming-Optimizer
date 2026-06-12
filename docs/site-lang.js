(function () {
  "use strict";

  var STORAGE_KEY = "redline-site-lang";
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
    var stored = localStorage.getItem(STORAGE_KEY);
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
    localStorage.setItem(STORAGE_KEY, code);
    applyDocumentLang(code);
    applyStrings(code);
    syncUi(code);
  }

  function applyStrings(code) {
    var all = strings();
    var table = all[code] || all.en;
    if (!table) return;

    document.querySelectorAll("[data-i18n-aria]").forEach(function (el) {
      var key = el.getAttribute("data-i18n-aria");
      if (table[key]) el.setAttribute("aria-label", table[key]);
    });

    document.querySelectorAll("[data-i18n]").forEach(function (el) {
      var key = el.getAttribute("data-i18n");
      var htmlKey = key + ".html";
      if (table[htmlKey]) el.innerHTML = table[htmlKey];
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
    });

    document.querySelectorAll(".lang-select-more").forEach(function (sel) {
      sel.value = isPrimary(code) ? "" : code;
      sel.classList.toggle("is-active", !isPrimary(code));
    });
  }

  function buildSwitcher() {
    var wrap = document.createElement("div");
    wrap.className = "lang-switch";
    wrap.setAttribute("role", "navigation");
    wrap.setAttribute("aria-label", "Language / Sprache");

    var label = document.createElement("span");
    label.className = "lang-switch-label";
    label.setAttribute("data-i18n", "lang.label");
    label.textContent = "Language";
    wrap.appendChild(label);

    var row = document.createElement("div");
    row.className = "lang-switch-row";

    var btns = document.createElement("div");
    btns.className = "lang-switch-btns";
    PRIMARY.forEach(function (code) {
      var btn = document.createElement("button");
      btn.type = "button";
      btn.className = "lang-btn";
      btn.setAttribute("data-set-lang", code);
      btn.textContent = code.toUpperCase();
      btn.title = (meta()[code] && meta()[code].label) || code;
      btn.addEventListener("click", function () {
        setLang(code);
      });
      btns.appendChild(btn);
    });
    row.appendChild(btns);

    var select = document.createElement("select");
    select.className = "lang-select-more";
    select.setAttribute("aria-label", "More languages");

    var placeholder = document.createElement("option");
    placeholder.value = "";
    placeholder.setAttribute("data-i18n", "lang.more");
    placeholder.textContent = "More";
    select.appendChild(placeholder);

    var m = meta();
    codes().forEach(function (code) {
      if (isPrimary(code)) return;
      var opt = document.createElement("option");
      opt.value = code;
      opt.textContent = (m[code] && m[code].label) || code;
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

  function init() {
    var lang = getLang();
    if (codes().indexOf(lang) < 0) lang = "en";
    applyDocumentLang(lang);
    injectSwitcher();
    applyStrings(lang);
    syncUi(lang);
  }

  window.RedlineSiteLang = { setLang: setLang, getLang: getLang, init: init };

  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", init);
  } else {
    init();
  }
})();
