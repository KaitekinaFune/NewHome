using DG.Tweening;
using UnityEngine;
using Utils;

public class CameraManager : Singleton<CameraManager>
{
    public Transform Pivot;

    public Camera Camera;

    private Vector3 _defaultPosition;

    [SerializeField] private float zoomInAnimationDuration = 0.4f;
    [SerializeField] private float zoomCameraOrthoSizeDefault = 9f;
    [SerializeField] private float zoomCameraOrthoZoomedInSize = 5f;

    private void Awake()
    {
        _defaultPosition = transform.position;
    }

    public void ZoomInOnTarget(Transform target)
    {
        Camera.DOOrthoSize(zoomCameraOrthoZoomedInSize, zoomInAnimationDuration);
        Pivot.DOMove(new Vector3(target.position.x, _defaultPosition.y, target.position.z), zoomInAnimationDuration);
    }

    public void ZoomOut()
    {
        AudioManager.Instance.PlayUI1();
        Pivot.DOMove(_defaultPosition, zoomInAnimationDuration);
        Camera.DOOrthoSize(zoomCameraOrthoSizeDefault, zoomInAnimationDuration);
    }
}