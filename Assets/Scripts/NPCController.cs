using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : BaseClassCharacter
{
    public GameObject Player;
    private BaseClassCharacter baseClassPlayer;
    private bool got_on_bus = false;
    private float leaving_speed = 0f;
    private bool isDestructionStarted = false;

    public Animator anim;

    List<GameObject> npcs = new List<GameObject>();
    Collider2D col;
    SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isFalling = false;

    // Define the distance at which the NPC will attack the player
    public float attackDistance = 1.7f;
    // Define the attack cooldown to avoid multiple attacks in a short time
    private float attackCooldown = 1.5f;
    private float lastAttackTime;
    private bool isKnockedBack = false;

    void Start()
    {
        Player = GameObject.Find("player");
        
        if (Player != null)
        {
            // Obține componenta BaseClassCharacter de la player
            baseClassPlayer = Player.GetComponent<BaseClassCharacter>();
        }
        else
        {
            Debug.LogError("Player object not found!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        lastAttackTime = -attackCooldown; // Initialize to allow immediate attack

    }

    void Update()
    {
        NPCSpawnVariables spawnVariables = NPCSpawnVariables.Instance;
        if (base.getHealth() <= 0 && !isFalling)
        {
            spawnVariables.npcsalive -= 1;
            StartCoroutine(FallAndDie());
        }
        if (spawnVariables.spawning == false)
        {   
            if(Player != null)
                anim.SetFloat("Distance_To_Player", Vector2.Distance(transform.position, Player.transform.position));
        }
        else
        {
            anim.SetFloat("Distance_To_Player", 1000f);  //sa nu atace cat e imbarcare
        }
        // Check distance to the player and call the Attack function if close enough
        if(Player != null)
            if (Vector2.Distance(transform.position, Player.transform.position) <= attackDistance && spawnVariables.spawning == false)
            {
                if (Time.time > lastAttackTime + attackCooldown)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }

        // Move away from bus if conditions are met
        if (!got_on_bus && !spawnVariables.spawning)
        {
            transform.position -= new Vector3(leaving_speed, 0, 0) * Time.deltaTime;
            leaving_speed += 0.01f;

            if (!isDestructionStarted)
            {
                isDestructionStarted = true;
                StartCoroutine(DestroyAfterDelay(15)); // 15 seconds delay
            }
        }

        // Ensure the NPC is always in front of other objects
        Vector3 vec = new Vector3(transform.position.x, transform.position.y, -0.5f);
        transform.position = vec;

        // Move on the same Y-axis as the player
        if(Player != null)
            if (Vector2.Distance(transform.position, Player.transform.position) >= attackDistance - 0.2f)
            {
                Vector3 v = new Vector3(Player.transform.position.x, transform.position.y, 3f);
                transform.position = Vector3.MoveTowards(transform.position, v, 2f * Time.deltaTime);
            }

        // Flip the sprite based on the direction to the player
        if (Player != null)
            spriteRenderer.flipX = transform.position.x > Player.transform.position.x;
    }

    // Coroutine to destroy the NPC after a delay
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.gameObject); // bogos
        NPCSpawnVariables spawnVariables = NPCSpawnVariables.Instance;

        if (other.gameObject.CompareTag("tp_trigger") && spawnVariables.spawning)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 3f);
            spawnVariables.npcsalive += 1;
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

    // Coroutine to flash the NPC red
    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;   // Change color to red 
        yield return new WaitForSeconds(0.5f);  // Wait for 0.5 seconds
        spriteRenderer.color = Color.white; // Reset color to normal
    }

    // Coroutine to handle punch interaction
    private IEnumerator HandlePunch(Collider2D other)
    {
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

    // Coroutine to handle falling and dying
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

        GameObject.FindWithTag("money").GetComponent<CurrencyManager>().AddMoney(3);
        Destroy(gameObject);
    }

    // Metoda pentru a aplica damage player-ului
    private void ApplyDamageToPlayer(float damage)
    {
        if (baseClassPlayer != null)
        {
            baseClassPlayer.getPunched(damage); // Aplică damage player-ului
            Debug.Log("Player health reduced by " + damage);
        }
        else
        {
            Debug.LogError("Player BaseClassCharacter not found!");
        }
    }

    // Define the Attack function
    private void Attack()
    {
        Debug.Log("NPC is attacking the player!");

        // Check the distance and apply damage if close enough
        if (Vector2.Distance(transform.position, Player.transform.position) <= attackDistance - 0.1f)
        {   
            ApplyDamageToPlayer(DifficultyClass.damageMultiplier * 50); // Aplica damage player-ului
            MainCharacter playerMainCharacter = Player.GetComponent<MainCharacter>();
            playerMainCharacter.StartCoroutine(playerMainCharacter.FlashRed()); //facem playerul rosu

        }

        // Start the coroutine to reset the trigger
        StartCoroutine(ResetAttackTrigger());
       
        if (baseClassPlayer.getHealth() <= 0)
        {
            StartCoroutine(DestroyPlayerAfterDelay(1f));
        }
    }

    // Coroutine to reset the attack trigger
    private IEnumerator ResetAttackTrigger()
    {
        // Wait for the duration of the attack animation
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

    }

    private IEnumerator DestroyPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(Player);
        
    }

    public void ApplyKnockback(Vector2 knockbackForce)
    {
        StartCoroutine(HandleKnockback(knockbackForce));
    }

    private IEnumerator HandleKnockback(Vector2 knockbackForce)
    {
        isKnockedBack = true;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);

        // Wait a short time to allow the knockback to take effect
        yield return new WaitForSeconds(0.1f);

        isKnockedBack = false;
    }
}
