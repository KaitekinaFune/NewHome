using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField] private Vector3 to;
    [SerializeField] private float fadeinDuration;

    public void Move()
    {
        DOasync().Forget();
    }

    private async UniTaskVoid DOasync()
    {
        try
        {
            gameObject.transform.DOLocalMove(to, fadeinDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(fadeinDuration),
                cancellationToken: this.GetCancellationTokenOnDestroy());
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