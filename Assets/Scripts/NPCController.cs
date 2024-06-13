using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : BaseClassCharacter
{
    GameObject Player;

    private bool got_on_bus = false;
    private float leaving_speed = 0f;
    private bool isDestructionStarted = false;

    List<GameObject> npcs = new List<GameObject>();
    Collider2D col;
    SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isFalling = false;


    void Start()
    {
        Player = GameObject.Find("player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


    void Update()
    {
        if (base.getHealth() <= 0 && !isFalling)
        {
            Debug.Log(base.getHealth());
            NPCSpawnVariables.npcsalive -= 1;
            StartCoroutine(FallAndDie());
        }


        transform.position = Vector2.MoveTowards(transform.position,
                                                Player.transform.position,
                                                2f * Time.deltaTime);

        if (got_on_bus == false && NPCSpawnVariables.spawning == false)
        {  //scapam de npcuri ramase afara
            transform.position -= new Vector3(leaving_speed, 0, 0) * Time.deltaTime;
            leaving_speed += 0.01f;

            if (!isDestructionStarted)
            {
                isDestructionStarted = true;
                StartCoroutine(DestroyAfterDelay(15)); // 15 seconds delay
            }
        }
        Vector3 vec = new Vector3(transform.position.x, transform.position.y, 3);
        transform.position = vec;
        Vector3 v = new Vector3(Player.transform.position.x, transform.position.y, 3f);
        transform.position = Vector3.MoveTowards(transform.position, v, 2f * Time.deltaTime);

        if (transform.position.x < Player.transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }


    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject); // bogos

        if (other.gameObject.CompareTag("tp_trigger") && NPCSpawnVariables.spawning == true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 3f);
            NPCSpawnVariables.npcsalive += 1;
            got_on_bus = true;
        }
        else
        {
            if (other.gameObject.CompareTag("punch"))
            {
               
                StartCoroutine(HandlePunch(other));

            }
            else
            {
                GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("npc");
                foreach (GameObject npc in npcObjects)
                {
                    npcs.Add(npc);
                }

                foreach (GameObject npc in npcs)
                {
                    if (npc != null)
                    {
                        Collider2D npcCollider = npc.GetComponent<Collider2D>();
                        Physics2D.IgnoreCollision(npcCollider, col);
                    }
                }
            }
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;   // Change color to red 
        yield return new WaitForSeconds(0.5f);  // Wait for 0.5 seconds
        spriteRenderer.color = Color.white; // Reset color to normal
    }


    private IEnumerator HandlePunch(Collider2D other)
        {
            // Optional: Add a delay before applying the punch effects
            yield return new WaitForSeconds(0.2f); // Adjust the delay time as needed

            // Apply the punch effects
            base.getPunched(100);

            // Apply knockback
            Vector2 direction = (transform.position - other.transform.position).normalized;
            direction += new Vector2(0, 1).normalized;
            Vector2 knockback = direction.normalized * 5f;
            rb.AddForce(knockback, ForceMode2D.Impulse);

            // Start the flash red coroutine
            StartCoroutine(FlashRed());
        }

    private IEnumerator FallAndDie()
    {
        isFalling = true;


        // Duration of the fall animation
        float fallDuration = 0.5f;

        // Initial and target rotation
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);

        // Initial and target position
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = transform.position + new Vector3(0, -0.5f, 0); // Adjust the fall distance as needed

        // Timer
        float elapsedTime = 0;

        // Animate the fall
        while (elapsedTime < fallDuration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / fallDuration);
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / fallDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position and rotation are set
        transform.rotation = targetRotation;
        transform.position = targetPosition;

        // Destroy the game object after the animation
        Destroy(gameObject);
    }

}


