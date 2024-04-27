using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivacyShower : MonoBehaviour
{
    [SerializeField] private UniWebView _uni;
    private void Start()
    {
        OpenPrivacy();
    }

    public void OpenPrivacy()
    {
        _uni.Load(PlayerPrefs.GetString("link"));
        _uni.Show();
    }
}
