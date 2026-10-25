#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Linq;

public static class NexDriveBuild
{
    public static void BuildAndroid()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.nexvora.nexdrive3d");
        PlayerSettings.productName = "NexDrive 3D";
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.Android.bundleVersionCode = 1;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel35;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        var scenes = new[] { "Assets/Scenes/Main.unity" };
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions { scenes = scenes, locationPathName = "build/NexDrive3D.apk", target = BuildTarget.Android, options = BuildOptions.None });
        if (report.summary.result != BuildResult.Succeeded) throw new System.Exception("Android build failed: " + report.summary.result);
        Debug.Log("NEXDRIVE_BUILD_SUCCESS: " + report.summary.outputPath);
    }
}
#endif
