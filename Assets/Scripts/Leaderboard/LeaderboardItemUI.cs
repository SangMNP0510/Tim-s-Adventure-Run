using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItemUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text nameText;
    public TMP_Text scoreText;
    public TMP_Text rankText;
    public Image avatarImage;

    [Header("Avatar")]
    public Sprite[] avatarSprites;

    [Header("Highlight")]
    public GameObject highlightObj;

    public void Setup(PlayerData data, int rank, bool isMe)
    {
        if (nameText != null)
            nameText.text = data.name;

        if (scoreText != null)
            scoreText.text = data.totalScore.ToString();

        if (rankText != null)
            rankText.text = "Top " + rank;

        if (avatarImage != null && avatarSprites != null)
        {
            if (data.avatarIndex >= 0 && data.avatarIndex < avatarSprites.Length)
            {
                avatarImage.sprite = avatarSprites[data.avatarIndex];
            }
        }

        if (highlightObj != null)
        {
            highlightObj.SetActive(isMe);
        }
    }
}