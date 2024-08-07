using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace FolderCopyOnBuild
{
    [CustomEditor(typeof(FolderCopyOnBuildMarker))]
    public class FolderCopyOnBuildMarkerEditor : Editor
    {
        private bool _foldout = true;
        private List<string> _pathBuffer = new List<string>();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            var marker = target as FolderCopyOnBuildMarker;

            _foldout = EditorGUILayout.BeginFoldoutHeaderGroup(_foldout, "Detected files");
            if (_foldout)
            {
                FolderCopyOnBuildProcessor.FindFiles(marker, _pathBuffer);
                var markerDirPath = FolderCopyOnBuildProcessor.GetDirectoryFullPath(marker);
                foreach (var path in _pathBuffer)
                {
                    EditorGUILayout.LabelField(Path.GetRelativePath(markerDirPath, path));
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}
