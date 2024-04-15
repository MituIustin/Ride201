using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float delay = 5f;


    void Start()
    {
        Destroy(gameObject, delay); 
    }
}
