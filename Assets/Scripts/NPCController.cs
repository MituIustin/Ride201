using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    GameObject Player;
    private bool got_on_bus = false;
    private float leaving_speed = 0f;
    private bool isDestructionStarted = false;

    void Start()
    {
        Player = GameObject.Find("player");
    }


    void Update()
    {
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

    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("tp_trigger") && NPCSpawnVariables.spawning == true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 3f);
            NPCSpawnVariables.npcsalive += 1;
            got_on_bus = true;
        }
    }


    
}
