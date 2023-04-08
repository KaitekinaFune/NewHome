using TMPro;
using UnityEngine;

public class UICurrency : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private PlayerCurrency _playerCurrency;

    private void Start()
    {
        _playerCurrency = PlayerCurrency.Instance;
        _playerCurrency.OnCurrencyChanged += OnCurrencyChanged;
    }

    private void OnDestroy()
    {
        _playerCurrency.OnCurrencyChanged -= OnCurrencyChanged;
    }

    private void OnCurrencyChanged(CurrencyChangedEventArgs obj)
    {
        _text.text = obj.NewValue.ToString();
    }
}