using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : MobileScreen
{
    [SerializeField] private Transform _congratsText = null;
    [SerializeField] private AnimationCurve _congratsAnimCurve = default;
    [SerializeField] private float _congratsAnimTime = 1f;
    [SerializeField] private float _congratsAnimMinScale = 1f;
    [SerializeField] private float _congratsAnimMaxScale = 1.3f;


    public override void FinishAppearing()
    {
        base.FinishAppearing();
        StartCoroutine(CongratulationsAnimCoroutine());
    }

    private IEnumerator CongratulationsAnimCoroutine()
    {
        float lElapsedTime = 0f;
        float lRatio = 0f;
        while (lElapsedTime < _congratsAnimTime)
        {
            lRatio = _congratsAnimCurve.Evaluate(lElapsedTime / _congratsAnimTime);
            _congratsText.localScale = Vector3.one * Mathf.Lerp(_congratsAnimMinScale, _congratsAnimMaxScale, lRatio);

            lElapsedTime += Time.deltaTime;

            yield return null;  
        }
    }
}
