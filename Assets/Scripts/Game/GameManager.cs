using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using PSG.SpaceCargo.Core;
using Photon.Pun;
using Photon.Realtime;

namespace PSG.SpaceCargo.Game
{
    /// <summary>
    /// Class to manage the game, turns and rules.
    /// </summary>
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Serialized fields
        [Tooltip("Database of hexes used in the game.")]
        [SerializeField]
        private Database database;

        [Tooltip("Initial hex and card position before animation.")]
        [SerializeField]
        private Vector3 animationStartPosition;

        #region Hex fields
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

        [Tooltip("Positions of player on the table.")]
        [SerializeField]
        private PlayerPosition[] playerPositions;
        #endregion

        #endregion

        #region Private variables
        private List<HexData> usedHexes;
        private Transform[] hexPositions;
        private Transform[] deckPositions;
        private GamePlayer[] players;
        private GamePlayer currentPlayer;
        private int currentPlayerID;
        private int localPlayerID;
        #endregion

        #region MonoBehaviour callbacks
        void Start()
        {
            DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(1000, 100);

            Random.InitState((int)PhotonNetwork.CurrentRoom.CustomProperties[Constants.MATCH_SEED]);

            if (PhotonNetwork.IsMasterClient)
            {
                NetworkHelpers.SaveHexesToRoom(database.GetRandomHexes(7));
            }

            GetHexPositions();
            GetDeckPositions();
            InitializePlayers();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                players[0].DrawCards(1);
            }
        }
        #endregion

        #region Photon callbacks
        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(Constants.USED_HEXES))
                SpawnHexesAndDecks((string[])PhotonNetwork.CurrentRoom.CustomProperties[Constants.USED_HEXES]);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Spawn hexes for the current game.
        /// </summary>
        private void SpawnHexesAndDecks(string[] hexes)
        {
            usedHexes = database.GetHexesFromTitles(hexes);
            usedHexes.Insert(0, database.HubHex);
            usedHexes[0].SpaceshipSpaces = PhotonNetwork.CurrentRoom.PlayerCount;

            GameObject hexParent = new GameObject("Hexes");
            GameObject deckParent = new GameObject("Decks");

            for (int i = 0; i < hexCount; i++)
            {
                if(hexPositions.Length > i && hexPositions[i] != null)
                {
                    SpawnHex(usedHexes[i], hexPositions[i].position, i * 0.1f, hexParent.transform);

                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (usedHexes[i].Card != null && deckPositions[i] != null)
                        {
                            // Spawn cards first
                            List<GameObject> cards = new List<GameObject>();
                            for (int j = 0; j < 10; j++)
                            {
                                cards.Add(SpawnCard(usedHexes[i].Card));
                            }

                            Deck deck = Deck.CreateDeck(usedHexes[i].Card.Title, cards, deckPositions[i].position, false, deckParent.transform, database, cardPrefab, 0.3f * i);
                            deck.Save();
                        }
                    }
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
        /// Spawn a card.
        /// </summary>
        /// <param name="cardData">Data to initialize the card with.</param>
        private GameObject SpawnCard(CardData cardData)
        {
            return Card.CreateCardObject(cardData, cardPrefab, animationStartPosition, transform.rotation);
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
        /// Initialize the player order.
        /// </summary>
        private void InitializePlayers()
        {
            var networkPlayers = PhotonNetwork.CurrentRoom.Players.OrderBy(x => x.Key).Shuffle().ToList();

            for (int i = 0; i < networkPlayers.Count; i++)
            {
                if (networkPlayers[i].Value.IsLocal)
                {
                    localPlayerID = i;
                    break;
                }
            }

            players = new GamePlayer[networkPlayers.Count];
            // Asign player positions in the current user perspective.
            // Also create deck and draw hand.
            for (int i = 0; i < networkPlayers.Count; i++)
            {
                int positionIndex = (localPlayerID - i) % networkPlayers.Count;
                if (positionIndex < 0)
                    positionIndex += networkPlayers.Count;

                if(playerPositions.Length >= positionIndex - 1 && playerPositions[positionIndex] != null)
                {
                    players[i] = new GamePlayer(networkPlayers[i].Value, database, cardPrefab, networkPlayers[i].Key, playerPositions[positionIndex]);
                }

                if (PhotonNetwork.IsMasterClient)
                {
                    List<GameObject> startingCards = new List<GameObject>();
                    for (int j = 0; j < 5; j++)
                    {
                        startingCards.Add(SpawnCard(database.NothingCard));
                    }
                    for (int j = 0; j < 3; j++)
                    {
                        startingCards.Add(SpawnCard(database.CreditCard));
                    }
                    startingCards = startingCards.Shuffle().ToList();

                    players[i].PlayerDeck.AddCards(startingCards.Take(4).ToList());
                    players[i].AddCardsToHand(startingCards.Skip(4).Take(4).ToList());
                    players[i].SaveDecks();
                }
            }

            currentPlayerID = 0;
            currentPlayer = players[0];
        }
        #endregion
    }
}
