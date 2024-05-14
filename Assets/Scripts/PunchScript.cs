using Assets;
using System.Collections;
using UnityEngine;

public class PunchScript : MonoBehaviour
{
    public GameObject punchObject;
    public GameObject player;
    private bool isPunching = false;
    public float punchDuration = 0.3f;

    private void Start()
    {
        punchObject.SetActive(false);
        player = GameObject.Find("player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isPunching && NPCSpawnVariables.spawning == false)
        {
            punchObject.SetActive(true);
            StartCoroutine(DeactivatePunchAfterDelay(punchDuration));
        }
    }

    IEnumerator DeactivatePunchAfterDelay(float delay)
    {
        isPunching = true;
        yield return new WaitForSeconds(delay);
        punchObject.SetActive(false);
        isPunching = false;
    }

}
