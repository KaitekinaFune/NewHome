using UnityEngine;
using Random = UnityEngine.Random;

public class Ship : MonoBehaviour
{
    [SerializeField] private int shotsTickInterval;
    [SerializeField] private int damage;
    [SerializeField] private int health;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationMoveSpeed;

    private bool reachedTarget;
    private Vector3 _target;

    private EnemiesManager _enemiesManager;
    private GameManager _gameManager;
    private int _lastTickShot;

    private void OnEnable()
    {
        _enemiesManager = EnemiesManager.Instance;
        _enemiesManager.ActiveShips.Add(this);

        _gameManager = GameManager.Instance;
        _gameManager.OnTick += OnTick;
    }

    private void OnDisable()
    {
        _enemiesManager.ActiveShips.Remove(this);
        _gameManager.OnTick -= OnTick;
    }

    public void Init(Vector3 moveTowards)
    {
        Vector2 randomOffset = Random.insideUnitCircle * 1;
        _target = moveTowards + new Vector3(randomOffset.x, 0, randomOffset.y);
        transform.LookAt(_target);
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            AudioManager.Instance.PlayShipDestroyedSound(transform);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!reachedTarget)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _target);

            if (distanceToTarget < 3)
            {
                AudioManager.Instance.PlayShipArrivedSound(transform);
                reachedTarget = true;
            }
        }

        if (reachedTarget)
        {
            transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
            transform.position += transform.forward * Time.deltaTime * rotationMoveSpeed;
        }
        else
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
    }


    private void OnTick(int tick)
    {
        if (!reachedTarget)
        {
            return;
        }

        if (tick % shotsTickInterval == 0)
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        BuildingsManager.Instance.MainBuilding.Damage(damage);
        AudioManager.Instance.PlayShipShootSound(transform);
    }
}