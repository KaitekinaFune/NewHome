using DG.Tweening;
using UnityEngine;

public class PlanetEntity : MonoBehaviour
{
    protected Transform _cachedTransform;

    public Vector2Int Size;

    public void Spawn(Vector3 position, bool offset = true)
    {
        _cachedTransform ??= transform;

        _cachedTransform.transform.position =
            new Vector3(
                offset ? position.x + .5f : position.x,
                position.y,
                offset ? position.z + .5f : position.z);
        _cachedTransform.localScale = Vector3.zero;
        _cachedTransform.DOScale(1f, 0.45f);
    }
}