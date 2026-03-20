using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButtonPressEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float pressedScale;
    public float darkenAmount;

    private Vector3 originalScale;
    private Graphic[] graphics;

    void Start()
    {
        originalScale = transform.localScale;

        graphics = GetComponentsInChildren<Graphic>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * pressedScale;

        foreach (Graphic g in graphics)
        {
            g.color *= darkenAmount;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;

        foreach (Graphic g in graphics)
        {
            g.color /= darkenAmount;
        }
    }
}