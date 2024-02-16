using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]AudioMixerGroup Master;
    [SerializeField]AudioMixerGroup Music;
    [SerializeField]AudioMixerGroup Effects;


    // Singleton instance
    private static AudioManager _instance;

    // Audio source attached to the AudioManager
    private AudioSource audioSource;

    // List of music clips to be played in sequence
    public AudioClip[] musicClips;

    // Index to keep track of the current music clip
    private int currentClipIndex = 0;

    // Method to get the singleton instance
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // If no instance exists, try to find one in the scene
                _instance = FindObjectOfType<AudioManager>();

                // If no instance is found, create a new GameObject and attach the AudioManager script
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("AudioManager");
                    _instance = singletonObject.AddComponent<AudioManager>();
                }
            }

            // Make sure the instance persists between scenes
            DontDestroyOnLoad(_instance.gameObject);

            return _instance;
        }
    }

    // Initialization
    private void Awake()
    {
        // Ensure there is only one instance of the AudioManager
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Check if an AudioSource already exists
            audioSource = GetComponent<AudioSource>();

            // If no AudioSource exists, attach a new one
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.outputAudioMixerGroup = Music;

            // Start playing the music by default
            StartMusic();
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextClip();
        }
    }

    // Method to start playing the music
    public void StartMusic()
    {
        if (musicClips.Length > 0)
        {
            // Set the current music clip
            audioSource.clip = musicClips[currentClipIndex];

            // Play the music
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No music clips assigned to AudioManager.");
        }
    }

    // Method to stop the music
    public void StopMusic()
    {
        audioSource.Stop();
    }

    // Method to play the next music clip in the sequence
    public void PlayNextClip()
    {
        currentClipIndex = (currentClipIndex + 1) % musicClips.Length;
        audioSource.clip = musicClips[currentClipIndex];
        audioSource.Play();
    }
    public void SetMasterVolume(float volume)
    {
        Master.audioMixer.SetFloat("MasterVolume",Mathf.Log10(volume)*20);
    }
    public void SetMusicVolume(float volume)
    {
        Music.audioMixer.SetFloat("MusicVolume",Mathf.Log10(volume)*20);
    }
    public void SetEffectsVolume(float volume)
    {
        Effects.audioMixer.SetFloat("EffectsVolume",Mathf.Log10(volume)*20);
    }
    public AudioMixerGroup GetEffectsGroup()
    {
        return Effects;
    }
}
