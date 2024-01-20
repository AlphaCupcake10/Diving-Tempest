using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Singleton instance
    private static MusicManager _instance;

    // Audio source attached to the MusicManager
    private AudioSource audioSource;

    // List of music clips to be played in sequence
    public AudioClip[] musicClips;

    // Index to keep track of the current music clip
    private int currentClipIndex = 0;

    // Method to get the singleton instance
    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // If no instance exists, try to find one in the scene
                _instance = FindObjectOfType<MusicManager>();

                // If no instance is found, create a new GameObject and attach the MusicManager script
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("MusicManager");
                    _instance = singletonObject.AddComponent<MusicManager>();
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
        // Ensure there is only one instance of the MusicManager
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

            // Start playing the music by default
            StartMusic();
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
            Debug.LogWarning("No music clips assigned to MusicManager.");
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
}
