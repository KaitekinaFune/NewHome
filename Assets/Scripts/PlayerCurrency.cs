using System;
using UnityEngine;
using Utils;

public class PlayerCurrency : Singleton<PlayerCurrency>
{
    [SerializeField] private long _startingCurrency;

    private void Awake()
    {
        CurrentCurrency = _startingCurrency;
    }

    private long _currentCurrency;

    public long CurrentCurrency
    {
        get => _currentCurrency;
        set
        {
            if (_currentCurrency == value)
            {
                return;
            }

            long oldValue = _currentCurrency;
            _currentCurrency = value;

            Debug.Log("PlayerCurrency: " + _currentCurrency);
            OnCurrencyChanged?.Invoke(new CurrencyChangedEventArgs(oldValue, _currentCurrency));
        }
    }

    public event Action<CurrencyChangedEventArgs> OnCurrencyChanged;
}

public struct CurrencyChangedEventArgs
{
    public long OldValue;

    public long NewValue;

    public CurrencyChangedEventArgs(long oldValue, long newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }
}