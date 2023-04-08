using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class EnemiesManager : Singleton<EnemiesManager>
{
    [SerializeField] private Ship smallShip;
    [SerializeField] private Ship mediumShip;
    [SerializeField] private Ship bigShip;

    [SerializeField] private float shipSpawnOffsetX = 20;
    [SerializeField] private float shipSpawnHeight = 7.25f;
    [SerializeField] private float shipSpawnOffsetZ = 35f;

    [SerializeField] private int smallShipFirstAppearanceTick;
    [SerializeField] private int mediumShipFirstAppearanceTick;
    [SerializeField] private int bigShipFirstAppearanceTick;

    [SerializeField] private int smallShipSpawnRate;
    [SerializeField] private int mediumShipSpawnRate;
    [SerializeField] private int bigShipSpawnRate;

    [SerializeField] private int smallShipMinSpawnRate;
    [SerializeField] private int mediumShipMinSpawnRate;
    [SerializeField] private int bigShipMinSpawnRate;

    [SerializeField] private float smallShipSpawnRateAppearanceRate;
    [SerializeField] private float mediumShipSpawnRateAppearanceRate;
    [SerializeField] private float bigShipSpawnRateAppearanceRate;


    public List<Ship> ActiveShips { get; } = new List<Ship>();

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
        if (tick >= smallShipFirstAppearanceTick && tick % smallShipSpawnRate == 0)
        {
            smallShipSpawnRate = Math.Max(smallShipMinSpawnRate,
                (int)(smallShipSpawnRate * smallShipSpawnRateAppearanceRate));
            SpawnShip(smallShip);
        }

        if (tick >= mediumShipFirstAppearanceTick && tick % mediumShipSpawnRate == 0)
        {
            mediumShipSpawnRate = Math.Max(mediumShipMinSpawnRate,
                (int)(mediumShipSpawnRate * mediumShipSpawnRateAppearanceRate));
            SpawnShip(mediumShip);
        }

        if (tick >= bigShipFirstAppearanceTick && tick % bigShipSpawnRate == 0)
        {
            bigShipSpawnRate = Math.Max(bigShipMinSpawnRate, (int)(bigShipSpawnRate * bigShipSpawnRateAppearanceRate));
            SpawnShip(bigShip);
        }
    }

    private void SpawnShip(Ship shipPrefab)
    {
        bool isHorizontalRandom = Random.value > .5f;
        Vector3 randomSpawnPoint = new Vector3
        {
            x = isHorizontalRandom ? Random.Range(-shipSpawnOffsetX, shipSpawnOffsetX) : shipSpawnOffsetX,
            y = shipSpawnHeight,
            z = !isHorizontalRandom ? Random.Range(-shipSpawnOffsetZ, shipSpawnOffsetZ) : shipSpawnOffsetZ
        };

        var ship = Instantiate(shipPrefab);
        ship.transform.position = randomSpawnPoint;
        ship.Init(new Vector3(0, shipSpawnHeight, 0));
    }
}