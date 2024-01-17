using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Telekinesis : MonoBehaviour
{
    PlayerInput input;
    public LayerMask physicsObjects;
    public Vector2 throwDirection;
    public float range = 1;
    public float minGrabDistance = 1;
    public float weightLimit = 30;
    float grabDistance = 1;

    public Rigidbody2D grabbed;
    public bool fullyGrabbed = false;
    public float force;

    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if(input.grabKey)
        {
            GrabNearest();
            return;
        }
        if(grabbed != null)
        {
            if(input.throwKey)
            {
                Throw(true);
            }
            else if(input.grabKey)
            {
                Throw(false);
            }
            else
            {
                UpdateGrabbed();
            }
        }
    }

    private void UpdateGrabbed()
    {
        grabbed.velocity = Vector2.zero;

        grabDistance = minGrabDistance + grabbed.GetComponent<Collider2D>().bounds.extents.magnitude;

        throwDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        throwDirection.Normalize();
        Vector2 TargetPosition = (Vector2)transform.position + throwDirection  * grabDistance;

        Vector2 oldPosition = grabbed.position;
        oldPosition += (TargetPosition-grabbed.position)/10;
        grabbed.position = (oldPosition);

        if(fullyGrabbed && (grabbed.position - (Vector2)transform.position).sqrMagnitude < minGrabDistance*minGrabDistance *.75f)
        {
            Throw(false);
        }
    }

    private void GrabNearest()
    {
        RaycastHit2D[]hits = Physics2D.CircleCastAll(transform.position,range,transform.forward,range,physicsObjects);
        if(hits.Length == 0)return;
        RaycastHit2D closest = hits[0];
        foreach(RaycastHit2D hit in hits)
        {
            if(hit.distance < closest.distance && closest.rigidbody.mass <= weightLimit)
            {
                closest = hit;
            }
        }
        Grab(closest.rigidbody);
    }
    void ResetGrabbedStatus()
    {
        fullyGrabbed = true;
    }
    private void Throw(bool addForce)
    {
        if(addForce)grabbed.AddForce(throwDirection*force);
        grabbed = null;
    }
    private void Grab(Rigidbody2D rigidbody)
    {
        grabbed = rigidbody;
        CancelInvoke("ResetGrabbedStatus");
        fullyGrabbed = false;
        Invoke("ResetGrabbedStatus",.5f);
    }
}
