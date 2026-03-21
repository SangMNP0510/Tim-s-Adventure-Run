using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RewardUIHandler : MonoBehaviour
{
    public GameObject popup;
    public Button openPopupButton;
    public Button watchAdsButton;

    private const string LAST_AD_TIME_KEY = "LastRewardAdTime";

    private void Start()
    {
        CheckCooldown();

        watchAdsButton.onClick.AddListener(OnClickWatchAds);
    }

    void CheckCooldown()
    {
        if (!PlayerPrefs.HasKey(LAST_AD_TIME_KEY))
        {
            openPopupButton.gameObject.SetActive(true);
            return;
        }

        long lastTimeTicks = Convert.ToInt64(PlayerPrefs.GetString(LAST_AD_TIME_KEY));
        DateTime lastTime = new DateTime(lastTimeTicks);

        TimeSpan diff = DateTime.Now - lastTime;

        if (diff.TotalHours >= 24)
        {
            openPopupButton.gameObject.SetActive(true);
        }
        else
        {
            openPopupButton.gameObject.SetActive(false);
        }
    }

    void OnClickWatchAds()
    {
        var ads = FindObjectOfType<RewardedAdController>();

        if (ads == null)
        {
            Debug.LogError("Không tìm thấy RewardedAdController!");
            return;
        }

        ads.ShowRewardedAd(() =>
        {
            // 🎯 1. Cộng coin
            CoinManager.Instance.AddCoins(50);

            // 🎯 2. Ẩn popup
            popup.SetActive(false);

            // 🎯 3. Lưu thời gian
            PlayerPrefs.SetString(LAST_AD_TIME_KEY, DateTime.Now.Ticks.ToString());
            PlayerPrefs.Save();

            // 🎯 4. Ẩn button trong 24h
            openPopupButton.gameObject.SetActive(false);

            Debug.Log("Đã nhận 50 coin + cooldown 24h");
        });
    }
}