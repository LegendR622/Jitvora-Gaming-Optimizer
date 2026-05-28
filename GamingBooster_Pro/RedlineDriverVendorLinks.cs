using System;
using System.Collections.Generic;

namespace GamingBooster_Pro
{
    internal sealed class DriverVendorTarget
    {
        public string LabelDe { get; set; } = "";
        public string LabelEn { get; set; } = "";
        public string OfficialUrl { get; set; } = "";
        public string? WingetPackageId { get; set; }
    }

    internal static class RedlineDriverVendorLinks
    {
        public static DriverVendorTarget Resolve(string deviceName, HardwareProfile? hp = null)
        {
            string d = (deviceName ?? "").ToLowerInvariant();
            hp ??= new HardwareProfile();

            if (d.Contains("nvidia") || d.Contains("geforce") || d.Contains("rtx") || d.Contains("gtx"))
                return Target("NVIDIA Grafiktreiber", "NVIDIA graphics driver",
                    "https://www.nvidia.com/Download/index.aspx", "NVIDIA.GraphicsDriver");

            if (d.Contains("radeon") || (d.Contains("amd") && d.Contains("graphics")))
                return Target("AMD Grafiktreiber", "AMD graphics driver",
                    "https://www.amd.com/en/support/download/drivers.html", null);

            if (d.Contains("intel") && (d.Contains("graphics") || d.Contains("uhd") || d.Contains("iris") || d.Contains("arc")))
                return Target("Intel Grafiktreiber", "Intel graphics driver",
                    "https://www.intel.com/content/www/us/en/support/detect.html", null);

            if (d.Contains("realtek") && d.Contains("audio"))
                return Target("Realtek Audio", "Realtek audio",
                    "https://www.realtek.com/Download/List?cate_id=584", null);

            if (d.Contains("realtek"))
                return Target("Realtek LAN/Audio", "Realtek LAN/audio",
                    "https://www.realtek.com/Download/List?cate_id=584", null);

            if (d.Contains("high definition audio") || d.Contains("audio"))
                return Target("Audio-Treiber", "Audio driver",
                    "https://www.realtek.com/Download/List?cate_id=584", null);

            if (d.Contains("killer") || d.Contains("e2200") || d.Contains("e2500"))
                return Target("Killer Ethernet", "Killer Ethernet",
                    "https://www.intel.com/content/www/us/en/download-center/home.html", null);

            if (d.Contains("marvell") || d.Contains("aqtion") || d.Contains("10g"))
                return Target("Marvell LAN", "Marvell LAN",
                    "https://www.marvell.com/support.html", null);

            if (d.Contains("wi-fi") || d.Contains("wifi") || d.Contains("wireless") || d.Contains("ax201") || d.Contains("ax210"))
                return Target("Intel Wi-Fi", "Intel Wi-Fi",
                    "https://www.intel.com/content/www/us/en/download/19351/intel-wireless-wi-fi-drivers-for-windows-10-and-windows-11.html", null);

            if (d.Contains("ethernet") || d.Contains("i225") || d.Contains("i219") || d.Contains("lan"))
                return Target("Intel Ethernet", "Intel Ethernet",
                    "https://www.intel.com/content/www/us/en/download/19779/intel-ethernet-adapter-complete-driver-pack.html", null);

            if (d.Contains("bluetooth"))
                return Target("Bluetooth", "Bluetooth",
                    "https://www.intel.com/content/www/us/en/download/18649/intel-wireless-bluetooth-drivers-for-windows-10-and-windows-11.html", null);

            if (d.Contains("chipset") || d.Contains("smbus") || d.Contains("pci express"))
            {
                if (RedlineHardwareProfile.CpuVendor(hp.CpuName) == "AMD")
                    return Target("AMD Chipsatz", "AMD chipset",
                        "https://www.amd.com/en/support/chipsets/amd-socket-am5/am5", "AMD.Chipset.Software");
                return Target("Intel Chipsatz", "Intel chipset",
                    "https://www.intel.com/content/www/us/en/support/detect.html", null);
            }

            string mb = (hp.MotherboardManufacturer + " " + hp.MotherboardProduct).ToLowerInvariant();
            if (mb.Contains("asus"))
                return Target("ASUS Mainboard", "ASUS motherboard",
                    "https://www.asus.com/support/download-center/", null);
            if (mb.Contains("msi"))
                return Target("MSI Mainboard", "MSI motherboard",
                    "https://www.msi.com/support/download", null);
            if (mb.Contains("gigabyte"))
                return Target("Gigabyte Mainboard", "Gigabyte motherboard",
                    "https://www.gigabyte.com/Support", null);
            if (mb.Contains("asrock"))
                return Target("ASRock Mainboard", "ASRock motherboard",
                    "https://www.asrock.com/support/index.asp", null);

            return Target("Hersteller-Support", "Vendor support",
                "https://www.intel.com/content/www/us/en/support/detect.html", null);
        }

        private static DriverVendorTarget Target(string de, string en, string url, string? wingetId) =>
            new DriverVendorTarget { LabelDe = de, LabelEn = en, OfficialUrl = url, WingetPackageId = wingetId };
    }
}
