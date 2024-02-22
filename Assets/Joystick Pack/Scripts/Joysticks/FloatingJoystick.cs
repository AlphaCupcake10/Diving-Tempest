using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public delegate void JoystickDownAction();
    public event JoystickDownAction OnJoystickDown;

    public delegate void JoystickUpAction();
    public event JoystickUpAction OnJoystickUp;

    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
        InvokeJoystickDownAction();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
        InvokeJoystickUpAction();
    }

    private void InvokeJoystickDownAction()
    {
        if (OnJoystickDown != null)
        {
            OnJoystickDown.Invoke();
        }
    }

    private void InvokeJoystickUpAction()
    {
        if (OnJoystickUp != null)
        {
            OnJoystickUp.Invoke();
        }
    }
}
