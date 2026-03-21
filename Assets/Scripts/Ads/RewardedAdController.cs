using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RewardedAdController : MonoBehaviour
{
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string _adUnitId = "unused";
#endif

    private RewardedAd _rewardedAd;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        if (_adUnitId == "unused")
        {
            Debug.LogWarning("RewardedAdController: Unsupported platform.");
            return;
        }

        Debug.Log("RewardedAdController: Requesting rewarded ad...");

        AdRequest request = new AdRequest();

        RewardedAd.Load(_adUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError($"RewardedAdController: Failed to load. Reason: {error.GetMessage()}");
                return;
            }

            Debug.Log("RewardedAdController: Rewarded ad loaded.");
            _rewardedAd = ad;

            RegisterEvents(_rewardedAd);
        });
    }

    public void ShowRewardedAd(Action onReward)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("RewardedAdController: Showing rewarded ad.");
            _rewardedAd.Show(reward =>
            {
                Debug.Log($"RewardedAdController: User earned reward: {reward.Amount} {reward.Type}");
                onReward?.Invoke();
            });
        }
        else
        {
            Debug.Log("RewardedAdController: Not ready, loading new one.");
            LoadRewardedAd();
        }
    }

    private void RegisterEvents(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("RewardedAdController: Ad closed. Loading next one.");
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError($"RewardedAdController: Failed to show. Reason: {error.GetMessage()}");
        };
    }

    private void OnDestroy()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
        }
    }
}