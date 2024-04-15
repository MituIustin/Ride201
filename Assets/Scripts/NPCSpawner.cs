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
            int npcType = Random.Range(0, 3);
            Vector2 spawnPoint = new Vector2(Random.Range(-8, 8), 2);
            Instantiate(enemies[npcType], spawnPoint, Quaternion.identity, transform);
            yield return new WaitForSeconds(2f);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
