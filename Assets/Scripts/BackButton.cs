using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void backToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
