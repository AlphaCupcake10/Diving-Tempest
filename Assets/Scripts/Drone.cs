using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NPC_AI))]
public class Drone : MonoBehaviour
{
    public LayerMask PassThrough;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public float turnRange = 1;

    [Space]

    public GunConfig config = new GunConfig();
    [System.Serializable]
    public class GunConfig
    {
        public float Force = 1000f;
        public float LifeTime = 1;
        public float FireDelay = 1f;
        public float ChargeDelay = 1f;
    }

    [Space]

    public Transform ShootPoint;
    public GameObject Projectile;
    public GameObject Explosion;
    public UnityEvent OnCharge;
    public UnityEvent OnShoot;


    NPC_AI AI;
    int spriteIndex = 0;
    Vector2 dir;
    bool isGrabbed = false;
    public bool isDetected = false;
    public float DetectionRange = 10;
    public float ShootRange = 8;
    public float FallBackRange = 4;


    public void SetGrabbedState(bool val)
    {
        isGrabbed = val;
    }

    void Start()
    {
        AI = GetComponent<NPC_AI>();
        AI.StopFollowing();
    }


    void Update()
    {
        if(AI.target == null)return;

        float Distance = Vector3.Distance(AI.target.position,transform.position);
        
        if(Distance < DetectionRange && !isDetected)
        {
            RaycastHit2D hit = Physics2D.Raycast(ShootPoint.position,AI.target.position-transform.position,DetectionRange,PassThrough);
            if(hit && hit.collider)
            if(hit.collider.transform == AI.target)
                isDetected = true;
                
        }

        if(isDetected)
        {
            RotateGraphic();
            if(Distance < ShootRange)CallShoot();
            SetFollowingState(Distance > FallBackRange);
        }
    }

    bool isFollowing = false;
    public void SetFollowingState(bool val)
    {
        if(isFollowing == val)return;
        isFollowing = val;
        if(isFollowing)AI.StartFollowing();
        if(!isFollowing)AI.StopFollowing();
    }

    private void RotateGraphic()
    {
        spriteIndex = sprites.Length/2 + Mathf.RoundToInt((AI.target.position.x - transform.position.x)/turnRange);
        spriteIndex = Mathf.Clamp(spriteIndex,0,sprites.Length-1);
        spriteRenderer.sprite = sprites[spriteIndex];

        dir = AI.target.position-transform.position;
        transform.localScale = new Vector3(Mathf.Sign(dir.x)*((isGrabbed)?-1:1),1,1);
        AI?.rb?.MoveRotation(Mathf.Atan2(dir.y*Mathf.Sign(dir.x),Mathf.Abs(dir.x))*Mathf.Rad2Deg);
    }

    float timer = 0;
    private void CallShoot()
    {
        timer+=Time.deltaTime;
        if(timer > config.FireDelay)
        {
            timer = 0;
            OnCharge.Invoke();
            Invoke("Shoot",config.ChargeDelay);
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(Projectile,ShootPoint.position,ShootPoint.rotation);
        Rigidbody2D RB = projectile.GetComponent<Rigidbody2D>();
        RB.AddForce(ShootPoint.right*Mathf.Sign(dir.x)*config.Force / Time.timeScale *((isGrabbed)?-1:1));
        // Destroy(projectile,config.LifeTime); TODO CHANGE
        OnShoot.Invoke();
    }

    public void Explode()
    {
        Destroy(Instantiate(Explosion,transform.position,transform.rotation),1);
        Destroy(gameObject);
    }
}
