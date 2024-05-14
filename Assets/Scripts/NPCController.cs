using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : BaseClassCharacter
{
    GameObject Player;

    private bool got_on_bus = false;
    private float leaving_speed = 0f;
    private bool isDestructionStarted = false;

    List<GameObject> npcs = new List<GameObject>();
    Collider2D col;
    SpriteRenderer spriteRenderer;


    void Start()
    {
        Player = GameObject.Find("player");
        spriteRenderer = GetComponent<SpriteRenderer>();
       
        col = GetComponent<Collider2D>();
    }


    void Update()
    {
        /*
        if(base.getHealth() <= 0)
        {
            Debug.Log(base.getHealth());
            Destroy(gameObject);
        }
        */

        transform.position = Vector2.MoveTowards(transform.position,
                                                Player.transform.position,
                                                2f * Time.deltaTime);

        if (got_on_bus == false && NPCSpawnVariables.spawning == false)
        {  //scapam de npcuri ramase afara
            transform.position -= new Vector3(leaving_speed, 0, 0) * Time.deltaTime;
            leaving_speed += 0.005f;

            if (!isDestructionStarted)
            {
                isDestructionStarted = true;
                StartCoroutine(DestroyAfterDelay(5)); // 5 seconds delay
            }
        }
        Vector3 vec = new Vector3(transform.position.x, transform.position.y, 3);
        transform.position = vec;
        Vector3 v = new Vector3(Player.transform.position.x, transform.position.y, 3f);
        transform.position = Vector3.MoveTowards(transform.position, v, 2f * Time.deltaTime);

        if (transform.position.x < Player.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }


    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.CompareTag("tp_trigger") && NPCSpawnVariables.spawning == true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 3f);
            NPCSpawnVariables.npcsalive += 1;
            got_on_bus = true;
        }
        else
        {
            if (other.gameObject.CompareTag("punch"))
            {
                //base.getPunched(100);
            }
            else
            {
                GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("npc");
                foreach (GameObject npc in npcObjects)
                {
                    npcs.Add(npc);
                }

                foreach (GameObject npc in npcs)
                {
                    Collider2D npcCollider = npc.GetComponent<Collider2D>();
                    Physics2D.IgnoreCollision(npcCollider, col);
                }
            }
        }
    }
    
}

