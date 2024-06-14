using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NPCControllerNonHostile : BaseClassCharacter
{
    GameObject Player;
    List<GameObject> npcs = new List<GameObject>();
    Collider2D col;
    SpriteRenderer spriteRenderer;

    private bool got_on_bus = false;
    private float leaving_speed = 0f;
    private bool isDestructionStarted = false;

    public float speed = 1f;
    public float changeDirectionTime;
    private bool ok = false;

    private Rigidbody2D rb;
    private float directionTimer;
    private int currentDirection = -1;
    private GameObject healthbar;
    private bool isFalling = false;


    public GameObject slider;
    private List<Collider2D> ignoredColliders = new List<Collider2D>();



    void Start()
    {
        Player = GameObject.FindWithTag("player");
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        changeDirectionTime = UnityEngine.Random.Range(5f, 20f);
        directionTimer = 500f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        SetInitialOrientation();
        StartCoroutine(GetOnBus());
    }


    void SetInitialOrientation()
    {
        Vector3 newScale = transform.localScale;
        if (currentDirection < 0)
        {
            newScale.x = -Mathf.Abs(newScale.x); // Ensure facing left
        }
        else
        {
            newScale.x = Mathf.Abs(newScale.x); // Ensure facing right
        }
        transform.localScale = newScale;
    }

    IEnumerator GetOnBus()
    {   // fara coliziuni pana urca in autobuz plus viteza mai mare
        speed = 5f;
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("npc");
     

        foreach (GameObject obj in taggedObjects)
        {
            Collider2D collider = obj.GetComponent<Collider2D>();
            if (collider != null)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, true);
                ignoredColliders.Add(collider);
            }
        }
        Vector2 movement = new Vector2(currentDirection * speed, rb.velocity.y);
        rb.velocity = movement;
        yield return new WaitForSeconds(0.1f);
        ok = true;
    }
    void ChangeDirection()
    {
        currentDirection *= -1;
    
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }


    void Update()
    {
        if (base.getHealth() <= 0 && !isFalling)
        {
            Debug.Log(base.getHealth());
            NPCSpawnVariables.npcsalive -= 1;
            StartCoroutine(FallAndDie()); 
        }

        if (healthbar)
        {
            healthbar.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            healthbar.GetComponent<HealthBarScript>().changeHealth(base.getHealth());
        }


        if (ok == false)
        {
            return;
        }

        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0)
        {
            ChangeDirection();
            directionTimer = changeDirectionTime;
        }

        Vector2 movement = new Vector2(currentDirection * speed, rb.velocity.y);
        rb.velocity = movement;

        if (got_on_bus == false && NPCSpawnVariables.spawning == false)
        {  //scapam de npcuri ramase afara
            speed = 0f;
            transform.position -= new Vector3(leaving_speed, 0, 0) * Time.deltaTime;
            leaving_speed += 0.01f;

            if (!isDestructionStarted)
            {
                isDestructionStarted = true;
                StartCoroutine(DestroyAfterDelay(15)); // 15 seconds delay
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { 
        if (other.gameObject.CompareTag("tp_trigger") && NPCSpawnVariables.spawning == true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 3f);
            NPCSpawnVariables.npcsalive += 1;
            got_on_bus = true;

            //ii lasam sa se invarta normal dupa ce urca si la viteza normala
            directionTimer = changeDirectionTime;
            speed = 1f;


            /*aducem coliziunile inapoi
            if(ignoredColliders.Count > 0)
                foreach (Collider2D collider in ignoredColliders)
                    {
                     Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, false);
                    }
            */

            //HEALTHBAR

            GameObject canvasObject = new GameObject("Canvas");

            // Add Canvas component
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            // Add CanvasScaler component
            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            // Add GraphicRaycaster component
            canvasObject.AddComponent<GraphicRaycaster>();

            // Set the Canvas as a child of the character and reset its position
            canvasObject.transform.SetParent(gameObject.transform, false);
            canvasObject.transform.localPosition = new Vector3(0, 1.5f, 0); // Adjust the Y position as needed


            // Instantiate the health bar slider and set it as a child of the Canvas
            healthbar = Instantiate(slider, Vector3.zero, Quaternion.identity);
            healthbar.transform.SetParent(canvasObject.transform, false);

            // Adjust the size and position of the health bar
            RectTransform rectTransform = healthbar.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100, 20); // Adjust size as needed
            rectTransform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            rectTransform.anchoredPosition = Vector3.zero; // Center the health bar in the canvas
        }
        else
        {
            if (other.CompareTag("punch") && NPCSpawnVariables.spawning == false)
            {
                // Start the punch effect coroutine with a delay
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


        if (healthbar != null)
        {
            Destroy(healthbar.gameObject);
        }

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

