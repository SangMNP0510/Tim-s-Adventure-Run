using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup canvasGroup;
    public RectTransform logoGroup;
    public Image whiteFade;

    [Header("Timing")]
    public float fadeInDuration;
    public float zoomDuration;
    public float holdAfterZoom;
    public float fadeOutDuration;

    private void Start()
    {
        StartCoroutine(PlaySplash());
    }

    IEnumerator PlaySplash()
    {
        float t = 0;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeInDuration);
            yield return null;
        }

        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(0.1f);

        t = 0;
        Vector3 startScale = Vector3.one;
        Vector3 targetScale = Vector3.one * 1.08f;

        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            logoGroup.localScale = Vector3.Lerp(startScale, targetScale, t / zoomDuration);
            yield return null;
        }

        yield return new WaitForSeconds(holdAfterZoom);

        t = 0;
        Color c = whiteFade.color;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeOutDuration);
            whiteFade.color = c;
            yield return null;
        }

        SceneManager.LoadScene("LoadingInGame");
    }
}