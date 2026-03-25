using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    public Transform contentParent;
    public GameObject itemPrefab;
    public int maxItem = 10;

    private async void Start()
    {
        await LoadLeaderboard();
    }

    private async Task LoadLeaderboard()
    {
        ClearContent();

        if (PlayerInformationService.Instance == null)
        {
            Debug.LogError("LeaderboardUI: PlayerInformationService.Instance is null.");
            return;
        }

        if (contentParent == null)
        {
            Debug.LogError("LeaderboardUI: contentParent is not assigned.");
            return;
        }

        if (itemPrefab == null)
        {
            Debug.LogError("LeaderboardUI: itemPrefab is not assigned.");
            return;
        }

        List<PlayerData> players = await PlayerInformationService.Instance.GetTopScores(maxItem);
        if (players == null) players = new List<PlayerData>();

        for (int i = 0; i < players.Count; i++)
        {
            PlayerData player = players[i];
            int rank = i + 1;

            GameObject item = Instantiate(itemPrefab, contentParent);

            TMP_Text nameText = FindTMPText(item.transform, "NameText_TMP", "NameText");
            TMP_Text scoreText = FindTMPText(item.transform, "ScoreText_TMP", "ScoreText");
            TMP_Text rankText = FindTMPText(item.transform, "RankText_TMP", "RankText");

            if (nameText != null) nameText.text = player.name;
            if (scoreText != null) scoreText.text = player.totalScore.ToString();
            if (rankText != null) rankText.text = rank.ToString();
        }

        Debug.Log("LeaderboardUI: Loaded leaderboard items count = " + players.Count);
    }

    private void ClearContent()
    {
        if (contentParent == null) return;

        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }
    }

    private static TMP_Text FindTMPText(Transform root, params string[] possibleNames)
    {
        for (int i = 0; i < possibleNames.Length; i++)
        {
            Transform tr = root.Find(possibleNames[i]);
            if (tr == null) continue;

            TMP_Text tmp = tr.GetComponent<TMP_Text>();
            if (tmp != null) return tmp;
        }

        return null;
    }
}

