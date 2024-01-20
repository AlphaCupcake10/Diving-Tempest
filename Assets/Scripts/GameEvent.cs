using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameEvent:MonoBehaviour
{
    public Object m_object;
    public float m_float;
    public Vector3 m_vector3;
    public void SetGravity(Collider2D col)
    {
        col?.GetComponent<Gravity>()?.SetGravityAngle(m_float);
    }
    public void SetCheckPoint()
    {
        CheckpointManager.Instance.setCheckPoint((int)m_float);
    }
    public void AddForce(Collider2D col)
    {
        col?.GetComponent<Rigidbody2D>()?.AddForce(m_vector3);
    }
    public void AddForce(Rigidbody2D RB)
    {
        RB?.AddForce(m_vector3);
    }
    public void TeleportObject(Collider2D col)
    {
        Rigidbody2D rb = ((GameObject)m_object).GetComponent<Rigidbody2D>();
        if(rb)rb.velocity = Vector2.zero;
        col.transform.position = ((GameObject)m_object).transform.position;
    }
    public void DeltaHealth(Collider2D col)
    {
        col?.GetComponent<EntityHealth>()?.DeltaHealth(m_float);
    }
    public void DeltaHealth(EntityHealth health)
    {
        health?.DeltaHealth(m_float);
    }
}
