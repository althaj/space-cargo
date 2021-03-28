using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using PSG.SpaceCargo.Core;

namespace PSG.SpaceCargo.Game
{
    public class GameManager : MonoBehaviour
    {
        #region Serialized fields
        [Tooltip("Database of hexes used in the game.")]
        [SerializeField]
        private Database database;

        [Tooltip("Hex for the hub (recruiting workers).")]
        [SerializeField]
        private HexData hubHex;

        [Tooltip("Prefab of a hex piece for spawning.")]
        [SerializeField]
        private GameObject hexPrefab;

        [Tooltip("Number of hexes to spawn.")]
        [SerializeField]
        [Range(1, 20)]
        private int hexCount;

        [Tooltip("Initial hex position before animation.")]
        [SerializeField]
        private Vector3 hexStartPosition;
        #endregion

        #region Private variables
        private List<HexData> usedHexes;
        private Transform[] hexPositions;
        #endregion

        #region MonoBehaviour callbacks
        void Start()
        {
            DOTween.Init(true, true, LogBehaviour.Verbose);

            GetHexPositions();

            usedHexes = database.GetRandomHexes(hexCount - 1);
            usedHexes.Insert(0, hubHex);

            SpawnHexes();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Spawn hexes for the current game.
        /// </summary>
        public void SpawnHexes()
        {
            for (int i = 0; i < hexCount; i++)
            {
                if(hexPositions.Length > i && hexPositions[i] != null)
                {
                    SpawnHex(usedHexes[i], hexPositions[i].position, i * 0.1f);
                }
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Spawn a hex at a position.
        /// </summary>
        /// <param name="hexData">Hex data used with the hex.</param>
        /// <param name="position">Target position of the hex.</param>
        /// <param name="delay">Delay before playing the spawn animation.</param>
        private void SpawnHex(HexData hexData, Vector3 position, float delay = 0)
        {
            Instantiate(hexPrefab, hexStartPosition, Quaternion.identity).GetComponent<Hex>().Initialize(hexData, position, delay);
        }

        /// <summary>
        /// Get the spawn positions for hexes.
        /// </summary>
        private void GetHexPositions()
        {
            GameObject hexSpawn = GameObject.FindGameObjectWithTag("Hex Positions");
            if (hexSpawn != null)
            {
                hexPositions = new Transform[hexSpawn.transform.childCount];
                for (int i = 0; i < hexPositions.Length; i++)
                {
                    hexPositions[i] = hexSpawn.transform.GetChild(i);
                }
            }

            if (hexPositions != null && hexPositions.Length > 0)
                hexPositions.OrderBy(x => Vector3.Distance(x.position, hexPositions[0].position));
        }
        #endregion
    }
}
