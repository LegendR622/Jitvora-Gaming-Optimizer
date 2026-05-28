using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GamingBooster_Pro
{
    internal sealed class RedlineDriverUpdateService
    {
        public static RedlineDriverUpdateService Instance { get; } = new();

        private CancellationTokenSource? _cts;

        public bool IsRunning { get; private set; }

        public void Cancel() => _cts?.Cancel();

        public async Task RunAsync(
            HardwareProfile hardware,
            bool installPackages,
            Func<string, Task> log,
            bool isEnglish,
            CancellationToken externalCt = default)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(externalCt);
            CancellationToken ct = _cts.Token;
            IsRunning = true;
            try
            {
                bool en = isEnglish;
                await log(en ? "Scanning hardware…" : "Hardware wird erkannt…");

                if (!installPackages)
                {
                    await log(en ? "Packages for your PC:" : "Pakete für deinen PC:");
                    foreach (WingetDriverPackage pkg in RedlineHardwareProfile.BuildWingetPackagesForHardware(hardware))
                        await log("• " + (en ? pkg.LabelEn : pkg.LabelDe));
                    await log(en ? "Click Install to start." : "„Installieren“ startet die Installation.");
                    return;
                }

                await log(en ? "Installing matching drivers…" : "Installiere passende Treiber…");
                await TryWingetHardwareUpgradeAsync(hardware, log, en, ct);
                await log(en ? "Done." : "Fertig.");
            }
            catch (OperationCanceledException)
            {
                await log(isEnglish ? "Cancelled." : "Abgebrochen.");
            }
            finally
            {
                IsRunning = false;
                _cts?.Dispose();
                _cts = null;
            }
        }

        public async Task<bool> InstallSingleByDeviceHintAsync(
            string deviceHint,
            Func<string, Task> log,
            bool isEnglish,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(deviceHint))
                return false;

            DriverVendorTarget target = RedlineDriverVendorLinks.Resolve(deviceHint);
            bool en = isEnglish;
            string label = en ? target.LabelEn : target.LabelDe;

            if (!string.IsNullOrWhiteSpace(target.WingetPackageId))
            {
                await log(en ? "Installing " + label + "…" : "Installiere " + label + "…");
                bool ok = await TryWingetPackageAsync(target.WingetPackageId, log, en, ct);
                if (ok)
                {
                    RedlineDriverStatus.MarkRecentlyUpdated(deviceHint.Trim());
                    await log(en ? label + " updated." : label + " aktualisiert.");
                    return true;
                }
            }

            await log(en ? "Opening official page…" : "Öffne offizielle Seite…");
            try
            {
                Process.Start(new ProcessStartInfo { FileName = target.OfficialUrl, UseShellExecute = true });
                await log(en ? "Opened: " + label : "Geöffnet: " + label);
            }
            catch (Exception ex)
            {
                await log(en ? "Could not open browser: " + ex.Message : "Browser konnte nicht öffnen: " + ex.Message);
            }

            return false;
        }

        private static async Task TryWingetHardwareUpgradeAsync(
            HardwareProfile hardware,
            Func<string, Task> log,
            bool en,
            CancellationToken ct)
        {
            string which = await RunCmdCaptureAsync("where.exe", "winget", ct);
            if (string.IsNullOrWhiteSpace(which) || which.Contains("INFO: Could not find", StringComparison.OrdinalIgnoreCase))
            {
                await log(en ? "winget not found — install App Installer from Microsoft Store." : "winget fehlt — App Installer aus dem Microsoft Store installieren.");
                return;
            }

            List<WingetDriverPackage> plan = RedlineHardwareProfile.BuildWingetPackagesForHardware(hardware);
            string wingetArgs = "--disable-interactivity --accept-source-agreements --accept-package-agreements -e --silent";
            bool any = false;
            int i = 0;
            int total = plan.Count;

            foreach (WingetDriverPackage pkg in plan)
            {
                ct.ThrowIfCancellationRequested();
                i++;
                string label = en ? pkg.LabelEn : pkg.LabelDe;
                await log(en
                    ? $"Installing {label} ({i}/{total})…"
                    : $"Installiere {label} ({i}/{total})…");

                string target = pkg.IdOrQuery;
                string args = pkg.UseExactId
                    ? "upgrade --id \"" + target + "\" " + wingetArgs
                    : "upgrade --query \"" + target + "\" " + wingetArgs;

                (int code, string output) = await RunCmdCaptureAsyncFull("winget", args, ct, 300000);
                string result = SummarizeWingetOutput(output, code, en);
                await log(result);
                if (result.Contains("✅", StringComparison.Ordinal))
                    any = true;
            }

            if (!any)
                await log(en ? "Already up to date or not in winget." : "Bereits aktuell oder nicht über winget verfügbar.");
        }

        private static async Task<bool> TryWingetPackageAsync(
            string packageId,
            Func<string, Task> log,
            bool en,
            CancellationToken ct)
        {
            string which = await RunCmdCaptureAsync("where.exe", "winget", ct);
            if (string.IsNullOrWhiteSpace(which))
                return false;

            string wingetArgs = "--disable-interactivity --accept-source-agreements --accept-package-agreements -e --silent";
            string args = "upgrade --id \"" + packageId + "\" " + wingetArgs;
            (int code, string output) = await RunCmdCaptureAsyncFull("winget", args, ct, 300000);
            string result = SummarizeWingetOutput(output, code, en);
            await log(result);
            return result.Contains("✅", StringComparison.Ordinal);
        }

        private static string SummarizeWingetOutput(string output, int code, bool en)
        {
            foreach (string line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string t = line.Trim();
                if (t.Contains("Erfolgreich installiert", StringComparison.OrdinalIgnoreCase)
                    || t.Contains("Successfully installed", StringComparison.OrdinalIgnoreCase)
                    || t.Contains("upgraded", StringComparison.OrdinalIgnoreCase))
                    return "✅ " + (en ? "Updated." : "Aktualisiert.");

                if (t.Contains("Es wurden keine", StringComparison.OrdinalIgnoreCase)
                    || t.Contains("No applicable", StringComparison.OrdinalIgnoreCase)
                    || t.Contains("no upgrades", StringComparison.OrdinalIgnoreCase)
                    || t.Contains("nicht installiert", StringComparison.OrdinalIgnoreCase)
                    || t.Contains("not installed", StringComparison.OrdinalIgnoreCase))
                    return en ? "· Already current." : "· Bereits aktuell.";
            }

            if (code == 0)
                return en ? "· Already current." : "· Bereits aktuell.";
            return en ? "· No change (check vendor page)." : "· Keine Änderung (ggf. Hersteller-Seite).";
        }

        private static async Task<string> RunCmdCaptureAsync(string file, string args, CancellationToken ct)
        {
            (int _, string o) = await RunCmdCaptureAsyncFull(file, args, ct, 60000);
            return o;
        }

        private static async Task<(int exitCode, string output)> RunCmdCaptureAsyncFull(
            string file, string args, CancellationToken ct, int timeoutMs)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = file,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using Process? p = Process.Start(psi);
            if (p == null)
                return (-1, "");

            using CancellationTokenSource timeout = CancellationTokenSource.CreateLinkedTokenSource(ct);
            timeout.CancelAfter(timeoutMs);
            try
            {
                await p.WaitForExitAsync(timeout.Token);
            }
            catch (OperationCanceledException)
            {
                try { if (!p.HasExited) p.Kill(true); } catch { }
                throw;
            }

            string stdout = await p.StandardOutput.ReadToEndAsync();
            string stderr = await p.StandardError.ReadToEndAsync();
            return (p.ExitCode, stdout + stderr);
        }
    }
}
