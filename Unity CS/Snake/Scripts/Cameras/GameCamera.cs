using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Transform _camera = null;
    [SerializeField] private Vector3 _positionOffset = Vector3.zero;
    [SerializeField] private float _pitchRotationOffset = 10f;
    [SerializeField, Range(0f, 1f)] private float _smoothCoeff = 0.1f;
    [SerializeField] private float _initialDistanceFromTarget = 5f;
    [SerializeField] private float _maxDistanceFromTarget = 30f;
    [SerializeField] private float _bodyPartOffsetDist = 2.5f;
    [SerializeField] private float _bodyPartTransitionTime = 0.5f;
    [SerializeField] private AnimationCurve _bodyPartTransitionCurve = default;

    // Target
    private Transform _target;
    private float _distanceFromTarget = 0f;

    private Vector3 _velocity;

    public Transform Target 
    { 
        get => _target; 
        set
        {
            _target = value;
            transform.position = _target.position;
        }
    }

    public float DistanceFromTarget 
    { 
        get => _distanceFromTarget; 
        set
        {
            _distanceFromTarget = value;
            PositionCamera();
        }
    }

    private void Awake()
    {
        if (_camera == null) 
            _camera = GetComponentInChildren<Transform>();

        _positionOffset.Normalize();
        _distanceFromTarget = _initialDistanceFromTarget;
        PositionCamera();
    }

    // Update is called once per frame at the end of it
    void LateUpdate()
    {
        if (Target != null) FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 lTargetPosition = Target.position;
        transform.position = Vector3.SmoothDamp(transform.position, lTargetPosition, ref _velocity, _smoothCoeff);
    }

    private void PositionCamera()
    {
        _camera.localPosition = _positionOffset * DistanceFromTarget;
        _camera.localRotation = Quaternion.LookRotation(-_camera.localPosition) * Quaternion.AngleAxis(_pitchRotationOffset, transform.right);
    }

    public void IncreaseDistance(int partGained = 1)
    {
        StartCoroutine(ChangeDistanceCoroutine(partGained, true));
    }
    public void DecreaseDistance(int partLost = 1)
    {
        StartCoroutine(ChangeDistanceCoroutine(partLost, false));
    }

    private IEnumerator ChangeDistanceCoroutine(int partGained, bool isIncreasing)
    {
        int lCoeff = isIncreasing ? 1 : -1;
        float lStartDistance = _distanceFromTarget;
        float lTargetDistance = _distanceFromTarget + lCoeff * partGained * _bodyPartOffsetDist;
        lTargetDistance = Mathf.Clamp(lTargetDistance, _initialDistanceFromTarget, _maxDistanceFromTarget);
        float lElapsedTime = 0f;
        while (this && lElapsedTime < _bodyPartTransitionTime)
        {
            _distanceFromTarget = Mathf.Lerp(lStartDistance, lTargetDistance, _bodyPartTransitionCurve.Evaluate(lElapsedTime / _bodyPartTransitionTime));
            PositionCamera();

            lElapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }


}
