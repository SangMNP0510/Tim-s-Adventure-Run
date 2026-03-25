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
        int totalScore = GameManager.GetTotalScore();
        DatabaseReference playerRef = dbRef.Child("playerinfor").Child(playerId);

        bool playerExists = false;
        try
        {
            DataSnapshot snapshot = await playerRef.GetValueAsync();
            playerExists = snapshot.Exists;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("PlayerInformationService: Could not verify existing player. Error: " + ex.Message);
        }

        var submitTasks = new List<Task>
        {
            playerRef.Child("totalScore").SetValueAsync(totalScore),
            playerRef.Child("levelScores").Child(levelIndex.ToString()).SetValueAsync(levelScore)
        };

        if (!playerExists)
        {
            submitTasks.Add(playerRef.Child("name").SetValueAsync(playerName));
            submitTasks.Add(playerRef.Child("avatarIndex").SetValueAsync(avatarIndex));
        }

        try
        {
            await Task.WhenAll(submitTasks);
            Debug.Log("PlayerInformationService: SubmitScore success for playerId = " + playerId);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("PlayerInformationService: SubmitScore failed. Error: " + ex.Message);
        }
    }
}
