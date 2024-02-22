using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
    public string SIGNIN_URL = "/user/signin";
    public string SIGNUP_URL = "/user/signup";
    public string USER_URL = "/user";
    public string LEADERBOARD_URL = "/leaderboard";

    public string Token = "";

    // Define a class to represent the JSON response structure
    [Serializable]
    public class SignUpResponse
    {
        public string token;
    }
    public class SignInResponse
    {
        public string token;
    }
    public class UserResponse
    {
        public string username;
    }

    void Start()
    {

        Token = PlayerPrefs.GetString("token", ""); // Load the token from PlayerPrefs

        // Usage Example
        HttpService httpService = HttpService.Instance;



        // Set headers
        // Dictionary<string, string> headers = new Dictionary<string, string>();
        // headers.Add("Authorization", Token);


        // GET Request with headers
        // string getResponse = await httpService.GetAsync("/leaderboard", headers);
        // Debug.Log("GET Response:");
        // Debug.Log(getResponse);

        // POST Request with headers
        // string postData = "{\"title\": \"foo\", \"body\": \"bar\", \"userId\": 1}";
        // string postResponse = await httpService.PostAsync("/posts", postData, headers);
        // Debug.Log("\nPOST Response:");
        // Debug.Log(postResponse);

        // // DELETE Request with headers
        // string deleteResponse = await httpService.DeleteAsync("/posts/1", headers);
        // Debug.Log("\nDELETE Response:");
        // Debug.Log(deleteResponse);
        
        // SignUp("username", "password");
    }
    public async void SignUp(string username, string password)
    {
        HttpService httpService = HttpService.Instance;
        string postData = "{\"username\": \"" + username + "\", \"password\": \"" + password + "\"}";
        string postResponse = await httpService.PostAsync(SIGNUP_URL, postData);
        
        Debug.Log(postResponse);

        try
        {
            // Assuming the server returns a JSON response containing a "token" field
            var responseJson = JsonUtility.FromJson<SignUpResponse>(postResponse);
            Token = responseJson.token; // Assign the token value to the Token variable
            PlayerPrefs.SetString("token", Token); // Save the token to PlayerPrefs
        }
        catch (Exception e)
        {
            Debug.LogError("Error parsing JSON response: " + e.Message);
        }
    }
}
