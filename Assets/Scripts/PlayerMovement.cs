using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{
    //References
    public Animator animator;
    CharacterController2D controller;    
    Gravity gravity;

    [Header("Camera")]
    public CinemachineVirtualCamera vcam;
    [Range(0,1)]public float SlerpFactor = 0.5f; 


    // STATE VARS
    bool _InputJump;
    bool _InputCrouch;
    Vector2 _InputAxis;


    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        gravity = GetComponent<Gravity>();
    }
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(0,0,gravity.GetGravityAngle()),SlerpFactor);
        
        if(vcam)vcam.m_Lens.Dutch = transform.rotation.eulerAngles.z;

        GetInput();
        if(animator)UpdateAnimations();
    }
    void GetInput()
    {
        _InputJump = Input.GetButton("Jump");
        _InputCrouch = Input.GetButton("Crouch");
        _InputAxis = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        if(Input.GetKey(KeyCode.LeftShift))
            controller.SetInput(Vector2.zero,_InputJump,_InputCrouch);
        else
            controller.SetInput(_InputAxis,_InputJump,_InputCrouch);
    }
    void UpdateAnimations()
    {
        animator.SetBool("isGrounded",controller.GetIsGrounded());
        animator.SetBool("isCrouching",controller.GetIsCrouching());
        animator.SetBool("isSliding",controller.GetIsSliding());
        animator.SetFloat("Speed",Mathf.Abs(_InputAxis.x) * (Input.GetKey(KeyCode.LeftShift)?0:1));
        if(!controller.GetIsSliding() && _InputAxis.x != 0)transform.localScale = new Vector3(_InputAxis.x,1,1);
    }
}
