using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class OreManager : Singleton<OreManager>
{
    [SerializeField] private List<Ore> _orePrefabs;

    private GameManager _gameManager;

    public int SpawnRateInTicks = 15;
    public int DurationInTicks = 10;
    public int BaseAmountPerClick = 10;
    public int ClicksAmount;

    public void SpawnRandomOre()
    {
        Ore randomOrePrefab = _orePrefabs[Random.Range(0, _orePrefabs.Count)];
        Ore spawnedOre = Instantiate(randomOrePrefab);
        var suitableCell = PlanetManager.Instance.FindRandomNonOccupiedCell(spawnedOre.Size);

        spawnedOre.Spawn(suitableCell.transform.position, false);
        spawnedOre.Init(DurationInTicks, BaseAmountPerClick, ClicksAmount);
        PlanetManager.Instance.OccupyCells(spawnedOre, suitableCell.transform.position);
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.OnTick += OnTick;
    }

    private void OnDestroy()
    {
        _gameManager.OnTick -= OnTick;
    }

    private void OnTick(int tick)
    {
        if (tick % SpawnRateInTicks == 0)
        {
            SpawnRandomOre();
        }
    }
}