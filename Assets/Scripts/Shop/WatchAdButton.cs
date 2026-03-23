using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchAdButton : MonoBehaviour
{
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClickWatchAd);
    }

    void OnClickWatchAd()
    {
        var ads = FindObjectOfType<RewardedAdController>();

        if (ads == null)
        {
            Debug.LogError("Không tìm thấy RewardedAdController!");
            return;
        }

        ads.ShowRewardedAd(
            () =>
            {
                Debug.Log("Nhận 50 coins");
                CoinManager.Instance.AddCoins(50);
            },
            () =>
            {
                Debug.Log("Ads đóng");

                StartCoroutine(DelayAfterAd());
            }
        );
    }

    IEnumerator DelayAfterAd()
    {
        yield return null;

        while (!Application.isFocused)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.2f);

        Debug.Log("Safe sau ads");
    }
}