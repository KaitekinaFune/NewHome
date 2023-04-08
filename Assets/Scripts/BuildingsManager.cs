using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

public class BuildingsManager : Singleton<BuildingsManager>
{
    public List<Building> ActiveBuildings { get; } = new List<Building>();

    public event Action<BuildingEarnedEventArgs> OnBuildingEarned;

    public event Action OnBuildingsChanged;

    private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;
        _gameManager.OnTick += OnTick;
    }

    private void OnDisable()
    {
        _gameManager.OnTick -= OnTick;
    }

    private void OnTick(int tick)
    {
        int earningTotal = 0;
        foreach (var activeBuilding in ActiveBuildings)
        {
            int earningPerTick = activeBuilding.CurrentIncome;
            earningTotal += earningPerTick;
            OnBuildingEarned?.Invoke(new BuildingEarnedEventArgs(activeBuilding, earningPerTick));
        }

        if (earningTotal != 0)
        {
            AudioManager.Instance.PlayMoneyGainedSound();
        }

        PlayerCurrency.Instance.CurrentCurrency += earningTotal;
    }

    public void BuildingsChanged()
    {
        GameManager.Instance.OreModifier = ActiveBuildings.Count == 0 ? 1f : ActiveBuildings.Max(x => x.OreModifier);
        OnBuildingsChanged?.Invoke();
    }

    public int CurrentIncome()
    {
        int earningTotal = 0;
        foreach (var activeBuilding in ActiveBuildings)
        {
            int earningPerTick = activeBuilding.CurrentIncome;
            earningTotal += earningPerTick;
        }

        return earningTotal;
    }
}

public struct BuildingEarnedEventArgs
{
    public Building Building;

    public int Amount;

    public BuildingEarnedEventArgs(Building building, int amount)
    {
        Building = building;
        Amount = amount;
    }
}