using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace CS2GSI;

internal static class GsiConfigInstaller
{
    internal static void InstallGsi()
    {
        string gameInstallDir = GetInstallDirectory(); 
        string installLocation = Path.Combine(gameInstallDir, "game", "csgo", "cfg", "gamestate_integration_cs2gsi.cfg");
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
                appManifestFolderPath = Path.Join("steamapps", $"appmanifest_{appId}.acf");

        if (libraryPath is null)
            throw new DirectoryNotFoundException("No LibraryFolder path.");
        else if(appManifestFolderPath is null)
            throw new FileNotFoundException("No app manifest.");
        else
            appManifestFolderPath = Path.Combine(libraryPath, appManifestFolderPath);

        string installationPath = "";
        if (appManifestFolderPath is null)
            throw new DirectoryNotFoundException($"No {appId} Installation found.");
        foreach(string line in File.ReadAllLines(appManifestFolderPath))
            if (line.Contains("installdir"))
                installationPath = Path.Combine(libraryPath, "steamapps", "common", line.Split("\"").Last(split => split.Length > 0));
          
        return installationPath;
    }
    
    private static string GetLibraryFoldersFilePath()
    {
        string? path = null;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            path = GetLibraryFoldersFilePathWindows();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            string home = Environment.GetEnvironmentVariable("HOME") ?? "~";
            path = $"{home}/.local/share/Steam/steamapps/libraryfolders.vdf";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            string home = Environment.GetEnvironmentVariable("HOME") ?? "~";
            path = $"{home}/Library/Application Support/Steam/steamapps/libraryfolders.vdf";
        }
        else throw new Exception(Resources.No_Installation_Folderpath);
        return path ?? throw new FileNotFoundException(Resources.No_Libraryfolders_vdf);
    }

    [SupportedOSPlatform("windows")]
    private static string GetLibraryFoldersFilePathWindows()
    {
        string steamInstallation =
            (string)(Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamPath", null) ??
                     throw new DirectoryNotFoundException(Resources.No_Steam));
        return Path.Join(steamInstallation, "steamapps", "libraryfolders.vdf");
    }
}