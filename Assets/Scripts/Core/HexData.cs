using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.SpaceCargo.Core
{
    [CreateAssetMenu(fileName = "Hex", menuName = "Space Cargo/Database/Hex", order = 2)]
    public class HexData : ScriptableObject
    {
        public string Title;
        public int SpaceshipSpaces;
        public int RequiredSpaceships;
        public int RequiredCredits;
        public CardData Card;
    }
}
