using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject looseScreen;
    [SerializeField] private UIUpgradePanel upgradePanel;
    [SerializeField] private FloatingNumber floatingNumberPrefab;


    private BuildingsManager _buildingsManager;
    private GameManager _gameManager;

    public GameObject ProductionButton;
    public GameObject DefenseButton;

    private void Start()
    {
        _buildingsManager = BuildingsManager.Instance;
        _gameManager = GameManager.Instance;

        _buildingsManager.OnBuildingEarned += OnBuildingEarned;
    }

    private void OnDestroy()
    {
        _buildingsManager.OnBuildingEarned -= OnBuildingEarned;
    }

    public void EarnedMoney(Transform target, int value)
    {
        ShowMoneyEarnedAsync(target, value).Forget();
    }

    public void OnBuildingEarned(BuildingEarnedEventArgs obj)
    {
        EarnedMoney(obj.Building.transform, obj.Amount);
    }

    private async UniTask ShowMoneyEarnedAsync(Transform target, int value)
    {
        const float animationDuration = 1f;
        try
        {
            var floatingNumber = Instantiate(floatingNumberPrefab);
            floatingNumber.transform.position = target.position + Vector3.up * 1.5f;
            floatingNumber.ShowEarnings(value, true, animationDuration);

            await UniTask.Delay(TimeSpan.FromSeconds(animationDuration));
            Destroy(floatingNumber.gameObject);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void SelectBuilding(Building building)
    {
        upgradePanel.Show(building);
    }

    public void ShowLoseScreen()
    {
        looseScreen.gameObject.SetActive(true);
    }
}