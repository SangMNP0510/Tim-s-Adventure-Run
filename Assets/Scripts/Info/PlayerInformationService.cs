using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerInformationService : MonoBehaviour
{
    public static PlayerInformationService Instance { get; private set; }

    private const string DatabaseUrl = "https://tim-adventure-run-team5-default-rtdb.asia-southeast1.firebasedatabase.app/";
    private DatabaseReference dbRef;

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

    private void Start()
    {
        dbRef = FirebaseDatabase.GetInstance(DatabaseUrl).RootReference;
        Debug.Log("PlayerInformationService initialized: Firebase Realtime Database connected.");
    }

    private static int ReadIntSafe(DataSnapshot snap)
    {
        if (snap == null || snap.Value == null) return 0;
        try { return System.Convert.ToInt32(snap.Value); }
        catch { return 0; }
    }

    private static string ReadStringSafe(DataSnapshot snap)
    {
        if (snap == null || snap.Value == null) return string.Empty;
        try { return snap.Value.ToString(); }
        catch { return string.Empty; }
    }

    public async Task SavePlayer()
    {
        if (dbRef == null)
        {
            Debug.LogError("PlayerInformationService: Firebase Database reference is not initialized.");
            return;
        }

        if (PlayerProfileManager.Instance == null)
        {
            Debug.LogError("PlayerInformationService: PlayerProfileManager instance not found.");
            return;
        }

        string playerId = PlayerProfileManager.Instance.GetPlayerId();
        if (string.IsNullOrEmpty(playerId))
        {
            Debug.LogError("PlayerInformationService: playerId is empty.");
            return;
        }

        string playerName = PlayerProfileManager.Instance.playerName;
        int avatarIndex = PlayerProfileManager.Instance.avatarIndex;
        DatabaseReference playerRef = dbRef.Child("playerinfor").Child(playerId);

        var tasks = new List<Task>
        {
            playerRef.Child("name").SetValueAsync(playerName),
            playerRef.Child("avatarIndex").SetValueAsync(avatarIndex)
        };

        try
        {
            await Task.WhenAll(tasks);
            Debug.Log("PlayerInformationService: SavePlayer success for playerId = " + playerId);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("PlayerInformationService: SavePlayer failed. Error: " + ex.Message);
        }
    }

    public async Task SubmitScore(int levelIndex, int levelScore)
    {
        if (dbRef == null)
        {
            Debug.LogError("PlayerInformationService: Firebase Database reference is not initialized.");
            return;
        }

        if (PlayerProfileManager.Instance == null)
        {
            Debug.LogError("PlayerInformationService: PlayerProfileManager instance not found.");
            return;
        }

        string playerId = PlayerProfileManager.Instance.GetPlayerId();
        if (string.IsNullOrEmpty(playerId))
        {
            Debug.LogError("PlayerInformationService: playerId is empty.");
            return;
        }

        string playerName = PlayerProfileManager.Instance.playerName;
        int avatarIndex = PlayerProfileManager.Instance.avatarIndex;

        DatabaseReference playerRef = dbRef.Child("playerinfor").Child(playerId);
        DatabaseReference levelScoreRef = playerRef.Child("levelScores").Child(levelIndex.ToString());

        int oldScore = 0;
        bool hasOldScore = false;

        // Only read the exact node we need: levelScores/levelIndex
        try
        {
            DataSnapshot oldSnapshot = await levelScoreRef.GetValueAsync();
            if (oldSnapshot != null && oldSnapshot.Exists)
            {
                hasOldScore = true;

                try
                {
                    oldScore = System.Convert.ToInt32(oldSnapshot.Value);
                }
                catch
                {
                    Debug.LogWarning("PlayerInformationService: oldScore parse failed. Treating as 0. playerId=" + playerId);
                    oldScore = 0;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("PlayerInformationService: Could not read old score. Error: " + ex.Message + " (will treat oldScore as 0)");
        }

        // BEST SCORE logic: only submit when new score is higher
        if (hasOldScore && levelScore <= oldScore)
        {
            Debug.Log("PlayerInformationService: Skip submit: score not higher. playerId=" + playerId + " level=" + levelIndex + " old=" + oldScore + " new=" + levelScore);
            return;
        }

        int totalScore = GameManager.GetTotalScore();
        var submitTasks = new List<Task>
        {
            levelScoreRef.SetValueAsync(levelScore),
            playerRef.Child("totalScore").SetValueAsync(totalScore)
        };

        // Fallback: if no old score existed, also ensure identity fields exist.
        if (!hasOldScore)
        {
            submitTasks.Add(playerRef.Child("name").SetValueAsync(playerName));
            submitTasks.Add(playerRef.Child("avatarIndex").SetValueAsync(avatarIndex));
        }

        try
        {
            await Task.WhenAll(submitTasks);
            Debug.Log("PlayerInformationService: SubmitScore update success. playerId=" + playerId + " level=" + levelIndex + " score=" + levelScore);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("PlayerInformationService: SubmitScore update failed. Error: " + ex.Message);
        }
    }

    public async Task<List<PlayerData>> GetTopScores(int limit = 10)
    {
        List<PlayerData> result = new List<PlayerData>();
        if (limit <= 0) return result;

        if (dbRef == null)
        {
            Debug.LogError("PlayerInformationService: Firebase Database reference is not initialized.");
            return result;
        }

        try
        {
            DataSnapshot playersSnapshot = await dbRef.Child("playerinfor").GetValueAsync();
            if (playersSnapshot == null || !playersSnapshot.Exists) return result;

            foreach (DataSnapshot playerSnap in playersSnapshot.Children)
            {
                string playerId = playerSnap.Key;
                string name = ReadStringSafe(playerSnap.Child("name"));
                int avatarIndex = ReadIntSafe(playerSnap.Child("avatarIndex"));
                int totalScore = ReadIntSafe(playerSnap.Child("totalScore"));

                result.Add(new PlayerData
                {
                    playerId = playerId,
                    name = name,
                    avatarIndex = avatarIndex,
                    totalScore = totalScore
                });
            }

            Debug.Log("PlayerInformationService: Loaded " + result.Count + " players from Firebase.");

            result.Sort((a, b) => b.totalScore.CompareTo(a.totalScore));
            if (result.Count > limit) result.RemoveRange(limit, result.Count - limit);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("PlayerInformationService: GetTopScores failed. Error: " + ex.Message);
            return new List<PlayerData>();
        }

        return result;
    }
}

[System.Serializable]
public class PlayerData
{
    public string playerId;
    public string name;
    public int avatarIndex;
    public int totalScore;
}
