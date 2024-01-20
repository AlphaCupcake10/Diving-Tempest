using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject Trail;
    public float Damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(Trail,1);
        transform.DetachChildren();
        Destroy(gameObject);

        col?.collider?.GetComponent<EntityHealth>()?.DeltaHealth(Damage,GetComponent<Rigidbody2D>().velocity);
    }
}
