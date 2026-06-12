(function () {
  var k = "redline-site-lang";
  var codes = window.REDLINE_LANG_CODES || ["de", "en"];
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
  document.documentElement.dir = meta.rtl ? "rtl" : "ltr";
})();
