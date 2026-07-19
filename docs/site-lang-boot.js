/* Early language boot — self-contained so the big site-i18n.js dictionary can load deferred.
   Keep this code list in sync with REDLINE_LANG_META in docs/i18n-full.json / site-i18n.js. */
(function () {
  var k = "redline-site-lang";
  var codes = window.REDLINE_LANG_CODES || [
    "de", "en", "fr", "es", "it", "pt", "nl", "pl", "tr", "ru", "uk", "cs",
    "ar", "zh", "ja", "ko", "hi", "sv", "ro", "hu", "el", "th", "vi", "id",
    "da", "no", "fi"
  ];
  var rtl = { ar: true };
  var l = localStorage.getItem(k);
  if (!l) {
    var n = (navigator.language || "en").toLowerCase();
    var i;
    l = "en";
    for (i = 0; i < codes.length; i++) {
      if (n === codes[i] || n.indexOf(codes[i] + "-") === 0) {
        l = codes[i];
        break;
      }
    }
    if (l === "en" && n.indexOf("de") === 0) l = "de";
  }
  if (codes.indexOf(l) < 0) l = "en";
  var meta = (window.REDLINE_LANG_META || {})[l] || {};
  document.documentElement.setAttribute("data-site-lang", l);
  document.documentElement.setAttribute("data-legal-lang", l === "de" ? "de" : "en");
  document.documentElement.lang = l;
  document.documentElement.dir = (meta.rtl || rtl[l]) ? "rtl" : "ltr";
})();
