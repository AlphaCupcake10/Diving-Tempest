using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterConfig", order = 1)]
public class CharacterConfig : ScriptableObject
{
    public float MaxVelocity = 4;
    [Range(1,25)] public float Smoothing = 7;
    [Range(0,1)] public float AirControlCoef = .2f;
    public float JumpHeight = 1;
    [Range(0,1)] public float CrouchSpeedCoef = 0.25f; 
    [Range(.9f,1)] public float SlidingSmoothness = .99f; 
    [Range(0,1)] public float SlideStartThreshold = .7f; 
    [Range(0,1)] public float SlideStopThreshold = .5f; 
    [Range(0,1)] public float SlideJumpStartThreshold = .8f;
    [Range(0,8)] public float SlideJumpSpeedBoost = 2f;
    public float SlideJumpHeight = 2;
    public float CayoteTimeMS = 100;
    public float JumpCooldownMS = 500;
    public float JumpBufferTimeMS = 50;
}
