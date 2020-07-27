using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

public class TestPreset
{
    [MenuItem("Test/Apply to PlayerSettings Preset")]
    private static void ApplyToPlayerSettingsPreset()
    {
        var preset = AssetDatabase.LoadAssetAtPath<Preset>("Assets/Presets/PlayerSettingsRelease.preset");
        var projectSettings = AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/ProjectSettings.asset");
        preset.ApplyTo(projectSettings);
        Debug.Log("Done");
    }
}
