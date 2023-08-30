using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Cinemachine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{
    //References
    public Animator animator;
    CharacterController2D controller;    
    PlayerInput input;
    Gravity gravity;

    [Header("Camera")]
    public CinemachineVirtualCamera vcam;
    [Range(0,1)]public float SlerpFactor = 0.5f; 

    public AnimatorOverrideController[] controllers;


    void Start()
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController2D>();
        gravity = GetComponent<Gravity>();
    }
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(0,0,gravity.GetGravityAngle()),SlerpFactor);
        
        if(vcam)vcam.m_Lens.Dutch = transform.rotation.eulerAngles.z;

        controller.SetInput(input.MovementAxis.x,input.Jump,input.Crouch);

        if(animator)UpdateAnimations();
    }

    void UpdateAnimations()
    {
        animator.SetBool("isGrounded",controller.GetIsGrounded());
        animator.SetBool("isCrouching",controller.GetIsCrouching());
        animator.SetBool("isSliding",controller.GetIsSliding());
        animator.SetFloat("Speed",Mathf.Abs(input.MovementAxis.x));
        if(!controller.GetIsSliding() && input.MovementAxis.x != 0)transform.localScale = new Vector3(input.MovementAxis.x,1,1);

        if(Input.anyKey)
        {
            if(input.MovementAxis.y > 0)
            {
                animator.runtimeAnimatorController = controllers[0];

                if(Mathf.Abs(input.MovementAxis.y) > 0)
                {
                    animator.runtimeAnimatorController = controllers[1];
                }   
            }
            else if(Input.GetAxisRaw("Vertical") == 0)
            {
                animator.runtimeAnimatorController = controllers[2];
            }
            else
            {
                animator.runtimeAnimatorController = controllers[3];
            }
        }
    }
}
