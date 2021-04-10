using PSG.SpaceCargo.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PSG.SpaceCargo.Core
{
    public class Card : MonoBehaviour
    {
        #region Private fields
        private CardData cardData;
        #endregion

        #region Serialized fields
        [Tooltip("Game icons to display on the card.")]
        [SerializeField]
        private UIDatabase uiDatabase;

        [Tooltip("Text field displaying the title of the card.")]
        [SerializeField]
        private TMP_Text titleText;

        [Tooltip("Panel containing iconc.")]
        [SerializeField]
        private GameObject iconsPanel;

        [Tooltip("Custom description text.")]
        [SerializeField]
        private TMP_Text customDescriptionText;

        [Tooltip("Card type text.")]
        [SerializeField]
        private TMP_Text cardTypeText;
        #endregion

        #region Public methods
        /// <summary>
        /// Initialize a new card with data and position.
        /// </summary>
        /// <param name="cardData">Data to fill the card with.</param>
        public void Initialize(CardData cardData)
        {
            this.cardData = cardData;
            name = cardData.Title;

            // Draw the data
            if (cardData != null)
            {
                titleText.text = cardData.Title;

                iconsPanel.SetActive(cardData.Actions > 0 || cardData.Cards > 0 || cardData.Credits > 0);
                customDescriptionText.gameObject.SetActive(false);

                for (int i = 0; i < cardData.Actions; i++)
                {
                    GameHelpers.CreateIcon(uiDatabase.ActionSprite, iconsPanel.transform);
                }

                for (int i = 0; i < cardData.Cards; i++)
                {
                    GameHelpers.CreateIcon(uiDatabase.CardSprite, iconsPanel.transform);
                }

                for (int i = 0; i < cardData.Credits; i++)
                {
                    GameHelpers.CreateIcon(uiDatabase.CreditSprite, iconsPanel.transform);
                }

                cardTypeText.text = cardData.CardType;
            }
        }

        /// <summary>
        /// Get the card data of the current card.
        /// </summary>
        public CardData CardData
        {
            get
            {
                return cardData;
            }
        }
        #endregion

        #region Static methods
        /// <summary>
        /// Instantiate a card object.
        /// </summary>
        /// <param name="data">Card data of the card.</param>
        /// <param name="cardPrefab">Prefab to instantiate the card from.</param>
        /// <param name="position">Position of the card.</param>
        /// <param name="rotation">Rotation of the card.</param>
        /// <returns></returns>
        public static GameObject CreateCardObject(CardData data, GameObject cardPrefab, Vector3 position, Quaternion rotation)
        {
            GameObject cardObject = Instantiate(cardPrefab, position, rotation);
            cardObject.GetComponent<Card>().Initialize(data);
            return cardObject;
        }
        #endregion
    }
}
