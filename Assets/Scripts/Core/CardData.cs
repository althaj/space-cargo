using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.SpaceCargo.Core
{
    [CreateAssetMenu(fileName = "Card", menuName = "Space Cargo/Database/Card", order = 3)]
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
