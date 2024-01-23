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

    public AnimationCurve curve;

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
        CancelTween();
        SetTimeScale(0.01f);
    }

    public void ResumeGame()
    {
        CancelTween();
        SetTimeScale(1f);
    }

    public void SlowMotion(float slowFactor,float duration)
    {
        onSlowMotion.Invoke();
        CancelTween();
        StartTween(slowFactor,duration);
    } 
    public void StopSlowMotion()
    {
        CancelTween();
        SetTimeScale(1f);
    }
    LTDescr Tween;
    void StartTween(float slowFactor,float duration)
    {
        Tween = LeanTween.value(gameObject, SetTimeScale, slowFactor, 1f, duration);
    }
    void CancelTween()
    {
        if(Tween != null)LeanTween.cancel(Tween.id);
    }

    void OnDestroy()
    {
        ResumeGame();
    }
}
