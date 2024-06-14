using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private SpriteRenderer spriteRenderer;

    private Vector3 dashDirection;
    private Rigidbody2D rb;
    public Animator animator;

    private bool wasGrounded;

    // Start is called before the first frame update
    void MainChrConstructor()
    {
        // EXEMPLU
        Health = 20;
        Damage = 3;
        Hostile = false;
        Speed = 20f;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>(); 
        MainChrConstructor();
        facingRight = true;
        wasGrounded = checkGrd();
    }

    private bool checkGrd()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("grd"));
        return hit.collider != null && hit.collider.CompareTag("ground");
    }

    private void Update()
    {
        animator.SetFloat("Health", base.getHealth());
        Debug.Log(base.getHealth());
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
        }

        if (Input.GetKeyDown(KeyCode.Z) && !isGrounded)
        {
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
        Moving(horizontalMove, jump);
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

    public void IncreaseSpeed()
    {
        StartCoroutine(ChangeSpeed());
    }

    private IEnumerator ChangeSpeed()
    {
        Speed = 30f;

        GameObject.FindWithTag("scrollView").GetComponent<UiConsumables>().AddItemUi(0);

        if (GameObject.FindWithTag("scrollView"))
        {
            Debug.Log("TEST");
        }

        Debug.Log("VITEZA");

        yield return new WaitForSeconds(20f);

        Debug.Log("FARA VITEZA");

        Speed = 20f;
    }

    public IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;   // Change color to red 
        yield return new WaitForSeconds(0.5f);  // Wait for 0.5 seconds
        spriteRenderer.color = Color.white; // Reset color to normal
    }

}
