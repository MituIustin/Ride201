using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControllerNonHostile : MonoBehaviour
{
    GameObject Player;
    List<GameObject> npcs = new List<GameObject>();
    Collider2D col;


    public float speed = 1f;
    public float changeDirectionTime; 

    private Rigidbody2D rb;
    private float directionTimer;
    private int currentDirection = 1;


    void Start()
    {
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("npc");
        col = GetComponent<Collider2D>();
        foreach (GameObject npc in npcObjects)
        {
            npcs.Add(npc);
        }

        foreach (GameObject npc in npcs)
        {
            Collider2D npcCollider = npc.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(npcCollider, col);

        }
        rb = GetComponent<Rigidbody2D>();
        changeDirectionTime = Random.Range(1f, 3f);
        directionTimer = changeDirectionTime;
    }
    void ChangeDirection()
    {
        currentDirection *= -1;
    
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }


    void Update()
    {
        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0)
        {
            ChangeDirection();
            directionTimer = changeDirectionTime;
        }

        Vector2 movement = new Vector2(currentDirection * speed, rb.velocity.y);
        rb.velocity = movement;
    }


}
