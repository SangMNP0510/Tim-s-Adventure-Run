using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class BannerAdController : MonoBehaviour
{
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
    private string _adUnitId = "unused";
#endif

    private BannerView _bannerView;
    private ScreenOrientation _currentOrientation;
    [Header("Scenes Allow Banner")]
    public string[] allowedScenes;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        _currentOrientation = Screen.orientation;
        Debug.Log($"BannerAdController: Initial orientation = {_currentOrientation}");

        CreateAndLoadBanner();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        bool allow = false;

        foreach (var s in allowedScenes)
        {
            if (scene.name == s)
            {
                allow = true;
                break;
            }
        }

        if (allow)
        {
            ShowBanner();
        }
        else
        {
            HideBanner();
        }
    }

    public void ShowBanner()
    {
        if (_bannerView == null)
        {
            CreateAndLoadBanner();
        }
        else
        {
            _bannerView.Show();
        }
    }

    public void HideBanner()
    {
        if (_bannerView != null)
        {
            _bannerView.Hide();
        }
    }

    private void Update()
    {
        if (Screen.orientation != _currentOrientation)
        {
            Debug.Log($"BannerAdController: Orientation changed to {Screen.orientation}");
            _currentOrientation = Screen.orientation;
            RecreateBannerForNewOrientation();
        }
    }

    private void RecreateBannerForNewOrientation()
    {
        if (_bannerView != null)
        {
            Debug.Log("BannerAdController: Destroying old banner.");
            _bannerView.Destroy();
            _bannerView = null;
        }

        CreateAndLoadBanner();
    }

    private void CreateAndLoadBanner()
    {
        if (_adUnitId == "unused")
        {
            Debug.LogWarning("BannerAdController: Unsupported platform.");
            return;
        }

        Debug.Log($"BannerAdController: Creating banner for orientation: {_currentOrientation}");

        AdSize adaptiveSize =
            AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        _bannerView = new BannerView(_adUnitId, adaptiveSize, AdPosition.Bottom);

        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("BannerAdController: Banner loaded.");
        };

        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError($"BannerAdController: Failed to load banner. Reason: {error.GetMessage()}");
        };

        AdRequest request = new AdRequest();
        Debug.Log("BannerAdController: Loading banner...");
        _bannerView.LoadAd(request);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }
    }
}