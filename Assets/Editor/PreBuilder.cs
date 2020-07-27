using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class PreBuilder : IPreprocessBuildWithReport
{
    public int callbackOrder { get; }
    
    public void OnPreprocessBuild(BuildReport report)
    {
        // 빌드 전에 해야할 작업을 수행한다.
        Debug.Log("PreBuild");
    }
}
