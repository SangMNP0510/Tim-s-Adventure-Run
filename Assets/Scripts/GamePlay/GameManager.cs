using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Intro Effect")]
    [SerializeField] private RectTransform cloudLeft;
    [SerializeField] private RectTransform cloudRight;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private GameObject PanelText;

    [SerializeField] private float cloudMoveTime = 2f;
    [SerializeField] private float textStayTime = 1f;

    private Vector2 leftStartPos;
    private Vector2 rightStartPos;
    public static GameManager Instance;
    [SerializeField] private GameObject startPopupUi;
    [SerializeField] private GameObject shopSkillUi;
    private bool isGameStarted = false;
    private bool isPaused = false;
    private int score = 0;

    [Header("UI")]
    [SerializeField] private GameObject pauseUi;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUi;
    [SerializeField] private GameObject reviveUi;
    [SerializeField] private TextMeshProUGUI reviveText;
    [SerializeField] private GameObject countdownUi;
    [SerializeField] private TextMeshProUGUI countdownText;
    private bool hasRevived = false;

    private bool isGameOver = false;

    [Header("Win UI")]
    [SerializeField] private GameObject winUi;
    [SerializeField] private TextMeshProUGUI winScoreText;
    [SerializeField] private Image starImage;
    [SerializeField] private Sprite star1;
    [SerializeField] private Sprite star2;
    [SerializeField] private Sprite star3;
    [SerializeField] private GameObject newHighscoreText;

    [Header("Level Config")]
    [SerializeField] private int levelIndex = 1;
    [SerializeField] private int star1Score = 2000;
    [SerializeField] private int star2Score = 2500;

    private Vector3 lastCheckpointPos;
    private GameObject player;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int startTime = 400;
    private float currentTime;

    [Header("Progress")]
    [SerializeField] private Transform startPointPlayer;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private UnityEngine.UI.Slider progressSlider;

    [Header("Coin")]
    [SerializeField] private TextMeshProUGUI coinText;
    private int tempCoins = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateScore();

        gameOverUi.SetActive(false);
        reviveUi.SetActive(false);
        countdownUi.SetActive(false);

        currentTime = startTime;

        player = GameObject.FindGameObjectWithTag("Player");
        lastCheckpointPos = player.transform.position;

        tempCoins = 0;
        UpdateCoinUI();
        startPopupUi.SetActive(false);
        shopSkillUi.SetActive(false);
        Time.timeScale = 0;
        isGameStarted = false;
        isPaused = true;

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.StartsWith("Level"))
        {
            string number = sceneName.Replace("Level", "");
            levelIndex = int.Parse(number);
        }

        levelText.gameObject.SetActive(true);
        levelNumberText.gameObject.SetActive(true);
        PanelText.SetActive(true);
        levelNumberText.text = "Level " + levelIndex;

        leftStartPos = cloudLeft.anchoredPosition;
        rightStartPos = cloudRight.anchoredPosition;

        StartCoroutine(PlayIntroEffect());
    }

    IEnumerator PlayIntroEffect()
    {
        float time = 0;

        Vector2 leftEnd = leftStartPos + Vector2.left * 1000f;
        Vector2 rightEnd = rightStartPos + Vector2.right * 1000f;

        while (time < cloudMoveTime)
        {
            time += Time.unscaledDeltaTime;
            float t = time / cloudMoveTime;

            cloudLeft.anchoredPosition = Vector2.Lerp(leftStartPos, leftEnd, t);
            cloudRight.anchoredPosition = Vector2.Lerp(rightStartPos, rightEnd, t);

            yield return null;
        }

        cloudLeft.gameObject.SetActive(false);
        cloudRight.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(textStayTime);

        levelText.gameObject.SetActive(false);
        levelNumberText.gameObject.SetActive(false);
        PanelText.SetActive(false);

        startPopupUi.SetActive(true);

        isPaused = true;
        Time.timeScale = 0;
    }

    public void OpenShopSkill()
    {
        shopSkillUi.SetActive(true);

        isPaused = true;
        Time.timeScale = 0;
    }

    public void CloseShopSkill()
    {
        shopSkillUi.SetActive(false);

        isPaused = false;
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        if (isGameStarted) return;
        Debug.Log("START GAME CALLED !!!");
        startPopupUi.SetActive(false);
        shopSkillUi.SetActive(false);
        isGameStarted = true;
        isPaused = false;
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        if (isGameOver) return;

        isPaused = true;
        Time.timeScale = 0;

        pauseUi.SetActive(true);
    }

    public void ResumeGame()
    {
        if (!isGameStarted) return;
        isPaused = false;
        Time.timeScale = 1;

        pauseUi.SetActive(false);
    }

    public void PauseForAds()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    public void AddCoin(int amount)
    {
        tempCoins += amount;
        UpdateCoinUI();
    }

    void UpdateCoinUI()
    {
        coinText.text = tempCoins.ToString();
    }

    void Update()
    {
        if (!isGameStarted) return;

        if (isGameOver || isPaused) return;

        HandleTimer();
        UpdateProgress();
    }

    void UpdateProgress()
    {
        float startX = startPoint.position.x;
        float endX = endPoint.position.x;
        float playerX = startPointPlayer.position.x;

        float progress = (playerX - startX) / (endX - startX);

        progress = Mathf.Clamp01(progress);

        progressSlider.value = Mathf.Lerp(progressSlider.value, progress, Time.deltaTime * 5f);
    }

    void HandleTimer()
    {
        currentTime -= Time.deltaTime;

        int time = Mathf.CeilToInt(currentTime);
        timerText.text = time.ToString();

        if (currentTime <= 0)
        {
            currentTime = 0;
            timerText.text = "0";

            TimeUp();
        }
    }

    void TimeUp()
    {
        if (isGameOver) return;

        isGameOver = true;

        Time.timeScale = 0;

        ShowLose();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }

    public static int GetTotalScore()
    {
        int total = 0;
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        int maxLevel = Mathf.Max(1, unlockedLevel);

        for (int i = 1; i <= maxLevel; i++)
        {
            total += PlayerPrefs.GetInt("LevelScore_" + i, 0);
        }

        return total;
    }

    private void UpdateScore()
    {
        scoreText.text = score.ToString();
    }

    public void SetCheckpoint(Vector3 pos)
    {
        lastCheckpointPos = pos;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Time.timeScale = 0;
        if (hasRevived)
        {
            ShowLose();
            return;
        }

        reviveUi.SetActive(true);
        StartCoroutine(ReviveCountdown());
    }

    IEnumerator ReviveCountdown()
    {
        int time = 5;

        while (time > 0)
        {
            reviveText.text = time.ToString();
            yield return new WaitForSecondsRealtime(1f);
            time--;
        }

        ShowLose();
    }

    public void WatchAdsRevive()
    {
        StopAllCoroutines();

        FindObjectOfType<RewardedAdController>().ShowRewardedAd(
            onReward: () =>
            {
                hasRevived = true;
            },
            onClose: () =>
            {
                reviveUi.SetActive(false);
                StartCoroutine(DelayRevive());
            }
        );
    }

    IEnumerator DelayRevive()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        if (this != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(RevivePlayer());
        }
    }
    IEnumerator RevivePlayer()
    {
        countdownUi.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }

        countdownUi.SetActive(false);

        player.transform.position = lastCheckpointPos + Vector3.up * 0.5f;
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.ResetPlayer();

        Time.timeScale = 1;
        isGameOver = false;
    }

    public void ShowLose()
    {
        reviveUi.SetActive(false);
        if (tempCoins > 0)
        {
            CoinManager.Instance.AddCoins(tempCoins);
            tempCoins = 0;
        }
        gameOverUi.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        hasRevived = false;
        tempCoins = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void CloseRevive()
    {
        StopAllCoroutines();
        ShowLose();
    }

    public void WinGame()
    {
        if (isGameOver) return;

        isGameOver = true;

        StartCoroutine(HandleWin());
    }

    IEnumerator HandleWin()
    {
        Time.timeScale = 0;

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();

        float t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            sr.color = new Color(1, 1, 1, 1 - t);
            yield return null;
        }

        player.SetActive(false);

        if (tempCoins > 0)
        {
            CoinManager.Instance.AddCoins(tempCoins);
            tempCoins = 0;
        }

        ShowWinUI();
    }

    void ShowWinUI()
    {
        winUi.SetActive(true);

        winScoreText.text = score.ToString();

        int starCount = CalculateStars(score);
        Debug.Log("SAVE STAR: Level " + levelIndex + " = " + starCount);

        if (starCount == 1) starImage.sprite = star1;
        else if (starCount == 2) starImage.sprite = star2;
        else starImage.sprite = star3;

        int oldScore = PlayerPrefs.GetInt("LevelScore_" + levelIndex, 0);
        int oldStar = PlayerPrefs.GetInt("LevelStar_" + levelIndex, 0);

        if (score > oldScore)
        {
            PlayerPrefs.SetInt("LevelScore_" + levelIndex, score);
            newHighscoreText.SetActive(true);
        }
        else
        {
            newHighscoreText.SetActive(false);
        }

        PlayerPrefs.SetInt("LevelStar_" + levelIndex, starCount);

        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (levelIndex >= unlocked)
        {
            PlayerPrefs.SetInt("UnlockedLevel", levelIndex + 1);
        }

        PlayerPrefs.Save();

        if (PlayerInformationService.Instance != null)
        {
            _ = PlayerInformationService.Instance.SubmitScore(levelIndex, score);
        }
    }

    int CalculateStars(int score)
    {
        if (score <= star1Score) return 1;
        if (score <= star2Score) return 2;
        return 3;
    }

    public void NextLevel()
    {
        Time.timeScale = 1;

        LevelLoader.selectedLevel = levelIndex + 1;
        SceneManager.LoadScene("LoadingMap");
    }
}