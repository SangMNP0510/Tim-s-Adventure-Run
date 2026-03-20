using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingImageEffect : MonoBehaviour
{
    [Header("Breathing Settings")]
    public float speed;     
    public float stretchAmount; 

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float wave = Mathf.Sin(Time.time * speed);

        float scaleX = 1 + wave * stretchAmount;
        float scaleY = 1 - wave * stretchAmount;

        transform.localScale = new Vector3(
            originalScale.x * scaleX,
            originalScale.y * scaleY,
            originalScale.z
        );
    }
}