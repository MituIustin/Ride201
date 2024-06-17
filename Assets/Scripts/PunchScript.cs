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
    AudioManager audio;

    private void Start()
    {
        audio = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
        punchObject.SetActive(false);
        player = GameObject.Find("player");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        NPCSpawnVariables spawnVariables = NPCSpawnVariables.Instance;
        if (Input.GetKeyDown(KeyCode.X) && !isPunching && spawnVariables.spawning == false && animator.GetBool("CanJump") == false)
        {
            audio.PlaySFX(audio.punch);
            punchObject.SetActive(true);
            StartCoroutine(DeactivatePunchAfterDelay(punchDuration));
            StartCoroutine(PunchCooldown(0.7f));
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

    IEnumerator PunchCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
    }

}
