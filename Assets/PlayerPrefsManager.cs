using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private const string versionKey = "Version";
    string currentVersion = "1.0.0"; // Default version

    private void Awake()
    {
        currentVersion = Application.version;
        CheckVersion();
    }

    private void CheckVersion()
    {
        string savedVersion = PlayerPrefs.GetString(versionKey, "");
        
        if (savedVersion != currentVersion)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString(versionKey, currentVersion);
            PlayerPrefs.Save();
        }
    }
}
