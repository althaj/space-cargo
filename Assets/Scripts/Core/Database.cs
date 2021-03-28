using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace PSG.SpaceCargo.Core
{
    [CreateAssetMenu(fileName = "Database", menuName = "Space Cargo/Database/Database", order = 1)]
    public class Database : ScriptableObject
    {
        public List<HexData> HexList;

        /// <summary>
        /// Get a random shuffled list of hexes from this database.
        /// In case more hexes are requested that are in the database, the remaining values will be null.
        /// </summary>
        /// <param name="count">Number of hexes to get.</param>
        /// <returns></returns>
        public List<HexData> GetRandomHexes(int count)
        {
            List<HexData> result = new List<HexData>();

            if(HexList != null)
                result = HexList.Shuffle().Take(count).ToList();


            while (result.Count < count)
                result.Add(null);

            return result;
        }
    }
}
