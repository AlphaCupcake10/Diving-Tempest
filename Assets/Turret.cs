using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Turret : MonoBehaviour
{
    public bool canAim = false;
    public Transform TurretHead;
    public Transform TurretBarrel;

    public GameObject Projectile;
    public GameObject Explosion;
    public UnityEvent OnShoot;
    public float ShootRange = 8;
    public Transform Target;


    public GunConfig config = new GunConfig();
    [System.Serializable]
    public class GunConfig
    {
        public float Force = 6000f;
        public float LifeTime = 1;
        public float FireDelay = 2f;
    }

    Vector3 Direction;

    void Update()
    {
        if(Target == null)return;
        float Distance = Vector3.Distance(Target.position,transform.position);
        if(canAim)RotateBarrel();
        if(Distance < ShootRange) CallShoot();
    }

    void RotateBarrel()
    {
        Direction = Target.position-transform.position;
        TurretHead.rotation = Quaternion.AngleAxis((Mathf.Atan2(Direction.y,Direction.x)*Mathf.Rad2Deg),transform.forward);
    }

    float timer = 0;
    private void CallShoot()
    {
        timer+=Time.deltaTime;
        if(timer > config.FireDelay)
        {
            timer = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(Projectile,TurretBarrel.position,TurretBarrel.rotation);
        Rigidbody2D RB = projectile.GetComponent<Rigidbody2D>();
        RB.AddForce(TurretBarrel.right*config.Force / Time.timeScale);
        // Destroy(projectile,config.LifeTime); TODO CHANGE
        OnShoot.Invoke();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ShootRange);
    }
}
