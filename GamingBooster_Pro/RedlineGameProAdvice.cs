using System;
using System.Collections.Generic;
using System.IO;

namespace GamingBooster_Pro
{
    internal enum GameTipKind
    {
        Recommended,
        Optional,
        Avoid
    }

    internal enum GameTipImpact
    {
        High,
        Medium,
        Low
    }

    internal sealed class GameProTip
    {
        public string Id { get; init; } = "";
        public string TitleDe { get; init; } = "";
        public string TitleEn { get; init; } = "";
        public string DetailDe { get; init; } = "";
        public string DetailEn { get; init; } = "";
        public GameTipKind Kind { get; init; }
        public GameTipImpact Impact { get; init; }
        /// <summary>GameMode, PowerPlan, FlushDns, OpenGameBar, OpenGraphics, OpenRustFolder, OpenNvidiaPanel, ShaderCacheSafe</summary>
        public string? ApplyAction { get; init; }
    }

    internal static class RedlineGameProAdvice
    {
        public static List<GameProTip> BuildFor(string gameName, bool english)
        {
            string g = (gameName ?? "").Trim();
            if (g.StartsWith("Rust", StringComparison.OrdinalIgnoreCase))
                return BuildRustTips();
            if (g.StartsWith("ARC", StringComparison.OrdinalIgnoreCase))
                return BuildArcTips();
            return BuildGenericTips();
        }

        public static RustInstallProbe ProbeRustInstall()
        {
            string pf86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string steam = Path.Combine(pf86, @"Steam\steamapps\common\Rust");
            string appData = Path.Combine(local, @"Facepunch Studios LTD\Rust");
            return new RustInstallProbe
            {
                SteamFolderExists = Directory.Exists(steam),
                LocalFolderExists = Directory.Exists(appData),
                SteamPath = steam,
                LocalPath = appData
            };
        }

        private static List<GameProTip> BuildRustTips()
        {
            return new List<GameProTip>
            {
                Tip("rust-fps-power", "Hochleistungs-Energieplan", "High performance power plan",
                    "Windows auf Höchstleistung — spürbar mehr FPS, weniger Drosselung.", "Set Windows to high performance — less throttling, more FPS.",
                    GameTipKind.Recommended, GameTipImpact.High, "PowerPlan"),
                Tip("rust-fps-gamemode", "Windows Game Mode", "Windows Game Mode",
                    "Game Mode an — priorisiert das aktive Spiel.", "Enable Game Mode — prioritizes the active game.",
                    GameTipKind.Recommended, GameTipImpact.High, "GameMode"),
                Tip("rust-fps-gamebar", "Xbox Game Bar / Overlays", "Xbox Game Bar / overlays",
                    "Game Bar und unnötige Overlays aus — oft +5–15 FPS in Rust.", "Disable Game Bar and overlays — often +5–15 FPS in Rust.",
                    GameTipKind.Recommended, GameTipImpact.High, "OpenGameBar"),
                Tip("rust-fps-ingame", "Rust Grafik: Schatten & Objekte", "Rust graphics: shadows & objects",
                    "Schatten, Gras, Objektdetails runter — größter FPS-Gewinn INGAME ohne Anti-Cheat-Risiko.", "Lower shadows, grass, object detail — biggest in-game FPS gain, anti-cheat safe.",
                    GameTipKind.Recommended, GameTipImpact.High, "OpenRustFolder"),
                Tip("rust-fps-bg", "Discord / Browser im Hintergrund", "Discord / browser in background",
                    "Schließe Browser-Tabs, Discord-Hardwarebeschleunigung testweise aus.", "Close browser tabs; try disabling Discord hardware acceleration.",
                    GameTipKind.Recommended, GameTipImpact.Medium, null),
                Tip("rust-fps-gpu", "NVIDIA: Leistungsmodus", "NVIDIA: prefer maximum performance",
                    "Im NVIDIA Control Panel: Verwaltete 3D-Einstellungen → Leistungsmodus.", "In NVIDIA Control Panel: manage 3D settings → performance mode.",
                    GameTipKind.Recommended, GameTipImpact.High, "OpenNvidiaPanel"),
                Tip("rust-opt-dns", "DNS Cache leeren (optional)", "Flush DNS (optional)",
                    "Kann Ping-Stabilität verbessern — kein direkter FPS-Boost.", "Can help ping stability — not a direct FPS boost.",
                    GameTipKind.Optional, GameTipImpact.Low, "FlushDns"),
                Tip("rust-opt-shader", "Shader Cache nach Treiberupdate", "Shader cache after driver update",
                    "Nur nach GPU-Treiberupdate: D3D/NVIDIA Cache leeren (Spiel einmal neu kompilieren lassen).", "Only after GPU driver update: clear D3D/NVIDIA cache (let game rebuild shaders).",
                    GameTipKind.Optional, GameTipImpact.Medium, "ShaderCacheSafe"),
                Tip("rust-opt-launch", "Steam Startoptionen", "Steam launch options",
                    "-high kann Priorität erhöhen. Keine riskanten Memory-Hacks — Bans möglich.", "-high can raise priority. Avoid risky memory hacks — bans possible.",
                    GameTipKind.Optional, GameTipImpact.Medium, null),
                Tip("rust-avoid-eac", "EasyAntiCheat nicht anfassen", "Do not touch EasyAntiCheat",
                    "Keine EAC-Dateien löschen/umbenennen — Spiel startet nicht oder Ban-Risiko.", "Never delete/rename EAC files — game won't start or ban risk.",
                    GameTipKind.Avoid, GameTipImpact.High, null),
                Tip("rust-avoid-cfg", "Rust cfg / client.cfg", "Rust cfg / client.cfg",
                    "Config nicht automatisch löschen. Backup vor manuellen Änderungen.", "Don't auto-delete config. Backup before manual edits.",
                    GameTipKind.Avoid, GameTipImpact.High, null),
                Tip("rust-avoid-mem", "RAM-Cleaner während Rust", "RAM cleaners while Rust runs",
                    "Keine aggressiven RAM-Tools bei laufendem Spiel — Stutter & Crashes.", "No aggressive RAM tools while playing — stutter and crashes.",
                    GameTipKind.Avoid, GameTipImpact.High, null),
                Tip("rust-avoid-oc", "Blindes GPU-Overclocking", "Blind GPU overclocking",
                    "OC nur stabil testen — Instabilität wirkt wie Spiel-Crashes.", "Only use stable OC — instability looks like game crashes.",
                    GameTipKind.Avoid, GameTipImpact.Medium, null),
            };
        }

        private static List<GameProTip> BuildArcTips()
        {
            return new List<GameProTip>
            {
                Tip("arc-power", "Hochleistungsmodus", "High performance mode", "Energieplan auf Performance.", "Power plan to performance.",
                    GameTipKind.Recommended, GameTipImpact.High, "PowerPlan"),
                Tip("arc-gamemode", "Game Mode", "Game Mode", "Windows Game Mode aktivieren.", "Enable Windows Game Mode.",
                    GameTipKind.Recommended, GameTipImpact.Medium, "GameMode"),
                Tip("arc-shader", "Shader Cache (optional)", "Shader cache (optional)", "Nach Treiberupdate optional leeren.", "Optional clear after driver update.",
                    GameTipKind.Optional, GameTipImpact.Medium, "ShaderCacheSafe"),
                Tip("arc-avoid-ac", "Anti-Cheat Dateien", "Anti-cheat files", "Keine Anti-Cheat-Ordner bearbeiten.", "Do not modify anti-cheat folders.",
                    GameTipKind.Avoid, GameTipImpact.High, null),
            };
        }

        private static List<GameProTip> BuildGenericTips()
        {
            return new List<GameProTip>
            {
                Tip("gen-power", "Hochleistungsmodus", "High performance", "Windows Energieplan prüfen.", "Check Windows power plan.",
                    GameTipKind.Recommended, GameTipImpact.High, "PowerPlan"),
                Tip("gen-gamemode", "Game Mode", "Game Mode", "Für die meisten Spiele sinnvoll.", "Useful for most games.",
                    GameTipKind.Recommended, GameTipImpact.Medium, "GameMode"),
                Tip("gen-autostart", "Autostart prüfen", "Check autostart", "Hintergrund-Launcher reduzieren.", "Reduce background launchers.",
                    GameTipKind.Optional, GameTipImpact.Medium, null),
                Tip("gen-avoid-files", "Spiel-Dateien", "Game files", "Redline ändert keine Spiel-Configs automatisch.", "Redline does not auto-change game configs.",
                    GameTipKind.Avoid, GameTipImpact.High, null),
            };
        }

        private static GameProTip Tip(string id, string de, string en, string detailDe, string detailEn,
            GameTipKind kind, GameTipImpact impact, string? apply) =>
            new GameProTip
            {
                Id = id,
                TitleDe = de,
                TitleEn = en,
                DetailDe = detailDe,
                DetailEn = detailEn,
                Kind = kind,
                Impact = impact,
                ApplyAction = apply
            };
    }

    internal sealed class RustInstallProbe
    {
        public bool SteamFolderExists { get; init; }
        public bool LocalFolderExists { get; init; }
        public string SteamPath { get; init; } = "";
        public string LocalPath { get; init; } = "";
    }
}
