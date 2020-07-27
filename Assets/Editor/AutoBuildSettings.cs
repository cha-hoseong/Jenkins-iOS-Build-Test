using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Presets;
using UnityEngine;

[CreateAssetMenu(fileName = "AutoBuildSettings", menuName = "Auto Build Settings")]
public class AutoBuildSettings : ScriptableObject
{
    [Header("PlayerSettings Preset")]
    [SerializeField] private Preset development = null;
    [SerializeField] private Preset debug = null;
    [SerializeField] private Preset release = null;

    // ReSharper disable once InconsistentNaming
    private static AutoBuildSettings instance;

    private const string ProjectSettingsAssetPath = "ProjectSettings/ProjectSettings.asset";

    private AutoBuildSettings()
    {
        instance = this;
    }
    
    [MenuItem("Build/Android/Development")]
    private static void PerformAndroidBuildForDevelopment()
    {
        var arguments = System.Environment.GetCommandLineArgs();

        if (arguments.Contains("-batchmode"))
        {
            
        }
        
        var projectSettings = AssetDatabase.LoadAssetAtPath<Object>(ProjectSettingsAssetPath);
        instance.development.ApplyTo(projectSettings);
        
        var scenes = FindEnabledEditorScenes();
        var targetDirectory = $"Build/Android/Development/{System.DateTime.Now:yyyy-MM-dd}";
        if (!Directory.Exists(targetDirectory))
            Directory.CreateDirectory(targetDirectory);
        var appName = $"{PlayerSettings.productName} Development {System.DateTime.Now:HHmm}.apk";
        var output = Path.Combine(targetDirectory, appName);
        const BuildOptions options = BuildOptions.Development | BuildOptions.ConnectWithProfiler;

        PerformBuild(scenes, output, BuildTarget.Android, options);
    }

    private string GetBuildPath()
    {
        
    }

    private static void PerformBuild(string[] scenes, string location, BuildTarget target, BuildOptions options)
    {
        var report = BuildPipeline.BuildPlayer(scenes, location, target, options);
    }

    private static string[] FindEnabledEditorScenes()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled == true)
            .Select(scene => scene.path)
            .ToArray();
    }

    public static string GetArgumentValue(string argument)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (var i = 0; i < args.Length; ++i)
        {
            if (args[i] == argument && i + 1 < args.Length)
                return args[i + 1];
        }
        return string.Empty;
    }
}