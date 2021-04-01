using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.SpaceCargo
{
    public static class GameHelpers
    {
        #region Extension methods
        /// <summary>
        /// Shuffle a collection randomly.
        /// </summary>
        /// <param name="original">The original collection to be shuffled.</param>
        /// <returns></returns>
        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> original) => original.OrderBy(x => Guid.NewGuid());
        #endregion

        #region UI helpers
        /// <summary>
        /// Create an icon and put it in the icons requirements panel.
        /// </summary>
        /// <param name="sprite">Sprite to use for the icon.</param>
        /// <param name="parent">Parent transform for the icon.</param>
        public static void CreateIcon(Sprite sprite, Transform parent)
        {
            GameObject imageObject = new GameObject();
            imageObject.name = sprite.name;
            imageObject.transform.SetParent(parent, false);

            imageObject.AddComponent<CanvasRenderer>();
            Image image = imageObject.AddComponent<Image>();
            image.sprite = sprite;
            image.preserveAspect = true;
        }
        #endregion
    }
}
