using Assets;
using System.Collections;
using UnityEngine;

public class PunchScript : MonoBehaviour
{
    public GameObject punchObject;
    public GameObject player;
    private bool isPunching = false;
    public float punchDuration = 0.3f;
    private Rigidbody2D rb;
    public Animator animator;

    private void Start()
    {
        punchObject.SetActive(false);
        player = GameObject.Find("player");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isPunching && NPCSpawnVariables.spawning == false && rb.velocity.y == 0)
        {   
            punchObject.SetActive(true);
            StartCoroutine(DeactivatePunchAfterDelay(punchDuration));
        }
    }

    IEnumerator DeactivatePunchAfterDelay(float delay)
    {
        animator.SetBool("CanPunch", true);
        isPunching = true;
        yield return new WaitForSeconds(delay);
        punchObject.SetActive(false);
        isPunching = false;
        animator.SetBool("CanPunch", false);
    }

}
