using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ShootController))]
[RequireComponent(typeof(Pathfinder_AI))]
public class Drone : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public float turnRange = 1;
    public float MovementForce = 20f;

    [Space]

    public bool isDetected = false;
    public float DetectionCheckDelay = 1;
    public float DetectionRange = 10;
    public float ShootRange = 8;

    public float MinRange = 2;
    public float MaxRange = 5;

    public LayerMask RaycastLayer;
    public LayerMask PlayerLayer;

    [Space]

    public Transform ShootPoint;
    public GameObject Explosion;
    Pathfinder_AI AI;
    int spriteIndex = 0;
    Vector2 dir = Vector3.one;
    bool p_isGrabbed = false;
    bool isGrabbed = false;
    Rigidbody2D rb;
    ShootController SC;
    public void SetGrabbedState(bool val)
    {
        isGrabbed = val;
    }

    void Start()
    {
        SC = GetComponent<ShootController>();
        rb = GetComponent<Rigidbody2D>();
        AI = GetComponent<Pathfinder_AI>();
        InvokeRepeating("GetTarget",0,DetectionCheckDelay);
    }

    void GetTarget()
    {
        if(AI.target)return;
        Collider2D player = Physics2D.OverlapCircle(transform.position,DetectionRange,PlayerLayer);
        if(player?.GetComponent<EntityHealth>())
        {
            AI.target = player.transform;
        }
    }

    void FixedUpdate()
    {
        if(AI.target == null)return;
        if(isDetected)
        {
            if(((Vector2)AI.target.position - rb.position).sqrMagnitude < MinRange*MinRange)
            {
                rb.AddForce(-AI.GetDirection()*MovementForce);
            }
            else if(((Vector2)AI.target.position - rb.position).sqrMagnitude > MaxRange*MaxRange)
            {
                rb.AddForce(AI.GetDirection()*MovementForce);
            }
        }
    }

    void Update()
    {
        if(AI.target == null)return;

        float Distance = Vector3.Distance(AI.target.position,transform.position);
        
        if(Distance < DetectionRange && !isDetected)
        {
            RaycastHit2D hit = Physics2D.Raycast(ShootPoint.position,AI.target.position-transform.position,DetectionRange,RaycastLayer);
            if(hit && hit.collider)
            if(hit.collider.transform == AI.target)
                isDetected = true;
        }

        if(isDetected)
        {
            RotateGraphic();
            if(Distance < ShootRange)CallShoot();
        }

        if(p_isGrabbed != isGrabbed)
        {
            p_isGrabbed = isGrabbed;
            SC.rateModifier = (isGrabbed)?4f:1f;
        }
    }

    private void RotateGraphic()
    {
        spriteIndex = sprites.Length/2 + Mathf.RoundToInt((AI.target.position.x - transform.position.x)/turnRange);
        spriteIndex = Mathf.Clamp(spriteIndex,0,sprites.Length-1);
        spriteRenderer.sprite = sprites[spriteIndex];

        dir = AI.target.position-transform.position;
        transform.localScale = new Vector3(Mathf.Sign(dir.x)*((isGrabbed)?-1:1),1,1);
        rb?.MoveRotation(Mathf.Atan2(dir.y*Mathf.Sign(dir.x),Mathf.Abs(dir.x))*Mathf.Rad2Deg);
    }

    private void CallShoot()
    {
        // SC.flipped = ((isGrabbed)?1:-1);

        SC.CallShoot();
    }

    public void Explode()
    {
        Destroy(Instantiate(Explosion,transform.position,transform.rotation),1);
        Destroy(gameObject);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ShootRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, MinRange);
        Gizmos.DrawWireSphere(transform.position, MaxRange);
    }
}
