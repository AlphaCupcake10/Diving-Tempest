using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class NPCMovement : MonoBehaviour
{
    //References
    CharacterController2D controller;    
    Gravity gravity;

    public Transform Graphic;
    

    [Header("Camera")]
    [Range(0,1)]public float SlerpFactor = 0.5f; 

    bool facingRight = true;
    bool isShooting = false;
    bool isDead = false;
    public Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        gravity = GetComponent<Gravity>();
    }
    void Update()
    {
        if(isDead)Destroy(this);
        if(gravity)
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(0,0,gravity.GetGravityAngle()),SlerpFactor);

        controller.SetInput(Vector2.zero,false,false);
        UpdateFlip();
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        animator.SetFloat("XSpeed",Mathf.Abs(controller.GetXSpeed()));
        animator.SetBool("isShooting",isShooting);
        animator.SetBool("isDead",isDead);
    }

    void UpdateFlip()
    {
        if(controller.GetIsWalled())
        {
            Flip(controller.GetIsWalledDir(),controller.GetIsWalledDir(),controller.GetIsGrounded());
        }
        else
        {
            Flip(controller.GetXSpeed(),controller.GetIsWalledDir(),controller.GetIsGrounded());
        }
    }
    void Flip(float X,int isWalledDir , bool isGrounded)
    {
        if((X > 0.05f) && !facingRight)
        {
            facingRight = true;
            Graphic.localScale = new Vector3(1,1,1);
        }
        if((X < -0.05f) && facingRight)
        {
            facingRight = false;
            Graphic.localScale = new Vector3(-1,1,1);
        }
    }
}
