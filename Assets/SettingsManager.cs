using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager _instance;

    public static SettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SettingsManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(SettingsManager).Name);
                    _instance = singletonObject.AddComponent<SettingsManager>();
                }
            }
            return _instance;
        }
    }

    private void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        AudioManager.Instance?.SetMasterVolume(MasterVolumeSlider);
        AudioManager.Instance?.SetMusicVolume(MusicVolumeSlider);
        AudioManager.Instance?.SetEffectsVolume(EffectsVolumeSlider);


        int touchScheme = TouchScheme;
        if (TouchScheme == 0)
        {
            PlayerInput.Instance.touchScheme = PlayerInput.TouchScheme.JoysticksOnly;
        }
        else if (TouchScheme == 1)
        {
            PlayerInput.Instance.touchScheme = PlayerInput.TouchScheme.Buttons;
        }
    }

    public bool ShadowsEnabled
    {
        get
        {
            return PlayerPrefs.GetInt("ShadowsEnabled", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("ShadowsEnabled", value ? 1 : 0);
        }
    }

    public float MasterVolumeSlider
    {
        get {
            return PlayerPrefs.GetFloat("MasterVolumeSlider", 1);
        }
        set {
            AudioManager.Instance?.SetMasterVolume(value);
            PlayerPrefs.SetFloat("MasterVolumeSlider", value);
        }
    }
    public float MusicVolumeSlider
    {
        get {
            return PlayerPrefs.GetFloat("MusicVolumeSlider", 1);
        }
        set {
            AudioManager.Instance?.SetMusicVolume(value);
            PlayerPrefs.SetFloat("MusicVolumeSlider", value);
        }
    }
    public float EffectsVolumeSlider
    {
        get {
            return PlayerPrefs.GetFloat("EffectsVolumeSlider", 1);
        }
        set {
            AudioManager.Instance?.SetEffectsVolume(value);
            PlayerPrefs.SetFloat("EffectsVolumeSlider", value);
        }
    }
    public int TouchScheme
    {
        get {
            return PlayerPrefs.GetInt("TouchScheme", 0);
        }
        set {
            if (value == 0)
            {
                PlayerInput.Instance.touchScheme = PlayerInput.TouchScheme.JoysticksOnly;
            }
            else if (value == 1)
            {
                PlayerInput.Instance.touchScheme = PlayerInput.TouchScheme.Buttons;
            }
            PlayerPrefs.SetInt("TouchScheme", value);
        }
    }
}
