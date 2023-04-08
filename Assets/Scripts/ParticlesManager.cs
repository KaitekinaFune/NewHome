using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

public class ParticlesManager : Singleton<ParticlesManager>
{
    [SerializeField] private ShootEffect _shootEffect;

    [SerializeField] private GameObject _explosionEffect;

    public void Shoot(Transform from, Transform to)
    {
        var effect = Instantiate(_shootEffect);
        effect.transform.position = from.position;
        effect.Init(to);
    }

    public void ShipDestroyed(Vector3 position)
    {
        DelayedDestroyAsync(position).Forget();
    }

    private async UniTaskVoid DelayedDestroyAsync(Vector3 position)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: this.GetCancellationTokenOnDestroy());
            var effect = Instantiate(_explosionEffect);
            effect.transform.position = position;
            // Destroy(obj);
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