using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarImageEffect : MonoBehaviour
{
    [Header("Images")]
    public List<RectTransform> images = new List<RectTransform>();

    [Header("Effect Settings")]
    public float rotateSpeed;
    public float scaleSpeed;
    public float minScale;
    public float maxScale;

    [Header("Delay Pattern")]
    public float delayStep;

    private List<bool> shrinkingStates = new List<bool>();
    private List<float> startTimes = new List<float>();

    void Start()
    {
        shrinkingStates.Clear();
        startTimes.Clear();

        for (int i = 0; i < images.Count; i++)
        {
            shrinkingStates.Add(true);

            float delay = (i % 3) * delayStep;

            startTimes.Add(Time.time + delay);
        }
    }

    void Update()
    {
        for (int i = 0; i < images.Count; i++)
        {
            RectTransform img = images[i];
            if (img == null) continue;

            img.Rotate(0, 0, rotateSpeed * Time.deltaTime);

            if (Time.time < startTimes[i]) continue;

            Vector3 scale = img.localScale;

            if (shrinkingStates[i])
            {
                scale -= Vector3.one * scaleSpeed * Time.deltaTime;

                if (scale.x <= minScale)
                {
                    scale = Vector3.one * minScale;
                    shrinkingStates[i] = false;
                }
            }
            else
            {
                scale += Vector3.one * scaleSpeed * Time.deltaTime;

                if (scale.x >= maxScale)
                {
                    scale = Vector3.one * maxScale;
                    shrinkingStates[i] = true;
                }
            }

            img.localScale = scale;
        }
    }
}