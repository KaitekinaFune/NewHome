using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Building : PlanetEntity
{
    public string BuildingName;

    [FormerlySerializedAs("Price")] public int BasePrice;
    public float PriceIncreasePerLevel = 1.15f;
    public int MaxUpgrades = 10;
    public int CurrentLevel { get; set; } = 1;
    public int CurrentPrice { get; set; }

    public List<int> MajorUpgradeLevels = new List<int> { 1, 4, 9 };
    public List<GameObject> MajorUpgradeObjects;

    protected virtual void Awake()
    {
        _cachedTransform = transform;
        CurrentPrice = BasePrice;
    }

    protected virtual void Start()
    {
        BuildingsManager.Instance.BuildingsChanged();
        AudioManager.Instance.PlayBuildingCreatedSound(_cachedTransform);
    }

    protected virtual void OnDestroy()
    {
        BuildingsManager.Instance.BuildingsChanged();
    }

    private void OnMouseUpAsButton()
    {
        GameManager.Instance.SelectBuilding(this);
    }

    public void Upgrade()
    {
        if (CurrentLevel + 1 > MaxUpgrades)
        {
            return;
        }

        CurrentLevel++;
        int majorUpgradeIndex = MajorUpgradeLevels.IndexOf(CurrentLevel);
        bool majorUpgradeDone = false;
        if (majorUpgradeIndex != -1)
        {
            for (var index = 0; index < MajorUpgradeObjects.Count; index++)
            {
                var majorUpgrade = MajorUpgradeObjects[index];
                majorUpgrade.SetActive(index == majorUpgradeIndex);
                majorUpgradeDone = true;
            }
        }

        if (majorUpgradeDone)
        {
            AudioManager.Instance.PlayBuildingMajorUpgrade(_cachedTransform);
        }
        else
        {
            AudioManager.Instance.PlayBuildingMinorUpgrade(_cachedTransform);
        }

        CurrentPrice = GetNextUpgradePrice();
        OnUpgraded();
        BuildingsManager.Instance.BuildingsChanged();
    }

    protected virtual void OnUpgraded()
    {
    }

    public int GetNextUpgradePrice()
    {
        return (int)(BasePrice * CurrentLevel * PriceIncreasePerLevel);
    }
}