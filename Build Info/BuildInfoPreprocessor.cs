using System;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Somnambulo.Scripts.Runtime.Infrastructure.Debugging;
using Debug = UnityEngine.Debug;

namespace Somnambulo.Scripts.Editor.Build
{
    // Automatically updates the BuildDate in the config asset before building.
    public class BuildInfoPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            var guids = AssetDatabase.FindAssets("t:BuildInfoConfig");
            
            if (guids.Length == 0)
            {
                Debug.LogError($"[{nameof(BuildInfoPreprocessor)}] - No build info config found!");
                return;
            }
            
            if (guids.Length > 1)
            {
                Debug.LogError($"[{nameof(BuildInfoPreprocessor)}] - Multiple build info configs found! Using first one.");
            }

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var config = AssetDatabase.LoadAssetAtPath<BuildInfoConfig>(path);

            if (config == null)
            {
                Debug.LogError($"[{nameof(BuildInfoPreprocessor)}] - Failed to load build info config!");
                return;
            }
            
            config.BuildDate = DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm 'UTC'");
            config.BundleVersionCode = PlayerSettings.Android.bundleVersionCode;
            config.CommitHash = GetGitHash();
            
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
            Debug.Log($"[[{nameof(BuildInfoPreprocessor)}] - Updated BuildInfoConfig - BuildDate: {config.BuildDate} | BundleVersionCode: {config.BundleVersionCode}");
        }

        private static string GetGitHash()
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "git",
                        // "rev-parse --short HEAD" returns short hash (7 symbols). 
                        // remove "--short", to get full hash
                        Arguments = "rev-parse --short HEAD", 
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        // Application.dataPath is Assets folder, we need project root folder
                        WorkingDirectory = System.IO.Directory.GetParent(Application.dataPath)?.FullName
                    }
                };

                process.Start();
                
                var output = process.StandardOutput.ReadToEnd().Trim();
                var error = process.StandardError.ReadToEnd().Trim();
                
                process.WaitForExit();

                if (process.ExitCode != 0 || string.IsNullOrEmpty(output))
                {
                    Debug.LogWarning($"[{nameof(BuildInfoPreprocessor)}] - Git Error: {error}");
                    return "GIT_ERR";
                }

                return output;
            }
            catch (Exception e)
            {
                // no git found or other system error
                Debug.LogWarning($"[{nameof(BuildInfoPreprocessor)}] - Failed to get git hash: {e.Message}");
                return "NO_GIT";
            }
        }
    }
}