using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootController : MonoBehaviour
{
    bool autoFire = false;
    public Transform ShootPoint;
    public GunConfig config;
    public float ChargeDelay = .2f;

    [Serializable]
    public class Events
    {
        public UnityEvent OnCharge;
        public UnityEvent OnShoot;
    }

    [Space] public Events ShootEvents;

    private float lastFireTime;
    private int burstCount;

    [HideInInspector]
    public float rateModifier = 1f;

    void Start()
    {
        lastFireTime = Time.time;
    }

    public void CallShoot()
    {
        if (Time.time - lastFireTime > 1 / (config.FireRate*rateModifier))
        {
            lastFireTime = Time.time;
            Fire();
        }
    }

    void Update()
    {
        if (autoFire)
        {
            CallShoot();
        }
    }

    private void Fire()
    {
        // Handle burst firing
        if (config.burst.Count >= 1)
        {
            StartCoroutine(BurstFire());
        }
    }

    private IEnumerator BurstFire()
    {
        burstCount = config.burst.Count;

        ShootEvents.OnCharge.Invoke();
        yield return new WaitForSeconds(ChargeDelay);

        while (burstCount > 0)
        {
            Shoot();
            burstCount--;
            yield return new WaitForSeconds(config.burst.Delay);
        }
    }

    private void Shoot()
    {
        // Apply accuracy
        float spread = 1f - config.Accuracy;
        Vector3 direction = ShootPoint.right;

        if (spread > 0f)
        {
            // Apply random spread to the shooting direction
            float randomSpreadX = UnityEngine.Random.Range(-spread, spread);
            float randomSpreadY = UnityEngine.Random.Range(-spread, spread);
            direction += new Vector3(randomSpreadX, randomSpreadY, 0f);
            direction.Normalize();
        }

        // Instantiate the projectile with the adjusted direction
        GameObject projectile = Instantiate(config.Projectile, ShootPoint.position, Quaternion.LookRotation(Vector3.forward, direction));
        Rigidbody2D RB = projectile.GetComponent<Rigidbody2D>();
        RB.AddForce(direction * config.Force * Mathf.Sign(transform.localScale.x)/Time.timeScale);
        ShootEvents.OnShoot.Invoke();
    }

    public void SetAutoFire(bool value)
    {
        autoFire = value;
    }
}
