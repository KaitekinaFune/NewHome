using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Ore : PlanetEntity
{
    private int _tickUntil;
    private int _amountPerClick;
    private int _clicksAmount;

    private bool _isDestroying;

    public void Init(int durationInTicks, int amountPerClick, int clicksAmount)
    {
        _tickUntil = GameManager.Instance.CurrentTick + durationInTicks;
        _amountPerClick = amountPerClick;
        _clicksAmount = clicksAmount;
        _isDestroying = false;
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
        if (_isDestroying || _clicksAmount == 0)
        {
            return;
        }

        int amount = GetClickedMoney();
        if (amount == 0)
        {
            return;
        }

        UIManager.Instance.EarnedMoney(_cachedTransform, amount);
        _clicksAmount--;
        PlayerCurrency.Instance.CurrentCurrency += amount;

        if (_clicksAmount == 0)
        {
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

    private async UniTaskVoid DelayedDestroy()
    {
        const float animDuration = 0.3f;

        _isDestroying = true;
        transform.DOScale(0f, animDuration);
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: this.GetCancellationTokenOnDestroy());

            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}