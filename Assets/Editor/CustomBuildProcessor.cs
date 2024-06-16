using UnityEditor;
using UnityEditor.Build.Reporting;

public static class CustomBuildProcessor
{
    public static void CustomBuildMethod()
    {
        var options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/MainScene.unity" },
            locationPathName = "Builds/Build/Build.exe",  // Директория для билда
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        Naninovel.BuildProcessor.PreprocessBuild(options);
        BuildPipeline.BuildPlayer(options);
        Naninovel.BuildProcessor.PostprocessBuild();
    }
}
