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
    CharacterController2D controller;    
    PlayerInput input;
    Gravity gravity;

    public Transform Graphic;

    [Header("Camera")]
    public CinemachineVirtualCamera[] vcams;
    [Range(0,1)]public float SlerpFactor = 0.5f; 

    bool movementEnabled = true;
    bool facingRight = true;
    public Animator animator;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController2D>();
        gravity = GetComponent<Gravity>();
    }
    void Update()
    {
        if(gravity)
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(0,0,gravity.GetGravityAngle()),SlerpFactor);
        
        foreach(CinemachineVirtualCamera vcam in vcams)
            vcam.m_Lens.Dutch = transform.rotation.eulerAngles.z;

        if(movementEnabled)
        {
            // if(gravity?.GetGravityAngle() == 180)
                // controller.SetInput(new Vector2(-input.MovementAxis.x,input.MovementAxis.y),input.Jump,input.Crouch);
            // else
                controller.SetInput(input.MovementAxis,input.Jump,input.Crouch);
        }
        else
        {
            controller.SetInput(Vector2.zero,false,false);
        }
        UpdateFlip();
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        // Debug.Log("help");
        animator.SetFloat("XSpeed",Mathf.Abs(controller.GetXSpeed()));
        animator.SetFloat("YSpeed",controller.GetYSpeed());
        animator.SetBool("isCrouching",controller.GetIsCrouching());
        animator.SetBool("isSliding",controller.GetIsSliding());
        animator.SetBool("isGrounded",controller.GetIsGrounded());
        animator.SetBool("isWalled",controller.GetIsWalled());
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
    
    public void DisableMovement()
    {
        CancelInvoke("EnableMovement");
        movementEnabled = false;
    }
    public void EnableMovement()
    {
        movementEnabled = true;
    }
    public void DisableMovement(float For)
    {
        movementEnabled = false;
        Invoke("EnableMovement",For);
    }
}
