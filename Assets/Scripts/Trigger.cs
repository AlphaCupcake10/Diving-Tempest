using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent onStart;
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public bool DisableRenderer = true;

    void Start()
    {  
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if(renderer && DisableRenderer)renderer.enabled = false;
        onStart.Invoke();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        onTriggerEnter.Invoke();
    }
    void OnTriggerExit2D(Collider2D col)
    {
        onTriggerExit.Invoke();
    }

}
