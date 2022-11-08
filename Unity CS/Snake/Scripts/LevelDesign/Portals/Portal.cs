using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct PortalSettings
{
    public bool randomizeIsBonus;
    public bool isBonus;
    public bool randomizeIsAddition;
    public bool isAddition;
    public bool randomizeImpactValue;
    public int impactValue;

}

public class Portal : MonoBehaviour
{
    public static event Action<Portal> onHit;

    [Header("Components")]
    [SerializeField] private GameObject _panel = null;
    [SerializeField] private TextMeshProUGUI _text = null;

    [Header("Colors")]
    [SerializeField] private Color _colorBonus = Color.blue;
    [SerializeField] private Color _colorMalus = Color.red;

    [Header("Values Range")]
    [SerializeField] private bool _isRandom = true;
    [SerializeField, Range(1, 40)] private int _minAdditionValue = 1;
    [SerializeField, Range(1, 40)] private int _maxAdditionValue = 10;
    [SerializeField, Range(1, 3)] private int _minMultiplicationValue = 1;
    [SerializeField, Range(1, 3)] private int _maxMultiplicationValue = 3;

    [Header("Effects")]
    [SerializeField] private Transform _particleBonus = null;
    [SerializeField] private Transform _particleMalus = null;
    [SerializeField] private string _soundBonus = "";
    [SerializeField] private string _soundMalus = "";

    private int _impactValue = 0;
    private bool _isBonus = true;
    private bool _isAddition = true;

    public int ImpactValue => _impactValue;
    public bool IsBonus => _isBonus;
    public bool IsAddition => _isAddition;

    public bool IsRandom 
    { 
        get => _isRandom; 
        set
        {
            _isRandom = value; 
            if(_isRandom)
                SetupPortal();
        }
    }

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        if(_isRandom)
            SetupPortal();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(SnakeController.TAG))
        {
            InvokeOnHit();
            PlayFX(other.ClosestPoint(transform.position));
        }
    }
    #endregion Unity Methods 

    #region Setup

    public void SetupPortal()
    {
        RandomizeIsBonus();
        RandomizeIsAddition();
        RandomizeImpactValue();

        ApplyVisualEffects();
    }
    public void SetupPortal(bool isBonus)
    {
        _isBonus = isBonus;
        RandomizeIsAddition();
        RandomizeImpactValue();

        ApplyVisualEffects();
    }

    public void SetupPortal(bool isBonus, bool isAddition)
    {
        _isBonus = isBonus;
        _isAddition = isAddition;
        RandomizeImpactValue();

        ApplyVisualEffects();
    }

    public void SetupPortal(bool isBonus, bool isAddition, int impactValue)
    {
        _isBonus = isBonus;
        _isAddition = isAddition;
        _impactValue = impactValue;

        ApplyVisualEffects();
    }


    #region Randomization

    private void RandomizeIsBonus()
    {
        _isBonus = UnityEngine.Random.Range(0f, 1f) > 0.5f ? true : false;
    }
    private void RandomizeIsAddition()
    {
        _isAddition = UnityEngine.Random.Range(0f, 1f) > 0.5f ? true : false;
    }
    private void RandomizeImpactValue()
    {
        if (_isAddition)
        {
            _impactValue = UnityEngine.Random.Range(_minAdditionValue, _maxAdditionValue);
        }
        else _impactValue = UnityEngine.Random.Range(_minMultiplicationValue, _maxMultiplicationValue);
    }
    #endregion Randomization

    #endregion Setup





    #region Effects

    private void PlayFX(Vector3 pos)
    {
        Transform lParticle = Instantiate(_isBonus ? _particleBonus : _particleMalus);
        lParticle.position = pos;

        SoundManager.Instance.Play(IsBonus ? _soundBonus : _soundMalus);

    }
    private void ApplyVisualEffects()
    {
        string lText = _impactValue.ToString();
        string lSign = "";

        if (_isBonus)
        {
            if (_isAddition)
                lSign = "+";
            else lSign = "*";
        }
        else
        {
            if (_isAddition)
                lSign= "-";
            else lSign= "/";
        }
        _panel.GetComponent<MeshRenderer>().material.color = _isBonus ? _colorBonus : _colorMalus;


        _text.text = lSign + lText;
    }
    #endregion


    #region Events
    private void InvokeOnHit()
    {
        onHit?.Invoke(this);
    }

    #endregion Events


}
