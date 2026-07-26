/* Jitvora landing page — interactions.
   Vanilla, no dependencies. Every effect degrades to a usable static page. */
(function () {
  "use strict";

  var reduced = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
  var finePointer = window.matchMedia("(pointer: fine)").matches;

  function $(sel, root) { return (root || document).querySelector(sel); }
  function $$(sel, root) { return Array.prototype.slice.call((root || document).querySelectorAll(sel)); }

  /* ————— nav: stuck state, scroll progress, active section ————— */
  (function nav() {
    var bar = $("[data-nav]");
    var progress = $("[data-nav-progress]");
    if (!bar) return;

    var links = $$(".nav-links a[href^='#']");
    var sections = links
      .map(function (a) { return document.getElementById(a.getAttribute("href").slice(1)); })
      .filter(Boolean);

    var ticking = false;
    function update() {
      ticking = false;
      var y = window.scrollY || document.documentElement.scrollTop;
      bar.classList.toggle("is-stuck", y > 12);

      if (progress) {
        var max = document.documentElement.scrollHeight - window.innerHeight;
        progress.style.transform = "scaleX(" + (max > 0 ? Math.min(y / max, 1) : 0) + ")";
      }

      // active link = last section whose top has passed the nav line
      var line = y + (window.innerHeight * 0.32);
      var activeId = null;
      for (var i = 0; i < sections.length; i++) {
        if (sections[i].offsetTop <= line) activeId = sections[i].id;
      }
      links.forEach(function (a) {
        a.classList.toggle("is-current", a.getAttribute("href") === "#" + activeId);
      });
    }
    function onScroll() {
      if (!ticking) { ticking = true; window.requestAnimationFrame(update); }
    }
    window.addEventListener("scroll", onScroll, { passive: true });
    window.addEventListener("resize", onScroll, { passive: true });
    update();
  })();

  /* ————— mobile drawer ————— */
  (function drawer() {
    var toggle = $("[data-drawer-toggle]");
    var panel = $("[data-drawer]");
    if (!toggle || !panel) return;

    function setOpen(open) {
      panel.classList.toggle("is-open", open);
      toggle.setAttribute("aria-expanded", open ? "true" : "false");
    }
    toggle.addEventListener("click", function () {
      setOpen(toggle.getAttribute("aria-expanded") !== "true");
    });
    panel.addEventListener("click", function (e) {
      if (e.target.closest("a")) setOpen(false);
    });
    document.addEventListener("keydown", function (e) {
      if (e.key === "Escape" && toggle.getAttribute("aria-expanded") === "true") {
        setOpen(false);
        toggle.focus();
      }
    });
    document.addEventListener("click", function (e) {
      if (toggle.getAttribute("aria-expanded") !== "true") return;
      if (panel.contains(e.target) || toggle.contains(e.target)) return;
      setOpen(false);
    });
    // matches the nav's collapse breakpoint — the drawer must close once the
    // full bar comes back, otherwise it hangs open behind the desktop nav
    var wide = window.matchMedia("(min-width: 1201px)");
    (wide.addEventListener ? wide.addEventListener.bind(wide, "change") : wide.addListener.bind(wide))(
      function (e) { if (e.matches) setOpen(false); });
  })();

  /* ————— scroll reveals ————— */
  (function reveals() {
    var targets = $$(".reveal, .stagger, .draw");
    if (reduced || !("IntersectionObserver" in window)) {
      targets.forEach(function (el) { el.classList.add("is-visible"); });
      return;
    }
    var io = new IntersectionObserver(function (entries) {
      entries.forEach(function (entry) {
        if (!entry.isIntersecting) return;
        entry.target.classList.add("is-visible");
        io.unobserve(entry.target);
      });
    }, { rootMargin: "0px 0px -8% 0px", threshold: 0.06 });
    targets.forEach(function (el) { io.observe(el); });
  })();

  /* ————— tablists: feature explorer + screenshot gallery ————— */
  function wireTabs(listSel) {
    var list = $(listSel);
    if (!list) return;
    var tabEls = $$("[role='tab']", list);
    if (!tabEls.length) return;

    function panelOf(tab) { return document.getElementById(tab.getAttribute("aria-controls")); }

    function select(tab, focus) {
      tabEls.forEach(function (t) {
        var on = t === tab;
        t.setAttribute("aria-selected", on ? "true" : "false");
        t.tabIndex = on ? 0 : -1;
        var p = panelOf(t);
        if (!p) return;
        p.hidden = !on;
        p.classList.remove("is-entering");
        if (on && !reduced) {
          void p.offsetWidth;          // restart the enter animation
          p.classList.add("is-entering");
        }
      });
      if (focus) tab.focus();
    }

    list.addEventListener("click", function (e) {
      var tab = e.target.closest("[role='tab']");
      if (tab) select(tab, false);
    });

    list.addEventListener("keydown", function (e) {
      var i = tabEls.indexOf(document.activeElement);
      if (i < 0) return;
      // the lists run vertically on desktop and horizontally on mobile — accept both axes
      var next = null;
      if (e.key === "ArrowDown" || e.key === "ArrowRight") next = tabEls[(i + 1) % tabEls.length];
      else if (e.key === "ArrowUp" || e.key === "ArrowLeft") next = tabEls[(i - 1 + tabEls.length) % tabEls.length];
      else if (e.key === "Home") next = tabEls[0];
      else if (e.key === "End") next = tabEls[tabEls.length - 1];
      if (!next) return;
      e.preventDefault();
      select(next, true);
    });

    // horizontal lists can be swiped on touch devices
    var startX = null;
    list.addEventListener("touchstart", function (e) {
      startX = e.touches.length === 1 ? e.touches[0].clientX : null;
    }, { passive: true });
    list.addEventListener("touchend", function (e) {
      if (startX === null) return;
      var dx = e.changedTouches[0].clientX - startX;
      startX = null;
      if (Math.abs(dx) < 60) return;
      var cur = tabEls.findIndex(function (t) { return t.getAttribute("aria-selected") === "true"; });
      if (cur < 0) return;
      select(tabEls[(cur + (dx < 0 ? 1 : tabEls.length - 1)) % tabEls.length], false);
    }, { passive: true });

    select(tabEls.filter(function (t) { return t.getAttribute("aria-selected") === "true"; })[0]
           || tabEls[0], false);
  }
  wireTabs(".showcase-tabs");
  wireTabs(".gal-tabs");

  /* Gallery captures stay lazy for visitors who never scroll that far, but the
     moment the section comes into view they are warmed up — otherwise switching
     tabs shows an empty frame while the image is still being fetched. */
  (function warmGallery() {
    var gallery = $(".gallery");
    if (!gallery) return;
    function warm() {
      $$("img[loading='lazy']", gallery).forEach(function (img) {
        img.setAttribute("loading", "eager");
        img.setAttribute("fetchpriority", "low");
      });
    }
    if (!("IntersectionObserver" in window)) { warm(); return; }
    var io = new IntersectionObserver(function (entries) {
      if (!entries.some(function (e) { return e.isIntersecting; })) return;
      io.disconnect();
      warm();
    }, { rootMargin: "300px 0px" });
    io.observe(gallery);
  })();

  /* ————— hero cursor light ————— */
  (function spotlight() {
    if (reduced || !finePointer) return;
    var hero = $("[data-spotlight]");
    if (!hero) return;
    var pending = false, px = 0, py = 0;
    hero.addEventListener("pointermove", function (e) {
      var r = hero.getBoundingClientRect();
      px = ((e.clientX - r.left) / r.width) * 100;
      py = ((e.clientY - r.top) / r.height) * 100;
      if (pending) return;
      pending = true;
      window.requestAnimationFrame(function () {
        pending = false;
        hero.style.setProperty("--mx", px.toFixed(1) + "%");
        hero.style.setProperty("--my", py.toFixed(1) + "%");
      });
    }, { passive: true });
  })();

  /* ————— release facts: one source of truth for the whole page —————
     trust-latest.json carries version, file name, download URL and SHA256.
     The HTML ships the current values as a no-JS fallback; this keeps every
     body reference in sync when the JSON is bumped. Head metadata and the
     JSON-LD block stay static so crawlers see them without scripting. */
  var RELEASE = {
    version: "3.0.9",
    file: "Jitvora_Gaming_Optimizer_Setup_v3.0.9.exe",
    url: null,
    sha256: null
  };

  function paintRelease() {
    $$("[data-app-version]").forEach(function (el) { el.textContent = "v" + RELEASE.version; });
    if (RELEASE.url) {
      $$("[data-dl-link]").forEach(function (el) { el.href = RELEASE.url; });
    }
    if (RELEASE.sha256) {
      $$("[data-hash-full]").forEach(function (el) { el.textContent = RELEASE.sha256; });
      $$("[data-hash-short]").forEach(function (el) {
        el.textContent = RELEASE.sha256.slice(0, 8) + "…" + RELEASE.sha256.slice(-6);
      });
    }
  }

  /* Translations carry {version} / {file} rather than a baked release number,
     so a bump touches trust-latest.json only — never 27 locale files. */
  function fillTokens(root) {
    var walker = document.createTreeWalker(root || document.body, NodeFilter.SHOW_TEXT);
    var node, hits = [];
    while ((node = walker.nextNode())) {
      if (node.nodeValue.indexOf("{version}") >= 0 || node.nodeValue.indexOf("{file}") >= 0) {
        hits.push(node);
      }
    }
    hits.forEach(function (n) {
      n.nodeValue = n.nodeValue
        .replace(/\{version\}/g, "v" + RELEASE.version)
        .replace(/\{file\}/g, RELEASE.file);
    });
    var desc = document.querySelector('meta[name="description"][data-i18n-desc]');
    if (desc) {
      desc.setAttribute("content", desc.getAttribute("content")
        .replace(/\{version\}/g, "v" + RELEASE.version)
        .replace(/\{file\}/g, RELEASE.file));
    }
  }

  /* ————— i18n: placeholders + token fill, re-run on every language switch ————— */
  (function i18nExtras() {
    function apply() {
      var table = (window.REDLINE_I18N || {})[document.documentElement.getAttribute("data-site-lang")]
               || window.REDLINE_I18N_BASE;
      if (table) {
        $$("[data-i18n-ph]").forEach(function (el) {
          var v = table[el.getAttribute("data-i18n-ph")];
          if (v) el.setAttribute("placeholder", v.replace(/\{version\}/g, "v" + RELEASE.version));
        });
      }
      fillTokens();
      paintRelease();
    }
    apply();
    // site-lang.js rewrites the strings and flips this attribute — mirror it
    new MutationObserver(apply).observe(document.documentElement, {
      attributes: true, attributeFilter: ["data-site-lang"]
    });

    if (!("fetch" in window)) return;
    fetch("trust-latest.json", { cache: "no-cache" })
      .then(function (r) { return r.ok ? r.json() : null; })
      .then(function (d) {
        if (!d) return;
        if (d.version) RELEASE.version = d.version;
        if (d.fileName) RELEASE.file = d.fileName;
        if (d.downloadUrl && /^https:\/\/github\.com\/LegendR622\//.test(d.downloadUrl)) {
          RELEASE.url = d.downloadUrl;
        }
        if (typeof d.sha256 === "string" && d.sha256.length === 64) RELEASE.sha256 = d.sha256;
        paintRelease();
      })
      .catch(function () { /* the static HTML values remain correct */ });
  })();

  /* ————— command palette ————— */
  (function palette() {
    var backdrop = $("[data-cmdk-backdrop]");
    var input = $("[data-cmdk-input]");
    var listEl = $("[data-cmdk-list]");
    var emptyEl = $("[data-cmdk-empty]");
    if (!backdrop || !input || !listEl) return;

    var ICONS = {
      download: '<path d="M12 3v12M7 11l5 5 5-5M4 21h16"/>',
      section: '<path d="M4 6h16M4 12h16M4 18h10"/>',
      shield: '<path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/>',
      doc: '<path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/>',
      link: '<path d="M10 13a5 5 0 0 0 7 0l3-3a5 5 0 0 0-7-7l-1 1"/><path d="M14 11a5 5 0 0 0-7 0l-3 3a5 5 0 0 0 7 7l1-1"/>',
      mail: '<rect x="2" y="4" width="20" height="16" rx="2"/><path d="M2 7l10 6 10-6"/>'
    };

    // label: i18n key (falls back to `text`) · href · icon · hint
    var COMMANDS = [
      { key: "hero.download", text: "Download for Windows", href: "#download", icon: "download", hint: "version" },
      { key: "nav.features", text: "Features", href: "#features", icon: "section" },
      { key: "netwatch.label", text: "Network Watch", href: "#netwatch", icon: "section" },
      { key: "preview.label", text: "Preview", href: "#preview", icon: "section" },
      { key: "how.label", text: "How it works", href: "#how", icon: "section" },
      { key: "nav.trust", text: "Trust", href: "#trust", icon: "shield" },
      { key: "faq.label", text: "FAQ", href: "#faq", icon: "section" },
      { key: "hero.trust", text: "Trust & SHA256", href: "trust.html", icon: "shield" },
      { key: "nav.changelog", text: "Changelog", href: "changelog.html", icon: "doc" },
      { key: "nav.releases", text: "Releases", href: "https://github.com/LegendR622/Jitvora-Gaming-Optimizer/releases", icon: "link", external: true },
      { key: "nav.github", text: "GitHub", href: "https://github.com/LegendR622/Jitvora-Gaming-Optimizer", icon: "link", external: true },
      { key: "nav.support", text: "Support", href: "support.html", icon: "mail" },
      { key: "nav.privacy", text: "Privacy", href: "datenschutz.html", icon: "doc" },
      { key: "nav.imprint", text: "Imprint", href: "impressum.html", icon: "doc" },
      { key: "nav.licenses", text: "Licenses", href: "licenses.html", icon: "doc" }
    ];

    var active = 0;
    var visible = [];
    var lastFocus = null;

    function label(cmd) {
      var table = (window.REDLINE_I18N || {})[document.documentElement.getAttribute("data-site-lang")]
               || window.REDLINE_I18N_BASE || {};
      // palette rows are built after fillTokens has run, so resolve them here too
      return (table[cmd.key] || cmd.text)
        .replace(/\{version\}/g, "v" + RELEASE.version)
        .replace(/\{file\}/g, RELEASE.file);
    }

    function render(query) {
      var q = (query || "").trim().toLowerCase();
      visible = COMMANDS.filter(function (c) {
        return !q || label(c).toLowerCase().indexOf(q) >= 0 || c.text.toLowerCase().indexOf(q) >= 0;
      });
      active = 0;
      listEl.innerHTML = "";
      visible.forEach(function (cmd, i) {
        var li = document.createElement("li");
        var btn = document.createElement("button");
        btn.type = "button";
        btn.className = "cmdk-item" + (i === 0 ? " is-active" : "");
        btn.setAttribute("role", "option");
        btn.setAttribute("aria-selected", i === 0 ? "true" : "false");
        btn.innerHTML =
          '<span class="cmdk-item-ico" aria-hidden="true">' +
          '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">' +
          (ICONS[cmd.icon] || ICONS.section) + "</svg></span>" +
          "<span></span>" +
          (cmd.hint ? '<span class="cmdk-item-hint"></span>' : "");
        btn.children[1].textContent = label(cmd);
        if (cmd.hint) {
          btn.children[2].textContent = cmd.hint === "version" ? "v" + RELEASE.version : cmd.hint;
        }
        btn.addEventListener("click", function () { run(cmd); });
        li.appendChild(btn);
        listEl.appendChild(li);
      });
      if (emptyEl) emptyEl.hidden = visible.length > 0;
    }

    function paintActive() {
      $$(".cmdk-item", listEl).forEach(function (b, i) {
        var on = i === active;
        b.classList.toggle("is-active", on);
        b.setAttribute("aria-selected", on ? "true" : "false");
        if (on && b.scrollIntoView) b.scrollIntoView({ block: "nearest" });
      });
    }

    function run(cmd) {
      close();
      if (cmd.external) {
        window.open(cmd.href, "_blank", "noopener");
      } else if (cmd.href.charAt(0) === "#") {
        var target = document.querySelector(cmd.href);
        if (target) {
          target.scrollIntoView({ behavior: reduced ? "auto" : "smooth", block: "start" });
          history.replaceState(null, "", cmd.href);
        }
      } else {
        window.location.href = cmd.href;
      }
    }

    function open() {
      lastFocus = document.activeElement;
      backdrop.hidden = false;
      document.body.classList.add("cmdk-open");
      input.value = "";
      render("");
      input.focus();
    }

    function close() {
      if (backdrop.hidden) return;
      backdrop.hidden = true;
      document.body.classList.remove("cmdk-open");
      if (lastFocus && lastFocus.focus) lastFocus.focus();
    }

    $$("[data-cmdk-open]").forEach(function (b) { b.addEventListener("click", open); });

    input.addEventListener("input", function () { render(input.value); });

    backdrop.addEventListener("mousedown", function (e) {
      if (e.target === backdrop) close();
    });

    backdrop.addEventListener("keydown", function (e) {
      if (e.key === "Escape") { e.preventDefault(); close(); return; }
      if (e.key === "ArrowDown") {
        e.preventDefault();
        if (visible.length) { active = (active + 1) % visible.length; paintActive(); }
      } else if (e.key === "ArrowUp") {
        e.preventDefault();
        if (visible.length) { active = (active - 1 + visible.length) % visible.length; paintActive(); }
      } else if (e.key === "Enter") {
        e.preventDefault();
        if (visible[active]) run(visible[active]);
      } else if (e.key === "Tab") {
        // trap focus inside the dialog
        e.preventDefault();
        input.focus();
      }
    });

    document.addEventListener("keydown", function (e) {
      var typing = /^(INPUT|TEXTAREA|SELECT)$/.test(e.target.tagName) || e.target.isContentEditable;
      if ((e.ctrlKey || e.metaKey) && (e.key === "k" || e.key === "K")) {
        e.preventDefault();
        backdrop.hidden ? open() : close();
        return;
      }
      if (e.key === "/" && !typing && backdrop.hidden && !e.ctrlKey && !e.metaKey && !e.altKey) {
        e.preventDefault();
        open();
      }
    });
  })();
})();
