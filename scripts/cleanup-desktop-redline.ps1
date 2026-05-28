# Alte REDLINE MP4 + GamingBooster EXE vom Desktop entfernen (eine Verknuepfung bleibt)
$desktop = [Environment]::GetFolderPath("Desktop")
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)

@("REDLINE_Intro.mp4","REDLINE_Promo.mp4","REDLINE.mp4","Redline Gaming Optimizer V9.lnk","Redline UPDATE-TEST (V9.0).lnk") | ForEach-Object {
    $p = Join-Path $desktop $_
    if (Test-Path $p) { Remove-Item $p -Force; Write-Host "Geloescht: $_" }
}

Get-ChildItem $desktop -Filter "*.mp4" -File -EA SilentlyContinue | Where-Object { $_.Name -match 'REDLINE|Redline|GamingBooster|Intro|Promo' } | Remove-Item -Force
Get-ChildItem $desktop -Filter "GamingBooster_Pro.exe" -File -Recurse -EA SilentlyContinue | Remove-Item -Force
$df = Join-Path $desktop "GamingBooster_Pro"
$repoCsproj = Join-Path $df "GamingBooster_Pro\GamingBooster_Pro.csproj"
if ((Test-Path $df) -and -not (Test-Path $repoCsproj)) {
    Remove-Item $df -Recurse -Force
    Write-Host "Ordner-Kopie geloescht: $df"
}
elseif (Test-Path $repoCsproj) {
    Write-Host "Projektordner bleibt (ist dein Quellcode)."
}

$exe = Join-Path $root "publish\win-x64\GamingBooster_Pro.exe"
$lnk = Join-Path $desktop "Redline Gaming Optimizer.lnk"
$icon = Join-Path $root "GamingBooster_Pro\redline.ico"
if (Test-Path $exe) {
    $wsh = New-Object -ComObject WScript.Shell
    $sc = $wsh.CreateShortcut($lnk)
    $sc.TargetPath = $exe
    $sc.WorkingDirectory = Split-Path $exe
    if (Test-Path $icon) { $sc.IconLocation = "$icon,0" }
    $sc.Save()
    Write-Host "OK: eine Verknuepfung -> $exe"
}
