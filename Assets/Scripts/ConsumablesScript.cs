using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumablesScript : MonoBehaviour
{
    AudioManager audio;

    public void Start()
    {
        audio = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
    }

    // Pick-Up Method 

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="player")
        {
            audio.PlaySFX(audio.pickup);
            GameObject.FindWithTag("player").GetComponent<MainCharacter>().IncreaseSpeed();
            Destroy(gameObject);
        }
    }

    // Ignore collisions with all npcs  

    private void Update()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("npc");
        foreach (GameObject npc in npcs)
        {
            Physics2D.IgnoreCollision(npc.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
