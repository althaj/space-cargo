using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.SpaceCargo.UI
{
    /// <summary>
    /// Database containing all the UI related stuff.
    /// </summary>
    [CreateAssetMenu(fileName = "UI database", menuName = "Space Cargo/UI/UI Database", order = 1)]
    public class UIDatabase : ScriptableObject
    {
        /// <summary>
        /// Light UI color.
        /// </summary>
        [Tooltip("Color used for displaying light icons.")]
        public UIColor LightColor;

        /// <summary>
        /// Dark UI color.
        /// </summary>
        [Tooltip("Color used for displaying dark icons.")]
        public UIColor DarkColor;

        /// <summary>
        /// Grey UI color.
        /// </summary>
        [Tooltip("Color used for displaying grey icons.")]
        public UIColor GreyColor;

        /// <summary>
        /// Sprite used as a Credit icon.
        /// </summary>
        [Tooltip("Sprite used to display a credit.")]
        public Sprite CreditSprite;

        /// <summary>
        /// Sprite used as a spaceship icon.
        /// </summary>
        [Tooltip("Sprite used to display a spaceship.")]
        public Sprite SpaceshipSprite;

        /// <summary>
        /// Sprite used as a Draw card icon.
        /// </summary>
        [Tooltip("Sprite used to display a card.")]
        public Sprite CardSprite;

        /// <summary>
        /// Sprite used as an Action icon.
        /// </summary>
        [Tooltip("Sprite used to display action.")]
        public Sprite ActionSprite;
    }
}
