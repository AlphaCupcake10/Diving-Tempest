using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ControllerEvents
{
    public UnityEvent onJump;
    public UnityEvent onSlide;
    public UnityEvent onSpecialJump;
    public UnityEvent onSpecialJumpReady;
    public UnityEvent onSpecialJumpCancel;
    public UnityEvent onLand;
    public UnityEvent onSuperSonic;
    public UnityEvent onSuperSonicCancel;
}

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    public CharacterConfig config;

    // REFERENCES
    Rigidbody2D RB;

    [Header("Checks")]
    public LayerMask WhatIsGround;
    public LayerMask WhatIsWall;
    public float CheckRadius = .2f;
    public Transform GroundCheckPoint;
    public Transform CeilingCheckPoint;
    public Transform WallCheckPointLeft;
    public Transform WallCheckPointRight;
    
    [Header("Misc")]
    public Collider2D CrouchCollider;
    public ControllerEvents Events;
    
    // INPUT VARIABLES
    Vector2 InputMove; // stores AD
    bool InputJump = false; //store Jump
    bool InputJumpBuffer = false; //store Jump
    bool InputCrouch = false; //store Crouch
    

    // STATE VARIABLES
    Vector2 velocity;
    bool isGrounded = false;
    bool p_isGrounded = false;
    bool isGroundedCayote = false;
    bool isSliding = false;
    bool isCrouching = false;
    float jumpTimer = 0;
    float chargeTimer = 0;
    float perfectTime = 0;
    bool isSpecialJumpReady = false;
    bool p_isSpecialJumpReady = false;
    bool isSuperSonic = false;
    bool p_isSuperSonic = false;

    bool isWalledLeft = false;
    bool isWalledRight = false;
    
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    public void SetInput(Vector2 _InputMove,bool _InputJump,bool _InputCrouch)
    {
        InputMove = _InputMove;
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
        if(config == null)return;
        CheckForSuperSonic();
        Move();
    }

    void CheckForSuperSonic()
    {
        isSuperSonic = RB.velocity.sqrMagnitude > config.MaxVelocity*config.MaxVelocity;
        
        if(p_isSuperSonic != isSuperSonic)
        {
            if(isSuperSonic)
            {
                Events.onSuperSonic.Invoke();
            }
            else
            {
                Events.onSuperSonicCancel.Invoke();
            }
            p_isSuperSonic = isSuperSonic;
        }
    }

    void WallCheck()
    {
        if(WallCheckPointLeft) isWalledLeft = Physics2D.OverlapCircle(WallCheckPointLeft.position,CheckRadius,WhatIsWall) /*&& (Mathf.Sign(velocity.x) * InputMove.x == 1)*/;
        if(WallCheckPointRight) isWalledRight = Physics2D.OverlapCircle(WallCheckPointRight.position,CheckRadius,WhatIsWall) /*&& (Mathf.Sign(velocity.x) * InputMove.x == 1)*/;
    }

    void CheckGroundCeil()
    {
        if(CeilingCheckPoint)isCrouching = Physics2D.OverlapCircle(CeilingCheckPoint.position,CheckRadius,WhatIsGround);
        
        // if(jumpTimer < config.JumpCooldownMS/1000)return; WHY WAS THIS HERE???

        if(GroundCheckPoint)isGrounded = Physics2D.OverlapCircle(GroundCheckPoint.position,CheckRadius,WhatIsGround);
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
        WallCheck();
        if(InputCrouch)isCrouching = true;
        velocity = new Vector2(Vector2.Dot(transform.right,RB.velocity),Vector2.Dot(transform.up,RB.velocity));

        if(!isSliding && isCrouching && isGrounded && Mathf.Abs(velocity.x) > config.MaxVelocity * config.SlideStartThreshold)
        {
            isSliding = true;
            Events.onSlide.Invoke();
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
                velocity.x /= config.SlidingSmoothing;
                if(Mathf.Abs(velocity.x) < config.MaxVelocity * config.SlideStopThreshold)
                {
                    isSliding = false;
                }
            }
            else if(isCrouching)
            {
                velocity.x += (config.MaxVelocity * config.CrouchSpeedCoef * InputMove.x - velocity.x)/config.Smoothing;
            }
            else
            {
                float TargetVelocity = Mathf.Max(Mathf.Abs(velocity.x)*config.MaxVelocityDampingCoef,config.MaxVelocity);//Don't slow down if going fast
                velocity.x += (TargetVelocity * InputMove.x - velocity.x)/config.Smoothing;
            }
        }
        else//Air Movement
        {
            if(InputMove.x != 0)//To preserve momentum
            {
                float TargetVelocity = Mathf.Max(Mathf.Abs(velocity.x),config.MaxVelocity);//Don't slow down if going fast
                velocity.x += (TargetVelocity * InputMove.x - velocity.x)/config.Smoothing*config.AirControlCoef;
            }
        }
        bool willBonk = false;
        if(CeilingCheckPoint) willBonk = Physics2D.OverlapCircle(CeilingCheckPoint.position,CheckRadius,WhatIsGround);

        // JUMPING LOGICS BELOW
        if(!isSpecialJumpReady && isGroundedCayote && !willBonk)
        {
            if(isSliding && Mathf.Abs(velocity.x) <= config.MaxVelocity * config.SlideJumpStartThreshold)
            {
                isSpecialJumpReady = true;
                perfectTime = Time.time + config.PerfectDelayMS/1000;
            }
            else if(isCrouching)
            {
                chargeTimer += Time.fixedDeltaTime;
                if(chargeTimer > config.ChargeJumpTimeMS/1000)
                {
                    isSpecialJumpReady = true;
                    perfectTime = Time.time + config.PerfectDelayMS/1000;
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

        if(isSpecialJumpReady && !isCrouching && !isSliding)
        {
            chargeTimer = 0;
            isSpecialJumpReady = false;
        }

        if(p_isSpecialJumpReady != isSpecialJumpReady)
        {
            if(isSpecialJumpReady)Events.onSpecialJumpReady.Invoke();
            else Events.onSpecialJumpCancel.Invoke();
            p_isSpecialJumpReady = isSpecialJumpReady;
        }

        // ACTUAL JUMPING

        jumpTimer += Time.fixedDeltaTime;
        if(isGroundedCayote && InputJumpBuffer && jumpTimer > config.JumpCooldownMS/1000 && !willBonk)
        {
            if(isSliding)
            {
                if(isSpecialJumpReady)
                {
                    if(Mathf.Abs(Time.time-perfectTime) <= config.PerfectWindowMS/1000)
                    {
                        velocity.y = Mathf.Sqrt(Mathf.Abs(2*Physics2D.gravity.y*config.JumpHeight*config.SlideJumpHeightRatio));
                        velocity.x = (config.MaxVelocity * config.SlideJumpSpeedRatio * Mathf.Sign(velocity.x));
                        Events.onSpecialJump.Invoke();
                    }
                    else
                    {
                        velocity.y = Mathf.Sqrt(Mathf.Abs(2*Physics2D.gravity.y*config.JumpHeight*config.SlideJumpHeightRatio*config.ImperfectRatio));
                        velocity.x = (config.MaxVelocity * config.SlideJumpSpeedRatio * Mathf.Sign(velocity.x)*config.ImperfectRatio);
                    }
                    ResetJumpVars();
                }
            }
            else
            {
                if(isSpecialJumpReady)
                {
                    if(Mathf.Abs(Time.time-perfectTime) <= config.PerfectWindowMS/1000)
                    {
                        velocity.y = Mathf.Sqrt(Mathf.Abs(2*Physics2D.gravity.y*config.JumpHeight*config.ChargedJumpHeightRatio));
                        Events.onSpecialJump.Invoke();
                    }
                    else
                    {
                        velocity.y = Mathf.Sqrt(Mathf.Abs(2*Physics2D.gravity.y*config.JumpHeight*config.ChargedJumpHeightRatio*config.ImperfectRatio));
                    }
                    chargeTimer = 0;
                }
                else
                {
                    Events.onJump.Invoke();
                    velocity.y = Mathf.Sqrt(Mathf.Abs(2*Physics2D.gravity.y*config.JumpHeight));
                }
                ResetJumpVars();
            }
        }

        //Wall Logic
        else if(isWalledLeft || isWalledRight)
        {
            if(velocity.y <= 0)
            {
                if(InputCrouch)
                    velocity.y*=Mathf.Clamp01(config.WallSlideFactor*1.85f);
                else
                    velocity.y*=config.WallSlideFactor;
            }
            if(/*InputJumpBuffer*/ ((isWalledLeft && InputMove.x > 0) || (isWalledRight && InputMove.x < 0)) && jumpTimer > config.JumpCooldownMS/1000 && !willBonk)
            {
                Vector2 jumpVector = Vector2.zero; 
                if(!InputCrouch)
                {
                    jumpVector.y = Mathf.Sqrt(Mathf.Abs(2*Physics2D.gravity.y*config.JumpHeight*config.WallJumpHeightRatio/2));
                }
                jumpVector.x = (config.MaxVelocity * config.WallJumpSpeedRatio * (isWalledLeft?1:-1));
                velocity = jumpVector;
                ResetJumpVars();
            }
            else if(InputJumpBuffer && jumpTimer > config.JumpCooldownMS/1000 && !willBonk)
            {
                Vector2 jumpVector = Vector2.zero; 
                jumpVector.y = Mathf.Sqrt(Mathf.Abs(2*Physics2D.gravity.y*config.JumpHeight*config.WallJumpHeightRatio));
                jumpVector.x = (config.MaxVelocity * config.WallJumpSpeedRatio * (isWalledLeft?1:-1));
                velocity = jumpVector;
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
        if(GroundCheckPoint)Gizmos.DrawSphere(GroundCheckPoint.position, CheckRadius);
        if(CeilingCheckPoint)Gizmos.DrawSphere(CeilingCheckPoint.position, CheckRadius);
        if(WallCheckPointLeft)Gizmos.DrawSphere(WallCheckPointLeft.position, CheckRadius);
        if(WallCheckPointRight)Gizmos.DrawSphere(WallCheckPointRight.position, CheckRadius);
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
    public bool GetIsWalled()
    {
        return (isWalledLeft || isWalledRight) && !isGroundedCayote;
    }
    public float GetXSpeed()
    {
        return Vector2.Dot(transform.right,RB.velocity);
    }
    public float GetYSpeed()
    {
        return Vector2.Dot(transform.up,RB.velocity);
    }
    public int GetIsWalledDir()
    {
        if(!isWalledLeft && !isWalledRight || isWalledLeft && isWalledRight)
        {
            return 0;
        }
        if(isWalledLeft)return -1;
        if(isWalledRight)return 1;
        return 0;
    }
}