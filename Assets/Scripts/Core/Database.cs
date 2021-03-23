using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PSG.SpaceCargo.Core
{
    [CreateAssetMenu(fileName = "Database", menuName = "Space Cargo/Database", order = 1)]
    public class Database : ScriptableObject
    {
        public List<HexData> HexList;
    }
}
