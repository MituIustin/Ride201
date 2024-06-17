using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    public int lvl;
    //public next nextlvlScript;

    void Start()
    {
        lvl = 0;
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        NPCSpawnVariables spawnVariables = NPCSpawnVariables.Instance;
        while (true)
        {
            if (spawnVariables.spawning == false && spawnVariables.npcsalive == 0)
            {
                //nextlvlScript.Setup(lvl);
                //lvl++;
                spawnVariables.spawning = true;
            }
                

            if (spawnVariables.spawning == true && spawnVariables.npcsalive >= 7)
                spawnVariables.spawning = false;

            if (spawnVariables.spawning)
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
