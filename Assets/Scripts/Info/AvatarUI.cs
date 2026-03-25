using UnityEngine;
using UnityEngine.UI;

public class AvatarUI : MonoBehaviour
{
    [Header("UI")]
    public Image avatarImage;

    [Header("Avatar List")]
    public Sprite[] avatarSprites;

    void Start()
    {
        LoadPlayerAvatar();
    }

    void LoadPlayerAvatar()
    {
        if (PlayerProfileManager.Instance == null)
        {
            Debug.LogError("MainMenuUI: PlayerProfileManager.Instance is null");
            return;
        }

        int avatarIndex = PlayerProfileManager.Instance.avatarIndex;

        if (avatarSprites != null && avatarIndex >= 0 && avatarIndex < avatarSprites.Length)
        {
            avatarImage.sprite = avatarSprites[avatarIndex];
        }
        else
        {
            Debug.LogWarning("MainMenuUI: avatar index out of range");
        }
    }
}