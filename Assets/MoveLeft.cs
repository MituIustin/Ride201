using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Update is called once per frame

    public float speed = 7f;
    private float actual_speed;

    private void Start()
    {
        actual_speed = speed;
    }
    void Update()
    {
        if (NPCSpawnVariables.spawning == true && actual_speed > 0f)
            actual_speed -= 0.005f;

        if (NPCSpawnVariables.spawning == false && actual_speed < 7f)
            actual_speed += 0.005f;


        transform.position -= new Vector3(actual_speed, 0, 0) * Time.deltaTime;

    }
}
