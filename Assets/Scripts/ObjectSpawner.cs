using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Transform Target;
    public GameObject Prefab;
    public float RepeatRate = 3;
    public int TotalSpawns = 0;
    public List<GameObject> spawns;
    public float checkRadius = 5;
    public SpriteRenderer spriteRenderer;


    void Start()
    {
        spriteRenderer.enabled = false;
        InvokeRepeating("Spawn",0,RepeatRate);
    }

    void Spawn()
    {
        spawns.RemoveAll(spawn => spawn == null);
        spawns.RemoveAll(spawn => Vector3.Distance(spawn.transform.position,transform.position) > checkRadius);
        
        if(spawns.Count >= TotalSpawns)return;


        GameObject Spawned = Instantiate(Prefab,transform.position,transform.rotation,transform);
        Spawned?.SetActive(true);
        spawns.Add(Spawned);

        Spawned?.GetComponent<NPC_AI>()?.SetTarget(Target);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
