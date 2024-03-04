using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public CameraFocusPoint Player;
    public List<CameraFocusPoint> Points;

    void Awake()
    {
        Instance = this;
    }

    public void AddPoint(CameraFocusPoint gravity)
    {
        if(Points.Contains(gravity))return;
        Points.Add(gravity);
    }
    public void RemovePoint(CameraFocusPoint gravity)
    {
        if(!Points.Contains(gravity))return;
        Points.Remove(gravity);
    }
    
    void FixedUpdate()
    {
        if(Points.Count==0)return;
        Vector3 avg = Vector3.zero;
        int count = 0;
        count += Player.Priority;
        avg += Player.transform.position * Player.Priority;
        foreach(CameraFocusPoint point in Points)
        {
            if(point == null)continue;
            if(point == Player)continue;
            if(Vector2.Distance(Player.transform.position,point.transform.position) > point.Range)continue;
            count += point.Priority;
            avg += point.transform.position * point.Priority;
        }
        avg /= count;
        transform.position = avg;
    }
}
