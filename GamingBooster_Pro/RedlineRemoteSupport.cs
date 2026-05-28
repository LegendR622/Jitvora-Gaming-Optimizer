using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace GamingBooster_Pro
{
    internal sealed class RemoteSupportStatus
    {
        public bool RemoteDesktopEnabled { get; set; }
        public bool QuickAssistAvailable { get; set; }
        public bool QuickAssistRunning { get; set; }
        public bool RemoteAssistanceAvailable { get; set; }
        public string QuickAssistPath { get; set; } = "";
        public string QuickAssistDetail { get; set; } = "";
    }

    internal static class RedlineRemoteSupport
    {
        public static RemoteSupportStatus Query()
        {
            bool running = IsQuickAssistRunning();
            bool installed = TryFindQuickAssist(out string? path);
            bool protocol = SupportsQuickAssistProtocol();

            RemoteSupportStatus s = new RemoteSupportStatus
            {
                RemoteDesktopEnabled = IsRemoteDesktopEnabled(),
                QuickAssistRunning = running,
                QuickAssistAvailable = installed || running || protocol,
                RemoteAssistanceAvailable = File.Exists(Path.Combine(Environment.SystemDirectory, "msra.exe")),
                QuickAssistPath = path ?? "",
                QuickAssistDetail = BuildQuickAssistDetail(installed, running, protocol, path)
            };
            return s;
        }

        private static string BuildQuickAssistDetail(bool installed, bool running, bool protocol, string? path)
        {
            if (running) return "process";
            if (!string.IsNullOrWhiteSpace(path)) return "exe:" + path;
            if (installed) return "installed";
            if (protocol) return "ms-quick-assist";
            return "";
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

        public static bool IsQuickAssistRunning()
        {
            try
            {
                return Process.GetProcessesByName("QuickAssist").Length > 0
                    || Process.GetProcessesByName("QuickAssistApp").Length > 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool SupportsQuickAssistProtocol()
        {
            try
            {
                if (Environment.OSVersion.Version.Build >= 22000)
                    return true;
            }
            catch { }
            return false;
        }

        public static bool TryFindQuickAssist(out string? path)
        {
            string sys = Path.Combine(Environment.SystemDirectory, "quickassist.exe");
            if (File.Exists(sys))
            {
                path = sys;
                return true;
            }

            try
            {
                string windowsApps = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "WindowsApps");
                if (Directory.Exists(windowsApps))
                {
                    foreach (string dir in Directory.EnumerateDirectories(windowsApps, "*QuickAssist*", SearchOption.TopDirectoryOnly))
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

            try
            {
                string localApps = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Microsoft", "WindowsApps");
                string stub = Path.Combine(localApps, "MicrosoftCorporationII.QuickAssist_8wekyb3d8bbwe", "QuickAssist.exe");
                if (File.Exists(stub))
                {
                    path = stub;
                    return true;
                }
            }
            catch { }

            if (IsQuickAssistAppxPresent(out string? appxName))
            {
                path = "appx:" + appxName;
                return true;
            }

            path = null;
            return false;
        }

        private static bool IsQuickAssistAppxPresent(out string packageName)
        {
            packageName = "";
            try
            {
                using Process p = new Process();
                p.StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "-NoProfile -Command \"(Get-AppxPackage -Name *QuickAssist* | Select-Object -First 1).Name\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                p.Start();
                string output = p.StandardOutput.ReadToEnd().Trim();
                p.WaitForExit(8000);
                if (output.Contains("QuickAssist", StringComparison.OrdinalIgnoreCase))
                {
                    packageName = output;
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static string FormatStatusLabel(RemoteSupportStatus s, bool english)
        {
            string rdp = s.RemoteDesktopEnabled
                ? (english ? "Remote Desktop: ON" : "Remote Desktop: AN")
                : (english ? "Remote Desktop: OFF" : "Remote Desktop: AUS");

            string qa;
            if (s.QuickAssistRunning)
                qa = english ? "Quick Assist: running" : "Quick Assist: läuft";
            else if (s.QuickAssistAvailable)
                qa = english ? "Quick Assist: available" : "Quick Assist: verfügbar";
            else
                qa = english ? "Quick Assist: not found" : "Quick Assist: nicht gefunden";

            return rdp + " · " + qa;
        }
    }
}
