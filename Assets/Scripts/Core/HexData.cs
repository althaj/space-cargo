using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PSG.SpaceCargo.Core
{
    [CreateAssetMenu(fileName = "Hex", menuName ="Space Cargo/Hex", order = 1)]
    public class HexData : ScriptableObject
    {
        public string Title;
        public int WorkerSpaces;
        public int RequiredWorkers;
        public int RequiredCredits;
        // public CardData Card;

        /// <summary>
        /// Create a new empty hex asset.
        /// </summary>
        /// <returns></returns>
        public static HexData CreateHex()
        {
            HexData newHex = CreateInstance<HexData>();
            newHex.Title = "New hex";

            AssetHelpers.CreateFolder(Constants.HEX_PATH);
            AssetDatabase.CreateAsset(newHex, AssetDatabase.GenerateUniqueAssetPath(Constants.HEX_PATH + "/New hex.asset"));
            AssetDatabase.SaveAssets();

            return newHex;
        }

        /// <summary>
        /// Delete this hex.
        /// </summary>
        public void DeleteHex()
        {
            string path = AssetDatabase.GetAssetPath(this);
            AssetDatabase.DeleteAsset(path);
        }
    }
}
