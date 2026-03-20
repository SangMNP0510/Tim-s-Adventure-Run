using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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
        isPaused = false;
        Time.timeScale = 1;

        pauseUi.SetActive(false);
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

        // TODO: sau này gắn ads ở đây

        hasRevived = true;

        reviveUi.SetActive(false);

        StartCoroutine(RevivePlayer());
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
        Time.timeScale = 0;

        if (tempCoins > 0)
        {
            CoinManager.Instance.AddCoins(tempCoins);
            tempCoins = 0;
        }

        // show win UI
    }
}