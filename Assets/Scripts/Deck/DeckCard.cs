using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Deck
{
    public class DeckCard : MonoBehaviour, IPointerClickHandler
    {
        public TextMeshProUGUI cardName;
        public Image cardIcon;
        public TextMeshProUGUI cardPrice;
        public Building buildingPrefab;

        public UIOutline Outline;

        private bool _selected;

        private void Awake()
        {
            Outline.enabled = false;
            cardName.text = buildingPrefab.BuildingName;
            cardPrice.text = buildingPrefab.BasePrice.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_selected)
            {
                DeselectCard();
            }
            else
            {
                SelectCard();
            }
        }

        public void SelectCard()
        {
            _selected = true;
            transform.DOScale(1.1f, 0.4f);
            Outline.enabled = true;
            GameManager.Instance.EnterBuildingState(this);
        }

        public void DeselectCard()
        {
            _selected = false;
            transform.DOScale(1f, 0.4f);
            Outline.enabled = false;
            GameManager.Instance.ExitBuildingState();
        }
    }
}