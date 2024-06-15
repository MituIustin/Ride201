using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAtLight : MonoBehaviour
{
    public float waitTime = 2f; // Time to wait before creating a new object and applying force
    public float launchForce = 25f; // Force applied to launch the objects
    public GameObject greenLight; // Reference to the green light object
    private Collider2D triggerCollider;

    private void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider == null)
        {
            Debug.LogError("Collider2D component not found on " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            MoveLeft.actual_speed = 0;
            StartCoroutine(HandleStopAndLaunch());
            // Deactivate the trigger collider
            triggerCollider.enabled = false;
        }
    }

    private IEnumerator HandleStopAndLaunch()
    {
        // Find all objects with the tag "player" or "npc"
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("npc");

        Debug.Log("Applying force to players and npcs");

        // Apply force to each object
        ApplyForceToObjects(players);
        ApplyForceToObjects(npcs);

        // Wait for the specified time, while continuously setting actual_speed to 0
        float elapsedTime = 0f;
        while (elapsedTime < waitTime)
        {
            MoveLeft.actual_speed = 0;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Activate the green light
        if (greenLight != null)
        {
            greenLight.SetActive(true);
        }

        // Destroy the current traffic light object
        Destroy(gameObject);
    }

    private void ApplyForceToObjects(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {

                Vector2 direction = new Vector2(1f, 0.1f); // Mostly right with a slight upward component
                Vector2 knockback = direction.normalized * launchForce;

                MainCharacter mainCharacter = obj.GetComponent<MainCharacter>();
                NPCControllerNonHostile nonhostile = obj.GetComponent<NPCControllerNonHostile>();
                NPCController hostile = obj.GetComponent<NPCController>();
                if (mainCharacter != null)
                {
                    mainCharacter.ApplyKnockback(knockback);
                }
                if (nonhostile != null)
                {
                    nonhostile.ApplyKnockback(knockback);
                }
                if (hostile != null)
                {
                    hostile.ApplyKnockback(knockback);
                }
            }
            else
            {
                Debug.LogWarning("No Rigidbody2D found on " + obj.name);
            }
        }
    }
}
