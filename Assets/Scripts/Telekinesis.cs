using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Telekinesis : MonoBehaviour
{
    public GameObject Crosshair;
    public LayerMask physicsObjects;
    public LayerMask parryAble;
    public float range = 1;
    public float minGrabDistance = 1.5f;
    public float weightLimit = 30;
    public float force = 10000;
    public float recoilMultiplier = 1.5f;
    public float slowMotionDuration = 5f;
    public float coolDownTime = .1f;
    [Space]
    public Rigidbody2D grabbed;
    public GameObject friendlyBullet;
    [Space]
    public UnityEvent OnGrab;
    public UnityEvent OnThrow;


    Rigidbody2D RB;
    float grabDistance = 1;
    PlayerInput input;
    Vector2 throwDirection;
    CharacterController2D controller;


    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        input = PlayerInput.Instance;
        RB = GetComponent<Rigidbody2D>();
        Cursor.lockState = CursorLockMode.Confined;
    }

    bool tryingToGrab = false;
    bool p_grabKey = false;
    bool isOnCooldown = false;

    void ClearCooldown()
    {
        isOnCooldown = false;
    }
    void Update()
    {
        if(controller.GetIsGrounded())
        {
            SetSlowMotionState(false);
        }

        if(p_grabKey != input.grabKey)
        {
            p_grabKey = input.grabKey;
            tryingToGrab = input.grabKey;
        }

        if(tryingToGrab && grabbed == null && !isOnCooldown)
        {
            GrabNearest();
            return;
        }
        else if(grabbed != null)
        {
            if(!Crosshair.activeInHierarchy)
                Crosshair.SetActive(true);
            UpdateGrabbed();
            if(input.throwKey)
            {
                Throw(true);
            }
            if(tryingToGrab)
            {
                tryingToGrab = false;
                Throw(false);
            }
        }
        else
        {
            SetSlowMotionState(false);
            if(Crosshair.activeInHierarchy)
                Crosshair.SetActive(false);
        }
    }

    private void UpdateGrabbed()
    {
        grabbed.velocity = Vector2.zero;

        grabDistance = minGrabDistance + grabbed.GetComponent<Collider2D>().bounds.extents.magnitude;

        if(input.inputType == PlayerInput.InputType.Touch || input.inputType == PlayerInput.InputType.Controller)
        {
            throwDirection += input.AimAxis/5;
        }
        else
        {
            throwDirection += ((Vector2)GetWorldPositionOnPlane(Input.mousePosition,0)-RB.position)/10;
        }
        
        throwDirection.Normalize();
        Crosshair.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(throwDirection.y,throwDirection.x)*Mathf.Rad2Deg,Crosshair.transform.forward);

        Vector2 TargetPosition = (Vector2)transform.position + throwDirection  * grabDistance;
        grabbed.position = (TargetPosition);
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
        if(grabbed == null)return;
        bool wasSlowMotion = isSlowMotion;
        SetSlowMotionState(false);
        if(addForce)
        {
            OnThrow.Invoke();
            if(wasSlowMotion)RB.velocity = Vector2.zero;
            if((parryAble.value & (1 << grabbed.gameObject.layer)) != 0 && !controller.GetIsGrounded())
            {
                RB.AddForce(-throwDirection*force*recoilMultiplier / Time.timeScale);
            }
            else if((parryAble.value & (1 << grabbed.gameObject.layer)) == 0)
            {
                RB.AddForce(-throwDirection*force / Time.timeScale);
            }
            grabbed.AddForce(throwDirection*force / Time.timeScale);
        }
        grabbed?.GetComponent<Drone>()?.SetGrabbedState(false);
        grabbed.GetComponent<Collider2D>().enabled = true;
        grabbed = null;

        isOnCooldown = true;
        CancelInvoke("ClearCooldown");
        Invoke("ClearCooldown",coolDownTime);
    }
    private void Grab(Rigidbody2D rigidbody)
    {
        if(!controller.GetIsGrounded() && (parryAble.value & (1 << rigidbody.gameObject.layer)) != 0)
        {
            SetSlowMotionState(true);
        }
        rigidbody = ConvertProjectile(rigidbody);
        OnGrab.Invoke();
        grabbed = rigidbody;
        throwDirection = (grabbed.position - RB.position).normalized;
        rigidbody?.GetComponent<Drone>()?.SetGrabbedState(true);
        tryingToGrab = false;
    }

    Rigidbody2D ConvertProjectile(Rigidbody2D rigidbody)
    {
        if(rigidbody.GetComponent<Projectile>() != null)
        {
            GameObject newBullet = Instantiate(friendlyBullet,rigidbody.position,rigidbody.transform.rotation);
            Destroy(rigidbody.gameObject);
            rigidbody = newBullet.GetComponent<Rigidbody2D>();
            rigidbody.GetComponent<Collider2D>().enabled = false;
        }
        return rigidbody;
    }

    bool isSlowMotion = false;
    void SetSlowMotionState(bool val)
    {
        if(isSlowMotion != val)
        {
            isSlowMotion = val;
            if(val)TimeManager.Instance.SlowMotion(.05f,slowMotionDuration);
            else TimeManager.Instance.StopSlowMotion();
        }
    }
}
