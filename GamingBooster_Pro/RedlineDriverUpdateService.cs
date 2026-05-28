using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GamingBooster_Pro
{
    internal sealed class RedlineDriverUpdateService
    {
        public static RedlineDriverUpdateService Instance { get; } = new();

        private CancellationTokenSource? _cts;

        public bool IsRunning { get; private set; }
        public IReadOnlyList<string> PendingWindowsDriverTitles { get; private set; } = Array.Empty<string>();

        public void Cancel() => _cts?.Cancel();

        public async Task RunAsync(
            bool installAfterSearch,
            bool installOnly,
            Func<string, Task> log,
            bool isEnglish,
            CancellationToken externalCt = default)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(externalCt);
            CancellationToken ct = _cts.Token;
            IsRunning = true;
            try
            {
                if (!installOnly)
                {
                    await log(isEnglish
                        ? "Step 1: Searching Windows driver updates…"
                        : "Schritt 1: Suche Windows-Treiber-Updates…");

                    int count = await SearchWindowsDriverUpdatesAsync(log, isEnglish, ct);
                    if (count == 0)
                        await log(isEnglish
                            ? "No pending Windows driver updates found."
                            : "Keine ausstehenden Windows-Treiber-Updates gefunden.");
                    else if (installAfterSearch)
                        await log(isEnglish
                            ? count + " update(s) found — installing automatically…"
                            : count + " Update(s) gefunden — installiere automatisch…");
                    else
                        await log(isEnglish
                            ? count + " update(s) found. Use Install or click a driver on the left."
                            : count + " Update(s) gefunden. „Installieren“ oder Treiber links anklicken.");

                    await log("");
                    await log(isEnglish ? "Step 2: Optional GPU tools (winget)…" : "Schritt 2: Optionale GPU-Tools (winget)…");
                    await TryWingetGpuUpgradeAsync(log, isEnglish, ct);
                }

                if (installAfterSearch || installOnly)
                {
                    await log("");
                    await log(isEnglish
                        ? "Installing Windows driver updates… (admin recommended)"
                        : "Installiere Windows-Treiber-Updates… (Admin empfohlen)");
                    await InstallWindowsDriverUpdatesAsync(null, log, isEnglish, ct);
                }

                await log("");
                await log(isEnglish ? "Refreshing driver status…" : "Aktualisiere Treiber-Status…");
                await SearchWindowsDriverUpdatesAsync(log, isEnglish, ct);
                await log(isEnglish ? "Driver update finished." : "Treiber-Update abgeschlossen.");
            }
            catch (OperationCanceledException)
            {
                await log(isEnglish ? "Driver update cancelled." : "Treiber-Update abgebrochen.");
            }
            finally
            {
                IsRunning = false;
                _cts?.Dispose();
                _cts = null;
            }
        }

        public async Task<int> SearchOnlyAsync(Func<string, Task> log, bool isEnglish, CancellationToken ct = default)
        {
            return await SearchWindowsDriverUpdatesAsync(log, isEnglish, ct);
        }

        public async Task<bool> InstallSingleByDeviceHintAsync(
            string deviceHint,
            Func<string, Task> log,
            bool isEnglish,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(deviceHint))
                return false;

            string hint = deviceHint.Trim();
            if (PendingWindowsDriverTitles.Count == 0)
            {
                await log(isEnglish ? "Searching Windows Update…" : "Suche Windows Update…");
                await SearchWindowsDriverUpdatesAsync(log, isEnglish, ct);
            }

            string? match = RedlineDriverStatus.FindWindowsUpdateMatch(hint, PendingWindowsDriverTitles);
            if (match == null)
                match = PendingWindowsDriverTitles.FirstOrDefault(t => RedlineDriverStatus.NamesLikelyMatch(hint, t));

            if (match != null)
            {
                await log(isEnglish ? "Matched package: " + match : "Passendes Paket: " + match);
                await InstallWindowsDriverUpdatesAsync(match, log, isEnglish, ct);
                PendingWindowsDriverTitles = PendingWindowsDriverTitles.Where(t => t != match).ToList();
                RedlineDriverStatus.MarkRecentlyUpdated(hint);
                return true;
            }

            await log(isEnglish
                ? "No Windows package — opening vendor page…"
                : "Kein Windows-Paket — öffne Hersteller-Seite…");
            await OpenVendorUpdateForDeviceAsync(hint, log, isEnglish, ct);
            return false;
        }

        private static async Task OpenVendorUpdateForDeviceAsync(string deviceHint, Func<string, Task> log, bool en, CancellationToken ct)
        {
            string d = deviceHint.ToLowerInvariant();
            string url = "ms-settings:windowsupdate";
            if (d.Contains("nvidia") || d.Contains("geforce"))
                url = "https://www.nvidia.com/Download/index.aspx";
            else if (d.Contains("amd") || d.Contains("radeon"))
                url = "https://www.amd.com/en/support/download/drivers.html";
            else if (d.Contains("intel"))
                url = "https://www.intel.com/content/www/us/en/support/detect.html";
            else if (d.Contains("realtek"))
                url = "https://www.realtek.com/Download/List?cate_id=584";

            try
            {
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                await log(en ? "Opened: " + url : "Geöffnet: " + url);
            }
            catch (Exception ex)
            {
                await log(en ? "Could not open: " + ex.Message : "Konnte nicht öffnen: " + ex.Message);
            }
        }

        private async Task<int> SearchWindowsDriverUpdatesAsync(Func<string, Task> log, bool en, CancellationToken ct)
        {
            const string script = @"
$ErrorActionPreference = 'SilentlyContinue'
try {
  $s = New-Object -ComObject Microsoft.Update.Session
  $searcher = $s.CreateUpdateSearcher()
  $r = $searcher.Search('IsInstalled=0 and Type=''Driver''')
  Write-Output ('COUNT:' + $r.Updates.Count)
  for ($i = 0; $i -lt $r.Updates.Count; $i++) {
    $u = $r.Updates.Item($i)
    Write-Output ('TITLE:' + $u.Title)
  }
} catch {
  Write-Output ('ERROR:' + $_.Exception.Message)
  exit 1
}
";
            (int code, string output) = await RunPowerShellAsync(script, ct);
            List<string> titles = new List<string>();
            int count = 0;
            foreach (string line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                ct.ThrowIfCancellationRequested();
                if (line.StartsWith("COUNT:", StringComparison.OrdinalIgnoreCase))
                {
                    int.TryParse(line.Substring(6).Trim(), out count);
                    continue;
                }
                if (line.StartsWith("TITLE:", StringComparison.OrdinalIgnoreCase))
                {
                    string title = line.Substring(6).Trim();
                    titles.Add(title);
                    await log("• " + title);
                }
                if (line.StartsWith("ERROR:", StringComparison.OrdinalIgnoreCase))
                    await log(en ? "Windows Update error: " + line.Substring(6) : "Windows-Update-Fehler: " + line.Substring(6));
            }

            PendingWindowsDriverTitles = titles;

            if (code != 0 && count == 0)
                await log(en
                    ? "Windows Update search failed. Run as administrator."
                    : "Windows-Update-Suche fehlgeschlagen. Als Administrator starten.");
            return count;
        }

        private static async Task InstallWindowsDriverUpdatesAsync(
            string? titleContains,
            Func<string, Task> log,
            bool en,
            CancellationToken ct)
        {
            string filter = string.IsNullOrWhiteSpace(titleContains)
                ? ""
                : "$match = '" + EscapePs(titleContains) + "'\r\n";
            string selectLoop = string.IsNullOrWhiteSpace(titleContains)
                ? "  for ($i = 0; $i -lt $r.Updates.Count; $i++) { [void]$coll.Add($r.Updates.Item($i)) }\r\n"
                : @"  for ($i = 0; $i -lt $r.Updates.Count; $i++) {
    $u = $r.Updates.Item($i)
    if ($u.Title -like ""*$match*"") { [void]$coll.Add($u) }
  }
";

            string script = @"
$ErrorActionPreference = 'Stop'
try {
" + filter + @"
  $s = New-Object -ComObject Microsoft.Update.Session
  $searcher = $s.CreateUpdateSearcher()
  $r = $searcher.Search('IsInstalled=0 and Type=''Driver''')
  if ($r.Updates.Count -eq 0) { Write-Output 'NONE'; exit 0 }
  $coll = New-Object -ComObject Microsoft.Update.UpdateColl
" + selectLoop + @"
  if ($coll.Count -eq 0) { Write-Output 'NOMATCH'; exit 0 }
  $dl = $s.CreateUpdateDownloader()
  $dl.Updates = $coll
  Write-Output 'DOWNLOADING'
  $dlResult = $dl.Download()
  Write-Output ('DLRESULT:' + $dlResult.ResultCode)
  $inst = $s.CreateUpdateInstaller()
  $inst.Updates = $coll
  Write-Output 'INSTALLING'
  $instResult = $inst.Install()
  Write-Output ('INSTRESULT:' + $instResult.ResultCode)
  Write-Output ('REBOOT:' + $instResult.RebootRequired)
} catch {
  Write-Output ('ERROR:' + $_.Exception.Message)
  exit 1
}
";
            (int code, string output) = await RunPowerShellAsync(script, ct);
            foreach (string line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                ct.ThrowIfCancellationRequested();
                if (line == "NONE") { await log(en ? "Nothing to install." : "Nichts zu installieren."); return; }
                if (line == "NOMATCH") { await log(en ? "No matching package." : "Kein passendes Paket."); return; }
                if (line == "DOWNLOADING") await log(en ? "Downloading…" : "Lade herunter…");
                else if (line == "INSTALLING") await log(en ? "Installing…" : "Installiere…");
                else if (line.StartsWith("DLRESULT:", StringComparison.OrdinalIgnoreCase))
                    await log(en ? "Download: " + line.Substring(9) : "Download: " + line.Substring(9));
                else if (line.StartsWith("INSTRESULT:", StringComparison.OrdinalIgnoreCase))
                    await log(en ? "Install: " + line.Substring(11) : "Installation: " + line.Substring(11));
                else if (line.StartsWith("REBOOT:", StringComparison.OrdinalIgnoreCase))
                    await log(en ? "Reboot required: " + line.Substring(7) : "Neustart nötig: " + line.Substring(7));
                else if (line.StartsWith("ERROR:", StringComparison.OrdinalIgnoreCase))
                    await log(en ? "Error: " + line.Substring(6) : "Fehler: " + line.Substring(6));
            }

            if (code != 0)
                await log(en ? "Install failed — run as administrator." : "Installation fehlgeschlagen — als Administrator starten.");
        }

        private static string EscapePs(string s) => s.Replace("'", "''");

        private static async Task TryWingetGpuUpgradeAsync(Func<string, Task> log, bool en, CancellationToken ct)
        {
            string which = await RunCmdCaptureAsync("where.exe", "winget", ct);
            if (string.IsNullOrWhiteSpace(which) || which.Contains("INFO: Could not find", StringComparison.OrdinalIgnoreCase))
            {
                await log(en ? "winget not found — skipped." : "winget nicht gefunden — übersprungen.");
                return;
            }

            await log(en ? "Checking winget for GPU…" : "Prüfe winget auf GPU…");
            (int code, string output) = await RunCmdCaptureAsyncFull("winget", "upgrade --disable-interactivity --accept-source-agreements --accept-package-agreements", ct, 120000);
            bool any = false;
            foreach (string line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.Contains("NVIDIA", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("AMD", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("Intel", StringComparison.OrdinalIgnoreCase)
                    || line.Contains("Graphics", StringComparison.OrdinalIgnoreCase))
                {
                    any = true;
                    await log("winget: " + line.Trim());
                }
            }

            if (!any)
                await log(en ? "No GPU winget upgrades listed." : "Keine GPU winget-Updates.");
        }

        private static async Task<(int exitCode, string output)> RunPowerShellAsync(string script, CancellationToken ct)
        {
            string path = Path.Combine(Path.GetTempPath(), "redline-driver-ps-" + Guid.NewGuid().ToString("N") + ".ps1");
            try
            {
                await File.WriteAllTextAsync(path, script, Encoding.UTF8, ct);
                return await RunCmdCaptureAsyncFull("powershell.exe",
                    "-NoProfile -ExecutionPolicy Bypass -File \"" + path + "\"", ct, 300000);
            }
            finally
            {
                try { if (File.Exists(path)) File.Delete(path); } catch { }
            }
        }

        private static async Task<string> RunCmdCaptureAsync(string file, string args, CancellationToken ct)
        {
            (int _, string o) = await RunCmdCaptureAsyncFull(file, args, ct, 60000);
            return o;
        }

        private static async Task<(int exitCode, string output)> RunCmdCaptureAsyncFull(
            string file, string args, CancellationToken ct, int timeoutMs)
        {
            var sb = new StringBuilder();
            using Process p = new Process();
            p.StartInfo = new ProcessStartInfo
            {
                FileName = file,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };
            p.OutputDataReceived += (_, e) => { if (e.Data != null) sb.AppendLine(e.Data); };
            p.ErrorDataReceived += (_, e) => { if (e.Data != null) sb.AppendLine(e.Data); };
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            using var reg = ct.Register(() => { try { if (!p.HasExited) p.Kill(true); } catch { } });
            bool done = await Task.Run(() => p.WaitForExit(timeoutMs), ct);
            if (!done)
            {
                try { p.Kill(true); } catch { }
                throw new OperationCanceledException();
            }
            return (p.ExitCode, sb.ToString());
        }
    }
}
