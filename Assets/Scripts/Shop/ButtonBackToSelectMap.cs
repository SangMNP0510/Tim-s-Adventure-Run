using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBackToSelectMap : MonoBehaviour
{
    public void BackToSelectMap()
    {
        SceneManager.LoadScene("SelectMap");
    }
}