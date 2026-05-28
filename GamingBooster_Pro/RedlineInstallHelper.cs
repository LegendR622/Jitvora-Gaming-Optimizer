using System;
using System.IO;
using Microsoft.Win32;

namespace GamingBooster_Pro
{
    internal static class RedlineInstallHelper
    {
        private const string UninstallKeyName = "A7B3C9E1-4F2D-4A8B-9C0E-REDLINE-GAMING-01_is1";
        public const string AppMutexName = "RedlineGamingOptimizerMutex";

        public static bool TryGetInstalledLocation(out string installDir)
        {
            installDir = "";
            try
            {
                foreach (RegistryHive hive in new[] { RegistryHive.CurrentUser, RegistryHive.LocalMachine })
                {
                    using RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64);
                    using RegistryKey? key = baseKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\" + UninstallKeyName);
                    string? loc = key?.GetValue("InstallLocation") as string;
                    if (!string.IsNullOrWhiteSpace(loc) && Directory.Exists(loc))
                    {
                        installDir = loc.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        return true;
                    }
                }
            }
            catch { }

            return false;
        }

        public static bool IsSetupInstalled() => TryGetInstalledLocation(out _);

        public static string BuildSilentInstallerArgs()
        {
            string args = "/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /CLOSEAPPLICATIONS";
            if (TryGetInstalledLocation(out string dir))
                return args + " /DIR=\"" + dir + "\"";
            if (IsUnderProgramFiles(AppContext.BaseDirectory))
                return args + " /DIR=\"" + AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + "\"";
            return args;
        }

        private static bool IsUnderProgramFiles(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;
            string p = path.ToLowerInvariant();
            return p.Contains(@"\program files\redline gaming optimizer")
                || p.Contains(@"\program files (x86)\redline gaming optimizer");
        }
    }
}
