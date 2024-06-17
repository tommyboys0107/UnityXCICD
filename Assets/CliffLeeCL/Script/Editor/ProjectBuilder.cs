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
            var target = EditorUserBuildSettings.activeBuildTarget;
            var buildSetting = new BuildSetting("", BuildSetting.DefineSymbolConfig.Debug);

            // Handle command line arguments.
            if (UnityEditorInternal.InternalEditorUtility.inBatchMode)
            {
                Dictionary<string, string> commandLineArgumentToValue = ParseCommandLineArgument();

                if (commandLineArgumentToValue.TryGetValue("-outputPath", out var outputPath))
                    buildSetting.outputPath = outputPath;
                if (commandLineArgumentToValue.TryGetValue("-defineSymbolConfig", out var defineSymbolConfig))
                    buildSetting.symbolConfig = (BuildSetting.DefineSymbolConfig)Enum.Parse(typeof(BuildSetting.DefineSymbolConfig), defineSymbolConfig);
            }
            BuildProject(target, buildSetting);
        }

        [MenuItem("Cliff Lee CL/Build project Windows 64-bit _F6", false, 1)]
        public static void BuildProjectWindows64()
        {
            BuildProject(BuildTarget.StandaloneWindows64, new BuildSetting("", BuildSetting.DefineSymbolConfig.Debug));
        }

        [MenuItem("Cliff Lee CL/Build project WebGL _F7", false, 2)]
        public static void BuildProjectWebGL()
        {
            BuildProject(BuildTarget.WebGL, new BuildSetting("", BuildSetting.DefineSymbolConfig.Debug));
        }

        /// <summary>
        /// Build Unity project.
        /// </summary>
        /// <param name="buildTarget">Target platform.</param>
        /// <param name="buildSetting">The custom build setting.</param>
        private static void BuildProject(BuildTarget buildTarget, BuildSetting buildSetting)
        {
            var defineSymbolBeforeBuild = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildPipeline.GetBuildTargetGroup(buildTarget));
            var buildPlayerOption = new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where((s) => s.enabled).Select((s) => s.path).ToArray(),
                locationPathName = GetBuildPath(buildTarget, buildSetting.outputPath),
                target = buildTarget,
                options = BuildOptions.None,
            };
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                BuildPipeline.GetBuildTargetGroup(buildTarget),
                BuildSetting.ConfigToDefineSymbolDict[buildSetting.symbolConfig]
                );

            var buildReport = BuildPipeline.BuildPlayer(buildPlayerOption);
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
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                BuildPipeline.GetBuildTargetGroup(buildTarget), defineSymbolBeforeBuild);
            AssetDatabase.SaveAssets();
            Debug.Log(buildSetting);
            Debug.Log("Build project at: " + buildPlayerOption.locationPathName);
        }

        private static string GetBuildPath(BuildTarget buildTarget, string outputPath = "")
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var fileExtension = GetFileExtension(buildTarget);
            string buildPath;

            outputPath = (outputPath == "") ? desktopPath : outputPath;
            buildPath = Path.Combine(outputPath, $"{PlayerSettings.productName}{fileExtension}");
            buildPath = buildPath.Replace(@"\", @"\\");

            return buildPath;
        }

        /// <summary>
        /// Parse command line argument and extract custom command line arguments.
        /// </summary>
        /// -outputPath: Set output path (directory) for executables.
        /// -defineSymbolConfig: Set config for define symbol.
        /// <returns>Dictionary about custom command line arguments.</returns>
        private static Dictionary<string, string> ParseCommandLineArgument()
        {
            var commandLineArgToValue = new Dictionary<string, string>();
            var commandLineArgArray = Environment.GetCommandLineArgs();
            var customCommandLineArgArray = new []{ "-outputPath", "-defineSymbolConfig"};

            for (int i = 0; i < commandLineArgArray.Length; i++)
            {
                foreach (var arg in customCommandLineArgArray)
                    if (commandLineArgArray[i] == arg)
                        commandLineArgToValue.Add(arg, commandLineArgArray[(i + 1) % commandLineArgArray.Length]);
            }

            return commandLineArgToValue;
        }
        
        /// <summary>
        /// Return file extension according to build target.
        /// </summary>
        /// <param name="target">The build target.</param>
        /// <returns>file extension string.</returns>
        private static string GetFileExtension(BuildTarget target)
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
                case BuildTarget.Android:
                    return EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk";
                default:
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

        public static readonly Dictionary<DefineSymbolConfig, string> ConfigToDefineSymbolDict = new()
        { 
            [DefineSymbolConfig.Debug] = "Debug;",
            [DefineSymbolConfig.Release] = "Release;"
        };

        public string outputPath;
        public DefineSymbolConfig symbolConfig;

        public BuildSetting(string outputPath, DefineSymbolConfig symbolConfig)
        {
            this.outputPath = outputPath;
            this.symbolConfig = symbolConfig;
        }

        public override string ToString()
        {
            return $"{nameof(BuildSetting)}: symbolConfig={symbolConfig}, outputPath={outputPath}";
        }
    }
}
