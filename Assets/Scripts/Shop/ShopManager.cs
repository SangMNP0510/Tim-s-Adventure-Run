using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopItemUI shopItemPrefab;
    public Transform content;

    public Sprite[] itemIcons;

    void Start()
    {
        CreateCoinPack(itemIcons[0], "1000 Coins", "$1", true, false);
        CreateCoinPack(itemIcons[1], "5000 Coins", "$1", true, false);
        CreateCoinPack(itemIcons[2], "10000 Coins", "$1", false, true);

        CreateRemoveAds(itemIcons[3], "Remove Ads", "$1");

        CreateWatchAd(itemIcons[4], "50 Coins");
    }

    void CreateCoinPack(Sprite icon, string title, string price, bool sale, bool best)
    {
        ShopItemUI item = Instantiate(shopItemPrefab, content);
        item.SetupCoinPack(icon, title, price, sale, best);
    }

    void CreateRemoveAds(Sprite icon, string title, string price)
    {
        ShopItemUI item = Instantiate(shopItemPrefab, content);
        item.SetupRemoveAds(icon, title, price);
    }

    void CreateWatchAd(Sprite icon, string title)
    {
        ShopItemUI item = Instantiate(shopItemPrefab, content);
        item.SetupWatchAd(icon, title);
    }
}