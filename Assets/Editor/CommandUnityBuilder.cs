using System.Collections;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class CommandUnityBuilder
{
    private static void PerformAndroid()
    {
        var type = GetArgumentValue("-buildType");
        if (string.IsNullOrEmpty(type))
        {
            Debug.Log($"Invalid command: {type}");
            return;
        }

        type = type.ToLower();
        var path = GetBuildPath();

        switch (type)
        {
            case "development":
                UniversalBuildSettings.PerformAndroidForDevelopment(path);
                break;
            case "qa":
                break;
            case "release":
                break;
        }
    }
    
    private static void PerformiOSBuild()
    {
        var type = GetArgumentValue("-buildType");
        if (string.IsNullOrEmpty(type))
        {
            Debug.Log($"Invalid command: {type}");
            return;
        }
        
        type = type.ToLower();
        var path = GetBuildPath();
        
        switch (type)
        {
            case "development":
                UniversalBuildSettings.PerformiOSBuildForDevelopment(path);
                break;
            case "qa":
                break;
            case "release":
                break;
        }
    }

    private static string GetBuildPath()
    {
        var buildPath = GetArgumentValue("-buildPath");
        return string.IsNullOrEmpty(buildPath) ? "./" : buildPath;
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