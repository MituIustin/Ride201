using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Spawner : MonoBehaviour
{
    public GameObject roadSection_normal;
    public GameObject roadSection_traffic_light;
    private GameObject roadSection;
    private bool has_spawned = false;

    void Start()
    {
        CreateRoadSection();
    }

    void CreateRoadSection()
    {

        // Check if actual_speed is 7 and roll a 1/10 chance
        if (MoveLeft.actual_speed > 6.9f && Random.Range(0, 10) == 0)
        {
            roadSection = roadSection_traffic_light;
        }
        else
        {
            roadSection = roadSection_normal;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("player") && has_spawned == false)
        {
            // Debug.Log("Triggered: " + gameObject.name + ", Tag: " + gameObject.tag);
            has_spawned = true;
            GameObject newRoadSection = Instantiate(roadSection, new Vector3(transform.position.x + 17.97f, 0.52f, transform.position.z), Quaternion.identity);

            // Assuming the roadSection has a script with a public attribute you want to set
            MoveLeft roadSectionScript = newRoadSection.GetComponent<MoveLeft>();
            if (roadSectionScript != null)
            {
                roadSectionScript.speed = MoveLeft.actual_speed; 
            }
        }
    }
}
