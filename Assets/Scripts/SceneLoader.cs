using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public Image Fader;
    public float transitionDuration = .5f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Fader.gameObject.SetActive(true);
        LeanTween.alpha(Fader.rectTransform,1,.001f);    
        LeanTween.alpha(Fader.rectTransform,0,transitionDuration);    
    }
    void LoadSceneUsingManager()
    {
        print("Starting to Load");
        SceneManager.LoadScene(sceneToLoad);
    }
    int sceneToLoad = 0;
    public void LoadScene(int index)
    {
        sceneToLoad = index;
        LeanTween.alpha(Fader.rectTransform,1,transitionDuration).setOnComplete(LoadSceneUsingManager).setIgnoreTimeScale(true);
    }
    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
