using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsInitializer : MonoBehaviour
{
    private static AdsInitializer instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("AdsInitializer: Initializing Mobile Ads...");

        MobileAds.Initialize(initStatus =>
        {
            if (initStatus == null)
            {
                Debug.LogError("AdsInitializer: Initialization failed.");
                return;
            }

            Debug.Log("AdsInitializer: Mobile Ads initialized.");
        });
    }
}