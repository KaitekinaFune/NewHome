using System;
using System.Collections.Generic;
using System.Linq;
using Deck;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class GameManager : Singleton<GameManager>
{
    public LayerMask TerrainLayer;
    [SerializeField] private float tickPerSeconds;

    public int CurrentTick { get; set; }
    private float _tickTimer;

    public event Action<int> OnTick;

    public GameState State { get; private set; }

    public DeckCard SelectedCardToBuild { get; set; }

    public Building SelectedBuilding { get; set; }

    public Cell HoveredCell { get; set; }

    public float OreModifier { get; set; } = 1f;
    public DeckBar SelectedDeck { get; set; }

    private void Update()
    {
        Tick();

        Camera cam = CameraManager.Instance.Camera;
        var ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, 10000f, TerrainLayer.value))
        {
            if (hitInfo.transform.TryGetComponent<Cell>(out var c))
            {
                HoveredCell = c;
            }
        }
        else
        {
            HoveredCell = HoveredCell = null;
        }
    }

    private void Tick()
    {
        if (State == GameState.Paused)
        {
            return;
        }

        _tickTimer += Time.deltaTime;
        if (_tickTimer >= tickPerSeconds)
        {
            _tickTimer -= tickPerSeconds;
            CurrentTick++;
            OnTick?.Invoke(CurrentTick);
        }
    }

    public void TryClickOnCell(Cell cell)
    {
        if (IsPointerOverUIElement())
        {
            return;
        }

        if (State == GameState.BuildingSelected && SelectedBuilding != null)
        {
            DeselectBuilding();
            return;
        }

        if (State != GameState.Building || SelectedCardToBuild == null)
        {
            return;
        }

        if (!PlanetManager.Instance.CanPlaceBuilding(SelectedCardToBuild.buildingPrefab.Size, cell))
        {
            return;
        }

        if (PlayerCurrency.Instance.CurrentCurrency < SelectedCardToBuild.buildingPrefab.BasePrice)
        {
            return;
        }

        PlayerCurrency.Instance.CurrentCurrency -= SelectedCardToBuild.buildingPrefab.BasePrice;

        Building building = Instantiate(SelectedCardToBuild.buildingPrefab);
        building.Spawn(cell.transform.position);
        cell.CellGridDisplay.SetOccupied();
        PlanetManager.Instance.OccupyCells(building, cell.transform.position);
    }

    public void EnterBuildingState(DeckCard card)
    {
        if (State == GameState.Building && SelectedCardToBuild != null)
        {
            SelectedCardToBuild.DeselectCard();
        }

        State = GameState.Building;
        SelectedCardToBuild = card;
        SelectedDeck.OnClicked();
    }

    public void ExitBuildingState()
    {
        State = GameState.Active;
        SelectedCardToBuild = null;
    }

    private bool IsPointerOverUIElement()
    {
        var eventSystemRaysastResults = GetEventSystemRaycastResults();

        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == 5)
                return true;
        }

        return false;
    }

    public void SelectBuilding(Building building)
    {
        if (IsPointerOverUIElement())
        {
            return;
        }

        if (SelectedDeck != null)
        {
            SelectedDeck.Lower();
        }

        if (State == GameState.Building && SelectedCardToBuild != null)
        {
            SelectedCardToBuild.DeselectCard();
        }

        State = GameState.BuildingSelected;
        SelectedBuilding = building;
        building.transform.DOScale(1.35f, 0.2f);
        CameraManager.Instance.ZoomInOnTarget(building.transform);
        UIManager.Instance.SelectBuilding(building);
    }

    public void DeselectBuilding()
    {
        if (SelectedBuilding != null)
        {
            SelectedBuilding.transform.DOScale(1f, 0.2f);
        }

        SelectedBuilding = null;
        CameraManager.Instance.ZoomOut();
    }

    public void SellBuilding(Building building)
    {
        int returnAmount = building.BasePrice * building.CurrentLevel / 2;
        UIManager.Instance.EarnedMoney(building.transform, returnAmount);
        PlayerCurrency.Instance.CurrentCurrency += returnAmount;
        BuildingsManager.Instance.ActiveBuildings.Remove(building);
        Destroy(building.gameObject);
        DeselectBuilding();
    }

    public bool TryUpgradeBuilding(Building building)
    {
        if (PlayerCurrency.Instance.CurrentCurrency < building.GetNextUpgradePrice())
        {
            return false;
        }

        PlayerCurrency.Instance.CurrentCurrency -= building.GetNextUpgradePrice();
        building.Upgrade();
        return true;
    }

    private static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}