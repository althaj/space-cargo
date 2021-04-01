using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

        public string CardType
        {
            get
            {
                List<string> types = new List<string>();
                if (Credits > 0)
                    types.Add("Credits");
                if (Actions > 0 || Cards > 0)
                    types.Add("Action");

                return types.Count > 0 ? string.Join(" | ", types) : "Nothing";
            }
        }
        // public CardAction[] ExtraActions;
    }
}
