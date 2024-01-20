using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    public int CheckpointIndex = 0;

    void Awake()
    {
        Instance = this;
    }
    public void setCheckPoint(int index)
    {
        if(index < CheckpointIndex)return;
        CheckpointIndex = index;
    }

    public Vector3 getCheckPointPosition()
    {
        return transform.GetChild(CheckpointIndex).position;
    }
    
    public void SaveCheckPoint()
    {
        PlayerPrefs.SetInt("CheckpointIndex",CheckpointIndex);
    }
    public void LoadPointFromPrefs()
    {
        CheckpointIndex = PlayerPrefs.GetInt("CheckpointIndex",CheckpointIndex);
    }
}
