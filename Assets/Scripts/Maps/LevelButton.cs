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
    public Image[] stars;

    public Sprite starOn;
    public Sprite starOff;

    public void Setup(int level, bool unlocked)
    {
        levelIndex = level;

        levelText.text = level.ToString();

        lockIcon.SetActive(!unlocked);

        button.interactable = unlocked;

        int starCount = PlayerPrefs.GetInt("LevelStar_" + levelIndex, 0);

        if (starCount == 0)
        {
            starsParent.SetActive(false);
        }
        else
        {
            starsParent.SetActive(true);
            UpdateStars(starCount);
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickLevel);
    }

    void UpdateStars(int count)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = (i < count) ? starOn : starOff;
        }
    }

    void OnClickLevel()
    {
        LevelLoader.selectedLevel = levelIndex;
        SceneManager.LoadScene("LoadingMap");
    }
}