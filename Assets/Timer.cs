using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float startTime;
    private bool timerActive;

    public static Timer Instance;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        startTime = Time.time;
        timerActive = true;
        LoadTimer();
    }

    void Update()
    {
        if (timerActive)
        {
            float t = Time.time - startTime;

            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f0");

            timerText.text = minutes.PadLeft(2, '0') + ":" + seconds.PadLeft(2, '0');
        }
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
        timerActive = false;
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
}
