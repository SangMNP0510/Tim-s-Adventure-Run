using UnityEngine;
using System;

public class PlayerProfileManager : MonoBehaviour
{
    public static PlayerProfileManager Instance;

    public string playerId { get; private set; }
    public string playerName { get; private set; }
    public int avatarIndex { get; private set; }

    private const string PLAYER_ID_KEY = "PlayerProfile_PlayerId";
    private const string PLAYER_NAME_KEY = "PlayerProfile_PlayerName";
    private const string AVATAR_INDEX_KEY = "PlayerProfile_AvatarIndex";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProfile();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Lưu profile
    public void SaveProfile(string name, int avatarIndex)
    {
        playerName = string.IsNullOrEmpty(name) ? "Player" : name.Trim();
        this.avatarIndex = Mathf.Clamp(avatarIndex, 0, 5);

        if (string.IsNullOrEmpty(playerId))
        {
            playerId = Guid.NewGuid().ToString();
        }

        PlayerPrefs.SetString(PLAYER_ID_KEY, playerId);
        PlayerPrefs.SetString(PLAYER_NAME_KEY, playerName);
        PlayerPrefs.SetInt(AVATAR_INDEX_KEY, this.avatarIndex);
        PlayerPrefs.Save();
    }

    // Load khi mở game
    public void LoadProfile()
    {
        playerId = PlayerPrefs.GetString(PLAYER_ID_KEY, string.Empty);
        playerName = PlayerPrefs.GetString(PLAYER_NAME_KEY, string.Empty);
        avatarIndex = Mathf.Clamp(PlayerPrefs.GetInt(AVATAR_INDEX_KEY, 0), 0, 5);

        if (string.IsNullOrEmpty(playerId))
        {
            playerId = Guid.NewGuid().ToString();
            PlayerPrefs.SetString(PLAYER_ID_KEY, playerId);
            PlayerPrefs.Save();
        }
    }

    // Kiểm tra đã có profile chưa
    public bool HasProfile()
    {
        return !string.IsNullOrEmpty(playerName);
    }

    public string GetPlayerId()
    {
        return playerId;
    }
}