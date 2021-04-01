using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using PSG.SpaceCargo.Core;
using System;

namespace PSG.SpaceCargo.Game
{
    public class GameManager : MonoBehaviour
    {
        #region Serialized fields
        [Tooltip("Database of hexes used in the game.")]
        [SerializeField]
        private Database database;

        [Tooltip("Initial hex and card position before animation.")]
        [SerializeField]
        private Vector3 animationStartPosition;

        #region Hex fields
        [Tooltip("Hex for the hub (recruiting workers).")]
        [SerializeField]
        private HexData hubHex;

        [Tooltip("Prefab of a hex piece for spawning.")]
        [SerializeField]
        private GameObject hexPrefab;

        [Tooltip("Transform containing all hex positions as children.")]
        [SerializeField]
        private Transform hexPositionsParent;

        [Tooltip("Number of hexes to spawn.")]
        [SerializeField]
        [Range(1, 20)]
        private int hexCount;
        #endregion

        #region Card and deck fields
        [Tooltip("Transform containing all deck positions as children.")]
        [SerializeField]
        private Transform deckPositionsParent;

        [Tooltip("Prefab of a card for spawning.")]
        [SerializeField]
        private GameObject cardPrefab;
        #endregion

        #endregion

        #region Private variables
        private List<HexData> usedHexes;
        private Transform[] hexPositions;
        private Transform[] deckPositions;
        #endregion

        #region MonoBehaviour callbacks
        void Start()
        {
            DOTween.Init(true, true, LogBehaviour.Verbose);

            GetHexPositions();
            GetDeckPositions();

            usedHexes = database.GetRandomHexes(hexCount - 1);
            usedHexes.Insert(0, hubHex);

            SpawnHexes();
            SpawnDecks();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Spawn hexes for the current game.
        /// </summary>
        private void SpawnHexes()
        {
            GameObject parentObject = new GameObject("Hexes");
            for (int i = 0; i < hexCount; i++)
            {
                if(hexPositions.Length > i && hexPositions[i] != null)
                {
                    SpawnHex(usedHexes[i], hexPositions[i].position, i * 0.1f, parentObject.transform);
                }
            }
        }

        /// <summary>
        /// Spawn a hex at a position.
        /// </summary>
        /// <param name="hexData">Hex data used with the hex.</param>
        /// <param name="position">Target position of the hex.</param>
        /// <param name="delay">Delay before playing the spawn animation.</param>
        private void SpawnHex(HexData hexData, Vector3 position, float delay, Transform parent)
        {
            GameObject hex = Instantiate(hexPrefab, animationStartPosition, Quaternion.identity);
            hex.transform.SetParent(parent, false);
            hex.GetComponent<Hex>().Initialize(hexData, position, delay);
        }

        /// <summary>
        /// Get the spawn positions for hexes.
        /// </summary>
        private void GetHexPositions()
        {
            if (hexPositionsParent != null)
            {
                hexPositions = new Transform[hexPositionsParent.transform.childCount];
                for (int i = 0; i < hexPositions.Length; i++)
                {
                    hexPositions[i] = hexPositionsParent.transform.GetChild(i);
                }
            }

            if (hexPositions != null && hexPositions.Length > 0)
                hexPositions.OrderBy(x => Vector3.Distance(x.position, hexPositions[0].position));
        }

        /// <summary>
        /// Get the spawn positions for decks.
        /// </summary>
        private void GetDeckPositions()
        {
            if (deckPositionsParent != null)
            {
                deckPositions = new Transform[deckPositionsParent.transform.childCount];
                for (int i = 0; i < deckPositions.Length; i++)
                {
                    deckPositions[i] = deckPositionsParent.transform.GetChild(i);
                }
            }

            if (deckPositions != null && deckPositions.Length > 0)
                deckPositions.OrderBy(x => Vector3.Distance(x.position, deckPositions[0].position));
        }

        /// <summary>
        /// Spawn the deck at their spawn positions;
        /// </summary>
        private void SpawnDecks()
        {
            GameObject deckParent = new GameObject("Decks");

            for (int i = 0; i < hexCount; i++)
            {
                if (deckPositions.Length > i && deckPositions[i] != null)
                {
                    if (usedHexes[i].Card != null)
                    {
                        // Spawn cards first
                        List<GameObject> cards = new List<GameObject>();
                        for (int j = 0; j < 10; j++)
                        {
                            cards.Add(SpawnCard(usedHexes[i].Card));
                        }

                        Deck.CreateDeck(cards, deckPositions[i].position, false, deckParent.transform, 0.3f * i);
                    }
                }
            }
        }

        /// <summary>
        /// Spawn a card.
        /// </summary>
        /// <param name="cardData">Data to initialize the card with.</param>
        private GameObject SpawnCard(CardData cardData)
        {
            GameObject card = Instantiate(cardPrefab, animationStartPosition, Quaternion.identity);
            card.GetComponent<Card>().Initialize(cardData);
            return card;
        }
        #endregion
    }
}
