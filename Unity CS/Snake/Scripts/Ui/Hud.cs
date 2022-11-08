using Com.AndyBastel.Common.Ui;
using System.Collections;
using TMPro;
using UnityEngine;

public class Hud : UiScreen
{
    [Header("Container")]
    [SerializeField] private Transform _coinsContainer = null;
    [SerializeField] private float _scaleUp = 1.5f;
    [SerializeField] private AnimationCurve _scaleCurve = default;
    [SerializeField] private float _scaleAnimationTime = 0.3f;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _coinsText;

    private float _initialCoinsContainerScale = 1f;

    public string CoinsText
    {
        get => _coinsText.text;
        set
        {
            _coinsText.text = value;
            PlayGetCoinsAnimation();
        }
    }
    public string LevelText
    {
        get => _levelText.text;
        set => _levelText.text = value;
    }

    private void PlayGetCoinsAnimation()
    {
        StartCoroutine(GetCoinsAnimationCoroutine());
    }

    private IEnumerator GetCoinsAnimationCoroutine()
    {
        float lElapsedTime = 0f;
        float lRatio = 0f;

        while (lElapsedTime < _scaleAnimationTime)
        {
            lRatio = _scaleCurve.Evaluate(lElapsedTime / _scaleAnimationTime);
            _coinsContainer.transform.localScale = Vector3.one * Mathf.Lerp(_initialCoinsContainerScale, _scaleUp, lRatio);

            lElapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
