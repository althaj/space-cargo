using DG.Tweening;
using Photon.Pun;
using PSG.SpaceCargo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PSG.SpaceCargo.Core
{
    public class Deck : MonoBehaviourPunCallbacks
    {
        #region Private fields
        private List<GameObject> cards;
        private bool isFaceDown;
        private float cardOffeset = 0.015f;
        private Database database;
        private string id;
        private GameObject cardPrefab;
        #endregion

        #region Public methods
        /// <summary>
        /// Number of cards in the deck.
        /// </summary>
        public int Count { get { return cards == null ? 0 : cards.Count; } }

        /// <summary>
        /// Initialize a deck with list of cards.
        /// </summary>
        /// <param name="cards">List of cards to initialize the deck from.</param>
        /// <returns>The deck object, so you can chain the methods.</returns>
        public Deck InitializeDeck(string id, List<GameObject> cards, bool isFaceDown, Database database, GameObject cardPrefab, float delay)
        {
            this.id = id;
            this.cards = new List<GameObject>(cards);
            this.isFaceDown = isFaceDown;
            this.database = database;
            this.cardPrefab = cardPrefab;

            foreach (GameObject card in cards)
            {
                card.transform.SetParent(transform, false);
            }

            MoveCardsToDeck(delay);

            return this;
        }

        /// <summary>
        /// Shuffles the deck.
        /// </summary>
        /// <returns>The deck object, so you can chain the methods.</returns>
        public Deck Shuffle()
        {
            cards = cards.Shuffle().ToList();

            return this;
        }

        /// <summary>
        /// Add a card to the deck.
        /// </summary>
        /// <param name="card"></param>
        /// <returns>The deck object, so you can chain the methods.</returns>
        public Deck AddCard(GameObject card)
        {
            if (this.cards == null)
                cards = new List<GameObject>();

            cards.Add(card);

            card.transform.SetParent(transform, true);

            MoveCardsToDeck(new List<GameObject> { card }, 0);

            return this;
        }

        /// <summary>
        /// Add multiple cards to the deck.
        /// </summary>
        /// <param name="card">List of cards to add to the deck.</param>
        /// <returns>The deck object, so you can chain the methods.</returns>
        public Deck AddCards(List<GameObject> cards)
        {
            if (this.cards == null)
                return InitializeDeck(id, cards, isFaceDown, database, cardPrefab, 0);

            this.cards.AddRange(cards);

            foreach (GameObject card in cards)
            {
                card.transform.SetParent(transform, false);
            }

            MoveCardsToDeck(cards, 0);

            return this;
        }

        /// <summary>
        /// Deal the top card from the deck.
        /// </summary>
        /// <returns>Dealt card.
        /// Null in case of empty / non existent list of cards, or if the object has no Card component.</returns>
        public GameObject DealCard()
        {
            if (cards == null)
            {
                Debug.LogError("Trying to deal a card while the cards are not initialized.", this);
                return null;
            }

            if (cards.Count == 0)
            {
                Debug.LogWarning("Trying to deal a card while the deck is empty.", this);
                return null;
            }

            GameObject cardObject = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);

            Debug.Log("Dealing card", cardObject);

            return cardObject;
        }

        /// <summary>
        /// Save the deck as array of titles.
        /// </summary>
        /// <returns>Array of titles of all cards in the deck.</returns>
        public void Save()
        {
            string[] result = new string[cards.Count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = cards[i].name;
            }

            NetworkHelpers.SetRoomProperty(string.Format(Constants.PLAYER_DECK, id), result);
        }

        /// <summary>
        /// Load deck from titles.
        /// </summary>
        /// <param name="titles">Titles of the cards</param>
        /// <param name="cardPrefab">Prefab of the card to use when no card with the title is present.</param>
        public void LoadDeck(string[] titles)
        {
            List<GameObject> newDeck = new List<GameObject>();

            foreach (string title in titles)
            {
                GameObject card = cards.Where(x => x.name == title).FirstOrDefault();

                if (card != null)
                {
                    int index = cards.IndexOf(card);
                    newDeck.Add(card);
                    cards.RemoveAt(index);
                }
                else
                {
                    CardData data = database.GetCardData(title);

                    Card.CreateCardObject(data, cardPrefab, transform.position, transform.rotation);
                }
            }

            foreach(GameObject remainingCard in cards)
            {
                Destroy(remainingCard);
            }

            cards = newDeck;
        }
        #endregion

        #region Private methods
        private void MoveCard(GameObject card, int indexInDeck, int indexInSequence, float delay)
        {
            Vector3 tempPosition = transform.position;
            tempPosition.y = 5f;

            card.transform.DORotate(isFaceDown ? transform.eulerAngles + Vector3.forward * 180 : transform.eulerAngles, 0.1f + delay);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(card.transform.DOMove(tempPosition, 0.1f).SetDelay(0.05f * indexInSequence + delay));
            sequence.Append(card.transform.DOMove(transform.position + Vector3.up * cardOffeset * (indexInDeck + 1), 0.3f).SetEase(Ease.OutBounce));
            sequence.Play();
        }

        /// <summary>
        /// Move card objects to the deck position.
        /// </summary>
        /// <param name="cardsToMove">Which cards to move.</param>
        private void MoveCardsToDeck(List<GameObject> cardsToMove, float delay)
        {
            for (int i = 0; i < cardsToMove.Count; i++)
            {
                int index = cards.IndexOf(cardsToMove[i]);

                MoveCard(cardsToMove[i], index, i, delay);
            }
        }

        /// <summary>
        /// Move card objects to the deck position.
        /// </summary>
        private void MoveCardsToDeck(float delay)
        {
            MoveCardsToDeck(cards, delay);
        }
        #endregion

        #region Static methods
        /// <summary>
        /// Create a deck of cards.
        /// </summary>
        /// <param name="cards">Cards to put into the deck.</param>
        /// <param name="position">Position of the deck in the world.</param>
        /// <param name="isFaceDown">Is the deck face down?</param>
        /// <returns></returns>
        public static Deck CreateDeck(string id, List<GameObject> cards, Vector3 position, bool isFaceDown, Transform parent, Database database, GameObject cardPrefab, float delay = 0f, string deckName = null)
        {
            if (string.IsNullOrEmpty(deckName))
            {
                if (cards.Count == 0)
                    deckName = "Deck";
                else
                    deckName = cards[0].name + " deck";
            }

            GameObject deckObject = new GameObject(deckName);
            deckObject.transform.position = position;
            deckObject.transform.rotation = parent.rotation;

            if (parent != null)
                deckObject.transform.SetParent(parent, true);

            Deck deck = deckObject.AddComponent<Deck>();
            return deck.InitializeDeck(id, cards, isFaceDown, database, cardPrefab, delay);
        }
        #endregion

        #region Photon callbacks
        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(string.Format(Constants.PLAYER_DECK, id)))
            {
                LoadDeck((string[])PhotonNetwork.CurrentRoom.CustomProperties[string.Format(Constants.PLAYER_DECK, id)]);
            }
        }
        #endregion
    }
}
