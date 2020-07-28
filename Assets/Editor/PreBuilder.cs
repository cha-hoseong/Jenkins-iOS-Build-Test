using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class PreBuilder : IPreprocessBuildWithReport
{
    public int callbackOrder { get; }
    
    public void OnPreprocessBuild(BuildReport report)
    {
        if (UniversalBuildSettings.AssetBuild)
            AddressableAssetSettings.BuildPlayerContent();
    }
}