using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.SpaceCargo.UI
{
    [CreateAssetMenu(fileName = "UI database", menuName = "Space Cargo/UI/Database", order = 1)]
    public class UIDatabase : ScriptableObject
    {

        [Tooltip("Color used for displaying light icons.")]
        public UIColor LightColor;

        [Tooltip("Color used for displaying dark icons.")]
        public UIColor DarkColor;

        [Tooltip("Color used for displaying grey icons.")]
        public UIColor GreyColor;

        [Tooltip("Sprite used to display a credit.")]
        public Sprite CreditSprite;

        [Tooltip("Sprite used to display a worker.")]
        public Sprite WorkerSprite;

        [Tooltip("Sprite used to display a card.")]
        public Sprite CardSprite;

        [Tooltip("Sprite used to display action.")]
        public Sprite ActionSprite;
    }
}
