
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider EffectsVolumeSlider;
    [DllImport("__Internal")]
    private static extern void QuitGame();

    public void SetMasterVolume(float volume)
    {
        AudioManager.Instance?.SetMasterVolume(volume);
    }
    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance?.SetMusicVolume(volume);
    }
    public void SetEffectsVolume(float volume)
    {
        AudioManager.Instance?.SetEffectsVolume(volume);
    }
    public void TriggerQuit () {
    #if UNITY_WEBGL == true && UNITY_EDITOR == false
        QuitGame ();
    #endif
    }
}
