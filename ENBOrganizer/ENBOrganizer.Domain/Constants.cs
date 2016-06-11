using System.Collections.Generic;

namespace ENBOrganizer.Domain
{
    public class GameNames
    {
        private const string Skyrim = "The Elder Scrolls V: Skyrim";
        private const string Oblivion = "The Elder Scrolls IV: Oblivion";
        private const string Fallout4 = "Fallout 4";
        private const string Fallout3 = "Fallout 3";
        private const string FalloutNewVegas = "Fallout: New Vegas";

        public static Dictionary<string, string> GameFriendlyNameMap = new Dictionary<string, string>
        {
            { Skyrim, "Skyrim" },
            { Oblivion, "Oblivion" },
            { Fallout3, "Fallout 3" },
            { Fallout4, "Fallout 4" },
            { FalloutNewVegas, "Fallout NV" }
        };

        public static Dictionary<string, string> KnownGamesDictionary = new Dictionary<string, string>
        {
            { Skyrim, "TESV.exe" },
            { Oblivion, "Oblivion.exe" },
            { Fallout3, "Fallout3.exe" },
            { Fallout4, "Fallout4.exe" },
            { FalloutNewVegas, "FalloutNV.exe" }
        };
    }

    // TODO: needs Fallout 3 and Oblivion data
    public class DirectoryNames
    {
        public const string Games = "Games";
        public const string Data = "Data";
        public const string Presets = "Presets";
        public const string Binaries = "Binaries";

        public static List<string> EssentialNames = new List<string>
        {
            "atimgpud.dll",
            "binkw32.dll",
            "Data",
            "ExitData.mhd",
            "Fallout New Vegas.lnk",
            "FalloutNV.exe",
            "FalloutNV.ico",
            "FalloutNVLauncher.exe",
            "Fallout_default.ini",
            "GDFFalloutNV.dll",
            "high.ini",
            "InstallScript.vdf",
            "libvorbis.dll",
            "libvorbisfile.dll",
            "low.ini",
            "MainTitle.wav",
            "mcm.log",
            "medium.ini",
            "nvse.log",
            "nvse_1_4.dll",
            "nvse_1_4ng.dll",
            "nvse_editor_1_4.dll",
            "nvse_loader.exe",
            "nvse_loader.log",
            "nvse_readme.txt",
            "nvse_steam_loader.dll",
            "nvse_steam_loader.log",
            "nvse_whatsnew.txt",
            "pn_nvse.log",
            "Readme.txt",
            "Redists",
            "steam_api.dll",
            "ui_organizer.log",
            "VeryHigh.ini",
            "bink2w64.dll",
            "cudart64_75.dll",
            "Fallout4",
            "Fallout4.exe",
            "Fallout4Launcher.exe",
            "Fallout4_Default.ini",
            "flexExtRelease_x64.dll",
            "flexRelease_x64.dll",
            "GFSDK_GodraysLib.x64.dll",
            "GFSDK_SSAO_D3D11.win64.dll",
            "installscript.vdf",
            "libScePad.dll",
            "msvcp110.dll",
            "msvcr110.dll",
            "nvdebris.txt",
            "nvToolsExt64_1.dll",
            "steam_api64.dll",
            "Ultra.ini",
            "_CommonRedist",
            "DirectX10",
            "DotNetFX",
            "Skyrim",
            "SkyrimLauncher.exe",
            "Skyrim_default.ini",
            "TESV.exe",
            "VCRedist"
        };
    }
}
