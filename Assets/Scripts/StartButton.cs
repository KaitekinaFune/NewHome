using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField] private Image fadeOutImage;
    [SerializeField] private float delay;

    private void Awake()
    {
        fadeOutImage.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        StartGameAsync().Forget();
    }

    private async UniTaskVoid StartGameAsync()
    {
        fadeOutImage.color = Color.clear;
        fadeOutImage.gameObject.SetActive(true);
        fadeOutImage.DOColor(Color.black, delay);
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: this.GetCancellationTokenOnDestroy());
            SceneManager.LoadScene(1);
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