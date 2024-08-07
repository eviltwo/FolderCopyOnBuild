using System.IO;
using UnityEngine;

namespace FolderCopyOnBuild
{
    [CreateAssetMenu(fileName = "FolderCopyOnBuildMarker", menuName = "FolderCopyOnBuild/Marker")]
    public class FolderCopyOnBuildMarker : ScriptableObject
    {
        [SerializeField]
        public string[] Filters = new string[] { "*.txt" };

        [SerializeField]
        public SearchOption SearchOption = SearchOption.TopDirectoryOnly;

        [SerializeField]
        public bool DeleteOldFiles = true;
    }
}
