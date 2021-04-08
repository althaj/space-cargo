using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace PSG.SpaceCargo.Core
{
    [CreateAssetMenu(fileName = "Database", menuName = "Space Cargo/Database/Database", order = 1)]
    public class Database : ScriptableObject
    {
        public List<HexData> HexList;

        public HexData HubHex;
        public CardData NothingCard;
        public CardData CreditCard;

        /// <summary>
        /// Get a random shuffled list of hexes from this database.
        /// In case more hexes are requested that are in the database, the remaining values will be null.
        /// </summary>
        /// <param name="count">Number of hexes to get.</param>
        /// <returns></returns>
        public List<HexData> GetRandomHexes(int count)
        {
            List<HexData> result = new List<HexData>();

            if(HexList != null)
                result = HexList.Shuffle().Take(count).ToList();


            while (result.Count < count)
                result.Add(null);

            return result;
        }

        /// <summary>
        /// Get Hexes from array of hex titles.
        /// </summary>
        /// <param name="titles">Titles of the hexes to get.</param>
        /// <returns>List of Hex data.</returns>
        public List<HexData> GetHexesFromTitles(string[] titles)
        {
            List<HexData> result = new List<HexData>();

            if (HexList != null)
            {
                for (int i = 0; i < titles.Length; i++)
                {
                    if(!string.IsNullOrEmpty(titles[i]))
                        result.Add(HexList.Where(x => x.Title.CompareTo(titles[i]) == 0).FirstOrDefault());
                }
            }

            while (result.Count < titles.Length)
                result.Add(null);

            return result;
        }

        /// <summary>
        /// Get card data from title.
        /// </summary>
        /// <param name="title">Title of the card.</param>
        /// <returns>Card data of a card with the same title, or null if not found.</returns>
        public CardData GetCardData(string title)
        {
            if (title == NothingCard.Title)
                return NothingCard;

            if (title == CreditCard.Title)
                return CreditCard;

            return HexList.Where(x => x.Card.Title == title).Select(x => x.Card).FirstOrDefault();
        }
    }
}
