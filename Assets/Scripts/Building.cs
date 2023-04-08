using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Building : PlanetEntity
{
    public string BuildingName;

    [FormerlySerializedAs("EarningPerTick")]
    public int BaseEarningPerTick;

    [FormerlySerializedAs("Price")] public int BasePrice;
    public float PriceIncreasePerLevel = 1.15f;
    public float EarningsIncreasePerLevel = 2f;
    public int MaxUpgrades = 10;
    public int CurrentLevel { get; set; }
    public int CurrentIncome { get; set; }
    public int CurrentPrice { get; set; }
    public float OreModifier = 1f;

    private BuildingsManager _buildingsManager;

    public List<int> MajorUpgradeLevels = new List<int> { 1, 5, 10 };
    public List<GameObject> MajorUpgradeObjects;

    private void Awake()
    {
        CurrentIncome = BaseEarningPerTick;
        CurrentPrice = BasePrice;
        CurrentLevel = 1;
    }

    private void Start()
    {
        BuildingsManager.Instance.BuildingsChanged();
    }

    private void OnDestroy()
    {
        BuildingsManager.Instance.BuildingsChanged();
    }

    private void OnEnable()
    {
        _buildingsManager = BuildingsManager.Instance;
        _buildingsManager.ActiveBuildings.Add(this);
    }

    private void OnDisable()
    {
        _buildingsManager.ActiveBuildings.Remove(this);
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

        int majorUpgradeIndex = MajorUpgradeLevels.IndexOf(CurrentLevel);
        if (majorUpgradeIndex != -1)
        {
            for (var index = 0; index < MajorUpgradeObjects.Count; index++)
            {
                var majorUpgrade = MajorUpgradeObjects[index];
                majorUpgrade.SetActive(index == majorUpgradeIndex);
            }
        }

        CurrentLevel++;
        CurrentIncome = (int)(BaseEarningPerTick * CurrentLevel * EarningsIncreasePerLevel);
        CurrentPrice = GetNextUpgradePrice();
        BuildingsManager.Instance.BuildingsChanged();
    }

    public int GetNextUpgradePrice()
    {
        return (int)(BasePrice * CurrentLevel * PriceIncreasePerLevel);
    }
}