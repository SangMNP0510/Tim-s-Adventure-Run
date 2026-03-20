using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRewardButton : MonoBehaviour
{
    public GameObject popupWatchAds;

    public void OpenPopup()
    {
        popupWatchAds.SetActive(true);
    }
}