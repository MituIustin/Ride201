using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Main Menu Manager

public class mainMenu : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void selectShop()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void selectSettings()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void backToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
