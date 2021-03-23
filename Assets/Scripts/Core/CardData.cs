using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.SpaceCargo.Core
{
    [CreateAssetMenu(fileName = "Hex", menuName = "Space Cargo/Hex", order = 2)]
    public class CardData : ScriptableObject
    {
        public string Title;
        public Sprite Image;
        public int Credits;
        public int Actions;
        public int Cards;
        // public CardAction[] ExtraActions;
    }
}
