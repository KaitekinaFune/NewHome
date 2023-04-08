using TMPro;
using UnityEngine;

public class UIIncome : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private BuildingsManager _buildingsManager;

    private void Start()
    {
        _buildingsManager = BuildingsManager.Instance;
        _buildingsManager.OnBuildingsChanged += OnBuildingsChanged;
        OnBuildingsChanged();
    }

    private void OnDestroy()
    {
        _buildingsManager.OnBuildingsChanged -= OnBuildingsChanged;
    }

    private void OnBuildingsChanged()
    {
        _text.text = _buildingsManager.CurrentIncome() + " / в сек";
    }
}