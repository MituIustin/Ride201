using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSpawner : MonoBehaviour
{
    public List<GameObject> enemies;
    public GameObject nextlvl;
    public int lvl;
    public Text lvltext;
    //public next nextlvlScript;

    void Start()
    {
        lvl = 1;
        nextlvl = GameObject.FindGameObjectWithTag("nlvl");
        nextlvl.SetActive(false);
        lvltext.text = "LVL " + lvl.ToString();
        lvl++;
        StartCoroutine(HideBackgroundAfterTime(5.0f));
        NPCSpawnVariables spawnVariables = NPCSpawnVariables.Instance;
        spawnVariables.spawning = true;
        spawnVariables.npcsalive = 0;
        StartCoroutine(Spawning());
    }
    System.Collections.IEnumerator HideBackgroundAfterTime(float time)
    {
        nextlvl.SetActive(true);
        yield return new WaitForSeconds(time);
        nextlvl.SetActive(false);
    }

    IEnumerator Spawning()
    {
        NPCSpawnVariables spawnVariables = NPCSpawnVariables.Instance;
        while (true)
        {
            if (spawnVariables.spawning == false && spawnVariables.npcsalive == 0)
            {
                lvltext.text = "LVL " + lvl.ToString();
                lvl++;
                StartCoroutine(HideBackgroundAfterTime(5.0f));
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
