using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckAll : MonoBehaviour
{
    [SerializeField] private ConfigData _allConfigData;

    private bool _showTerms = true;
    private const string sceneName = "SampleScene";
    private const string privacyName = "Privacy";

    private void Awake()
    {
        StartLoading();
    }
    private void StartLoading()
    {
        string HtmlText = GetHtmlFromUri("http://google.com");

        if (HtmlText != "")
        {
            LoadFirebaseConfig();
        }

        else
        {
            LoadScene();
        }
    }
    void LoadScene()
    {
        if (_showTerms)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(privacyName);
        }
    }
    public void LoadFirebaseConfig()
    {
        CheckRemoteConfigValues();
    }
    public Task CheckRemoteConfigValues()
    {
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            LoadScene();
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            LoadScene();
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
          .ContinueWithOnMainThread(
            task => {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");

                foreach (var item in remoteConfig.AllValues)
                {
                    switch (item.Key)
                    {
                        case "url":
                            {
                                _allConfigData.Url = item.Value.StringValue;
                                break;
                            }
                        case "showAgree":
                            {
                                _allConfigData.ShowTerms = item.Value.BooleanValue;
                                break;
                            }
                    }
                }

                _showTerms = _allConfigData.ShowTerms;
                Debug.Log(_showTerms + "/" + _allConfigData.ShowTerms);
                PlayerPrefs.SetString("link", _allConfigData.Url);
                LoadScene();
            });

    }
    public string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to 
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }
}
[Serializable]
public class ConfigData
{
    public string Url;
    public bool ShowTerms;
}