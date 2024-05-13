using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCharacter : BaseClassCharacter
{
    public float jumpForce = 400f;
    public float dashForce = 500f;
    public float horizontalMove;
    private bool facingRight;
    public bool jump = false;
    public bool dash = false;
    Rigidbody2D rb;

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
    private void Update()
    {   
        //moving direction
        horizontalMove = Input.GetAxis("Horizontal") * Speed;

        //rotating
        if (horizontalMove < 0f) transform.localEulerAngles = new Vector3(0, 180, 0);
        if (horizontalMove > 0f) transform.localEulerAngles = new Vector3(0, 0, 0);

        //jumping
        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<BoxCollider2D>().IsTouchingLayers()) jump = true;
        if (Input.GetKeyDown(KeyCode.Z)) dash = true;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) facingRight = true;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) facingRight = false;
    }
    private void FixedUpdate()
    {
        
        //we create our moving function
        Moving(horizontalMove, jump, dash);
       
    }

    void Moving(float movement, bool canjump, bool candash)
    {
        rb.velocity = new Vector2(movement * Speed * Time.fixedDeltaTime, rb.velocity.y);

        if (canjump && GetComponent<BoxCollider2D>().IsTouchingLayers())
        {
            rb.AddForce(new Vector2(0, jumpForce));
            jump = !canjump;
        }

        if (candash)
        {
            int dashDirection = facingRight ? 1 : -1;
            rb.AddForce(new Vector2(dashForce * dashDirection * 10, 0f), ForceMode2D.Force);
            dash = false; 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("npc") && NPCSpawnVariables.spawning == false)
        {
            Destroy(collision.gameObject);
            NPCSpawnVariables.npcsalive -= 1;
        }
    }

   public void IncreaseSpeed()
    {
        StartCoroutine(ChangeSpeed());
    }

    private IEnumerator ChangeSpeed()
    {
        Speed = 50f;

        GameObject.FindWithTag("scrollView").GetComponent<UiConsumables>().AddItemUi(0);

        if (GameObject.FindWithTag("scrollView"))
        {
            Debug.Log("TEST");
        }

        Debug.Log("VITEZA");

        yield return new WaitForSeconds(15f);

        Debug.Log("FARA VITEZA");

        Speed = 20f;


    }
}
