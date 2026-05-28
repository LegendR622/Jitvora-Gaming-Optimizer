using System;
using System.IO;
using Microsoft.Win32;

namespace GamingBooster_Pro
{
    internal sealed class RemoteSupportStatus
    {
        public bool RemoteDesktopEnabled { get; set; }
        public bool QuickAssistAvailable { get; set; }
        public bool RemoteAssistanceAvailable { get; set; }
        public string QuickAssistPath { get; set; } = "";
    }

    internal static class RedlineRemoteSupport
    {
        public static RemoteSupportStatus Query()
        {
            RemoteSupportStatus s = new RemoteSupportStatus
            {
                RemoteDesktopEnabled = IsRemoteDesktopEnabled(),
                QuickAssistAvailable = TryFindQuickAssist(out string? qaPath),
                RemoteAssistanceAvailable = File.Exists(Path.Combine(Environment.SystemDirectory, "msra.exe")),
                QuickAssistPath = qaPath ?? ""
            };
            return s;
        }

        public static bool IsRemoteDesktopEnabled()
        {
            try
            {
                using RegistryKey? key = Registry.LocalMachine.OpenSubKey(
                    @"System\CurrentControlSet\Control\Terminal Server");
                object? val = key?.GetValue("fDenyTSConnections");
                if (val is int i)
                    return i == 0;
                if (val is uint u)
                    return u == 0;
            }
            catch { }
            return false;
        }

        public static bool TryFindQuickAssist(out string? path)
        {
            string[] candidates =
            {
                Path.Combine(Environment.SystemDirectory, "quickassist.exe"),
                @"C:\Program Files\WindowsApps\MicrosoftCorporationII.QuickAssist_*\QuickAssist.exe"
            };

            if (File.Exists(candidates[0]))
            {
                path = candidates[0];
                return true;
            }

            try
            {
                string baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "WindowsApps");
                if (Directory.Exists(baseDir))
                {
                    foreach (string dir in Directory.GetDirectories(baseDir, "MicrosoftCorporationII.QuickAssist_*"))
                    {
                        string exe = Path.Combine(dir, "QuickAssist.exe");
                        if (File.Exists(exe))
                        {
                            path = exe;
                            return true;
                        }
                    }
                }
            }
            catch { }

            path = null;
            return false;
        }

        public static string FormatStatusLabel(RemoteSupportStatus s, bool english)
        {
            string rdp = s.RemoteDesktopEnabled
                ? (english ? "Remote Desktop: ON" : "Remote Desktop: AN")
                : (english ? "Remote Desktop: OFF" : "Remote Desktop: AUS");
            string qa = s.QuickAssistAvailable
                ? (english ? "Quick Assist: available" : "Quick Assist: verfügbar")
                : (english ? "Quick Assist: not found" : "Quick Assist: nicht gefunden");
            return rdp + " · " + qa;
        }
    }
}
