using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using PSG.SpaceCargo.UI;

namespace PSG.SpaceCargo.Core
{
    public class Hex : MonoBehaviour
    {
        #region Private fields

        private HexData hexData;
        private SpaceshipSpace[] spaceshipSpaces;

        #endregion

        #region Serialized fields
        [Tooltip("Game icons to display on the hex.")]
        [SerializeField]
        private UIDatabase uiDatabase;

        [Tooltip("Text field displaying the title of the hex.")]
        [SerializeField]
        private TMP_Text titleText;

        [Tooltip("Panel containing icon requirements.")]
        [SerializeField]
        private GameObject iconRequirementsPanel;

        [Tooltip("Custom requirements text.")]
        [SerializeField]
        private TMP_Text customRequirementsText;

        [Tooltip("Prefab of a spaceship space.")]
        [SerializeField]
        private GameObject spaceshipSpacePrefab;
        #endregion

        #region Public methods
        /// <summary>
        /// Initialize a new hex with data and position.
        /// Animation will be played to move the hex from current position.
        /// </summary>
        /// <param name="hexData">Data to fill the hex with.</param>
        /// <param name="targetPosition">Position of the hex.</param>
        /// <param name="delay">Delay before the initialization happens.</param>
        public void Initialize(HexData hexData, Vector3 targetPosition, float delay = 0f)
        {
            this.hexData = hexData;
            name = hexData.Title;

            Vector3 tempPosition = targetPosition;
            tempPosition.y = 5f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(tempPosition, 0.3f).SetDelay(delay));
            sequence.Append(transform.DOMove(targetPosition, 0.5f).SetEase(Ease.OutBounce));
            sequence.Play();

            // Draw the data
            if(hexData != null)
            {
                titleText.text = hexData.Title;

                iconRequirementsPanel.SetActive(hexData.RequiredSpaceships > 0 || hexData.RequiredCredits > 0);
                customRequirementsText.gameObject.SetActive(false);

                for (int i = 0; i < hexData.RequiredSpaceships; i++)
                {
                    GameHelpers.CreateIcon(uiDatabase.SpaceshipSprite, iconRequirementsPanel.transform);
                }

                for (int i = 0; i < hexData.RequiredCredits; i++)
                {
                    GameHelpers.CreateIcon(uiDatabase.CreditSprite, iconRequirementsPanel.transform);
                }
                
                spaceshipSpaces = SpawnSpaceshipSpaces(hexData.SpaceshipSpaces);
            }    
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Spawn spaceship spaces for the current hex.
        /// </summary>
        /// <param name="count">Number of spaceship spaces to spawn.</param>
        /// <returns>Array of all spaceship spaces.</returns>
        private SpaceshipSpace[] SpawnSpaceshipSpaces(int count)
        {
            SpaceshipSpace[] spaces = new SpaceshipSpace[count];
            GameObject parent = new GameObject("Spaceship spaces");
            parent.transform.SetParent(transform, false);

            switch (count)
            {
                case 1:
                    spaces[0] = SpawnSpaceshipSpace(new Vector3(0, 0, -0.25f), parent.transform);
                    break;
                case 2:
                    spaces[0] = SpawnSpaceshipSpace(new Vector3(-0.25f, 0, -0.25f), parent.transform);
                    spaces[1] = SpawnSpaceshipSpace(new Vector3(0.25f, 0, -0.25f), parent.transform);
                    break;
                case 3:
                    spaces[0] = SpawnSpaceshipSpace(new Vector3(-0.5f, 0, -0.25f), parent.transform);
                    spaces[1] = SpawnSpaceshipSpace(new Vector3(0f, 0, -0.25f), parent.transform);
                    spaces[1] = SpawnSpaceshipSpace(new Vector3(0.5f, 0, -0.25f), parent.transform);
                    break;
                case 4:
                    spaces[0] = SpawnSpaceshipSpace(new Vector3(-0.25f, 0, -0.15f), parent.transform);
                    spaces[1] = SpawnSpaceshipSpace(new Vector3(0.25f, 0, -0.15f), parent.transform);
                    spaces[0] = SpawnSpaceshipSpace(new Vector3(-0.25f, 0, -0.5f), parent.transform);
                    spaces[1] = SpawnSpaceshipSpace(new Vector3(0.25f, 0, -0.5f), parent.transform);
                    break;
                case 5:
                    spaces[0] = SpawnSpaceshipSpace(new Vector3(-0.5f, 0, -0.25f), parent.transform);
                    spaces[1] = SpawnSpaceshipSpace(new Vector3(0f, 0, -0.25f), parent.transform);
                    spaces[1] = SpawnSpaceshipSpace(new Vector3(0.5f, 0, -0.25f), parent.transform);
                    spaces[0] = SpawnSpaceshipSpace(new Vector3(-0.25f, 0, -0.5f), parent.transform);
                    spaces[1] = SpawnSpaceshipSpace(new Vector3(0.25f, 0, -0.5f), parent.transform);
                    break;
            }

            return spaces;
        }

        /// <summary>
        /// Spawn a spaceship space on the hex.
        /// </summary>
        /// <returns>The spawned spaceship space.</returns>
        private SpaceshipSpace SpawnSpaceshipSpace(Vector3 position, Transform parent)
        {
            GameObject spaceObject = Instantiate(spaceshipSpacePrefab, position, Quaternion.identity);
            spaceObject.transform.SetParent(parent, false);
            spaceObject.name = "Spaceship space";
            return spaceObject.GetComponent<SpaceshipSpace>();
        }
        #endregion
    }
}
