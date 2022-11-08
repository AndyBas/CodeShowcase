using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Events
    public event Action<Vector2> onFingerMoved;
    public event Action onFirstScreenTouch;

    // Fields
    private TouchController _touchController;
    private bool _pressed = false;

    private Vector2 _firstTouchViewportPos = -Vector2.one;

    private Camera _camera;
    public Camera GameCamera { get => _camera; set => _camera = value; }



    private void Awake()
    {
        _touchController = new TouchController();
        _touchController.Touch.TouchPress.started += TouchPress_started;
        _touchController.Touch.TouchPress.started += TouchPress_CheckFirstScreenTouch;
        _touchController.Touch.TouchPress.canceled += TouchPress_canceled;
    }

    private void OnEnable()
    {
        _touchController.Enable();
    }

    private void OnDisable()
    {
        _touchController.Disable();
    }


    private void Update()
    {
        CheckTouchScreen();
    }

    private void CheckTouchScreen()
    {
        Vector2 lDelta;
        Vector2 lTouchPosViewport;


        if (_pressed)
        {
            lTouchPosViewport = _camera.ScreenToViewportPoint(_touchController.Touch.TouchPosition.ReadValue<Vector2>());
            if (_firstTouchViewportPos == -Vector2.one) _firstTouchViewportPos = lTouchPosViewport;

            lDelta = lTouchPosViewport - _firstTouchViewportPos;
            InvokeOnFingerMoved(lDelta);
        }
        else if (!_pressed && _firstTouchViewportPos != -Vector2.one)
            _firstTouchViewportPos = -Vector2.one;
    }

    private void TouchPress_CheckFirstScreenTouch(InputAction.CallbackContext context)
    {
        _touchController.Touch.TouchPress.started -= TouchPress_CheckFirstScreenTouch;

        InvokeOnFirstScreenTouch();
    }
    private void TouchPress_started(InputAction.CallbackContext context)
    {
        _pressed = true;
    }

    private void TouchPress_canceled(InputAction.CallbackContext context)
    {
        _pressed = false;
    }

    #region Events Handling
    private void InvokeOnFingerMoved(Vector2 delta)
    {
        onFingerMoved?.Invoke(delta);
    }

    private void InvokeOnFirstScreenTouch()
    {
        onFirstScreenTouch?.Invoke();
    }
    #endregion


    private void OnDestroy()
    {

        _touchController.Touch.TouchPress.started   -= TouchPress_started;
        _touchController.Touch.TouchPress.started   -= TouchPress_CheckFirstScreenTouch;
        _touchController.Touch.TouchPress.canceled  -= TouchPress_canceled;
    }
}
