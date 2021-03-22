using UnityEngine;
using UnityEditor;
using System.IO;

namespace PSG.SpaceCargo.Core
{
    public static class AssetHelpers
    {
        /// <summary>
        /// Create a folder recursively.
        /// </summary>
        /// <param name="path">Path to the folder to create.</param>
        public static void CreateFolder(string path)
        {
            path.Replace("\\", "/");

            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Rename an asset, with a unique name.
        /// </summary>
        /// <param name="path">Path to the asset.</param>
        /// <param name="name">New name to use.</param>
        public static void RenameAsset(string path, string name)
        {
            string newPath = $"{path.Substring(0, path.LastIndexOf("/"))}/{name}.asset";
            string newName = AssetDatabase.GenerateUniqueAssetPath(newPath).Substring(path.LastIndexOf("/") + 1);

            AssetDatabase.RenameAsset(path, newName);
            AssetDatabase.SaveAssets();
        }
    }
}