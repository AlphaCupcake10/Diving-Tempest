using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsCounter;
    public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames = 0;
    private float fps;
    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }
    void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            fps = (float)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
            if(fpsCounter)
                fpsCounter.text = fps.ToString("00");
        }
    }
}
