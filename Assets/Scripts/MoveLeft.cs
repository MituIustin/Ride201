using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Background Moving Manager

public class MoveLeft : MonoBehaviour
{
    public float speed = 7f;
    public static float actual_speed;

    private void Start()
    {
        actual_speed = speed;
    }

    void Update()
    {
        NPCSpawnVariables spawnVariables = NPCSpawnVariables.Instance;
        if (spawnVariables.spawning == true && actual_speed > 0f)
            actual_speed -= 0.001f;

        if (spawnVariables.spawning == false && actual_speed < 7f)
            actual_speed += 0.001f;

        transform.position -= new Vector3(actual_speed, 0, 0) * Time.deltaTime;

    }
}
