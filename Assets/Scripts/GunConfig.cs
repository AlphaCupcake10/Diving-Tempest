using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Gun", order = 1)]
public class GunConfig : ScriptableObject
{
    [Serializable]
    public class Burst
    {
        public int Count = 3;
        public float Delay = 0.1f;
    }

    public Burst burst;
    public float Force = 6000f;
    public float FireRate = 0.5f;
    [Range(0, 1)] public float Accuracy = 0.5f;
    public GameObject Projectile;
}