using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("player").transform;
    }

    private void Update()
    {
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, player.transform.position.x, 0.05f),
            transform.position.y,
            -1
            );
    }
}