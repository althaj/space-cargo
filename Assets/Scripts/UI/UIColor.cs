using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.SpaceCargo.UI
{
    /// <summary>
    /// UI color that can be created as an asset.
    /// </summary>
    [CreateAssetMenu(fileName = "UI color", menuName = "Space Cargo/UI/Color", order = 2)]
    public class UIColor : ScriptableObject
    {
        /// <summary>
        /// The color.
        /// </summary>
        public Color Color;
    }
}
