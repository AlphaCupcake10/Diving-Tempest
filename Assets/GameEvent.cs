using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameEvent:MonoBehaviour
{
    public List<Object> m_object;
    public List<float> m_floats;
    public List<Vector3> m_vector3;
    public void SetGravity(Collider2D col)
    {
        col?.GetComponent<Gravity>()?.SetGravityAngle(m_floats[0]);
    }
    public void AddForce(Collider2D col)
    {
        col?.GetComponent<Rigidbody2D>()?.AddForce(m_vector3[0]);
    }
    public void TeleportObject(Collider2D col)
    {
        col.transform.position = ((GameObject)m_object[0]).transform.position;
    }
}
