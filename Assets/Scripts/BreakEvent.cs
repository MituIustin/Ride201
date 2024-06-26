using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEvent : MonoBehaviour
{
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        int seconds = Random.Range(4, 10);  // waiting time for green
        brk(seconds);
    }

    // Bus stops 

    public void brk(int sec)
    {
        Vector2 movement = new Vector2(200, rb.velocity.y);
        rb.velocity = movement;
    }
}
