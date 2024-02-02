using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform Target;

    public float Damping = 10;

    // Update is called once per frame
    void Update()
    {
        if(!Target)return;
        transform.position += (Target.position - transform.position)/Damping;
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }
}
