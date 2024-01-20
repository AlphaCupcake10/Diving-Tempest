using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Telekinesis : MonoBehaviour
{
    public GameObject Crosshair;
    public LayerMask physicsObjects;
    public float range = 1;
    public float autoAimRadius = 1;
    public float minGrabDistance = 1.5f;
    public float weightLimit = 30;
    public float force = 10000;
    [Space]
    public Rigidbody2D grabbed;

    float grabDistance = 1;
    PlayerInput input;
    Vector2 throwDirection;


    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if(input.grabKey && grabbed == null)
        {
            GrabNearest();
            return;
        }
        if(grabbed != null)
        {
            if(!Crosshair.activeInHierarchy)
                Crosshair.SetActive(true);
            UpdateGrabbed();
            if(input.throwKey)
            {
                Throw(true);
            }
            if(input.grabKey)
            {
                Throw(false);
            }
        }
        else
        {
            if(Crosshair.activeInHierarchy)
                Crosshair.SetActive(false);
        }
    }

    private void UpdateGrabbed()
    {
        grabbed.velocity = Vector2.zero;

        grabDistance = minGrabDistance + grabbed.GetComponent<Collider2D>().bounds.extents.magnitude;

        throwDirection = AutoAim(GetWorldPositionOnPlane(Input.mousePosition,0)-transform.position).normalized;

        Vector2 TargetPosition = (Vector2)transform.position + throwDirection  * grabDistance;

        Crosshair.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(throwDirection.y,throwDirection.x)*Mathf.Rad2Deg,Crosshair.transform.forward);

        Vector2 oldPosition = grabbed.position;
        oldPosition += (TargetPosition-grabbed.position)/2;
        grabbed.position = (oldPosition);
    }
    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        Vector3 MousePoint = ray.GetPoint(distance);
        return MousePoint;
    }
    public Vector3 AutoAim(Vector3 MousePoint)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(MousePoint,autoAimRadius,Vector2.zero,autoAimRadius,physicsObjects);
        if(hits.Length == 0)return MousePoint;
        RaycastHit2D closest = hits[0];
        foreach(RaycastHit2D hit in hits)
        {
            if(closest.distance > hit.distance)closest = hit;
        }
        return closest.collider.transform.position;
    }
    private void GrabNearest()
    {
        RaycastHit2D[]hits = Physics2D.CircleCastAll(transform.position+(GetWorldPositionOnPlane(Input.mousePosition,0)-transform.position).normalized/2,range,transform.forward,range,physicsObjects);
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
    private void Throw(bool addForce)
    {
        if(addForce)grabbed.AddForce(throwDirection*force / Time.timeScale);
        grabbed?.GetComponent<Drone>()?.SetGrabbedState(false);
        grabbed = null;
    }
    private void Grab(Rigidbody2D rigidbody)
    {
        grabbed = rigidbody;
        rigidbody?.GetComponent<Drone>()?.SetGrabbedState(true);
    }
}
