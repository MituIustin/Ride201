using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainCharacter : BaseClassCharacter
{
    public float jumpForce = 400f;
    public float horizontalMove;
    private bool facingRight;
    public bool jump = false;
    public float dashDistance = 3f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 2f;
    private bool isDashing = false;
    private bool isKnockedBack = false;
    private SpriteRenderer spriteRenderer;
    AudioManager audio;
    public GameObject gmover;
    private Vector3 dashDirection;
    private Rigidbody2D rb;
    public Animator animator;

    private bool wasGrounded;
    private Coroutine speedBuffCoroutine;
    private Coroutine damageBuffCoroutine;





    // Start is called before the first frame update
    void MainChrConstructor()
    {
        // EXEMPLU
        Health = 20;
        Damage = 1;
        Hostile = false;
        Speed = 20f;
    }

    private void Start()
    {
        gmover = GameObject.FindGameObjectWithTag("gameover");
        gmover.SetActive(false);
        audio = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        MainChrConstructor();
        facingRight = true;
        wasGrounded = checkGrd();

        GameObject canvasObject = new GameObject("Canvas");

        // Add Canvas component
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Use ScreenSpaceOverlay to render on top of everything

        // Add CanvasScaler component
        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // Add GraphicRaycaster component
        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject slider = GameObject.FindWithTag("healthBar");

        

        // Instantiate the health bar slider
        GameObject healthbar = Instantiate(slider, Vector3.zero, Quaternion.identity);

        // Set the health bar as a child of the Canvas
        healthbar.transform.SetParent(canvasObject.transform, false);

        // Adjust the size and position of the health bar
        RectTransform rectTransform = healthbar.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(100, 20); // Adjust size as needed
        rectTransform.localScale = new Vector3(2f, 1f, 1f); // Set scale to 1

        // Set anchor and pivot to the middle of the screen
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // Center the health bar in the canvas
        rectTransform.anchoredPosition = new Vector3(0,-170,0);

        if (PlayerPrefs.GetInt("kit1",0)==1)
        {
            IncreaseSpeed();
            PlayerPrefs.SetInt("kit1", 0);
        }
        if (PlayerPrefs.GetInt("kit2", 0) == 1)
        {
            IncreaseDamage();
            PlayerPrefs.SetInt("kit2", 1);
        }
        

    }

    private bool checkGrd()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("grd"));
        return hit.collider != null && hit.collider.CompareTag("ground");
    }

    private void Update()
    {
        animator.SetFloat("Health", base.getHealth());
        //Debug.Log(base.getHealth());
        //Debug.Log(MoveLeft.actual_speed);
        bool isGrounded = checkGrd();
        //moving direction
        horizontalMove = Input.GetAxis("Horizontal") * Speed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // Rotating
        if (horizontalMove < 0f) transform.localEulerAngles = new Vector3(0, 180, 0);
        if (horizontalMove > 0f) transform.localEulerAngles = new Vector3(0, 0, 0);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && checkGrd())
        {
            jump = true;
            audio.PlaySFX(audio.jump);
        }

        if (Input.GetKeyDown(KeyCode.Z) && !isGrounded)
        {
            audio.PlaySFX(audio.dash);
            float horizontalInput = Input.GetAxis("Horizontal");
            dashDirection = new Vector3(horizontalInput, 0f, transform.position.y).normalized;
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            facingRight = true;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            facingRight = false;

        // Update jumping animation state only when grounded state changes
        if (isGrounded != wasGrounded)
        {
            animator.SetBool("CanJump", !isGrounded);
            wasGrounded = isGrounded;
        }
    }

    private void FixedUpdate()
    {
        if (!isKnockedBack) // Prevents interference during knockback and dashing
        {
            Moving(horizontalMove, jump);
        }
    }

    void Moving(float movement, bool canjump)
    {
        rb.velocity = new Vector2(movement * Speed * Time.fixedDeltaTime, rb.velocity.y);

        if (canjump && checkGrd())
        {
            rb.AddForce(new Vector2(0, jumpForce));
            jump = false; // Reset jump after applying force
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        animator.SetBool("CanDash", true);

        // Find all colliders with the specific tag
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("npc");
        List<Collider2D> ignoredColliders = new List<Collider2D>();

        foreach (GameObject obj in taggedObjects)
        {
            Collider2D collider = obj.GetComponent<Collider2D>();
            if (collider != null)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, true);
                ignoredColliders.Add(collider);
            }
        }

        Vector3 startPos = transform.position;
        Vector3 endPos = facingRight
            ? new Vector3(transform.position.x + dashDistance, transform.position.y, transform.position.z)
            : new Vector3(transform.position.x - dashDistance, transform.position.y, transform.position.z);

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(Vector3.Lerp(startPos, endPos, (Time.time - startTime) / dashDuration));
            yield return null;
        }

        rb.MovePosition(endPos);
        animator.SetBool("CanDash", false);

        // Re-enable collisions after the dash
        foreach (Collider2D collider in ignoredColliders)
        {
            if (collider != null)
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, false);
        }

        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }

    public void IncreaseDamage()
    {
        if (damageBuffCoroutine != null)
        {
            StopCoroutine(damageBuffCoroutine);
            Debug.Log("PAUZA");
        }
        damageBuffCoroutine = StartCoroutine(ChangeDamage());
    }

    private IEnumerator ChangeDamage()
    {
        Damage = 2;

        GameObject.FindWithTag("scrollView").GetComponent<UiConsumables>().AddItemUi(1);


        yield return new WaitForSeconds(20f);


        Damage = 1;

    }

    public void IncreaseSpeed()
    {
        if (speedBuffCoroutine!=null)
        {
            StopCoroutine(speedBuffCoroutine);
            Debug.Log("PAUZA");
        }
        speedBuffCoroutine=StartCoroutine(ChangeSpeed());
    }

    private IEnumerator ChangeSpeed()
    {
        Speed = 30f;
        
        GameObject.FindWithTag("scrollView").GetComponent<UiConsumables>().AddItemUi(0);

        

        Debug.Log("VITEZA");

        yield return new WaitForSeconds(20f);

        Debug.Log("FARA VITEZA");

        Speed = 20f;
    }

    public void RestoreHealth()
    {
        if (base.getHealth() < 250)
        {
            int heal = (int)(base.getHealth() + 50);
            base.setHealth(heal);
        }
        else
        {
            base.setHealth(300);
        }
    }

    public IEnumerator FlashRed()
    {
        audio.PlaySFX(audio.damage);
        spriteRenderer.color = Color.red;   // Change color to red 
        yield return new WaitForSeconds(0.5f);  // Wait for 0.5 seconds
        spriteRenderer.color = Color.white; // Reset color to normal

        if(base.getHealth() <= 0)
            gmover.SetActive(true);
       
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
        yield return new WaitForSeconds(1f);

        isKnockedBack = false;
    }

    
}
