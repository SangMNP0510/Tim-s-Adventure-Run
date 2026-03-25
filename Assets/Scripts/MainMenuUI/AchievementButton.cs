using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementButton : MonoBehaviour
{
    public void OpenAchievement()
    {
        SceneManager.LoadScene("Achievement");
    }
}