using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Linq;

public class BuildAll : EditorWindow
{
    private string _binaryName = "";
    private static readonly string ReleaseFolder = Application.dataPath + "/../Release/";
    private string _release = "";
    private string _publishRoot = "";
    private string _packageFolder = "";
    private bool _buildWindows;
    private bool _buildWindows64;
    private bool _buildLinux;
    private bool _buildWeb;
    private bool _buildPackage;
    private bool _buildAndroid;

    [MenuItem("File/BuildAll/Settings", priority=3)]
    public static void OpenSettings()
    {
        GetWindow<BuildAll>();
    }

    public BuildAll()
    {
        title = "Build All";

        _binaryName = BuildAllSettings.GetSetting("BinaryName");
        _release = BuildAllSettings.GetSetting("Release");
        _publishRoot = BuildAllSettings.GetSetting("PublishRoot");
        _packageFolder = BuildAllSettings.GetSetting("PackageFolder");
        _buildWindows = bool.Parse(BuildAllSettings.GetSetting("BuildWindows"));
        _buildWindows64 = bool.Parse(BuildAllSettings.GetSetting("BuildWindows64"));
        _buildLinux = bool.Parse(BuildAllSettings.GetSetting("BuildLinux"));
        _buildWeb = bool.Parse(BuildAllSettings.GetSetting("BuildWeb"));
        _buildPackage = bool.Parse(BuildAllSettings.GetSetting("BuildPackage"));
        _buildAndroid = bool.Parse(BuildAllSettings.GetSetting("BuildAndroid"));
    }

    public void OnGUI()
    {         

        GUILayout.Label("Binary Name: ");
        _binaryName = GUILayout.TextField(_binaryName);

        GUILayout.Label("Release Version: ");
        _release = GUILayout.TextField(_release);

        GUILayout.Label("Publish Root Folder: ");
        _publishRoot = GUILayout.TextField(_publishRoot);

        GUILayout.Label("Package Root Folder: ");
        _packageFolder = GUILayout.TextField(_packageFolder);

        GUILayout.Label("Build Targets: ");
        _buildWindows = GUILayout.Toggle(_buildWindows, "Windows");
        _buildWindows64 = GUILayout.Toggle(_buildWindows64, "Windows 64");
        _buildLinux = GUILayout.Toggle(_buildLinux, "Linux");
        _buildWeb = GUILayout.Toggle(_buildWeb, "Web");
        _buildAndroid = GUILayout.Toggle(_buildAndroid, "Android");
        _buildPackage = GUILayout.Toggle(_buildPackage, "Package");

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Build"))
        {
            SaveSettings();
            Build();
        }

        if (GUILayout.Button("Apply"))
        {
            SaveSettings();
        }

        if (GUILayout.Button("Create Folders"))
        {
            CreateFolders();
        }

        if (GUILayout.Button("Clean Up"))
        {
            if(Directory.Exists(ReleaseFolder))
                Directory.Delete(ReleaseFolder, true);

            Directory.CreateDirectory(ReleaseFolder);
        }

        GUILayout.EndHorizontal();
    }

    public void SaveSettings()
    {
        BuildAllSettings.SetSetting("BinaryName", _binaryName);
        BuildAllSettings.SetSetting("Release", _release);
        BuildAllSettings.SetSetting("PublishRoot", _publishRoot);
        BuildAllSettings.SetSetting("PackageFolder", _packageFolder);
        BuildAllSettings.SetSetting("BuildWindows", _buildWindows.ToString());
        BuildAllSettings.SetSetting("BuildWindows64", _buildWindows64.ToString());
        BuildAllSettings.SetSetting("BuildLinux", _buildLinux.ToString());
        BuildAllSettings.SetSetting("BuildWeb", _buildWeb.ToString());
        BuildAllSettings.SetSetting("BuildPackage", _buildPackage.ToString());
        BuildAllSettings.SetSetting("BuildAndroid", _buildAndroid.ToString());
    }

    public static void CreateFolders()
    {
        var projectFolder = Application.dataPath + "/../";
        var folders = new[]
        {
            projectFolder + "NonUnityAssets", projectFolder + "Release", projectFolder + "Documents",
            Application.dataPath + "/Scripts", Application.dataPath + "/Scenes", Application.dataPath + "/Prefabs", 
            Application.dataPath + "/Animation", Application.dataPath + "/Sounds", Application.dataPath + "/Sprites",
            Application.dataPath + "/Models", Application.dataPath + "/Materials"
        };

        foreach (var f in folders.Where(f => !Directory.Exists(f)))
            Directory.CreateDirectory(f);

        if (File.Exists(projectFolder + ".gitignore")) return;

        var gitfile = (from a in AssetDatabase.GetAllAssetPaths()
            where a.EndsWith(".gitignore")
            select a).First();

        File.Copy(gitfile, projectFolder + ".gitignore");
    }

    [MenuItem("File/BuildAll/Build", priority = 1)]
    public static void Build()
    {
        var scenes = (from s in EditorBuildSettings.scenes where s.enabled select s.path).ToArray();
        var binaryName = BuildAllSettings.GetSetting("BinaryName");
        var buildWindows = bool.Parse(BuildAllSettings.GetSetting("BuildWindows"));
        var buildWindows64 = bool.Parse(BuildAllSettings.GetSetting("BuildWindows64"));
        var buildLinux = bool.Parse(BuildAllSettings.GetSetting("BuildLinux"));
        var buildWeb = bool.Parse(BuildAllSettings.GetSetting("BuildWeb"));
        var buildPackage = bool.Parse(BuildAllSettings.GetSetting("BuildPackage"));
        var buildAndroid = bool.Parse(BuildAllSettings.GetSetting("BuildAndroid"));


        //Build Players - Add more or comment out as needed
        if (buildWindows)
            BuildPlayer(scenes, ReleaseFolder + "Windows", binaryName + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);
        if (buildWindows64)
            BuildPlayer(scenes, ReleaseFolder + "Windows 64", binaryName + "64.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
        if (buildWeb)
            BuildPlayer(scenes, ReleaseFolder + "Web", binaryName, BuildTarget.WebPlayer, BuildOptions.None);
        if (buildLinux)
            BuildPlayer(scenes, ReleaseFolder + "Linux", binaryName, BuildTarget.StandaloneLinuxUniversal, BuildOptions.None);
        if(buildAndroid)
            BuildPlayer(scenes, ReleaseFolder + "Android", binaryName + ".apk", BuildTarget.Android, BuildOptions.None);

        //Export packages 
        if (buildPackage)
            BuildPackage("Assets/Packages/BuildAll", ReleaseFolder + "Package", binaryName + ".unitypackage");

        EditorUtility.DisplayDialog("Build Complete", "", "Ok");
    }

    [MenuItem("File/BuildAll/Publish", priority = 2)]
    public static void PublishAll()
    {
        var publishRoot = BuildAllSettings.GetSetting("PublishRoot");
        var release = BuildAllSettings.GetSetting("Release");
        var binaryName = BuildAllSettings.GetSetting("BinaryName");
        var publishFolder = publishRoot + "/Release" + release;
        var buildWindows = bool.Parse(BuildAllSettings.GetSetting("BuildWindows"));
        var buildWindows64 = bool.Parse(BuildAllSettings.GetSetting("BuildWindows64"));
        var buildLinux = bool.Parse(BuildAllSettings.GetSetting("BuildLinux"));
        var buildWeb = bool.Parse(BuildAllSettings.GetSetting("BuildWeb"));
        var buildPackage = bool.Parse(BuildAllSettings.GetSetting("BuildPackage"));
        var buildAndroid = bool.Parse(BuildAllSettings.GetSetting("BuildAndroid"));

        if (Directory.Exists(publishFolder))
        {
            Directory.Delete(publishFolder, true);
        }

        Directory.CreateDirectory(publishFolder);

        if (buildPackage)
            File.Copy(ReleaseFolder + "Package/" + binaryName + ".unitypackage", publishFolder + "/" + binaryName + ".unitypackage");
        if (buildAndroid)
            File.Copy(ReleaseFolder + "Android/" + binaryName + ".apk", publishFolder + "/" + binaryName + ".apk");
        if (buildWindows)
            ZipFolder(ReleaseFolder + "Windows", publishFolder + "/" + binaryName + "-Windows.zip");
        if (buildWindows64)
            ZipFolder(ReleaseFolder + "Windows 64", publishFolder + "/" + binaryName + "-Windows64.zip");
        if (buildLinux)
            ZipFolder(ReleaseFolder + "Linux", publishFolder + "/" + binaryName + "-Linux-Universal.zip");
        if (buildWeb)
            CopyFolder(ReleaseFolder + "Web", publishFolder);

        EditorUtility.DisplayDialog("Publish Complete", "", "Ok");
    }

    static void BuildPackage(string assetfolder, string folder, string filename)
    {
        if (Directory.Exists(folder))
        {
            Directory.Delete(folder, true);
        }

        Directory.CreateDirectory(folder);

        AssetDatabase.ExportPackage(assetfolder, folder + "/" + filename, ExportPackageOptions.Recurse);
    }

    static void BuildPlayer(string[] scenes, string folder, string binary, BuildTarget target, BuildOptions options)
    {
        if (Directory.Exists(folder))
        {
            Directory.Delete(folder, true);
        }

        if (binary != "")
            BuildPipeline.BuildPlayer(scenes, folder + "/" + binary, target, options);
        else
            BuildPipeline.BuildPlayer(scenes, folder, target, options);
    }

    static void CopyFolder(string source, string target)
    {
        var sourceInfo = new DirectoryInfo(source);

        if (!Directory.Exists(target))
            Directory.CreateDirectory(target);

        foreach (var f in sourceInfo.GetFiles())
            f.CopyTo(target + "\\" + f.Name);

        foreach (var d in sourceInfo.GetDirectories())
        {
            CopyFolder(d.FullName, target + "/" + d.Name);
        }
    }

    static void ZipFolder(string source, string filename)
    {
        var zipPath = (from a in AssetDatabase.GetAllAssetPaths()
                       where a.ToLower().EndsWith("7za.exe")
                       select a).First();

        var procinfo = new ProcessStartInfo
        {
            FileName = Application.dataPath + "/../" + zipPath,
            Arguments = "a -tzip \"" + filename + "\" \"" + source + "\"",
            WindowStyle = ProcessWindowStyle.Hidden
        };

        var process = Process.Start(procinfo);
        if (process != null) process.WaitForExit();
    }

}
