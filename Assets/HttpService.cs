using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class HttpService : MonoBehaviour
{
    private static HttpService instance;
    private HttpClient httpClient;
    public string baseUrl = "https://jsonplaceholder.typicode.com";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the GameObject persists across scenes
            InitializeHttpClient();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void InitializeHttpClient()
    {
        httpClient = new HttpClient();
    }

    public static HttpService Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("HttpService");
                instance = go.AddComponent<HttpService>();
                DontDestroyOnLoad(go);
                instance.InitializeHttpClient();
            }
            return instance;
        }
    }

    public async Task<string> GetAsync(string endpoint, Dictionary<string, string> headers = null)
    {
        string url = baseUrl + endpoint;
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PostAsync(string endpoint, string data, Dictionary<string, string> headers = null)
    {
        string url = baseUrl + endpoint;
        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = content;

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> DeleteAsync(string endpoint, Dictionary<string, string> headers = null)
    {
        string url = baseUrl + endpoint;
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }
}
