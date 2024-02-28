
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider EffectsVolumeSlider;

    public TMP_Dropdown InputTypeDropdown;

    public Toggle ShadowsToggle;

    [DllImport("__Internal")]
    private static extern void QuitGame();

    void Start()
    {
        MasterVolumeSlider.value = SettingsManager.Instance.MasterVolumeSlider;
        MusicVolumeSlider.value = SettingsManager.Instance.MusicVolumeSlider;
        EffectsVolumeSlider.value = SettingsManager.Instance.EffectsVolumeSlider;

        InputTypeDropdown.value = SettingsManager.Instance.TouchScheme;

        ShadowsToggle.isOn = SettingsManager.Instance.ShadowsEnabled;
    }

    public void SetMasterVolume(float volume)
    {
        SettingsManager.Instance.MasterVolumeSlider = volume;
    }
    public void SetMusicVolume(float volume)
    {
        SettingsManager.Instance.MusicVolumeSlider = volume;
    }
    public void SetEffectsVolume(float volume)
    {
        SettingsManager.Instance.EffectsVolumeSlider = volume;
    }
    public void SetTouchScheme(int type)
    {
        SettingsManager.Instance.TouchScheme = type;
    }
    public void SetShadow(bool value)
    {
        SettingsManager.Instance.ShadowsEnabled = value;
    }
    public void TriggerQuit () {
    #if UNITY_WEBGL == true && UNITY_EDITOR == false
        QuitGame ();
    #endif
    }
}
