using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    public List<AchievementData> achievements = new List<AchievementData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else Destroy(gameObject);
    }

    void Init()
    {
        achievements = new List<AchievementData>()
        {
            new AchievementData{ id="kill", name="Kill 20 enemies", target=20, reward=100 },
            new AchievementData{ id="block", name="Hit 30 blocks", target=30, reward=50 },
            new AchievementData{ id="level", name="Complete 3 levels", target=3, reward=200 },
        };

        Load();
    }

    public void AddProgress(string id, int amount = 1)
    {
        var ach = achievements.Find(a => a.id == id);
        if (ach == null || ach.isCompleted) return;

        ach.current += amount;

        if (ach.current >= ach.target)
        {
            ach.current = ach.target;
            ach.isCompleted = true;
        }

        Save();
    }

    public void Claim(string id)
    {
        var ach = achievements.Find(a => a.id == id);
        if (ach == null || !ach.isCompleted || ach.isClaimed) return;

        ach.isClaimed = true;

        CoinManager.Instance.AddCoins(ach.reward);

        Save();
    }

    void Save()
    {
        foreach (var a in achievements)
        {
            PlayerPrefs.SetInt(a.id + "_cur", a.current);
            PlayerPrefs.SetInt(a.id + "_done", a.isCompleted ? 1 : 0);
            PlayerPrefs.SetInt(a.id + "_claim", a.isClaimed ? 1 : 0);
        }
    }

    void Load()
    {
        foreach (var a in achievements)
        {
            a.current = PlayerPrefs.GetInt(a.id + "_cur", 0);
            a.isCompleted = PlayerPrefs.GetInt(a.id + "_done", 0) == 1;
            a.isClaimed = PlayerPrefs.GetInt(a.id + "_claim", 0) == 1;
        }
    }
}