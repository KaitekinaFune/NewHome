using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TurretBuilding : Building
{
    public int ShootInterval;

    public int BaseDamage;

    public int DamageIncreasePerLevel;

    public float Range;

    [SerializeField] private List<Transform> turretHeads;

    [SerializeField] Transform currentTurretHead;

    private Ship target;

    private GameManager _gameManager;

    public int Damage { get; set; }

    private void Awake()
    {
        OnUpgraded();
    }

    protected override void Start()
    {
        base.Start();
        _gameManager = GameManager.Instance;
        _gameManager.OnTick += OnTick;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _gameManager.OnTick -= OnTick;
    }

    private void OnTick(int tick)
    {
        if (ShootInterval > 0 && tick % ShootInterval == 0)
        {
            TryFindNewTarget();

            if (target != null)
            {
                target.Damage(Damage);
                ParticlesManager.Instance.Shoot(transform, target.transform);
                AudioManager.Instance.PlayTurretShootSound(_cachedTransform);
            }
        }
    }

    private void TryFindNewTarget()
    {
        if ((target != null && Vector3.Distance(_cachedTransform.position, target.transform.position) > Range) ||
            target == null)
        {
            var orderedShipByDistance = EnemiesManager.Instance.ActiveShips.OrderBy(s =>
                Vector3.Distance(_cachedTransform.position, s.transform.position)).ToList();

            var newTarget = orderedShipByDistance.FirstOrDefault(s =>
                Vector3.Distance(_cachedTransform.position, s.transform.position) <= Range);

            target = newTarget;

            if (target == null && currentTurretHead != null)
            {
                currentTurretHead.transform.DORotate(Vector3.zero, 0.2f);
            }
        }
    }

    protected override void OnUpgraded()
    {
        Damage = CurrentLevel == 1 ? BaseDamage : BaseDamage * CurrentLevel * DamageIncreasePerLevel;

        int majorUpgradeIndex = MajorUpgradeLevels.IndexOf(CurrentLevel);
        if (majorUpgradeIndex != -1)
        {
            currentTurretHead = turretHeads[majorUpgradeIndex];
        }
    }

    private void Update()
    {
        if (currentTurretHead != null && target != null)
        {
            currentTurretHead.LookAt(target.transform);
        }
    }
}