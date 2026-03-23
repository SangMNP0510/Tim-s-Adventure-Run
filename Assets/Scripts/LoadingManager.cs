using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Slider loadingSlider;

    [Header("Scene Names")]
    public string profileSceneName = "Profile";
    public string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        string targetScene = profileSceneName;

        if (PlayerProfileManager.Instance != null &&
            PlayerProfileManager.Instance.HasProfile())
        {
            targetScene = mainMenuSceneName;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.3f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}