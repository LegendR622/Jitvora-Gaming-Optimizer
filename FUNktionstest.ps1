# Redline – Funktionstest (Build + System + Cleaner-Pfade)
$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$proj = Join-Path $root "GamingBooster_Pro\GamingBooster_Pro.csproj"
$exe = Join-Path $root "publish\win-x64\GamingBooster_Pro.exe"
$results = New-Object System.Collections.Generic.List[string]

function Add-Result($name, $ok, $detail = "") {
    $status = if ($ok) { "OK" } else { "FAIL" }
    $line = "[$status] $name"
    if ($detail) { $line += " | $detail" }
    $results.Add($line) | Out-Null
    Write-Host $line
}

Write-Host "=== Redline Funktionstest ===" -ForegroundColor Cyan

try {
    dotnet build $proj -c Release -v q | Out-Null
    Add-Result "dotnet build" $true
} catch {
    Add-Result "dotnet build" $false $_.Exception.Message
}

Add-Result "publish EXE" (Test-Path $exe) $exe

# Speicher WMI (wie App V9.3+)
try {
    $sys = [System.Environment]::SystemDirectory.Substring(0,2)
    $disk = Get-CimInstance Win32_LogicalDisk -Filter ("DeviceID='$sys'") | Select-Object -First 1
    $gb = [math]::Round($disk.Size / 1GB, 1)
    Add-Result "Festplatte $sys WMI" ($disk.Size -gt 0) ("$gb GB gesamt")
} catch {
    Add-Result "Festplatte WMI" $false $_.Exception.Message
}

# Cleaner-Ziele (Pfade existieren / erreichbar)
$local = [Environment]::GetFolderPath("LocalApplicationData")
$cleanerPaths = @(
    (Join-Path $local "D3DSCache"),
    (Join-Path $env:TEMP ""),
    "C:\Windows\Temp"
)
$okPaths = ($cleanerPaths | Where-Object { Test-Path $_ }).Count
Add-Result "Cleaner Temp/Cache Pfade" ($okPaths -ge 2) ("$okPaths von $($cleanerPaths.Count)")

try {
    $chrome = Join-Path $local "Google\Chrome\User Data\Default\Cache"
    Add-Result "Chrome Cache Pfad" (Test-Path (Split-Path $chrome)) $chrome
} catch { Add-Result "Chrome Cache Pfad" $false $_.Exception.Message }

# Update version.json
$verFile = Join-Path $root "version.json"
if (Test-Path $verFile) {
    $v = (Get-Content $verFile -Raw | ConvertFrom-Json).version
    Add-Result "version.json" ($v -eq "9.4") "v$v"
}

# Update-Log Ordner
$updLog = Join-Path $env:APPDATA "RedlineGamingOptimizer"
Add-Result "AppData Ordner" (Test-Path $updLog) $updLog

# App Smoke
if (Test-Path $exe) {
    Get-Process -Name "GamingBooster_Pro","Redline Gaming Optimizer" -EA SilentlyContinue | Stop-Process -Force -EA SilentlyContinue
    $env:REDLINE_SKIP_INTRO = "1"
    $env:REDLINE_START_PAGE = "Cleaner"
    $p = Start-Process $exe -PassThru
    Start-Sleep -Seconds 4
    $running = -not $p.HasExited
    Add-Result "App start (Cleaner)" $running ("PID $($p.Id)")
    if ($running) { Stop-Process -Id $p.Id -Force -EA SilentlyContinue }
    Remove-Item Env:REDLINE_START_PAGE -EA SilentlyContinue
}

$fail = @($results | Where-Object { $_ -like "[FAIL]*" }).Count
Write-Host ""
Write-Host "Ergebnis: $($results.Count - $fail)/$($results.Count) OK, $fail Fehler" -ForegroundColor $(if ($fail -eq 0) { "Green" } else { "Yellow" })
