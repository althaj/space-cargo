using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.SpaceCargo.UI
{
    [CreateAssetMenu(fileName = "UI color", menuName = "Space Cargo/UI/Color", order = 2)]
    public class UIColor : ScriptableObject
    {
        public Color Color;
    }
}
