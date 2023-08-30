using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ControllerEvents
{
    public UnityEvent onJump;
    public UnityEvent onSpecialJump;
    public UnityEvent onSpecialJumpReady;
    public UnityEvent onSpecialJumpCancel;
    public UnityEvent onLand;
}

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    public CharacterConfig config;

    // REFERENCES
    Rigidbody2D RB;

    [Header("Checks")]
    public LayerMask WhatIsGround;
    public float CheckRadius = .2f;
    public Transform GroundCheckPoint;
    public Transform CeilingCheckPoint;
    
    [Header("Misc")]
    public Collider2D CrouchCollider;
    public ControllerEvents Events;
    
    // INPUT VARIABLES
    float InputX = 0; // stores AD
    bool InputJump = false; //store Jump
    bool InputJumpBuffer = false; //store Jump
    bool InputCrouch = false; //store Crouch
    

    // STATE VARIABLES
    bool isGrounded = false;
    bool p_isGrounded = false;
    bool isGroundedCayote = false;
    bool isSliding = false;
    bool isCrouching = false;
    float jumpTimer = 0;
    float chargeTimer = 0;
    bool isSpecialJumpReady = false;
    bool p_isSpecialJumpReady = false;
    
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    public void SetInput(float _InputMoveX,bool _InputJump,bool _InputCrouch)
    {
        InputX = _InputMoveX;
        InputJump = _InputJump;
        if(InputJump)
        {
            UpdateJumpBuffer();
            CancelInvoke("UpdateJumpBuffer");
        }
        else
        {
            Invoke("UpdateJumpBuffer",config.JumpBufferTimeMS/1000);
        }
        InputCrouch = _InputCrouch;
    }
    void FixedUpdate()
    {
        Move();
    }

    void CheckGroundCeil()
    {
        isCrouching = Physics2D.OverlapCircle(CeilingCheckPoint.position,CheckRadius,WhatIsGround);
        
        if(jumpTimer < config.JumpCooldownMS/1000)return;

        isGrounded = Physics2D.OverlapCircle(GroundCheckPoint.position,CheckRadius,WhatIsGround);
        if(isGrounded && !p_isGrounded)
        {
            Events.onLand.Invoke();
            p_isGrounded = isGrounded;
        }
        else if(isGrounded != p_isGrounded)
        {
            p_isGrounded = isGrounded;
        }

        if(isGrounded)//Delaying Cayote only when false
        {
            UpdateCayote();CancelInvoke("UpdateCayote");
        }
        else
        {
            Invoke("UpdateCayote",config.CayoteTimeMS/1000);
        }

    }
    void UpdateCayote()
    {
        isGroundedCayote = isGrounded;
    }
    void UpdateJumpBuffer()
    {
        InputJumpBuffer = InputJump;
    }
    void Move()
    {
        // Initialization
        CheckGroundCeil();
        if(InputCrouch)isCrouching = true;
        Vector2 velocity = new Vector2(Vector2.Dot(transform.right,RB.velocity),Vector2.Dot(transform.up,RB.velocity));

        if(!isSliding && isCrouching && isGrounded && Mathf.Abs(velocity.x) > config.MaxVelocity * config.SlideStartThreshold)
        {
            isSliding = true;
        }
        if(!isGroundedCayote)
        {
            isSliding = false;
        }

        if(CrouchCollider)CrouchCollider.enabled = !((isCrouching || isSliding) && isGrounded);

        //Handle X movement
        if(isGrounded)
        {
            if(isSliding)
            {    
                velocity.x *= config.SlidingSmoothness;
                if(Mathf.Abs(velocity.x) < config.MaxVelocity * config.SlideStopThreshold)
                {
                    isSliding = false;
                }
            }
            else if(isCrouching)
            {
                velocity.x += (config.MaxVelocity * config.CrouchSpeedCoef * InputX - velocity.x)/config.Smoothing;
            }
            else
            {
                float TargetVelocity = Mathf.Max(Mathf.Abs(velocity.x),config.MaxVelocity);//Don't slow down if going fast
                velocity.x += (TargetVelocity * InputX - velocity.x)/config.Smoothing;
            }
        }
        else//Air Movement
        {
            if(InputX != 0)//To preserve momentum
            {
                float TargetVelocity = Mathf.Max(Mathf.Abs(velocity.x),config.MaxVelocity);//Don't slow down if going fast
                velocity.x += (TargetVelocity * InputX - velocity.x)/config.Smoothing*config.AirControlCoef;
            }
        }

        // JUMPING LOGICS BELOW
        if(!isSpecialJumpReady && isGroundedCayote)
        {
            if(isSliding && Mathf.Abs(velocity.x) <= config.MaxVelocity * config.SlideJumpStartThreshold)
            {
                isSpecialJumpReady = true;
            }
            else if(isCrouching)
            {
                chargeTimer += Time.fixedDeltaTime;
                if(chargeTimer > config.ChargeJumpTimeMS/1000)
                {
                    isSpecialJumpReady = true;
                }
            }
            else
            {
                chargeTimer = 0;
            }
        }
        else
        {
            chargeTimer = 0;
        }
        // if(isGroundedCayote && isCrouching && !isSliding)
        // {
        //     if(chargeTimer >= config.ChargeJumpTimeMS/1000)
        //     {
        //         isChargedJumpReady = true;
        //     }
        //     else
        //     {
        //         isChargedJumpReady = false;
        //         chargeTimer += Time.fixedDeltaTime;
        //     }
        // }
        // else
        // {
        //     isChargedJumpReady = false;
        //     chargeTimer = 0;
        // }
        // if(isGroundedCayote && isSliding && Mathf.Abs(velocity.x) <= config.MaxVelocity * config.SlideJumpStartThreshold)
        // {
        //     isSlideJumpReady = true;
        //     isChargedJumpReady = true;
        //     chargeTimer = config.ChargeJumpTimeMS/1000;
        // }
        // else
        // {
        //     isSlideJumpReady = false;
        // }

        if(p_isSpecialJumpReady != isSpecialJumpReady)
        {
            if(isSpecialJumpReady)Events.onSpecialJumpReady.Invoke();
            else Events.onSpecialJumpCancel.Invoke();
            p_isSpecialJumpReady = isSpecialJumpReady;
        }

        // ACTUAL JUMPING

        jumpTimer += Time.fixedDeltaTime;
        if(isGroundedCayote && InputJumpBuffer && jumpTimer > config.JumpCooldownMS/1000)
        {
            if(isSliding)
            {
                if(isSpecialJumpReady)
                {
                    Events.onSpecialJump.Invoke();
                    velocity.y = Mathf.Sqrt(Mathf.Abs(2*Physics2D.gravity.y*config.SlideJumpHeight));
                    velocity.x = (config.MaxVelocity * config.SlideJumpSpeedBoost * Mathf.Sign(velocity.x));
                    ResetJumpVars();
                }
            }
            else
            {
                if(isSpecialJumpReady)
                {
                    velocity.y = Mathf.Sqrt(Mathf.Abs(2*Physics2D.gravity.y*config.ChargedJumpHeight));
                    chargeTimer = 0;
                    Events.onSpecialJump.Invoke();
                }
                else
                {
                    Events.onJump.Invoke();
                    velocity.y = Mathf.Sqrt(Mathf.Abs(2*Physics2D.gravity.y*config.JumpHeight));
                }
                ResetJumpVars();
            }
        }
        RB.velocity = transform.right * velocity.x + transform.up * velocity.y;
    }
    void ResetJumpVars()
    {
        jumpTimer = 0; 
        isSpecialJumpReady = false;
        isGrounded = false;
        UpdateCayote();
        UpdateJumpBuffer();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(GroundCheckPoint.position, CheckRadius);
        Gizmos.DrawSphere(CeilingCheckPoint.position, CheckRadius);
    }

    public bool GetIsGrounded()
    {
        return isGroundedCayote;
    }
    public bool GetIsCrouching()
    {
        return isCrouching;
    } 
    public bool GetIsSliding()
    {
        return isSliding;
    }
    public float GetSpeed()
    {
        return Vector2.Dot(transform.right,RB.velocity);
    }
}