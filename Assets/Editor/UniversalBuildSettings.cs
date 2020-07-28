using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class UniversalBuildSettings : ScriptableObject
{
    private enum PlayerSettingsArchitecture
    {
        None,
        ARM64,
        Universal,
    }
    
    private static string[] EnabledLevels => EditorBuildSettings.scenes
        .Where(scene => scene.enabled)
        .Select(scene => scene.path)
        .ToArray();
    
    public static void PerformiOSBuild(string outputDirectory)
    {
        PlayerSettings.colorSpace = ColorSpace.Linear;
        PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS, new [] { GraphicsDeviceType.Metal });
        PlayerSettings.MTRendering = true;
        PlayerSettings.SetMobileMTRendering(BuildTargetGroup.iOS, true);
        PlayerSettings.iOS.appleDeveloperTeamID = "Archipin";
        PlayerSettings.iOS.appleEnableAutomaticSigning = true;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.muteOtherAudioSources = true;
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad;
        PlayerSettings.iOS.targetOSVersionString = "12.4";
        PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, (int)PlayerSettingsArchitecture.ARM64);

        var targetDirectory = Path.Combine(outputDirectory, "Build/iOS/Development/Project");
        if (!Directory.Exists(targetDirectory))
            Directory.CreateDirectory(targetDirectory);
        const BuildOptions options = BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.SymlinkLibraries;
        
        PerformBuild(targetDirectory, BuildTarget.iOS, options);
    }

    private static void PerformBuild(string location, BuildTarget target, BuildOptions options)
    {
        var report = BuildPipeline.BuildPlayer(EnabledLevels, location, target, options);
        Debug.Log($"Build {report.summary.result}");
    }
}
