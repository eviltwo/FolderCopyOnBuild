using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace FolderCopyOnBuild
{
    public class FolderCopyOnBuildProcessor : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            var markerGuids = AssetDatabase.FindAssets("t:FolderCopyOnBuildMarker", new[] { "Assets" });
            var count = markerGuids.Length;
            if (count == 0)
            {
                return;
            }

            var outputDir = Directory.GetParent(report.summary.outputPath);
            var filePathBuffer = new List<string>();
            foreach (var markerGuid in markerGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(markerGuid);
                var marker = AssetDatabase.LoadAssetAtPath<FolderCopyOnBuildMarker>(path);

                if (marker.DeleteOldFiles)
                {
                    var markerRelativeDirPath = Path.GetRelativePath("Assets", Directory.GetParent(path).FullName);
                    var markerTargetDir = Path.Combine(outputDir.FullName, markerRelativeDirPath);
                    if (Directory.Exists(markerTargetDir))
                    {
                        Directory.Delete(markerTargetDir, true);
                    }
                }

                var markerDirPath = GetDirectoryFullPath(marker);
                var markerParentDirPath = Directory.GetParent(markerDirPath).FullName;
                filePathBuffer.Clear();
                FindFiles(marker, filePathBuffer);
                foreach (var filePath in filePathBuffer)
                {
                    var relativePath = Path.GetRelativePath(markerParentDirPath, filePath);
                    var targetPath = Path.Combine(outputDir.FullName, relativePath);
                    var targetDir = Directory.GetParent(targetPath);
                    if (!Directory.Exists(targetDir.FullName))
                    {
                        Directory.CreateDirectory(targetDir.FullName);
                    }
                    File.Copy(filePath, targetPath, true);
                }
                Debug.Log($"Copied {filePathBuffer.Count} files from {markerDirPath} to {outputDir}");
            }
        }

        public static string GetDirectoryFullPath(Object asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            return Path.Combine(Directory.GetParent(Application.dataPath).FullName, Path.GetDirectoryName(path));
        }

        public static void FindFiles(FolderCopyOnBuildMarker marker, List<string> result)
        {
            var markerDirPath = GetDirectoryFullPath(marker);
            result.Clear();
            foreach (var filter in marker.Filters)
            {
                result.AddRange(Directory.GetFiles(markerDirPath, filter, marker.SearchOption));
            }
        }
    }
}
