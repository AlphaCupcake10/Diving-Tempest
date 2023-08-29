using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Gravity : MonoBehaviour
{
    [SerializeField]
    float Angle;
    Rigidbody2D RB;
    float defaultGravityScale = 1;
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        defaultGravityScale = RB.gravityScale;
        RB.gravityScale = 0;
        GravityManager.Instance.AddEntity(this);
    }

    public void SetGravityAngle(float val)
    {
        Angle = val;
    }
    public float GetGravityAngle()
    {
        return Angle;
    }
    public Rigidbody2D GetRB()
    {
        return RB;
    }
    void onDisable()
    {
        GravityManager.Instance.RemoveEntity(this);
        RB.gravityScale = defaultGravityScale;
    }
}
