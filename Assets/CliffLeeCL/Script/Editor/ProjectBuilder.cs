using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CliffLeeCL
{
    /// <summary>
    /// Build project with menu item and provide method for CI auto build.
    /// </summary>
    public class ProjectBuilder
    {
        [MenuItem("Cliff Lee CL/Build project Default _F5", false, 0)]
        public static void BuildProject()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            BuildSetting buildSetting = new BuildSetting();

            // Handle command line arguments.
            if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
            {
                Dictionary<string, string> commandLineArgumentToValue = ParseCommandLineArgument();

                if (commandLineArgumentToValue.ContainsKey("-outputPath"))
                    buildSetting.outputPath = commandLineArgumentToValue["-outputPath"];
                if (commandLineArgumentToValue.ContainsKey("-defineSymbolConfig"))
                    buildSetting.symbolConfig = (BuildSetting.DefineSymbolConfig)Enum.Parse(typeof(BuildSetting.DefineSymbolConfig), commandLineArgumentToValue["-defineSymbolConfig"]);
            }
            BuildProject(target, buildSetting);
        }

        [MenuItem("Cliff Lee CL/Build project Windowsx64 _F6", false, 1)]
        public static void BuildProjectWindows64()
        {
            BuildProject(BuildTarget.StandaloneWindows64, new BuildSetting());
        }

        [MenuItem("Cliff Lee CL/Build project WebGL _F7", false, 2)]
        public static void BuildProjectWebGL()
        {
            BuildProject(BuildTarget.WebGL, new BuildSetting());
        }

        /// <summary>
        /// Build Unity project.
        /// </summary>
        /// <param name="buildTarget">Target platform.</param>
        /// <param name="buildSetting">The custom build setting.</param>
        static void BuildProject(BuildTarget buildTarget, BuildSetting buildSetting)
        {
            BuildReport buildReport;
            BuildPlayerOptions buildPlayerOption = new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where((s) => s.enabled).Select((s) => s.path).ToArray(),
                locationPathName = GetBuildPath(buildTarget, buildSetting.outputPath),
                target = buildTarget,
                options = BuildOptions.None
            };

            buildReport = BuildPipeline.BuildPlayer(buildPlayerOption);
            if (buildReport.summary.result == BuildResult.Succeeded)
            {
                Debug.Log("[ProjectBuilder] Build Success: Time:" + buildReport.summary.totalTime + " Size:" + buildReport.summary.totalSize + " bytes");
                if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
                    EditorApplication.Exit(0);
            }
            else
            {
                if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
                    EditorApplication.Exit(1);
                throw new Exception("[ProjectBuilder] Build Failed: Time:" + buildReport.summary.totalTime + " Total Errors:" + buildReport.summary.totalErrors);
            }
            Debug.Log(buildSetting);
            Debug.Log("Build project at: " + buildPlayerOption.locationPathName);
        }

        static string GetBuildPath(BuildTarget buildTarget, string outputPath = "")
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string fileName = PlayerSettings.productName + GetFileExtension(buildTarget);
            string timeStamp = DateTime.Now.ToString("yyyyMMdd-HH-mm");
            string buildPath;

            outputPath = (outputPath == "") ? desktopPath : outputPath;
            buildPath = Path.Combine(outputPath, PlayerSettings.productName, $"{buildTarget}_{timeStamp}", fileName);
            buildPath = buildPath.Replace(@"\", @"\\");

            return buildPath;
        }

        /// <summary>
        /// Parse command line argument and extract custom command line arguments.
        /// </summary>
        /// -outputPath <pathName>: Set output path (directory) for executables.
        /// -defineSymbolConfig <configName>: Set config for define symbol.
        /// <returns>Dictionary about custom command line arguments.</returns>
        static Dictionary<string, string> ParseCommandLineArgument()
        {
            Dictionary<string, string> commandLineArgToValue = new Dictionary<string, string>();
            string[] customCommandLineArg = { "-outputPath", "-defineSymbolConfig"};
            string[] commandLineArg = Environment.GetCommandLineArgs();

            for (int i = 0; i < commandLineArg.Length; i++)
            {
                for (int j = 0; j < customCommandLineArg.Length; j++)
                    if (commandLineArg[i] == customCommandLineArg[j])
                        commandLineArgToValue.Add(customCommandLineArg[j], commandLineArg[(i + 1) % commandLineArg.Length]);
            }

            return commandLineArgToValue;
        }

        /// <summary>
        /// Return file extension according to build target.
        /// </summary>
        /// <param name="target">The build target.</param>
        /// <returns>file extension string.</returns>
        static string GetFileExtension(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return ".exe";
                case BuildTarget.StandaloneOSX:
                    return ".app";
                case BuildTarget.StandaloneLinux64:
                    return ".x86_64";
                case BuildTarget.WebGL:
                    return ".webgl";
                case BuildTarget.Android:
                    return EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk";
                case BuildTarget.iOS:
                    return ".iosversion";
                default:
                    Debug.LogError("No corresponding extension!");
                    return "";
            }
        }
    }

    public class BuildSetting
    {
        public enum DefineSymbolConfig
        {
            Debug,
            Release
        }

        public readonly Dictionary<DefineSymbolConfig, string> symbolConfigToDefineSymbol = new Dictionary<DefineSymbolConfig, string> { 
            [DefineSymbolConfig.Debug] = "Debug;",
            [DefineSymbolConfig.Release] = "Release;"
        };

        public string outputPath = "";
        public DefineSymbolConfig symbolConfig = DefineSymbolConfig.Debug;

        public override string ToString()
        {
            return $"{nameof(BuildSetting)}: symbolConfig={symbolConfig}, ouputPath={outputPath}";
        }
    }
}
