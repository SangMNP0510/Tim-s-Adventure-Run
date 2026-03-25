using UnityEngine;

public class ScoreService : MonoBehaviour
{
    public static ScoreService Instance { get; private set; }

    [SerializeField] private ScoreConfig scoreConfig;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(ScoreEventType type)
    {
        int points = GetPoints(type);
        if (points == 0) return;

        GameManager gameManager = FindAnyObjectByType<GameManager>();
        if (gameManager == null) return;

        gameManager.AddScore(points);
    }

    private int GetPoints(ScoreEventType type)
    {
        if (scoreConfig == null)
        {
            Debug.LogWarning("ScoreService: ScoreConfig is not assigned.");
            return 0;
        }

        switch (type)
        {
            case ScoreEventType.Coin:
                return scoreConfig.coinScore;
            case ScoreEventType.Enemy:
                return scoreConfig.enemyScore;
            case ScoreEventType.Bonus:
                return scoreConfig.bonusScore;
            case ScoreEventType.LevelComplete:
                return scoreConfig.levelCompleteScore;
            default:
                return 0;
        }
    }
}
