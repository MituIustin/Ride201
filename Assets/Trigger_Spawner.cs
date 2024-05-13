using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Spawner : MonoBehaviour
{
    public GameObject roadSection;
    private bool has_spawned = false;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("player") && has_spawned == false)
        {
            // Debug.Log("Triggered: " + gameObject.name + ", Tag: " + gameObject.tag);
            has_spawned = true;
            Instantiate(roadSection, new Vector3(transform.position.x + 17.97f, 0.52f, transform.position.z), Quaternion.identity);
        }
    }
}
