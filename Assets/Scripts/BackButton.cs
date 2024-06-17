using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        SceneManager.LoadSceneAsync(0);
    }

    public void backToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
