using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEvent : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //while(true)
        {
            int seconds = Random.Range(4, 10);
            Debug.Log("ASsssss");
            StartCoroutine(brk(seconds));
        }
    }

    IEnumerator brk(int sec)
    {
        Vector2 movement = new Vector2(200, rb.velocity.y);
        rb.velocity = movement;
        yield return new WaitForSeconds(sec);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
