public class Ore : PlanetEntity
{
    private int _tickUntil;
    private int _amountPerClick;
    private int _clicksAmount;

    public void Init(int durationInTicks, int amountPerClick, int clicksAmount)
    {
        _tickUntil = GameManager.Instance.CurrentTick + durationInTicks;
        _amountPerClick = amountPerClick;
        _clicksAmount = clicksAmount;
        IsDestroying = false;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnTick += OnTick;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnTick -= OnTick;
    }

    private void OnMouseDown()
    {
        if (IsDestroying || _clicksAmount == 0)
        {
            return;
        }

        int amount = GetClickedMoney();
        if (amount == 0)
        {
            return;
        }

        AudioManager.Instance.PlayOreDigSound(_cachedTransform);
        UIManager.Instance.EarnedMoney(_cachedTransform, amount);
        _clicksAmount--;
        PlayerCurrency.Instance.CurrentCurrency += amount;

        if (_clicksAmount == 0)
        {
            AudioManager.Instance.PlayOreBreakSound(_cachedTransform);
            DelayedDestroy().Forget();
        }
    }

    private int GetClickedMoney()
    {
        if (_clicksAmount == 0)
        {
            return 0;
        }

        if (_clicksAmount == 1)
        {
            return (int)(_amountPerClick * 3 * GameManager.Instance.OreModifier);
        }

        return (int)(_amountPerClick * GameManager.Instance.OreModifier);
    }

    private void OnTick(int tick)
    {
        if (tick > _tickUntil)
        {
            DelayedDestroy().Forget();
        }
    }
}