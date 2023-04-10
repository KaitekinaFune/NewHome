using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PlanetEntity : MonoBehaviour
{
    protected Transform _cachedTransform;

    public Vector2Int Size;

    protected bool IsDestroying;

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

    public async UniTaskVoid DelayedDestroy(Action doAfter = null)
    {
        const float animDuration = 0.65f;

        IsDestroying = true;
        _cachedTransform.DOScale(0f, animDuration).SetEase(Ease.OutQuad);

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(animDuration),
                cancellationToken: this.GetCancellationTokenOnDestroy());

            if (gameObject != null)
            {
                Destroy(gameObject);
            }

            doAfter?.Invoke();
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