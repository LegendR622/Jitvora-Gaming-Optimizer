# Redline Gaming Optimizer — Release Status (V1.9.6)

**Date:** 2026-06-12  
**Status:** Published online — **Full QA passed** (see `QA_FULL_REPORT.md`)

| Item | Value |
|------|--------|
| **Version** | **1.9.6** |
| **Installer** | `Redline_Gaming_Optimizer_Setup_v1.9.6.exe` |
| **Public EXE** | `Redline Gaming Optimizer.exe` |
| **Release** | https://github.com/LegendR622/Redline-Gaming-Optimizer/releases/tag/v1.9.6 |
| **Download** | https://github.com/LegendR622/Redline-Gaming-Optimizer/releases/download/v1.9.6/Redline_Gaming_Optimizer_Setup_v1.9.6.exe |
| **Website** | https://legendr622.github.io/Redline-Gaming-Optimizer/ |
| **Desktop** | `C:\Users\Tobi\Desktop\Redline_Release_Latest` |

## V1.9.6 highlights

- AMD Ryzen CPU temp: no fake readings; admin/unavailable hints; Help + Retry (LibreHardwareMonitor info only)
- Driver link fixes (1.9.5) + dashboard telemetry polish

## QA summary (2026-06-12)

| Area | Result |
|------|--------|
| Build + installer | OK |
| UI selftest (all pages + 26 actions) | OK |
| Pro selftest (35 tools) | OK |
| Headless selftest | 272/273 OK (Authenticode only) |
| Live download + trust SHA | OK |
| Secrets scan | OK |
| VM risky tests | Skipped (no Sandbox) |

## SHA256 (published installer)

```
ac648dc475a7441df70893dda109bf476f40c8452ca4db70cf41f2c4250cfa62
```

## Notes

- Unsigned build (SmartScreen warning expected)
- AMD 7800X3D: CPU temp may show unavailable until LHM/PawnIO reads valid sensor; GPU temp OK
