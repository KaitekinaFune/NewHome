using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EarningBuilding : Building
{
    [FormerlySerializedAs("EarningPerTick")]
    public int BaseEarningPerTick;

    public float EarningsIncreasePerLevel = 2f;
    public int CurrentLevel { get; set; }
    public int CurrentIncome { get; set; }
    public int CurrentPrice { get; set; }

    private BuildingsManager _buildingsManager;

    public List<int> MajorUpgradeLevels = new List<int> { 1, 5, 10 };
    public List<GameObject> MajorUpgradeObjects;

    public float OreModifier;

    private void Awake()
    {
        CurrentIncome = BaseEarningPerTick;
        CurrentPrice = BasePrice;
        CurrentLevel = 1;
    }

    private void Start()
    {
        BuildingsManager.Instance.BuildingsChanged();
        AudioManager.Instance.PlayBuildingCreatedSound(_cachedTransform);
    }

    private void OnDestroy()
    {
        BuildingsManager.Instance.BuildingsChanged();
    }

    private void OnEnable()
    {
        _buildingsManager = BuildingsManager.Instance;
        _buildingsManager.ActiveEarningBuildings.Add(this);
    }

    private void OnDisable()
    {
        _buildingsManager.ActiveEarningBuildings.Remove(this);
    }

    private void OnMouseUpAsButton()
    {
        GameManager.Instance.SelectBuilding(this);
    }

    protected override void OnUpgraded()
    {
        CurrentIncome = (int)(BaseEarningPerTick * CurrentLevel * EarningsIncreasePerLevel);
    }
}