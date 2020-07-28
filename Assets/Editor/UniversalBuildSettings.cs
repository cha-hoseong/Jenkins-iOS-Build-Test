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

    public static void PerformAndroidForDevelopment(string outputDirectory)
    {
        PlayerSettings.colorSpace = ColorSpace.Linear;
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android,
            new [] { GraphicsDeviceType.Vulkan, GraphicsDeviceType.OpenGLES3 });
        PlayerSettings.MTRendering = true;
        PlayerSettings.SetMobileMTRendering(BuildTargetGroup.Android, true);
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel19;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android, Il2CppCompilerConfiguration.Debug);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Debug;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.development = true;
        EditorUserBuildSettings.connectProfiler = true;
        EditorUserBuildSettings.allowDebugging = true;

        var targetDirectory = Path.Combine(outputDirectory, "Build/Android/Development");
        if (!Directory.Exists(targetDirectory))
            Directory.CreateDirectory(targetDirectory);
        
        PerformBuild(targetDirectory, BuildTarget.Android, BuildOptions.CompressWithLz4);
    }
    
    public static void PerformiOSBuildForDevelopment(string outputDirectory)
    {
        PlayerSettings.colorSpace = ColorSpace.Linear;
        PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS, new [] { GraphicsDeviceType.Metal });
        PlayerSettings.MTRendering = true;
        PlayerSettings.SetMobileMTRendering(BuildTargetGroup.iOS, true);
        PlayerSettings.iOS.appleDeveloperTeamID = "V8F8H7L35W";
        PlayerSettings.iOS.appleEnableAutomaticSigning = true;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.muteOtherAudioSources = true;
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad;
        PlayerSettings.iOS.targetOSVersionString = "12.4";
        PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, (int)PlayerSettingsArchitecture.ARM64);

        EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Debug;
        EditorUserBuildSettings.development = true;
        EditorUserBuildSettings.connectProfiler = true;
        EditorUserBuildSettings.allowDebugging = true;
        EditorUserBuildSettings.symlinkLibraries = true;
        
        var targetDirectory = Path.Combine(outputDirectory, "Build/iOS/Development/Project");
        if (!Directory.Exists(targetDirectory))
            Directory.CreateDirectory(targetDirectory);

        PerformBuild(targetDirectory, BuildTarget.iOS, BuildOptions.CompressWithLz4);
    }

    private static void PerformBuild(string location, BuildTarget target, BuildOptions options)
    {
        var report = BuildPipeline.BuildPlayer(EnabledLevels, location, target, options);
        Debug.Log($"Build {report.summary.result}");
    }
}
