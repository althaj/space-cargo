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

                iconRequirementsPanel.SetActive(hexData.RequiredWorkers > 0 || hexData.RequiredCredits > 0);
                customRequirementsText.gameObject.SetActive(false);

                for (int i = 0; i < hexData.RequiredWorkers; i++)
                {
                    GameHelpers.CreateIcon(uiDatabase.WorkerSprite, iconRequirementsPanel.transform);
                }

                for (int i = 0; i < hexData.RequiredCredits; i++)
                {
                    GameHelpers.CreateIcon(uiDatabase.CreditSprite, iconRequirementsPanel.transform);
                }
            }    
        }
        #endregion
    }
}
