using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public CanvasGroup Fader;
    public float transitionDuration = .5f;
    public static PauseMenu Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Fader.gameObject.SetActive(false);
        LeanTween.alphaCanvas(Fader,0,transitionDuration);
    }

    void Update()
    {
        if (PlayerInput.Instance.pauseKey)
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (Fader.alpha == 0)
        {
            Fader.gameObject.SetActive(true);
            LeanTween.alphaCanvas(Fader,1,transitionDuration).setIgnoreTimeScale(true);
        }
        else
        {
            LeanTween.alphaCanvas(Fader,0,transitionDuration).setOnComplete(()=>Fader.gameObject.SetActive(false)).setIgnoreTimeScale(true);
        }
    }
}
