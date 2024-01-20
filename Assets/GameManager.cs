using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject DiedScreen;

    public Player player;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        Instance = this;
        
    }

    void Start()
    {
        CheckpointManager.Instance.LoadPointFromPrefs();
        player.transform.position = CheckpointManager.Instance.getCheckPointPosition();
    }

    public void RestartScreen()
    {
        CheckpointManager.Instance.SaveCheckPoint();
        TimeManager.Instance.PauseGame();
        DiedScreen.SetActive(true);
    }

}
