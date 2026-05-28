@echo off
title REDLINE Promo erstellen
echo Oeffne ChatGPT (Plus) fuer Text-Ideen...
start https://chatgpt.com/
echo.
echo Starte Aufnahme + Effekte (2K, eine MP4 auf Desktop)...
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\build-promo-final.ps1"
pause
