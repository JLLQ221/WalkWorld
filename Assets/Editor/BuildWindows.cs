using UnityEditor;
using UnityEngine;

public class BuildWindows
{
    public static void PerformBuild()
    {
        // Ruta de salida del build
        string buildPath = "C:/Users/jael1/Downloads/Build WalkWorld/PC/WalkWorld.exe";

        // Escenas a incluir (ajusta estas rutas según tu proyecto)
        var scenes = new[]
        {
            "Assets/Scenes/Word.unity", "Assets/Scenes/PlaceSeedd.unity", "Assets/Scenes/SeedPlanten.unity"
        };

        BuildPipeline.BuildPlayer(
            scenes,
            buildPath,
            BuildTarget.StandaloneWindows64,
            BuildOptions.None
        );
    }
}
