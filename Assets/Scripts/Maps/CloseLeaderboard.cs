using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseLeaderboard : MonoBehaviour
{
    public void CloseRanking()
    {
        SceneManager.LoadScene("PlayerInfo");
    }
}