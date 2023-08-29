using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent onStart;
    public UnityEvent<Collider2D> onTriggerEnter,onTriggerExit;
    public bool DisableRenderer = true;
    public LayerMask Mask;

    void Start()
    {  
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if(renderer && DisableRenderer)renderer.enabled = false;
        onStart.Invoke();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(( Mask & (1 << col.gameObject.layer)) != 0)
        {
            onTriggerEnter.Invoke(col);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(( Mask & (1 << col.gameObject.layer)) != 0)
        {
            onTriggerExit.Invoke(col);
        }
    }
}
