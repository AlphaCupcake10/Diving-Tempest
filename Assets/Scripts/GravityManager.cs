using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance;
    public List<Gravity> Entities;
    public float GlobalAngle;

    void Awake()
    {
        Instance = this;
    }

    public void AddEntity(Gravity gravity)
    {
        Entities.Add(gravity);
    }
    public void RemoveEntity(Gravity gravity)
    {
        Entities.Remove(gravity);
    }
    
    void FixedUpdate()
    {
        foreach (Gravity Entity in Entities)
        {
            Rigidbody2D RB = Entity.GetRB();
            float Angle = (GlobalAngle+Entity.GetGravityAngle()+90)*Mathf.Deg2Rad;
            RB.AddForce(new Vector2(Mathf.Cos(Angle),Mathf.Sin(Angle))*RB.mass*Physics2D.gravity.y);
        }
    }
    
}
