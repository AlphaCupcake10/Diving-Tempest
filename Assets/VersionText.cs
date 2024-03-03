using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    TMPro.TextMeshPro text;

    void Start()
    {
        text = GetComponent<TMPro.TextMeshPro>();
        text.text = "Version: " + Application.version;
    }
}
