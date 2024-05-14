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

    private Vector3 dashDirection;
    private Rigidbody2D rb;

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
        MainChrConstructor();
        facingRight = true;
    }

    private bool checkGrd()
    {
        try
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("grd"));
            if (hit.collider.CompareTag("ground"))
            {
                return true;
            }
        }
        catch (Exception e)
        {
        }
        return false;
    }

    private void Update()
    {   
        //moving direction
        horizontalMove = Input.GetAxis("Horizontal") * Speed;

        // Rotating
        if (horizontalMove < 0f) transform.localEulerAngles = new Vector3(0, 180, 0);
        if (horizontalMove > 0f) transform.localEulerAngles = new Vector3(0, 0, 0);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<BoxCollider2D>().IsTouchingLayers())
            jump = true;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            dashDirection = new Vector3(horizontalInput, 0f, transform.position.y).normalized;
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            facingRight = true;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            facingRight = false; 
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
            jump = !canjump;
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        Vector3 startPos = transform.position;
        Vector3 endPos;
        if(facingRight)
            endPos = new Vector3(transform.position.x + dashDistance, transform.position.y, transform.position.z);
        else
            endPos = new Vector3(transform.position.x - dashDistance, transform.position.y, transform.position.z);
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(Vector3.Lerp(startPos, endPos, (Time.time - startTime) / dashDuration));
            yield return null;
        }
        rb.MovePosition(endPos);
        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("npc") && NPCSpawnVariables.spawning == false)
        {
            Destroy(collision.gameObject);
            NPCSpawnVariables.npcsalive -= 1;
        }
    }*/

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
}
