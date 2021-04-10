using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PSG.SpaceCargo.UI
{
    public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private RectTransform rectTransform;
        private Vector2 normalPosition;
        private int siblingIndex;
        private bool isPointerOver = false;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isPointerOver)
            {
                normalPosition = rectTransform.anchoredPosition;
                siblingIndex = rectTransform.GetSiblingIndex();
                rectTransform.SetAsLastSibling();

                rectTransform.DOAnchorPosY(normalPosition.y + 50f, 0.5f);
                isPointerOver = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isPointerOver)
            {
                rectTransform.SetSiblingIndex(siblingIndex);

                rectTransform.DOAnchorPosY(normalPosition.y, 0.5f);
                isPointerOver = false;
            }
        }
    }
}
