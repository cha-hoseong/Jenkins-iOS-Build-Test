using System.Linq;
using UnityEngine;

public class CommandUnityBuilder
{
    private static void PerformAndroidBuild()
    {
        try
        {
            UniversalBuildSettings.AssetBuild = ShouldPerformAssetBuild();
            UniversalBuildSettings.PerformAndroidBuild(GetBuildType(), GetBuildPath());
        }
        catch (System.Exception error)
        {
            Debug.LogError(error);
        }
    }
    
    private static void PerformiOSBuild()
    {
        // var buildPath = GetArgumentValue("-buildPath");
        // if (string.IsNullOrEmpty(buildPath))
        //     buildPath = "./";
        // UniversalBuildSettings.PerformiOSBuild(buildPath);
    }

    private static bool ShouldPerformAssetBuild()
    {
        return HasArgument("-assetBuild");
    }

    private static string GetBuildType()
    {
        var type = GetArgumentValue("-buildType");
        if (string.IsNullOrEmpty(type))
            throw new System.ArgumentNullException("-buildType", "Must have build type!");
        return type.ToLower();
    }

    private static string GetBuildPath()
    {
        var buildPath = GetArgumentValue("-buildPath");
        return string.IsNullOrEmpty(buildPath) ? "./" : buildPath;
    }

    private static bool HasArgument(string argument)
    {
        var args = System.Environment.GetCommandLineArgs();
        return args.Contains(argument);
    }

    private static string GetArgumentValue(string argument)
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