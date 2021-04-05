using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PSG.SpaceCargo
{
    public static class GameHelpers
    {
        #region Game helpers
        /// <summary>
        /// Shuffle a collection randomly.
        /// </summary>
        /// <param name="original">The original collection to be shuffled.</param>
        /// <returns>Shuffled collection.</returns>
        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> original) => original.OrderBy(x => Random.Range(0f, 1f));

        /// <summary>
        /// Shuffle a collection according to indexes.
        /// This is to do persistent shuffling across all players.
        /// </summary>
        /// <param name="original">The original collection to be shuffled.</param>
        /// <param name="indexes">Indexes of the result.</param>
        /// <returns>Shuffled collection.</returns>
        public static IOrderedEnumerable<T> ShuffleByIndex<T>(this IEnumerable<T> original, int[] indexes)
        {
            List<T> temp = original.ToList();
            return original.OrderBy(x => indexes[temp.IndexOf(x)]);
        }

        /// <summary>
        /// Shuffles indexes.
        /// This is to do persistent shuffling across all players.
        /// Feed the result shuffled indexes to Shuffle extension.
        /// </summary>
        /// <see cref="ShuffleByIndex" />
        /// <param name="count">Number of items in the shuffled result set.</param>
        /// <returns>Shuffled indexes.</returns>
        public static IOrderedEnumerable<int> ShuffleIndexes(int count)
        {
            IEnumerable<int> result = Enumerable.Range(0, count);
            return result.Shuffle();
        }
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
