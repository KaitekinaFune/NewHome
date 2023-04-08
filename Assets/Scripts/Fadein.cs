using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fadein : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float fadeinDuration;

    private void Start()
    {
        DOasync().Forget();
    }

    private async UniTaskVoid DOasync()
    {
        try
        {
            image.DOFade(0f, fadeinDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(fadeinDuration),
                cancellationToken: this.GetCancellationTokenOnDestroy());
            gameObject.SetActive(false);
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