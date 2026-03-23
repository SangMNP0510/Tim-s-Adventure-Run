using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class BoosterPopupUI : MonoBehaviour
{
    public Button shieldBtn;
    public Button powerBtn;
    public Button bombBtn;

    public CanvasGroup shieldGroup;
    public CanvasGroup powerGroup;
    public CanvasGroup bombGroup;

    public TextMeshProUGUI shieldText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI bombText;

    private int shieldWatchCount;
    private int powerWatchCount;
    private int bombWatchCount;

    private int shieldMax = 3;
    private int powerMax = 2;
    private int bombMax = 4;

    private RewardedAdController ads;

    private const string LAST_RESET_TIME = "LAST_RESET_TIME";

    private void Start()
    {
        ads = FindObjectOfType<RewardedAdController>();

        LoadData();
        CheckReset24h();

        shieldBtn.onClick.AddListener(() => WatchAd("Shield"));
        powerBtn.onClick.AddListener(() => WatchAd("Power"));
        bombBtn.onClick.AddListener(() => WatchAd("Bomb"));

        UpdateUI();
    }

    void CheckReset24h()
    {
        string lastTimeStr = PlayerPrefs.GetString(LAST_RESET_TIME, "");

        if (string.IsNullOrEmpty(lastTimeStr))
        {
            SaveResetTime();
            return;
        }

        DateTime lastTime = DateTime.Parse(lastTimeStr);

        if ((DateTime.Now - lastTime).TotalHours >= 24)
        {
            Debug.Log("Reset lượt ads!");

            shieldWatchCount = 0;
            powerWatchCount = 0;
            bombWatchCount = 0;

            SaveData();
            SaveResetTime();
        }
    }

    void SaveResetTime()
    {
        PlayerPrefs.SetString(LAST_RESET_TIME, DateTime.Now.ToString());
    }

    void WatchAd(string type)
    {
        if (ads == null)
        {
            Debug.LogError("Không tìm thấy RewardedAdController!");
            return;
        }

        if (!CanWatch(type))
        {
            Debug.Log("Đã đạt giới hạn!");
            return;
        }

        GameManager.Instance.PauseForAds();

        ads.ShowRewardedAd(
            () =>
            {
                SkillManager.Instance.AddSkill(type, 1);

                switch (type)
                {
                    case "Shield": shieldWatchCount++; break;
                    case "Power": powerWatchCount++; break;
                    case "Bomb": bombWatchCount++; break;
                }

                SaveData();
                UpdateUI();
            },
            () =>
            {
                Debug.Log("Ads closed → start game");

                StartCoroutine(WaitForAdClosed());
            }
        );
    }

    IEnumerator WaitForAdClosed()
    {
        yield return null;

        while (Application.isFocused == false)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.2f);

        Debug.Log("Ads đã đóng → start game");

        GameManager.Instance.StartGame();
    }

    bool CanWatch(string type)
    {
        switch (type)
        {
            case "Shield": return shieldWatchCount < shieldMax;
            case "Power": return powerWatchCount < powerMax;
            case "Bomb": return bombWatchCount < bombMax;
        }
        return false;
    }

    void UpdateUI()
    {
        shieldText.text = shieldWatchCount + "/" + shieldMax;
        powerText.text = powerWatchCount + "/" + powerMax;
        bombText.text = bombWatchCount + "/" + bombMax;

        UpdateButton(shieldBtn, shieldGroup, shieldWatchCount < shieldMax);
        UpdateButton(powerBtn, powerGroup, powerWatchCount < powerMax);
        UpdateButton(bombBtn, bombGroup, bombWatchCount < bombMax);
    }

    void UpdateButton(Button btn, CanvasGroup group, bool canUse)
    {
        btn.interactable = canUse;

        if (group != null)
        {
            group.alpha = canUse ? 1f : 0.8f;
            group.interactable = canUse;
            group.blocksRaycasts = canUse;
        }
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("ShieldWatch", shieldWatchCount);
        PlayerPrefs.SetInt("PowerWatch", powerWatchCount);
        PlayerPrefs.SetInt("BombWatch", bombWatchCount);
    }

    void LoadData()
    {
        shieldWatchCount = PlayerPrefs.GetInt("ShieldWatch", 0);
        powerWatchCount = PlayerPrefs.GetInt("PowerWatch", 0);
        bombWatchCount = PlayerPrefs.GetInt("BombWatch", 0);
    }
}