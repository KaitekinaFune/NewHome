using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class PlanetManager : Singleton<PlanetManager>
{
    public Dictionary<Vector3, Cell> Cells = new Dictionary<Vector3, Cell>();

    [SerializeField] public CellGridDisplay _gridDisplayPrefab;

    public bool CanPlaceBuilding(Vector2Int size, Cell cell)
    {
        if (!cell.CanBuildOn)
        {
            return false;
        }

        Vector3 cellPosition = cell.transform.position;

        for (float x = cellPosition.x; x < cellPosition.x + size.x; x++)
        {
            for (float z = cellPosition.z; z < cellPosition.z + size.y; z++)
            {
                if (Cells.TryGetValue(new Vector3(x, cellPosition.y, z), out Cell c))
                {
                    if (c.OccupiedEntity != null || !c.CanBuildOn)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void OccupyCells(PlanetEntity entity, Vector3 cellPosition)
    {
        for (float x = cellPosition.x; x < cellPosition.x + entity.Size.x; x++)
        {
            for (float z = cellPosition.z; z < cellPosition.z + entity.Size.y; z++)
            {
                if (Cells.TryGetValue(new Vector3(x, cellPosition.y, z), out Cell c))
                {
                    c.OccupiedEntity = entity;
                }
            }
        }
    }

    public Cell FindRandomNonOccupiedCell(Vector2Int size)
    {
        var suitableCells = Cells.Where(c =>
            c.Value.CanBuildOn && c.Value.OccupiedEntity == null && HasFreeNeighbours(c.Key, size)).ToList();

        if (suitableCells.Count == 0)
        {
            return null;
        }

        return suitableCells[Random.Range(0, suitableCells.Count)].Value;
    }

    private bool HasFreeNeighbours(Vector3 position, Vector2Int size)
    {
        for (float x = position.x; x < position.x + size.x; x++)
        {
            for (float z = position.z; z < position.z + size.y; z++)
            {
                if (Cells.TryGetValue(new Vector3(x, position.y, z), out Cell c))
                {
                    if (c.OccupiedEntity != null || !c.CanBuildOn)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }
}