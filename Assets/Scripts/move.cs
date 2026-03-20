using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    void Update()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPoint.z = 0;

        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float clampedX = Mathf.Clamp(worldPoint.x, min.x, max.x);
        float clampedY = Mathf.Clamp(worldPoint.y, min.y, max.y);

        transform.position = new Vector3(clampedX, clampedY, 0);
    }
}
