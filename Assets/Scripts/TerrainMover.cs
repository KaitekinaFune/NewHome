using System;
using UnityEngine;

public class TerrainMover : MonoBehaviour
{
    private Transform _cachedTransform;

    private void Awake()
    {
        _cachedTransform = transform;
        var position = _cachedTransform.position;
        position = new Vector3(
            (float)HalfRound(position.x),
            (float)HalfRound(position.y),
            (float)HalfRound(position.z));
        _cachedTransform.position = position;
    }

    private double HalfRound(double value)
    {
        return Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2;
    }
}