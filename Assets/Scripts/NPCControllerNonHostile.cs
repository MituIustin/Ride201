using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.EventSystems.EventTrigger;

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
    private int currentDirection = 1;
    private GameObject healhtbar;

    public GameObject slider;

    void Start()
    {
        Player = GameObject.FindWithTag("player");
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        changeDirectionTime = UnityEngine.Random.Range(5f, 20f);
        directionTimer = changeDirectionTime;
        StartCoroutine(GetOnBus());

    }

    IEnumerator GetOnBus()
    {
        currentDirection = -1;
        Vector2 movement = new Vector2(currentDirection * 10, rb.velocity.y);
        rb.velocity = movement;
        yield return new WaitForSeconds(5f);
        ok = true;
        
        GameObject canvasObject = new GameObject("Canvas");

        Canvas canvas = canvasObject.AddComponent<Canvas>();

        canvas.renderMode = RenderMode.WorldSpace;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        canvasObject.AddComponent<GraphicRaycaster>();

        canvasObject.transform.SetParent(gameObject.transform, false);

        healhtbar = Instantiate(slider, new Vector3(0, 0, 1), Quaternion.identity);
        healhtbar.transform.SetParent(canvasObject.transform, false);


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
        if (base.getHealth() <= 0)
        {
            Debug.Log(base.getHealth());
            NPCSpawnVariables.npcsalive -= 1;
            Destroy(gameObject);
        }

        if (healhtbar)
        {
            healhtbar.GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.position.x, transform.position.y+7);
            healhtbar.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 10);
            healhtbar.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0);
            healhtbar.GetComponent<HealthBarScript>().changeHealth(base.getHealth());
            Debug.Log(base.getHealth());
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
            transform.position -= new Vector3(leaving_speed, 0, 0) * Time.deltaTime;
            leaving_speed += 0.005f;

            if (!isDestructionStarted)
            {
                isDestructionStarted = true;
                StartCoroutine(DestroyAfterDelay(5)); // 5 seconds delay
            }
        }
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
            if (other.gameObject.CompareTag("punch") && NPCSpawnVariables.spawning == false)
            {
                base.getPunched(100);
                // Apply knockback
                Vector2 direction = (transform.position - Player.transform.position).normalized;
                direction += new Vector2(0, 1).normalized;
                Vector2 knockback = direction.normalized * 5f;
                rb.AddForce(knockback, ForceMode2D.Impulse);

                // Start the flash red coroutine
                StartCoroutine(FlashRed());

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

}

