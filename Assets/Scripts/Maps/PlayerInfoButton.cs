using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfoButton : MonoBehaviour
{
    public void OpenPlayerInfo()
    {
        SceneManager.LoadScene("PlayerInfo");
    }
}