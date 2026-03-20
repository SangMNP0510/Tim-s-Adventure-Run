using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text titleText;

    public GameObject badge50;
    public GameObject badgeBest;

    public GameObject watchText;
    public GameObject watchAdButton;

    public GameObject priceButton;
    public TMP_Text priceText;

    public void SetupCoinPack(Sprite itemIcon, string title, string price, bool sale, bool best)
    {
        icon.sprite = itemIcon;
        titleText.text = title;

        priceButton.SetActive(true);
        watchAdButton.SetActive(false);
        watchText.SetActive(false);

        priceText.text = price;

        badge50.SetActive(sale);
        badgeBest.SetActive(best);
    }

    public void SetupRemoveAds(Sprite itemIcon, string title, string price)
    {
        icon.sprite = itemIcon;
        titleText.text = title;

        priceButton.SetActive(true);
        watchAdButton.SetActive(false);
        watchText.SetActive(false);

        priceText.text = price;

        badge50.SetActive(false);
        badgeBest.SetActive(false);
    }

    public void SetupWatchAd(Sprite itemIcon, string title)
    {
        icon.sprite = itemIcon;
        titleText.text = title;

        priceButton.SetActive(false);
        watchAdButton.SetActive(true);
        watchText.SetActive(true);

        badge50.SetActive(false);
        badgeBest.SetActive(false);
    }
}