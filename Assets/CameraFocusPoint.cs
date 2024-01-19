using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusPoint : MonoBehaviour
{
    public int Priority = 10;
    public float Range = 10;
    void Start()
    {
        CameraController.Instance.AddPoint(this);
    }
    void onDisable()
    {
        CameraController.Instance.RemovePoint(this);
    }
    void onDestroy()
    {
        CameraController.Instance.RemovePoint(this);
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
