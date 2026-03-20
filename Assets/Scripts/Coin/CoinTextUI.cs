using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinTextUI : MonoBehaviour
{
    private TMP_Text coinText;

    void Start()
    {
        coinText = GetComponent<TMP_Text>();

        if (CoinManager.Instance != null)
        {
            UpdateCoin(CoinManager.Instance.CurrentCoins);

            CoinManager.Instance.OnCoinChanged += UpdateCoin;
        }
    }

    void UpdateCoin(int amount)
    {
        coinText.text = amount.ToString();
    }

    void OnDestroy()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.OnCoinChanged -= UpdateCoin;
        }
    }
}