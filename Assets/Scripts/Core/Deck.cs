using DG.Tweening;
using PSG.SpaceCargo;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PSG.SpaceCargo.Core
{
    public class Deck : MonoBehaviour
    {
        #region Private fields
        private List<GameObject> cards;
        private bool isFaceDown;
        private float cardOffeset = 0.015f;
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
        public Deck InitializeDeck(List<GameObject> cards, bool isFaceDown, float delay)
        {
            this.cards = new List<GameObject>(cards);
            this.isFaceDown = isFaceDown;

            foreach (GameObject card in cards)
            {
                card.transform.SetParent(transform, true);
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
            if (cards == null)
                cards = new List<GameObject>();

            cards.Add(card);

            return this;
        }

        /// <summary>
        /// Add multiple cards to the deck.
        /// </summary>
        /// <param name="card">List of cards to add to the deck.</param>
        /// <returns>The deck object, so you can chain the methods.</returns>
        public Deck AddCards(List<GameObject> cards)
        {
            if (cards == null)
                return InitializeDeck(cards, isFaceDown, 0);

            this.cards.AddRange(cards);

            return this;
        }

        /// <summary>
        /// Deal the top card from the deck.
        /// </summary>
        /// <returns>Dealt card.
        /// Null in case of empty / non existent list of cards, or if the object has no Card component.</returns>
        public Card DealCard()
        {
            if(cards == null)
            {
                Debug.LogError("Trying to deal a card while the cards are not initialized.", this);
                return null;
            }

            if(cards.Count == 0)
            {
                Debug.LogWarning("Trying to deal a card while the deck is empty.", this);
                return null;
            }

            GameObject cardObject = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);

            Card card = cardObject.GetComponent<Card>();
            if(card == null)
                Debug.LogError("The card dealt from the deck has no Card component!", card);

            return card;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Move card objects to the deck position.
        /// </summary>
        private void MoveCardsToDeck(float delay)
        {
            Vector3 tempPosition = transform.position;
            tempPosition.y = 5f;

            for (int i = 0; i < Count; i++)
            {
                cards[i].transform.DORotate(isFaceDown ? transform.eulerAngles + Vector3.forward * 180 : transform.eulerAngles, 0.1f + delay);

                Sequence sequence = DOTween.Sequence();
                sequence.Append(cards[i].transform.DOMove(tempPosition, 0.1f).SetDelay(0.05f * i + delay));
                sequence.Append(cards[i].transform.DOMove(transform.position + Vector3.up * cardOffeset * (i + 1), 0.3f).SetEase(Ease.OutBounce));
                sequence.Play();
            }
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
        public static Deck CreateDeck(List<GameObject> cards, Vector3 position, bool isFaceDown, Transform parent, float delay = 0f)
        {
            GameObject deckObject = new GameObject(cards[0].name + " deck");
            deckObject.transform.position = position;

            if (parent != null)
                deckObject.transform.SetParent(parent, false);

            Deck deck = deckObject.AddComponent<Deck>();
            return deck.InitializeDeck(cards, isFaceDown, delay);
        }
        #endregion
    }
}
