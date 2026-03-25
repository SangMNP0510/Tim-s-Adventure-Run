using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    public Transform contentParent;
    public GameObject itemPrefab;
    public int maxItem = 10;

    [Header("Current Player")]
    public Transform currentPlayerParent;
    public GameObject currentPlayerPrefab;

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

        List<PlayerData> players = await PlayerInformationService.Instance.GetTopScores(maxItem);
        if (players == null) players = new List<PlayerData>();

        string myId = PlayerProfileManager.Instance.GetPlayerId();

        PlayerData myData = null;
        int myRank = -1;

        for (int i = 0; i < players.Count; i++)
        {
            PlayerData player = players[i];
            int rank = i + 1;

            bool isMe = player.playerId == myId;

            if (isMe)
            {
                myData = player;
                myRank = rank;
            }

            GameObject item = Instantiate(itemPrefab, contentParent);
            LeaderboardItemUI itemUI = item.GetComponent<LeaderboardItemUI>();

            if (itemUI != null)
            {
                itemUI.Setup(player, rank, isMe);
            }
        }

        if (myData == null)
        {
            List<PlayerData> allPlayers = await PlayerInformationService.Instance.GetTopScores(9999);

            for (int i = 0; i < allPlayers.Count; i++)
            {
                if (allPlayers[i].playerId == myId)
                {
                    myData = allPlayers[i];
                    myRank = i + 1;
                    break;
                }
            }
        }

        if (currentPlayerParent != null && currentPlayerPrefab != null && myData != null)
        {
            for (int i = currentPlayerParent.childCount - 1; i >= 0; i--)
            {
                Destroy(currentPlayerParent.GetChild(i).gameObject);
            }

            GameObject item = Instantiate(currentPlayerPrefab, currentPlayerParent);
            LeaderboardItemUI itemUI = item.GetComponent<LeaderboardItemUI>();

            if (itemUI != null)
            {
                itemUI.Setup(myData, myRank, true);
            }
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
}