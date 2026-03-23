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

    private Action _onReward;
    private Action _onClose;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        if (_adUnitId == "unused")
        {
            Debug.LogWarning("Unsupported platform.");
            return;
        }

        AdRequest request = new AdRequest();

        RewardedAd.Load(_adUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError("Load fail: " + error.GetMessage());
                return;
            }

            _rewardedAd = ad;

            RegisterEvents(_rewardedAd);
        });
    }

    public void ShowRewardedAd(Action onReward, Action onClose)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("Show ads");

            _onReward = onReward;
            _onClose = onClose;

            _rewardedAd.OnAdFullScreenContentClosed -= HandleAdClosed;
            _rewardedAd.OnAdFullScreenContentClosed += HandleAdClosed;

            _rewardedAd.Show(reward =>
            {
                Debug.Log("Reward received");
                _onReward?.Invoke();
            });
        }
        else
        {
            Debug.Log("Ad not ready");

            LoadRewardedAd();

            // fallback
            _onReward?.Invoke();
            _onClose?.Invoke();
        }
    }

    private void HandleAdClosed()
    {
        Debug.Log("Ad CLOSED");

        _onClose?.Invoke();

        _rewardedAd.OnAdFullScreenContentClosed -= HandleAdClosed;

        LoadRewardedAd();
    }

    private void RegisterEvents(RewardedAd ad)
    {
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Show fail: " + error.GetMessage());
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