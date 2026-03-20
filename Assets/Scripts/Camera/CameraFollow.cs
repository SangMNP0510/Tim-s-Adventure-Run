using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    float maxX;

    Camera cam;

    void Start()
    {
        maxX = transform.position.x;
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (player.position.x > maxX)
        {
            maxX = player.position.x;
        }

        transform.position = new Vector3(
            maxX,
            transform.position.y,
            transform.position.z
        );

        float cameraLeft = transform.position.x - cam.orthographicSize * cam.aspect;

        if (player.position.x < cameraLeft)
        {
            player.position = new Vector3(
                cameraLeft,
                player.position.y,
                player.position.z
            );
        }
    }
}