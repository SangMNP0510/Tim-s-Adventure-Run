using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;

    public TMP_Text levelText;
    public GameObject lockIcon;
    public Button button;

    public GameObject starsParent;

    public GameObject star1Obj;
    public GameObject star2Obj;
    public GameObject star3Obj;

    public void Setup(int level, bool unlocked)
    {
        levelIndex = level;

        levelText.text = level.ToString();

        lockIcon.SetActive(!unlocked);
        button.interactable = unlocked;

        int starCount = PlayerPrefs.GetInt("LevelStar_" + levelIndex, 0);

        starsParent.SetActive(starCount > 0);

        UpdateStars(starCount);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickLevel);
    }

    void UpdateStars(int count)
    {
        star1Obj.SetActive(false);
        star2Obj.SetActive(false);
        star3Obj.SetActive(false);

        if (count == 1)
            star1Obj.SetActive(true);
        else if (count == 2)
            star2Obj.SetActive(true);
        else if (count >= 3)
            star3Obj.SetActive(true);
    }

    void OnClickLevel()
    {
        LevelLoader.selectedLevel = levelIndex;
        SceneManager.LoadScene("LoadingMap");
    }
}