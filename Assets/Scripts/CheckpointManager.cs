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
        SaveCheckPoint();
    }

    public Vector3 getCheckPointPosition()
    {
        return transform.GetChild(CheckpointIndex).position;
    }
    
    public void SaveCheckPoint()
    {
        Timer.Instance?.SaveTimer();
        PlayerPrefs.SetInt("CheckpointIndex",CheckpointIndex);
        if(CheckpointIndex == 0)
        {
            Timer.Instance?.ResetTimer();
        }
    }
    public void LoadPointFromPrefs()
    {
        CheckpointIndex = PlayerPrefs.GetInt("CheckpointIndex",CheckpointIndex);
    }
}
