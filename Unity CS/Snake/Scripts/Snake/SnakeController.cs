using Com.AndyBastel.ExperimentLab.Common.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : StateObject
{
    public const string TAG = "Snake";
    private const float MAX_ANGLE = 90f;

    public event Action onLost;

    [Header("Body")]
    [SerializeField] private int _gapBetweenParts = 10;
    [SerializeField] private float _disappearTime = 0.5f;
    [SerializeField] private AnimationCurve _disappearCurve = default;

    [Header("Movements")]
    [SerializeField] private float _speed = 5f; 
    [SerializeField] private float _rotationSpeed = 45f; 

    private bool _pressed = false;
    private Vector2 _sidewaysVector = Vector2.zero;
    private float _angle = 0f;

    private float _halfWidth = 0f;

    private List<Transform> _bodyParts = new List<Transform>();

    public int BodyPartsNb => _bodyParts.Count;

    #region Unity Methods
    protected override void Start()
    {
        base.Start();
        _halfWidth = transform.localScale.x / 2;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    #endregion Unity Methods

    #region Do Actions
    protected override void DoActionNormal()
    {
        base.DoActionNormal();
        if (_pressed)
            SetModeMove();

        if (transform.forward != Vector3.forward)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward), _rotationSpeed * Time.deltaTime);

        MoveBodyParts();
    }

    private void SetModeLost()
    {
        Rigidbody lRigidB = GetComponentInChildren<Rigidbody>();
        lRigidB.isKinematic = false;
        lRigidB.AddForce(transform.forward * _speed, ForceMode.Impulse);

        InvokeOnLost();
        Passive();
    }

    private void SetModeMove()
    {
        DoAction = DoActionMove;
    }

    private void DoActionMove()
    {
        Move();
    }
    #endregion Do Actions

    #region Movements
    public void StartMove()
    {
        SetModeMove();
    }

    private void Move()
    {
        if(!_pressed)
        {
            
            SetModeNormal();
            return;
        }

        Vector3 lPos = transform.position;

        lPos += transform.forward * _speed * Time.deltaTime;


        _angle = Mathf.Clamp(_angle + _sidewaysVector.x * Time.deltaTime, -MAX_ANGLE, MAX_ANGLE);

        if (lPos.x - _halfWidth < -Block.HALF_WIDTH_HEIGHT)
            lPos.x = ClampedXToBlock(true);
        else if (lPos.x + _halfWidth > Block.HALF_WIDTH_HEIGHT)
            lPos.x = ClampedXToBlock(false);


        transform.rotation = Quaternion.AngleAxis(_angle, Vector3.up);

        transform.position = lPos;


        MoveBodyParts();

        _pressed = false;
    }
    private float ClampedXToBlock(bool fromLeft)
    {
        float lNewX;
        int lCoeff = fromLeft ? -1 : 1;
        lNewX = lCoeff * Block.HALF_WIDTH_HEIGHT - lCoeff * _halfWidth;

        _angle = Mathf.MoveTowardsAngle(_angle, 0f, _rotationSpeed * Time.deltaTime);

        return lNewX;
    }

    public void ApplyMoveSideways(Vector2 delta)
    {
        _sidewaysVector = delta * _rotationSpeed;
        _pressed = true;
    }

    #endregion  Movements

    #region Body Parts
    private void MoveBodyParts()
    {
        int lIndex = 0;
        Vector3 lPoint = Vector3.zero;
        Vector3 lMoveDirection = Vector3.zero;
        foreach (Transform bodyPart in _bodyParts)
        {
            if (lIndex == 0)
                lPoint = transform.position;
            else lPoint = _bodyParts[lIndex - 1].position;

            lMoveDirection = lPoint - bodyPart.position;

            // If the length between the current pos of the part and the one its aiming for is superior to the gap we want move it.
            if (lMoveDirection.sqrMagnitude >= _gapBetweenParts * _gapBetweenParts)
                bodyPart.position += lMoveDirection * _speed * Time.deltaTime;

            ++lIndex;
        }
    }
    private void GenerateBodyPart()
    {
        GameObject lBodyPart = ObjectPooler.Instance.GetPooledObject();

        if (lBodyPart == null) return;

        lBodyPart.SetActive(true);

        _bodyParts.Add(lBodyPart.transform);
    }
    #endregion Body Parts

    #region Size Management
    public void IncreaseSize(int increaseNb = 1)
    {
        int lCount = increaseNb;
        int lIndex;

        for (lIndex = 0; lIndex < lCount; ++lIndex)
        {
            GenerateBodyPart();
        }
    }

    public void DecreaseSize(int decreaseNb = 1)
    {
        if (_bodyParts.Count == 0)
        {
            SetModeLost();
            return;
        }

        int lCount = Mathf.Min(decreaseNb, _bodyParts.Count);
        int lIndex;
        Transform lBodyPart;
        for (lIndex = lCount - 1; lIndex >= 0; --lIndex)
        {
            lBodyPart = _bodyParts[_bodyParts.Count - 1];

            _bodyParts.Remove(lBodyPart);
            StartCoroutine(BodyPartDisappearCouroutine(lBodyPart));
        }
    }

    #endregion Size Management

    #region Disappearance
    private IEnumerator BodyPartDisappearCouroutine(Transform bodyPart)
    {
        Transform lPart = bodyPart;
        float lElapsedTime = 0f;
        while (lElapsedTime < _disappearTime)
        {

            lPart.localScale = Vector3.one * _disappearCurve.Evaluate(lElapsedTime / _disappearTime);
            lElapsedTime += Time.deltaTime;
            yield return null;
        }

        if (lPart != null)
        {
            lPart.transform.localScale = Vector3.one;
            lPart.transform.position = Vector3.zero;
            lPart.gameObject.SetActive(false);
        }
    }

    #endregion Disappearance

    #region Events
    private void InvokeOnLost()
    {
        onLost?.Invoke();
    }

    #endregion Events



}
