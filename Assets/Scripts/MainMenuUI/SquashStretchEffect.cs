using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SquashStretchEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Breathing Effect")]
    public float speed;
    public float stretchAmount;

    [Header("Press Effect")]
    public float pressedScale;

    private Vector3 originalScale;
    private bool isPressed = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float wave = Mathf.Sin(Time.time * speed);

        float scaleX = 1 + wave * stretchAmount;
        float scaleY = 1 - wave * stretchAmount;

        float pressMultiplier = isPressed ? pressedScale : 1f;

        transform.localScale = new Vector3(
            originalScale.x * scaleX * pressMultiplier,
            originalScale.y * scaleY * pressMultiplier,
            originalScale.z
        );
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}