using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterConfig", order = 1)]
public class CharacterConfig : ScriptableObject
{
    [Header("Basic")]
    public float MaxVelocity = 4;
    public float JumpHeight = 1;
    [Range(1,25)] public float Smoothing = 7;
    [Range(0,1)] public float AirControlCoef = .2f;
    [Range(0,1)] public float CrouchSpeedCoef = 0.25f; 
    [Range(0,1)] public float MaxVelocityDampingCoef = .95f;


    [Header("Sliding Thresholds")]
    [Range(.9f,1)] public float SlidingSmoothness = .99f; 
    [Range(0,1)] public float SlideStartThreshold = .7f; 
    [Range(0,1)] public float SlideStopThreshold = .5f; 
    [Range(0,1)] public float SlideJumpStartThreshold = .8f;

    [Header("Special Jumps")]
    [Range(0,8)] public float SlideJumpSpeedRatio = 2f;
    [Range(0,4)] public float SlideJumpHeightRatio = 2;

    [Range(0,4)] public float ChargedJumpHeightRatio = 2.5f;
    [Range(0,1)] public float ImperfectRatio = .5f;

    [Header("Timings")]
    public float PerfectDelayMS = 100; 
    public float PerfectWindowMS = 100; 
    public float ChargeJumpTimeMS = 1000;
    public float CayoteTimeMS = 100;
    public float JumpCooldownMS = 500;
    public float JumpBufferTimeMS = 50;


    public float GetAirTime(float jumpHeight)
    {
        return Mathf.Sqrt(Mathf.Abs(8*jumpHeight/Physics2D.gravity.y));
    }
}
