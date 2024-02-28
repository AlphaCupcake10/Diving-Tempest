using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
        ShadowCaster2D[] shadows = FindObjectsOfType<ShadowCaster2D>();
        foreach(ShadowCaster2D shadow in shadows)
        {
            if(SettingsManager.Instance.ShadowsEnabled)
            {
                shadow.enabled = true;
            }
            else
            {
                shadow.enabled = false;
                Debug.Log(SettingsManager.Instance.ShadowsEnabled);
                Debug.Log(shadow.enabled);
            }
        }
        
        CheckpointManager.Instance.LoadPointFromPrefs();
        player.transform.position = CheckpointManager.Instance.getCheckPointPosition();
        CameraController.Instance.transform.position = player.transform.position;
        DiedScreen.SetActive(false);
    }

    void Update()
    {
        if(DiedScreen.activeSelf)
        {
            if(PlayerInput.Instance.restartKey)
            {
                SceneLoader.Instance.ReloadScene();
            }
        }
    }

    public void RestartScreen()
    {
        Timer.Instance?.SaveTimer();
        TimeManager.Instance.PauseGame();
        DiedScreen.SetActive(true);
    }

}
