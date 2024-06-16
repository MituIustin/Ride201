using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aspacardinScript : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            GameObject.FindWithTag("player").GetComponent<MainCharacter>().RestoreHealth();
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        //Debug.Log("UPDATE");
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("npc");
        foreach (GameObject npc in npcs)
        {
            Physics2D.IgnoreCollision(npc.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}