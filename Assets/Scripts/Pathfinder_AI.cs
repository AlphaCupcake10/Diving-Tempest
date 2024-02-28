using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class Pathfinder_AI : MonoBehaviour
{
    public Transform target;
    public float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    public float  Distance
    {
        get;
        private set;
    }

    Seeker seeker;
    [HideInInspector]

    // Start is called before the first frame update
    void Awake()
    {
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath",0,.1f);
    }

    void UpdatePath()
    {
        if(!target)return;
        if(seeker.IsDone())
        {
            seeker.StartPath(transform.position,target.position, OnPathComplete);
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
            return;
        }

        Distance = Vector2.Distance(transform.position,path.vectorPath[currentWaypoint]);

        if(Distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    public Vector2 GetDirection()
    {
        if(path == null)
            return Vector2.zero;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            return Vector2.zero;
        }

        return ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;
    }

    public void SetTarget(Transform val)
    {
        target = val;
    }

}
