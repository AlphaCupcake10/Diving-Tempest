using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HREF : MonoBehaviour
{
    public string urlToRedirect;

    public void Redirect()
    {
        Application.OpenURL(urlToRedirect);
    }
}
