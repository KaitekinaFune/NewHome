using UnityEngine.Serialization;

public class EarningBuilding : Building
{
    [FormerlySerializedAs("EarningPerTick")]
    public int BaseEarningPerTick;

    public float EarningsIncreasePerLevel = 2f;
    public int CurrentIncome { get; set; }

    private BuildingsManager _buildingsManager;

    public float OreModifier;

    protected override void Awake()
    {
        base.Awake();
        CurrentIncome = BaseEarningPerTick;
        CurrentPrice = BasePrice;
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

    protected override void OnUpgraded()
    {
        CurrentIncome = (int)(BaseEarningPerTick * CurrentLevel * EarningsIncreasePerLevel);
    }
}