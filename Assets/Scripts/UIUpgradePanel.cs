using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class UIUpgradePanel : MonoBehaviour
{
    [SerializeField] private GameObject m_upgradeButton;

    [SerializeField] private TextMeshProUGUI m_currentLevelText;
    [SerializeField] private TextMeshProUGUI m_currentIncomeText;
    [SerializeField] private TextMeshProUGUI m_nextUpgradePriceText;
    public Building Building { get; set; }

    [UsedImplicitly]
    public void OnSellButtonPressed()
    {
        if (Building == null)
        {
            return;
        }

        GameManager.Instance.SellBuilding(Building);
        Close();
    }

    [UsedImplicitly]
    public void OnUpgradeButtonPressed()
    {
        if (Building == null)
        {
            return;
        }

        GameManager.Instance.TryUpgradeBuilding(Building);
        Show(Building);
    }

    public void Show(Building building)
    {
        gameObject.SetActive(true);
        Building = building;
        m_upgradeButton.SetActive(building.CurrentLevel != building.MaxUpgrades);

        if (building is EarningBuilding earningBuilding)
        {
            m_currentIncomeText.text = "Доход:\n" + earningBuilding.CurrentIncome;
        }

        m_currentLevelText.text = "Текущий уровень:\n" + building.CurrentLevel;

        m_nextUpgradePriceText.text = "Стоимость улучшения:\n" + building.GetNextUpgradePrice();
    }

    public void Close()
    {
        GameManager.Instance.DeselectBuilding();
        gameObject.SetActive(false);
        Building = null;
    }
}