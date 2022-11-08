using Com.AndyBastel.Common.Ui;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileScreen : UiScreen
{
    [SerializeField] private Button _touchButton = null;
    private EventTrigger _trigger;
    private EventTrigger.Entry _pointerDownEvent;
    private void OnEnable()
    {
        _trigger = _touchButton.gameObject.AddComponent<EventTrigger>();
        var _pointerDownEvent = new EventTrigger.Entry();
        _pointerDownEvent.eventID = EventTriggerType.PointerDown;
        _pointerDownEvent.callback.AddListener(ScreenTouched);
        _trigger.triggers.Add(_pointerDownEvent);

        //_touchButton.onClick.AddListener(ScreenTouched);
        //_touchButton.onP
    }

    private void ScreenTouched(BaseEventData arg0)
    {
        _trigger.triggers.Remove(_pointerDownEvent);
        Close();
    }
}
