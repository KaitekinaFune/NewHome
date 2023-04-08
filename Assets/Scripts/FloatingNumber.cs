using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private Color increaseColor = Color.green;
    [SerializeField] private Color decreaseColor = Color.red;
    [SerializeField] private Color earningsColor = Color.green;

    [SerializeField] private float travelDistance = 50f;


    public void ShowEarnings(int amount, bool goAbove, float duration)
    {
        _text.color = increaseColor;
        _text.text = $"+{amount}";
        _text.DOFade(0f, duration);

        var currentYPos = transform.position.y;

        transform.DOMoveY(goAbove ? currentYPos + travelDistance : currentYPos - travelDistance, duration);
    }
}