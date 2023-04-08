using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fadein : MonoBehaviour
{
    [SerializeField] private Graphic image;
    [SerializeField] private float fadeinDuration;
    [SerializeField] private float delay;


    private void Start()
    {
        DOasync().Forget();
    }

    private async UniTaskVoid DOasync()
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay),
                cancellationToken: this.GetCancellationTokenOnDestroy());
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