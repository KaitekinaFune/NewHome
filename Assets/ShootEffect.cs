using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ShootEffect : MonoBehaviour
{
    [SerializeField] private float speed;

    private Transform _target;

    private bool _isDestroying;

    public void Init(Transform target)
    {
        _target = target;
    }

    private void Update()
    {
        if (_target == null && !_isDestroying)
        {
            _isDestroying = true;
            DelayedDestroyAsync().Forget();
        }

        transform.position += transform.forward * Time.deltaTime * speed;

        if (_isDestroying)
        {
            return;
        }

        transform.LookAt(_target);
        if (Vector3.Distance(_target.position, transform.position) > 0.1)
        {
            return;
        }

        Destroy(gameObject);
    }

    private async UniTaskVoid DelayedDestroyAsync()
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: this.GetCancellationTokenOnDestroy());
            Destroy(gameObject);
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