using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public List<GameObject> enemies;

    void Start()
    {
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        while (true)
        {
            if (NPCSpawnVariables.spawning == false && NPCSpawnVariables.npcsalive == 0)
                NPCSpawnVariables.spawning = true;

            if (NPCSpawnVariables.spawning == true && NPCSpawnVariables.npcsalive >= 7)
                NPCSpawnVariables.spawning = false;

            if (NPCSpawnVariables.spawning)
            {
                int npcType = Random.Range(0, 3);
                Vector2 spawnPoint = new Vector2(Random.Range(20, 25), 2);
                Instantiate(enemies[npcType], spawnPoint, Quaternion.identity, transform);
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return null;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
