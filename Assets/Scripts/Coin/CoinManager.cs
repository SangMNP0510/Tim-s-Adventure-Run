using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int CurrentCoins { get; private set; }

    public event Action<int> OnCoinChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CurrentCoins = PlayerPrefs.GetInt("Coins", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        CurrentCoins += amount;

        PlayerPrefs.SetInt("Coins", CurrentCoins);
        PlayerPrefs.Save();

        OnCoinChanged?.Invoke(CurrentCoins);
    }

    public void SpendCoins(int amount)
    {
        CurrentCoins -= amount;

        if (CurrentCoins < 0)
            CurrentCoins = 0;

        PlayerPrefs.SetInt("Coins", CurrentCoins);
        PlayerPrefs.Save();

        OnCoinChanged?.Invoke(CurrentCoins);
    }
}