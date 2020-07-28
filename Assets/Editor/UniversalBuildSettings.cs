using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class UniversalBuildSettings : ScriptableObject
{
    private enum PlayerSettingsArchitecture
    {
        None = 0,
        ARM64 = 1,
        Universal = 2,
    }
    
    public static bool AssetBuild { get; set; }

    private static string[] EnabledLevels => EditorBuildSettings.scenes
        .Where(scene => scene.enabled)
        .Select(scene => scene.path)
        .ToArray();

    #region Android
    public static void PerformAndroidBuild(string type, string path)
    {
        var location = Path.Combine(path, "Build/Android");
        BuildOptions options;
        
        switch (type)
        {
            case "development":
                SetAndroidBuildSettingsDevelopment();
                location = Path.Combine(location, "Development");
                options = BuildOptions.CompressWithLz4;
                break;
            case "qa":
                SetAndroidBuildSettingsQA();
                location = Path.Combine(location, "QA");
                options = BuildOptions.CompressWithLz4HC;
                break;
            case "release":
                SetAndroidBuildSettingsRelease();
                location = Path.Combine(location, "Release");
                options = BuildOptions.CompressWithLz4HC;
                break;
            default:
                throw new System.ArgumentException($"Invalid argument: {type}");
        }
        
        Directory.CreateDirectory(location);
        var output = Path.Combine(location, $"{PlayerSettings.productName}.apk");
        
        PerformBuild(output, BuildTarget.Android, options);
    }

    private static void SetAndroidBuildSettingsDevelopment()
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
    }

    private static void SetAndroidBuildSettingsQA()
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
        PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android, Il2CppCompilerConfiguration.Release);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.development = true;
        EditorUserBuildSettings.connectProfiler = true;
        EditorUserBuildSettings.allowDebugging = false;
    }

    private static void SetAndroidBuildSettingsRelease()
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
        PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android, Il2CppCompilerConfiguration.Master);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.connectProfiler = false;
        EditorUserBuildSettings.allowDebugging = false;
    }
    #endregion
    
    #region iOS
    public static void PerformiOSBuild(string type, string location)
    {
        var output = Path.Combine(location, "Build/iOS");
        BuildOptions options;
        
        switch (type)
        {
            case "development":
                SetiOSBuildSettingsDevelopment();
                output = Path.Combine(output, "Development");
                options = BuildOptions.CompressWithLz4;
                break;
            case "qa":
                output = Path.Combine(output, "QA");
                options = BuildOptions.CompressWithLz4HC;
                break;
            case "release":
                output = Path.Combine(output, "Release");
                options = BuildOptions.CompressWithLz4HC;
                break;
            default:
                throw new System.ArgumentException($"Invalid argument: {type}");
        }
        
        PerformBuild(output, BuildTarget.Android, options);
    }

    private static void SetiOSBuildSettingsDevelopment()
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
    }
    #endregion

    private static void PerformBuild(string location, BuildTarget target, BuildOptions options)
    {
        var report = BuildPipeline.BuildPlayer(EnabledLevels, location, target, options);
        Debug.Log($"Build {report.summary.result}");
    }
}