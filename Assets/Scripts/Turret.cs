using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShootController))]
public class Turret : MonoBehaviour
{
    public bool canAim = false;
    public Transform TurretHead;
    public float ShootRange = 8;
    public Transform Target;
    Vector3 Direction;

    ShootController SC;

    private void Start()
    {
        SC = GetComponent<ShootController>();
    }

    void Update()
    {
        if(Target == null)return;
        float Distance = Vector3.Distance(Target.position,transform.position);
        if(canAim)RotateBarrel();
        if(Distance < ShootRange) SC?.CallShoot();
    }

    void RotateBarrel()
    {
        Direction = Target.position-transform.position;
        TurretHead.rotation = Quaternion.AngleAxis((Mathf.Atan2(Direction.y,Direction.x)*Mathf.Rad2Deg),transform.forward);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ShootRange);
    }
}
