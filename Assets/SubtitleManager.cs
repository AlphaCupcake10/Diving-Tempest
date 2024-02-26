using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleText;

    public static SubtitleManager Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void DisplaySubtitle(string subtitle) {
        subtitleText.text = subtitle;
        CancelInvoke("ClearSubtitle");
        Invoke("ClearSubtitle", 10f);
    }
    public void ClearSubtitle() {
        subtitleText.text = "";
    }
    public void Start()
    {
        ClearSubtitle();
    }
}
