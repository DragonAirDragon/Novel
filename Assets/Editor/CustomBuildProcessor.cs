using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class CustomBuildProcessor
{
    public static void CustomBuildMethod()
    {
        var options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/MainScene.unity" },
            locationPathName = "Builds/MyUnityGame.exe",  // Директория для билда
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        Debug.Log("Starting pre-build processing...");
        Naninovel.BuildProcessor.PreprocessBuild(options);
        Debug.Log("Building player...");
        BuildPipeline.BuildPlayer(options);
        Debug.Log("Starting post-build processing...");
        Naninovel.BuildProcessor.PostprocessBuild();
        Debug.Log($"Build completed. File saved to: {options.locationPathName}");
    }
}
