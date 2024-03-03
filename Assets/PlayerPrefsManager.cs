using UnityEditor;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private const string versionKey = "Version";

    private void Awake()
    {
        CheckVersion();
    }

    private void CheckVersion()
    {
        string savedVersion = PlayerPrefs.GetString(versionKey, "");
        string currentVersion = GetAppVersion();

        if (savedVersion != currentVersion)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString(versionKey, currentVersion);
            PlayerPrefs.Save();
        }
    }

    private string GetAppVersion()
    {
        // Retrieve the app version from Player Settings
        return PlayerSettings.bundleVersion;
    }
}
