using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelButton levelPrefab;
    public Transform levelParent;
    public int totalLevels;

    void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        foreach (Transform child in levelParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 1; i <= totalLevels; i++)
        {
            LevelButton btn = Instantiate(levelPrefab, levelParent);

            bool unlocked = i <= unlockedLevel;

            btn.Setup(i, unlocked);
        }
    }
}