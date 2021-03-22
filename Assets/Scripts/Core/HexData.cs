using System.Collections;
using System.Collections.Generic;
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
    }
}
