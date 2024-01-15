using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace CS2GSI;

public class GsiConfigInstaller
{
    internal static void InstallGsi()
    {
        string installLocation = Path.Combine(GetInstallDirectory(), "game\\csgo\\cfg\\gamestate_integration_cs2gsi.cfg");
        File.WriteAllText(installLocation, Resources.GSI_CFG_Content);
    }

    private static string GetInstallDirectory(int appId = 730)
    {
        string libraryFolderFilepath = GetLibraryFoldersFilePath();
        string? libraryPath = null;
        string? appManifestFolderPath = null;
        foreach (string line in File.ReadAllLines(libraryFolderFilepath))
            if (line.Contains("path"))
                libraryPath = line.Split("\"").Last(split => split.Length > 0);
            else if (line.Contains($"\"{appId}\""))
                appManifestFolderPath = Path.Combine(libraryPath!, $"steamapps\\appmanifest_{appId}.acf");

        string installationPath = "";
        if (appManifestFolderPath is null)
            throw new DirectoryNotFoundException($"No {appId} Installation found.");
        foreach(string line in File.ReadAllLines(appManifestFolderPath))
            if (line.Contains("installdir"))
                installationPath = Path.Combine(libraryPath!, "steamapps\\common", line.Split("\"").Last(split => split.Length > 0));
          
        return installationPath;
    }
    
    private static string GetLibraryFoldersFilePath()
    {
        string? path = null;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            path = GetLibraryFoldersFilePathWindows();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            path = "~/.local/share/Steam/steamapps/libraryfolders.vdf";
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            path = "~/Library/Application Support/Steam/steamapps/libraryfolders.vdf";
        else throw new Exception("Could not get Installation FolderPath");
        return path ?? throw new FileNotFoundException("No libraryfolders.vdf found");
    }

    [SupportedOSPlatform("windows")]
    private static string GetLibraryFoldersFilePathWindows()
    {
        string steamInstallation =
            (string)(Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamPath", null) ??
                     throw new DirectoryNotFoundException("No Steam Installation found."));
        return Path.Combine(steamInstallation, "steamapps\\libraryfolders.vdf");
    }
}