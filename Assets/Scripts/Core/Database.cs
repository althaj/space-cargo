using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PSG.SpaceCargo.Core
{
    public class Database : ScriptableObject
    {
        public List<HexData> HexList;

        [MenuItem("Assets/Create/Inventory Item List")]
        public static Database CreateDatabase()
        {
            Database asset = CreateInstance<Database>();

            AssetHelpers.CreateFolder(Constants.DATABASE_PATH);

            asset.HexList = new List<HexData>();

            AssetDatabase.CreateAsset(asset, Constants.DATABASE_PATH + "/Database.asset");
            AssetDatabase.SaveAssets();

            string databasePath = AssetDatabase.GetAssetPath(asset);
            EditorPrefs.SetString(Constants.DATABASE_PATH_KEY, databasePath);

            return asset;
        }
    }
}
