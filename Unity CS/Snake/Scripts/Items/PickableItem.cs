using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _rotationSpeed = 45f;

    [Header("Disappearance")]
    [SerializeField] private float _timeToDisappear = 0.5f;
    [SerializeField] private AnimationCurve _disappearCurve = default;

    [Header("Parameters")]
    [SerializeField] private float _offsetY = 0.5f;

    [Header("Effects")]
    [SerializeField] private Transform _particlesPrefab = null;
    [SerializeField] private List<string> _hitSoundsNames = null;

    protected Vector3 _initialScale = Vector3.zero;

    #region Unity Methods
    // Start is called before the first frame update
    virtual protected void Start()
    {
        transform.position += Vector3.up * _offsetY;
        _initialScale = transform.localScale;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(SnakeController.TAG))
        {
            CollideWithSnake(other.ClosestPoint(transform.position));
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        transform.rotation = Quaternion.AngleAxis(_rotationSpeed * Time.deltaTime, Vector3.up) * transform.rotation;
    }
    #endregion Unity Methods

    #region Collision
    virtual protected void CollideWithSnake(Vector3 contactPos)
    {
        StartCoroutine(DisappearCoroutine());
        PlayFX(contactPos);
    }

    #endregion Collision

    #region Effects
    private void PlayFX(Vector3 pos)
    {
        Transform _particles = Instantiate(_particlesPrefab);
        _particles.position = pos;

        PlayRandomSound();
    }

    private void PlayRandomSound()
    {
        int lInd = UnityEngine.Random.Range(0, _hitSoundsNames.Count);
        string lSoundName = _hitSoundsNames[lInd];

        SoundManager.Instance.Play(lSoundName);
    }

    #endregion

    #region Disappearance
    private IEnumerator DisappearCoroutine()
    {
        float lElapsedTime = 0f;

        while (lElapsedTime < _timeToDisappear)
        {
            transform.localScale = _initialScale * _disappearCurve.Evaluate(lElapsedTime / _timeToDisappear);
            lElapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);

    }

    #endregion Disappearance
}
