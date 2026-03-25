using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderBoardButton : MonoBehaviour
{
    public void GoLeaderBoard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }
}