using System;
using System.Collections.Generic;
using UnityEngine;

public class FloatingJoystick : MonoBehaviour
{
    public event Action OnJoystickDown;
    public event Action OnJoystickUp;

    RectTransform rectTransform;
    public RectTransform StickRect;
    public RectTransform HandleRect;

    public float HandleRange = 50;

    public float Horizontal { get { return HandleRect.anchoredPosition.x / HandleRange; } }
    public float Vertical { get { return HandleRect.anchoredPosition.y / HandleRange; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    bool p_isTouching = false;
    bool isTouching = false;

    public int touches = 0;

    public List<Touch> touchesInRect;
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
                StickRect?.gameObject.SetActive(true);
                StickRect.position = touchesInRect[0].position;
                OnJoystickDown?.Invoke();
            }
            else
            {
                StickRect?.gameObject.SetActive(false);
                HandleRect.anchoredPosition = Vector2.zero;
                OnJoystickUp?.Invoke();
            }
        }
        if (isTouching)
        {
            StickRect?.gameObject.SetActive(true);
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, touchesInRect[0].position))
            {
                HandleRect.localPosition = Vector2.ClampMagnitude(((Vector3)touchesInRect[0].position - StickRect.position), HandleRange);
            }
        }
    }
}
