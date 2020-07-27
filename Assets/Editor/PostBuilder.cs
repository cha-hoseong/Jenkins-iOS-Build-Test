using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class PostBuilder : IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
        // 빌드 후 처리해야할 작업을 수행한다.
        Debug.Log("PostBuild");
    }
}
