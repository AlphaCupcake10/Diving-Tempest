using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkController : MonoBehaviour
{
    public string LEADERBOARD_URL = "/leaderboard";
    public TextMeshProUGUI response;

    Timer timer;

    void Start()
    {
        timer = Timer.Instance;

        // GET Request with headers
        // string getResponse = await httpService.GetAsync("/leaderboard", headers);
        // Debug.Log("GET Response:");
        // Debug.Log(getResponse);

        // // DELETE Request with headers
        // string deleteResponse = await httpService.DeleteAsync("/posts/1", headers);
        // Debug.Log("\nDELETE Response:");
        // Debug.Log(deleteResponse);
        
        // SignUp("username", "password");
    }
    public async void UploadTime()
    {
        // POST Request with headers
        // Usage Example
        HttpService httpService = HttpService.Instance;

        // Set headers
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", HttpService.Instance.token);
        string TIME = timer.GetTime().ToString("0");
        string postData = "{\"time\": \""+TIME+"\"}";
        string postResponse = await httpService.PostAsync(LEADERBOARD_URL, postData, headers);
        response.text = postResponse;
    }
}
