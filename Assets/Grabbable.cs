using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grabbable : MonoBehaviour
{
    public UnityEvent OnGrabEvent;
    public UnityEvent OnThrowEvent;
    public bool autoAlign = false;
    public float offsetAngle = 0;
    public void OnGrab()
    {
        OnGrabEvent.Invoke();
    }
    public void OnThrow()
    {   
        OnThrowEvent.Invoke();
    }
    
}
