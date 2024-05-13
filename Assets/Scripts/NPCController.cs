using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    GameObject Player;

    void Start()
    {
        Player = GameObject.Find("player");
    }


    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,
                                                Player.transform.position,
                                                2f * Time.deltaTime);
    }

  
}
