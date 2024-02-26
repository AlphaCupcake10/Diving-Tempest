using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusPoint : MonoBehaviour
{
    public int Priority = 10;
    public float Range = 10;
    void OnEnable()
    {
        CameraController.Instance?.AddPoint(this);
    }
    void Start()
    {
        CameraController.Instance?.AddPoint(this);
    }
    void OnDisable()
    {
        CameraController.Instance?.RemovePoint(this);
    }
    void OnTransformParentChanged()
    {
        CameraController.Instance?.RemovePoint(this);
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
