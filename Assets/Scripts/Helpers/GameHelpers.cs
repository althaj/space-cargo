using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    }
}
