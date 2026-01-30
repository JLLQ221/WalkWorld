using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Collections.Generic;

public static class BuildWebGL
{
    public static void PerformBuild()
    {
        // Ruta de salida del build
        string buildPath = "C:/Users/jael1/Downloads/Build WalkWorld/Web/";

        // Escenas a incluir (ajusta estas rutas según tu proyecto)
        var scenes = new[]
        {
            "Assets/Scenes/Word.unity", "Assets/Scenes/PlaceSeedd.unity", "Assets/Scenes/SeedPlanten.unity"
        };

        // Opciones de build
        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        // Player settings recomendados para WebGL
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip; // Brotli si tu servidor lo soporta
        PlayerSettings.WebGL.debugSymbols = false;
        PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm;
        PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.WebGL, Il2CppCompilerConfiguration.Release);

        // Realizar build
        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            UnityEngine.Debug.Log($"Build WebGL completado: {summary.totalSize} bytes en {buildPath}");
        }
        else
        {
            throw new System.Exception($"Build WebGL falló: {summary.result}");
        }
    }
}
