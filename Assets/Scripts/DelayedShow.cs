using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DelayedShow : MonoBehaviour
{
    [SerializeField] private List<Graphic> graphics;
    [SerializeField] private float animDuration;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (IntroSkip.Instance != null)
        {
            IntroSkip.Instance.Skipped = true;
        }

        DelayedShowAsync().Forget();
    }

    private async UniTaskVoid DelayedShowAsync()
    {
        CancellationToken token = this.GetCancellationTokenOnDestroy();
        foreach (var graphic in graphics)
        {
            Color color = graphic.color;
            float originalAlpha = color.a;
            color.a = 0;
            graphic.color = color;
            graphic.DOFade(originalAlpha, animDuration);
        }

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(animDuration), cancellationToken: token);
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