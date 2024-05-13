using UnityEngine;


public class SectionSpawner : MonoBehaviour
{
    public GameObject roadSection;
    public float delay = 3f; // Time in seconds before the action is performed

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(PerformAction), delay);
    }

    // Method to be called after the delay
    void PerformAction()
    {
        Instantiate(roadSection, new Vector3(transform.position.x + 21.2f, -0.58f, transform.position.z), Quaternion.identity);
    }
}


