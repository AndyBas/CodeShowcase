using Com.AndyBastel.ExperimentLab.Common.Collisions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEnd : Block
{
    public static event Action onFinishLineHit;

    [SerializeField] private ChildTrigger3D _finishTrigger = null;
    [SerializeField] private GameObject _fireworkPrefab = null;
    [SerializeField] private List<Transform> _fireworkPositions = new List<Transform>();

    private void Awake()
    {
        _finishTrigger.OnChildTriggerEnter += FinishTrigger_OnChildTriggerEnter;
    }

    private void FinishTrigger_OnChildTriggerEnter(Collider other)
    {
        if (other.CompareTag(SnakeController.TAG))
        {
            _finishTrigger.OnChildTriggerEnter -= FinishTrigger_OnChildTriggerEnter;

            PlayFireworks();

            InvokeOnFinishLineHit();
        }
    }

    private void PlayFireworks()
    {
        int lCount = _fireworkPositions.Count;
        int lIndex;
        for (lIndex = 0; lIndex < lCount; ++lIndex)
        {
            Instantiate(_fireworkPrefab, _fireworkPositions[lIndex]);

        }
    }

    private void InvokeOnFinishLineHit()
    {
        onFinishLineHit?.Invoke();
    }

    private void OnDestroy()
    {
        _finishTrigger.OnChildTriggerEnter -= FinishTrigger_OnChildTriggerEnter;
    }
}
