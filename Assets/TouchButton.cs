using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButton : MonoBehaviour
{
    RectTransform rectTransform;
    public event Action OnJoystickDown;
    public event Action OnJoystickUp;
    public List<Touch> touchesInRect;
    bool p_isTouching = false;
    public bool isTouching = false;
    public int touches = 0;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        touchesInRect = new List<Touch>();
        foreach(Touch touch in Input.touches)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, touch.position))
            {
                touchesInRect.Add(touch);
            }
        }

        isTouching = touchesInRect.Count != 0;

        touches = Input.touchCount;

        if(p_isTouching != isTouching)
        {
            p_isTouching = isTouching;
            if (isTouching)
            {
                OnJoystickDown?.Invoke();
            }
            else
            {
                OnJoystickUp?.Invoke();
            }
        }
    }
}
