using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject DiedScreen;

    public Player player;

    public Light2D GlobalLight;
    public float OriginalLightIntensity;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        Instance = this;
    }

    void Start()
    {
        OriginalLightIntensity = GlobalLight.intensity;
        UpdateShadows();
        
        CheckpointManager.Instance.LoadPointFromPrefs();
        player.transform.position = CheckpointManager.Instance.getCheckPointPosition();
        CameraController.Instance.transform.position = player.transform.position;
        DiedScreen.SetActive(false);
    }

    void UpdateShadows()
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
        if(PlayerInput.Instance.debugKeys[0])
        {
            SettingsManager.Instance.ShadowsEnabled = !SettingsManager.Instance.ShadowsEnabled;
            UpdateShadows();
        }
        if(PlayerInput.Instance.debugKeys[1])
        {
            if(GlobalLight.intensity == 1)
            {
                GlobalLight.intensity = OriginalLightIntensity;
            }
            else
            {
                GlobalLight.intensity = 1;
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
