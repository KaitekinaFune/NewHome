using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CellGridDisplay : MonoBehaviour
{
    private const float animationDuration = 0.04f;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color occupiedColor;
    [SerializeField] private Color freeColor;

    public void SetFree()
    {
        sprite.color = freeColor;
    }

    public void SetOccupied()
    {
        sprite.color = occupiedColor;
    }

    public void FadeIn(bool occupied)
    {
        if (occupied)
        {
            SetOccupied();
        }
        else
        {
            SetFree();
        }

        sprite.DOFade(1f, animationDuration);
        DelayedSwitch(true, animationDuration, this.GetCancellationTokenOnDestroy()).Forget();
    }

    public void FadeOut()
    {
        sprite.DOFade(0f, animationDuration);
        DelayedSwitch(false, animationDuration, this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTask DelayedSwitch(bool value, float duration, CancellationToken cancellationToken)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: cancellationToken);
            if (gameObject != null)
            {
                gameObject.SetActive(value);
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