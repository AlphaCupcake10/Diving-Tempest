using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    public float MaxHealth = 1;
    public float Health = 1;
    public float ThresholdForce = 10000;
    public float ThresholdForceDamage = 1;

    public UnityEvent onHit;
    public UnityEvent onHeal;
    public UnityEvent onDie;
    public UnityEvent onMaxHealth;

    Rigidbody2D RB;
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }
    public void DeltaHealth(float delta,Vector3 Direction)
    {
        RB?.AddForce((Direction+transform.up*Direction.magnitude));
        DeltaHealth(delta);
    }
    public void DeltaHealth(float delta)
    {
        Health += delta;

        if(delta < 0)
        {
            Debug.Log("Hit");
            onHit.Invoke();
        }
        else if(delta > 0)
        {
            Debug.Log("Heal");
            onHeal.Invoke();
        }
        
        if(Health <= 0)
        {
            Debug.Log("Die");
            Die();
        }
        if(Health >= MaxHealth)
        {
            Debug.Log("Max Health");
            Health = MaxHealth;
            onMaxHealth.Invoke();
        }
    }

    public void Die()
    {
        onDie.Invoke();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D otherRigidbody = collision.collider.GetComponent<Rigidbody2D>();
        if (otherRigidbody != null)
        {
            // Calculate the collision force
            Vector2 collisionForce = collision.relativeVelocity * otherRigidbody.mass;
            // print(collisionForce.magnitude);

            // Log the collision force
            if(collisionForce.sqrMagnitude > ThresholdForce * ThresholdForce)
            {
                DeltaHealth(-ThresholdForceDamage);
            }
        }
    }
}
