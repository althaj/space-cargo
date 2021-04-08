using Photon.Realtime;
using PSG.SpaceCargo.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.SpaceCargo.Game
{
    public class GamePlayer
    {
        #region Public fields
        public Player Player { get; set; }
        public Deck PlayerDeck { get; set; }
        public Deck Discard { get; set; }
        public Deck Hand { get; set; }
        public PlayerPosition PlayerPosition { get; set; }
        public int PlayerID { get; private set; }
        #endregion

        #region Private fields
        private Database database;
        #endregion

        #region Constructors
        public GamePlayer(Player player, Database database, GameObject cardPrefab, int playerID, PlayerPosition position)
        {
            Player = player;
            this.database = database;
            this.PlayerID = playerID;

            PlayerPosition = position;

            PlayerDeck = Deck.CreateDeck("Player_" + playerID + "Deck", new List<GameObject>(), PlayerPosition.DeckPosition.position, true, PlayerPosition.DeckPosition.transform, database, cardPrefab, 0f, "Deck");
            Discard = Deck.CreateDeck("Player_" + playerID + "Discard", new List<GameObject>(), PlayerPosition.DiscardPosition.position, true, PlayerPosition.DiscardPosition.transform, database, cardPrefab, 0f, "Discard");
            Hand = Deck.CreateDeck("Player_" + playerID + "Hand", new List<GameObject>(), PlayerPosition.HandPosition.position, true, PlayerPosition.HandPosition.transform, database, cardPrefab, 0f, "Hand");
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Save player decks to the Room Properties.
        /// </summary>
        public void SaveDecks()
        {
            PlayerDeck.Save();
            Hand.Save();
            Discard.Save();
        }

        /// <summary>
        /// Draw cards from the deck to the hand.
        /// </summary>
        /// <param name="count">Number of cards to draw.</param>
        public void DrawCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if(PlayerDeck.Count > 0)
                {
                    Hand.AddCard(PlayerDeck.DealCard());
                }
            }
        }
        #endregion
    }
}
