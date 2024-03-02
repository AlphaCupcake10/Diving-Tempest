using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float startTime;
    public bool timerActive = true;

    public static Timer Instance;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        startTime = Time.time;
        LoadTimer();
        UpdateTimer();
    }

    void Update()
    {
        if (timerActive)
        {
            UpdateTimer();
        }
    }

    float actualTime;

    void UpdateTimer()
    {
        actualTime = Time.time - startTime;

        string minutes = ((int)actualTime / 60).ToString();
        string seconds = (actualTime % 60).ToString("f0");

        timerText.text = minutes.PadLeft(2, '0') + ":" + seconds.PadLeft(2, '0');
    }

    public void SaveTimer()
    {
        PlayerPrefs.SetFloat("Timer", Time.time - startTime);
    }

    public void LoadTimer()
    {
        startTime = Time.time - PlayerPrefs.GetFloat("Timer");
    }

    public void StopTimer()
    {
        if(timerActive)
        {
            timerActive = false;
            UpdateTimer();
        }
    }

    public void ResumeTimer()
    {
        timerActive = true;
        startTime = Time.time - (float.Parse(timerText.text.Split(':')[0]) * 60) - float.Parse(timerText.text.Split(':')[1]);
    }

    public void ResetTimer()
    {
        startTime = Time.time;
        PlayerPrefs.SetFloat("Timer", 0);
    }

    public float GetTime()
    {
        return actualTime;
    }
}
