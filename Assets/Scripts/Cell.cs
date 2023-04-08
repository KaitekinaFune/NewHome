using System;
using JetBrains.Annotations;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [CanBeNull] public PlanetEntity OccupiedEntity { get; set; }

    public bool CanBuildOn;

    private Transform _cachedTransform;

    public CellGridDisplay CellGridDisplay { get; set; }

    private void Awake()
    {
        _cachedTransform = transform;
        var position = _cachedTransform.position;
        position = new Vector3(
            (float)HalfRound(position.x),
            (float)HalfRound(position.y),
            (float)HalfRound(position.z));
        _cachedTransform.position = position;

        CellGridDisplay = Instantiate(PlanetManager.Instance._gridDisplayPrefab, _cachedTransform);
        PlanetManager.Instance.Cells[_cachedTransform.position] = this;
        Hide();
    }

    public void Show()
    {
        if (CellGridDisplay == null)
        {
            return;
        }

        CellGridDisplay.FadeIn(OccupiedEntity != null || !CanBuildOn);
    }

    public void Hide()
    {
        if (CellGridDisplay == null)
        {
            return;
        }

        CellGridDisplay.FadeOut();
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Building)
        {
            ShowBuildingPreview();
        }
        else
        {
            ShowGrid();
        }
    }

    private void ShowGrid()
    {
        if (GameManager.Instance.HoveredCell == this)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void ShowBuildingPreview()
    {
        if (GameManager.Instance.HoveredCell == null)
        {
            Hide();
            return;
        }

        if (GameManager.Instance.HoveredCell == this)
        {
            ShowGrid();
            return;
        }

        if (GameManager.Instance.HoveredCell.transform.position.x > _cachedTransform.position.x ||
            GameManager.Instance.HoveredCell.transform.position.z > _cachedTransform.position.z)
        {
            Hide();
            return;
        }

        float xDelta = _cachedTransform.position.x - GameManager.Instance.HoveredCell.transform.position.x;
        float yDelta = _cachedTransform.position.z - GameManager.Instance.HoveredCell.transform.position.z;
        
        // Vector3 distance = _cachedTransform.position - GameManager.Instance.HoveredCell.transform.position;
        bool isBuildingPreview = xDelta < GameManager.Instance.SelectedCardToBuild.buildingPrefab.Size.x &&
                                 yDelta < GameManager.Instance.SelectedCardToBuild.buildingPrefab.Size.y;

        if (isBuildingPreview)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private double HalfRound(double value)
    {
        return Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2;
    }

    private void OnMouseUpAsButton()
    {
        GameManager.Instance.TryClickOnCell(this);
    }
}