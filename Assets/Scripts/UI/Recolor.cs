using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.SpaceCargo.UI
{
    /// <summary>
    /// Component that recolors UI on start.
    /// </summary>
    public class Recolor : MonoBehaviour
    {
        /// <summary>
        /// Color used to recolor the elements.
        /// </summary>
        [Tooltip("Color used to recolor the elements.")]
        [SerializeField]
        private UIColor color;

        /// <summary>
        /// Should we recolor children as well?
        /// </summary>
        [Tooltip("Should we recolor children as well?")]
        [SerializeField]
        private bool recolorChildren;

        /// <summary>
        /// Should we recolor texts and TMPro texts?
        /// </summary>
        [Tooltip("Should we recolor texts and TMPro texts?")]
        [SerializeField]
        private bool recolorText;

        /// <summary>
        /// Should we recolor images?"
        /// </summary>
        [Tooltip("Should we recolor images?")]
        [SerializeField]
        private bool recolorImages;

        void Start()
        {
            Text[] texts = new Text[0];
            TMP_Text[] tmpTexts = new TMP_Text[0];
            Image[] images = new Image[0];

            if (recolorChildren)
            {
                if (recolorText)
                {
                    texts = GetComponentsInChildren<Text>();
                    tmpTexts = GetComponentsInChildren<TMP_Text>();
                }

                if (recolorImages)
                {
                    images = GetComponentsInChildren<Image>();
                }
            }
            else
            {
                if (recolorText)
                {
                    texts = GetComponents<Text>();
                    tmpTexts = GetComponents<TMP_Text>();
                }

                if (recolorImages)
                {
                    images = GetComponents<Image>();
                }
            }

            foreach (Text text in texts)
            {
                text.color = color.Color;
            }

            foreach (TMP_Text tmpText in tmpTexts)
            {
                tmpText.color = color.Color;
            }

            foreach (Image image in images)
            {
                image.color = color.Color;
            }
        }

    }
}
