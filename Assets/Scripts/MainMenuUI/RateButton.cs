using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateButton : MonoBehaviour
{
    public string packageName = "com.HocBaiChamChi.TimsAdventureRun";

    public void RateGame()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + packageName);
#else
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + packageName);
#endif
    }
}