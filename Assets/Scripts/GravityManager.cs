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
            RB.AddForce(new Vector2(Mathf.Cos((GlobalAngle+Entity.Angle+90)*Mathf.Deg2Rad),Mathf.Sin((GlobalAngle+Entity.Angle+90)*Mathf.Deg2Rad))*RB.mass*Physics2D.gravity.y);
        }
    }
    
}
