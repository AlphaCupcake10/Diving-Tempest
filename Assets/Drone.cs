using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NPC_AI))]
public class Drone : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public float turnRange = 1;
    public float stopDistance = 3;
    bool isFollowing = true;
    public Config config = new Config();
    public Transform ShootPoint;
    public GameObject Projectile;
    NPC_AI AI;
    int spriteIndex = 0;
    Vector2 dir;

    bool isGrabbed = false;

    public UnityEvent OnShoot;

    public void SetGrabbedState(bool val)
    {
        isGrabbed = val;
    }

    [System.Serializable]
    public class Config
    {
        public float Force = 1000f;
        public float LifeTime = 1;
        public float fireDelay = .1f;
    }

    void Start()
    {
        AI = GetComponent<NPC_AI>();
    }
    void Update()
    {
        if(AI.target == null)return;

        spriteIndex = sprites.Length/2 + Mathf.RoundToInt((AI.target.position.x - transform.position.x)/turnRange);
        spriteIndex = Mathf.Clamp(spriteIndex,0,sprites.Length-1);
        spriteRenderer.sprite = sprites[spriteIndex];

        dir = AI.target.position-transform.position;
        transform.localScale = new Vector3(Mathf.Sign(dir.x)*((isGrabbed)?-1:1),1,1);
        AI.rb.MoveRotation(Mathf.Atan2(dir.y*Mathf.Sign(dir.x),Mathf.Abs(dir.x))*Mathf.Rad2Deg);
        CallShoot();

        float distanceSqr = Vector3.SqrMagnitude(transform.position-AI.target.position);
        if(isFollowing && distanceSqr < stopDistance * stopDistance)
        {
            AI.StopFollowing();
            isFollowing = false;
        }
        if(!isFollowing && distanceSqr >= stopDistance * stopDistance)
        {
            AI.StartFollowing();
            isFollowing = true;
        }
    }

    float timer = 0;
    private void CallShoot()
    {
        timer+=Time.deltaTime;
        if(timer > config.fireDelay)
        {
            timer = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(Projectile,ShootPoint.position,ShootPoint.rotation);
        Rigidbody2D RB = projectile.GetComponent<Rigidbody2D>();
        RB.AddForce(ShootPoint.right*Mathf.Sign(dir.x)*config.Force*((isGrabbed)?-1:1));
        // Destroy(projectile,config.LifeTime); TODO CHANGE
        OnShoot.Invoke();
    }
}
