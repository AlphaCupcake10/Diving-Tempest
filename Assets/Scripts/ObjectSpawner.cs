using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Transform Target;
    public GameObject Prefab;
    public float RepeatRate = 3;
    public int RepeatLimit = 0;
    
    int currentSpawned = 0;

    void Start()
    {
        InvokeRepeating("Spawn",0,RepeatRate);
    }

    void Spawn()
    {
        if(RepeatLimit != 0)if(currentSpawned>=RepeatLimit)return;
        currentSpawned++;
        Instantiate(Prefab,transform.position,transform.rotation).GetComponent<NPC_AI>().target = Target;//TODO change to function
    }
}
