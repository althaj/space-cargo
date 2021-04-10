using PSG.SpaceCargo.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.SpaceCargo.UI
{
    public class GameWindow : MonoBehaviour
    {
        #region Serialized fields
        [SerializeField]
        [Tooltip("Prefab of a card in the UI")]
        private GameObject cardPrefab;

        [SerializeField]
        [Tooltip("Player hand panel.")]
        private Transform playerHandPanel;
        #endregion

        #region Public methods
        /// <summary>
        /// Add a card to the player hand.
        /// </summary>
        /// <param name="cardData">Data of the card.</param>
        public void AddCard(CardData cardData)
        {
            GameObject cardObject = Instantiate(cardPrefab);
            cardObject.name = cardData.Title;
            cardObject.transform.SetParent(playerHandPanel, true);

            Card card = cardObject.GetComponent<Card>();
            card.Initialize(cardData);

            UpdateCardPositions();
        }

        /// <summary>
        /// Remove a card from the player UI deck.
        /// </summary>
        /// <param name="index">Index of the card to be removed.</param>
        public void RemoveCard(int index)
        {
            if(playerHandPanel.childCount > index)
                Destroy(playerHandPanel.GetChild(index));

            UpdateCardPositions();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Updates rotations of cards in player hand.
        /// </summary>
        private void UpdateCardPositions()
        {
            for(int i = 0; i < playerHandPanel.childCount; i++)
            {
                float positionInHand = i - playerHandPanel.childCount / 2f + 0.5f;

                RectTransform card = playerHandPanel.GetChild(i).GetComponent<RectTransform>();
                Vector2 position = card.anchoredPosition;
                position.x = positionInHand * card.sizeDelta.x * 0.6f;
                position.y = Mathf.Pow(positionInHand, 2) * -5;
                card.anchoredPosition = position;

                Vector3 rotation = card.localRotation.eulerAngles;
                rotation.z = positionInHand * -5;
                card.localRotation = Quaternion.Euler(rotation);
            }
        }
        #endregion
    }

}