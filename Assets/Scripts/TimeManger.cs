using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    // Current time scale (1 for normal speed, <1 for slow motion, 0 for pause)
    private float currentTimeScale = 1f;

    // Original fixed delta time, used to reset it after slow motion
    private float originalFixedDeltaTime;

    public UnityEvent onSlowMotion;

    void Start()
    {
        Instance = this;
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void SetTimeScale(float newTimeScale)
    {
        currentTimeScale = Mathf.Max(0f, newTimeScale); // Ensure time scale is not negative
        Time.timeScale = currentTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime * currentTimeScale;
    }

    public void PauseGame()
    {
        SetTimeScale(0.01f);
    }

    public void ResumeGame()
    {
        SetTimeScale(1f);
    }

    public void SlowMotion(float slowFactor)
    {
        SetTimeScale(slowFactor);
        onSlowMotion.Invoke();
    }
    void OnDestroy()
    {
        ResumeGame();
    }
}
