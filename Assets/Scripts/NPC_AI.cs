using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class NPC_AI : MonoBehaviour
{
    public Transform target;

    public float speed = 20f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    bool FollowTarget = true;

    Seeker seeker;
    [HideInInspector]
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath",0,.1f);
    }

    public void StartFollowing()
    {
        FollowTarget = true;
    }
    public void StopFollowing()
    {
        FollowTarget = false;
    }


    void UpdatePath()
    {
        if(!target)return;
        if(seeker.IsDone())
        {
            seeker.StartPath(rb.position,target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        float distance = Vector2.Distance(rb.position,path.vectorPath[currentWaypoint]);
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 Force = direction * speed * distance;

        if(distance >= .1f)
        rb.AddForce(Force);



        if(distance < nextWaypointDistance && FollowTarget)
        {
            currentWaypoint++;
        }

    }

    public void SetTarget(Transform val)
    {
        target = val;
    }

}
